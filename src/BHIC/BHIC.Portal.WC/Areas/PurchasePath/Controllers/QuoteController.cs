using BHIC.Common;
using BHIC.Common.Client;
using BHIC.Common.Config;
using BHIC.Contract.Background;
using BHIC.Contract.Policy;
using BHIC.Contract.PurchasePath;
using BHIC.Core;
using BHIC.Core.Masters;
using BHIC.Core.Policy;
using BHIC.Core.PurchasePath;
using BHIC.DML.WC.DataContract;
using BHIC.Domain.Background;
using BHIC.Domain.Policy;
using BHIC.Domain.Service;
using BHIC.Portal.WC.App_Start;
using BHIC.Common.Quote;
using BHIC.ViewDomain;
using BHIC.ViewDomain.Landing;
using BHIC.ViewDomain.QuestionEngine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Text;
using BHIC.Common.CommonUtilities;

namespace BHIC.Portal.WC.Areas.PurchasePath.Controllers
{
    //[CustomTransactionLogFilterAttribute]
    //[ValidateSession]
    public class QuoteController : BaseController
    {
        #region Private Variables


        public QuoteController()
        {

        }

        #endregion Private Variables End

        #region Public Methods

        /// <summary>
        /// Returns Purchase Path base page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //Comment : Progress bar navigation flag handling
            if (IsCustomSessionNull())
            {
                SetCustomSession(new BHIC.ViewDomain.CustomSession());
            }

            CustomSession customSession = GetCustomSession();
            if (customSession.PageFlag == 0)
            {
                customSession.PageFlag = 1;
                SetCustomSession(customSession);
            }
            return View();
        }

        /// <summary>
        /// Returns Default View of Quote Capture Page
        /// </summary>
        /// <returns></returns>
        public ActionResult CaptureQuote()
        {
            return PartialView("_GetQuote");
        }

