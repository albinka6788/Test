#region Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using BHIC.Common.Config;
using BHIC.Contract.PurchasePath;
using BHIC.Core.PurchasePath;
using BHIC.Domain.QuestionEngine;
using BHIC.Portal.WC.App_Start;
using BHIC.Common.Quote;
using BHIC.ViewDomain;
using BHIC.ViewDomain.QuestionEngine;
using Newtonsoft.Json;

using DML_DTO = BHIC.DML.WC.DTO;
using System.Web;
using BHIC.Domain.Service;
using BHIC.Domain.Dashboard;
using BHIC.Common.XmlHelper;
using BHIC.Common;
using System.Text.RegularExpressions;
using BHIC.ViewDomain.Landing;
using BHIC.Common.CommonUtilities;

#endregion

namespace BHIC.Portal.WC.Areas.PurchasePath.Controllers
{
    //[CustomTransactionLogFilterAttribute]
    [ValidateSession]
    [CustomAntiForgeryToken]
    public class QuestionsController : BaseController
    {
        #region Variables : Page Level Local Variables Decalration

        private static string QuestionnairePage = "QuestionnairePage";
        private static string QuestionPageBaseMethod = "GetQuestions";
        private static string FeinXModFactor = "FeinXmodFactor";
        private static string XModExpiryDate = "XmodExpiryDate";

        CustomSession appSession;

        #endregion

        #region Methods : public methods

        #region MethodType : GET

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// This method gets list of the Questions based exposure data & policy data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetQuestions(string quoteId)
        {
            #region Comment : Here if request query string is found(means request came from mail embedded link) then do following action

            if (quoteId != null)
            {
                //get quote-id in int format for future references
                int intQuoteId = 0;

                ICommonFunctionality commonFunctionality = GetCommonFunctionalityProvider();
                appSession = RetrieveSavedQuote(quoteId, commonFunctionality, out intQuoteId);

                //Comment : Here if able to retrieve saved session from DB layer then do following
                if (appSession != null)
                {
                    //Comment : Here finlly update this into session
                    SetCustomSession(appSession);

                    //Save/Update QuoteId
                    HttpContextBase context = this.ControllerContext.HttpContext;
                    QuoteCookieHelper.Cookie_SaveQuoteId(context, intQuoteId);
                }

            }

            #endregion

            #region Comment : Here get all custom session information reuired for this page processing

            GetSession();

            #endregion

            #region Comment : Here must check for this quote Questionnaire should be displayed ?

            IsValidPageRequest();

            #endregion

            #region Comment : Here Questionnaire interface refernce to do/make process all business logic

            IQuestionnaire questionnaireBLL = GetQuestionnireProvider();

            #endregion

            //Comment : Here set VM object pass it to view
            QuestionnaireViewModelAngular questionsViewModel = new QuestionnaireViewModelAngular();

            //stored in a cookie on the user's machine
            int wcQuoteId = GetCookieQuoteId();

            //Comment : Here get questions
            var questionResponse = questionnaireBLL.GetQuestionsResponse(wcQuoteId);

            #region Comment : Here set error message from API response in case of any non-system error type

            if (questionResponse != null && !questionResponse.OperationStatus.RequestSuccessful)
            {
                questionResponse.OperationStatus.Messages.ForEach(msg => questionsViewModel.Messages.Add(msg.Text));
            }

            #endregion

            //Comment : Here don not unnecessarily call below method call
            if (questionResponse.Questions.Count > 0)
            {
                questionsViewModel.Questions = questionResponse.Questions ?? new List<Question>();
                //questionsViewModel.FeinApplicable = questionsViewModel.Questions.Count > 0 ? questionnaireBLL.IsFeinNumberApplicable(wcQuoteId) : false; //Old Line                

                //Add FEIN Number from session (in case of user request come from mailed link)
                if (appSession.QuestionnaireVM != null)
                {
                    questionsViewModel.FeinApplicable = (!appSession.IsNull() && !appSession.QuestionnaireVM.IsNull()) ? appSession.QuestionnaireVM.FeinApplicable : false;

                    //Comment : Here must check IsFeinApplicable then only set FEIN otherwise RESET it to default
                    questionsViewModel.TaxIdNumber = questionsViewModel.FeinApplicable ? appSession.QuestionnaireVM.TaxIdNumber : string.Empty;
                    questionsViewModel.TaxIdType = questionsViewModel.FeinApplicable ? appSession.QuestionnaireVM.TaxIdType : string.Empty;                    
                }

                //Comment : Here new finctional changes requested to auto-fill following question with Expoure screen value and don't display but required to posted.
                var howLongBeenInBusinessQuestionId = ConfigCommonKeyReader.HowLongBeenInBusinessQuestionId;
                questionnaireBLL.SetHowLongBeenInBusinessQuestionResponse(questionResponse.Questions, howLongBeenInBusinessQuestionId);


                //Comment : Here same time set this to CustomSession object
                if (appSession != null)
                {
                    //Comment : Here must check object instance existance
                    var sessionQuestionnaireVM = questionnaireBLL.GetQuestionnaireVM();

                    if (sessionQuestionnaireVM == null)
                    {
                        appSession.QuestionnaireVM = new QuestionnaireViewModel();
                    }

                    #region Comment : Here To maintain user Referral/Decline history we need to get "All List of Previous Referrals Details"

                    //Comment : Here Keep the "ReferralDeclineHistory" from session 
                    List<List<int>> referralDeclineHistory = new List<List<int>>();
                    ReferralHistory referralDeclineHistoryNew  = new ReferralHistory();

                    if (!appSession.QuestionnaireVM.IsNull() && !appSession.QuestionnaireVM.ReferralScenariosHistory.IsNull())
                    {
                        //referralDeclineHistory = new List<List<int>>(appSession.QuestionnaireVM.ReferralScenariosHistory);
                        //appSession.QuestionnaireVM.ReferralScenariosHistory.ForEach(refList => referralDeclineHistory.Add(refList));
                        referralDeclineHistory = appSession.QuestionnaireVM.ReferralScenariosHistory;
                        referralDeclineHistoryNew = appSession.QuestionnaireVM.ReferralHistory;
                    }

                    #endregion

                    //Comment : Here add this data into current local custom session object then let it update in final copy in next step
                    appSession.QuestionnaireVM.QuestionsResponse = null;
                    //Comment : Here on page load reset "Referral Scenarios" list
                    appSession.QuestionnaireVM.QuoteReferralMessages.Clear();
                    appSession.QuestionnaireVM.ReferralScenarioIds.Clear();

                    //Comment : Here finally add this Referral/Decline history into Session object
                    appSession.QuestionnaireVM.ReferralScenariosHistory = referralDeclineHistory;
                    appSession.QuestionnaireVM.ReferralHistory = referralDeclineHistoryNew;

                    //Comment : Here finlly update this into session
                    SetCustomSession(appSession);
                }
            }

            //Comment : Progress bar nagivation binding based on flag 
            List<NavigationModel> links = new List<NavigationModel>();
            NavigationController nc = new NavigationController();
            links = nc.GetProgressBarLinks(appSession.PageFlag);
            questionsViewModel.NavigationLinks = links;
            return PartialView("_GetQuestions", questionsViewModel);
        }

