#region Using Directives

using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

using BHIC.Common;
using BHIC.Common.Client;
using BHIC.Common.Config;
using BHIC.Common.Mailing;
using BHIC.Contract.PurchasePath;
using BHIC.Core.PurchasePath;
using BHIC.Domain.Policy;
using BHIC.Domain.Service;
using BHIC.Portal.WC.App_Start;
using BHIC.Common.Configuration;
using BHIC.Common.Quote;
using BHIC.ViewDomain;
using BHIC.ViewDomain.QuestionEngine;
using Newtonsoft.Json;
using BHIC.Common.XmlHelper;
using System.Web;
using System.IO;
using BHIC.Common.CommonUtilities;
using System.IO.Compression;

using BHIC.Domain.PurchasePath;
using BHIC.Domain.QuestionEngine;
using BHIC.ViewDomain.Landing;

#endregion


namespace BHIC.Portal.WC.Areas.PurchasePath.Controllers
{
    //[CustomTransactionLogFilterAttribute]
    [ValidateSession]
    [CustomAntiForgeryToken]
    public class ReferralQuoteController : BaseController
    {
        #region Variables : Page Level Local Variables Decalration

        private static string QuoteSoftReferral = "Soft Referral";
        private static string QuoteHardReferral = "Hard Referral";
        private static string ReferralQuotePage = "ReferralQuotePage";

        CustomSession appSession;

        #endregion

        #region Methods : Public Methods

        #region MethodType : GET

        //
        // GET: /PurchasePath/ReferralQuote/
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Return referral quote view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetReferralQuote()
        {
            GetSession();
            //this page flag will be set in Exposure and Questions page
            //if (appSession.PageFlag == 2)
            //    appSession.PageFlag = 1;
            //else
            //    appSession.PageFlag = appSession.PageFlag - 1;

            SetCustomSession(appSession);

            #region Commented XmlReader Code
            /*
            XmlParser xmlParser = new XmlParser();

            var xmlData = "<?xml version=\"1.0\" encoding=\"ibm850\"?><Test xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><value1>Value 111</value1><value2>Value 222</value2></Test>";

            var referralReason = new ReferralReason
            {
                ScenarioId = 1,
                ReferrarPage = "Exposure"
            };

            ReferralReasons referralReasons = new ReferralReasons
            {
                ReferralReason = new List<ReferralReason>
                {
                    new ReferralReason
                    {
                        ScenarioId = 1,
                        ReferrarPage="Exposure"
                    },
                    new ReferralReason
                    {
                        ScenarioId = 2,
                        ReferrarPage="Quote"
                    },
                }
            };

            XmlFileReader xmlReader = new XmlFileReader();
            xmlData = xmlReader.GetXmlDocumentString(Server.MapPath("~/ReferralReasons.xml"));

            var getReferralReasons = XmlParser.ToObject<ReferralReasons>(xmlData);

            //var getReferralReasons = XmlParser.ToXml(referralReasons);
            if (!getReferralReasons.IsNull())
            {

            }
            */
            #endregion

            #region Comment : Here get all custom session information reuired for this page processing



            //Comment : Here stored in a cookie on the user's machine
            int wcQuoteId = GetCookieQuoteId();

            #endregion

            #region Comment : Here must check for this quote QuoteStatus belongs to Referral ?

            IsValidPageRequest();

            #endregion

            //Comment : Here same time set this to CustomSession object
            if (appSession != null && wcQuoteId > 0)
            {
                #region Comment : Here must check for this quote QuoteStatus belongs to Referral ?

                var quuestionnaireVM = appSession.QuestionnaireVM;
                if (quuestionnaireVM != null)
                {
                    var quoteStatus = quuestionnaireVM.QuoteStatus;
                    if (!string.IsNullOrEmpty(quoteStatus) && quoteStatus.Equals(QuoteSoftReferral))
                    {
                        var quoteVM = appSession.QuoteVM;
                        //if (quoteVM != null && (quoteVM.IsMultiStateApplicable || !quoteVM.IsMultiClassPrimaryExposureValid || quoteVM.MoreClassRequired || quoteVM.IsMultiClass))    //Prevs. Implem
                        appSession.BusinessInfoVM.IsMultiStateOrMultiClass = (quoteVM != null && quoteVM.IsMultiStateApplicable);
                    }
                }

                #endregion

                #region Comment : Here As per Guru's(10.05.2016) suggestion Save/Update running quote into database to maintain Referral/Decline history

                ICommonFunctionality commonFunctionality = new CommonFunctionality();

                //Comment : Here call BLL to make this data stored in DB
                bool isStoredInDB = commonFunctionality.AddApplicationCustomSession(
                    new DML.WC.DTO.CustomSession() { QuoteID = wcQuoteId, SessionData = commonFunctionality.StringifyCustomSession(appSession), IsActive = true, CreatedDate = DateTime.Now, CreatedBy = GetLoggedUserId() ?? 1, ModifiedDate = DateTime.Now, ModifiedBy = GetLoggedUserId() ?? 1 });

                #endregion
            }

            return PartialView("_GetReferralQuote", GetReferralModel());
        }

