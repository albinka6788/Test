#region Using Directives

using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

using BHIC.Common.Config;
using BHIC.Contract.PurchasePath;
using BHIC.Core.PurchasePath;
using BHIC.Domain.Background;
using BHIC.Domain.Policy;
using BHIC.Portal.WC.App_Start;
using BHIC.Common.Quote;
using BHIC.ViewDomain;

using DML_DTO = BHIC.DML.WC.DTO;
using System.Web;
using BHIC.Domain.Service;
using BHIC.ViewDomain.Landing;
using BHIC.Common.CommonUtilities;
using Newtonsoft.Json;
using BHIC.Common.Client;
using Newtonsoft.Json.Linq;
using BHIC.Domain.BackEnd;
using BHIC.Contract.Background;
using BHIC.Core.Background;
using System.Globalization;

#endregion

namespace BHIC.Portal.WC.Areas.PurchasePath.Controllers
{
    //[CustomTransactionLogFilterAttribute]
    [ValidateSession]
    [CustomAntiForgeryToken]
    public class QuoteSummaryController : BaseController
    {
        #region Variables : Page Level Local Variables Decalration

        private static string QuoteSummaryPage = "QuoteSummaryPage";
        private static string QuoteSummaryPageBaseMethod = "QuoteSummary";

        CustomSession appSession;

        #endregion

        #region Methods : public methods

        #region MethodType : GET

