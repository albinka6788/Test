using BHIC.Common;
using BHIC.Common.Caching;
using BHIC.Common.Client;
using BHIC.Common.Mailing;
using BHIC.Contract.Account;
using BHIC.Contract.Background;
using BHIC.Contract.Policy;
using BHIC.Contract.QuestionEngine;
using BHIC.Core;
using BHIC.Core.Account;
using BHIC.Core.Background;
using BHIC.Core.Masters;
using BHIC.Core.Policy;
using BHIC.Core.QuestionEngine;
using BHIC.Domain.Account;
using BHIC.Domain.Background;
using BHIC.Domain.Policy;
using BHIC.Domain.QuestionEngine;
using BHIC.Domain.Service;
using BHIC.Portal.Code.Configuration;
using BHIC.Portal.Code.Quote;
using BHIC.ViewDomain.Landing;
using BHIC.ViewDomain.QuestionEngine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;


namespace BHIC.Portal.Areas.Landing.Controllers
{
    public class QuoteController : BaseController
    {
        #region Variables : Page Level Local Variables Decalration

        // ----------------------------------------
        // class-level declarations
        // ----------------------------------------

        // adding these here for now, because I expect that these values will change as requirements evolve
        private static string LineOfBusiness = "WC";
        private static string Quote_SoftReferral = "Soft Referral";
        private static string Quote_HardReferral = "Hard Referral";

        private IIndustryService industryService;
        private ISubIndustryService subIndustryService;
        private IClassDescriptionService classDescriptionService;
        private IClassDescKeywordService classDescKeywordService;
        private IQuestionService questionService;
        public IUserProfileService userProfileService;
        public IExposureService exposureService;
        private IPolicyDataService policyDataService;
        private IPolicyDetailsService policyDetailsService;
        private IPolicyCreateService policyCreateService;
        private IVCityStateZipCodeService vCityStateZipService;
        ServiceProvider provider = new GuardServiceProvider() { ProviderName = ProviderNames.Guard };
        public static int count = 0;

        #endregion

        #region Constructor : Page Construnctor & Default Initialization

        public QuoteController()
        {
            industryService = new IndustryService();
            subIndustryService = new SubIndustryService();
            classDescriptionService = new ClassDescriptionService();
            classDescKeywordService = new ClassDescKeywordService();
            questionService = new QuestionService();
            userProfileService = new UserProfileService();
            policyDataService = new PolicyDataService(provider);
            policyDetailsService = new PolicyDetailsService(provider);
            policyCreateService = new PolicyCreateService(provider);
            vCityStateZipService = new VCityStateZipCodeService();
            exposureService = new ExposureService();
        }

        #endregion

        #region Methods : Landing/Home Page Default View

        public ActionResult NewIndex()
        {
            //Comment : Here set default landing page html
            TempData["defaultLandingPageHtml"] = @"LandingPages\wc-default-landing-page.html";

            //Comment : Here based on copain encoded url business class get landing page html name and pass it to view for dynamic rendering            
            TempData["landingPageHtml"] = @"LandingPages\2123_landing-page";

            return View("NewIndex");
        }

        public ActionResult GeicoIndex()
        {
            return View();
        }

        // GET: Landing/Quote
        public ActionResult Index(int? campaign, string gclid = "")
        {
            //Comment : Here generate Quote page index view based on cookies value
            var landingViewModel = LandingViewData(this.ControllerContext.HttpContext, campaign, gclid);

            return View();
        }

        public WcLandingViewModel LandingViewData(HttpContextBase context, int? campaign, string gclid)
        {
            // ----------------------------------------
            // variables / initialization
            // ----------------------------------------
            IQuoteStatusService quoteStatusService = new QuoteStatusService();
            WcLandingViewModel landingViewModel = new WcLandingViewModel();
            string AdId = "";

            #region Comment : Here Get the quote currently in progress if available, otherwise, create a new one

            // stored in a cookie on the user's machine
            int wcQuoteId = QuoteCookieHelper.Cookie_GetQuoteId(context);

            #endregion

            #region Comment : Here If Any Active Quote Exists Then Populate Filled Index View

            // ----------------------------------------
            // present the user with the specified quote, if it's available
            // ----------------------------------------
            if (wcQuoteId != 0)
            {
                //get exisitng saved Quote details
                string existingQuote = string.Empty;

                // ********** get the existing Quote details for re-popullation purpose
                var getOperationStatus = quoteStatusService.GetQuoteStatus(new QuoteStatusRequestParms { QuoteId = wcQuoteId }, provider);

                //Comment : Here if request successful then get new QuoteId
                if (getOperationStatus.QuoteId != null)
                {
                    //Comment : Here use this information to return cached view
                    //based on stage show move user to specified view/page
                    //DateTime LandingSaved = (DateTime)getOperationStatus.LandingSaved;

                    //Comment : Here return populated view
                }

                return landingViewModel;
            }

            #endregion

            #region Comment : Here Create New Quote

            // ----------------------------------------
            // if we made it this far, there's no active quote, create one
            // ----------------------------------------

            // ********** check for google adword id
            // NOTE; at the time this was authored, approach wasn't 100% clear; the id may be in the query string, or it may be in the referrer url

            // first, see if it was passed as a parameter in the request's querystring
            if (!string.IsNullOrEmpty(gclid))
            {
                AdId = gclid;
            }
            // if it's not found in the querystring, try the referrer
            else if (context.Request.UrlReferrer != null)
            {
                try
                {
                    var q = HttpUtility.ParseQueryString(context.Request.UrlReferrer.Query);
                    AdId = q["gclid"];
                }
                catch
                {
                    // swallow the exception; parameter not available; means the request is not associated with google adwords
                }
            }

            // ********** get the user's IP address
            string userIP = GetUser_IP(context);

            // ********** create the quote
            //landingViewModel.QuoteViewModel = CreateDsQuote(true, (User != null) ? User.Identity.Name : "", AdId, userIP);
            //var postOperationStatus = SvcClient.CallService<QuoteStatus, OperationStatus>("QuoteStatus", "POST", new QuoteStatus { AdId = null, EnteredOn = DateTime.Now, UserIP = userIP });

            var postOperationStatus = quoteStatusService.AddQuoteStatus(new QuoteStatus { AdId = null, EnteredOn = DateTime.Now, UserIP = userIP }, provider);

            //Comment : Here if request successful then get new QuoteId
            if (postOperationStatus.RequestSuccessful)
            {
                //Comment : Here get QuoteId from returned effected id
                var effectedQuoteDTO = postOperationStatus.AffectedIds
                    .SingleOrDefault(res => res.DTOProperty == "QuoteId");

                //Comment : Here get retuned effected id/QuoteId
                wcQuoteId = Convert.ToInt32(effectedQuoteDTO.IdValue);
            }

            #endregion

            #region Comment : Here Update/Save New QuoteId Into Cookies

            //Comment : Here save this QuoteId in user cookies for later use
            QuoteCookieHelper.Cookie_SaveQuoteId(context, wcQuoteId);			// save the quote id off in a cookie

            #endregion

            landingViewModel.IsPopulated = false;

            return landingViewModel;
        }


        /// <summary>
        /// Return exposure data based on supplied QuoteId
        /// </summary>
        /// <param name="wcQuoteViewModel"></param>
        /// <returns></returns>
        ///
        [HttpGet]
        public JsonResult GetExposures()
        {

            //Comment : Here if response contains data
            bool isLandingSaved = false;
            Exposure exposure = null;
            PolicyData policyData = null;
            string policyInceptionDate = null;

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

                    // flag that determines how to respond after processing the response received
                    bool success = true;

                    // deserialize each response returned by the service, and copy the OperationStatus to the view model for demo purposes
                    OperationStatus operationStatus = new OperationStatus();

                    #endregion

                    #region Comment : Here get Exposure and PolicyData to maintain in session for Pre-Popullation

                    var policyDataGetAction = new BatchAction { ServiceName = "PolicyData", ServiceMethod = "GET", RequestIdentifier = "PolicyData" };
                    policyDataGetAction.BatchActionParameters.Add(
                        new BatchActionParameter { Name = "policyDataRequestParms", Value = JsonConvert.SerializeObject(new PolicyDataRequestParms { QuoteId = wcQuoteId }) });
                    batchActionList.BatchActions.Add(policyDataGetAction);

                    var exposureGetAction = new BatchAction { ServiceName = "Exposures", ServiceMethod = "GET", RequestIdentifier = "Exposures" };
                    exposureGetAction.BatchActionParameters.Add(
                        new BatchActionParameter { Name = "exposureRequestParms", Value = JsonConvert.SerializeObject(new ExposureRequestParms { QuoteId = wcQuoteId }) });
                    batchActionList.BatchActions.Add(exposureGetAction);

                    jsonResponse = SvcClientOld.CallServiceBatch(batchActionList);

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
                                //Comment : Here this session will be used to get this infor mation to set on other pages in Purchase process flow
                                Session["LastSavedPolicyData"] = policyData;
                                policyInceptionDate = Convert.ToDateTime(policyData.InceptionDate).ToShortDateString();
                                string policyOrMgaCode = policyData.MgaCode == "" ? "" : policyData.MgaCode;
                                Session["policyOrMgaCode"] = policyOrMgaCode;
                            }
                        }
                        #endregion

                        #region Comment : Here get Exposure

                        var exposureResponse = batchResponseList.BatchResponses.SingleOrDefault(m => m.RequestIdentifier == "Exposures").JsonResponse;