        [HttpGet]
        public ActionResult DeclinedQuote()
        {
            #region Comment : Here get all custom session information reuired for this page processing

            GetSession();

            //this page flag will be set in Exposure and Questions page
            //if (appSession.PageFlag == 2)
            //    appSession.PageFlag = 1;
            //else
            //    appSession.PageFlag = appSession.PageFlag - 1;

            SetCustomSession(appSession);

            #endregion

            #region Comment : Here must check for this quote QuoteStatus belongs to Decline ?

            IsValidPageRequestDecline();

            #endregion

            #region Comment : Here As per Guru's(10.05.2016) suggestion Save/Update running quote into database to maintain Referral/Decline history

            //Comment : Here stored in a cookie on the user's machine
            int wcQuoteId = GetCookieQuoteId();

            ICommonFunctionality commonFunctionality = new CommonFunctionality();

            //Comment : Here call BLL to make this data stored in DB
            bool isStoredInDB = commonFunctionality.AddApplicationCustomSession(
                new DML.WC.DTO.CustomSession() { QuoteID = wcQuoteId, SessionData = commonFunctionality.StringifyCustomSession(appSession), IsActive = true, CreatedDate = DateTime.Now, CreatedBy = GetLoggedUserId() ?? 1, ModifiedDate = DateTime.Now, ModifiedBy = GetLoggedUserId() ?? 1 });

            #endregion

            return PartialView("_DeclinedQuote");
        }

        /// <summary>
        /// Method will return rendered partial view for all primary & companion classes
        /// </summary>
        /// <returns></returns>
        public ActionResult GetReferralQuoteClassInformation()
        {
            return PartialView("_GetReferralQuoteClassInformation");
        }

        /// <summary>
        /// Method will return rendered partial view for question & response along with question-ruled highlighed in RED
        /// </summary>
        /// <returns></returns>
        public ActionResult GetReferralQuoteQuestionsAndResponses()
        {
            return PartialView("_GetReferralQuoteQuestionsAndResponses");
        }

        #endregion

        #region MethodType : POST

