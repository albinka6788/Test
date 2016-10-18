using BHIC.Common;
using BHIC.Common.Client;
using BHIC.Common.CommonUtilities;
using BHIC.Common.Config;
using BHIC.Common.Quote;
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
using BHIC.Domain.PurchasePath;
using BHIC.Domain.Service;
using BHIC.Portal.WC.App_Start;
using BHIC.ViewDomain;
using BHIC.ViewDomain.Landing;
using BHIC.ViewDomain.QuestionEngine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BHIC.Portal.WC.Areas.PurchasePath.Controllers
{
    [ValidateSession]
    [CustomAntiForgeryToken]
    public class ExposureController : BaseController
    {
        ServiceProvider provider = new GuardServiceProvider() { ServiceCategory = Constants.LOB.WC };

        #region Public Methods
        /// <summary>
        /// Returns Purchase Path base page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            CustomSession customSession = GetCustomSession();
            SetCustomSession(customSession);
            return View();
        }

        /// <summary>
        /// Fetches Exposure details if either quoteId is provided or from session
        /// </summary>
        /// <param name="quoteId"></param>
        /// <returns></returns>
        public ActionResult GetExposureDetails(string quoteId)
        {
            QuoteViewModel expViewModel = new QuoteViewModel();
            CustomSession customSession = GetCustomSession();

            if (string.IsNullOrWhiteSpace(quoteId))
            {
                ICaptureQuote captureQuote = new CaptureQuote();
                IStateTypeService stateType = new StateTypeService();
                expViewModel.QuoteId = customSession.QuoteID.ToString();
                #region New Quote Id Creation Logic

                expViewModel.County = captureQuote.GetCountyData(customSession.ZipCode.Trim(), customSession.StateAbbr.Trim());

                #endregion

                #region Depending on the state type set the Quote Status and State type (good or bad)

                //If record for any state is not found in database then it is considered to be bad state
                if (customSession.QuoteVM.IsNull())
                {
                    customSession.QuoteVM = new QuoteViewModel();
                }
                customSession.QuoteVM.IsGoodState = stateType.GetAllGoodAndBadState().Where(x => x.StateCode.Equals(customSession.StateAbbr)).FirstOrDefault().IsGoodState;
                customSession.QuoteVM.County = expViewModel.County;

                #endregion Depending on the state type set the Quote Status and State type (good or bad)

                customSession.QuoteVM.QuoteId = expViewModel.QuoteId;
                SetCustomSession(customSession);
                expViewModel.Industries = GetIndustries();
                expViewModel.QuoteId = customSession.QuoteID.ToString();

            }
            else
            {
                //get quote-id in int format for future references
                int intQuoteId = 0;

                ICommonFunctionality commonFunctionality = GetCommonFunctionalityProvider();
                customSession = RetrieveSavedQuote(quoteId, commonFunctionality, out intQuoteId);
                expViewModel.QuoteId = intQuoteId.ToString();

                //Comment : Here if able to retrieve saved session from DB layer then do following
                if (customSession != null)
                {

                    //Comment : Here finlly update this into session
                    SetCustomSession(customSession);

                    //Save/Update QuoteId
                    QuoteCookieHelper.Cookie_SaveQuoteId(this.ControllerContext.HttpContext, intQuoteId);

                }
                else
                {
                    //Comment : Here in case when retrieve quote get NULL then throw error 
                    throw new ApplicationException("Application unhandled exception occured !!");
                }
            }

            expViewModel = !customSession.IsNull() ? customSession.QuoteVM : expViewModel;
            expViewModel.YearsInBusinessList =
                new List<YearsInBusiness>()
                {
                    //new YearsInBusiness(){ value= "", text= "Please select years in business" },
                    new YearsInBusiness(){ value= "0a", text= "Brand new venture - not started yet" },
                    new YearsInBusiness(){ value= "0b", text="Started earlier this year" },
                    new YearsInBusiness(){ value= "1", text="Started last year" },
                    new YearsInBusiness(){ value= "2", text="Started 2 years ago" },
                    new YearsInBusiness(){ value= "3", text="Started 3 years ago" },
                    new YearsInBusiness(){ value= "4", text="Started 4 years ago" },
                    new YearsInBusiness(){ value= "5", text="Started 5 years ago" },
                    new YearsInBusiness(){ value= "6", text="Started 6 years ago" },
                    new YearsInBusiness(){ value= "7", text="Started 7 years ago" },
                    new YearsInBusiness(){ value= "8", text="Started 8 years ago" },
                    new YearsInBusiness(){ value= "9", text="Started 9 years ago" },
                    new YearsInBusiness(){ value= "10",text= "Been around a while,  started 10 or more years ago" }
                };

            #region Comment : Here Included for navigation functionality (Implemented by Krishna)

            //Comment : Progress bar nagivation binding based on flag 
            List<NavigationModel> links = new List<NavigationModel>();
            NavigationController nc = new NavigationController();
            links = nc.GetProgressBarLinks(customSession.PageFlag);
            expViewModel.NavigationLinks = links;

            #endregion
            Session["quoteId"] = expViewModel.QuoteId.ToString();

            //Comment : Here set "ProspectInfo Email" if exists
            expViewModel.ProspectInfoEmail = (customSession.BusinessInfoVM != null) ? customSession.BusinessInfoVM.Email : string.Empty;

            return PartialView("_GetExposureDetails", expViewModel);
        }

        /// <summary>
        /// This method gets all the Sub-Industries based on Industry Id 
        /// </summary>
        /// <param name="industryId"></param>
        /// <returns></returns>
        public JsonResult GetSubIndustries(int industryId)
        {
            CustomSession appSession = GetCustomSession();
            appSession.QuoteVM.IndustryId = industryId;
            ISubIndustryService subIndustryService = new SubIndustryService();
            var subIndustryList = subIndustryService.GetSubIndustryList(new SubIndustryRequestParms { Lob = Constants.GetLineOfBusiness(Constants.LineOfBusiness.WC) },
                                                                    provider);
            if (!subIndustryList.IsNull() && subIndustryList.Count > 0 && subIndustryList.Any(res => res.IndustryId == industryId))
            {
                subIndustryList = subIndustryList.Where(x => x.IndustryId == industryId).ToList();
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
        private JsonResult ValidateExposureAmount(string exposureAmt, int? classDescriptionId, int? classDescKeywordId, string classCode, string directSalesOK, int? industryId, int? subIndustryId, ref bool result)
        {
            CustomSession appSession = GetCustomSession();
            VExposuresMinPayrollResponse exposureAmountResponse;
            bool resultStatus = false;
            string resultMinAmount = string.Empty;

            if (string.IsNullOrWhiteSpace(exposureAmt))
            {
                resultStatus = false;
                resultMinAmount = string.Empty;
            }
            else if (classDescKeywordId.IsNull() && classDescriptionId.IsNull())
            {
                resultStatus = false;
                resultMinAmount = string.Empty;
            }
            else if (classDescriptionId == -1 || string.IsNullOrWhiteSpace(classCode) || string.IsNullOrWhiteSpace(directSalesOK))
            {
                resultStatus = true;
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

                    //Comment : Here amount is not validated for supplied "ClassDescriptionId" then send false with 'MinimumExposure"
                    if (!(exposureAmountResponse.IsNull() && exposureAmountResponse.OperationStatus.IsNull()))
                    {
                        resultStatus = exposureAmountResponse.OperationStatus.RequestSuccessful;
                        resultMinAmount = Convert.ToString(exposureAmountResponse.MinimumExposure);
                        appSession.QuoteVM.TotalPayroll = exposureAmt.Trim();
                        appSession.QuoteVM.MinExpValidationAmount = exposureAmountResponse.MinimumExposure;
                        appSession.QuoteVM.IsGoodStateApplicable = !exposureAmountResponse.OperationStatus.RequestSuccessful;
                    }
                }
                catch (Exception)
                {
                    resultStatus = false;
                    // Changed minimum amount to $25000, as per Neelam with subject FW: Minimum Payroll Testing received on Wed 20-01-2016 20:35
                    resultMinAmount = "$25,000"; //"$15,000";
                }
            }
            //SetCustomSession(appSession);
            return Json(new { resultStatus = resultStatus, resultMinAmount = resultMinAmount }, JsonRequestBehavior.AllowGet);
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
            CustomSession appSession = GetCustomSession();
            ICaptureQuote captureQuote = new CaptureQuote();
            List<CompanionClassData> returnCompClassList = null;

            if (appSession.QuoteSummaryVM != null)
            {
                appSession.QuoteSummaryVM.Deductibles = null;
                appSession.QuoteSummaryVM.selectedDeductible = null;
                appSession.QuoteSummaryVM.ListOfDeductiblesTypes = null;
            }

            var companionClassList = captureQuote.FetchCompanionClasses(classDescId, appSession.StateAbbr, appSession.ZipCode);
            if (!companionClassList.IsNull() && companionClassList.Count > 0)
            {
                appSession.apiCompClassList = companionClassList;
                returnCompClassList = new List<CompanionClassData>();
                companionClassList.ForEach(res => returnCompClassList.Add(AddCompClassData(res)));
            }
            return Json(new { companionClassList = returnCompClassList }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Save Exposure Details
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SubmitExposureDetails(QuoteViewModel request, bool fromSaveForLater = false)
        {
            ICaptureQuote captureQuote = new CaptureQuote();
            var listOfErrors = new List<string>();
            CustomSession customSession = GetCustomSession();
            string redirectionPath = string.Empty;
            bool exposureAndPolicyReset = true;
            bool amountValidated = (fromSaveForLater ? fromSaveForLater : ValidateExposureAmountOnSubmit(ref request));
            if (!amountValidated)
            {
                return Json(new
                {
                    response = false,
                    path = string.Empty,
                    errorList = listOfErrors,
                    resultStatus = request.MinPayrollAllResponseRecieved,
                    resultMinAmount = request.MinExpValidationAmount
                }, JsonRequestBehavior.AllowGet);
            }
            var validRequest = ValidateExposureData(request, ref listOfErrors, customSession);
            if (validRequest)
            {
                int wcQuoteId = QuoteCookieHelper.Cookie_GetQuoteId(this.ControllerContext.HttpContext);
                var exposure = new Exposure();
                var policyData = new PolicyData();
                List<Exposure> lastSavedExposurelist = null;
                PolicyData lastSavedPolicyData = null;
                bool proceedForSubmission = false;

                // GUIN - 226
                if (request.ClassCode == "-1" && !string.IsNullOrWhiteSpace(request.OtherClassDesc))
                {
                    captureQuote.AddClassKeyword(request.OtherClassDesc.Trim());
                }

                proceedForSubmission = (
                        (request.SelectedSearch == 0 && UtilityFunctions.IsValidInt(request.ClassDescriptionId) && request.ClassDescriptionId > 0) ||
                        (request.SelectedSearch == 1 && (!request.Class.IsNull() && UtilityFunctions.IsValidInt(request.Class.ClassDescriptionId) && request.Class.ClassDescriptionId > 0)));

                if (!customSession.QuoteVM.IsNull())
                {
                    if (!customSession.QuoteVM.Exposures.IsNull() && customSession.QuoteVM.Exposures.Count > 0)
                    {
                        lastSavedExposurelist = customSession.QuoteVM.Exposures;
                    }
                    if (!customSession.QuoteVM.PolicyData.IsNull() && customSession.QuoteVM.PolicyData.PolicyDataId.HasValue)
                    {
                        lastSavedPolicyData = customSession.QuoteVM.PolicyData;
                    }
                }
                customSession.QuoteVM = request;
                customSession.QuoteVM.IsMultiClassPrimaryExposureValid = true;
                customSession.QuoteVM.Exposures = new List<Exposure>();
                var batchActionList = new BatchActionList();	// BatchActionList: list of actions routed to the Insurance Service for response
                BatchResponseList batchResponseList = null;			// BatchResponseList: list of responses returned from the Insurance Service
                string jsonResponse;							// JSON-formated response data returned from the Insurance Service
                bool success = true;
                OperationStatus operationStatus = new OperationStatus();

                #region Comment : Here [GUIN-202-SreeRam] Handled Conversion Count(1.a.iii) & ExposureScreenDropCount(4.a.i) for "UserReports"

                //Comment : Here [GUIN-202-Prem] Check for existing values of "QuoteStatus" flags
                var quoteStatusVM = !customSession.QuoteStatusVM.IsNull() ? customSession.QuoteStatusVM : new QuoteStatusViewModel();

                var quotestatusData = new QuoteStatus();
                quotestatusData.LandingSaved = quoteStatusVM.LandingSaved;
                quotestatusData.ClassesSelected = DateTime.Now;
                quotestatusData.ExposuresSaved = DateTime.Now;
                quotestatusData.QuoteId = wcQuoteId;
                quotestatusData.UserIP = UtilityFunctions.GetUserIPAddress(this.ControllerContext.HttpContext.ApplicationInstance.Context);

                //update QuoteStatus VM in current active session state
                quoteStatusVM.ClassesSelected = DateTime.Now;
                quoteStatusVM.ExposuresSaved = DateTime.Now;
                customSession.QuoteStatusVM = quoteStatusVM;

                var quotestatusAction = new BatchAction { ServiceName = "QuoteStatus", ServiceMethod = "POST", RequestIdentifier = "QuoteStatus Data" };
                quotestatusAction.BatchActionParameters.Add(new BatchActionParameter { Name = "QuoteStatus", Value = JsonConvert.SerializeObject(quotestatusData) });
                batchActionList.BatchActions.Add(quotestatusAction);

                #endregion

                if (proceedForSubmission)
                {

                    #region Comment : Here get user last exposure details if exists in Session to avoid Guard API call overhead otherwise call "Exposure" and re-assign session for later usage

                    if (!lastSavedPolicyData.IsNull() && UtilityFunctions.IsValidInt(lastSavedPolicyData.PolicyDataId) && request.ClassDescriptionId != -1)
                    {
                        policyData.PolicyDataId = lastSavedPolicyData.PolicyDataId;
                    }
                    if (!lastSavedExposurelist.IsNull() && lastSavedExposurelist.Count() > 0)
                    {
                        var lastSavedPrimaryExposure = lastSavedExposurelist.Where(x => x.CompanionClassId.IsNull()).First();
                        exposure.ExposureId = lastSavedPrimaryExposure.ExposureId;
                        exposure.CoverageStateId = lastSavedPrimaryExposure.CoverageStateId;//=GetLobDataAndCoverageStateIds().QuoteVM.CoverageStateIds[0]
                    }

                    #endregion

                    #region Comment : Here create exposure object

                    if (request.SelectedSearch == 0)
                    {
                        exposure.ClassDescriptionKeywordId = request.ClassDescriptionKeywordId;
                        exposure.ClassDescriptionId = request.ClassDescriptionId;
                    }
                    else
                    {
                        exposure.IndustryId = !request.Industry.IsNull() ? (request.Industry.IndustryId > 0 ? request.Industry.IndustryId : 0) : 0;
                        exposure.SubIndustryId = !request.SubIndustry.IsNull() ? (request.SubIndustry.SubIndustryId > 0 ? request.SubIndustry.SubIndustryId : 0) : 0;
                        exposure.ClassDescriptionId = !request.Class.IsNull() ? (request.Class.ClassDescriptionId > 0 ? request.Class.ClassDescriptionId : 0) : 0;
                        if (request.Industry.IndustryId == -1 || request.SubIndustry.SubIndustryId == -1 || request.Class.ClassDescriptionId == -1)
                        {
                            exposure.OtherClassDesc = request.OtherClassDesc;
                            exposure.ClassDescriptionId = null;
                        }
                    }
                    exposure.QuoteId = wcQuoteId;
                    exposure.LOB = Constants.LOB.WC;
                    exposure.ExposureAmt = Convert.ToDecimal(UtilityFunctions.ToNumeric(request.TotalPayroll));
                    if (!request.CompClassData.IsNull() && request.CompClassData.Count > 0 && exposure.ExposureAmt >= 50000)
                    {
                        foreach (var data in request.CompClassData)
                        {
                            if (!string.IsNullOrWhiteSpace(data.PayrollAmount))
                                exposure.ExposureAmt -= (data.IsExposureAmountRequired.HasValue && data.IsExposureAmountRequired.Value) ? Convert.ToInt64(UtilityFunctions.ToNumeric(data.PayrollAmount)) : 0;
                        }

                        #region Comment : Here when MC applicable then only check for MinPayrollThreshold

                        long totalPayroll = Convert.ToInt64(UtilityFunctions.ToNumeric(request.TotalPayroll));
                        long expAmount = Convert.ToInt64(exposure.ExposureAmt);
                        if (!(totalPayroll == expAmount))
                        {
                            BHIC.DML.WC.DTO.PrimaryClassCodeDTO primaryClassCodeData = captureQuote.GetMinimumPayrollThreshold(customSession.StateAbbr, request.SelectedSearch == 0 ? request.ClassDescriptionId.Value : request.Class.ClassDescriptionId);
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

                    //Assign this primary class payroll in customsession for Referral mails
                    customSession.QuoteVM.AnnualPayroll = exposure.ExposureAmt.Value;

                    exposure.ZipCode = customSession.ZipCode;
                    exposure.StateAbbr = customSession.StateAbbr;

                    #endregion

                    #region Comment : Here create policy data object
                    if (string.IsNullOrWhiteSpace(exposure.OtherClassDesc))
                    {
                        policyData.QuoteId = wcQuoteId;
                        policyData.InceptionDate = DateTime.Parse(request.InceptionDate, new System.Globalization.CultureInfo("en-US", true));
                    }
                    policyData.YearsInBusiness = request.SelectedYearInBusiness.value.ToCharArray()[0] == '0' ? 0 : Convert.ToInt16(request.SelectedYearInBusiness.value);

                    #endregion

                    #region Comment : Here Service call batch of actions creation

                    // populate a BatchAction that will be used to submit the Exposure DTO to the Insurance Service
                    var exposuresAction = new BatchAction { ServiceName = "Exposures", ServiceMethod = "POST", RequestIdentifier = "Exposure Data" };
                    exposuresAction.BatchActionParameters.Add(new BatchActionParameter { Name = "exposure", Value = JsonConvert.SerializeObject(exposure) });
                    batchActionList.BatchActions.Add(exposuresAction);

                    //Add this exposure to custom Session
                    if (!customSession.QuoteVM.IsNull())
                    {
                        customSession.QuoteVM.Exposures.Add(exposure);
                    }

                    #region Remove all the companion Exposures whose Class code are not present in request
                    if (!lastSavedExposurelist.IsNull() && lastSavedExposurelist.Count > 0)
                    {
                        var companionExposuresList = lastSavedExposurelist.ExceptWhere(x => x.CompanionClassId.IsNull()).ToList();
                        if (!companionExposuresList.IsNull() && companionExposuresList.Count > 0)
                        {
                            companionExposuresList.ForEach(res => AddDeleteBatchAction(res, request.CompClassData, batchActionList));
                        }
                    }
                    #endregion Remove all the companion Exposures whose Class code are not present in request

                    #region Add Companion Class Data in request

                    if (!request.CompClassData.IsNull() && request.CompClassData.Count > 0 && exposure.ClassDescriptionId != null)
                    {
                        customSession.QuoteVM.Exposures.Where(x => !x.CompanionClassId.IsNull()).ToList().ForEach(i => i = null);
                        foreach (var data in request.CompClassData)
                        {
                            Exposure exp = new Exposure();
                            if (!lastSavedExposurelist.IsNull() && lastSavedExposurelist.Count > 0 && lastSavedExposurelist.Any(x => x.ClassCode == data.ClassCode && x.CompanionClassId == data.CompanionClassId))
                            {
                                exp = lastSavedExposurelist.Find(x => x.ClassCode == data.ClassCode || x.CompanionClassId == data.CompanionClassId);
                            }
                            exp.CompanionClassId = data.CompanionClassId;
                            exp.ClassCode = data.ClassCode;
                            exp.QuoteId = wcQuoteId;
                            exp.LOB = Constants.GetLineOfBusiness(Constants.LineOfBusiness.WC);
                            exp.ExposureAmt = data.IsExposureAmountRequired.HasValue && data.IsExposureAmountRequired.Value && !String.IsNullOrWhiteSpace(data.PayrollAmount) ? Convert.ToDecimal(UtilityFunctions.ToNumeric(data.PayrollAmount)) : 0;
                            exp.ZipCode = customSession.ZipCode;
                            exp.StateAbbr = customSession.StateAbbr;
                            if (data.IsExposureAmountRequired.HasValue && data.IsExposureAmountRequired.Value)
                            {
                                customSession.QuoteVM.Exposures.Add(exp);
                                var compData = new BatchAction { ServiceName = "Exposures", ServiceMethod = "POST", RequestIdentifier = String.Format("CompanionExposure_{0}", data.CompanionClassId) };
                                compData.BatchActionParameters.Add(new BatchActionParameter { Name = "exposure", Value = JsonConvert.SerializeObject(exp) });
                                batchActionList.BatchActions.Add(compData);
                            }
                            else
                            {
                                if (exp.ExposureId > 0)
                                {
                                    DeleteExposure(exp, batchActionList);
                                }
                            }
                        }


                    }

                    #endregion

                    // populate a BatchAction that will be used to submit the PolicyData DTO to the Insurance Service
                    if (exposure.ClassDescriptionId != null)
                    {
                        var policyDataAction = new BatchAction { ServiceName = Constants.ServiceNames.PolicyData, ServiceMethod = Constants.MethodType.POST, RequestIdentifier = "Policy Data" };
                        policyDataAction.BatchActionParameters.Add(new BatchActionParameter { Name = "policyData", Value = JsonConvert.SerializeObject(policyData) });
                        batchActionList.BatchActions.Add(policyDataAction);
                        customSession.QuoteVM.PolicyData = policyData;
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
                else
                {
                    //Comment :  Here Handled [GUIN-202-SreeRam] Failure To Class Count(3.a.i) for "UserReports"
                    if (
                           request.SelectedSearch == 0 ||
                           (
                               request.SelectedSearch == 1 &&
                               request.Industry.IndustryId == -1 || (!request.SubIndustry.IsNull() && request.SubIndustry.SubIndustryId == -1) || (!request.Class.IsNull() && request.Class.ClassDescriptionId == -1)
                           )
                      )
                    {
                        var lastSavedPrimaryExposure = (!lastSavedExposurelist.IsNull() && lastSavedExposurelist.Any()) ?
                            lastSavedExposurelist.Where(x => x.CompanionClassId.IsNull()).First() : new Exposure();

                        if (!lastSavedExposurelist.IsNull() && lastSavedExposurelist.Any())
                        {
                            //Comment : Here [GUIN-202-Prem] In case when user comes back with some last/previuos exposure list then only DELETE
                            if (string.IsNullOrWhiteSpace(lastSavedExposurelist.First().OtherClassDesc))
                            {
                                //Comment : Here get "PrimaryExposure"
                                var lastSavedCompanionExposures = lastSavedExposurelist.Where(x => !x.CompanionClassId.IsNull());

                                foreach (var delExp in lastSavedCompanionExposures)
                                {
                                    DeleteExposure(delExp, batchActionList);
                                }
                                lastSavedExposurelist = null;
                            }
                        }

                        #region Comment : Here Handled [GUIN-202-SreeRam] Failure To Class Count(3.a.i) for "UserReports"

                        if (request.ClassCode == "-1" && !string.IsNullOrWhiteSpace(request.OtherClassDesc))
                        {
                            //Comment : Here add previuos "Expo. & CoverageState Ids" for data "UPDATE" instrunction to API
                            exposure.ExposureId = !lastSavedPrimaryExposure.IsNull() ? lastSavedPrimaryExposure.ExposureId : null;
                            exposure.CoverageStateId = !lastSavedPrimaryExposure.IsNull() ? lastSavedPrimaryExposure.CoverageStateId : null;

                            exposure.QuoteId = wcQuoteId;
                            exposure.OtherClassDesc = request.OtherClassDesc.Trim();
                            exposure.LOB = Constants.LOB.WC;
                            exposure.ExposureAmt = Convert.ToDecimal(UtilityFunctions.ToNumeric(request.TotalPayroll));
                            exposure.ZipCode = customSession.ZipCode;
                            exposure.StateAbbr = customSession.StateAbbr;

                            var exposuresAction = new BatchAction { ServiceName = "Exposures", ServiceMethod = "POST", RequestIdentifier = "Exposure Data" };
                            exposuresAction.BatchActionParameters.Add(new BatchActionParameter { Name = "exposure", Value = JsonConvert.SerializeObject(exposure) });
                            batchActionList.BatchActions.Add(exposuresAction);
                        }

                        #endregion

                        if (request.ClassCode == "-1" && !string.IsNullOrWhiteSpace(request.OtherClassDesc))
                        {
                            #region Comment : Here After understading with Nishank removed eariler implemetation to DELETE PolicyData

                            if (!lastSavedPolicyData.IsNull() && UtilityFunctions.IsValidInt(lastSavedPolicyData.PolicyDataId))
                            {
                                policyData.PolicyDataId = lastSavedPolicyData.PolicyDataId;
                            }
                            policyData.QuoteId = wcQuoteId;
                            policyData.InceptionDate = DateTime.Parse(request.InceptionDate, new System.Globalization.CultureInfo("en-US", true));

                            policyData.YearsInBusiness = request.SelectedYearInBusiness.value.ToCharArray()[0] == '0' ? 0 : Convert.ToInt16(request.SelectedYearInBusiness.value);

                            var policyDataAction = new BatchAction { ServiceName = Constants.ServiceNames.PolicyData, ServiceMethod = Constants.MethodType.POST, RequestIdentifier = "Policy Data" };
                            policyDataAction.BatchActionParameters.Add(new BatchActionParameter { Name = "policyData", Value = JsonConvert.SerializeObject(policyData) });
                            batchActionList.BatchActions.Add(policyDataAction);

                            #endregion
                        }

                        if (batchActionList.BatchActions.Count > 0)
                        {
                            jsonResponse = SvcClient.CallServiceBatch(batchActionList, provider);

                            // flag that determines how to respond after processing the response received
                            // deserialize the results into a BatchResponseList
                            batchResponseList = JsonConvert.DeserializeObject<BatchResponseList>(jsonResponse);

                            // deserialize each response returned by the service, and copy the OperationStatus to the view model for demo purposes
                            operationStatus = new OperationStatus();
                            exposureAndPolicyReset = !batchResponseList.BatchResponses.Any(res => !res.RequestSuccessful);
                        }
                    }
                    customSession.QuoteVM.AnnualPayroll = Convert.ToInt64(UtilityFunctions.ToNumeric(request.TotalPayroll));
                    customSession.QuoteVM.Exposures = new List<Exposure>() { exposure };
                    customSession.QuoteVM.PolicyData = policyData;
                }

                #region Comment : Here returning response to user

                // TODO: Do something with the response
                if (!success && !exposureAndPolicyReset)
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
                        errorList = listOfErrors,
                        resultStatus = true,
                        resultMinAmount = request.MinExpValidationAmount
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    #region Comment : Here get ExposureId for newly created Exposure and store that into session object
                    //submitted = true;
                    //customSession.QuoteVM = request;
                    if (batchResponseList != null)
                    {
                        if (batchResponseList.BatchResponses.Exists(m => m.RequestIdentifier == "Policy Data"))
                        {
                            var policyDataResponse = batchResponseList.BatchResponses.SingleOrDefault(m => m.RequestIdentifier == "Policy Data").JsonResponse;
                            if (policyDataResponse != null)
                            {
                                var operationStatusDeserialized = JsonConvert.DeserializeObject<OperationStatus>(policyDataResponse);

                                if (!operationStatusDeserialized.IsNull() && operationStatusDeserialized.AffectedIds.Exists(m => m.DTOProperty == "PolicyDataId"))
                                {
                                    policyData.PolicyDataId = Convert.ToInt32(operationStatusDeserialized.AffectedIds.SingleOrDefault(m => m.DTOProperty == "PolicyDataId").IdValue);
                                    customSession.QuoteVM.PolicyData = policyData;
                                }
                            }
                        }

                        #region Update Primary Exposure Into Custom Session

                        if (batchResponseList.BatchResponses.Any(m => m.RequestIdentifier == "Exposure Data"))
                        {
                            var exposureResponse = batchResponseList.BatchResponses.SingleOrDefault(m => m.RequestIdentifier == "Exposure Data").JsonResponse;
                            if (exposureResponse != null)
                            {
                                var operationStatusDeserialized = JsonConvert.DeserializeObject<OperationStatus>(exposureResponse);
                                if (!operationStatusDeserialized.IsNull())
                                {
                                    if (!customSession.QuoteVM.Exposures.IsNull())
                                    {
                                        foreach (var item in customSession.QuoteVM.Exposures.Where(x => x.CompanionClassId.IsNull()))
                                        {
                                            if (operationStatusDeserialized.AffectedIds.Exists(m => m.DTOProperty == "ExposureId"))
                                            {
                                                item.ExposureId = Convert.ToInt32(operationStatusDeserialized.AffectedIds.SingleOrDefault(m => m.DTOProperty == "ExposureId").IdValue);
                                            }
                                            if (operationStatusDeserialized.AffectedIds.Exists(m => m.DTOProperty == "CoverageStateId"))
                                            {
                                                item.CoverageStateId = Convert.ToInt32(operationStatusDeserialized.AffectedIds.SingleOrDefault(m => m.DTOProperty == "CoverageStateId").IdValue);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        #endregion

                        #region Update Companion Exposure in Session
                        //To do 
                        if (!request.CompClassData.IsNull() && request.CompClassData.Count > 0)
                        {
                            foreach (var data in request.CompClassData)
                            {
                                if (data.IsExposureAmountRequired.HasValue && data.IsExposureAmountRequired.Value)
                                {
                                    if (batchResponseList.BatchResponses.Any(m => m.RequestIdentifier == String.Format("CompanionExposure_{0}", data.CompanionClassId)))
                                    {
                                        var companionExposureResponse = batchResponseList.BatchResponses.SingleOrDefault(m => m.RequestIdentifier == String.Format("CompanionExposure_{0}", data.CompanionClassId)).JsonResponse;
                                        if (companionExposureResponse != null)
                                        {
                                            var operationStatusDeserialized = JsonConvert.DeserializeObject<OperationStatus>(companionExposureResponse);
                                            if (!operationStatusDeserialized.IsNull())
                                            {
                                                foreach (var item in customSession.QuoteVM.Exposures.Where(x => !x.CompanionClassId.IsNull() && x.CompanionClassId == data.CompanionClassId))
                                                {
                                                    item.ExposureId = Convert.ToInt32(operationStatusDeserialized.AffectedIds.SingleOrDefault(m => m.DTOProperty == "ExposureId").IdValue);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        #endregion Update Companion Exposure in Session
                    }

                    #region Add All Other Data Object Into Custom Session Object

                    #region Comment : Here based on expousre/quote page inputs decide next page navigation path

                    //Comment : Here set default redirection path to "Question Page"
                    redirectionPath = "/GetQuestions";
                    customSession.QuoteVM.IsValidQuestionPageRequest = false;

                    //Comment : Here by default reset QuoteStatus to blank
                    if (!customSession.QuestionnaireVM.IsNull() && !string.IsNullOrEmpty(customSession.QuestionnaireVM.QuoteStatus))
                    {
                        customSession.QuestionnaireVM.QuoteStatus = string.Empty;
                        customSession.QuestionnaireVM.FeinApplicable = false;
                    }


                    #region Comment : All "Referral" scenarios condition require "QuestionnaireViewModel" initilization

                    List<int> currentReferralScenarioIds = new List<int>();
                    List<List<int>> referralDeclineHistory = null;
                    ReferralHistory referralDeclineHistoryNew = null;

                    //Comment : Here below are Referral conidtion
                    //1. Other chosen (indus,sub-indus,class) - "Soft Referral"
                    //2. Multi state choosen - "Soft Referral"
                    //3. Direct sales "N" exist for bussiness class - "Decline"
                    //4. GoodBad State Applicable And This State Is Bad State With InValid MinPayroll Amount - "Decline"

                    if (
                            !string.IsNullOrWhiteSpace(request.OtherClassDesc) ||
                            request.IsMultiStateApplicable ||
                            (request.BusinessClassDirectSales != null && request.BusinessClassDirectSales.Equals("N")) ||
                            (request.IsGoodStateApplicable && request.IsGoodState.HasValue && !request.IsGoodState.Value)
                       )
                    {
                        #region Comment : Here "QuestionnaireViewModel" initilization

                        if (customSession.QuoteVM.IsNull())
                        {
                            customSession.QuoteVM = new QuoteViewModel();
                        }

                        #region Comment : Here To maintain user Referral/Decline history we need to get "All List of Previous Referrals Details"

                        if (!customSession.QuestionnaireVM.IsNull() && !customSession.QuestionnaireVM.ReferralScenariosHistory.IsNull())
                        {
                            referralDeclineHistory = customSession.QuestionnaireVM.ReferralScenariosHistory;
                            referralDeclineHistoryNew = customSession.QuestionnaireVM.ReferralHistory;
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
                        currentReferralScenarioIds.Add(1);

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
                        currentReferralScenarioIds.Add(4);

                        redirectionPath = "/ReferralQuote";
                    }

                    #endregion

                    #region comment: Here R-Scenario-3. if user has choosen business class which have Direct sales status is "N" then

                    if (request.BusinessClassDirectSales != null && request.BusinessClassDirectSales.Equals("N"))
                    {
                        customSession.QuestionnaireVM.QuoteStatus = "Hard Referral";
                        customSession.QuestionnaireVM.QuoteReferralMessages.Clear();
                        customSession.QuestionnaireVM.ReferralScenarioIds.Clear();
                        currentReferralScenarioIds.Clear();

                        //Comment : Here set "Decline" reason
                        customSession.QuestionnaireVM.ReferralScenarioIds.Add(10);
                        currentReferralScenarioIds.Add(10);

                        //To redirect user to "Decline Screen"
                        redirectionPath = "/DeclinedQuote";
                    }

                    #endregion

                    #region comment: Here R-Scenario-4. if user has given less than MinPayroll and have BAD STATE (GoodBadStateApplicable & BadState) then

                    if (request.IsGoodStateApplicable && request.IsGoodState.HasValue && !request.IsGoodState.Value)
                    {
                        customSession.QuestionnaireVM.QuoteStatus = "Hard Referral";

                        //Comment : Here on page load reset "Referral Scenarios" list
                        customSession.QuestionnaireVM.QuoteReferralMessages.Clear();
                        customSession.QuestionnaireVM.ReferralScenarioIds.Clear();
                        currentReferralScenarioIds.Clear();

                        //Comment : Here set "Decline" reason
                        customSession.QuestionnaireVM.ReferralScenarioIds.Add(11);
                        currentReferralScenarioIds.Add(11);

                        //To redirect user to "Decline Screen"
                        redirectionPath = "/DeclinedQuote";
                    }

                    #endregion

                    #region Comment : All Referral scenarios which goes direct to "Referral/Decline Page" skipping "Questionnaire Page"

                    //Comment : Here below are Referral conidtion
                    //1. Other chosen (indus,sub-indus,class) - "Soft Referral"
                    //2. Multi state choosen - "Soft Referral"
                    //3. Direct sales "N" exist for bussiness class - "Decline"
                    //4. GoodBad State Applicable And This State Is Bad State With InValid MinPayroll Amount - "Decline"

                    if (
                            !string.IsNullOrWhiteSpace(request.OtherClassDesc) ||
                            request.IsMultiStateApplicable ||
                            (request.BusinessClassDirectSales != null && request.BusinessClassDirectSales.Equals("N")) ||
                            (request.IsGoodStateApplicable && request.IsGoodState.HasValue && !request.IsGoodState.Value)
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
                            //referralDeclineHistory.Add(new List<int>(customSession.QuestionnaireVM.ReferralScenarioIds));
                            referralDeclineHistory.Add(new List<int>(currentReferralScenarioIds));

                            if (referralDeclineHistoryNew.IsNull())
                            {
                                referralDeclineHistoryNew = new ReferralHistory();
                            }

                            #region Comment : Here [GUIN-270-Prem] Based on all scenario ids get "All Referral Reasons & Description"

                            //Comment : Here get first list of master data for referral reason scenarios and then all scenarios "Reason & Description" list 
                            IReferralQuote referralQuoteBLL = new ReferralQuote(GetCustomSession(), guardServiceProvider);

                            var referralProcessing = new ReferralProcessing()
                            {
                                ReferralScenarioIds = currentReferralScenarioIds,
                                FilePath = Server.MapPath("~/ReferralReasons.xml")
                            };

                            //Comment : Here get all current referral reason & descriptions
                            referralQuoteBLL.GetAllReferralReasonsNew(referralProcessing);

                            if (currentReferralScenarioIds.Any())
                            {
                                referralDeclineHistoryNew.XModValueMessage = referralProcessing.XModValueMessage;
                                referralDeclineHistoryNew.ReferralScenarioIdsList.Add(currentReferralScenarioIds);
                                referralDeclineHistoryNew.ReferralScenarioTextList.Add
                                (
                                    new ReferralData()
                                    {
                                        ReasonsList = new List<string>(referralProcessing.ReasonsList),
                                        DescriptionList = new List<string>(referralProcessing.DescriptionList),
                                    }
                                );
                            }

                            //Comment : Here finally add this Referral/Decline history into Session object
                            customSession.QuestionnaireVM.ReferralScenariosHistory = new List<List<int>>(referralDeclineHistory);
                            //new referral history
                            customSession.QuestionnaireVM.ReferralHistory = referralDeclineHistoryNew;

                            #endregion
                        }

                        #endregion
                    }

                    #endregion

                    #endregion

                    #endregion

                    #endregion

                    //Comment : Here finally update current active session into CustomSession
                    customSession.QuoteVM.BusinessYears = request.SelectedYearInBusiness.value.ToCharArray()[0] == '0' ? 0 : Convert.ToInt16(request.SelectedYearInBusiness.value);

                    //Comment : Progress bar nagivation binding based on flag 
                    //Comment : Nishank [GUIN-483]: Set Default page flag for Exposure page to 1
                    customSession.PageFlag = 1;

                    SetCustomSession(customSession);

                    //Comment : Here if finally quiestion redirection has been decided then only set this flag
                    if (redirectionPath.Contains("GetQuestions"))
                    {
                        //Comment : Nishank [GUIN-483]: If next page is Questions then set page flag to 2
                        customSession.PageFlag = 2;
                        customSession.QuoteVM.IsValidQuestionPageRequest = true;

                        if (customSession.QuestionnaireVM.IsNull())
                        {
                            customSession.QuestionnaireVM = new QuestionnaireViewModel();
                        }

                        IQuestionnaire questionnaireBLL = new Questionnaire(GetCustomSession(), guardServiceProvider);
                        customSession.QuestionnaireVM.FeinApplicable = questionnaireBLL.IsFeinNumberApplicable(wcQuoteId);
                    }


                    //finally gain set running session state 
                    SetCustomSession(customSession);

                    #region Comment : Here according to new requirement in case quote already exists then must UPDATE in DB for future consequenses

                    ICommonFunctionality commonFunctionality = new CommonFunctionality();

                    //Comment : Here call BLL to make this data stored in DB
                    commonFunctionality.AddApplicationCustomSession(
                        new DML.WC.DTO.CustomSession()
                        {
                            QuoteID = wcQuoteId,
                            SessionData = commonFunctionality.StringifyCustomSession(customSession),
                            IsActive = true,
                            CreatedDate = DateTime.Now,
                            CreatedBy = GetLoggedUserId() ?? 1,
                            ModifiedDate = DateTime.Now,
                            ModifiedBy = GetLoggedUserId() ?? 1,
                            UpdateOnly = true
                        });


                    #endregion

                    return Json(new
                    {
                        response = true,
                        path = redirectionPath,
                        errorList = listOfErrors,
                        resultStatus = request.MinPayrollAllResponseRecieved,
                        resultMinAmount = request.MinExpValidationAmount
                    }, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                return Json(new
                {
                    response = false,
                    path = redirectionPath,
                    errorList = listOfErrors,
                    resultStatus = true,
                    resultMinAmount = request.MinExpValidationAmount
                }, JsonRequestBehavior.AllowGet);
            }
                #endregion

        }

        /// <summary>
        /// This method searches business on the search string and classDescriptionKeywordId. Each Search string is stored in CYBClassKeywords Table.
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="classDescriptionId"></param>
        /// <returns></returns>
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

            return Json(businessSearchResultList, JsonRequestBehavior.AllowGet);
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
                catch (Exception ex)
                {
                    loggingService.Fatal(ex);
                    return Json(new { resultStatus = "NOK", resultText = "Something went wrong while processing save for later !" }, JsonRequestBehavior.AllowGet);
                }
            }

            #endregion

            loggingService.Trace(string.Format("Failed due to either Page Name: {0} or Email ID: {1} is empty", pageName, emailId));
            return Json(new { resultStatus = "NOK", resultText = "Something went wrong while processing save for later !" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveForLaterComplete(QuoteViewModel request, string emailId)
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
                    var exposureResponse = SubmitExposureDetails(request, true);

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
                                string CaptureQuotePage = "GetExposureDetails";
                                bool mailSent = SendMailSaveForLater(CaptureQuotePage, wcQuoteId, emailId);

                                //Comment : Here based on mail sent status through user a message
                                if (mailSent)
                                    return Json(new { resultStatus = "OK", resultText = "Successfully submitted" }, JsonRequestBehavior.AllowGet);
                            }

                            #endregion
                        }
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

        #endregion Public Methods

        #region Private Methods

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
            dataToInsert.PromptText = data.PromptText;
            return dataToInsert;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<Industry> GetIndustries()
        {
            IIndustryService industryService = new IndustryService();
            var industryList = industryService.GetIndustryList(new IndustryRequestParms { Lob = Constants.LOB.WC }, provider);
            if (!industryList.IsNull() && industryList.Count > 0)
            {
                //industryList.Insert(0, new Industry { Description = "Please Select an Industry", IndustryId = 0 });
                industryList.Add(new Industry { Description = Constants.OtherDescription, IndustryId = Constants.OtherDescriptionId });
            }
            return industryList;
        }

        /// <summary>
        /// Set County Data in Session
        /// </summary>
        /// <param name="session"></param>
        /// <param name="quoteVM"></param>
        private void SetCountyAndQuoteVmInCustomSession(ref CustomSession session, QuoteViewModel quoteVM)
        {
            int wcQuoteID = GenerateQuoteId();

            //Comment : Here [GUIN-267-Prem] To maintain only "BusinessInfo/PorspactInfo" and create NewSession onwards this page
            var sessionBusinessInfoVM = !session.IsNull() ? session.BusinessInfoVM : null;

            session = new CustomSession();
            session.QuoteID = wcQuoteID;
            session.StateAbbr = quoteVM.County.State;
            session.ZipCode = quoteVM.County.ZipCode;
            session.QuoteVM = quoteVM;

            //Comment : Here add current "BusinessInfo/PorspactInfo" into newly created session object
            session.BusinessInfoVM = sessionBusinessInfoVM ?? null;
        }

        /// <summary>
        /// Get Quote if exists, otherwise create new Quote
        /// </summary>
        /// <returns>Return Quote view model with populated fields</returns>
        private int GenerateQuoteId()
        {
            #region Variable initialization

            string adId = string.Empty;
            HttpContextBase context = this.ControllerContext.HttpContext;
            IQuoteStatusService quoteStatusService = new QuoteStatusService();
            int wcQuoteId = 0;

            #endregion

            #region Create QuoteId and Save it in cookie

            var postOperationStatus = quoteStatusService.AddQuoteStatus(new QuoteStatus
            {
                AdId = null,
                EnteredOn = DateTime.Now,
                UserIP = UtilityFunctions.GetUserIPAddress(context.ApplicationInstance.Context)
            }, provider);

            if (postOperationStatus.RequestSuccessful)
            {
                var effectedQuoteDTO = postOperationStatus.AffectedIds
                    .SingleOrDefault(res => res.DTOProperty == "QuoteId");
                wcQuoteId = Convert.ToInt32(effectedQuoteDTO.IdValue);
            }
            QuoteCookieHelper.Cookie_SaveQuoteId(context, wcQuoteId);
            #endregion

            return wcQuoteId;
        }

        /// <summary>
        /// Return exposure data based on supplied QuoteId
        /// </summary>
        /// <param name="wcQuoteViewModel"></param>
        /// <returns></returns>
        ///
        private bool GetExposures()
        {
            PolicyData policyData = null;
            bool success = true;
            CustomSession customSession;
            try
            {
                int wcQuoteId = QuoteCookieHelper.Cookie_GetQuoteId(this.ControllerContext.HttpContext);
                if (wcQuoteId != 0)
                {
                    #region Variables declaration and initialization

                    var batchActionList = new BatchActionList();
                    BatchResponseList batchResponseList;
                    string jsonResponse;
                    OperationStatus operationStatus = new OperationStatus();
                    customSession = GetCustomSession();

                    #endregion Variables declaration and initialization

                    #region LobData and Policy Data batch request creation
                    var lobDataGetAction = new BatchAction { ServiceName = "LobData", ServiceMethod = "GET", RequestIdentifier = "LobData" };
                    lobDataGetAction.BatchActionParameters.Add(
                        new BatchActionParameter { Name = "lobDataRequestParms", Value = JsonConvert.SerializeObject(new LobDataRequestParms { IncludeRelated = true, QuoteId = wcQuoteId, LobAbbr = LineOfBusiness }) });
                    batchActionList.BatchActions.Add(lobDataGetAction);

                    var policyDataGetAction = new BatchAction { ServiceName = "PolicyData", ServiceMethod = "GET", RequestIdentifier = "PolicyData" };
                    policyDataGetAction.BatchActionParameters.Add(
                        new BatchActionParameter { Name = "policyDataRequestParms", Value = JsonConvert.SerializeObject(new PolicyDataRequestParms { QuoteId = wcQuoteId }) });
                    batchActionList.BatchActions.Add(policyDataGetAction);

                    #endregion Variables declaration and initialization

                    #region Batch Call execution and Response processing

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
                                if (customSession.QuoteVM.IsNull())
                                {
                                    customSession.QuoteVM = new QuoteViewModel();
                                }
                                customSession.QuoteVM.PolicyData = policyData;
                            }
                        }
                        #endregion

                        #region Comment : Here get Lob Data and Coverage States

                        var lobDataResponse = batchResponseList.BatchResponses.SingleOrDefault(m => m.RequestIdentifier == "LobData").JsonResponse;

                        if (lobDataResponse != null)
                        {
                            List<LobData> lobDataList = JsonConvert.DeserializeObject<LobDataResponse>(lobDataResponse).LobDataList;
                            if (!lobDataList.IsNull() && lobDataList.Count > 0)
                            {
                                success = true;
                                //Comment : Here this session will be used to get this information to set on other pages in Purchase process flow
                                customSession = GetLobDataAndCoverageStateIds();
                                customSession.QuoteVM.LobDataIds.Add(lobDataList[0].LobDataId.Value);
                                if (!lobDataList[0].CoverageStates.IsNull() && lobDataList[0].CoverageStates.Count > 0)
                                {
                                    customSession.QuoteVM.CoverageStateIds.Add(lobDataList[0].CoverageStates[0].CoverageStateId.Value);
                                    customSession.QuoteVM.Exposures = lobDataList[0].CoverageStates[0].Exposures;
                                }
                            }
                        }
                        SetCustomSession(customSession);
                        #endregion
                    }

                    #endregion

                    #endregion Batch Call execution and Response processing
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
                DeleteExposure(exp, batchActionList);
            }
        }

        /// <summary>
        /// Adds batch action to delete exposure 
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="batchActionList"></param>
        private void DeleteExposure(Exposure exp, BatchActionList batchActionList)
        {
            ExposureRequestParms delExp = new ExposureRequestParms { ExposureId = exp.ExposureId };
            var compData = new BatchAction { ServiceName = "Exposures", ServiceMethod = "DELETE", RequestIdentifier = String.Format("Companion Code Exposure Delete {0}", exp.CompanionClassId) };
            compData.BatchActionParameters.Add(new BatchActionParameter { Name = "exposureRequestParms", Value = JsonConvert.SerializeObject(delExp) });
            batchActionList.BatchActions.Add(compData);
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
            customSession.QuestionnaireVM = new QuestionnaireViewModel();
            customSession.QuoteSummaryVM = null;
            customSession.PurchaseVM = null;
            customSession.QuestionnaireVM.QuoteStatus = "Soft Referral";
            customSession.QuestionnaireVM.QuoteReferralMessage = customMessage;

            loggingService.Trace(string.Format("Soft Referral processed , Reason :{0}", customSession.QuestionnaireVM.QuoteReferralMessage));

        }

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

        private bool ValidateExposureData(QuoteViewModel request, ref List<string> listOfErrors, CustomSession appSession)
        {

            #region Basic Validations

            bool checkPayroll = false;
            checkPayroll = (
                    (request.SelectedSearch == 0 && UtilityFunctions.IsValidInt(request.ClassDescriptionId) && request.ClassDescriptionId > 0) ||
                    (request.SelectedSearch == 1 && (!request.Class.IsNull() && UtilityFunctions.IsValidInt(request.Class.ClassDescriptionId) && request.Class.ClassDescriptionId > 0)));

            int wcQuoteId = QuoteCookieHelper.Cookie_GetQuoteId(this.ControllerContext.HttpContext);
            if (!UtilityFunctions.IsValidInt(wcQuoteId))
            {
                listOfErrors.Add("QuoteId not found");
            }

            #region Date Validation
            if (!String.IsNullOrWhiteSpace(request.InceptionDate))
            {
                DateTime parsed = DateTime.MinValue;
                bool valid = false;
                try
                {
                    parsed = DateTime.Parse(request.InceptionDate, new System.Globalization.CultureInfo("en-US", true));
                    valid = true;
                }
                catch (Exception ex)
                {
                    BHIC.Common.Logging.LoggingService.Instance.Trace(string.Format("Inception Date value {0}", request.InceptionDate), ex);
                }

                if (valid)
                {
                    string curDate = DateTime.Now.ToString("MM/dd/yyyy");
                    if (!(DateTime.Parse(curDate, new System.Globalization.CultureInfo("en-US", true)) < DateTime.Parse(parsed.ToShortDateString()) && DateTime.Parse(parsed.ToShortDateString()) <= DateTime.Parse(curDate, new System.Globalization.CultureInfo("en-US", true)).AddDays(60)))
                    {
                        listOfErrors.Add(Constants.ExposureDataErrors.INCORRECT_DATE);

                    }
                }
                else
                {
                    listOfErrors.Add(Constants.ExposureDataErrors.INVALID_DATE_FORMAT);
                }
            }
            #endregion Date Validation

            if (request.SelectedSearch == 1)
            {
                if (request.Industry.IsNull())
                {
                    listOfErrors.Add(String.Format("{0}-{1}", Constants.ExposureDataErrors.BUSINESS_INFO_EMPTY, "Industry not found"));
                }
                else if (request.Industry.IndustryId > 0 && request.SubIndustry.IsNull())
                {
                    listOfErrors.Add(String.Format("{0}-{1}", Constants.ExposureDataErrors.BUSINESS_INFO_EMPTY, "SubIndustry not found"));
                }
                else if (request.Industry.IndustryId == -1 && String.IsNullOrWhiteSpace(request.OtherClassDesc))
                {
                    listOfErrors.Add(String.Format("{0}-{1}", Constants.ExposureDataErrors.BUSINESS_INFO_EMPTY, "Industry Selected as other and Other Class description not found"));
                }
                else if (!request.SubIndustry.IsNull() && request.SubIndustry.SubIndustryId > 0 && request.Class.IsNull())
                {
                    listOfErrors.Add(String.Format("{0}-{1}", Constants.ExposureDataErrors.BUSINESS_INFO_EMPTY, "SubIndustry not Selected as other and Class not found"));
                }
                else if (!request.SubIndustry.IsNull() && request.SubIndustry.SubIndustryId == -1 && String.IsNullOrWhiteSpace(request.OtherClassDesc))
                {
                    listOfErrors.Add(String.Format("{0}-{1}", Constants.ExposureDataErrors.BUSINESS_INFO_EMPTY, "SubIndustry Selected as Other and Class description not found"));
                }
                else if (!request.Class.IsNull() && UtilityFunctions.IsValidInt(request.Class.ClassDescriptionId) && request.Class.ClassDescriptionId == 0)
                {
                    listOfErrors.Add(String.Format("{0}-{1}", Constants.ExposureDataErrors.BUSINESS_INFO_EMPTY, "SubIndustry not Selected as Other Class description not selected"));
                }
                else if (!request.Class.IsNull() && request.Class.ClassDescriptionId == -1 && String.IsNullOrWhiteSpace(request.OtherClassDesc))
                {
                    listOfErrors.Add(String.Format("{0}-{1}", Constants.ExposureDataErrors.BUSINESS_INFO_EMPTY, "SubIndustry Selected as Other Class description not found"));
                }
            }
            else
            {
                if (!UtilityFunctions.IsValidInt(request.ClassDescriptionId) || !UtilityFunctions.IsValidInt(request.ClassDescriptionKeywordId))
                {
                    listOfErrors.Add(String.Format("{0}-{1}", Constants.ExposureDataErrors.BUSINESS_INFO_EMPTY, "Search Selected was through keyword but no option selected"));
                }
            }
            if (checkPayroll && request.MinPayrollAllResponseRecieved && !String.IsNullOrWhiteSpace(appSession.QuoteVM.TotalPayroll) && !appSession.QuoteVM.TotalPayroll.Equals(request.TotalPayroll.Trim()))
            {
                listOfErrors.Add("Total Annual employee payroll altered");
            }
            if (checkPayroll && request.MinPayrollAllResponseRecieved && appSession.QuoteVM.MinExpValidationAmount != request.MinExpValidationAmount || appSession.QuoteVM.IsGoodStateApplicable != request.IsGoodStateApplicable)
            {
                listOfErrors.Add("Amount Validation failed");
            }

            if (request.SelectedYearInBusiness.IsNull())
            {
                listOfErrors.Add(Constants.ExposureDataErrors.EMPTY_BUSINESSYEAR);
            }

            if (checkPayroll && request.MinPayrollAllResponseRecieved && !request.IsGoodStateApplicable && !request.CompClassData.IsNull() && request.CompClassData.Count > 0)
            {
                if (request.CompClassData.Any(x => x.IsExposureAmountRequired.IsNull() || (x.IsExposureAmountRequired.Value && Convert.ToInt64(UtilityFunctions.ToNumeric(request.TotalPayroll)) >= 50000 && String.IsNullOrWhiteSpace(x.PayrollAmount) && request.BusinessClassDirectSales != "N")))
                {
                    listOfErrors.Add("Special employee details not fully filled");
                }
                else if (Convert.ToInt64(UtilityFunctions.ToNumeric(request.TotalPayroll)) >= 50000 && request.BusinessClassDirectSales != "N")
                {
                    long companionPayrollSum = 0;
                    foreach (var comp in request.CompClassData)
                    {
                        if (comp.IsExposureAmountRequired.Value)
                        {
                            companionPayrollSum += Convert.ToInt64(UtilityFunctions.ToNumeric(comp.PayrollAmount));
                        }
                    }
                    if (companionPayrollSum > Convert.ToInt64(UtilityFunctions.ToNumeric(request.TotalPayroll)))
                    {
                        listOfErrors.Add("Exposure companion details not Valid, sum of special employee payroll cannot exceed total annual payroll");
                    }
                }
            }

            if (listOfErrors.Count > 0)
            {
                return false;
            }

            #endregion

            return true;
        }

        private bool ValidateExposureAmountOnSubmit(ref QuoteViewModel request)
        {
            CustomSession appSession = GetCustomSession();
            VExposuresMinPayrollResponse exposureAmountResponse;
            if (!request.MinPayrollAllResponseRecieved)
            {
                if (string.IsNullOrWhiteSpace(request.TotalPayroll))
                {
                    request.MinPayrollAllResponseRecieved = false;
                    request.MinExpValidationAmount = 0;
                }
                else if (!UtilityFunctions.IsValidInt(request.ClassDescriptionId) && UtilityFunctions.IsValidInt(request.ClassDescriptionKeywordId))
                {
                    request.MinPayrollAllResponseRecieved = false;
                    request.MinExpValidationAmount = 0;
                }
                else if (request.ClassDescriptionId == -1 || string.IsNullOrWhiteSpace(request.ClassCode) || string.IsNullOrWhiteSpace(request.BusinessClassDirectSales))
                {
                    request.MinExpValidationAmount = 0;
                    request.IsGoodStateApplicable = false;
                }
                else
                {
                    try
                    {
                        IExposureService exposureService = new ExposureService();
                        exposureAmountResponse = exposureService.ValidateMinimumExposureAmount(new VExposuresMinPayrollRequestParms { ExposureAmt = Convert.ToDecimal(UtilityFunctions.ToNumeric(request.TotalPayroll)), ClassDescKeywordId = request.ClassDescriptionKeywordId, ClassDescriptionId = request.ClassDescriptionId }, provider);

                        //Comment : Here amount is not validated for supplied "ClassDescriptionId" then send false with 'MinimumExposure"
                        if (!(exposureAmountResponse.IsNull() && exposureAmountResponse.OperationStatus.IsNull()))
                        {
                            //appSession.QuoteVM.TotalPayroll = exposureAmt.Trim();
                            request.MinExpValidationAmount = exposureAmountResponse.MinimumExposure;
                            request.IsGoodStateApplicable = !exposureAmountResponse.OperationStatus.RequestSuccessful;
                            request.MinPayrollAllResponseRecieved = exposureAmountResponse.OperationStatus.RequestSuccessful;
                            appSession.QuoteVM.MinExpValidationAmount = exposureAmountResponse.MinimumExposure;
                            appSession.QuoteVM.IsGoodStateApplicable = !exposureAmountResponse.OperationStatus.RequestSuccessful;
                            if (request.MinPayrollAllResponseRecieved)
                            {
                                appSession.QuoteVM.TotalPayroll = request.TotalPayroll;
                                appSession.QuoteVM.MinPayrollAllResponseRecieved = request.MinPayrollAllResponseRecieved;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        request.MinPayrollAllResponseRecieved = false;
                        // Changed minimum amount to $25000, as per Neelam with subject FW: Minimum Payroll Testing received on Wed 20-01-2016 20:35
                        request.MinExpValidationAmount = 25000;
                    }
                }
            }
            else
            {
                request.MinExpValidationAmount = appSession.QuoteVM.MinExpValidationAmount;
                request.IsGoodStateApplicable = request.ClassDescriptionId == -1 ? false : appSession.QuoteVM.IsGoodStateApplicable;
                appSession.QuoteVM.IsGoodStateApplicable = request.IsGoodStateApplicable;
                appSession.QuoteVM.TotalPayroll = request.TotalPayroll;
                appSession.QuoteVM.MinPayrollAllResponseRecieved = request.MinPayrollAllResponseRecieved;
                return true;
            }
            //SetCustomSession(appSession);
            return request.MinPayrollAllResponseRecieved;
        }

        #endregion Private Methods

    }
}