        //
        // GET: /PurchasePath/QuoteSummary/
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// return quote summary based on exposure & policy data and others
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetQuoteSummary(string quoteId)
        {
            #region Comment : Here if request query string is found(means request came from mail embedded link) then do following action

            if (quoteId != null)
            {
                #region Comment : Here Added new functional requirement changes to check for PolicyCentre logged user session

                HttpContextBase context = this.ControllerContext.HttpContext;

                if (Session["user"] == null)
                {
                    ValidatePolicyCenterLoggedUser(context);
                }

                #endregion

                //get quote-id in int format for future references
                int intQuoteId = 0;

                //clear app session
                appSession = null;
                SetCustomSession(appSession);
                QuoteCookieHelper.Cookie_SaveQuoteId(context, intQuoteId);

                ICommonFunctionality commonFunctionality = GetCommonFunctionalityProvider();
                appSession = RetrieveSavedQuote(quoteId, commonFunctionality, out intQuoteId);

                //Comment : Here if able to retrieve saved session from DB layer then do following
                if (appSession != null)
                {
                    //Comment : Here finlly update this into session
                    SetCustomSession(appSession);

                    //Save/Update QuoteId
                    context = this.ControllerContext.HttpContext;
                    QuoteCookieHelper.Cookie_SaveQuoteId(context, intQuoteId);
                }
            }

            #endregion

            #region Comment : Here get all custom session information reuired for this page processing

            GetSession();

            //Comment : Here stored in a cookie on the user's machine
            int wcQuoteId = GetCookieQuoteId();

            #endregion

            #region Comment : Here must check for this quote QuoteStatus belongs to QuoteSummary page ?

            IsValidPageRequest();

            #endregion

            #region Comment : Here QuoteSummary interface refernce to do/make process all business logic

            //Comment : Here BLL stands for "Business Logic Layer"
            IQuoteSummary quoteSummaryBLL = GetQuoteSummaryProvider();

            #endregion

            #region Comment : Here get applicable PaymentPlans for this Quote

            QuoteSummaryViewModel quoteSummaryViewModel = new QuoteSummaryViewModel();

            #endregion

            //Comment : Here same time set this to CustomSession object
            if (appSession != null && wcQuoteId > 0)
            {
                #region Comment : Here must check payment plans retrieved or not

                decimal quotePremiumAmt = 0;

                var questionnaireVM = appSession.QuestionnaireVM;
                if (questionnaireVM != null)
                {
                    //Comment : Here first of all get PremiumAmount 
                    quotePremiumAmt = questionnaireVM.PremiumAmt;
                }

                //Comment : Here get PaymentPlans list
                var paymentPlansResponse = quoteSummaryBLL.GetPaymentPlans(quotePremiumAmt, Convert.ToInt32(wcQuoteId));

                //Comment : Here don not unnecessarily call below method call
                if (paymentPlansResponse.Count > 0)
                {
                    quoteSummaryViewModel.PaymentPlans = paymentPlansResponse ?? new List<PaymentPlan>();
                    quoteSummaryViewModel.PaymentTerms = null;
                    quoteSummaryViewModel.QuestionsResponse = null;

                    #region Comment : Here check state MandatoryDeductible applicabiolity

                    quoteSummaryViewModel.HasMandatoryDeductible = (appSession != null && appSession.StateAbbr.Length > 0) ? quoteSummaryBLL.StateHasMandatoryDeductible(appSession.StateAbbr) : false;

                    #endregion

                    //Add SelectedPaymentPlan from session (in case of user request come from mailed link)
                    if (appSession.QuoteSummaryVM != null)
                    {
                        //Comment : Here If prevously selected & stored payment-plan "Does-Not-Exists" in current PaymentPlans list then reset it
                        if (quoteSummaryViewModel.PaymentPlans.Any(res => res.PaymentPlanId == appSession.QuoteSummaryVM.SelectedPaymentPlan.PaymentPlanId))
                        {
                            quoteSummaryViewModel.SelectedPaymentPlan = appSession.QuoteSummaryVM.SelectedPaymentPlan;
                        }
                        else
                        {
                            quoteSummaryViewModel.SelectedPaymentPlan = new PaymentPlan();
                        }
                    }

                    #region Comment : Here get LowestInstallmentPremium and set VM object pass it to view

                    decimal lowestInstallmentPremium = 0;

                    //Comment : Here must check PremiumAmount for this quote and then get LowestInstallmentPremium                        
                    if (quotePremiumAmt > 0)
                    {
                        lowestInstallmentPremium = quoteSummaryBLL.GetLowestInstallmentPremium(paymentPlansResponse, quotePremiumAmt);
                        if (lowestInstallmentPremium > 0)
                        {
                            quoteSummaryViewModel.PremiumAmt = quotePremiumAmt;
                            quoteSummaryViewModel.LowestInstallmentPremium = lowestInstallmentPremium;

                            //other data
                            quoteSummaryViewModel.InstallmentFee = appSession.QuestionnaireVM.InstallmentFee;
                            quoteSummaryViewModel.XModValue = appSession.QuestionnaireVM.XModValue;
                            quoteSummaryViewModel.QuoteReferenceNo = wcQuoteId.ToString();
                        }
                    }

                    #endregion

                    #region Comment : Here get and set QuoteSummaryVM for future usage

                    //Comment : Here must check object instance existance
                    var sessionQuoteSummaryVM = quoteSummaryBLL.GetQuoteSummaryVM();

                    if (sessionQuoteSummaryVM == null)
                    {
                        appSession.QuoteSummaryVM = new QuoteSummaryViewModel();
                    }

                    //Comment : Here add this data into current local custom session object then let it update in final copy in next step
                    appSession.QuoteSummaryVM.PaymentPlans = paymentPlansResponse;
                    appSession.QuoteSummaryVM.LowestInstallmentPremium = lowestInstallmentPremium;
                    appSession.QuoteSummaryVM.QuoteReferenceNo = wcQuoteId.ToString();

                    //Comment : Here set null to heavy data object before assigning into session object
                    appSession.QuoteSummaryVM.QuestionsResponse = null;

                    //Comment : Here finlly update this into session
                    SetCustomSession(appSession);

                    #endregion
                }

                #endregion
            }

            IStateService stateService = new StateService();
            quoteSummaryViewModel.stateName = stateService.GetStateList(new StateRequestParms { StateAbbr = appSession.StateAbbr, StateName = string.Empty }, guardServiceProvider).Single(s => s.StateAbbr == appSession.StateAbbr).StateName;


            if (quoteSummaryViewModel.HasMandatoryDeductible)
            {
                WCDeductibleResponse wcdr = SvcClient.CallService<WCDeductibleResponse>("WCDeductibles?Carrier=" + appSession.QuestionnaireVM.Carrier + "&State=" + appSession.StateAbbr, guardServiceProvider);
                // quoteSummaryViewModel.Deductibles.AddRange(wcdr.Deductibles);
                //  quoteSummaryViewModel.selectedDeductible = (appSession.QuoteSummaryVM.selectedDeductible == null) ? new WCDeductibles() : appSession.QuoteSummaryVM.selectedDeductible;

                // Filter Deductible List
                // FilterDeductibleList(quoteSummaryViewModel.WCDeductibles)

                quoteSummaryViewModel.ListOfDeductiblesTypes = wcdr.DeductibleTypes;
                quoteSummaryViewModel.Deductibles = FilterDeductibles(wcdr.Deductibles);

                appSession.QuoteSummaryVM.Deductibles = quoteSummaryViewModel.Deductibles;
                quoteSummaryViewModel.selectedDeductible = (appSession.QuoteSummaryVM.selectedDeductible == null) ? new WCDeductibles() : appSession.QuoteSummaryVM.selectedDeductible;
                appSession.QuoteSummaryVM.ListOfDeductiblesTypes = quoteSummaryViewModel.ListOfDeductiblesTypes;
            }

            quoteSummaryViewModel.btnText = (quoteSummaryViewModel.Deductibles.Count > 0)
                                            ? quoteSummaryViewModel.btnText = (appSession.QuoteSummaryVM.btnText == "") ? "Continue" : appSession.QuoteSummaryVM.btnText
                                            : "Continue";

            WCLimitsListResponse wclr = SvcClient.CallService<WCLimitsListResponse>("WCLimitsList?QuoteId=" + appSession.QuoteID, guardServiceProvider);
            WCLimit wcl = wclr.WcLimits[0];
            if (wcl != null)
                quoteSummaryViewModel = SetEmployeeLimitTextAndValue(quoteSummaryViewModel, wcl);


            //Comment : Progress bar navigation
            List<NavigationModel> links = new List<NavigationModel>();
            NavigationController nc = new NavigationController();
            links = nc.GetProgressBarLinks(appSession.PageFlag);
            quoteSummaryViewModel.NavigationLinks = links;
            return PartialView("_GetQuoteSummary", quoteSummaryViewModel);
        }