        /// <summary>
        /// This method will submit referral quote business contact information
        /// </summary>
        /// <param name="referralQuoteModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostReferralQuoteData(ReferralQuoteViewModel referralQuoteModel)
        {
            //var files = currentPolicyFile as System.Web.HttpRequestBase;
            //var referralQuoteModel = new ReferralQuoteViewModel();
            var listOfErrors = new List<string>();
            List<string> listOfUploadedFiles = new List<string>();

            //Comment : Here first of all get uploade files count
            var currentPolicyUploadedFileCount = (!Request.IsNull() && !Request.Files.IsNull()) ? Request.Files.Count : 0;

            #region Comment : Here get upladed referral model data

            if (!Request.Form["DataModel"].IsNull())
            {
                referralQuoteModel = JsonConvert.DeserializeObject<ReferralQuoteViewModel>(Request.Form["DataModel"].ToString());
            }

            #endregion

            if (referralQuoteModel != null)
            {
                #region Comment : Here model state and server side validation

                var listOfModelErrors = ValidateReferralQuote(referralQuoteModel);

                //if model has error return error list
                if (listOfModelErrors.Count > 0)
                {
                    return Json(new
                    {
                        response = false,
                        resultStatus = "NOK",
                        message = "",  //This is need to be change, non-relevent message return type
                        resultMessages = listOfModelErrors
                    }, JsonRequestBehavior.AllowGet);
                }

                #endregion

                // stored in a cookie on the user's machine
                int wcQuoteId = GetCookieQuoteId();

                if (wcQuoteId != 0)
                {
                    #region Comment : Here get all custom session information reuired for this step processing

                    GetSession();

                    #endregion

                    #region Comment : Here STEP-1 collect all uploded document and verify files & other file validation

                    if (currentPolicyUploadedFileCount > 0)
                    {
                        //Comment : Here files has uploded then must check permissable count of total files totherwise retrun error
                        if (currentPolicyUploadedFileCount <= ConfigCommonKeyReader.MaxFileCount)
                        {
                            #region Comment : Here in case user has uploded permissable count of files then

                            #region Comment : Here get uploaded files as enumberable

                            var files = Enumerable.Range(0, Request.Files.Count).Select(i => Request.Files[i]);

                            #endregion

                            #region Comment : Here iterate each file to upload,verify,validation and etc

                            if (!files.IsNull())
                            {
                                foreach (HttpPostedFileBase file in files)
                                {
                                    #region Comment : Here get file-name and create file on server directory

                                    string fileName = string.Empty;
                                    if (file.FileName.Contains("\\"))
                                    {
                                        var tempFilePath = file.FileName.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                                        fileName = tempFilePath[tempFilePath.Length - 1];
                                    }
                                    else
                                    {
                                        fileName = file.FileName;
                                    }

                                    //Comment : Here create complete/absolute file-path
                                    string filePath = Path.Combine(ConfigCommonKeyReader.UploadFiles, wcQuoteId.ToString(), fileName);

                                    //Comment : Here if directory not exist add file to folder to server location
                                    if (Path.GetDirectoryName(filePath).Length > 0 && !Directory.Exists(Path.GetDirectoryName(filePath)))
                                    {
                                        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                                    }

                                    #endregion

                                    #region Comment : Here based on uploded file types process as followed

                                    if (filePath.IndexOf(".zip", StringComparison.OrdinalIgnoreCase) > -1)
                                    {
                                        #region Comment : Here "Zipped" format files

                                        using (Stream inputFileStream = new MemoryStream())
                                        {
                                            file.InputStream.CopyTo(inputFileStream);
                                            FileUploadValidationStaus fileUploadValidationStaus = IsZipContentValid(inputFileStream, filePath);
                                            if (!fileUploadValidationStaus.IsValid)
                                            {
                                                // Throw error
                                                listOfErrors.Add(fileUploadValidationStaus.ValidationMessages);

                                                return Json(new { resultStatus = "NOK", resultText = "Unable to upload documents", resultMessages = listOfErrors }, JsonRequestBehavior.AllowGet);
                                            }

                                            using (FileStream outFile = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                                            {
                                                file.InputStream.EncryptFile(outFile, wcQuoteId.ToString());
                                                listOfUploadedFiles.Add(filePath);
                                            }
                                        }

                                        #endregion
                                    }
                                    else
                                    {
                                        #region Comment : Here when files are not uploaded in "Zip" format

                                        FileUploadValidationStaus fileUploadValidationStaus = UploadHelper.ValidateUploadedFiles(file, filePath);
                                        if (fileUploadValidationStaus.IsValid)
                                        {
                                            using (FileStream outFile = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                                            {
                                                file.InputStream.EncryptFile(outFile, wcQuoteId.ToString());
                                                listOfUploadedFiles.Add(filePath);
                                            }
                                        }
                                        else
                                        {
                                            // Throw error
                                            listOfErrors.Add(fileUploadValidationStaus.ValidationMessages);

                                            return Json(new { resultStatus = "NOK", resultText = "Unable to upload documents", resultMessages = listOfErrors }, JsonRequestBehavior.AllowGet);
                                        }

                                        #endregion
                                    }

                                    #endregion
                                }

                            }

                            #endregion

                            #endregion
                        }
                        else
                        {
                            #region Comment : Here in case user has uploded non-permissable count of files then return current execution

                            listOfErrors.Add(string.Format("Maximum {0} files can be uploaded", ConfigCommonKeyReader.MaxFileCount));
                            return Json(new { resultStatus = "NOK", resultText = "Unable to upload documents", resultMessages = listOfErrors }, JsonRequestBehavior.AllowGet);

                            #endregion
                        }
                    }

                    #endregion

                    #region Comment : Here STEP-2 collect all posted information into targeted DTOs

                    //Comment : here collect "Business Information"
                    var insuredNameData = new InsuredName();
                    insuredNameData.QuoteId = wcQuoteId;
                    insuredNameData.Name = referralQuoteModel.BusinessName;
                    insuredNameData.PrimaryInsuredName = true;

                    //Comment : here collect "Business Contact Information"
                    var businessContactData = new Contact();
                    businessContactData.QuoteId = wcQuoteId;
                    businessContactData.Name = referralQuoteModel.ContactName;
                    //businessContactData.Company = referralQuoteModel.BusinessName;
                    businessContactData.ContactType = "Misc";
                    businessContactData.Email = referralQuoteModel.Email;

                    //Comment: Here get number & extention sepratly from request data
                    var phoneNumber = referralQuoteModel.PhoneNumber.Split(new[] { 'x' });
                    if (phoneNumber != null && phoneNumber.Length > 0)
                    {
                        businessContactData.Phones = new List<Phone> 
                        { 
                            new Phone { PhoneNumber = UtilityFunctions.ToNumeric(phoneNumber[0]), Extension = UtilityFunctions.ToNumeric(phoneNumber[1]), 
                                //phone type 0 for "Business"
                                PhoneType = "0" } 
                        };
                    }

                    #endregion

                    #region Comment : Here STEP-3 create batch action and execute it to post referral-quote data to rpovider system

                    #region Comment : Here create batch actions

                    /*----------------------------------------
                    populate a BatchActionList that contains all requests to be sent to the Gaurd API
                    ----------------------------------------*/

                    var batchActionList = new BatchActionList();	// BatchActionList: list of actions routed to the Insurance Service for response
                    BatchResponseList batchResponseList;			// BatchResponseList: list of responses returned from the Insurance Service
                    string jsonResponse;							// JSON-formated response data returned from the Insurance Service

                    // populate a BatchAction that will be used to submit the InsuredName DTO to the Insurance Service
                    var insuredNameAction = new BatchAction { ServiceName = "InsuredNames", ServiceMethod = "POST", RequestIdentifier = "InsuredNames Data" };
                    insuredNameAction.BatchActionParameters.Add(new BatchActionParameter { Name = "insuredName", Value = JsonConvert.SerializeObject(insuredNameData) });
                    batchActionList.BatchActions.Add(insuredNameAction);

                    var contactAction = new BatchAction { ServiceName = "Contacts", ServiceMethod = "POST", RequestIdentifier = "BusinessContact Data" };
                    contactAction.BatchActionParameters.Add(new BatchActionParameter { Name = "contact", Value = JsonConvert.SerializeObject(businessContactData) });
                    batchActionList.BatchActions.Add(contactAction);

                    #region Comment : Here [GUIN-202-SreeRam] Handled Refereal Count(2.a.i) for "UserReports"

                    //Comment : Here [GUIN-202-Prem] Check for existing values of "QuoteStatus" flags
                    var quoteStatusVM = !appSession.QuoteStatusVM.IsNull() ? appSession.QuoteStatusVM : new QuoteStatusViewModel();

                    var quotestatusData = new QuoteStatus();
                    quotestatusData.LandingSaved = quoteStatusVM.LandingSaved;
                    quotestatusData.ClassesSelected = quoteStatusVM.ClassesSelected;
                    quotestatusData.ExposuresSaved = quoteStatusVM.ExposuresSaved;
                    quotestatusData.ContactRequested = DateTime.Now;
                    quotestatusData.QuoteId = wcQuoteId;
                    quotestatusData.UserIP = UtilityFunctions.GetUserIPAddress(this.ControllerContext.HttpContext.ApplicationInstance.Context);

                    //update QuoteStatus VM in current active session state
                    quoteStatusVM.ContactRequested = DateTime.Now;
                    appSession.QuoteStatusVM = quoteStatusVM;

                    var quotestatusAction = new BatchAction { ServiceName = "QuoteStatus", ServiceMethod = "POST", RequestIdentifier = "QuoteStatus Data" };
                    quotestatusAction.BatchActionParameters.Add(new BatchActionParameter { Name = "QuoteStatus", Value = JsonConvert.SerializeObject(quotestatusData) });
                    batchActionList.BatchActions.Add(quotestatusAction);

                    #endregion

                    #endregion

                    #region Comment : Here get batch action result

                    // submit the BatchActionList to the Insurance Service
                    jsonResponse = SvcClient.CallServiceBatch(batchActionList, guardServiceProvider);

                    // flag that determines how to respond after processing the response received
                    bool success = true;

                    // deserialize the results into a BatchResponseList
                    batchResponseList = JsonConvert.DeserializeObject<BatchResponseList>(jsonResponse);

                    // deserialize each response returned by the service, and copy the OperationStatus to the view model for demo purposes
                    OperationStatus operationStatus = new OperationStatus();
                    foreach (var batchResponse in batchResponseList.BatchResponses)
                    {
                        // this flag tested later to determine whether to proceed to the next view or not
                        if (!batchResponse.RequestSuccessful) { success = false; }
                    }

                    #endregion

                    #region Comment : Here based on request successful status take further actions

                    // TODO: Do something with the response
                    if (!success)
                    {
                        #region Comment : Here Delete all files if data has not been uploaded to Guard

                        string folderDirectoryName = Path.Combine(ConfigCommonKeyReader.UploadFiles, wcQuoteId.ToString());

                        //Comment : Here if directory exist then only do the following
                        if (Directory.Exists(folderDirectoryName))
                        {
                            Directory.Delete(folderDirectoryName, true);
                        }

                        #endregion

                        #region Comment : Here set error message from API response in case of any non-system error type

                        if (batchResponseList != null &&
                            !batchResponseList.BatchResponses.Any(res => JsonConvert.DeserializeObject<OperationStatus>(res.JsonResponse).Messages.Any(msg => msg.MessageType == MessageType.SystemError)))
                        {
                            batchResponseList.BatchResponses.ForEach(res => JsonConvert.DeserializeObject<OperationStatus>(res.JsonResponse).Messages.ForEach(msg => listOfErrors.Add(msg.Text)));
                        }

                        #endregion

                        return Json(new { resultStatus = "NOK", resultText = "Unable to post referral quote details", resultMessages = listOfErrors }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        #region Comment : Here ReferralQuote interface refernce to do/make process all business logic

                        IReferralQuote referralQuoteBLL = new ReferralQuote(GetCustomSession(), guardServiceProvider);

                        #endregion

                        #region Mail Sending Code

                        // ********** SOFT REFERAL -no MGA code created due to question/decision engine results; send email to GUARD
                        if (appSession != null)
                        {
                            var questionnaireVM = appSession.QuestionnaireVM;
                            if (questionnaireVM != null)
                            {
                                var quoteStatus = questionnaireVM.QuoteStatus;
                                if (!string.IsNullOrEmpty(quoteStatus))
                                {
                                    if (quoteStatus.Equals(QuoteSoftReferral))
                                    {
                                        #region Comment : Here prepare/render referralQuote class-information html

                                        var classInformationHtml = ConvertRazorViewToString("_GetReferralQuoteClassInformation", appSession);

                                        #endregion

                                        #region Comment : Here prepare/render referralQuote class-information html

                                        var questionsAndResponsesHtml = string.Empty;

                                        //Comment : Here only one case (Other class description) which will bypass Questionnaire page before navigating to Referral screen
                                        //Added new condition on 18.03.2016 to skip Question rendering process (It will be excluded)
                                        //if (!questionnaireVM.ReferralScenarioIds.Contains(1) && !questionnaireVM.ReferralScenarioIds.Contains(4))
                                        //Also added Questions "Yellow Highlighting" in case "QuestionHistory" for running QuoteId have referral because of -ve Q response
                                        if (!(questionnaireVM.ReferralScenarioIds.Intersect(new List<int>() { 1, 4, 10, 11 }).Any()))
                                        {
                                            //Comment : Here get questions
                                            var questionResponse = appSession.QuestionnaireVM.QuestionsResponse;
                                            var questionsHistory = new List<Question>();

                                            //Comment : Here don not unnecessarily call below method call
                                            if (questionResponse.Questions.Count > 0)
                                            {
                                                #region Comment : Here added new Item in tuple for "Yellow Highlighting"

                                                var referralHisotry = questionnaireVM.ReferralHistory;
                                                int noOfReferralScenarios = referralHisotry.ReferralScenarioIdsList.Count;

                                                //Comment : Here if History have "MULTIPLE" referral reason in list then
                                                if (referralHisotry != null &&
                                                    referralHisotry.ReferralScenarioIdsList.Any() && noOfReferralScenarios > 1)
                                                {
                                                    //Comment : Here call API and keep data to pass RazorView
                                                    IQuestionnaire questionnaireBLL = new Questionnaire(GetCustomSession(), guardServiceProvider);

                                                    //Comment : Here first try to figure-out that "Is that first reason was due to DECLINE in referral history"
                                                    var firstReferralDeclineIds = referralHisotry.ReferralScenarioIdsList.First();
                                                    var recentReferralDeclineIds = referralHisotry.ReferralScenarioIdsList.Last();

                                                    //get "Second-Last" item
                                                    var oneStepBackToRecentReferralDeclineIds = noOfReferralScenarios > 2 ? //by-default first history item
                                                        referralHisotry.ReferralScenarioIdsList[noOfReferralScenarios - 2]  : firstReferralDeclineIds;

                                                    //Comment : Here get questions
                                                    var questionHistoryResponse = questionnaireBLL.GetQuestionsHistory(
                                                        new QuestionsHistoryRequestParms
                                                        {
                                                            QuoteId = wcQuoteId,
                                                            SuppressNonCurrent = false,
                                                            //Comment : Here set this FLAG based on recent QuoteStatus "Refer/Decline"
                                                            HistoryType = 
                                                            //Comment : Here in this case must check one history back also for verify was not "DECLINE"
                                                            recentReferralDeclineIds.Contains(8) 
                                                            & !oneStepBackToRecentReferralDeclineIds.Intersect(new List<int> { 12, 13 }).Any() ? 
                                                            HistoryType.MostRecentReferral :
                                                            (
                                                                (
                                                                    recentReferralDeclineIds.Intersect(new List<int> { 12, 13 }).Any()
                                                                    //|| firstReferralDeclineIds.Intersect(new List<int> { 12, 13 }).Any()
                                                                ) ? HistoryType.MostRecentDecline : HistoryType.MostRecent
                                                            )
                                                        });

                                                    questionsHistory = questionHistoryResponse != null ? questionHistoryResponse.Questions : null;
                                                }

                                                #endregion

                                                //Comment : Here prepare tuple VM to pass single Model to render view
                                                var tuple = new Tuple<CustomSession, QuestionsResponse, List<Question>>(appSession, questionResponse, questionsHistory);

                                                questionsAndResponsesHtml = ConvertRazorViewToString("_GetReferralQuoteQuestionsAndResponses", tuple);
                                            }
                                        }

                                        #endregion

                                        #region Comment : Here read XML file and get all ReferralReasons

                                        //Comment : Here first read xml file using XmlReader helper class
                                        XmlFileReader xmlReader = new XmlFileReader();
                                        var xmlData = xmlReader.GetXmlDocumentString(Server.MapPath("~/ReferralReasons.xml"));

                                        //Comment : Here after getting XML string of ReferralReasons deserealize into refernce object for futher processing
                                        var referralReasons = XmlParser.ToObject<ReferralReasons>(xmlData);

                                        if (referralReasons.IsNull())
                                        {
                                            loggingService.Trace("Unable to read xml & get ReferralReasons to preocess Soft Referral");
                                        }

                                        #endregion

                                        #region Comment : Here prepare referral mail MODEL and and then process Referral template with Model value and send mail

                                        var model = new ReferralQuoteMailViewModel
                                        {
                                            QuoteId = wcQuoteId > 0 ? wcQuoteId.ToString() : string.Empty,
                                            ContactName = referralQuoteModel.ContactName,
                                            BusinessName = referralQuoteModel.BusinessName,
                                            PhoneNumber = referralQuoteModel.PhoneNumber,
                                            Email = referralQuoteModel.Email,
                                            QuoteReferralMessage = questionnaireVM.QuoteReferralMessage,
                                            EstimatedTotalPremium =
                                            questionnaireVM.PremiumAmt > 0 ? string.Format("{0}{1}", "$", questionnaireVM.PremiumAmt.ToString("#,##0.00")) : string.Empty,
                                            AbsoulteURL = CDN.GetEmailImageUrl(),
                                            //Comment : Here Include Class information
                                            ClassInformationHtml = !string.IsNullOrEmpty(classInformationHtml.Trim()) ? classInformationHtml : string.Empty,

                                            //Comment : Here STEP-5. Include QuestionAndResponse information
                                            QuestionsAndResponsesHtml = !string.IsNullOrEmpty(questionsAndResponsesHtml.Trim()) ? questionsAndResponsesHtml : string.Empty
                                        };

                                        #region Comment : Here supply all collected model & objetcs and then call MailTemplatyeBuilder

                                        referralQuoteBLL.ProcessSoftReferralMailNew(model, listOfUploadedFiles);
                                        //referralQuoteBLL.ProcessSoftReferralMail(model, referralReasons, listOfUploadedFiles); //Old Method

                                        #endregion

                                        #region Comment : Here accordingly latest business requirement Clear/Expire all application session like "user, CustomSession"

                                        //Forcefully expire current active user session in application and redirect user to Home screen and 
                                        RemoveAllAppSessionAndCookie();

                                        #endregion

                                        #endregion
                                    }
                                    // ********** HARD REFERRAL - create a presubmission, send email to GUARD
                                    else if (quoteStatus.Equals(QuoteHardReferral))
                                    {
                                        //ProcessHardReferral(wcQuoteViewModel);
                                    }
                                }
                                else
                                {
                                    throw new ApplicationException("ReferralQuotePage : Invalid referral quote request !");
                                }
                            }
                        }

                        #endregion

                        return Json(new { resultStatus = "OK", resultText = "Quote referral details saved successfully" }, JsonRequestBehavior.AllowGet);
                    }

                    #endregion

                    #endregion
                }
            }

            return Json(new { resultStatus = "NOK", resultText = "Something went wrong while posting referral quote data !" }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region Methods : Private Methods

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
                throw new ApplicationException(string.Format("{0} : {1} !", ReferralQuotePage, Constants.QuoteIdCookieEmpty));
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
                    throw new ApplicationException(string.Format("{0},{1} : {2} !", ReferralQuotePage, Constants.SessionExpiredPartial, Constants.AppCustomSessionExpired));
                }
            }
        }

        /// <summary>
        /// Return status whether this page resuest is valid or un-authorized
        /// </summary>
        /// <returns></returns>
        private bool IsValidPageRequest()
        {
            var quoteVM = appSession.QuoteVM;
            var questionnaireVM = appSession.QuestionnaireVM;

            //Comment : Here check is this/current quote have associated quesionnaire QuoteStatus as "Soft Referral" otherwise invalid ReferralQuote page request
            var quoteStatus = string.Empty;
            if (questionnaireVM != null)
            {
                quoteStatus = questionnaireVM.QuoteStatus;
            }
            if (quoteVM == null || (questionnaireVM == null || (string.IsNullOrEmpty(quoteStatus) || !quoteStatus.Equals(QuoteSoftReferral))))
            {
                throw new ApplicationException(string.Format("{0},{1} : Invalid referral quote page request !", ReferralQuotePage, Constants.UnauthorizedRequest));
            }

            return true;
        }

        /// <summary>
        /// Return status whether this page resuest is valid or un-authorized
        /// </summary>
        /// <returns></returns>
        private bool IsValidPageRequestDecline()
        {
            var quoteVM = appSession.QuoteVM;
            var questionnaireVM = appSession.QuestionnaireVM;

            //Comment : Here check is this/current quote have associated quesionnaire QuoteStatus as "Hard Referral/Declined Quote" otherwise invalid DeclinedQuote page request
            var quoteStatus = string.Empty;
            if (questionnaireVM != null)
            {
                quoteStatus = questionnaireVM.QuoteStatus;
            }
            if (quoteVM == null || (questionnaireVM == null || (string.IsNullOrEmpty(quoteStatus) || !quoteStatus.Equals(QuoteHardReferral))))
            {
                throw new ApplicationException(string.Format("{0},{1} : Invalid declined quote page request !", ReferralQuotePage, Constants.UnauthorizedRequest));
            }

            return true;
        }

        /// <summary>
        /// It will validate referral quote model
        /// </summary>
        /// <param name="referralQuoteModel"></param>
        /// <returns>return list or errors if error found,empty list otherwise</returns>
        private List<string> ValidateReferralQuote(ReferralQuoteViewModel referralQuoteModel)
        {
            var listOfErrors = new List<string>();

            //validate contact name
            if (string.IsNullOrEmpty(referralQuoteModel.ContactName))
            {
                listOfErrors.Add(Constants.EmptyContactName);
            }

            //validate business name
            if (string.IsNullOrEmpty(referralQuoteModel.BusinessName))
            {
                listOfErrors.Add(Constants.EmptyBusinessName);
            }

            //validate phone number
            if (string.IsNullOrEmpty(referralQuoteModel.PhoneNumber))
            {
                listOfErrors.Add(Constants.EmptyPhoneNumber);
            }
            else if (!UtilityFunctions.IsValidRegex(referralQuoteModel.PhoneNumber, Constants.PhoneRegex))
            {
                listOfErrors.Add(Constants.InvalidPhoneNumber);
            }

            //validate email number
            if (string.IsNullOrEmpty(referralQuoteModel.Email))
            {
                listOfErrors.Add(Constants.EmptyEmail);
            }
            else if (!UtilityFunctions.IsValidRegex(referralQuoteModel.Email, Constants.EmailRegex))
            {
                listOfErrors.Add(Constants.InvalidEmail);
            }

            return listOfErrors;
        }

        /// <summary>
        /// Validate content uploded in ZIP format
        /// </summary>
        /// <param name="inputFileStream"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private FileUploadValidationStaus IsZipContentValid(Stream inputFileStream, string filePath)
        {
            FileUploadValidationStaus fileUploadValidationStaus = new FileUploadValidationStaus();
            using (ZipArchive archive = new ZipArchive(inputFileStream))
            {
                List<ZipArchiveEntry> allEntries = archive.Entries.ToList();
                foreach (ZipArchiveEntry entry in allEntries)
                {
                    if (entry.FullName.Contains(".zip"))
                    {
                        var stream = entry.Open();
                        fileUploadValidationStaus = IsZipContentValid(stream, filePath);
                        if (!fileUploadValidationStaus.IsValid)
                        {
                            return fileUploadValidationStaus;
                        }
                    }

                    if (entry.FullName.Contains('.'))
                    {
                        using (MemoryStream fileMemoryStream = new MemoryStream())
                        {
                            entry.Open().CopyTo(fileMemoryStream);
                            fileUploadValidationStaus = UploadHelper.ValidateUploadedFiles(fileMemoryStream, filePath, entry.FullName);
                            if (!fileUploadValidationStaus.IsValid)
                            {
                                return fileUploadValidationStaus;
                            }
                        }
                    }
                }
            }
            fileUploadValidationStaus.IsValid = true;
            return fileUploadValidationStaus;
        }

        /// <summary>
        /// Method will return partial view in string Format
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public string ConvertRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);

                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);