                        if (exposureResponse != null)
                        {
                            var listOfExposures = JsonConvert.DeserializeObject<ExposureResponse>(exposureResponse).Exposures;
                            if (listOfExposures.Count > 0)
                            {
                                exposure = listOfExposures.OrderByDescending(m => m.ExposureId).ElementAt(0);

                                //Comment : Here if Exposures exists for this/Cookies QuoteId then set it to TRUE means user has saved LandingPage data earlier
                                if (exposure != null && exposure.ExposureId != null)
                                {
                                    isLandingSaved = true;

                                    //Comment : Here this session will be used to get this infor mation to set on other pages in Purchase process flow
                                    Session["LastSavedExposure"] = exposure;
                                }
                            }
                        }
                        #endregion
                    }

                    #endregion

                    #endregion
                }
            }
            catch (Exception)
            {

            }

            return Json(new { success = true, isLandingSaved = isLandingSaved, exposureModel = exposure, policyModel = policyInceptionDate }, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Methods : Home Page Data Submission

        /// <summary>
        /// SaveLandingPageData
        /// Takes as input Exposure Data from front End
        /// </summary>
        /// <returns></returns>
        //public JsonResult SaveLandingPageData(QuotePageViewModel request)
        //{
        //    //Comment : Here first of all check for QuoteId if does not exist then redirect user back to "Index/Landing" view
        //    int wcQuoteId = QuoteCookieHelper.Cookie_GetQuoteId(this.ControllerContext.HttpContext);
        //    Session["ExposureDate"] = Convert.ToDateTime(request.InceptionDate).ToShortDateString();
        //    #region Comment : Here Throw exception which will redirect user ro back to landing page

        //    // if there's no active quote (e.g. - if the session expired), redirect back to the landing page
        //    if (wcQuoteId == 0)
        //    {
        //        throw new ApplicationException("user session has been expired, could not post exposure data !");
        //    }
        //    //if neither "KeywordSearched" nor "IndustrySelection" supplied in POST request then some issue on UI, return back to index view
        //    else if (request.ClassDescriptionKeywordId == null && request.IndustryId == null)
        //    {
        //        throw new ApplicationException("Improper savelanding page data post request !");
        //    }

        //    #endregion

        //    #region Comment : Here get user last exposure details if exists in Session to avoid Guard API call overhead otherwise call "Exposure" and re-assign session for later usage

        //    Exposure lastSavedExposure = null;
        //    PolicyData lastSavedPolicyData = null;

        //    if (Session["LastSavedExposure"] != null)
        //    {
        //        lastSavedExposure = Session["LastSavedExposure"] as Exposure;
        //        lastSavedPolicyData = Session["LastSavedPolicyData"] as PolicyData;
        //    }
        //    else
        //    {
        //        //Comment : Here call getExposure method which will create and assign last saved user exposure 
        //        GetExposures();
        //        lastSavedExposure = Session["LastSavedExposure"] as Exposure;
        //        lastSavedPolicyData = Session["LastSavedPolicyData"] as PolicyData;
        //    }

        //    //Comment : Here If lastexposure found then match it with current posted data inf there is any data attribute change then create new exposure, that will generate new Questionnaire on next page
        //    //Otherwise just redirect user to Question Page with auto filled(if user has submitted Question page earlier), no need to post 
        //    if
        //    (
        //        lastSavedExposure != null &&
        //        (
        //        //if last exposure was "KeywordSearched" then match with current exposure "KeywordSearched" data attributes
        //            (
        //                (lastSavedExposure.ClassDescriptionKeywordId != null && request.ClassDescriptionKeywordId != null)
        //                &&
        //                (lastSavedExposure.ClassDescriptionKeywordId == request.ClassDescriptionKeywordId)
        //            )
        //            ||
        //        //if last exposure was "IndustrySelection" then match with current exposure "IndustrySelection" data attributes
        //            (
        //                (lastSavedExposure.IndustryId != null && request.IndustryId != null)
        //                &&
        //                (lastSavedExposure.IndustryId == request.IndustryId)
        //                &&
        //        //then check for other exposure data attributes in "IndustrySelection" path
        //                (
        //                    (lastSavedExposure.SubIndustryId == request.SubIndustryId) && (lastSavedExposure.ClassDescriptionId == request.ClassDescriptionId)
        //                )
        //            )
        //        )
        //        &&
        //        //then check for other exposure and policy data attributes
        //        (
        //            (lastSavedExposure.ZipCode == request.ZipCode)
        //            &&
        //        //Policy data attributes
        //            (lastSavedPolicyData.InceptionDate.Value.ToShortDateString() == request.InceptionDate.Value.ToShortDateString())
        //        )
        //    )
        //    {
        //        #region Comment : Here Update/Save New QuoteId Into Cookies

        //        //Comment : Here save this QuoteId in user cookies for later use
        //        QuoteCookieHelper.Cookie_SaveQuoteId(ControllerContext.HttpContext, wcQuoteId);			// save the quote id off in a cookie

        //        #endregion

        //        var Url = "/Landing/Quote/QuestionsGet";
        //        return Json(Url, JsonRequestBehavior.AllowGet);
        //    }

        //    #endregion

        //    #region Comment : Here create exposure object

        //    var exposure = new Exposure();

        //    if (request.ClassDescriptionKeywordId != null)
        //        exposure.ClassDescriptionKeywordId = request.ClassDescriptionKeywordId;
        //    else
        //    {
        //        exposure.IndustryId = request.IndustryId;
        //        exposure.SubIndustryId = request.SubIndustryId;
        //        exposure.ClassDescriptionId = request.ClassDescriptionId;
        //    }

        //    exposure.QuoteId = wcQuoteId;
        //    exposure.LOB = LineOfBusiness;
        //    exposure.ExposureAmt = request.ExposureAmt;
        //    exposure.ZipCode = request.ZipCode;
        //    exposure.ExposureId = null;

        //    #endregion

        //    #region Comment : Here create policy data object

        //    var policyData = new PolicyData();
        //    policyData.QuoteId = wcQuoteId;
        //    policyData.InceptionDate = request.InceptionDate;

        //    //Comment : Here this session will be used to get this infor mation to set on other pages in Purchase process flow


        //    #endregion

        //    #region Comment : Here create BatchAction list object

        //    /*----------------------------------------
        //        populate a BatchActionList that contains all requests to be sent to the Gaurd API
        //    ----------------------------------------*/

        //    var batchActionList = new BatchActionList();	// BatchActionList: list of actions routed to the Insurance Service for response
        //    BatchResponseList batchResponseList;			// BatchResponseList: list of responses returned from the Insurance Service
        //    string jsonResponse;							// JSON-formated response data returned from the Insurance Service

        //    // flag that determines how to respond after processing the response received
        //    bool success = true;

        //    // deserialize each response returned by the service, and copy the OperationStatus to the view model for demo purposes
        //    OperationStatus operationStatus = new OperationStatus();

        //    #endregion

        //    #region Comment : Here If exists then get PolicyData and Latest Exposure for this QuoteId for "CREATE or UPDATE" batch operation type decision

        //    int? policyDataId = null, exposureId = null;
        //    try
        //    {
        //        #region Comment : Here Batch execution to get 3rd level of checking for reduce excpetion chances in "CREATE/UPDATE" request in next step batch execution

        //        //Comment : Here must check if both session exists then don't call following two calls
        //        if (lastSavedPolicyData == null)
        //        {
        //            var policyDataIdGetAction = new BatchAction { ServiceName = "PolicyData", ServiceMethod = "GET", RequestIdentifier = "Get PolicyDataId" };
        //            policyDataIdGetAction.BatchActionParameters.Add(new BatchActionParameter { Name = "policyDataRequestParms", Value = JsonConvert.SerializeObject(new PolicyDataRequestParms { QuoteId = wcQuoteId }) });
        //            batchActionList.BatchActions.Add(policyDataIdGetAction);
        //        }
        //        else
        //        {
        //            policyDataId = lastSavedPolicyData.PolicyDataId;
        //        }

        //        if (lastSavedExposure == null)
        //        {
        //            var exposureIdGetAction = new BatchAction { ServiceName = "Exposures", ServiceMethod = "GET", RequestIdentifier = "Get ExposureId" };
        //            exposureIdGetAction.BatchActionParameters.Add(new BatchActionParameter { Name = "exposureRequestParms", Value = JsonConvert.SerializeObject(new ExposureRequestParms { QuoteId = wcQuoteId }) });
        //            batchActionList.BatchActions.Add(exposureIdGetAction);
        //        }
        //        else
        //        {
        //            exposureId = lastSavedExposure.ExposureId;
        //        }

        //        #region If there is any action item in list

        //        if (batchActionList != null && batchActionList.BatchActions.Count > 0)
        //        {

        //            jsonResponse = SvcClientOld.CallServiceBatch(batchActionList);

        //            // flag that determines how to respond after processing the response received
        //            success = true;

        //            // deserialize the results into a BatchResponseList
        //            batchResponseList = JsonConvert.DeserializeObject<BatchResponseList>(jsonResponse);

        //            // deserialize each response returned by the service, and copy the OperationStatus to the view model for demo purposes
        //            operationStatus = new OperationStatus();
        //            foreach (var batchResponse in batchResponseList.BatchResponses)
        //            {
        //                // this flag tested later to determine whether to proceed to the next view or not
        //                if (!batchResponse.RequestSuccessful) { success = false; }
        //            }

        //            #region Comment : Here get ExposureId and get PolicyDataId

        //            if (batchResponseList != null)
        //            {
        //                #region Comment : Here get PolicyDataId
        //                var policyDataResponse = batchResponseList.BatchResponses.SingleOrDefault(m => m.RequestIdentifier == "Get PolicyDataId").JsonResponse;

        //                if (policyDataResponse != null)
        //                {
        //                    policyDataId = JsonConvert.DeserializeObject<PolicyDataResponse>(policyDataResponse).PolicyData.PolicyDataId;
        //                }
        //                #endregion

        //                #region Comment : Here get ExposureId
        //                var exposureResponse = batchResponseList.BatchResponses.SingleOrDefault(m => m.RequestIdentifier == "Get ExposureId").JsonResponse;

        //                if (exposureResponse != null)
        //                {
        //                    var listOfExposures = JsonConvert.DeserializeObject<ExposureResponse>(exposureResponse).Exposures;
        //                    if (listOfExposures.Count > 0)
        //                        exposureId = listOfExposures.OrderByDescending(m => m.ExposureId).ElementAt(0).ExposureId;
        //                }
        //                #endregion
        //            }

        //            #endregion

        //        }

        //        #endregion

        //        #endregion
        //    }
        //    catch (Exception)
        //    {
        //    }
        //    finally
        //    {
        //        //Comment : Here Reset batchaction list
        //        batchActionList = new BatchActionList();
        //        batchResponseList = null;
        //        jsonResponse = string.Empty;

        //        //Comment : Here set retrieved Ids into batch actions
        //        policyData.PolicyDataId = policyDataId;
        //        exposure.ExposureId = exposureId;
        //    }

        //    #endregion

        //    #region Comment : Here Service call batch of actions creation

        //    // populate a BatchAction that will be used to submit the Exposure DTO to the Insurance Service
        //    var exposuresAction = new BatchAction { ServiceName = "Exposures", ServiceMethod = "POST", RequestIdentifier = "Exposure Data" };
        //    exposuresAction.BatchActionParameters.Add(new BatchActionParameter { Name = "exposure", Value = JsonConvert.SerializeObject(exposure) });
        //    batchActionList.BatchActions.Add(exposuresAction);

        //    // populate a BatchAction that will be used to submit the PolicyData DTO to the Insurance Service
        //    var policyDataAction = new BatchAction { ServiceName = "PolicyData", ServiceMethod = "POST", RequestIdentifier = "Policy Data" };
        //    policyDataAction.BatchActionParameters.Add(new BatchActionParameter { Name = "policyData", Value = JsonConvert.SerializeObject(policyData) });
        //    batchActionList.BatchActions.Add(policyDataAction);

        //    #endregion

        //    #region Comment : Here Service call and response handling

        //    // submit the BatchActionList to the Insurance Service
        //    jsonResponse = SvcClientOld.CallServiceBatch(batchActionList);

        //    // flag that determines how to respond after processing the response received
        //    success = true;

        //    // deserialize the results into a BatchResponseList
        //    batchResponseList = JsonConvert.DeserializeObject<BatchResponseList>(jsonResponse);

        //    // deserialize each response returned by the service, and copy the OperationStatus to the view model for demo purposes
        //    operationStatus = new OperationStatus();
        //    foreach (var batchResponse in batchResponseList.BatchResponses)
        //    {
        //        // this flag tested later to determine whether to proceed to the next view or not
        //        if (!batchResponse.RequestSuccessful) { success = false; }
        //    }

        //    #endregion

        //    #region Comment : Here retuning reposne to user

        //    // TODO: Do something with the response
        //    if (!success)
        //    {
        //        throw new ApplicationException("Unable to post exposure and policy data");
        //    }
        //    else
        //    {
        //        #region Comment : Here get ExposureId for newly created Exposure and sore that into session object

        //        if (batchResponseList != null)
        //        {
        //            //Comment : Here set exposureId into session
        //            var exposureResponse = batchResponseList.BatchResponses.SingleOrDefault(m => m.RequestIdentifier == "Exposure Data").JsonResponse;

        //            if (exposureResponse != null)
        //            {
        //                var operationStatusDeserialized = JsonConvert.DeserializeObject<OperationStatus>(exposureResponse);
        //                if (operationStatusDeserialized != null)
        //                    Session["ExposureId"] = operationStatusDeserialized.AffectedIds.SingleOrDefault(m => m.DTOProperty == "ExposureId").IdValue;
        //            }
        //        }

        //        #endregion

        //        #region Comment : Here Update/Save New QuoteId Into Cookies

        //        //Comment : Here save this QuoteId in user cookies for later use
        //        QuoteCookieHelper.Cookie_SaveQuoteId(ControllerContext.HttpContext, wcQuoteId);			// save the quote id off in a cookie

        //        #endregion

        //        var Url = "/Landing/Quote/QuestionsGet";
        //        return Json(Url, JsonRequestBehavior.AllowGet);
        //    }

        //    #endregion

        //}

        #endregion

        #region Methods : Home Page Controls Binding Like Industries,etc.

        /// <summary>
        /// This method gets all the Industries 
        /// </summary>
        /// <returns></returns>
        public JsonResult GetIndustries()
        {
            var industryList = industryService.GetIndustryList(new IndustryRequestParms { Lob = LineOfBusiness }, provider);

            return Json(industryList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This method gets all the Sub-Industries based on Industry Id 
        /// </summary>
        /// <param name="industryId"></param>
        /// <returns></returns>
        public JsonResult GetSubIndustries(int industryId)
        {
            var subIndustryList = subIndustryService.GetSubIndustryList(new SubIndustryRequestParms { IndustryId = industryId }, provider);

            return Json(subIndustryList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This method gets all the Classes(business) based on subIndustryId
        /// </summary>
        /// <param name="subIndustryId"></param>
        /// <returns></returns>
        public JsonResult GetClassDescriptions(int subIndustryId)
        {
            var classDescriptionList = classDescriptionService.GetClassDescriptionList(new ClassDescriptionRequestParms { IncludeRelated = true, SubIndustryId = subIndustryId }, provider);

            return Json(classDescriptionList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This method searches business on the search string and classDescriptionKeywordId
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="classDescriptionId"></param>
        /// <returns></returns>
        public JsonResult SearchBusiness(string searchString, int? classDescriptionId)
        {
            var provider = new GuardServiceProvider() { ServiceCategory = LineOfBusiness };
            var businessSearchResultList = classDescKeywordService.GetClassDescKeywordList(
                new ClassDescKeywordRequestParms { LOB = LineOfBusiness, SearchString = searchString, ClassDescKeywordId = classDescriptionId }, provider);

            return Json(businessSearchResultList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Validates the minimum exposure amount
        /// </summary>
        /// <param name="exposureAmt"></param>
        /// <param name="classDescriptionId"></param>
        /// <param name="classDescKeywordId"></param>
        /// <returns></returns>
        public JsonResult ValidateExposureAmount(string exposureAmt, int? classDescriptionId, int? classDescKeywordId)
        {
            VExposuresMinPayrollResponse exposureAmountResponse;
            string resultStatus = string.Empty;
            string resultText = string.Empty;
            string resultMinAmount = string.Empty;
            try
            {
                exposureAmountResponse = exposureService.ValidateMinimumExposureAmount(new VExposuresMinPayrollRequestParms { ExposureAmt = Convert.ToDecimal(exposureAmt), ClassDescKeywordId = classDescKeywordId, ClassDescriptionId = classDescriptionId }, provider);

                //Comment : Here amount is not validated for supplied "ClassDescriptionId" then send "NOK" with 'MinimumExposure"
                if (!(UtilityFunctions.IsNull(exposureAmountResponse) && UtilityFunctions.IsNull(exposureAmountResponse.OperationStatus)))
                {
                    if (exposureAmountResponse.OperationStatus.RequestSuccessful)
                    {
                        resultStatus = "OK";
                        resultText = "";
                        resultMinAmount = Convert.ToString(exposureAmountResponse.MinimumExposure);

                    }
                    else if (!exposureAmountResponse.OperationStatus.RequestSuccessful)
                    {
                        resultStatus = "NOK";
                        resultText = exposureAmountResponse.OperationStatus.Messages[0].Text;
                        resultMinAmount = Convert.ToString(exposureAmountResponse.MinimumExposure);
                    }
                }
            }
            catch (Exception ex)
            {
                resultStatus = "NOK";
                resultText = ex.Message;
                resultMinAmount = "$15,000";
            }

            return Json(new { resultStatus = resultStatus, resultText = resultText, resultMinAmount = resultMinAmount }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Methods : Question Page Process Flow

        /// <summary>
        /// Return QuestionnaireView to generate quote details
        /// </summary>
        /// <returns></returns>
        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult QuestionsGet()
        {
            return View("QuestionsGet");
        }

        /// <summary>
        /// This method gets list of the Questions based exposure data & policy data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetQuestions()
        {
            // stored in a cookie on the user's machine
            int wcQuoteId = QuoteCookieHelper.Cookie_GetQuoteId(this.ControllerContext.HttpContext);

            //Comment : Here if response contains data
            string questionsJson = string.Empty;

            if (wcQuoteId != 0)
            {
                var questionsResponse = questionService.GetQuestionList(new QuestionRequestParms { QuoteId = wcQuoteId });

                if (questionsResponse.OperationStatus.RequestSuccessful)
                {
                    questionsJson = JsonConvert.SerializeObject(questionsResponse.Questions);
                }
            }

            return Json(questionsJson, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This method will submit Questionnaire to get Quote related information
        /// </summary>
        /// <param name="questionsList"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PostQuestionResponse(List<Question> questionsList)
        {
            if (questionsList != null)
            {
                // stored in a cookie on the user's machine

                int wcQuoteId = QuoteCookieHelper.Cookie_GetQuoteId(this.ControllerContext.HttpContext);

                if (wcQuoteId != 0)
                {
                    var questionsResponse = questionService.GetQuestionList(new QuestionRequestParms { QuoteId = wcQuoteId });

                    //Comment : Here if request successful then get new QuoteId
                    if (questionsResponse.OperationStatus.RequestSuccessful)
                    {
                        //Comment : Here update question list object 

                        questionsResponse.Questions = questionsList;
                        //questionService.PostQuestionResponse(new QuestionsResponse { Questions = questionsResponse.Questions });
                        questionsResponse = SvcClientOld.CallService<QuestionsResponse, QuestionsResponse>("Questions", "POST", questionsResponse);

                        //Comment : Here if request successful then get new QuoteId
                        if (questionsResponse.OperationStatus.RequestSuccessful)
                        {
                            #region Comment : Here on POST Questions success get Premium and InstallmentFee in session object for next page(Quote) usage

                            try
                            {
                                Session["InstallmentFee"] = questionsResponse.PerInstallmentCharge;
                                Session["PremiumAmt"] = questionsResponse.Premium;
                                Session["QuoteRefNo"] = wcQuoteId.ToString();
                                Session["QuoteStatus"] = questionsResponse.QuoteStatus;
                                Session["Agency"] = questionsResponse.QuestionsRequest.Agency;
                                Session["Carrrier"] = questionsResponse.QuestionsRequest.Carrier;
                                Session["ZipCode"] = questionsResponse.QuestionsRequest.ClassItems.FirstOrDefault().ZipCode;
                                Session["State"] = questionsResponse.QuestionsRequest.ClassItems.FirstOrDefault().StateAbbr;
                            }
                            catch (Exception)
                            {
                                throw;
                            }

                            #endregion

                            // ----------------------------------------
                            // return the view required by the decision / results above
                            // ----------------------------------------                            

                            // ***** questions ok: successful quote - user is presented with a payment information page
                            if (questionsResponse.QuoteStatus == "Quote")
                            {
                                // return the quote results
                                return Json(new { resultStatus = "OK", resultText = "Quote" }, JsonRequestBehavior.AllowGet);
                            }

                            // ***** questions result in SOFT REFERRAL (no policy will be added to GUARD systems; end user sees same referral page as above; email sent to designated GUARD resource) 
                            else if (questionsResponse.QuoteStatus == "Soft Referral")
                            {
                                return Json(new { resultStatus = "OK", resultText = "Soft Referral" }, JsonRequestBehavior.AllowGet);
                            }

                            // ***** questions result in HARD REFERRAL (user is presented with a referral page; policy needs to get added to GUARD systems) 
                            else if (questionsResponse.QuoteStatus == "Hard Referral")
                            {
                                return Json(new { resultStatus = "OK", resultText = "Hard Referral" }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                            return Json(new { resultStatus = "NOK" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }

            return Json("GetQuestions", JsonRequestBehavior.AllowGet);
        }

        #region Get Quote Process Flow (Which Includes Referral Process Flow)

        #region Referral Process Flow

        /// <summary>
        /// Return list of all ViewModel error validation messages
        /// </summary>
        /// <param name="wcQuoteViewModel"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetReferralValidations(WcQuoteViewModel wcQuoteViewModel)
        {
            var allErrors = this.GetModelAllErrors(ModelState);

            return Json(new { success = true, errors = allErrors }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Return list of all business types
        /// </summary>
        /// <param name="wcQuoteViewModel"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetBusinessTypes()
        {
            var businessTypeResponse = //SvcClient.CallService<BusinessTypeResponse>("BusinessTypes");
            SvcClientOld.CallService<BusinessTypeRequestParms, BusinessTypeResponse>("BusinessTypes", new BusinessTypeRequestParms { InsuredNameTypesOnly = true });

            if (businessTypeResponse != null && businessTypeResponse.OperationStatus.RequestSuccessful)
            {
                return Json(new { success = true, businessTypes = businessTypeResponse.BusinessTypes }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = false, businessTypes = "" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This action is called if the user receives a decline indicator from the question engine (the quote will be treated as a potential referral, and imported to GUARD systems)
        /// </summary>
        /// <returns></returns>
        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        //Instruct the browser to not cache the view returned by this action.  Reason: if the user clicks the back button to get to the view, 
        //the data will reload (which ensures that data altered since the last get to this view is rendered)
        public ActionResult ResultsReferQGet()
        {
            // ----------------------------------------
            // get the quote currently in progress
            // ----------------------------------------
            var wcQuoteViewModel = new WcQuoteViewModel();

            // stored in a cookie on the user's machine
            int wcQuoteId = QuoteCookieHelper.Cookie_GetQuoteId(this.ControllerContext.HttpContext);

            // if there's no active quote (e.g. - if the session expired), redirect back to the landing page
            if (wcQuoteId == 0)
            {
                return RedirectToAction("Index", wcQuoteViewModel);
            }

            // ----------------------------------------
            // if the user is authenticated, default their current contact info
            // ----------------------------------------

            // ----------------------------------------
            // return the populated view


            // ----------------------------------------
            if (Session["State"] != null || Session["ZipCode"] != null)
            {
                wcQuoteViewModel.MailState = Convert.ToString(Session["State"]);
                wcQuoteViewModel.MailZip = Convert.ToString(Session["ZipCode"]);
            }
            else
            {
                var exposureResponse = SvcClientOld.CallService<ExposureResponse>("Exposures?QuoteId=" + wcQuoteId);

                if (exposureResponse != null && exposureResponse.OperationStatus.RequestSuccessful)
                {

                    wcQuoteViewModel.MailState = exposureResponse.Exposures.ElementAtOrDefault(0).StateAbbr;
                    wcQuoteViewModel.MailZip = exposureResponse.Exposures.ElementAtOrDefault(0).ZipCode;
                }
            }

            return View("ResultsReferQ", wcQuoteViewModel);
        }

        // this get action added for those users that accidentally force browse to or bookmark a post action of the same name; it will redirect the user to associated get action.
        public ActionResult ResultsReferQPost()
        {
            return RedirectToAction("ResultsReferQGet");
        }

        /// <summary>
        /// Submit/collect contact information here, so we can contact the user (this view will be shown in case quote details can be generated based on User questionnaire response)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public JsonResult ResultsReferQPost(WcQuoteViewModel wcQuoteViewModel)
        {
            if (ModelState.IsValid || !ModelState.IsValid)
            {
                // stored in a cookie on the user's machine
                int wcQuoteId = QuoteCookieHelper.Cookie_GetQuoteId(this.ControllerContext.HttpContext);

                if (wcQuoteId != 0)
                {
                    #region Comment : Here collect all posted information into targeted DTOs

                    //Comment : here collect "Business Information"
                    var insuredNameData = new InsuredName();
                    insuredNameData.QuoteId = wcQuoteId;
                    insuredNameData.NameType = wcQuoteViewModel.BusinessType;
                    insuredNameData.Name = wcQuoteViewModel.BusinessName;

                    //Comment : here collect "Business Contact Information"
                    var userProfileData = new UserProfile();
                    userProfileData.Email = wcQuoteViewModel.Email;
                    userProfileData.FirstName = wcQuoteViewModel.ContactFirstName;
                    userProfileData.LastName = wcQuoteViewModel.ContactLastName;
                    userProfileData.PhoneNumber = wcQuoteViewModel.ContactPhone;
                    userProfileData.Password = "prem@123";
                    userProfileData.NewPassword = "prem@123";
                    userProfileData.UserProfilePostType = UserProfilePostType.CreateUserProfile;

                    //Comment : here collect "Business Mailing Address"
                    var addressData = new Address();
                    addressData.Addr1 = wcQuoteViewModel.MailAddr1;
                    addressData.Addr2 = wcQuoteViewModel.MailAddr2;
                    addressData.State = wcQuoteViewModel.MailState;
                    addressData.City = wcQuoteViewModel.MailCity;
                    addressData.Zip = wcQuoteViewModel.MailZip;

                    #endregion

                    /*
                     First get contact-id for these details which will be used to POST addresses API
                     */
                    #region Post Conatct details and get contact id for further use

                    int contactId = 0;
                    var postOperationStatus = SvcClientOld.CallService<Contact, OperationStatus>("Contacts", "POST", new Contact
                    {
                        QuoteId = wcQuoteId,
                        Name = string.Concat(wcQuoteViewModel.ContactFirstName, " ", wcQuoteViewModel.ContactLastName),
                        Company = wcQuoteViewModel.BusinessName,
                        ContactType = "Misc",
                        Email = wcQuoteViewModel.Email
                        //,Phones = new List<Phone> { new Phone { PhoneNumber = wcQuoteViewModel.ContactPhone, PhoneType = "Business" } }
                    });

                    //Comment : Here if able to post conatct details then get operation id
                    if (postOperationStatus.RequestSuccessful)
                    {
                        contactId = int.Parse(postOperationStatus.AffectedIds.ElementAtOrDefault(0).IdValue);

                        //Comment : Here update conatct id into address DTO object
                        addressData.ContactId = contactId;
                    }

                    #endregion

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

                    #region Comment : Here if conatct id found then only include Address POST API into batch statement

                    if (contactId > 0)
                    {
                        // populate a BatchAction that will be used to submit the UserProfile DTO to the Insurance Service
                        var addressesAction = new BatchAction { ServiceName = "Addresses", ServiceMethod = "POST", RequestIdentifier = "Addresses Data" };
                        addressesAction.BatchActionParameters.Add(new BatchActionParameter { Name = "address", Value = JsonConvert.SerializeObject(addressData) });
                        batchActionList.BatchActions.Add(addressesAction);
                    }

                    // populate a BatchAction that will be used to submit the UserProfile DTO to the Insurance Service
                    /*var userProfileAction = new BatchAction { ServiceName = "UserProfiles", ServiceMethod = "POST", RequestIdentifier = "UserProfiles Data" };
                    userProfileAction.BatchActionParameters.Add(new BatchActionParameter { Name = "userProfile", Value = JsonConvert.SerializeObject(userProfileData) });
                    batchActionList.BatchActions.Add(userProfileAction);*/

                    #endregion

                    // submit the BatchActionList to the Insurance Service
                    jsonResponse = SvcClientOld.CallServiceBatch(batchActionList);

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

                    // TODO: Do something with the response
                    if (!success)
                    {
                        //throw new ApplicationException("Unable to post referral contact details");
                        return Json(new { resultStatus = "NOK", resultText = "Unable to post referral contact details" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {

                        #region Comment : Here Update/Save New QuoteId Into Cookies

                        //Comment : Here save this QuoteId in user cookies for later use
                        QuoteCookieHelper.Cookie_SaveQuoteId(ControllerContext.HttpContext, wcQuoteId);			// save the quote id off in a cookie

                        #endregion


                        #region Mail Sending Code

                        // ********** SOFT REFERAL -no MGA code created due to question/decision engine results; send email to GUARD

                        if (Session["QuoteStatus"].ToString().Equals(Quote_SoftReferral))
                        {
                            ProcessSoftReferral(wcQuoteViewModel);
                        }

                        // ********** HARD REFERRAL - create a presubmission, send email to GUARD

                        else if (Session["QuoteStatus"].ToString().Equals(Quote_HardReferral))
                        {
                            ProcessHardReferral(wcQuoteViewModel);
                        }

                        #endregion
                        return Json(new { resultStatus = "OK", resultText = "Quote referral details saved successfully" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }

            return Json("ResultsReferQGet", JsonRequestBehavior.AllowGet);
        }

        public ActionResult ResultsReferQResponse()
        {
            // ----------------------------------------
            // get the quote currently in progress
            // ----------------------------------------

            // stored in a cookie on the user's machine
            int dsQuoteId = QuoteCookieHelper.Cookie_GetQuoteId(this.ControllerContext.HttpContext);

            // if there's no active quote (e.g. - if the session expired), redirect back to the landing page
            if (dsQuoteId == 0)
            {
                return RedirectToAction("Index");
            }

            // ----------------------------------------
            // if we got this far, it means that a referral was submitted; make the user start over, to help supress excessive referrals for the same user
            // ----------------------------------------

            QuoteCookieHelper.Cookie_DeleteQuoteId(this.ControllerContext.HttpContext);

            // ----------------------------------------
            // return the populated view
            // ----------------------------------------

            return View("ResultsReferQResponse");
        }

        #endregion

        #region Get Quote Information Process Flow

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //[OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]	// Instruct the browser to not cache the view returned by this action.  Reason: if the user clicks the back button to get to the view, the data will reload (which ensures that data altered since the last get to this view is rendered)
        public ActionResult ResultsQuoteGet()
        {
            #region Commented Code for API integration


            //// ----------------------------------------
            //// get cached variables
            //// ----------------------------------------

            //var systemVariables = this.HttpContext.HttpContext.SystemVariables();

            //// ----------------------------------------
            //// get the quote currently in progress
            //// ----------------------------------------

            //// stored in a cookie on the user's machine
            //int dsQuoteId = DsQuoteCookieHelper.Cookie_GetQuoteId(this.ControllerContext.HttpContext);

            //// if there's no active quote (e.g. - if the session expired), redirect back to the landing page
            //if (dsQuoteId == 0)
            //{
            //    return RedirectToAction("Index");
            //}

            //// variables / initialization
            //var directSellViewModel = new DirectSellViewModel();
            //directSellViewModel.DsQuoteViewModel = DsQuoteLib.GetDsQuote(dsQuoteId);

            //// get the payment plan select list
            //directSellViewModel.Fplans = DsQuoteLib.GetFplans(directSellViewModel.DsQuoteViewModel.GovStateAbbr, (decimal)directSellViewModel.DsQuoteViewModel.Premium, systemVariables["CompanyAddress_Physical"], systemVariables["Agency"]);

            //// get the lowest configured down payment for the available FPlans
            //directSellViewModel.DsQuoteViewModel.LowestDownPayment = DsQuoteLib.GetFPlansLowestDownPayment(directSellViewModel.Fplans, (decimal)directSellViewModel.DsQuoteViewModel.Premium);

            //// if no plan has been selected yet, default the first one in the list
            //if (directSellViewModel.DsQuoteViewModel.FplanId == 0)
            //{
            //    directSellViewModel.DsQuoteViewModel.FplanId = directSellViewModel.Fplans[0].FPLANID;
            //}
            #endregion

            //// return the populated view
            return RedirectToAction("ResultsQuote");
        }

        public ActionResult ResultsQuote()
        {
            return View();
        }

        /// <summary>
        /// Fetch Quote Result
        /// </summary>
        /// <returns></returns>
        public ActionResult FetchQuoteResult()
        {
            object installmentFee = Session["InstallmentFee"];
            object premiumAmt = Session["PremiumAmt"];


            //Comment : Here get LobAbbr either from Get LobData API or from Session object to avoid uneccessary service call
            string lobName = "WC";

            // stored in a cookie on the user's machine
            int wcQuoteId = QuoteCookieHelper.Cookie_GetQuoteId(this.ControllerContext.HttpContext);
            if (wcQuoteId == 0)
            {
                return RedirectToAction("Index");
            }

            var paymentPlanResponse = SvcClientOld.CallService<PaymentPlanResponse>(string.Format("PaymentPlans?LobAbbr={0}&Premium={1}", lobName, premiumAmt.ToString()));

            // Comment : Here if request successful then get new payment plan
            if (paymentPlanResponse.OperationStatus.RequestSuccessful)
            {
                return Json(new { data = new { installmentFee = Convert.ToDecimal(installmentFee.ToString()), premiumAmt = Convert.ToDecimal(premiumAmt.ToString()), quoteRefNo = Session["QuoteRefNo"].ToString(), paymentPlan = paymentPlanResponse.PaymentPlans } }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { error = "Unable to fetch response for payment plan" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Method will post Quote related payment terms details for a specifc QuoteRefNo
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostQuoteResult(QuoteResultViewModel model)
        {

            PaymentTerms paymentTerms = new PaymentTerms();

            paymentTerms.QuoteId = Convert.ToInt32(model.DsQuoteId);
            paymentTerms.PaymentPlanId = model.PaymentPlanId ?? 0;
            paymentTerms.DownPayment = Convert.ToDecimal(model.CurrentDue.Replace("$", ""));
            paymentTerms.Installments = Convert.ToInt16(model.NoOfInstallment.Replace("$", ""));
            paymentTerms.InstallmentAmount = Convert.ToDecimal(model.FutureInstallmentAmount.Replace("$", ""));
            paymentTerms.InstallmentFee = Convert.ToDecimal(model.InstallmentFee.Replace("$", ""));
            paymentTerms.Frequency = "";
            paymentTerms.FrequencyCode = model.FrequencyCode;

            var paymentTermsResponse = SvcClientOld.CallService<PaymentTerms, OperationStatus>("PaymentTerms", "POST", paymentTerms);

            Session["QuoteData"] = model;

            //Comment : Here if request successful then get new QuoteId
            if (paymentTermsResponse.RequestSuccessful)
            {
                return Json(new { status = "Success", resultText = "Successfully submitted" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { status = "Error", resultText = "Unable to fetch response for PaymentTerms API" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Purchase()
        {
            // ----------------------------------------
            // get the quote currently in progress
            // ----------------------------------------

            // stored in a cookie on the user's machine
            int wcQuoteId = QuoteCookieHelper.Cookie_GetQuoteId(this.ControllerContext.HttpContext);

            // if there's no active quote (e.g. - if the session expired), redirect back to the landing page
            if (wcQuoteId == 0)
            {
                return RedirectToAction("Index");
            }

            //// variables / initialization
            //var purchaseViewModel = new PurchaseViewModel();
            //purchaseViewModel.DsQuoteViewModel = DsQuoteLib.GetDsQuoteAndClassesAndDsClassDesc(dsQuoteId);

            //// ----------------------------------------
            //// for authenticated users, when adding contact info for the first time, default contact info from DS user account
            //// ----------------------------------------

            //if
            //(
            //    User.Identity.IsAuthenticated
            //    //&& string.IsNullOrEmpty(purchaseViewModel.DsQuoteViewModel.UserEmail)
            //    && string.IsNullOrEmpty(purchaseViewModel.DsQuoteViewModel.UserFirstName)
            //    && string.IsNullOrEmpty(purchaseViewModel.DsQuoteViewModel.UserLastName)
            //    && string.IsNullOrEmpty(purchaseViewModel.DsQuoteViewModel.UserPhoneNumber)
            //)
            //{
            //    var user = UserManager.FindByEmail(User.Identity.Name);
            //    purchaseViewModel.DsQuoteViewModel.UserEmail = user.Email;
            //    purchaseViewModel.DsQuoteViewModel.ConfirmUserEmail = user.Email;
            //    purchaseViewModel.DsQuoteViewModel.UserFirstName = user.FirstName;
            //    purchaseViewModel.DsQuoteViewModel.UserLastName = user.LastName;
            //    purchaseViewModel.DsQuoteViewModel.UserPhoneNumber = user.PhoneNumber;
            //}
            //// otherwise, use data previously provided by the user (if available)
            //else
            //{
            //    purchaseViewModel.DsQuoteViewModel.ConfirmEmail = purchaseViewModel.DsQuoteViewModel.Email;
            //    purchaseViewModel.DsQuoteViewModel.ConfirmUserEmail = purchaseViewModel.DsQuoteViewModel.UserEmail;
            //    purchaseViewModel.DsQuoteViewModel.Password = purchaseViewModel.DsQuoteViewModel.DsQuoteEntity.ContactInitCred;
            //    purchaseViewModel.DsQuoteViewModel.ConfirmPassword = purchaseViewModel.DsQuoteViewModel.DsQuoteEntity.ContactInitCred;
            //}

            //// ----------------------------------------
            //// get select lists
            //// ----------------------------------------

            ////purchaseViewModel.BusinessTypes = DsQuoteLib.GetLookups("WCLOCNAM", "BIZTYPE", "N", "WC", "");
            //purchaseViewModel.BusinessTypes = DsQuoteLib.GetLookups("WCLOCNAM", "BIZTYPE", "N", "*", "");	// C32317-031 - return the shorter biz type list used in the ASC
            //purchaseViewModel.StateList = DsQuoteLib.GetStateAbbrList();

            //// ----------------------------------------
            ////return the populated view
            //// ----------------------------------------

            //// if the purchase has already been completed, return a display-only view
            //// (It's possible that the user might force-browse to a policy that has already been purchased.)
            //if (purchaseViewModel.DsQuoteViewModel.DsQuoteEntity.Import_Messages == "OK")
            //{
            //    // BUSINESS RULE: 
            //    // As a courtesy, we'll include class info for purchased policies here; the normal flow tries to not be so busy, and suppresses this information (it's reviewed later in the flow)
            //    //	If the simple flow was used (user selects a single class on the landing page), class descriptions are availble at this point from the DsClassDescriptions entity attached by the call above.
            //    //	If the advanced flow was used, no DsClassDescriptions were selected; the user provided specific WcClass_Daily classcode / classsuffix, instead.
            //    //	Test here to see if the user used the advanced flow; if so, load up class descriptions from WcClass_Daily
            //    //	The test: see if any of the DsQuoteClass rows has null for DsQuoteClassDescriptionId, which represents a business selected on the landing page
            //    purchaseViewModel.DsQuoteViewModel.WcClass_List = DsQuoteLib.GetWcClass_DailyForQuote(purchaseViewModel.DsQuoteViewModel);

            //    return View("Purchased", purchaseViewModel);
            //}
            // otherwise, return an editable view

            return View();

        }

        [HttpPost]
        /// <summary>
        /// Post Purchase details
        /// </summary>
        /// <returns></returns>
        public ActionResult Purchase(WcPurchaseViewModel model)
        {
            Session["PurchaseViewModel"] = model;

            // stored in a cookie on the user's machine
            int wcQuoteId = QuoteCookieHelper.Cookie_GetQuoteId(this.ControllerContext.HttpContext);

            // if there's no active quote (e.g. - if the session expired), redirect back to the landing page
            if (wcQuoteId == 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                #region Comment : Here collect all posted information into targeted DTOs

                #region Comment : Here get complete Quote object to get previuos stages data for next level data processing

                int policyDataId = 0;
                string policyCodeOrMgaCode = string.Empty;

                var quoteResponse = SvcClientOld.CallService<QuoteResponse>("Quotes?QuoteId=" + wcQuoteId);

                if (quoteResponse != null && quoteResponse.OperationStatus.RequestSuccessful)
                {
                    policyDataId = quoteResponse.Quote.PolicyData.PolicyDataId ?? 0;
                    policyCodeOrMgaCode = quoteResponse.Quote.PolicyData.MgaCode ?? "";
                    Session["PolicyCode"] = policyCodeOrMgaCode;
                }

                //Comment : Here set PolicyDataId into POST API object
                var policyData = new PolicyData();
                policyData.QuoteId = wcQuoteId;
                policyData.PolicyDataId = policyDataId;
                policyData.InceptionDate = Convert.ToDateTime(model.Policy.PolicyDate);

                #endregion

                var personalPhone = model.PersonalContact.PhoneNumber.Split('x');//(222) 222-2222 x22222 model.PersonalContact.PhoneNumber
                var businessPhone = model.BusinessContact.PhoneNumber.Split('x'); ; //(222) 222-2222 x22222 model.BusinessContact.PhoneNumber

                //Comment : here collect "Create Your Account"
                //var userProfileData = new UserProfile();
                //userProfileData.Email = model.PersonalContact.Email;
                //userProfileData.FirstName = model.PersonalContact.FirstName;
                //userProfileData.LastName = model.PersonalContact.LastName;
                //userProfileData.PhoneNumber = UtilityFunctions.ToNumeric(personalPhone[0]);
                //userProfileData.Password = model.Account.Password;
                //userProfileData.NewPassword = model.Account.ConfirmPassword;
                //userProfileData.UserProfilePostType = UserProfilePostType.CreateUserProfile;

                //Comment : here collect "Additional Business Information"
                var insuredNameData = new InsuredName();
                insuredNameData.QuoteId = wcQuoteId;
                insuredNameData.PrimaryInsuredName = true;
                insuredNameData.NameType = model.BusinessInfo.BusinessType;
                insuredNameData.Name = model.BusinessInfo.BusinessName;
                insuredNameData.FEINType = model.BusinessInfo.TaxIdType;
                insuredNameData.FEIN = UtilityFunctions.ToNumeric(model.BusinessInfo.TaxIdOrSSN);// ?? "333-333333";

                //Comment : here collect "Business Mailing Address"
                var addressData = new Address();
                addressData.Addr1 = model.MailingAddress.AddressLine1;
                addressData.Addr2 = model.MailingAddress.AddressLine2;
                addressData.State = model.MailingAddress.State ?? "WI";
                addressData.City = model.MailingAddress.City ?? "Addison";
                addressData.Zip = model.MailingAddress.Zip ?? "54481";


                //Comment : here collect "Business Mailing Address"
                var personalContactPhone = new Phone();
                //personalContactPhone.PhoneType = "Home";
                personalContactPhone.PhoneType = "1";
                personalContactPhone.PhoneNumber = UtilityFunctions.ToNumeric(personalPhone[0]);
                personalContactPhone.Extension = UtilityFunctions.ToNumeric(personalPhone[1]);

                var businessConatctPhone = new Phone();
                //businessConatctPhone.PhoneType = "Business";
                businessConatctPhone.PhoneType = "0";
                businessConatctPhone.PhoneNumber = UtilityFunctions.ToNumeric(businessPhone[0]);
                businessConatctPhone.Extension = UtilityFunctions.ToNumeric(businessPhone[1]);
                //Comment : Here create contact object 
                var personalcontactData = new Contact();
                personalcontactData.QuoteId = wcQuoteId;
                personalcontactData.Name = string.Concat(model.PersonalContact.FirstName, " ", model.PersonalContact.LastName);
                personalcontactData.ContactType = "Misc";
                personalcontactData.Email = model.PersonalContact.Email;
                //personalcontactData.Phones = new List<Phone> { personalContactPhone };        //Right now child object submission is not exposed by Guard ex. Address in Contact details

                var businesscontactData = new Contact();
                businesscontactData.QuoteId = wcQuoteId;
                businesscontactData.Name = string.Concat(model.BusinessContact.FirstName, " ", model.BusinessContact.LastName);
                businesscontactData.Company = model.BusinessInfo.BusinessName;
                businesscontactData.ContactType = "Billing";
                businesscontactData.Email = model.BusinessContact.Email;
                //businesscontactData.Addresses = new List<Address> { addressData };            //Right now child object submission is not exposed by Guard ex. Address in Contact details
                //businesscontactData.Phones = new List<Phone> { businessConatctPhone };        //Right now child object submission is not exposed by Guard ex. Phones in Contact details

                // populate a BatchAction that will be used to submit the InsuredName DTO to the Insurance Service
                Location locationData = new Location();
                locationData.QuoteId = wcQuoteId;
                locationData.PrimaryCaMailAddr = false;
                locationData.LocationType = "M";
                locationData.Addr1 = model.MailingAddress.AddressLine1;
                locationData.Addr2 = model.MailingAddress.AddressLine2;
                locationData.City = model.MailingAddress.City;
                locationData.State = model.MailingAddress.State;
                locationData.Zip = model.MailingAddress.Zip;

                // populate a BatchAction that will be used to submit the InsuredName DTO to the Insurance Service
                Officer officerData = new Officer();

                officerData.QuoteId = wcQuoteId;
                officerData.Included = true;
                officerData.Name = model.BusinessInfo.BusinessName;
                officerData.Title = "MR";
                officerData.SSN = UtilityFunctions.RemoveSpecialCharacters(model.BusinessInfo.TaxIdOrSSN);
                officerData.Addr1 = model.MailingAddress.AddressLine1;
                officerData.Addr2 = model.MailingAddress.AddressLine2;
                officerData.City = model.MailingAddress.City;
                officerData.State = model.MailingAddress.State;
                officerData.Zip = model.MailingAddress.Zip;
                officerData.PercentageOwnership = System.Convert.ToDecimal(20.25);
                officerData.Payroll = Convert.ToDecimal(5000);

                #endregion

                #region Comment : Here create BatchAction list object

                /*----------------------------------------
                populate a BatchActionList that contains all requests to be sent to the Gaurd API
                ----------------------------------------*/

                var batchActionList = new BatchActionList();	// BatchActionList: list of actions routed to the Insurance Service for response
                BatchResponseList batchResponseList;			// BatchResponseList: list of responses returned from the Insurance Service
                string jsonResponse;							// JSON-formated response data returned from the Insurance Service

                #endregion

                #region Comment : Here First create batch action for "Contact - Personal" and "Contact - Business" API and based on EffectedIds POST related child object POST in Next Batch

                string personalContactId = null, businessContactId = null;

                // populate a BatchAction that will be used to submit the Contact ("Personal") DTO to the Insurance Service
                var contactPersonalAction = new BatchAction { ServiceName = "Contacts", ServiceMethod = "POST", RequestIdentifier = "ContactsPersonal Data" };
                contactPersonalAction.BatchActionParameters.Add(new BatchActionParameter { Name = "contact", Value = JsonConvert.SerializeObject(personalcontactData) });
                batchActionList.BatchActions.Add(contactPersonalAction);

                // populate a BatchAction that will be used to submit the Contact ("Business") DTO to the Insurance Service
                var contactBusinessAction = new BatchAction { ServiceName = "Contacts", ServiceMethod = "POST", RequestIdentifier = "ContactsBusiness Data" };
                contactBusinessAction.BatchActionParameters.Add(new BatchActionParameter { Name = "contact", Value = JsonConvert.SerializeObject(businesscontactData) });
                batchActionList.BatchActions.Add(contactBusinessAction);

                // submit the BatchActionList to the Insurance Service
                jsonResponse = SvcClientOld.CallServiceBatch(batchActionList);

                // flag that determines how to respond after processing the response received
                bool success = true;

                // deserialize the results into a BatchResponseList
                batchResponseList = JsonConvert.DeserializeObject<BatchResponseList>(jsonResponse);

                // deserialize each response returned by the service, and copy the OperationStatus to the view model for demo purposes
                OperationStatus operationStatus = new OperationStatus();
                foreach (var batchResponse in batchResponseList.BatchResponses)
                {
                    // this flag tested later to determine whether to proceed to the next view or not
                    if (batchResponse.RequestSuccessful)
                    {
                        try
                        {
                            //perosnal contacts details
                            var serviceResponse = batchResponseList.BatchResponses.SingleOrDefault(m => m.RequestIdentifier == "ContactsPersonal Data").JsonResponse;

                            if (serviceResponse != null)
                            {
                                var operationStatusDeserialized = JsonConvert.DeserializeObject<OperationStatus>(serviceResponse);
                                personalContactId = operationStatusDeserialized.AffectedIds.SingleOrDefault(m => m.DTOProperty == "ContactId").IdValue;
                            }

                            //business contacts details
                            serviceResponse = batchResponseList.BatchResponses.SingleOrDefault(m => m.RequestIdentifier == "ContactsBusiness Data").JsonResponse;

                            if (serviceResponse != null)
                            {
                                var operationStatusDeserialized = JsonConvert.DeserializeObject<OperationStatus>(serviceResponse);
                                businessContactId = operationStatusDeserialized.AffectedIds.SingleOrDefault(m => m.DTOProperty == "ContactId").IdValue;
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }

                //Comment : Here Reset batchaction list
                batchActionList = new BatchActionList();
                batchResponseList = null;

                #endregion

                #region Comment : Here other batch action "Phone" and "Address" and "InsuredNames" and "UserProfile" creation POST APIs

                #region Comment : Here if policyData id found then only include PolicyData POST API into batch statement

                if (policyDataId > 0)
                {
                    // populate a BatchAction that will be used to submit the InsuredName DTO to the Insurance Service
                    var policyDataAction = new BatchAction { ServiceName = "PolicyData", ServiceMethod = "POST", RequestIdentifier = "PolicyData Data" };
                    policyDataAction.BatchActionParameters.Add(new BatchActionParameter { Name = "policyData", Value = JsonConvert.SerializeObject(policyData) });
                    batchActionList.BatchActions.Add(policyDataAction);
                }

                #endregion

                #region Comment : Here if contact data submitted then only add related child objects in next batch execution

                if (!string.IsNullOrEmpty(personalContactId))
                {
                    personalContactPhone.ContactId = int.Parse(personalContactId);

                    // populate a BatchAction that will be used to submit the Contact ("Personal") DTO to the Insurance Service
                    var phoneContactPersonalAction = new BatchAction { ServiceName = "Phones", ServiceMethod = "POST", RequestIdentifier = "PersonalContactPhone Data" };
                    phoneContactPersonalAction.BatchActionParameters.Add(new BatchActionParameter { Name = "phone", Value = JsonConvert.SerializeObject(personalContactPhone) });
                    batchActionList.BatchActions.Add(phoneContactPersonalAction);
                }

                if (!string.IsNullOrEmpty(businessContactId))
                {
                    #region Comment : Here add related "Phones" data to CotactId

                    businessConatctPhone.ContactId = int.Parse(businessContactId);

                    // populate a BatchAction that will be used to submit the Contact ("Business") DTO to the Insurance Service
                    var phoneContactBusinessAction = new BatchAction { ServiceName = "Phones", ServiceMethod = "POST", RequestIdentifier = "BusinessConatctPhone Data" };
                    phoneContactBusinessAction.BatchActionParameters.Add(new BatchActionParameter { Name = "phone", Value = JsonConvert.SerializeObject(businessConatctPhone) });
                    batchActionList.BatchActions.Add(phoneContactBusinessAction);

                    #endregion

                    #region Comment : Here add related "Address" data to CotactId

                    addressData.ContactId = int.Parse(businessContactId);

                    // populate a BatchAction that will be used to submit the Contact ("Business") DTO to the Insurance Service
                    var addressContactBusinessAction = new BatchAction { ServiceName = "Addresses", ServiceMethod = "POST", RequestIdentifier = "BusinessConatctAddress Data" };
                    addressContactBusinessAction.BatchActionParameters.Add(new BatchActionParameter { Name = "address", Value = JsonConvert.SerializeObject(addressData) });
                    batchActionList.BatchActions.Add(addressContactBusinessAction);

                    #endregion
                }

                #endregion

                // populate a BatchAction that will be used to submit the InsuredName DTO to the Insurance Service
                var insuredNameAction = new BatchAction { ServiceName = "InsuredNames", ServiceMethod = "POST", RequestIdentifier = "InsuredNames Data" };
                insuredNameAction.BatchActionParameters.Add(new BatchActionParameter { Name = "insuredName", Value = JsonConvert.SerializeObject(insuredNameData) });
                batchActionList.BatchActions.Add(insuredNameAction);

                var locationAction = new BatchAction { ServiceName = "Locations", ServiceMethod = "POST", RequestIdentifier = "Locations Data" };
                locationAction.BatchActionParameters.Add(new BatchActionParameter { Name = "location", Value = JsonConvert.SerializeObject(locationData) });
                batchActionList.BatchActions.Add(locationAction);

                var officerResponse = new BatchAction { ServiceName = "Officers", ServiceMethod = "POST", RequestIdentifier = "Officers Data" };
                officerResponse.BatchActionParameters.Add(new BatchActionParameter { Name = "officer", Value = JsonConvert.SerializeObject(officerData) });
                batchActionList.BatchActions.Add(officerResponse);

                // populate a BatchAction that will be used to submit the UserProfile DTO to the Insurance Service
                //var userProfileAction = new BatchAction { ServiceName = "UserProfiles", ServiceMethod = "POST", RequestIdentifier = "UserProfiles Data" };
                //userProfileAction.BatchActionParameters.Add(new BatchActionParameter { Name = "userProfile", Value = JsonConvert.SerializeObject(userProfileData) });
                //batchActionList.BatchActions.Add(userProfileAction);

                #endregion

                #region Comment : Here Execute BatchActions list

                // submit the BatchActionList to the Insurance Service
                jsonResponse = SvcClientOld.CallServiceBatch(batchActionList);

                // flag that determines how to respond after processing the response received
                success = true;

                // deserialize the results into a BatchResponseList
                batchResponseList = JsonConvert.DeserializeObject<BatchResponseList>(jsonResponse);

                // deserialize each response returned by the service, and copy the OperationStatus to the view model for demo purposes
                operationStatus = new OperationStatus();
                foreach (var batchResponse in batchResponseList.BatchResponses)
                {
                    var requestProcessed = JsonConvert.DeserializeObject<object>(batchResponse.JsonResponse);
                    // this flag tested later to determine whether to proceed to the next view or not
                    if (!batchResponse.RequestSuccessful) { success = false; }
                }

                #endregion

                #region Comment : Here Based on result of BatchActions POST API do next level proceessing

                // TODO: Do something with the response
                if (!success)
                {
                    //throw new ApplicationException("Unable to post referral contact details");
                    //return Json(new { resultStatus = "NOK", resultText = "Unable to post referral contact details" }, JsonRequestBehavior.AllowGet);
                    return View();
                }
                else
                {
                    //Comment : Here call generate policy method
                    CreatePolicy();

                    #region Comment : Here Update/Save New QuoteId Into Cookies

                    //Comment : Here save this QuoteId in user cookies for later use
                    QuoteCookieHelper.Cookie_SaveQuoteId(ControllerContext.HttpContext, wcQuoteId);			// save the quote id off in a cookie

                    #endregion

                    //return Json(new { resultStatus = "OK", resultText = "Quote referral details saved successfully" }, JsonRequestBehavior.AllowGet);
                }

                #endregion

            }

            return View();
        }

        public ActionResult VerifyOrderGet()
        {
            // ----------------------------------------
            // get the quote currently in progress
            // ----------------------------------------

            // stored in a cookie on the user's machine
            int dsQuoteId = QuoteCookieHelper.Cookie_GetQuoteId(this.ControllerContext.HttpContext);

            // if there's no active quote (e.g. - if the session expired), redirect back to the landing page
            if (dsQuoteId == 0)
            {
                return RedirectToAction("Index");
            }

            // ----------------------------------------
            // get cached variables
            // ----------------------------------------

            //var systemVariables = this.HttpContext.SystemVariables();

            // ----------------------------------------
            // get the quote data
            // ----------------------------------------

            // BUSINESS RULE: 
            //	If the simple flow was used (user selects a single class on the landing page), class descriptions are availble at this point from the DsClassDescriptions entity attached by the call above.
            //	If the advanced flow was used, no DsClassDescriptions were selected; the user provided specific WcClass_Daily classcode / classsuffix, instead.
            //	Test here to see if the user used the advanced flow; if so, load up class descriptions from WcClass_Daily
            //	The test: see if any of the DsQuoteClass rows has null for DsQuoteClassDescriptionId, which represents a business selected on the landing page

            // get the associated agency

            // get the associated business type

            // ----------------------------------------
            // C32317-034 The Annual Premium at the top should make reference to the finance plan and payment terms previously selected.
            // ----------------------------------------

            // ----------------------------------------
            //return the populated view
            // ----------------------------------------

            return View("VerifyOrder");
        }

        /// <summary>
        /// Return list of all business types
        /// </summary>
        /// <param name="wcQuoteViewModel"></param>
        /// <returns></returns>
        public JsonResult GetPurchaseModel()
        {
            if (Session["PurchaseViewModel"] != null && Session["PurchaseViewModel"].ToString().Length > 0)
            {
                //Comment : Here get back Session into target type object
                var wcPurchaseModel = Session["PurchaseViewModel"] as WcPurchaseViewModel;
                wcPurchaseModel.Policy.PolicyDate = Convert.ToDateTime(wcPurchaseModel.Policy.PolicyDate).ToShortDateString();
                var wcPurchaseModelJson = JsonConvert.SerializeObject(wcPurchaseModel);

                var quoteResult = Session["QuoteData"];


                return Json(new { success = true, purchaseModel = wcPurchaseModelJson, quoteResultData = quoteResult }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                int wcQuoteId = QuoteCookieHelper.Cookie_GetQuoteId(this.ControllerContext.HttpContext);
                string zip = Convert.ToString(Session["ZipCode"]), state = Convert.ToString(Session["State"]);
                if (string.IsNullOrWhiteSpace(state) || string.IsNullOrWhiteSpace(state))
                {
                    var exposureResponse = SvcClientOld.CallService<ExposureResponse>("Exposures?QuoteId=" + wcQuoteId.ToString());

                    if (exposureResponse != null && exposureResponse.OperationStatus.RequestSuccessful)
                    {
                        state = exposureResponse.Exposures.ElementAtOrDefault(0).StateAbbr;
                        zip = exposureResponse.Exposures.ElementAtOrDefault(0).ZipCode;
                    }
                }
                return Json(new { exposureDate = Convert.ToDateTime(Session["ExposureDate"]).ToShortDateString(), zipCode = zip, stateName = state }, JsonRequestBehavior.AllowGet);
            }
        }


        #endregion

        #endregion

        #region Save For Later Process Flow

        public ActionResult SaveForLater()
        {
            return View();
        }

        [HttpPost]
        //Send response to client through mail
        public JsonResult SaveForLater(BHIC.ViewDomain.Landing.SaveForLaterViewModel model)
        {
            // stored in a cookie on the user's machine
            int wcQuoteId = QuoteCookieHelper.Cookie_GetQuoteId(this.ControllerContext.HttpContext);

            // get the Direct Sales site base url that will be formatted into the link sent to the user
            string baseUrl = Helper_GetBaseUrl();

            // build the link that the recipient will click to access their quote, and get the recipient's email address
            string anchorText = ThemeManager.ThemeSharedContent("useremail-saveforlater-anchortext");

            // build the link that the recipient will click to access their quote, and get the recipient's email address
            // var mailMsg = WcQuoteLib.BuildEmailLinkAndRecipient(baseUrl, wcQuoteId, anchorText, new List<string> { model.Email });


            var mailMsg = new MailMsg();

            // set the sender email address
            try
            {
                // mailMsg.SenderEmailAddr = "";
            }
            catch
            {
                // swallow the error; if the entry doesn't exist, email will use a default from address
            }

            //currently we could not found appropriate values, so hard codded values are used for testing purpose
            // get the canned message template, embed the link that the user will click on
            mailMsg.MessageBody = ThemeManager.ThemeSharedContent("useremail-saveforlater-body", new
            {
                ReturnLink = mailMsg.MessageBody,
                CompanyName = "National Liability & Fire, a Berkshire Hathaway Co",
                CompanyPoBox = "113247",
                CompanyCityStateZip = "Stamford, CT 06911-3247",
                SupportEmail = "csr@nlf-info.com",
                SupportPhone = "844-861-1199",
                Logo = "",
                WebsiteURL = "https://www.bhdwc.com/",
                RecipientEmail = mailMsg.RecipEmailAddr
            });

            // get the subject line
            mailMsg.Subject = ThemeManager.ThemeSharedContent("useremail-saveforlater-subject", new { CompanyName = "" });

            // send the message
            SendMail(mailMsg);

            return null;
        }

        //Show message if message send successfully
        public ActionResult SavedForLater()
        {
            return View();
        }

        public ActionResult SavedForLaterRetrieve(int quoteId)
        {
            QuoteCookieHelper.Cookie_SaveQuoteId(this.ControllerContext.HttpContext, quoteId);

            return View();
        }

        #endregion

        #region SoftReffral Process

        /// <summary>
        /// prepare mail content for SofReferral process
        /// </summary>
        /// <param name="wcQuoteViewModel"></param>
        private bool ProcessSoftReferral(WcQuoteViewModel wcQuoteViewModel)
        {
            // build the message to be sent to GUARD staff
            var mailMsg = new MailMsg();

            // recipient
            mailMsg.RecipEmailAddr = wcQuoteViewModel.Email;

            // build get the subject line, populating the html template in parameter 1 with the variables in parameter 2
            mailMsg.Subject = ThemeManager.ThemeSharedContent("guardemail-soft-referral-subject")
                + " ("
                + "Business Name: "
                + wcQuoteViewModel.BusinessName
                + ", "
                + "Governing State: "
                + ((!string.IsNullOrEmpty(wcQuoteViewModel.GovStateAbbr)) ? wcQuoteViewModel.GovStateAbbr : wcQuoteViewModel.MailState ?? "n/a")
                + ")";

            if (!wcQuoteViewModel.EffDate.HasValue)
            {
                wcQuoteViewModel.EffDate = DateTime.Now;
            }

            // build thge message body, populating the html template in parameter 1 with the variables in parameter 2
            mailMsg.MessageBody = ThemeManager.ThemeSharedContent("guardemail-soft-referral-body", new
            {
                GovState = wcQuoteViewModel.MailState,


                EffDate = ((DateTime)wcQuoteViewModel.EffDate).ToShortDateString(),

                // user contact info
                UserFirstName = wcQuoteViewModel.UserFirstName,
                UserLastName = wcQuoteViewModel.UserLastName,
                UserPhoneNumber = wcQuoteViewModel.UserPhoneNumber,
                UserEmail = wcQuoteViewModel.UserEmail,

                // business contact info
                ContactName = wcQuoteViewModel.ContactFirstName + " " + wcQuoteViewModel.ContactLastName,
                ContactPhone = wcQuoteViewModel.ContactPhone,
                ContactEmail = wcQuoteViewModel.Email,
                BusinessName = wcQuoteViewModel.BusinessName,
                MailAddr1 = wcQuoteViewModel.MailAddr1,
                MailAddr2 = wcQuoteViewModel.MailAddr2,
                MailCity = wcQuoteViewModel.MailCity,
                MailState = wcQuoteViewModel.MailState,
                MailZip = wcQuoteViewModel.MailZip,
            });

            //send mail to recipient
            if (SendMail(mailMsg))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// prepare mail content for HardReferral process
        /// </summary>
        /// <param name="wcQuoteViewModel"></param>
        private bool ProcessHardReferral(WcQuoteViewModel wcQuoteViewModel)
        {
            //var wcQuote = "";

            // build the message to be sent to GUARD staff
            var mailMsg = new MailMsg();

            // recipient
            mailMsg.RecipEmailAddr = wcQuoteViewModel.Email;

            // build get the subject line, populating the html template in parameter 1 with the variables in parameter 2
            mailMsg.Subject = ThemeManager.ThemeSharedContent("guardemail-hard-referral-subject")
                + " (MGA Code: " + ""
                + ", "
                + "Business Name: "
                + wcQuoteViewModel.BusinessName
                + ", "
                + "Governing State: "
                + wcQuoteViewModel.GovStateAbbr
                + ")";


            mailMsg.Subject = "Attention Required: " + mailMsg.Subject;

            // build thge message body, populating the html template in parameter 1 with the variables in parameter 2
            mailMsg.MessageBody = ThemeManager.ThemeSharedContent("guardemail-hard-referral-body", new
            {
                MGACode = "",
                Premium = string.Empty,
                GovState = wcQuoteViewModel.GovStateAbbr,
                EffDate = ((DateTime)wcQuoteViewModel.EffDate).ToShortDateString(),

                DecisionStatus = string.Empty,
                DecisionStatusDescription = string.Empty,
                ImportStatus = string.Empty,
                QuoteId = wcQuoteViewModel.QuoteId,

                ContactName = wcQuoteViewModel.ContactFirstName + " " + wcQuoteViewModel.ContactLastName,
                ContactPhone = wcQuoteViewModel.ContactPhone,
                ContactEmail = wcQuoteViewModel.Email,
                BusinessName = wcQuoteViewModel.BusinessName,
                MailAddr1 = wcQuoteViewModel.MailAddr1,
                MailAddr2 = wcQuoteViewModel.MailAddr2,
                MailCity = wcQuoteViewModel.MailCity,
                MailState = wcQuoteViewModel.MailState,
                MailZip = wcQuoteViewModel.MailZip,

            });

            //send mail to recipient
            if (SendMail(mailMsg))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Send mail with details to user
        /// </summary>
        /// <param name="mail"></param>
        private bool SendMail(MailMsg mail)
        {
            try
            {
                // init DTO
                SoapieMailMessageRequestParms mailMessage = new SoapieMailMessageRequestParms();
                mailMessage.To = (string.IsNullOrEmpty(mail.RecipEmailAddr) == true) ? "mail@test.com" : mail.RecipEmailAddr; //todo: provide recipient email address here
                mailMessage.From = (string.IsNullOrEmpty(mail.SenderEmailAddr) == true) ? "mail@test.com" : mail.SenderEmailAddr; //todo: provide sender email address here
                mailMessage.Body = mail.MessageBody.Substring(0, 1118); //todo: hardcoded value provided for testing purpose only
                mailMessage.PolicyCodeOrAgency = "NENATI20-PANONE99"; //todo: hardcoded value provided for testing purpose only
                mailMessage.Subject = mail.Subject;

                var postOperationStatus = SvcClientOld.CallService<SoapieMailMessageRequestParms, OperationStatus>("SendEmail", "POST", mailMessage);
            }
            catch (Exception)
            {

            }

            return true;
        }

        #endregion

        #endregion

        #region Purchase Page Data Flow

        public bool ValidateStateCityZip(string zipCode, string city, string state)
        {
            var ZipCityStateResponse = vCityStateZipService.GetVCityStateZipCodeData(new VCityStateZipCodeRequestParms { City = city, State = state, ZipCode = zipCode });
            if (!(ZipCityStateResponse.OperationStatus.RequestSuccessful && ZipCityStateResponse.OperationStatus.Messages.Count == 0))
                return true;
            else
                return false;
        }

        public JsonResult CreatePolicy()
        {
            try
            {
                var policyCreateRequest = new PolicyCreate();

                int wcQuoteId = QuoteCookieHelper.Cookie_GetQuoteId(this.ControllerContext.HttpContext);
                policyCreateRequest.LOB = "WC";
                policyCreateRequest.QuoteId = wcQuoteId;

                var generatePolicyResponse = policyCreateService.AddPolicy(policyCreateRequest);

                //Comment : Here amount is not validated for supplied "ClassDescriptionId" then send "NOK" with 'MinimumExposure"
                if (generatePolicyResponse.RequestSuccessful)
                {
                    GeneratePolicy(wcQuoteId);
                }
                else if (!generatePolicyResponse.RequestSuccessful)
                {
                    return Json(new { resultStatus = "NOK", resultText = generatePolicyResponse.Messages[0].Text }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
            }

            return Json(new { resultStatus = "NOK", resultText = "Minimum Exposure Amount Should not be less than $15,000" }, JsonRequestBehavior.AllowGet);
        }

        public void GeneratePolicy(int wcQuoteId)
        {
            try
            {
                //var policyResp = policyDataService.GetPolicyData(new PolicyDataRequestParms { QuoteId = wcQuoteId });
                var policyResp = SvcClientOld.CallService<PolicyDataRequestParms, PolicyDataResponse>("PolicyData", new PolicyDataRequestParms { QuoteId = wcQuoteId });

                PolicyDetailsResponse policyDetailResponse;
                if (policyResp != null && policyResp.PolicyData != null && !string.IsNullOrWhiteSpace(policyResp.PolicyData.MgaCode))
                {
                    //policyDetailResponse = policyDetailsService.GetPolicyDetails(new PolicyDetailsRequestParms { PolicyCode = policyResp.PolicyData.MgaCode });
                    policyDetailResponse = SvcClientOld.CallService<PolicyDetailsRequestParms, PolicyDetailsResponse>("PolicyData",
                                                                    new PolicyDetailsRequestParms { PolicyCode = policyResp.PolicyData.MgaCode });

                    if (!(policyDetailResponse.PolicyDetails == null && !policyDetailResponse.OperationStatus.RequestSuccessful))
                    {
                        //Fill the policy certificate model
                    }
                }
            }
            catch (Exception)
            {
            }

        }

        #endregion

        #region Error View

        // friendly 'error' page that can be displayed in the event of unrecoverable exceptions
        public ActionResult Error()
        {
            return View();
        }

        #endregion

        #region Cookies related helpers

        // ----------------------------------------
        // cookie test helpers (used to confirm that cookies are enabled on the user's machine)
        // ----------------------------------------
        // Use Cases: if a user requests a page where a cookie is expected, but none is found, it could mean that:
        //	a) The user's device doesn't support cookies, or
        //	b) The session used to write the cookie expired, or
        //	c) The user force-browsed to the page, outside an active quote session, via a bookmark (this is really just a specific instance of b) above)
        //
        // Business Rules:
        // a) if the user if a user requests a page where a cookie is expected, but none is found:
        //		- write a test cookie
        //		- redirect to an action that tries to read the test cookie
        //		- if the test cookie is found, cookies are enabled; redirect to the start page.
        //		- if the test cookie is not found, redirect to a page that advises the user to enable cookies.
        public ActionResult CookieTest_Save()
        {
            QuoteCookieHelper.Cookie_SaveTestCookie(this.ControllerContext.HttpContext);
            return RedirectToAction("CookieTest_Get");
        }

        public ActionResult CookieTest_Get()
        {
            var cookiesEnabled = QuoteCookieHelper.Cookie_GetTestCookie(this.ControllerContext.HttpContext);
            if (cookiesEnabled)
            {
                return RedirectToAction("Index");
            }
            else
            {
                //ErrorLog.LogError("Warning: Cookies are disabled on the user's device.  The user has been advised that cookies need to be enabled in order to continue.");
                return RedirectToAction("CookiesDisabled", "Home");
            }
        }

        #endregion

        #region Cache Demo

        private Timer Schedular;

        public ActionResult CacheView()
        {

            //MailHelper mail = new MailHelper();
            //mail.SendMailMessage(new List<string> { "anuj.singh@xceedance.com" }, "subject");

            Response.AddHeader("Refresh", "35");
            ViewBag.Timer = DateTime.Now.ToString();

            //string key1 = "Industry";
            //string key2 = "SubIndustry";
            //string key3 = "temp";

            //CacheHelper cache = CacheHelper.Instance;

            ////if cache is empty add some value to test functionality
            //if ((cache.Get<string>(key1) == null) && (cache.Get<string>(key2) == null))
            //{
            //    cache.Add(key1, "industry");
            //    cache.Add(key2, "subIndustry");
            //    cache.Add(key3, "temp");
            //}

            ////get all stored cache
            //var result = cache.GetAll();
            //var lenght = cache.GetCacheLength();

            ////remove one cache item based on key
            //cache.Remove(key3);

            ////get result after delete operation
            //result = cache.GetAll();
            //lenght = cache.GetCacheLength();

            ////remove all item from cache
            //cache.RemoveAll();

            ////get result after delete operation
            //result = cache.GetAll();
            //lenght = cache.GetCacheLength();

            return View();
        }

        public void ScheduleService()
        {
            try
            {
                Schedular = new Timer(new TimerCallback(SchedularCallback));
                //string mode = ConfigurationManager.AppSettings["Mode"].ToUpper();
                //this.WriteToFile("Simple Service Mode: " + mode + " {0}");

                ////Set the Default Time.
                //DateTime scheduledTime = DateTime.MinValue;

                //if (mode == "DAILY")
                //{
                //    //Get the Scheduled Time from AppSettings.
                //    scheduledTime = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["ScheduledTime"]);
                //    if (DateTime.Now > scheduledTime)
                //    {
                //        //If Scheduled Time is passed set Schedule for the next day.
                //        scheduledTime = scheduledTime.AddDays(1);
                //    }
                //}

                //if (mode.ToUpper() == "INTERVAL")
                //{
                //    //Get the Interval in Minutes from AppSettings.
                //    int intervalMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["IntervalMinutes"]);

                //    //Set the Scheduled Time by adding the Interval to Current Time.
                //    scheduledTime = DateTime.Now.AddMinutes(intervalMinutes);
                //    if (DateTime.Now > scheduledTime)
                //    {
                //        //If Scheduled Time is passed set Schedule for the next Interval.
                //        scheduledTime = scheduledTime.AddMinutes(intervalMinutes);
                //    }
                //}

                //TimeSpan timeSpan = scheduledTime.Subtract(DateTime.Now);
                //string schedule = string.Format("{0} day(s) {1} hour(s) {2} minute(s) {3} seconds(s)", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

                //this.WriteToFile("Simple Service scheduled to run after: " + schedule + " {0}");

                ////Get the difference in Minutes between the Scheduled and Current Time.
                //int dueTime = Convert.ToInt32(timeSpan.TotalMilliseconds);

                //Change the Timer's Due Time.
                Schedular.Change(0, 500);
            }
            catch (Exception ex)
            {
                WriteToFile("Simple Service Error on: {0} " + ex.Message + ex.StackTrace);
            }
        }

        private void SchedularCallback(object e)
        {
            this.WriteToFile("Simple Service Log: {0}");
            this.ScheduleService();
        }

        private void WriteToFile(string text)
        {
            string path = "C:\\ServiceLog.txt";
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(string.Format(text, DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt")));
                writer.Close();
            }
        }
        public JsonResult GetIndustryList()
        {
            string key = "Industry";
            var industry = new JsonResult();
            var flag = false;

            CacheHelper cache = CacheHelper.Instance;

            try
            {
                if (cache.Get<JsonResult>(key) == null)
                {
                    flag = true;
                    industry = GetIndustries();
                    cache.Add(key, industry);
                }

                industry = cache.Get<JsonResult>(key);

                return Json(new { industry = industry.Data, isUpdated = flag }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex) { Console.Write(ex.Message); }

            return null;
        }

        public string GetSchedularData()
        {
            var directory = new DirectoryInfo("D:\\Projects\\BHICRepository\\src\\BHIC\\BHIC.Portal\\Log");

            FileInfo filePath = directory.GetFiles()
             .OrderByDescending(f => f.LastWriteTime)
             .First();

            // Read the file as one string.
            System.IO.StreamReader myFile =
               new System.IO.StreamReader(filePath.FullName);
            string myString = myFile.ReadToEnd();

            return myString;
        }

        #endregion

    }
}