        #endregion

        #region MethodType : POST

        [HttpPost]
        public ActionResult PostQuotePaymentPlan(PaymentPlan selectedPaymentPlan)
        {
            //todo: validate quote model
            var listOfModelErrors = ValidateQuoteModel(selectedPaymentPlan);

            ////if model has error return error list
            //if (listOfModelErrors.Count > 0)
            //{
            //    return Json(new
            //    {
            //        response = false,
            //        resultStatus = "NOK",
            //        message = "",  //This is need to be change, non-relevent message return type
            //        resultMessages = listOfModelErrors
            //    }, JsonRequestBehavior.AllowGet);
            //}






            if (selectedPaymentPlan != null)
            {
                #region Comment : Here get all custom session information reuired for this page processing

                GetSession();

                //Comment : Here stored in a cookie on the user's machine
                int wcQuoteId = GetCookieQuoteId();

                #endregion

                //Comment : Here same time set this to CustomSession object
                if (appSession != null && wcQuoteId > 0)
                {
                    #region Comment : Here QuoteSummary interface refernce to do/make process all business logic

                    //Comment : Here BLL stands for "Business Logic Layer"
                    IQuoteSummary quoteSummaryBLL = GetQuoteSummaryProvider();

                    #endregion

                    #region Comment : Here custom session data object & quote-id foucnd then do next level processing

                    var questionnaireVM = new BHIC.ViewDomain.QuestionEngine.QuestionnaireViewModel();
                    if (appSession.QuestionnaireVM != null)
                    {
                        questionnaireVM = appSession.QuestionnaireVM;
                    }

                    //Commented : Here [GUIN-] Every Time Get PaymentTerms From API
                    PaymentTerms paymentTerms = quoteSummaryBLL.GetPaymentTerms(Convert.ToInt32(wcQuoteId));

                    //PaymentTerms paymentTerms =
                    //    (appSession.QuoteSummaryVM != null && appSession.QuoteSummaryVM.PaymentTerms.PaymentPlanId > 0) ?
                    //    appSession.QuoteSummaryVM.PaymentTerms : quoteSummaryBLL.GetPaymentTerms(Convert.ToInt32(wcQuoteId));

                    if (paymentTerms.PaymentPlanId.Value > 0)
                    {
                        #region Comment : Here successful PaymentPlan post add this in session object for oher page in Purchase-Flow (BuyPolicy) usage

                        try
                        {
                            #region Comment : STEP - 2. Here set new generated quote information into custom session for future application session refernces

                            //Comment : Here if local custom section is got cleared then call it gain from shared application custom session
                            if (appSession == null)
                            {
                                GetSession();
                            }
                            else if (appSession != null)
                            {
                                //Comment : Here must check object instance existance
                                var sessionQuoteSummaryVM = quoteSummaryBLL.GetQuoteSummaryVM();

                                if (sessionQuoteSummaryVM == null)
                                {
                                    appSession.QuoteSummaryVM = new QuoteSummaryViewModel();
                                }

                                #region Comment : Here add this data into current local custom session object then let it update in final copy in next step

                                //Comment : Here set null to heavy data object before assigning into session object
                                appSession.QuoteSummaryVM.PaymentTerms = paymentTerms;

                                //Comment : Here set selected payment plan in session for future selection from saved DB quote
                                //appSession.QuoteSummaryVM.SelectedPaymentPlan = selectedPaymentPlan;

                                #endregion

                                #region Comment : Here STEP - 3. Add this generated quote in application custom session persisatance which will be used for PC "Saved Quotes" functionalities

                                //Comment : Here get logged PC user id
                                var loggedUserId = GetLoggedUserId();

                                if (loggedUserId != null)
                                {
                                    //Comment : Here CommonFunctionality interface refernce to do/make process all business logic
                                    ICommonFunctionality commonFunctionality = GetCommonFunctionalityProvider();

                                    //Comment : Here call BLL to make this data stored in DB
                                    bool isStoredInDB = commonFunctionality.AddApplicationCustomSession(
                                        new DML_DTO.CustomSession() { QuoteID = wcQuoteId, SessionData = commonFunctionality.StringifyCustomSession(appSession), IsActive = true, CreatedDate = DateTime.Now, CreatedBy = loggedUserId ?? 1, ModifiedDate = DateTime.Now, ModifiedBy = loggedUserId ?? 1 });
                                }

                                #endregion
                                //Comment : Here finlly update this into session
                                SetCustomSession(appSession);
                            }

                            #endregion
                        }
                        catch (Exception)
                        {
                            throw;
                        }

                        #endregion

                        //Comment : Progress bar navigation flag handling
                        //Session["flag"] = 4;
                        appSession.PageFlag = 4;
                        SetCustomSession(appSession);

                        return Json(new { resultStatus = "OK", resultText = "Successfully submitted" }, JsonRequestBehavior.AllowGet);
                    }

                    #region Commented Exisitng Code
                    /*
                    PaymentTerms paymentTerms = new PaymentTerms();

                    paymentTerms.QuoteId = Convert.ToInt32(wcQuoteId);
                    paymentTerms.PaymentPlanId = selectedPaymentPlan.PaymentPlanId;
                    paymentTerms.DownPayment = quoteSummaryBLL.GetDownPaymentAmount(questionnaireVM.PremiumAmt, selectedPaymentPlan);
                    paymentTerms.Installments = selectedPaymentPlan.Pays;
                    paymentTerms.InstallmentAmount = quoteSummaryBLL.GetFutureInstallmentAmount(questionnaireVM.PremiumAmt, selectedPaymentPlan);
                    paymentTerms.InstallmentFee = questionnaireVM.InstallmentFee;
                    paymentTerms.Frequency = "";
                    paymentTerms.FrequencyCode = selectedPaymentPlan.Freq;

                    //Comment : Here update this payment plan into service provider system
                    var paymentTermsResponse = quoteSummaryBLL.AddPaymentTerms(paymentTerms);

                    #region Comment : Here set error message from API response in case of any non-system error type

                    if (paymentTermsResponse != null && !paymentTermsResponse.RequestSuccessful)
                    {
                        var listOfErrors = new List<string>();
                        paymentTermsResponse.Messages.ForEach(msg => listOfErrors.Add(msg.Text));

                        //Comment : Here retrun execution of code from here itself in case of some error occured
                        if (listOfErrors.Count > 0)
                        {
                            return Json(new { resultStatus = "NOK", resultText = "Something went wrong while posting payment plan data !", resultMessages = listOfErrors }, JsonRequestBehavior.AllowGet);
                        }
                    }

                    #endregion

                    if (paymentTermsResponse.RequestSuccessful)
                    {
                        #region Comment : Here successful PaymentPlan post add this in session object for oher page in Purchase-Flow (BuyPolicy) usage

                        try
                        {
                            #region Comment : STEP - 2. Here set new generated quote information into custom session for future application session refernces

                            //Comment : Here if local custom section is got cleared then call it gain from shared application custom session
                            if (appSession == null)
                            {
                                GetSession();
                            }
                            else if (appSession != null)
                            {
                                //Comment : Here must check object instance existance
                                var sessionQuoteSummaryVM = quoteSummaryBLL.GetQuoteSummaryVM();

                                if (sessionQuoteSummaryVM == null)
                                {
                                    appSession.QuoteSummaryVM = new QuoteSummaryViewModel();
                                }

                                #region Comment : Here add this data into current local custom session object then let it update in final copy in next step

                                //Comment : Here set null to heavy data object before assigning into session object
                                appSession.QuoteSummaryVM.PaymentTerms = paymentTerms;

                                //Comment : Here set selected payment plan in session for future selection from saved DB quote
                                appSession.QuoteSummaryVM.SelectedPaymentPlan = selectedPaymentPlan;

                                #endregion

                                #region Comment : Here STEP - 3. Add this generated quote in application custom session persisatance which will be used for PC "Saved Quotes" functionalities

                                //Comment : Here get logged PC user id
                                var loggedUserId = GetLoggedUserId();

                                if (loggedUserId != null)
                                {
                                    //Comment : Here CommonFunctionality interface refernce to do/make process all business logic
                                    ICommonFunctionality commonFunctionality = GetCommonFunctionalityProvider();

                                    //Comment : Here call BLL to make this data stored in DB
                                    bool isStoredInDB = commonFunctionality.AddApplicationCustomSession(
                                        new DML_DTO.CustomSession() { QuoteID = wcQuoteId, SessionData = commonFunctionality.StringifyCustomSession(appSession), IsActive = true, CreatedDate = DateTime.Now, CreatedBy = loggedUserId ?? 1, ModifiedDate = DateTime.Now, ModifiedBy = loggedUserId ?? 1 });
                                }

                                #endregion

                                //Comment : Here finlly update this into session
                                SetCustomSession(appSession);
                            }

                            #endregion
                        }
                        catch (Exception)
                        {
                            throw;
                        }

                        #endregion

                        //Comment : Progress bar navigation flag handling
                        Session["flag"] = 4;

                        return Json(new { resultStatus = "OK", resultText = "Successfully submitted" }, JsonRequestBehavior.AllowGet);
                    }
                    */
                    #endregion

                    #endregion
                }
            }

            return Json(new { resultStatus = "NOK", resultText = "Something went wrong while posting payment plan data !" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// It will validate Quote Model
        /// </summary>
        /// <param name="selectedPaymentPlan"></param>
        /// <returns>Return true if model is valid, false otherwise</returns>
        private List<string> ValidateQuoteModel(PaymentPlan selectedPaymentPlan)
        {
            #region Comment : Here list of payment-plans from session

            GetSession();
            var paymentPlans = appSession.QuoteSummaryVM != null ? appSession.QuoteSummaryVM.PaymentPlans : null;

            #endregion

            #region Comment : Here must match user selected plan with application session ListOfPaymentPlans

            PaymentPlan matchedPaymentPlan = paymentPlans != null ? (PaymentPlan)paymentPlans.Where(x => x.PaymentPlanId == selectedPaymentPlan.PaymentPlanId).FirstOrDefault() : null;

            ICommonFunctionality commonFunctionality = GetCommonFunctionalityProvider();

            return commonFunctionality.ComparePlanObjects(selectedPaymentPlan, matchedPaymentPlan);

            #endregion
        }

        /// <summary>
        /// Send mail with current saved quote revocation link
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveForLater(PaymentPlan selectedPaymentPlan, WCDeductibles selectedDeductible, string emailId)
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
                    var quoteSummaryPostResponse = PostQuotePaymentPlan(selectedPaymentPlan);

                    //Comment : Here if question post processed
                    if (quoteSummaryPostResponse != null)
                    {

                        JsonResult jsonResult = quoteSummaryPostResponse as JsonResult;
                        dynamic dResult = jsonResult.Data;

                        #region Comment : Here if user data has beedn posted successfully then only send mail & embedded link to user

                        if (dResult.ToString().Contains("resultStatus = OK"))
                        {
                            #region Comment : Here CommonFunctionality interface refernce to do/make process all business logic

                            ICommonFunctionality commonFunctionality = GetCommonFunctionalityProvider();

                            //Comment : Here call BLL to make this data stored in DB
                            bool isStoredInDB = commonFunctionality.AddApplicationCustomSession(
                                new DML_DTO.CustomSession() { QuoteID = wcQuoteId, SessionData = commonFunctionality.StringifyCustomSession(appSession), IsActive = true, CreatedDate = DateTime.Now, CreatedBy = GetLoggedUserId() ?? 1, ModifiedDate = DateTime.Now, ModifiedBy = GetLoggedUserId() ?? 1 });

                            #endregion

                            #region Comment : Here if stored in DB end then finally send user mail

                            if (wcQuoteId != 0 && isStoredInDB)
                            {
                                bool mailSent = SendMailSaveForLater(QuoteSummaryPageBaseMethod, wcQuoteId, emailId);

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


        [HttpPost]
        public ActionResult ReCalculateWCDeductibleQuote(WCDeductibles selectedWCDeductible)
        {
            try
            {
                GetSession();
                var diductibleModiferId = (appSession.QuoteSummaryVM.deductibleModiferId == null) ? null : appSession.QuoteSummaryVM.deductibleModiferId;

                var batchActionList = new BatchActionList();	// BatchActionList: list of actions routed to the Insurance Service for response
                BatchResponseList batchResponseList;			// BatchResponseList: list of responses returned from the Insurance Service
                string jsonResponse;							// JSON-formated response data returned from the Insurance Service
                string ActionType = string.Empty;
                bool flag = true;       

                if (selectedWCDeductible != null)
                {
                    appSession.QuoteSummaryVM.selectedDeductible = selectedWCDeductible;

                    // Adding Modifer Action to Batch List --- > Start 
                    var modifierPOST = new BatchAction() { ServiceName = "Modifiers", ServiceMethod = "POST", RequestIdentifier = "Modifiers" };
                    modifierPOST.BatchActionParameters.Add(new BatchActionParameter
                    {
                        Name = "Modifier",
                        Value = JsonConvert.SerializeObject(new Modifier
                        {
                            ModifierId = diductibleModiferId,
                            QuoteId = appSession.QuoteID,
                            LOB = appSession.LobId,
                            ZipCode = appSession.ZipCode,
                            State = appSession.StateAbbr,
                            ModType = "Deduct",
                            ModValue = selectedWCDeductible.DeductAmt,
                            ClassCode = selectedWCDeductible.ClassCode,
                            SeqCode = appSession.QuoteSummaryVM.ListOfDeductiblesTypes[0].SeqCode,
                            ClassSuffix = selectedWCDeductible.DBASE,
                        })
                    });
                    // Modifier Post Adding to list
                    batchActionList.BatchActions.Add(modifierPOST);
                    ActionType = "POST";
                }
                else
                {
                    var modifierDELETE = new BatchAction() { ServiceName = "Modifiers", ServiceMethod = "DELETE", RequestIdentifier = "Modifiers" };
                    modifierDELETE.BatchActionParameters.Add(new BatchActionParameter
                    {
                        Name = "ModifierRequestParms",
                        Value = JsonConvert.SerializeObject(new ModifierRequestParms
                        {
                            ModifierId = diductibleModiferId,
                            QuoteId = appSession.QuoteID,
                            Lob = appSession.LobId,
                            State = appSession.StateAbbr
                        })
                    });
                    // Modifier Delete Adding to list
                    if (diductibleModiferId != null)
                    {
                        ActionType = "DELETE";
                        batchActionList.BatchActions.Add(modifierDELETE);
                    }
                }

                // Adding Modifer Action to Batch List --- > End

                if (ActionType != "")
                {
                    // Adding Rating Post Action to Batch List --- > Start 
                    var ratingPOST = new BatchAction { ServiceName = "Rating", ServiceMethod = "POST", RequestIdentifier = "Rating" };
                    ratingPOST.BatchActionParameters.Add(new BatchActionParameter { Name = "RatingRequestParms", Value = JsonConvert.SerializeObject(new Modifier { QuoteId = appSession.QuoteID }) });
                    batchActionList.BatchActions.Add(ratingPOST);
                    // Adding Rating Post Action to Batch List --- > End

                    // Adding RatingData Post Action to Batch List --- > Start 
                    var ratingDataGET = new BatchAction { ServiceName = "RatingData", ServiceMethod = "GET", RequestIdentifier = "RatingData" };
                    ratingDataGET.BatchActionParameters.Add(new BatchActionParameter { Name = "RatingDataRequestParms", Value = JsonConvert.SerializeObject(new Modifier { QuoteId = appSession.QuoteID }) });
                    batchActionList.BatchActions.Add(ratingDataGET);
                    // Adding RatingData Post Action to Batch List --- > End

                    // submit the BatchActionList to the Insurance Service
                    jsonResponse = SvcClient.CallServiceBatch(batchActionList, guardServiceProvider);

                    // deserialize the results into a BatchResponseList
                    batchResponseList = JsonConvert.DeserializeObject<BatchResponseList>(jsonResponse);

                    if (batchResponseList != null)
                    {
                        //Comment : Here first get BATCH execution sucessful status
                        flag = !batchResponseList.BatchResponses.Any(res => !res.RequestSuccessful);

                        #region Comment : Here get "ModifierId" for subsequent POST api call

                        if (batchResponseList.BatchResponses.Exists(m => m.RequestIdentifier == "Modifiers"))
                        {
                            var modifiersResponse = batchResponseList.BatchResponses.SingleOrDefault(m => m.RequestIdentifier == "Modifiers").JsonResponse;
                            if (modifiersResponse != null)
                            {
                                var operationStatusDeserialized = JsonConvert.DeserializeObject<OperationStatus>(modifiersResponse);

                                if (operationStatusDeserialized != null && operationStatusDeserialized.AffectedIds.Exists(m => m.DTOProperty == "ModifierId"))
                                {

                                    appSession.QuoteSummaryVM.deductibleModiferId =
                                        Convert.ToInt32(operationStatusDeserialized.AffectedIds.SingleOrDefault(m => m.DTOProperty == "ModifierId").IdValue);

                                    JObject jo = JObject.Parse(batchResponseList.BatchResponses[2].JsonResponse);
                                    decimal premiumAmt = Convert.ToDecimal(jo.SelectToken(@"RatingData.Premium").Value<string>());
                                    IQuoteSummary quoteSummaryBLL = GetQuoteSummaryProvider();
                                    var paymentPlansResponse = quoteSummaryBLL.GetPaymentPlans(premiumAmt, Convert.ToInt32(appSession.QuoteID));
                                    decimal lowestInstallmentPremium = quoteSummaryBLL.GetLowestInstallmentPremium(paymentPlansResponse, premiumAmt);
                                    if (lowestInstallmentPremium > 0)
                                    {
                                        appSession.QuoteSummaryVM.PremiumAmt = Convert.ToDecimal(premiumAmt);
                                        appSession.QuoteSummaryVM.LowestInstallmentPremium = lowestInstallmentPremium;
                                        appSession.QuoteSummaryVM.btnText = "Continue";
                                        appSession.QuestionnaireVM.PremiumAmt = Convert.ToDecimal(premiumAmt);

                                    }

                                    if (ActionType == "DELETE")
                                    {
                                        // Resetting default options after delete success.
                                        appSession.QuoteSummaryVM.deductibleModiferId = null;
                                        appSession.QuoteSummaryVM.selectedDeductible = null;
                                    }

                                    // Setting app session to coustomsession



                                    //Comment : Here get logged PC user id
                                    var loggedUserId = GetLoggedUserId();

                                    if (loggedUserId != null)
                                    {
                                        //Comment : Here CommonFunctionality interface refernce to do/make process all business logic
                                        ICommonFunctionality commonFunctionality = GetCommonFunctionalityProvider();

                                        //Comment : Here call BLL to make this data stored in DB
                                        bool isStoredInDB = commonFunctionality.AddApplicationCustomSession(
                                            new DML_DTO.CustomSession() { QuoteID = appSession.QuoteID, SessionData = commonFunctionality.StringifyCustomSession(appSession), IsActive = true, CreatedDate = DateTime.Now, CreatedBy = loggedUserId ?? 1, ModifiedDate = DateTime.Now, ModifiedBy = loggedUserId ?? 1 });
                                    }

                                    SetCustomSession(appSession);

                                }
                            }
                        }

                        #endregion
                    }

                }
                return (flag) ? Json(new { resultStatus = "OK", resultText = "Success" }, JsonRequestBehavior.AllowGet)
                              : Json(new { resultStatus = "NOK", resultText = "Something went wrong while processing save for later !" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { resultStatus = "NOK", resultText = "Something went wrong while processing save for later !" }, JsonRequestBehavior.AllowGet);
            }

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
                throw new ApplicationException(string.Format("{0} : {1} !", QuoteSummaryPage, Constants.QuoteIdCookieEmpty));
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
                    throw new ApplicationException(string.Format("{0},{1} : {2} !", QuoteSummaryPage, Constants.SessionExpiredPartial, Constants.AppCustomSessionExpired));
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

            ////Comment : Here check is this/current quote have associated quesionnaire QuoteStatus as "Quote" otherwise invalid quote page request
            //var quoteStatus = string.Empty;
            //if (questionnaireVM != null)
            //{
            //    quoteStatus = questionnaireVM.QuoteStatus;
            //}
            //if (quoteVM == null || (questionnaireVM == null || (string.IsNullOrEmpty(quoteStatus) || !quoteStatus.Equals("Quote"))))
            //{
            //    throw new ApplicationException(string.Format("{0},{1} : You are not authorized to get quote !", QuoteSummaryPage, Constants.UnauthorizedRequest));
            //}

            if (appSession.PageFlag < 3)
            {
                throw new ApplicationException(string.Format("{0},{1} : You are not authorized to get quote !", QuoteSummaryPage, Constants.UnauthorizedRequest));
            }

            return true;
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
        /// Handling Each Accident/Policy/Each Employee Premium Display Text & Its Values
        /// </summary>
        /// <param name="qsv"></param>
        /// <param name="wcL"></param>
        /// <returns></returns>
        private QuoteSummaryViewModel SetEmployeeLimitTextAndValue(QuoteSummaryViewModel qsv, WCLimit wcL)
        {
            try
            {
                CultureInfo cinfo = new CultureInfo("en-US");
                Dictionary<string, string> lst = new Dictionary<string, string>();
                lst.Add((wcL.BIAE != null && wcL.BIAE != 0) ? "Each Accident" : "0", (wcL.BIAE != null && wcL.BIAE != 0) ? string.Format(cinfo, "{0:N0}", wcL.BIAE) : "0");
                lst.Add((wcL.BIDL != null && wcL.BIDL != 0) ? "Policy" : "0", (wcL.BIDL != null && wcL.BIDL != 0) ? string.Format(cinfo, "{0:N0}", wcL.BIDL) : "0");
                lst.Add((wcL.BIDE != null && wcL.BIDE != 0) ? "Each Employee Limit" : "0", (wcL.BIDE != null && wcL.BIDE != 0) ? string.Format(cinfo, "{0:N0}", wcL.BIDE) : "0");

                string Text = null;
                string Value = null;
                foreach (KeyValuePair<string, string> entry in lst)
                {
                    if (entry.Key != "0")
                    {
                        Text = (Text == null) ? entry.Key : Text + "/" + entry.Key;
                        Value = (Value == null) ? " $" + entry.Value : Value + "/ $" + entry.Value;
                    }
                }

                qsv.employeeLimitText = Text;
                qsv.employeeLimitValue = Value;
                return qsv;
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Deductibles recieved based on zip code are filtered Per Accident and Claims for top 3 values matching Business requirements
        /// </summary>
        /// <param name="wcDeductibles"></param>
        /// <returns></returns>
        private List<WCDeductibles> FilterDeductibles(List<WCDeductibles> wcDeductibles)
        {
            List<WCDeductibles> filteredDeductibles = new List<WCDeductibles>();

            filteredDeductibles.AddRange(wcDeductibles.Where(m => m.DeductAmt == 1000 || m.DeductAmt == 2500).ToList());

            if ((filteredDeductibles.Count(m => m.Names.ToLower().Contains("per accident") && m.DeductAmt == 1000) > 0 &&
                filteredDeductibles.Count(m => m.Names.ToLower().Contains("per claim") && m.DeductAmt == 1000) > 0) ||
                (filteredDeductibles.Count(m => m.Names.ToLower().Contains("per accident") && m.DeductAmt == 2500) > 0 &&
                filteredDeductibles.Count(m => m.Names.ToLower().Contains("per claim") && m.DeductAmt == 2500) > 0))
            {
                if (filteredDeductibles.Count(m => m.Names.ToLower().Contains("per claim")) > 1)
                {
                    if (filteredDeductibles.Count() > 3)
                    {
                        filteredDeductibles.Remove(filteredDeductibles.FirstOrDefault(m => m.Names.ToLower().Contains("per claim") && m.DeductAmt == 1000));
                    }

                    filteredDeductibles.Remove(filteredDeductibles.FirstOrDefault(m => m.Names.ToLower().Contains("per claim") && m.DeductAmt == 2500));
                }
                else
                {
                    filteredDeductibles.Remove(filteredDeductibles.FirstOrDefault(m => m.Names.ToLower().Contains("per claim")));
                }
            }

            filteredDeductibles.AddRange(wcDeductibles.Where(m => m.DeductAmt != 1000 && m.DeductAmt != 2500).OrderBy(m => m.DeductAmt).ThenBy(m => m.Names).ToList());

            // Remove Entries beyond 3rd value
            if (filteredDeductibles.Count() > 3)
                filteredDeductibles.RemoveRange(3, filteredDeductibles.Count() - 3);

            if (filteredDeductibles.Count() > 3)
                return filteredDeductibles.Where(m => m.DeductAmt <= 2500).OrderBy(m => m.DeductAmt).ToList();
            else
                return filteredDeductibles.OrderBy(m => m.DeductAmt).ToList();
        }

        #endregion
    }
}