        #endregion

        #region MethodType : POST

        /// <summary>
        /// This method will submit Questionnaire to get Quote related information
        /// </summary>
        /// <param name="questionsList"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostQuestionResponse(List<Question> questionsList, string taxIdNumber, string taxIdType)
        {
            if (questionsList != null)
            {
                #region Comment : Here Server side validation processing

                //var listOfModelErrors = ValidateQuestionModel(questionsList);

                ////if model has error return error list
                //if (listOfModelErrors.Count > 0)
                //{
                //    return Json(new
                //    {
                //        resultStatus = "NOK",
                //        resultText = "",  //This is need to be change, non-relevent message return type
                //        resultMessages = listOfModelErrors
                //    }, JsonRequestBehavior.AllowGet);
                //}

                #endregion

                #region Comment : Here Questionnaire interface refernce to do/make process all business logic

                IQuestionnaire questionnaireBLL = GetQuestionnireProvider();

                #endregion

                #region Comment : Here Is valid FeinNumber submmited

                string feinNumber = null;
                var isValidFein = false;

                if (taxIdNumber != "" && taxIdNumber != null)
                {
                    feinNumber = taxIdNumber;
                    feinNumber = questionnaireBLL.StripFeinNumber(feinNumber);
                    isValidFein = questionnaireBLL.IsValidFeinNumber(feinNumber);

                    if (!isValidFein && feinNumber != null)
                    {
                        //throw new Exception("Please enter the correct FEIN number !");
                        return Json(new { resultStatus = "NOK", resultText = "Please enter the correct FEIN number !" }, JsonRequestBehavior.AllowGet);
                    }
                }

                #endregion

                #region Comment : Here When posted QuestionList data found

                // stored in a cookie on the user's machine
                int wcQuoteId = GetCookieQuoteId();

                if (wcQuoteId != 0)
                {
                    #region Comment : Here get all custom session information reuired for this step processing

                    GetSession();

                    #endregion

                    //Comment : Here check appSession object 
                    if (appSession != null)
                    {
                        #region Comment : Here If valid FeinNumber provided then set this modifier on this Quote

                        var sessionQuestionnaireVM = questionnaireBLL.GetQuestionnaireVM();

                        //Comment : Here if applicable get FeinXmdFactor value & ExpiryDate
                        Dictionary<string, object> xModReturnedInfo = new Dictionary<string, object>();

                        //Comment : Here If FeinNumber submitted exists then POST this data using modifier Api if applicable                        
                        if (isValidFein && feinNumber != null)
                        {
                            questionnaireBLL.CheckAndSetXmodFactorApplicability(wcQuoteId, feinNumber, out xModReturnedInfo);
                        }
                        else
                        {
                            //Comment : Here new implemnentation done to handle DELETE existing modifier on QuoteId if FEIN no not supplied & app session having OLD XMod value
                            if (sessionQuestionnaireVM.XModValue != 0 && feinNumber == null)
                            {
                                questionnaireBLL.DeleteModifierForQuote(wcQuoteId, null);
                            }
                        }

                        #endregion

                        #region Comment : Here STEP - 1 First get QuestionResponse data for this quote storted in Session

                        QuestionsResponse questionsResponse = new QuestionsResponse();

                        #endregion

                        #region Comment : Here STEP - 2 Update question list object

                        questionsResponse.Questions = questionsList;
                        questionsResponse.QuoteId = wcQuoteId;

                        #endregion

                        #region Comment : Here STEP - 3 Post this updated question list object & fien, xMod-factor through API

                        //Comment : Here update this payment plan into service provider system                        
                        var serviceResponse = questionnaireBLL.PostQuestionnaire(questionsResponse);

                        #endregion

                        #region Comment : Here set error message from API response in case of any non-system error type

                        if (serviceResponse != null && !serviceResponse.OperationStatus.RequestSuccessful &&
                            !serviceResponse.OperationStatus.Messages.Any(res => res.MessageType == MessageType.SystemError))
                        {
                            var listOfErrors = new List<string>();
                            serviceResponse.OperationStatus.Messages.ForEach(msg => listOfErrors.Add(msg.Text));

                            //Comment : Here retrun execution of code from here itself in case of some error occured
                            if (listOfErrors.Count > 0)
                            {
                                return Json(new { resultStatus = "NOK", resultText = "Something went wrong while posting questionnaire data !", resultMessages = listOfErrors }, JsonRequestBehavior.AllowGet);
                            }
                        }

                        #endregion

                        #region Comment : Here STEP - 4 If POST API request successful then get new QuoteId

                        if (serviceResponse.OperationStatus.RequestSuccessful)
                        {
                            #region Comment : Here on POST Questions success get Premium and InstallmentFee in session object for next page(Quote) usage

                            try
                            {
                                //Comment : Here if local custom section is got cleared then call it gain from shared application custom session
                                if (appSession == null)
                                {
                                    GetSession();
                                }
                                else if (appSession != null)
                                {
                                    //Comment : Here must check object instance existance
                                    sessionQuestionnaireVM = questionnaireBLL.GetQuestionnaireVM();

                                    if (sessionQuestionnaireVM == null)
                                    {
                                        appSession.QuestionnaireVM = new QuestionnaireViewModel();
                                    }

                                    #region Comment : Here add this data into current local custom session object then let it update in final copy in next step

                                    #region Comment : Here Add XMod related information into seesion

                                    //Add FEIN
                                    appSession.QuestionnaireVM.TaxIdNumber = feinNumber;
                                    appSession.QuestionnaireVM.TaxIdType = taxIdType;

                                    //Add XModValue
                                    var xModValue = questionnaireBLL.GetCustomKeyValue(xModReturnedInfo, FeinXModFactor);
                                    appSession.QuestionnaireVM.XModValue = xModValue != null ? Math.Round(Convert.ToDecimal(xModValue), 3) : 0;

                                    //Add XModExpiryDate
                                    var xModExpiryDate = questionnaireBLL.GetCustomKeyValue(xModReturnedInfo, XModExpiryDate);
                                    if (xModExpiryDate != null)
                                    {
                                        appSession.QuestionnaireVM.XModExpiryDate = Convert.ToDateTime(xModExpiryDate);
                                    }

                                    #endregion

                                    #region Comment : Here Add Questions POST API response related information into seesion

                                    appSession.QuestionnaireVM.InstallmentFee = serviceResponse.PerInstallmentCharge;
                                    appSession.QuestionnaireVM.PremiumAmt = serviceResponse.Premium;
                                    appSession.QuestionnaireVM.QuoteStatus = serviceResponse.QuoteStatus;
                                    appSession.QuestionnaireVM.Agency = serviceResponse.QuestionsRequest.Agency;
                                    appSession.QuestionnaireVM.Carrier = serviceResponse.QuestionsRequest.Carrier;

                                    #endregion

                                    #endregion

                                    //Comment : Here finlly update this into session
                                    SetCustomSession(appSession);
                                }
                            }
                            catch (Exception)
                            {
                                throw;
                            }

                            #endregion

                            #region Comment : Here based on API call response take future action

                            // ----------------------------------------
                            // return the view required by the decision / results above
                            // ----------------------------------------     
                            string finalQuoteStatus = string.Empty;

                            #region Comment : Here Questionnaire interface refernce to do/make process all business logic

                            questionnaireBLL = GetQuestionnireProvider();

                            #endregion

                            //Comment : Herer To Handle Referral scenrario 8 is for "Question	Yes to non-eligible risk"
                            appSession.QuestionnaireVM.QuestionsResultMessage = serviceResponse.ResultMessages;

                            var objQuoteStatus = questionnaireBLL.GetQuoteStatusDecision(serviceResponse, xModReturnedInfo, Server.MapPath("~/ReferralReasons.xml"), out finalQuoteStatus);

                            #region Comment : Here after final quote-status decision made please record GUARD & XCD referral message

                            loggingService.Trace(string.Format("{0}, GuardResponse:{1}, XceedanceCustomResponse:{2}", "Final status on quote taken ", serviceResponse.QuoteStatus, finalQuoteStatus));
                            if (finalQuoteStatus.Equals("Soft Referral"))
                            {

                            }

                            #endregion

                            //Comment : Here must update custom decision status in session object
                            appSession.QuestionnaireVM.QuoteStatus = finalQuoteStatus;

                            //Comment : Progress bar nagivation flag handling
                            //Session["flag"] = 3;
                            //Nishank [GUIN-483]: Set page flag to default
                            appSession.PageFlag = 2;

                            //Comment : Here finlly update this into session
                            SetCustomSession(appSession);

                            #region Comment : Here save quote details in DB only when Status='QUOTE'

                            if (finalQuoteStatus.Equals("Quote"))
                            {
                                //Nishank [GUIN-483]: Set page flag to 3 if flows goes to quoteSummary
                                appSession.PageFlag = 3;
                                var quoteCreatedinDB = false;

                                try
                                {
                                    #region Comment : Here QuoteSummary interface refernce to do/make process all business logic

                                    //Comment : Here BLL stands for "Business Logic Layer"
                                    IQuoteSummary quoteSummaryBLL = GetQuoteSummaryProvider();

                                    #endregion

                                    #region Comment : Here STEP - 1. Insert this new generated quote into database for future FK refernces

                                    //Comment : Here get logged PC user id
                                    var loggedUserId = GetLoggedUserId();

                                    quoteCreatedinDB = quoteSummaryBLL.AddUpdateQuoteData(new DML_DTO.QuoteDTO()
                                    {
                                        QuoteNumber = wcQuoteId.ToString(),
                                        PremiumAmount = serviceResponse.Premium,
                                        LineOfBusinessId = 1, //Right now hard coded it will come in session from home page in future
                                        ExternalSystemID = 1, //Right now hard coded
                                        IsActive = true,
                                        RequestDate = DateTime.Now,
                                        AgencyCode = serviceResponse.QuestionsRequest.Agency,
                                        CreatedBy = loggedUserId ?? 1,
                                        CreatedDate = DateTime.Now,
                                        ExpiryDate = DateTime.Now.AddYears(1),
                                        ModifiedDate = DateTime.Now,
                                        ModifiedBy = loggedUserId ?? 1
                                    });


                                    #endregion

                                    #region Comment : Here STEP - 2. Updated newly created Quote with logged in user detail

                                    if (quoteCreatedinDB && loggedUserId != null)
                                    {
                                        PurchaseQuote purchaseQuote = new PurchaseQuote();

                                        // Calling Quote linking with Logged in user.
                                        purchaseQuote.UpdateQuoteUserId(new DML_DTO.OrganisationUserDetailDTO()
                                                                    {
                                                                        EmailID = GetLoggedInUserDetails().Email
                                                                    },
                                                                    new DML.WC.DTO.QuoteDTO()
                                                                    {
                                                                        QuoteNumber = wcQuoteId.ToString(),
                                                                        OrganizationUserDetailID = loggedUserId,
                                                                        ModifiedBy = loggedUserId ?? 1,
                                                                        ModifiedDate = DateTime.Now
                                                                    });
                                    }

                                    #endregion

                                    #region Comment : Here STEP - 3. Persist this quote into DB which will be used for PC "Saved Quotes" functionalities

                                    if (quoteCreatedinDB && loggedUserId != null)
                                    {
                                        //Comment : Here CommonFunctionality interface refernce to do/make process all business logic
                                        ICommonFunctionality commonFunctionality = GetCommonFunctionalityProvider();

                                        //Comment : Here call BLL to make this data stored in DB
                                        bool isStoredInDB = commonFunctionality.AddApplicationCustomSession(
                                            new DML_DTO.CustomSession() { QuoteID = wcQuoteId, SessionData = commonFunctionality.StringifyCustomSession(appSession), IsActive = true, CreatedDate = DateTime.Now, CreatedBy = loggedUserId ?? 1, ModifiedDate = DateTime.Now, ModifiedBy = loggedUserId ?? 1 });
                                    }

                                    #endregion

                                    #region Comment : Here If user response for last question (I hereby certify) is 'Y' then redirect user to decline quote

                                    /*
                                    // Moved here after Saurabh comment.
                                    var lastQuestion = questionsList.Last();

                                    //Comment : Here skip further processing if enswer to last question is "True/Y"
                                    if (
                                            lastQuestion != null &&
                                            (
                                                lastQuestion.UserResponse.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                                                lastQuestion.UserResponse.Equals("Y", StringComparison.OrdinalIgnoreCase)
                                            )
                                        )
                                    {
                                        finalQuoteStatus = "Hard Referral";

                                        //Comment : Here must update custom decision status in session object
                                        appSession.QuestionnaireVM.QuoteStatus = finalQuoteStatus;

                                        //Comment : Here finlly update this into session
                                        SetCustomSession(appSession);

                                        if (lastQuestion != null && (lastQuestion.UserResponse.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                                            lastQuestion.UserResponse.Equals("Y", StringComparison.OrdinalIgnoreCase)))
                                        {
                                            loggingService.Trace("Send to Hard Referral as last question is set as 'Y'");
                                        }

                                        //Comment : Here finally return response to user
                                        return Json(new { resultStatus = "OK", resultText = finalQuoteStatus, resultMessages = new List<string>() }, JsonRequestBehavior.AllowGet);
                                    }

                                    */
                                    #endregion
                                }
                                catch (Exception ex)
                                {
                                    loggingService.Fatal(ex);
                                }

                                #region Comment : Here in case not able to save quote details at DB layer

                                //Comment : Here must check is this quote details successfull register/saved on DB layer becoz these will be used in in future while making payment details on BuyPolicy page
                                if (!quoteCreatedinDB)
                                {
                                    return Json(new { resultStatus = "NOK", resultText = "Quote details has not been submitted !" }, JsonRequestBehavior.AllowGet);
                                }

                                #endregion

                            }
                            else
                            {
                                #region Comment : Here When referral & finally quote status must be done based on Guard Response then

                                //Referral scenrario 8 is for "Question	Yes to non-eligible risk"
                                //Added new condition on 18.03.2016 to skip Question page response into session
                                //if (!appSession.QuestionnaireVM.ReferralScenarioIds.Contains(1) && !appSession.QuestionnaireVM.ReferralScenarioIds.Contains(4))
                                if (!(appSession.QuestionnaireVM.ReferralScenarioIds.Intersect(new List<int>() { 1, 4, 10, 11 }).Any()))
                                {
                                    //Comment : Here must update custom decision status in session object
                                    appSession.QuestionnaireVM.QuestionsResponse = serviceResponse;

                                    //Comment : Here finlly update this into session
                                    SetCustomSession(appSession);
                                }

                                #endregion
                            }

                            #endregion

                            //Comment : Here finally return response to user

                         
                            return Json(objQuoteStatus, JsonRequestBehavior.AllowGet);

                            #endregion
                        }

                        #endregion
                    }
                }

                #endregion
            }

            return Json(new { resultStatus = "NOK", resultText = "Something went wrong while posting questionnaire data !", resultMessages = new List<string>() }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Send mail with current saved quote revocation link
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveForLater(List<Question> questionsList, string taxIdNumber, string taxIdType, string emailId)
        {
            #region Comment : Here get all custom session information reuired for this step processing

            GetSession();

            #endregion

            #region Comment : Here get current quoteId from cookie

            //stored in a cookie on the user's machine
            int wcQuoteId = GetCookieQuoteId();

            #endregion

            #region Comment : Here when session and quote-id existance verfied then if emailId is correct then send mail to user

            if (emailId.Trim().Length > 0)
            {
                try
                {
                    //Comment : Here post this data into provider system
                    var questionPostResponse = PostQuestionResponse(questionsList, taxIdNumber, taxIdType);

                    //Comment : Here if question post processed
                    if (questionPostResponse != null)
                    {
                        JsonResult jsonResult = questionPostResponse as JsonResult;
                        dynamic dResult = jsonResult.Data;

                        #region Comment : Here if user data has been posted successfully then only send mail & embedded link to user

                        if (dResult.ToString().Contains("resultStatus = OK"))
                        {
                            //Comment : Here according new implementation on 04.04.2016 (Add quote into Dashboard even when user does SaveForLater on Exposure/Question)
                            if (!dResult.ToString().Contains("resultText = Quote") && !GetLoggedUserId().IsNull())
                            {
                                #region Comment : Here add this quote in Dashboard even when post Questionnnaire it's "Reffered or Declined"

                                //Comment : Here Questionnaire interface refernce to do/make process all business logic
                                IQuestionnaire questionnaireBLL = GetQuestionnireProvider();

                                //Comment : Here add this quote inot Dashboard
                                questionnaireBLL.AddQuoteToDashboard(GetLoggedUserId(), wcQuoteId, GetLoggedInUserDetails().Email);

                                #endregion
                            }

                            #region Comment : Here CommonFunctionality interface refernce to do/make process all business logic

                            ICommonFunctionality commonFunctionality = GetCommonFunctionalityProvider();

                            //Comment : Here call BLL to make this data stored in DB
                            bool isStoredInDB = commonFunctionality.AddApplicationCustomSession(
                                new DML_DTO.CustomSession() { QuoteID = wcQuoteId, SessionData = commonFunctionality.StringifyCustomSession(appSession), IsActive = true, CreatedDate = DateTime.Now, CreatedBy = GetLoggedUserId() ?? 1, ModifiedDate = DateTime.Now, ModifiedBy = GetLoggedUserId() ?? 1 });

                            #endregion

                            #region Comment : Here if stored in DB end then finally send user mail

                            if (wcQuoteId != 0 && isStoredInDB)
                            {
                                bool mailSent = SendMailSaveForLater(QuestionPageBaseMethod, wcQuoteId, emailId);

                                //Comment : Here based on mail sent status through user a message
                                if (mailSent)
                                    return Json(new { resultStatus = "OK", resultText = "Successfully submitted" }, JsonRequestBehavior.AllowGet);
                            }

                            #endregion
                        }

                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    loggingService.Fatal(ex);
                    return Json(new { resultStatus = "NOK", resultText = "Something went wrong while processing save for later !" }, JsonRequestBehavior.AllowGet);
                }
            }

            #endregion

            loggingService.Trace("Failed due to empty Email ID");
            return Json(new { resultStatus = "NOK", resultText = "Something went wrong while processing save for later !" }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region Methods : private methods

        /// <summary>
        /// Return QuoteId stored in cookies with current Http request
        /// </summary>
        /// <returns></returns>
        private int GetCookieQuoteId()
        {
            // stored in a cookie on the user's machine
            int? wcQuoteId = QuoteCookieHelper.Cookie_GetQuoteId(this.ControllerContext.HttpContext);

            //if cookie QuoteId has been expired
            if (wcQuoteId == null)
            {
                throw new ApplicationException(string.Format("{0} : {1} !", QuestionnairePage, Constants.QuoteIdCookieEmpty));
            }

            return wcQuoteId ?? 0;
        }

        /// <summary>
        /// Get application current session
        /// </summary>
        private void GetSession()
        {
            try
            {
                if (!IsCustomSessionNull())
                {
                    appSession = GetCustomSession();
                }
            }
            catch { }
            finally
            {
                if (IsCustomSessionNull())
                {
                    throw new ApplicationException(string.Format("{0},{1} : {2} !", QuestionnairePage, Constants.SessionExpiredPartial, Constants.AppCustomSessionExpired));
                }
            }
        }

        /// <summary>
        /// Return status whether this page resuest is valid or un-authorized
        /// </summary>
        /// <returns></returns>
        private bool IsValidPageRequest()
        {
            //var quoteVM = appSession.QuoteVM;
            //var questionnaireVM = appSession.QuestionnaireVM;

            //Comment : Here check is this/current quote have associated exposres/policy data before generating and navigating user to Questionnaire page
            //if (
            //        quoteVM == null ||
            //        (quoteVM.Exposures == null || quoteVM.PolicyData == null) ||
            //        //Comment : Here if quote is referral/decline then "QuestionnaireVM" would exist but "QuoteStatus" will be other than "Quote"
            //        (!questionnaireVM.IsNull() && !quoteVM.IsValidQuestionPageRequest)
            //        //(!string.IsNullOrEmpty(questionnaireVM.QuoteStatus) && !questionnaireVM.QuoteStatus.Equals("Quote"))
            //   )
            if (appSession.PageFlag < 2)
            {
                throw new ApplicationException(string.Format("{0},{1} : Please first submit exposure and policy data to get questionnaire !", QuestionnairePage, Constants.UnauthorizedRequest));
            }

            return true;
        }

        /// <summary>
        /// Return interface refernce to make all business logic
        /// </summary>
        /// <returns></returns>
        private IQuestionnaire GetQuestionnireProvider()
        {
            #region Comment : Here Questionnaire interface refernce to do/make process all business logic

            IQuestionnaire questionnaire = new Questionnaire(GetCustomSession(), guardServiceProvider);

            #endregion

            return questionnaire;
        }

        /// <summary>
        /// Return interface refernce to make all business logic
        /// </summary>
        /// <returns></returns>
        private IQuoteSummary GetQuoteSummaryProvider()
        {
            #region Comment : Here Questionnaire interface refernce to do/make process all business logic

            IQuoteSummary quoteSummaryBLL = new QuoteSummary(GetCustomSession(), guardServiceProvider);

            #endregion

            return quoteSummaryBLL;
        }

        /// <summary>
        /// It will validate Question Model
        /// </summary>
        /// <returns>return list or errors if error found,empty list otherwise</returns>
        private List<string> ValidateQuestionModel(List<Question> questions)
        {
            var listOfErrors = new List<string>();
            try
            {
                #region Comment : Here local varibales used

                var errorMessage = string.Empty;
                List<Question> dependentQuestionList = new List<Question>();
                List<string> listOfQuestionType = new List<string> { "R", "T", "N", "P", "L", "D" };
                var questionResponse = string.Empty;
                var parentQuestionResponse = string.Empty;
                var parentQuestion = new Question();
                bool validateDependentQuestion = false;
                var questionWhenResponse = string.Empty;

                #endregion

                foreach (var question in questions)
                {
                    #region Comment : Here in each iteration reset local varibles

                    //dependentQuestionList = new List<Question>();
                    errorMessage = string.Empty;
                    questionResponse = string.Empty;
                    parentQuestionResponse = string.Empty;
                    parentQuestion = new Question();
                    validateDependentQuestion = false;
                    questionWhenResponse = string.Empty;

                    #endregion

                    #region Commeny : Here first of all basic/generic validation checks

                    //Comment : Here in case of 'P' type question it's not exceptionally mandatory
                    if (!string.IsNullOrEmpty(question.QuestionType) && !listOfQuestionType.Contains(question.QuestionType))
                    {
                        listOfErrors.Add(Constants.InvalidQType);
                        continue;
                    }
                    else if (question.questionId <= 0)
                    {
                        listOfErrors.Add(Constants.InvalidQNumber);
                        continue;
                    }


                    #endregion

                    #region Comment : Here Exceptionally if percentage question, and not attempted then auto fill with default value "0"

                    //Comment : Here in case of 'P' type question it's not exceptionally mandatory
                    if (question.QuestionType.Equals("P") && string.IsNullOrEmpty(question.UserResponse))
                    {
                        question.UserResponse = "0";
                    }

                    #endregion

                    #region Comment : Here process main/parent question & child questions

                    //check whether current question have any children or not
                    //dependentQuestionList = questions.Where(x => x.WhenQuestion == question.questionId).ToList();
                    questionResponse = question.UserResponse;

                    if (question.WhenQuestion >= 0)
                    {
                        //if current question don't have child itself
                        if (question.WhenQuestion == 0)
                        {
                            #region Comment : Here if question is independent then validate here

                            errorMessage = ValidateUserResponse(question);
                            if (!string.IsNullOrEmpty(errorMessage))
                            {
                                listOfErrors.Add(errorMessage);
                            }

                            #endregion
                        }
                        else if (question.WhenQuestion != 0)
                        {
                            #region Comment : Here handling dependent question validation handling based parent condition

                            //Comment : Here get parent question object
                            parentQuestion = questions.Where(x => x.questionId == question.WhenQuestion).Single();

                            if (!parentQuestion.IsNull() && parentQuestion.questionId > 0 && !string.IsNullOrEmpty(parentQuestion.UserResponse))
                            {
                                parentQuestionResponse = parentQuestion.UserResponse;

                                //Comment : Here parent question found then check for "WhenConsition,WhenResponse" criteria to validate DEPENDENT questions only 
                                #region Comment : Here WhenCondition & WhenResponse checking

                                //Comment : Here first we will check PARENT question type because if it's type 'R' then we can't apply "> & < " operator
                                if (parentQuestion.QuestionType.Equals("R"))
                                {
                                    #region Comment : Here if question type is Radiobutton

                                    if (question.WhenCondition == "=")
                                    {
                                        //it might be TRUE/Y/1 or FALSE/N/0 comparison
                                        if (question.WhenResponse == parentQuestionResponse)
                                            validateDependentQuestion = true;
                                    }

                                    #endregion
                                }
                                else
                                {
                                    #region Comment : Here if question type is other than Radiobutton

                                    //Comment : Here get question when-response
                                    questionWhenResponse = question.WhenResponse;

                                    //Comment : Here based on WhenCondition like '=,>,<,!=' do operation
                                    if (question.WhenCondition == "=")
                                    {
                                        if (Convert.ToInt32(parentQuestionResponse) == Convert.ToInt32(questionWhenResponse))
                                            validateDependentQuestion = true;
                                    }
                                    else if (question.WhenCondition == ">")
                                    {
                                        if (Convert.ToInt32(parentQuestionResponse) > Convert.ToInt32(questionWhenResponse))
                                            validateDependentQuestion = true;
                                    }
                                    else if (question.WhenCondition == "<")
                                    {
                                        if (Convert.ToInt32(parentQuestionResponse) < Convert.ToInt32(questionWhenResponse))
                                            validateDependentQuestion = true;
                                    }
                                    else if (question.WhenCondition == "!=")
                                    {
                                        if (Convert.ToInt32(parentQuestionResponse) != Convert.ToInt32(questionWhenResponse))
                                            validateDependentQuestion = true;
                                    }

                                    #endregion
                                }

                                #endregion

                                #region Comment : Here if according to parent question DEPENDENT question has to be validated the only check

                                if (validateDependentQuestion)
                                {
                                    errorMessage = ValidateUserResponse(question);
                                    if (!string.IsNullOrEmpty(errorMessage))
                                    {
                                        listOfErrors.Add(errorMessage);
                                    }
                                }

                                #endregion
                            }

                            #endregion
                        }
                    }
                    else
                    {
                        //Comment : Here WhenQuestion can not be null
                        errorMessage = string.Format("Q:{0} - {1}", question.questionId, Constants.EmptyReponse);
                        listOfErrors.Add(errorMessage);
                    }

                    #endregion
                }

            }
            catch (Exception)
            {
                //Comment : Here WhenQuestion can not be null
                listOfErrors.Add(Constants.QuestionValidationException);
            }

            return listOfErrors;
        }

        /// <summary>
        /// Validate individual question according question types and thier validation limits
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        private string ValidateUserResponse(Question question)
        {
            var errorMessage = string.Empty;
            var questionType = question.QuestionType;
            var questionResponse = question.UserResponse;

            if (!string.IsNullOrEmpty(questionResponse))
            {
                #region Comment : Here mandaotry and attempted but might having invalid format validation

                if (questionType.Equals("N"))
                {
                    bool isNumeric = UtilityFunctions.IsValidRegex(questionResponse, Constants.NumericRegex);

                    //Comment : Here if attempted then check for Min-Max limit for 'N' type question
                    bool isValidNumericVal = false;
                    if (isNumeric)
                    {
                        isValidNumericVal = (((Convert.ToInt32(questionResponse) >= Convert.ToInt32(question.QuestionResponseLimitList.First().Value)))
                           && (Convert.ToInt32(questionResponse) <= Convert.ToInt32(question.QuestionResponseLimitList.Last().Value))) ? true : false;
                    }

                    //if user reponse is not valid percentage value, return error
                    if (!isNumeric || !isValidNumericVal)
                    {
                        errorMessage = string.Format("Q:{0} - {1}", question.questionId, Constants.InvalidResponse);
                    }
                }
                else if (questionType.Equals("P"))
                {
                    bool isNumeric = UtilityFunctions.IsValidRegex(questionResponse, Constants.NumericRegex);
                    bool isValidPercentageVal = false;

                    if (isNumeric)
                    {
                        isValidPercentageVal = (((Convert.ToInt32(questionResponse) >= Convert.ToInt32(question.QuestionResponseLimitList.First().Value)))
                           && (Convert.ToInt32(questionResponse) <= Convert.ToInt32(question.QuestionResponseLimitList.Last().Value))) ? true : false;
                    }

                    //if user reponse is not valid percentage value, return error
                    if (!isNumeric || !isValidPercentageVal)
                    {
                        errorMessage = string.Format("Q:{0} - {1}", question.questionId, Constants.InvalidResponse);
                    }
                }
                else if (questionType.Equals("R"))
                {
                    bool isValidBoolVal =
                        (
                            questionResponse.Equals(question.QuestionResponseLimitList.First().Value)
                            || questionResponse.Equals(question.QuestionResponseLimitList.Last().Value)
                            || questionResponse.ToLower().Equals("true")
                            || questionResponse.ToLower().Equals("false")
                        ) ? true : false;

                    //if user Response is not valid boolean value, return error
                    if (!isValidBoolVal)
                    {
                        errorMessage = string.Format("Q:{0} - {1}", question.questionId, Constants.InvalidResponse);
                    }
                }

                #endregion
            }
            else
            {
                #region Comment : Here question is mandatory but not attempted

                errorMessage = string.Format("Q:{0} - {1}", question.questionId, Constants.EmptyReponse);

                /*
                //Comment : Here in case of 'P' type question it's not exceptionally mandatory
                if (!questionType.Equals("P"))
                {
                    errorMessage = string.Concat(Constants.EmptyReponse, ":", question.questionId);
                }
                */

                #endregion
            }

            return errorMessage;
        }

        #region Validate user FEIN/SSN/TIN number

        /// <summary>
        /// Check user account existance based on EmailId
        /// </summary>
        /// <param name="emailId"></param>
        /// <returns></returns>
        //[HttpGet]
        public JsonResult IsValidTaxIdNumber(string taxIdNumber, bool saveInSession)
        {
            //Comment : Here set default return status FALSE for suplied taxIdNumber
            var validTaxIdNumber = Json(new { resultStatus = "False", resultText = "" }, JsonRequestBehavior.AllowGet);

            #region Comment : Here validate taxIdNumber

            // bool validated = false;
            CustomSession appSession = GetCustomSessionWithPurchaseVM();
            try
            {
                CommonFunctionality commonFunctionality = new CommonFunctionality();

                //if FEIN/SSN/TIN is invalid, return false
                if (commonFunctionality.ValidateTaxIdAndSSN(taxIdNumber))
                {
                    //GUIN-238 : true in following promise will denote if feinNumber/taxId value will be updated in session or not 
                    if (saveInSession)
                    {
                        if (appSession.PurchaseVM.BusinessInfo.IsNull())
                        {
                            appSession.PurchaseVM.BusinessInfo = new BusinessInfo();
                        }

                        appSession.PurchaseVM.BusinessInfo.TaxIdOrSSN = taxIdNumber;
                        SetCustomSession(appSession);
                    }
                    validTaxIdNumber = Json(new { resultStatus = "True", resultText = "Valid tax-id number" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                BHIC.Common.Logging.ILoggingService logger = BHIC.Common.Logging.LoggingService.Instance;
                logger.Fatal(string.Format("Method {0} executed with error message : {1}", "IsValidTaxIdNumber", ex.ToString()));
            }

            #endregion

            return validTaxIdNumber;
        }

        #endregion

        #endregion
    }
}
