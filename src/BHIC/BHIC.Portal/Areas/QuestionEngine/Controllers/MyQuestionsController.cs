using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

//Comment : Here Additional Library
using BHIC.Contract.QuestionEngine;
using BHIC.Core.QuestionEngine;
using BHIC.Domain.QuestionEngine;
using System.Configuration;
using BHIC.Portal.Areas.QuestionEngine.Models;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using BHIC.Domain.Background;
using BHIC.Domain.Policy;
using BHIC.Domain.Service;
using BHIC.Common;


namespace BHIC.Portal.Areas.QuestionEngine.Controllers
{
    public class MyQuestionsController : Controller
    {
        #region Comment : Here Global variable declaration and initialization

        //Comment : Here create question service intance for getting data
        private readonly IMyQuestionsService objQuestionService;

        //Comment : Here get all seacrh inputs for data fetch
        MyQuestionParameters objQuestionParams = null;

        //Comment : Here Underline serive URLs of API application is consuming
        //string UrlAPI_Root = ConfigurationManager.AppSettings["ServiceRootPath"].ToString();
        string UrlAPI = (ConfigurationManager.AppSettings["ServiceRootPath"].ToString().Trim() == "") ?
                         ConfigurationManager.AppSettings["QuestionServiceRelativeUrl"].ToString() :
                         ConfigurationManager.AppSettings["QuestionServiceAbsoluteUrl"].ToString();


        #endregion

        #region Comment : Here Global variable declaration and initialization

        //Comment : Here for DI & IoC concept inject interface into controller constructor
        public MyQuestionsController(IMyQuestionsService objQuestionService)
        {
            this.objQuestionService = objQuestionService;
            //this.objQuestionService.UrlAPI = this.UrlAPI;
        }

        #endregion

        #region Comment : Here Question Controller Action Method

        public ActionResult Index()
        {
            return View();
        }

        // GET: QuestionEngine/Questions -- List
        public ActionResult MyGetQuestions(int? id)
        {

            MyQuestionsViewModel questionViewModel = null;

            try
            {
                if (id == null)
                {
                    //Comment : Here get result from Service
                    var model = this.objQuestionService.Questions.ToList();

                    //Comment : Here map these fetched details into BHIC.Portal ViewModel class property
                    questionViewModel = new MyQuestionsViewModel { QuestionsList = model };
                }
                else
                {
                    //Comment : Here to filter search result pass params to question serach engine interface method
                    objQuestionParams = new MyQuestionParameters();
                    objQuestionParams.ZipCode = id ?? 1;

                    //Comment : Here get result from Service
                    var model = this.objQuestionService.GetQuestionsList(objQuestionParams).ToList();

                    //Comment : Here map these fetched details into BHIC.Portal ViewModel class property
                    questionViewModel = new MyQuestionsViewModel { QuestionsList = model };
                }
            }
            catch (Exception Ex)
            {
                ViewBag.Error = Ex.Message;
            }

            //Comment : Here map these fetched details into BHIC.Portal ViewModel class property
            //var questionViewModel = new MyQuestionsViewModel { QuestionsList = model };

            //Comment : Here retrun BHIC.Portal ViewModel class result to UI View layer
            return View("MyGetQuestions", questionViewModel);

            //return Json(new { id = 5, Name = "Prem" }, JsonRequestBehavior.AllowGet);
        }