        /// <summary>
        /// Get Quote View Model
        /// </summary>
        /// <returns></returns>
        public JsonResult GetQuoteViewModel(string quoteId)
        {
            DateTime start = DateTime.Now; ;
            HttpContextBase context = this.ControllerContext.HttpContext;
            QuoteViewModel quoteVM = new QuoteViewModel();
            ICaptureQuote captureQuote = new CaptureQuote();
            CustomSession customSession = GetCustomSession();
            List<Exposure> expList = null;
            PolicyData policyData = null;
            int intQuoteId = 0;

            //Comment : Progress bar nagivation binding based on flag 
            List<NavigationModel> links = new List<NavigationModel>();
            NavigationController nc = new NavigationController();
            links = nc.GetProgressBarLinks(customSession.PageFlag);

            if (!String.IsNullOrWhiteSpace(quoteId))
            {
                //get quote-id in int format for future references
                ICommonFunctionality commonFunctionality = GetCommonFunctionalityProvider();
                var appSession = RetrieveSavedQuote(quoteId, commonFunctionality, out intQuoteId);
                quoteVM.QuoteId = intQuoteId.ToString();

                //Comment : Here if able to retrieve saved session from DB layer then do following
                if (!appSession.IsNull() && intQuoteId != customSession.QuoteID)
                {
                    //Update the custom session with retrieved Session
                    SetCustomSession(appSession);
                    quoteVM = appSession.QuoteVM;

                    //Update Quote Id in cookie
                    QuoteCookieHelper.Cookie_SaveQuoteId(context, intQuoteId);
                }
                else
                {
                    quoteVM = customSession.QuoteVM;
                }

                if (UtilityFunctions.IsValidInt(intQuoteId) && appSession.QuoteID != customSession.QuoteID)
                {
                    captureQuote.GetQuoteDetails(intQuoteId, ref expList, ref policyData);
                    quoteVM.Exposures = expList;

                    if (!policyData.IsNull())
                        quoteVM.PolicyData = policyData;
                    if (!quoteVM.Exposures.IsNull() && quoteVM.Exposures.Count > 0)
                    {
                        customSession = GetCustomSession();
                        customSession.ZipCode = quoteVM.Exposures[0].ZipCode;
                        customSession.StateAbbr = quoteVM.Exposures[0].StateAbbr;
                        if (quoteVM.County.IsNull())
                        {
                            quoteVM.County = new County();
                            quoteVM.County.ZipCode = quoteVM.Exposures[0].ZipCode;
                            quoteVM.County.State = quoteVM.Exposures[0].StateAbbr;
                        }
                        SetCustomSession(customSession);
                        if (UtilityFunctions.IsValidInt(quoteVM.Exposures[0].ClassDescriptionId))
                        {
                            quoteVM.BusinessName = appSession.QuoteVM.BusinessName;
                        }
                    }
                }

                quoteVM.NavigationLinks = links;
            }
            else if (!IsCustomSessionNull())
            {
                //Comment : Progress bar nagivation binding based on flag 
                links = nc.GetProgressBarLinks(customSession.PageFlag);
                customSession = GetCustomSession();

                /*Create new quote id if the zip code and state is changed or first time comes to exposure page */
                if (customSession.QuoteVM.IsNull() || customSession.QuoteVM.County.IsNull() ||
                    (customSession.ZipCode != customSession.QuoteVM.County.ZipCode && customSession.StateAbbr != customSession.QuoteVM.County.State))
                {
                    quoteVM.County = captureQuote.GetCountyData(customSession.ZipCode.Trim(), customSession.StateAbbr.Trim());
                    SetCountyAndQuoteVmInCustomSession(ref customSession, quoteVM);
                }

                quoteVM = customSession.QuoteVM;
                intQuoteId = QuoteCookieHelper.Cookie_GetQuoteId(this.ControllerContext.HttpContext);
                quoteVM.QuoteId = intQuoteId.ToString();

                /*If quote id is valid then fetch the exposure list and policy data*/
                if (UtilityFunctions.IsValidInt(intQuoteId))
                {
                    if (!quoteVM.Exposures.IsNull() && quoteVM.Exposures.Count > 0)
                    {
                        quoteVM.BusinessName = customSession.QuoteVM.BusinessName;
                    }
                }

                //if (!quoteVM.County.IsNull())
                //{
                //    //if (customSession.QuoteVM.IsNull())
                //    //{
                //    //    customSession.QuoteVM = new QuoteViewModel();
                //    //}
                //    intQuoteId = QuoteCookieHelper.Cookie_GetQuoteId(this.ControllerContext.HttpContext);
                //    if (!customSession.QuoteVM.County.IsNull())
                //    {
                //        if (customSession.QuoteVM.County.State != quoteVM.County.State)
                //        {
                //            SetCountyAndQuoteVmInCustomSession(ref customSession, quoteVM);
                //        }
                //    }
                //    else if (!UtilityFunctions.IsValidInt(intQuoteId))
                //    {
                //        SetCountyAndQuoteVmInCustomSession(ref customSession, quoteVM);
                //    }
                //    customSession.QuoteVM.County = quoteVM.County;
                //}
                quoteVM.NavigationLinks = links;
                SetCustomSession(customSession);
            }
            loggingService.Trace(String.Format("Time taken in QuoteViewModel method {0} ms", (DateTime.Now - start).TotalMilliseconds));
            return Json(quoteVM, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Quote if exists, otherwise create new Quote
        /// </summary>
        /// <param name="zipCode">zip code</param>
        /// <param name="gclid">Google campaign id</param>
        /// <returns>Return Quote view model with populated fields</returns>
        private int GenerateQuoteId(string zipCode, string gclid = "")
        {
            #region Variable initialization

            string adId = string.Empty;
            string userIP = string.Empty;
            HttpContextBase context = this.ControllerContext.HttpContext;
            IQuoteStatusService quoteStatusService = new QuoteStatusService();

            #endregion

            #region Get Quote

            //get current quote if exists, otherwise create new one
            //as we are creating quoteId so there is no use extracting it from cookies again 
            int wcQuoteId = 0;// QuoteCookieHelper.Cookie_GetQuoteId(context);

            #endregion

            #region Create Quote

            #region code not being used with google adwords , has to be removed after successful incorporation of google adwords
            //if (!string.IsNullOrWhiteSpace(gclid))
            //{
            //    adId = gclid.Trim();
            //}
            //else if (context.Request.UrlReferrer != null)
            //{
            //    try
            //    {
            //        var q = HttpUtility.ParseQueryString(context.Request.UrlReferrer.Query);
            //        adId = q["gclid"];
            //    }
            //    catch
            //    {
            //        // swallow the exception; parameter not available; means the request is not associated with google adwords
            //    }
            //}
            # endregion

            //Get user ip address
            //userIP = GetUser_IP(context); //Old line
            userIP = UtilityFunctions.GetUserIPAddress(context.ApplicationInstance.Context);

            ServiceProvider provider = new GuardServiceProvider() { ServiceCategory = LineOfBusiness };
            var postOperationStatus = quoteStatusService.AddQuoteStatus(new QuoteStatus { AdId = null, EnteredOn = DateTime.Now, UserIP = userIP }, provider);

            //if request is successful, get new QuoteId
            if (postOperationStatus.RequestSuccessful)
            {
                //Get QuoteId from returned effected id
                var effectedQuoteDTO = postOperationStatus.AffectedIds
                    .SingleOrDefault(res => res.DTOProperty == "QuoteId");

                //Get retuned effected id/QuoteId
                wcQuoteId = Convert.ToInt32(effectedQuoteDTO.IdValue);

            }

            #endregion

            #region Save/Update QuoteId

            QuoteCookieHelper.Cookie_SaveQuoteId(context, wcQuoteId);

            #endregion

            return wcQuoteId;
        }

        /// <summary>
        /// This method searches business on the search string and classDescriptionKeywordId. Each Search string is stored in CYBClassKeywords Table.
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="classDescriptionId"></param>
        /// <returns></returns>
        [ValidateSession]
        public JsonResult SearchBusiness(string searchString, int? classDescriptionId)
        {
            List<ClassDescriptionKeyword> businessSearchResultList = new List<ClassDescriptionKeyword>();

            try
            {
                if (!IsCustomSessionNull())
                {
                    if (!string.IsNullOrWhiteSpace(searchString))
                    {
                        CustomSession appSession = GetCustomSession();
                        if (appSession.QuoteVM.IsNull())
                        {
                            appSession.QuoteVM = new QuoteViewModel();
                        }
                        appSession.QuoteVM.IndustryId = null;
                        appSession.QuoteVM.SubIndustryId = null;
                        SetCustomSession(appSession);
                        ICaptureQuote captureQuote = new CaptureQuote();

                        captureQuote.AddClassKeyword(searchString.Trim());
                        businessSearchResultList = captureQuote.GetBusinessNamesList(searchString, classDescriptionId, appSession.ZipCode, appSession.StateAbbr);
                    }
                    else
                    {
                        loggingService.Trace("SearchBusiness called with Null/Empty searchString.");
                    }
                }
                else
                {
                    loggingService.Trace("SearchBusiness called with session expired");
                }
            }
            catch (Exception)
            {
                loggingService.Trace(string.Format("Invalid search string entered {0}", searchString));
            }

            //var jsonResponse = "[{'ClassDescKeywordId':63,'Keyword':'Sprinkler Installation (Inside Buildings)','ClassDescriptionId':64,'ClassCode':'5188','ClassSuffix':'00','DirectOK':'Y'},{'ClassDescKeywordId':113,'Keyword':'Building Material Dealer','ClassDescriptionId':114,'ClassCode':'8232','ClassSuffix':'01','DirectOK':'N'},{'ClassDescKeywordId':142,'Keyword':'Property Mgmt - Buildings','ClassDescriptionId':144,'ClassCode':'9015','ClassSuffix':'09','DirectOK':'Y'},{'ClassDescKeywordId':348,'Keyword':'Builder (General Contractor)','ClassDescriptionId':1266,'ClassCode':'5403','ClassSuffix':'00','DirectOK':'N'},{'ClassDescKeywordId':349,'Keyword':'Builder (Residential Carpentry)','ClassDescriptionId':12,'ClassCode':'5645','ClassSuffix':'00','DirectOK':'Y'},{'ClassDescKeywordId':442,'Keyword':'Building Operations (Commercial/Dwellings)','ClassDescriptionId':144,'ClassCode':'9015','ClassSuffix':'09','DirectOK':'Y'},{'ClassDescKeywordId':443,'Keyword':'Building Operations (Condos and Apartments)','ClassDescriptionId':145,'ClassCode':'9015','ClassSuffix':'09','DirectOK':'Y'},{'ClassDescKeywordId':444,'Keyword':'Building Operations (Mobile Home or Trailer Park)','ClassDescriptionId':146,'ClassCode':'9015','ClassSuffix':'09','DirectOK':'N'}]";
            //List<ClassDescriptionKeyword> businessSearchResultList = JsonConvert.DeserializeObject<List<ClassDescriptionKeyword>>(jsonResponse);

            return Json(businessSearchResultList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This method gets all the Industries 
        /// </summary>
        /// <returns></returns>
        //[OutputCache(Duration = 3600, VaryByParam = "none")]
        public JsonResult GetIndustries()
        {
            ServiceProvider provider = new GuardServiceProvider() { ServiceCategory = LineOfBusiness };
            IIndustryService industryService = new IndustryService();
            var industryList = industryService.GetIndustryList(new IndustryRequestParms { Lob = Constants.GetLineOfBusiness(Constants.LineOfBusiness.WC) }, provider);
            if (!industryList.IsNull() && industryList.Count > 0)
            {
                industryList.Add(new Industry { Description = Constants.OtherDescription, IndustryId = Constants.OtherDescriptionId });
            }
            return Json(industryList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This method gets all the Sub-Industries based on Industry Id 
        /// </summary>
        /// <param name="industryId"></param>
        /// <returns></returns>
        public JsonResult GetSubIndustries(int industryId)
        {
            CustomSession appSession = GetCustomSessionWithQuoteVM();
            appSession.QuoteVM.IndustryId = industryId;
            appSession.QuoteVM.ClassDescriptionKeywordId = null;
            SetCustomSession(appSession);
            ServiceProvider provider = new GuardServiceProvider() { ServiceCategory = LineOfBusiness };
            ISubIndustryService subIndustryService = new SubIndustryService();
            var subIndustryList = subIndustryService.GetSubIndustryList(new SubIndustryRequestParms { IndustryId = industryId }, provider);
            if (!subIndustryList.IsNull() && subIndustryList.Count > 0)
            {
                subIndustryList.Add(new SubIndustry { SubIndustryId = Constants.OtherDescriptionId, Description = Constants.OtherDescription });
            }
            return Json(subIndustryList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This method gets all the Classes(business) based on subIndustryId
        /// </summary>
        /// <param name="subIndustryId"></param>
        /// <returns></returns>
        public JsonResult GetClassDescriptions(int subIndustryId)
        {
            CustomSession appSession = GetCustomSession();
            appSession.QuoteVM.SubIndustryId = subIndustryId;
            appSession.QuoteVM.ClassDescriptionKeywordId = null;
            SetCustomSession(appSession);
            ServiceProvider provider = new GuardServiceProvider() { ServiceCategory = LineOfBusiness };
            IClassDescriptionService classDescriptionService = new ClassDescriptionService();
            var classDescriptionList = classDescriptionService.GetClassDescriptionList(new ClassDescriptionRequestParms { IncludeRelated = true, SubIndustryId = subIndustryId, State = appSession.StateAbbr, ZipCode = appSession.ZipCode, Lob = Constants.GetLineOfBusiness(Constants.LineOfBusiness.WC) }, provider);
            if (!classDescriptionList.IsNull() && classDescriptionList.Count > 0)
            {
                classDescriptionList.Add(new ClassDescription { Description = Constants.OtherDescription, ClassDescriptionId = Constants.OtherDescriptionId, ClassCode = null, DirectOK = null });
            }
            return Json(classDescriptionList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Validates the minimum exposure amount
        /// </summary>
        /// <param name="exposureAmt"></param>
        /// <param name="classDescriptionId"></param>
        /// <param name="classDescKeywordId"></param>
        /// <returns></returns>
        [ValidateSession]
        public JsonResult ValidateExposureAmount(string exposureAmt, int? classDescriptionId, int? classDescKeywordId, string classCode, string directSalesOK, int? industryId, int? subIndustryId)
        {
            CustomSession appSession = GetCustomSessionWithQuoteVM();
            VExposuresMinPayrollResponse exposureAmountResponse;
            string resultStatus = string.Empty;
            string resultText = string.Empty;
            string resultMinAmount = string.Empty;
            ServiceProvider provider = new GuardServiceProvider() { ServiceCategory = LineOfBusiness };

            #region Set Values in session

            appSession.QuoteVM.ClassDescriptionId = classDescriptionId.Value;
            appSession.QuoteVM.ClassDescriptionKeywordId = classDescKeywordId.HasValue ? classDescKeywordId : null;
            appSession.QuoteVM.AnnualPayroll = Convert.ToInt64(UtilityFunctions.ToNumeric(exposureAmt));
            appSession.QuoteVM.TotalPayroll = exposureAmt;
            appSession.QuoteVM.ClassCode = classCode;
            appSession.QuoteVM.BusinessClassDirectSales = directSalesOK;
            appSession.QuoteVM.IndustryId = industryId;
            appSession.QuoteVM.SubIndustryId = subIndustryId;

            #endregion Set Values in session

            if (string.IsNullOrWhiteSpace(exposureAmt))
            {
                resultStatus = "NOK";
                resultText = "Please Provide Exposure Amount";
                resultMinAmount = string.Empty;
            }
            else if (classDescKeywordId.IsNull() && classDescriptionId.IsNull())
            {
                resultStatus = "NOK";
                resultText = "Please provide your Business/Trade information to which this payroll is applicable";
                resultMinAmount = string.Empty;
            }
            else if (classDescriptionId == -1 || string.IsNullOrWhiteSpace(classCode) || string.IsNullOrWhiteSpace(directSalesOK))
            {
                resultStatus = "OK";
                resultText = "";
                resultMinAmount = string.Empty;
                appSession.QuoteVM.MinExpValidationAmount = 0;
                appSession.QuoteVM.IsGoodStateApplicable = false;
            }
            else
            {
                try
                {
                    IExposureService exposureService = new ExposureService();
                    exposureAmountResponse = exposureService.ValidateMinimumExposureAmount(new VExposuresMinPayrollRequestParms { ExposureAmt = Convert.ToDecimal(exposureAmt), ClassDescKeywordId = classDescKeywordId, ClassDescriptionId = classDescriptionId }, provider);

                    //Comment : Here amount is not validated for supplied "ClassDescriptionId" then send "NOK" with 'MinimumExposure"
                    if (!(exposureAmountResponse.IsNull() && exposureAmountResponse.OperationStatus.IsNull()))
                    {
                        if (exposureAmountResponse.OperationStatus.RequestSuccessful)
                        {
                            resultStatus = "OK";
                            resultText = "";
                            resultMinAmount = Convert.ToString(exposureAmountResponse.MinimumExposure);
                            appSession.QuoteVM.MinExpValidationAmount = exposureAmountResponse.MinimumExposure;
                            appSession.QuoteVM.IsGoodStateApplicable = false;

                            //Comment : Here if Exposure has not been submitted and "Bad State" referral scenarios added and later VALID payroll entered then clear exiting
                            if (appSession.QuoteVM.Exposures.IsNull() && !appSession.QuestionnaireVM.IsNull())
                            {
                                //Comment : Here on page load reset "Referral Scenarios" list
                                appSession.QuestionnaireVM.QuoteReferralMessages.Clear();
                                appSession.QuestionnaireVM.ReferralScenarioIds.Clear();

                                //Comment : Here In case any ReferralScenarioId exists for this QUOTE then add this into Quote's "ReferralScenariosHistory"
                                appSession.QuestionnaireVM.ReferralScenariosHistory.Clear();
                            }

                        }
                        else if (!exposureAmountResponse.OperationStatus.RequestSuccessful)
                        {
                            resultStatus = "NOK";
                            resultText = exposureAmountResponse.OperationStatus.Messages[0].Text;
                            resultMinAmount = Convert.ToString(exposureAmountResponse.MinimumExposure);
                            appSession.QuoteVM.MinExpValidationAmount = Convert.ToInt64(exposureAmountResponse.MinimumExposure);
                            appSession.QuoteVM.IsGoodStateApplicable = true;
                            GetStateType();
                        }
                    }
                }
                catch (Exception ex)
                {
                    resultStatus = "NOK";
                    resultText = ex.Message;
                    // Changed minimum amount to $25000, as per Neelam with subject FW: Minimum Payroll Testing received on Wed 20-01-2016 20:35
                    resultMinAmount = "$25,000"; //"$15,000";
                }
            }
            SetCustomSession(appSession);
            return Json(new { resultStatus = resultStatus, resultText = resultText, resultMinAmount = resultMinAmount }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// SaveLandingPageData
        /// Takes as input Exposure Data from front End
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveLandingPageData(WcHomePageViewModel request)
        {

            ICaptureQuote captureQuote = new CaptureQuote();
            var listOfErrors = new List<string>();
            CustomSession customSession = GetCustomSession();

            //Comment : Progress bar navigation flag handling
            //Session["flag"] = 2;
            customSession.PageFlag = 2;

            string redirectionPath = string.Empty;
            var validRequest = captureQuote.ValidateExposureData(request, ref listOfErrors, customSession);
            if (validRequest)
            {
                int wcQuoteId = QuoteCookieHelper.Cookie_GetQuoteId(this.ControllerContext.HttpContext);
                string zipCode = string.Empty;

                var exposure = new Exposure();
                var policyData = new PolicyData();

                List<Exposure> lastSavedExposurelist = null;
                PolicyData lastSavedPolicyData = null;
                var batchActionList = new BatchActionList();	// BatchActionList: list of actions routed to the Insurance Service for response
                BatchResponseList batchResponseList = null;			// BatchResponseList: list of responses returned from the Insurance Service
                string jsonResponse;							// JSON-formated response data returned from the Insurance Service
                bool success = true;
                ServiceProvider provider = new GuardServiceProvider() { ServiceCategory = LineOfBusiness };
                OperationStatus operationStatus = new OperationStatus();

                if (request.IndustryId != -1 && request.SubIndustryId != -1 && request.ClassDescriptionId != -1 && UtilityFunctions.IsValidInt(request.ClassDescriptionId))
                {

                    #region Comment : Here Throw exception which will redirect user back to Error Page

                    //If there's no active quote (e.g. - if the session expired), redirect back to the landing page
                    if (!UtilityFunctions.IsValidInt(wcQuoteId))
                    {
                        return Json(new
                        {
                            response = false,
                            message = Constants.QuoteCaptureErrors.SESSION_EXPIRED
                        }, JsonRequestBehavior.AllowGet);
                    }

                    #endregion Comment : Here Throw exception which will redirect user back to Error Page

                    #region Comment : Here get user last exposure details if exists in Session to avoid Guard API call overhead otherwise call "Exposure" and re-assign session for later usage

                    zipCode = customSession.ZipCode;
                    if (!customSession.QuoteVM.IsNull() && !customSession.QuoteVM.Exposures.IsNull() && customSession.QuoteVM.Exposures.Count > 0
                        && !customSession.QuoteVM.PolicyData.IsNull() && UtilityFunctions.IsValidInt(customSession.QuoteVM.PolicyData.PolicyDataId))
                    {
                        GetExposures();
                    }

                    /*Set last saved Exposure and Policydata*/
                    GetLastExposureListAndPolicyData(ref lastSavedExposurelist, ref lastSavedPolicyData);

                    if (!lastSavedPolicyData.IsNull() && UtilityFunctions.IsValidInt(lastSavedPolicyData.PolicyDataId) && request.ClassDescriptionId != -1)
                    {
                        policyData.PolicyDataId = lastSavedPolicyData.PolicyDataId;
                    }
                    if (!lastSavedExposurelist.IsNull() && lastSavedExposurelist.Count() > 0)
                    {
                        exposure.ExposureId = lastSavedExposurelist.Where(x => x.CompanionClassId.IsNull()).First().ExposureId;
                    }

                    #endregion

                    #region Comment : Here create exposure object

                    if (UtilityFunctions.IsValidInt(request.ClassDescriptionKeywordId))
                    {
                        exposure.ClassDescriptionKeywordId = request.ClassDescriptionKeywordId;
                        exposure.ClassDescriptionId = request.ClassDescriptionId;
                    }
                    else
                    {
                        exposure.IndustryId = request.IndustryId;
                        exposure.SubIndustryId = request.SubIndustryId;

                        if (request.IndustryId == -1 || request.SubIndustryId == -1 || request.ClassDescriptionId == -1)
                            exposure.OtherClassDesc = request.OtherClassDesc;

                        if (!string.IsNullOrWhiteSpace(exposure.OtherClassDesc))
                        {
                            exposure.ClassDescriptionId = null;
                        }
                        else
                        {
                            exposure.ClassDescriptionId = request.ClassDescriptionId;
                        }
                    }

                    exposure.QuoteId = wcQuoteId;
                    exposure.LOB = Constants.GetLineOfBusiness(Constants.LineOfBusiness.WC);
                    exposure.ExposureAmt = Convert.ToDecimal(UtilityFunctions.ToNumeric(request.ExposureAmt));
                    exposure.ZipCode = zipCode;
                    exposure.StateAbbr = customSession.StateAbbr;

                    #endregion

                    #region Comment : Here create policy data object
                    if (string.IsNullOrWhiteSpace(exposure.OtherClassDesc))
                    {
                        policyData.QuoteId = wcQuoteId;
                        policyData.InceptionDate = DateTime.Parse(request.InceptionDate, new System.Globalization.CultureInfo("en-US", true));

                    }
                    policyData.YearsInBusiness = request.BusinessYears.Value;
                    #endregion

                    //Comment : Set coverage State Id from last submitted exposure of same state and zip code
                    if (!lastSavedExposurelist.IsNull() && lastSavedExposurelist.Count > 0 && GetLobDataAndCoverageStateIds().QuoteVM.CoverageStateIds.Count > 0)
                    {
                        exposure.CoverageStateId = GetLobDataAndCoverageStateIds().QuoteVM.CoverageStateIds[0];
                    }

                    #region Comment : Here Service call batch of actions creation

                    // populate a BatchAction that will be used to submit the Exposure DTO to the Insurance Service
                    var exposuresAction = new BatchAction { ServiceName = "Exposures", ServiceMethod = "POST", RequestIdentifier = "Exposure Data" };
                    exposuresAction.BatchActionParameters.Add(new BatchActionParameter { Name = "exposure", Value = JsonConvert.SerializeObject(exposure) });
                    batchActionList.BatchActions.Add(exposuresAction);

                    #region Remove all the companion Exposures whose Class code are not present in request
                    if (!lastSavedExposurelist.IsNull() && lastSavedExposurelist.Count > 0)
                    {
                        var companionExposuresList = lastSavedExposurelist.ExceptWhere(x => x.CompanionClassId.IsNull()).ToList();// captureQuote.GetCompanionClassExposureList(wcQuoteId);
                        if (!companionExposuresList.IsNull() && companionExposuresList.Count > 0)
                        {
                            companionExposuresList.ForEach(res => AddDeleteBatchAction(res, request.CompClassData, batchActionList));
                        }
                    }
                    #endregion Remove all the companion Exposures whose Class code are not present in request

                    #region Add Companion Class Data in request

                    if (!request.CompClassData.IsNull() && request.CompClassData.Count > 0 && exposure.ClassDescriptionId != null)
                    {
                        int compDataCount = 0;
                        foreach (var data in request.CompClassData)
                        {
                            compDataCount++;
                            Exposure exp = null;
                            if (!lastSavedExposurelist.IsNull() && lastSavedExposurelist.Count > 0 && lastSavedExposurelist.Any(x => x.ClassCode == data.ClassCode && x.CompanionClassId == data.CompanionClassId))
                            {
                                exp = lastSavedExposurelist.Find(x => x.ClassCode == data.ClassCode || x.CompanionClassId == data.CompanionClassId);
                            }
                            else
                            {
                                exp = new Exposure();
                            }
                            exp.CompanionClassId = data.CompanionClassId;
                            exp.ClassCode = data.ClassCode;
                            exp.QuoteId = wcQuoteId;
                            exp.LOB = Constants.GetLineOfBusiness(Constants.LineOfBusiness.WC);
                            exp.ExposureAmt = !String.IsNullOrWhiteSpace(data.PayrollAmount) ? Convert.ToDecimal(UtilityFunctions.ToNumeric(data.PayrollAmount)) : 0;
                            exp.ZipCode = zipCode;
                            exp.StateAbbr = customSession.StateAbbr;
                            var compData = new BatchAction { ServiceName = "Exposures", ServiceMethod = "POST", RequestIdentifier = String.Format("Companion Code Data {0}", compDataCount) };
                            compData.BatchActionParameters.Add(new BatchActionParameter { Name = "exposure", Value = JsonConvert.SerializeObject(exp) });
                            batchActionList.BatchActions.Add(compData);
                        }
                    }

                    #endregion

                    // populate a BatchAction that will be used to submit the PolicyData DTO to the Insurance Service
                    if (exposure.ClassDescriptionId != null)
                    {
                        var policyDataAction = new BatchAction { ServiceName = Constants.ServiceNames.PolicyData, ServiceMethod = Constants.MethodType.POST, RequestIdentifier = "Policy Data" };
                        policyDataAction.BatchActionParameters.Add(new BatchActionParameter { Name = "policyData", Value = JsonConvert.SerializeObject(policyData) });
                        batchActionList.BatchActions.Add(policyDataAction);
                    }
                    #endregion

                    #region Comment : Here Service call and response handling

                    // submit the BatchActionList to the Insurance Service
                    jsonResponse = SvcClient.CallServiceBatch(batchActionList, provider);

                    // flag that determines how to respond after processing the response received
                    success = true;

                    // deserialize the results into a BatchResponseList
                    batchResponseList = JsonConvert.DeserializeObject<BatchResponseList>(jsonResponse);

                    // deserialize each response returned by the service, and copy the OperationStatus to the view model for demo purposes
                    operationStatus = new OperationStatus();
                    foreach (var batchResponse in batchResponseList.BatchResponses)
                    {
                        // this flag tested later to determine whether to proceed to the next view or not
                        if (!batchResponse.RequestSuccessful) { success = false; }
                    }

                    #endregion
                }

                #region Comment : Here returning response to user

                // TODO: Do something with the response
                if (!success)
                {
                    #region Comment : Here set error message from API response in case of any non-system error type

                    if (batchResponseList != null &&
                        !batchResponseList.BatchResponses.Any(res => JsonConvert.DeserializeObject<OperationStatus>(res.JsonResponse).Messages.Any(msg => msg.MessageType == MessageType.SystemError)))
                    {
                        batchResponseList.BatchResponses.ForEach(res => JsonConvert.DeserializeObject<OperationStatus>(res.JsonResponse).Messages.ForEach(msg => listOfErrors.Add(msg.Text)));
                    }

                    #endregion

                    return Json(new
                    {
                        response = false,
                        message = Constants.QuoteCaptureErrors.EXPOSURE_SUBMISSION_FAILED,
                        errorList = listOfErrors
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    #region Comment : Here get ExposureId for newly created Exposure and store that into session object
                    //submitted = true;

                    if (batchResponseList != null)
                    {
                        //Comment : Here set exposureId into session

                        var exposureResponse = batchResponseList.BatchResponses.SingleOrDefault(m => m.RequestIdentifier == "Exposure Data").JsonResponse;
                        if (exposure.OtherClassDesc.IsNull() || !exposure.ClassDescriptionId.IsNull())
                        {
                            var policyDataResponse = batchResponseList.BatchResponses.SingleOrDefault(m => m.RequestIdentifier == "Policy Data").JsonResponse;
                            if (policyDataResponse != null)
                            {
                                var operationStatusDeserialized = JsonConvert.DeserializeObject<OperationStatus>(policyDataResponse);

                                if (!operationStatusDeserialized.IsNull())
                                {
                                    policyData.PolicyDataId = Convert.ToInt32(operationStatusDeserialized.AffectedIds.SingleOrDefault(m => m.DTOProperty == "PolicyDataId").IdValue);

                                }
                            }
                        }
                        #region Add Exposure Into Custom Session

                        if (exposureResponse != null)
                        {
                            var operationStatusDeserialized = JsonConvert.DeserializeObject<OperationStatus>(exposureResponse);
                            if (!operationStatusDeserialized.IsNull())
                            {
                                customSession = GetCustomSessionWithQuoteVMExposuresList();
                                exposure = new Exposure();
                                exposure.ExposureId = Convert.ToInt32(operationStatusDeserialized.AffectedIds.SingleOrDefault(m => m.DTOProperty == "ExposureId").IdValue);
                                exposure.ExposureAmt = Int64.Parse(UtilityFunctions.ToNumeric(request.ExposureAmt));
                                exposure.ClassDescriptionId = request.ClassDescriptionId;
                                exposure.ClassDescriptionKeywordId = request.ClassDescriptionKeywordId;
                                if (!lastSavedExposurelist.IsNull() && lastSavedExposurelist.Count > 0)
                                {
                                    exposure.CoverageStateId = lastSavedExposurelist[0].CoverageStateId;
                                }
                                else
                                {
                                    exposure.CoverageStateId = Convert.ToInt32(operationStatusDeserialized.AffectedIds.SingleOrDefault(m => m.DTOProperty == "CoverageStateId").IdValue);
                                }

                                //Commen t: Hre update exposure as well as county
                                if (customSession.QuoteVM.County.IsNull())
                                {
                                    customSession.QuoteVM.County = new County();
                                }
                                if (UtilityFunctions.IsValidInt(request.ClassDescriptionKeywordId))
                                    exposure.ClassDescriptionKeywordId = request.ClassDescriptionKeywordId;
                                else
                                {
                                    exposure.IndustryId = request.IndustryId;
                                    exposure.SubIndustryId = request.SubIndustryId;
                                    if (request.ClassDescriptionId == -1)
                                        exposure.OtherClassDesc = request.OtherClassDesc;
                                }
                                policyData.YearsInBusiness = request.BusinessYears.Value;
                                customSession = GetCustomSessionWithPolicyData();
                                customSession.QuoteVM.PolicyData = policyData;
                                exposure.StateAbbr = request.StateAbbr;
                                exposure.ZipCode = request.ZipCode;
                                exposure.QuoteId = wcQuoteId;
                                exposure.LOB = Constants.GetLineOfBusiness(Constants.LineOfBusiness.WC);
                                exposure.ClassCode = request.ClassCode;
                                customSession.QuoteVM.Exposures = new List<Exposure>();
                                customSession.QuoteVM.Exposures.Add(exposure);
                                SetCustomSession(customSession);
                            }

                        }

                        #endregion
                    }

                    #region Add All Other Data Object Into Custom Session Object

                    if (customSession.QuoteVM.IsNull())
                    {
                        customSession.QuoteVM = new QuoteViewModel();
                    }
                    if (customSession.QuoteVM.PolicyData.IsNull())
                    {
                        customSession.QuoteVM.PolicyData = new PolicyData();
                    }
                    customSession.QuoteVM.PolicyData.InceptionDate = DateTime.Parse(request.InceptionDate, new System.Globalization.CultureInfo("en-US", true));
                    customSession.QuoteVM.ClassCode = request.ClassCode;
                    customSession.QuoteVM.BusinessYears = request.BusinessYears.Value;
                    customSession.QuoteVM.IndustryId = request.IndustryId;
                    customSession.QuoteVM.SubIndustryId = request.SubIndustryId;

                    customSession.QuoteVM.AnnualPayroll = Int64.Parse(UtilityFunctions.ToNumeric(request.ExposureAmt));
                    customSession.QuoteVM.ClassDescriptionId = request.ClassDescriptionId ?? 0;
                    customSession.QuoteVM.ClassDescriptionKeywordId = request.ClassDescriptionKeywordId;
                    customSession.QuoteVM.IsMultiClassPrimaryExposureValid = true;
                    customSession.QuoteVM.IsGoodStateApplicable = request.IsGoodStateApplicable;
                    customSession.QuoteVM.IsMultiClassApplicable = request.IsMultiClassApplicable;
                    customSession.QuoteVM.MoreClassRequired = request.MoreClassRequired;
                    customSession.QuoteVM.IsMultiStateApplicable = request.IsMultiStateApplicable;
                    customSession.QuoteVM.TotalPayroll = request.TotalPayroll;
                    customSession.QuoteVM.CompClassData = request.CompClassData;
                    customSession.QuoteVM.IsMultiClass = request.IsMultiClass;
                    customSession.QuoteVM.MinExpValidationAmount = request.MinExpValidationAmount;
                    customSession.QuoteVM.BusinessClassDirectSales = request.BusinessClassDirectSales;
                    customSession.QuoteVM.BusinessName = request.BusinessName;
                    customSession.QuoteVM.OtherClassDesc = request.OtherClassDesc;
                    //Newly added by Prem on 29.03.2016 (to send information into session object)
                    customSession.QuoteVM.IndustryName = request.IndustryName;
                    customSession.QuoteVM.SubIndustryName = request.SubIndustryName;
                    customSession.QuoteVM.BusinessYearsText = request.BusinessYearsText;

                    #region Comment : Here primary class having companion classes then validate primary class payroll

                    if (request.IsMultiClassApplicable && !request.MoreClassRequired)
                    {
                        #region Comment : Here primary class MinimumPayrollThreshold validation logic

                        long totalPayroll = Convert.ToInt64(UtilityFunctions.ToNumeric(request.TotalPayroll));
                        long expAmount = Convert.ToInt64(UtilityFunctions.ToNumeric(request.ExposureAmt));
                        if (!(totalPayroll == expAmount))
                        {

                            BHIC.DML.WC.DTO.PrimaryClassCodeDTO primaryClassCodeData = captureQuote.GetMinimumPayrollThreshold(request.StateAbbr, request.ClassDescriptionId.Value);
                            if (!primaryClassCodeData.IsNull())
                            {
                                if (primaryClassCodeData.MinimumPayrollThreshold * totalPayroll > expAmount)
                                {
                                    customSession.QuoteVM.IsMultiClassPrimaryExposureValid = false;
                                }
                                else
                                {
                                    customSession.QuoteVM.IsMultiClassPrimaryExposureValid = true;
                                }
                            }
                        }

                        #endregion
                    }

                    #endregion

                    #region Comment : Here based on expousre/quote page inputs decide next page navigation path

                    //Comment : Here set default redirection path to "Question Page"
                    redirectionPath = "/GetQuestions";

                    #region Comment : All "Referral" scenarios condition require "QuestionnaireViewModel" initilization

                    List<List<int>> referralDeclineHistory = null;

                    //Comment : Here below are Referral conidtion
                    //1. Other chosen (indus,sub-indus,class) - "Soft Referral"
                    //2. Multi state choosen - "Soft Referral"
                    //3. Direct sales "N" exist for bussiness class - "Decline"

                    if (
                            !string.IsNullOrWhiteSpace(request.OtherClassDesc) ||
                            request.IsMultiStateApplicable ||
                            (request.BusinessClassDirectSales != null && request.BusinessClassDirectSales.Equals("N"))
                       )
                    {
                        #region Comment : Here "QuestionnaireViewModel" initilization

                        if (customSession.QuoteVM.IsNull())
                        {
                            customSession.QuoteVM = new QuoteViewModel();
                        }
                        //else if (!customSession.QuoteVM.Exposures.IsNull())
                        //{
                        //    customSession.QuoteVM.Exposures = null;
                        //}

                        #region Comment : Here To maintain user Referral/Decline history we need to get "All List of Previous Referrals Details"

                        if (!customSession.QuestionnaireVM.IsNull() && !customSession.QuestionnaireVM.ReferralScenariosHistory.IsNull())
                        {
                            referralDeclineHistory = customSession.QuestionnaireVM.ReferralScenariosHistory;
                        }

                        #endregion

                        //Instantiate QuestionnaireVM object
                        customSession.QuestionnaireVM = new QuestionnaireViewModel();
                        customSession.QuoteSummaryVM = null;
                        customSession.PurchaseVM = null;

                        #endregion
                    }

                    #endregion

                    #region comment: Here R-Scenario-1. if user has SELECTED "OTHER" indust,subIndust,class then only

                    if (!string.IsNullOrWhiteSpace(request.OtherClassDesc) && request.OtherClassDesc.Trim().Length > 0)
                    {
                        customSession.QuestionnaireVM.QuoteStatus = "Soft Referral";
                        customSession.QuestionnaireVM.ReferralScenarioId = 1;
                        customSession.QuestionnaireVM.ReferralScenarioIds.Add(1);
                        customSession.QuestionnaireVM.QuoteReferralMessage = Constants.CustomQuoteReferralMessage5;
                        customSession.QuestionnaireVM.QuoteReferralMessages.Add(Constants.CustomQuoteReferralMessage5);

                        //To redirect user to "Referral Screen"
                        redirectionPath = "/ReferralQuote";
                    }

                    #endregion

                    #region comment: Here R-Scenario-2. if user selects multi-state, then throw referral screen straight after exposure screen. Do not ask questions

                    if (request.IsMultiStateApplicable)
                    {
                        //Comment : Here set REFERRAL scenario id to get related details for Mail-Template
                        customSession.QuestionnaireVM.QuoteStatus = "Soft Referral";
                        customSession.QuestionnaireVM.ReferralScenarioId = 4;
                        customSession.QuestionnaireVM.ReferralScenarioIds.Add(4);
                        customSession.QuestionnaireVM.QuoteReferralMessage = Constants.CustomQuoteReferralMessage7;
                        customSession.QuestionnaireVM.QuoteReferralMessages.Add(Constants.CustomQuoteReferralMessage7);

                        redirectionPath = "/ReferralQuote";
                    }

                    #endregion

                    #region comment: Here R-Scenario-3. if user has choosen business class which have Direct sales status is "N" then

                    if (request.BusinessClassDirectSales != null && request.BusinessClassDirectSales.Equals("N"))
                    {
                        customSession.QuestionnaireVM.QuoteStatus = "Hard Referral";

                        //Comment : Here must reset in case of HardReferral/Decline
                        //customSession.QuestionnaireVM.ReferralScenarioId = 0;
                        //customSession.QuestionnaireVM.ReferralScenarioIds.Clear();
                        //customSession.QuestionnaireVM.QuoteReferralMessage = null;
                        //customSession.QuestionnaireVM.QuoteReferralMessages.Clear();

                        //Comment : Here on page load reset "Referral Scenarios" list
                        customSession.QuestionnaireVM.QuoteReferralMessages.Clear();
                        customSession.QuestionnaireVM.ReferralScenarioIds.Clear();

                        //Comment : Here set "Decline" reason
                        customSession.QuestionnaireVM.ReferralScenarioIds.Add(10);

                        //To redirect user to "Decline Screen"
                        redirectionPath = "/DeclinedQuote";
                    }

                    #endregion

                    #region Comment : All Referral scenarios which goes direct to "Referral/Decline Page" skipping "Questionnaire Page"

                    //Comment : Here below are Referral conidtion
                    //1. Other chosen (indus,sub-indus,class) - "Soft Referral"
                    //2. Multi state choosen - "Soft Referral"
                    //3. Direct sales "N" exist for bussiness class - "Decline"

                    if (
                            !string.IsNullOrWhiteSpace(request.OtherClassDesc) ||
                            request.IsMultiStateApplicable ||
                            (request.BusinessClassDirectSales != null && request.BusinessClassDirectSales.Equals("N"))
                       )
                    {
                        #region Comment : Here In case any ReferralScenario exists for this QUOTE then add this into Quote's "ReferralScenariosHistory"

                        if (!customSession.QuestionnaireVM.IsNull() && customSession.QuestionnaireVM.ReferralScenarioIds.Count > 0)
                        {
                            //Comment : Here if not initialized then initialize
                            if (referralDeclineHistory.IsNull())
                            {
                                referralDeclineHistory = new List<List<int>>();
                            }
                            referralDeclineHistory.Add(customSession.QuestionnaireVM.ReferralScenarioIds);

                            //Comment : Here finally add this Referral/Decline history into Session object
                            customSession.QuestionnaireVM.ReferralScenariosHistory = new List<List<int>>(referralDeclineHistory);
                        }

                        #endregion
                    }

                    #endregion

                    #endregion

                    #endregion

                    #endregion

                    //Comment : Here finally update current active session into CustomSession
                    SetCustomSession(customSession);

                    #region Comment : Here Update/Save New QuoteId Into Cookies

                    //Comment : Here save this QuoteId in user cookies for later use
                    //QuoteCookieHelper.Cookie_SaveQuoteId(ControllerContext.HttpContext, wcQuoteId);			// save the quote id off in a cookie

                    #endregion

                    return Json(new
                    {
                        response = true,
                        path = redirectionPath
                    }, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                return Json(new
                {
                    response = false,
                    path = redirectionPath,
                    errorList = listOfErrors
                }, JsonRequestBehavior.AllowGet);
            }
                #endregion

        }

        /// <summary>
        /// Send mail with current saved quote revocation link
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveForLater(string pageName, string emailId)
        {
            #region Comment : Here get all custom session information reuired for this step processing

            CustomSession appSession = GetCustomSession();

            #endregion

            #region Comment : Here get current quoteId from cookie

            //stored in a cookie on the user's machine
            int wcQuoteId = QuoteCookieHelper.Cookie_GetQuoteId(this.ControllerContext.HttpContext);

            #endregion

            #region Comment : Here when session and quote-id existance verfied then if emailId is correct then send mail to user

            if (pageName.Trim().Length > 0 && emailId.Trim().Length > 0)
            {
                try
                {
                    //Comment : Here according new implementation on 04.04.2016 (Add quote into Dashboard even when user does SaveForLater on Exposure/Question)
                    if (!GetLoggedUserId().IsNull())
                    {
                        #region Comment : Here add this quote in Dashboard from this page iteself

                        //Comment : Here Questionnaire interface refernce to do/make process all business logic
                        IQuestionnaire questionnaireBLL = new Questionnaire(GetCustomSession(), guardServiceProvider);

                        //Comment : Here add this quote inot Dashboard
                        questionnaireBLL.AddQuoteToDashboard(GetLoggedUserId(), wcQuoteId, GetLoggedInUserDetails().Email);

                        #endregion
                    }

                    ICommonFunctionality commonFunctionality = new CommonFunctionality();

                    //Comment : Here call BLL to make this data stored in DB
                    bool isStoredInDB = commonFunctionality.AddApplicationCustomSession(
                        new DML.WC.DTO.CustomSession() { QuoteID = wcQuoteId, SessionData = commonFunctionality.StringifyCustomSession(appSession), IsActive = true, CreatedDate = DateTime.Now, CreatedBy = GetLoggedUserId() ?? 1, ModifiedDate = DateTime.Now, ModifiedBy = GetLoggedUserId() ?? 1 });

                    #region Comment : Here if stored in DB end then finally send user mail

                    if (wcQuoteId != 0 && isStoredInDB)
                    {
                        bool mailSent = SendMailSaveForLater(pageName, wcQuoteId, emailId);

                        //Comment : Here based on mail sent status through user a message
                        if (mailSent)
                            return Json(new { resultStatus = "OK", resultText = "Successfully submitted" }, JsonRequestBehavior.AllowGet);
                    }

                    #endregion
                }
                catch
                {
                    return Json(new { resultStatus = "NOK", resultText = "Something went wrong while processing save for later !" }, JsonRequestBehavior.AllowGet);
                }
            }

            #endregion

            return Json(new { resultStatus = "NOK", resultText = "Something went wrong while processing save for later !" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveForLaterComplete(WcHomePageViewModel request, string emailId)
        {
            #region Comment : Here get all custom session information reuired for this step processing

            CustomSession appSession = GetCustomSession();

            #endregion

            #region Comment : Here get current quoteId from cookie

            //stored in a cookie on the user's machine
            int wcQuoteId = QuoteCookieHelper.Cookie_GetQuoteId(this.ControllerContext.HttpContext);

            #endregion

            #region Comment : Here when session and quote-id existance verfied then if emailId is correct then send mail to user

            if (emailId.Trim().Length > 0)
            {
                try
                {
                    //Comment : Here post this data into provider system
                    var exposureResponse = SaveLandingPageData(request);

                    //Comment : Here if question post processed
                    if (!exposureResponse.IsNull())
                    {
                        JsonResult jsonResult = exposureResponse as JsonResult;
                        dynamic dResult = jsonResult.Data;
                        if (dResult.ToString().Contains("response = True"))
                        {
                            //Comment : Here according new implementation on 04.04.2016 (Add quote into Dashboard even when user does SaveForLater on Exposure/Question)
                            if (!GetLoggedUserId().IsNull())
                            {
                                #region Comment : Here add this quote in Dashboard even when post Questionnnaire it's "Reffered or Declined"

                                //Comment : Here Questionnaire interface refernce to do/make process all business logic
                                IQuestionnaire questionnaireBLL = new Questionnaire(GetCustomSession(), guardServiceProvider);

                                //Comment : Here add this quote inot Dashboard
                                questionnaireBLL.AddQuoteToDashboard(GetLoggedUserId(), wcQuoteId, GetLoggedInUserDetails().Email);

                                #endregion
                            }

                            ICommonFunctionality commonFunctionality = new CommonFunctionality();

                            //Comment : Here call BLL to make this data stored in DB
                            bool isStoredInDB = commonFunctionality.AddApplicationCustomSession(
                                new DML.WC.DTO.CustomSession() { QuoteID = wcQuoteId, SessionData = commonFunctionality.StringifyCustomSession(appSession), IsActive = true, CreatedDate = DateTime.Now, CreatedBy = GetLoggedUserId() ?? 1, ModifiedDate = DateTime.Now, ModifiedBy = GetLoggedUserId() ?? 1 });

                            #region Comment : Here if stored in DB end then finally send user mail

                            if (wcQuoteId != 0 && isStoredInDB)
                            {
                                string CaptureQuotePage = "ModifyQuote";
                                bool mailSent = SendMailSaveForLater(CaptureQuotePage, wcQuoteId, emailId);

                                //Comment : Here based on mail sent status through user a message
                                if (mailSent)
                                    return Json(new { resultStatus = "OK", resultText = "Successfully submitted" }, JsonRequestBehavior.AllowGet);
                            }

                            #endregion
                        }
                    }
                }
                catch
                {
                    return Json(new { resultStatus = "NOK", resultText = "Something went wrong while processing save for later !" }, JsonRequestBehavior.AllowGet);
                }
            }

            #endregion

            return Json(new { resultStatus = "NOK", resultText = "Something went wrong while processing save for later !" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the State type whether it is Good or Bad
        /// </summary>
        /// <param name="stateCode"></param>
        /// <returns></returns>
        [HttpGet]
        public bool GetStateType()
        {
            try
            {
                CustomSession appSession = GetCustomSessionWithQuoteVM();
                string stateCode = GetCustomSession().StateAbbr;
                ICaptureQuote captureQuote = new CaptureQuote();

                IStateTypeService stateType = new StateTypeService();

                //bool isGoodState = captureQuote.IsGoodState(stateCode);
                bool isGoodState = stateType.GetAllGoodAndBadState().Where(x => x.StateCode.Equals(stateCode)).FirstOrDefault().IsGoodState;

                CustomSession customSession = GetCustomSessionWithQuoteVM();
                customSession.QuoteVM.IsGoodState = isGoodState;

                if (customSession.QuestionnaireVM.IsNull())
                {
                    customSession.QuestionnaireVM = new QuestionnaireViewModel();
                }

                if (!isGoodState)
                {
                    customSession.QuestionnaireVM.QuoteStatus = "Hard Referral";

                    //Comment : Here if this decline scenario-id does not exist in list
                    if (!customSession.QuestionnaireVM.ReferralScenarioIds.Contains(11))
                    {
                        //Comment : Here set "Decline" reason
                        customSession.QuestionnaireVM.ReferralScenarioIds.Add(11);

                        //Comment : Here In case any ReferralScenarioId exists for this QUOTE then add this into Quote's "ReferralScenariosHistory"
                        customSession.QuestionnaireVM.ReferralScenariosHistory.Add(customSession.QuestionnaireVM.ReferralScenarioIds);
                    }
                }

                SetCustomSession(customSession);
                return isGoodState;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Get companion class codes
        /// </summary>
        /// <param name="classDescId"></param>
        /// <param name="state"></param>
        /// <param name="zipCode"></param>
        /// <returns></returns>
        public JsonResult GetCompanionClasses(int classDescId, string payrollAmount)
        {

            CustomSession appSession = GetCustomSessionWithQuoteVM();
            //Comment : Progress bar nagivation binding based on flag 
            //Session["flag"] = 2;
            appSession.PageFlag = 2;
            List<NavigationModel> links = new List<NavigationModel>();
            NavigationController nc = new NavigationController();
            links = nc.GetProgressBarLinks(appSession.PageFlag);

            if (appSession.QuoteVM.ClassDescriptionId != classDescId || (
                appSession.QuoteVM.AnnualPayroll != Convert.ToInt64(UtilityFunctions.ToNumeric(payrollAmount)) &&
                UtilityFunctions.ToNumeric(appSession.QuoteVM.TotalPayroll) != UtilityFunctions.ToNumeric(payrollAmount)))
            {
                return Json(new { Error = "Data altered" }, JsonRequestBehavior.AllowGet);
            }
            ICaptureQuote captureQuote = new CaptureQuote();
            List<CompanionClassData> returnCompClassList = null;
            var companionClassList = captureQuote.FetchCompanionClasses(classDescId, appSession.StateAbbr, appSession.ZipCode);
            if (!companionClassList.IsNull() && companionClassList.Count > 0)
            {
                appSession.apiCompClassList = companionClassList;
                SetCustomSession(appSession);
                returnCompClassList = new List<CompanionClassData>();


                foreach (var data in companionClassList)
                {
                    if (!appSession.IsNull() && !appSession.QuoteVM.IsNull() &&
                            !appSession.QuoteVM.CompClassData.IsNull() &&
                            appSession.QuoteVM.CompClassData.Count > 0 && appSession.QuoteVM.CompClassData.Any(x => x.ClassCode == data.ClassCode))
                    {
                        if (appSession.QuoteVM.TotalPayroll.Trim().Equals(payrollAmount.Trim()) && appSession.QuoteVM.ClassDescriptionId == classDescId && appSession.QuoteVM.CompClassData.Count == companionClassList.Count)
                        {
                            returnCompClassList.Add(appSession.QuoteVM.CompClassData.Find(x => x.ClassCode == data.ClassCode));
                        }
                        else
                        {
                            returnCompClassList.Add(AddCompClassData(data));
                        }
                    }
                    else
                    {
                        returnCompClassList.Add(AddCompClassData(data));
                    }

                }
                SetCustomSession(appSession);
            }
            return Json(new { companionClassList = returnCompClassList, NavLinks = links }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Primary class Code data based on Class description ID and State Code
        /// </summary>
        /// <param name="stateCode"></param>
        /// <param name="classDescId"></param>
        /// <returns></returns>
        public JsonResult GetPrimaryClassData(string stateCode, int classDescId)
        {
            CustomSession appSession = GetCustomSession();
            ICaptureQuote captureQuote = new CaptureQuote();
            var response = captureQuote.GetMinimumPayrollThreshold(appSession.StateAbbr, classDescId);
            return Json(new { primaryClassCodeData = response }, JsonRequestBehavior.AllowGet);
        }


        #endregion Public Methods End

        #region Private Methods

        /// <summary>
        /// Get Last Saved Exposure and Policy Data from Session
        /// </summary>
        /// <param name="expList"></param>
        /// <param name="policyData"></param>
        private void GetLastExposureListAndPolicyData(ref List<Exposure> expList, ref PolicyData policyData)
        {
            CustomSession customSession;
            customSession = GetCustomSessionWithQuoteVM();
            if (!customSession.QuoteVM.Exposures.IsNull() && customSession.QuoteVM.Exposures.Count > 0)
            {
                expList = customSession.QuoteVM.Exposures;
            }
            if (!customSession.QuoteVM.PolicyData.IsNull())
            {
                policyData = customSession.QuoteVM.PolicyData;
            }
        }

        /// <summary>
        /// Set County Data in Session
        /// </summary>
        /// <param name="session"></param>
        /// <param name="quoteVM"></param>
        private void SetCountyAndQuoteVmInCustomSession(ref CustomSession session, QuoteViewModel quoteVM)
        {
            session = null;
            int wcQuoteID = GenerateQuoteId(quoteVM.County.ZipCode);
            session = new CustomSession();
            session.QuoteID = wcQuoteID;
            session.StateAbbr = quoteVM.County.State;
            session.ZipCode = quoteVM.County.ZipCode;
            session.QuoteVM = new QuoteViewModel();
            session.QuoteVM.County = new County();
            session.QuoteVM.County = quoteVM.County;
        }

        /// <summary>
        /// Return exposure data based on supplied QuoteId
        /// </summary>
        /// <param name="wcQuoteViewModel"></param>
        /// <returns></returns>
        ///
        [HttpGet]
        public bool GetExposures()
        {
            //Comment : Here if response contains data
            PolicyData policyData = null;
            bool success = true;
            CustomSession customSession;
            try
            {

                // stored in a cookie on the user's machine
                int wcQuoteId = QuoteCookieHelper.Cookie_GetQuoteId(this.ControllerContext.HttpContext);

                if (wcQuoteId != 0)
                {
                    #region Comment : Here create BatchAction list object

                    /*----------------------------------------
                        populate a BatchActionList that contains all requests to be sent to the Gaurd API
                    ----------------------------------------*/

                    var batchActionList = new BatchActionList();	// BatchActionList: list of actions routed to the Insurance Service for response
                    BatchResponseList batchResponseList;			// BatchResponseList: list of responses returned from the Insurance Service
                    string jsonResponse;							// JSON-formated response data returned from the Insurance Service


                    // deserialize each response returned by the service, and copy the OperationStatus to the view model for demo purposes
                    OperationStatus operationStatus = new OperationStatus();

                    customSession = GetCustomSession();

                    #endregion

                    #region Comment : Here get Exposure and PolicyData to maintain in session for Pre-Popullation
                    var lobDataGetAction = new BatchAction { ServiceName = "LobData", ServiceMethod = "GET", RequestIdentifier = "LobData" };
                    lobDataGetAction.BatchActionParameters.Add(
                        new BatchActionParameter { Name = "lobDataRequestParms", Value = JsonConvert.SerializeObject(new LobDataRequestParms { IncludeRelated = true, QuoteId = wcQuoteId, LobAbbr = LineOfBusiness }) });
                    batchActionList.BatchActions.Add(lobDataGetAction);

                    var policyDataGetAction = new BatchAction { ServiceName = "PolicyData", ServiceMethod = "GET", RequestIdentifier = "PolicyData" };
                    policyDataGetAction.BatchActionParameters.Add(
                        new BatchActionParameter { Name = "policyDataRequestParms", Value = JsonConvert.SerializeObject(new PolicyDataRequestParms { QuoteId = wcQuoteId }) });
                    batchActionList.BatchActions.Add(policyDataGetAction);

                    var exposureGetAction = new BatchAction { ServiceName = "Exposures", ServiceMethod = "GET", RequestIdentifier = "Exposures" };
                    exposureGetAction.BatchActionParameters.Add(
                        new BatchActionParameter { Name = "exposureRequestParms", Value = JsonConvert.SerializeObject(new ExposureRequestParms { QuoteId = wcQuoteId }) });
                    batchActionList.BatchActions.Add(exposureGetAction);
                    ServiceProvider provider = new GuardServiceProvider() { ServiceCategory = LineOfBusiness };
                    jsonResponse = SvcClient.CallServiceBatch(batchActionList, provider);

                    // deserialize the results into a BatchResponseList
                    batchResponseList = JsonConvert.DeserializeObject<BatchResponseList>(jsonResponse);

                    // deserialize each response returned by the service, and copy the OperationStatus to the view model for demo purposes
                    operationStatus = new OperationStatus();
                    foreach (var batchResponse in batchResponseList.BatchResponses)
                    {
                        // this flag tested later to determine whether to proceed to the next view or not
                        if (!batchResponse.RequestSuccessful) { success = false; }
                    }

                    #region Comment : Here get ExposureId and get PolicyData

                    if (batchResponseList != null && success)
                    {
                        #region Comment : Here get PolicyDataId
                        var policyDataResponse = batchResponseList.BatchResponses.SingleOrDefault(m => m.RequestIdentifier == "PolicyData").JsonResponse;

                        if (policyDataResponse != null)
                        {
                            policyData = JsonConvert.DeserializeObject<PolicyDataResponse>(policyDataResponse).PolicyData;

                            if (policyData != null && policyData.PolicyDataId != null)
                            {
                                success = true;
                                //Comment : Here this session will be used to get this infor mation to set on other pages in Purchase process flow
                                if (customSession.QuoteVM.IsNull())
                                {
                                    customSession.QuoteVM = new QuoteViewModel();
                                }
                                customSession.QuoteVM.PolicyData = policyData;
                            }
                        }
                        #endregion

                        #region Comment : Here get Exposure

                        var exposureResponse = batchResponseList.BatchResponses.SingleOrDefault(m => m.RequestIdentifier == "Exposures").JsonResponse;
                        List<Exposure> listOfExposures = null; ;
                        if (exposureResponse != null)
                        {
                            listOfExposures = JsonConvert.DeserializeObject<ExposureResponse>(exposureResponse).Exposures;
                            if (listOfExposures.Count > 0)
                            {
                                //Comment : Here if Exposures exists for this/Cookies QuoteId then set it to TRUE means user has saved LandingPage data earlier
                                if (!listOfExposures.IsNull() && listOfExposures.Count > 0)
                                {
                                    success = true;
                                    //Comment : Here this session will be used to get this infor mation to set on other pages in Purchase process flow
                                    if (customSession.QuoteVM.IsNull())
                                    {
                                        customSession.QuoteVM = new QuoteViewModel();
                                    }
                                    customSession.QuoteVM.Exposures = listOfExposures;
                                    if (customSession.QuoteVM.CoverageStateIds.IsNull())
                                    {
                                        customSession.QuoteVM.CoverageStateIds = new List<int>();
                                    }
                                    customSession.QuoteVM.CoverageStateIds.Add(listOfExposures[0].CoverageStateId.Value);
                                }
                            }
                        }
                        SetCustomSession(customSession);
                        #endregion

                        #region Comment : Here get Lob Data and Coverage States

                        var lobDataResponse = batchResponseList.BatchResponses.SingleOrDefault(m => m.RequestIdentifier == "LobData").JsonResponse;

                        if (lobDataResponse != null)
                        {
                            List<LobData> lobDataList = JsonConvert.DeserializeObject<LobDataResponse>(lobDataResponse).LobDataList;
                            if (!lobDataList.IsNull() && lobDataList.Count > 0)
                            {
                                success = true;
                                //Comment : Here this session will be used to get this infor mation to set on other pages in Purchase process flow
                                customSession = GetLobDataAndCoverageStateIds();
                                customSession.QuoteVM.LobDataIds.Add(lobDataList[0].LobDataId.Value);
                            }
                        }
                        SetCustomSession(customSession);
                        #endregion
                    }

                    #endregion

                    #endregion
                }
            }
            catch (Exception)
            {

            }

            return success;
        }

        /// <summary>
        /// Adds and Delete batch action
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="compClassList"></param>
        /// <param name="batchActionList"></param>
        private void AddDeleteBatchAction(Exposure exp, List<CompanionClassData> compClassList, BatchActionList batchActionList)
        {
            if (compClassList.IsNull() || (!compClassList.IsNull() && !compClassList.Any(res => res.ClassCode == exp.ClassCode && res.CompanionClassId == exp.CompanionClassId)))
            {
                ExposureRequestParms delExp = new ExposureRequestParms { ExposureId = exp.ExposureId };
                var compData = new BatchAction { ServiceName = "Exposures", ServiceMethod = "DELETE", RequestIdentifier = String.Format("Companion Code Exposure Delete {0}", exp.CompanionClassId) };
                compData.BatchActionParameters.Add(new BatchActionParameter { Name = "exposureRequestParms", Value = JsonConvert.SerializeObject(delExp) });
                batchActionList.BatchActions.Add(compData);
            }
        }

        /// <summary>
        /// Adds companion class data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private CompanionClassData AddCompClassData(CompanionClass data)
        {
            CompanionClassData dataToInsert = new CompanionClassData();
            dataToInsert.ClassCode = data.ClassCode;
            dataToInsert.ClassSuffix = data.ClassSuffix;
            dataToInsert.CompanionClassId = data.CompanionClassId;
            dataToInsert.FriendlyLabel = data.FriendlyLabel;
            dataToInsert.HelpText = data.HelpText;
            return dataToInsert;
        }

        /// <summary>
        /// set referral values in customSession
        /// </summary>
        /// <param name="customSession"></param>
        /// <param name="customMessage"></param>
        private void SetReferralInCustomerSession(CustomSession customSession, string customMessage, int scenarioId = 1)
        {
            if (customSession.QuoteVM.IsNull())
            {
                customSession.QuoteVM = new QuoteViewModel();
            }
            else if (!customSession.QuoteVM.Exposures.IsNull())
            {
                customSession.QuoteVM.Exposures = null;
            }

            customSession.QuestionnaireVM = new QuestionnaireViewModel();
            customSession.QuoteSummaryVM = null;
            customSession.PurchaseVM = null;
            customSession.QuestionnaireVM.QuoteStatus = "Soft Referral";
            customSession.QuestionnaireVM.QuoteReferralMessage = customMessage;

            loggingService.Trace(string.Format("Soft Referral processed , Reason :{0}", customSession.QuestionnaireVM.QuoteReferralMessage));

        }
        #endregion Private Methods
    }
}
