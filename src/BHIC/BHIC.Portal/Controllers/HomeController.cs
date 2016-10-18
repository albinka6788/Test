using System.Web.Mvc;
using BHIC.Contract.Demo;
using BHIC.Contract.Background;
using BHIC.Domain.Background;
using BHIC.Common.Client;
using BHIC.Domain.Policy;
using BHIC.Domain.Service;
using Newtonsoft.Json;
using System.Linq;
using System;

namespace BHIC.Portal.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {            
        }

        public ActionResult Index()
        {
            //var industries = GetExposures();
            var str = GetIndustries();
            return View();
        }

        public ActionResult About()
        {

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult TestView()
        {
            return View();
        }

        /// <summary>
        /// This method gets all the Industries 
        /// </summary>
        /// <returns></returns>
        public JsonResult GetIndustries()
        {
            var industryResponseReturn = new System.Collections.Generic.List<Industry>();

            try
            {
                var provider = new GuardServiceProvider { ServiceCategory = "WC" };
                var industryResponse = SvcClient.CallService<IndustryResponse>("Industries?LOB=WC", provider);

                if (industryResponse.OperationStatus.RequestSuccessful)
                {
                    industryResponseReturn = industryResponse.Industries;
                }

            }
            catch (System.Exception)
            {               
            }

            return Json(industryResponseReturn, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetExposures()
        {

            //Comment : Here if response contains data
            bool isLandingSaved = false;
            Exposure exposure = null;
            PolicyData policyData = null;
            string policyInceptionDate = null;
            var provider = new GuardServiceProvider { ServiceCategory = "WC" };

            try
            {

                // stored in a cookie on the user's machine
                int wcQuoteId = 10427;

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

                    jsonResponse = SvcClient.CallServiceBatch(batchActionList,provider);

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

                    if (batchResponseList != null)
                    {
                        #region Comment : Here get PolicyDataId
                        var policyDataResponse = batchResponseList.BatchResponses.SingleOrDefault(m => m.RequestIdentifier == "PolicyData").JsonResponse;

                        if (policyDataResponse != null)
                        {
                            policyData = JsonConvert.DeserializeObject<PolicyDataResponse>(policyDataResponse).PolicyData;

                            if (policyData != null && policyData.PolicyDataId != null)
                            {
                                //Comment : Here this session will be used to get this infor mation to set on other pages in Purchase process flow
                                Session["LastSavedPolicyDataTest"] = policyData;
                                policyInceptionDate = Convert.ToDateTime(policyData.InceptionDate).ToShortDateString();
                                string policyOrMgaCode = policyData.MgaCode == "" ? "" : policyData.MgaCode;
                                Session["policyOrMgaCodeTest"] = policyOrMgaCode;
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
                                    Session["LastSavedExposureTest"] = exposure;
                                }
                            }
                        }
                        #endregion
                    }
                    else if (success)
                    {
                        string removeWarning = string.Empty;
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

        public JsonResult GetXMod()
        {
            string xModResponse = string.Empty;
            try
            {
                var provider = new NcciServiceProvider { ServiceCategory = ServiceProviderConstants.NcciServiceCategoryXMod };
                xModResponse = SvcClient.CallService<string>("GetModRiskId?riskid=999999999&modtype=C&userid=999999&password=XXXXXX&sitenumber=999999&format=J", provider);

                if (xModResponse != null)
                { 
                }

            }
            catch (System.Exception)
            {
            }

            return Json(xModResponse, JsonRequestBehavior.AllowGet);
        }

    }
}