                return sw.GetStringBuilder().ToString();
            }
        }

        /// <summary>
        /// Fill Referral model and send as per the project flow
        /// </summary>
        /// <returns></returns>
        public BusinessInfoViewModel GetReferralModel()
        {
            BusinessInfoViewModel returnModel = appSession.BusinessInfoVM;
            if (!appSession.PurchaseVM.IsNull())
            {
                if (!appSession.PurchaseVM.Account.IsNull() && !string.IsNullOrWhiteSpace(appSession.PurchaseVM.Account.Email))
                {
                    returnModel.Email = appSession.PurchaseVM.Account.Email;
                }
                if (!appSession.PurchaseVM.PersonalContact.IsNull())
                {
                    if (!string.IsNullOrWhiteSpace(appSession.PurchaseVM.PersonalContact.PhoneNumber))
                    {
                        returnModel.PhoneNumber = appSession.PurchaseVM.PersonalContact.PhoneNumber;
                    }
                    if (!string.IsNullOrWhiteSpace(appSession.PurchaseVM.PersonalContact.FirstName) && !string.IsNullOrWhiteSpace(appSession.PurchaseVM.PersonalContact.LastName))
                    {
                        returnModel.ContactName = string.Format("{0} {1}", appSession.PurchaseVM.PersonalContact.FirstName, appSession.PurchaseVM.PersonalContact.LastName);
                    }
                }
                if (!appSession.PurchaseVM.BusinessInfo.IsNull() && !string.IsNullOrWhiteSpace(appSession.PurchaseVM.BusinessInfo.BusinessType))
                {
                    if (appSession.PurchaseVM.BusinessInfo.BusinessType != "I" && !string.IsNullOrWhiteSpace(appSession.PurchaseVM.BusinessInfo.BusinessName))
                    {
                        returnModel.CompanyName = appSession.PurchaseVM.BusinessInfo.BusinessName;
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(appSession.PurchaseVM.BusinessInfo.FirstName) && string.IsNullOrWhiteSpace(appSession.PurchaseVM.BusinessInfo.LastName))
                        {
                            returnModel.CompanyName = string.Format("{0} {1}", appSession.PurchaseVM.BusinessInfo.FirstName, appSession.PurchaseVM.BusinessInfo.LastName);
                        }
                    }
                }
            }
            return returnModel;
        }

        #endregion
    }
}