        // GET: QuestionEngine/Questions -- List
        public ActionResult MyGetQuestionsJson(int? id)
        {
            try
            {
                if (id == null)
                {
                    //Comment : Here to filter search result pass params to question serach engine interface method
                    objQuestionParams = new MyQuestionParameters();
                    objQuestionParams.ZipCode = 1;

                    //Comment : Here get result from Service
                    var model = this.objQuestionService.GetQuestionsJson();

                    return Json(model, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //Comment : Here to filter search result pass params to question serach engine interface method
                    objQuestionParams = new MyQuestionParameters();
                    objQuestionParams.ZipCode = id ?? 1;

                    //Comment : Here get result from Service
                    var model = this.objQuestionService.GetQuestionsJson(objQuestionParams);

                    return Json(model, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception Ex)
            {
                ViewBag.Error = Ex.Message;
            }

            //Comment : Here default Json string
            return Json(new { id = 5, Name = "Prem" }, JsonRequestBehavior.AllowGet);
        }

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

   

        #endregion

        #region Comment : Here Question Controller (AngularJS) Action Methods

        public ActionResult MyGetQuestionsByAngular()
        {
            return View();
        }

        public ActionResult MyShowQuestions()
        {
            return View();
        }

        //public JsonResult 

        #endregion

        #region Comment : Here Guard API Service Action Methods

        public JsonResult SingleServiceCall()
        {
            // get the credentials required to request a security token
            var inputs = new NameValueCollection();
            inputs.Add("grant_type", "password");
            inputs.Add("username", "api@guard.com");
            inputs.Add("password", "Guard1234");

            string token = "";
            string responseJson = string.Empty;
            using (var client = new WebClient())
            {
                // get the token that's required for each request
                var tokenResponse = client.UploadValues("https://ydsml.guard.com/inssvc/auth", inputs);
                var oauthResponse = JsonConvert.DeserializeObject<OAuthModel>(Encoding.UTF8.GetString(tokenResponse));
                token = oauthResponse.AccessToken;

                // add the token to the header and execute the request
                client.Headers.Add("Authorization", "Bearer " + token);
                var svcResponse = client.DownloadString("https://ydsml.guard.com/inssvc/api/Industries");

                // deserialize / return the data
                var industryResponse = JsonConvert.DeserializeObject<IndustryResponse>(svcResponse);

                // do something with the data
                //responseJson = JsonConvert.SerializeObject(industryResponse.Industries);
                responseJson = svcResponse;
            }

            return Json(responseJson, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SingleServiceCall2()
        {
            //Comment: Here other variant for Single service call, in this authentication code wrapped inside calle method
            var industryResponse = SvcClientOld.CallService<IndustryResponse>("Industries");

            // do something with the data
            string responseJson = JsonConvert.SerializeObject(industryResponse.Industries);

            return Json(new { Industries = responseJson }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BatchServiceCall()
        {
            //Comment : Here to call Batch of serices in single execution create BatchAction and add in list for BatchServiceCall 
            var batchAction1 = new BatchAction();
            batchAction1.RequestIdentifier = "List Of Industries";
            batchAction1.ServiceName = "Industries";
            batchAction1.ServiceMethod = "GET";
            batchAction1.BatchActionParameters.Add(new BatchActionParameter { Name = "industryRequestParms", Value = JsonConvert.SerializeObject(new IndustryRequestParms { }) });

            //Comment : Here second batchAction to get sub-ndustries details 
            var batchAction2 = new BatchAction()
            {
                RequestIdentifier = "List Of Sub-Industries"
                ,
                ServiceName = "SubIndustries"
                ,
                ServiceMethod = "GET"
                ,
                BatchActionParameters = new List<BatchActionParameter> { new BatchActionParameter() { Name = "subIndustryRequestParms", Value = JsonConvert.SerializeObject(new SubIndustryRequestParms() { IndustryId = 10, Lob = null }) } }
            };

            //Comment : Here create bacthAction list object 
            var batchActionList = new BatchActionList();
            //Comment : Here { WAY -1 }
            batchActionList.BatchActions = new List<BatchAction>() { batchAction1, batchAction2 };
            //Comment : Here { WAY -2 }
            //batchActionList.BatchActions.Add(batchAction1);
            //batchActionList.BatchActions.Add(batchAction2);


            //Comment : Here object to grab returned bacth response
            BatchResponseList batchResponseList;

            //Comment : Here call batchService call and get ListOfResponse object from string data
            var jsonResponse = SvcClientOld.CallServiceBatch(batchActionList);
            batchResponseList = JsonConvert.DeserializeObject<BatchResponseList>(jsonResponse);

            //Comment : Here local variables
            string industryJson = string.Empty, subIndustryJson = string.Empty;

            //Comment : Here get individual requeset response from responseList by iterating it
            foreach (var specificResponse in batchResponseList.BatchResponses)
            {
                var requestIdentifier = specificResponse.RequestIdentifier;

                //Comment : Here based on passed key/rerquestIdentifier get resultant data 
                if (requestIdentifier.Equals("List Of Industries"))
                {
                    var thisRequestData = specificResponse.JsonResponse;

                    //Comment : Here we can get data into our custome .net class/object type after deserialization
                    var industryObject = JsonConvert.DeserializeObject<IndustryResponse>(thisRequestData);

                    industryJson = thisRequestData;
                }

                //Comment : Here based on passed key/rerquestIdentifier get resultant data 
                if (requestIdentifier.Equals("List Of Sub-Industries"))
                {
                    var thisRequestData = specificResponse.JsonResponse;

                    //Comment : Here we can get data into our custome .net class/object type after deserialization
                    var industryObject = JsonConvert.DeserializeObject<SubIndustryResponse>(thisRequestData);

                    subIndustryJson = thisRequestData;
                }
            }

            //Comment : Here finally return thid clubed & categorised data
            return Json(new { industryJson = industryJson, subIndustryJson = subIndustryJson }, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult 

        #endregion
    }
}