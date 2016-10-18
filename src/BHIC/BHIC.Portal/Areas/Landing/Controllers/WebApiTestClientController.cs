using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using BHIC.Portal.Code.Quote;
using BHIC.ViewDomain.Landing;
using BHIC.Domain.Policy;
using BHIC.Domain.Service;
using BHIC.Common;
using Newtonsoft.Json;
using BHIC.Domain.QuestionEngine;
using BHIC.Contract.Policy;
using BHIC.Common.Client;

namespace BHIC.Portal.Areas.Landing.Controllers
{
    public class WebApiTestClientController : Controller
    {
        private static int QuoteIdValue = 27259;

        public IAddressService addressService;
        public ICancellationRequestService cancellationRequestService;
        public IContactService contactService;
        public IExposureService exposureService;
        public IModifierService modifierService;
        public IPaymentTermService paymentTermService;
        public IPolicyDataService policyDataService;
        ServiceProvider provider = new GuardServiceProvider() { ProviderName = ProviderNames.Guard };
        public WebApiTestClientController(IAddressService _addressService, ICancellationRequestService _cancellationRequestService,
            IContactService _contactService, IExposureService _exposureService, IModifierService _modifierService, IPaymentTermService _paymentTermService, IPolicyDataService _policyDataService)
        {
            addressService = _addressService;
            cancellationRequestService = _cancellationRequestService;
            contactService = _contactService;
            exposureService = _exposureService;
            modifierService = _modifierService;
            policyDataService = _policyDataService;
            paymentTermService = _paymentTermService;
        }

        private void SetQuoteId(HttpContextBase context)
        {

            // ********** get the user's IP address
            string userIP = GetUser_IP(context);

            // ********** create the quote
            //landingViewModel.QuoteViewModel = CreateDsQuote(true, (User != null) ? User.Identity.Name : "", AdId, userIP);
            var postOperationStatus = SvcClientOld.CallService<QuoteStatus, OperationStatus>("QuoteStatus", "POST", new QuoteStatus { AdId = null, EnteredOn = DateTime.Now, UserIP = userIP });

            //Comment : Here if request successful then get new QuoteId
            if (postOperationStatus.RequestSuccessful)
            {
                //Comment : Here get QuoteId from returned effected id
                var effectedQuoteDTO = postOperationStatus.AffectedIds
                    .SingleOrDefault(res => res.DTOProperty == "QuoteId");
                //.Where(res => res.DTOProperty == "QuoteId")
                //.Select(res => new { res.IdValue });

                int QuoteId = Convert.ToInt32(effectedQuoteDTO.IdValue);

                ViewBag.QuoteId = QuoteId;
                QuoteIdValue = QuoteId;

                //System.Text.StringBuilder sb = new System.Text.StringBuilder("");
                //postOperationStatus.AffectedIds.ForEach(res => sb.AppendLine(res.IdValue));
                //string allIds = sb.ToString();
            }
        }

        protected string GetUser_IP(HttpContextBase context)
        {
            string VisitorsIPAddr = string.Empty;
            if (context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                VisitorsIPAddr = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else if (context.Request.UserHostAddress.Length != 0)
            {
                VisitorsIPAddr = context.Request.UserHostAddress;
            }
            return VisitorsIPAddr;
        }

        // GET: Landing/WebApiTestClient
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult TestAddressAPI(string message)
        {
            switch (message)
            {
                case "Get":
                    // SetQuoteId(this.ControllerContext.HttpContext);
                    AddressRequestParms addressReqParam = new AddressRequestParms();
                    addressReqParam.QuoteId = QuoteIdValue;
                    var resultGet = addressService.GetAddressList(addressReqParam);
                    break;
                case "Post":
                    Address address = new Address();
                    address.ContactId = 1;
                    var resultPost = addressService.AddAddress(address);
                    break;
                case "Delete":
                    AddressRequestParms addressReqParamDelete = new AddressRequestParms();
                    var resultDelete = addressService.DeleteAddress(addressReqParamDelete);
                    break;
            }

            return null;
        }

        public JsonResult TestCancellationRequestsAPI(string message)
        {
            switch (message)
            {
                case "Get":
                    CancellationRequestParms cancellationRequestParms = new CancellationRequestParms();
                    cancellationRequestParms.PolicyId = "1";

                    var resultGet = cancellationRequestService.GetCancellationRequestList(cancellationRequestParms);
                    break;
                case "Post":
                    CancellationRequest cancellationRequest = new CancellationRequest();
                    cancellationRequest.PolicyId = "1";
                    var resultPost = cancellationRequestService.AddCancellationRequest(cancellationRequest);
                    break;
            }

            return null;
        }

        public JsonResult TestContactRequestsAPI(string message)
        {
            switch (message)
            {
                case "Get":
                    ContactRequestParms contactRequest = new ContactRequestParms();
                    // SetQuoteId(this.ControllerContext.HttpContext);
                    contactRequest.QuoteId = QuoteIdValue;
                    var resultGet = contactService.GetContactList(contactRequest);
                    break;
                case "Post":
                    Contact contact = new Contact();
                    //Random r = new Random();
                    //contact.ContactId = r.Next();
                    //SetQuoteId(this.ControllerContext.HttpContext);
                    contact.ContactId = 1;
                    contact.Name = "Anuj"; contact.WebAddr = "www.abc@gmail.com"; contact.Title = "Mr"; contact.Company = "Xceedance";
                    contact.QuoteId = QuoteIdValue;
                    var resultPost = contactService.AddContact(contact);
                    break;
                case "Delete":
                    ContactRequestParms contactRequestDelete = new ContactRequestParms();
                    var resultDelete = contactService.DeleteContact(contactRequestDelete);
                    break;
            }

            return null;
        }

        public JsonResult TestExposureRequestsAPI(string message)
        {
            switch (message)
            {
                case "Get":
                    ExposureRequestParms exposureRequest = new ExposureRequestParms();
                    // SetQuoteId(this.ControllerContext.HttpContext);
                    exposureRequest.CoverageStateId = QuoteIdValue;
                    var resultGet = exposureService.GetExposureList(exposureRequest, provider);
                    break;
                case "Post":
                    Exposure exposure = new Exposure();
                    exposure.IndustryId = 1;
                    exposure.SubIndustryId = 1;
                    exposure.ClassDescriptionId = 1;
                    exposure.QuoteId = QuoteIdValue;
                    exposure.ExposureAmt = 20000;
                    exposure.ZipCode = "90012";
                    exposure.ExposureId = null;
                    var resultPost = exposureService.AddExposure(exposure, provider);
                    break;
                case "Delete":
                    ExposureRequestParms exposureRequestDelete = new ExposureRequestParms();
                    exposureRequestDelete.CoverageStateId = 27259;
                    exposureRequestDelete.ExposureId = 17931;
                    var resultDelete = exposureService.DeleteExposure(exposureRequestDelete, provider);
                    break;
            }

            return null;
        }

        public JsonResult TestModifierRequestsAPI(string message)
        {
            switch (message)
            {
                case "Get":
                    ModifierRequestParms modifierRequest = new ModifierRequestParms();
                    modifierRequest.QuoteId = QuoteIdValue;
                    var resultGet = modifierService.GetModifierList(modifierRequest);
                    break;
                case "Post":
                    BHIC.Domain.Policy.Modifier modifier = new BHIC.Domain.Policy.Modifier();
                    modifier.QuoteId = QuoteIdValue;
                    modifier.ModType = "Test";
                    modifier.ModValue = 1200;
                    var resultPost = modifierService.AddModifier(modifier);
                    break;
                case "Delete":
                    ModifierRequestParms modifierRequestDelete = new ModifierRequestParms();
                    modifierRequestDelete.QuoteId = 27259;
                    modifierRequestDelete.ModifierId = 1354;
                    var resultDelete = modifierService.DeleteModifier(modifierRequestDelete);
                    break;
            }

            return null;
        }

        public JsonResult TestPaymentTermsRequestsAPI(string message)
        {
            switch (message)
            {
                case "Get":
                    PaymentTermsRequestParms paymentTermRequest = new PaymentTermsRequestParms();
                    paymentTermRequest.QuoteId = QuoteIdValue;
                    var resultGet = paymentTermService.GetPaymentTermsList(paymentTermRequest);
                    break;
                case "Post":
                    PaymentTerms paymentTerms = new PaymentTerms();
                    paymentTerms.QuoteId = QuoteIdValue;
                    paymentTerms.PaymentPlanId = 1;
                    var resultPost = paymentTermService.AddPaymentTerms(paymentTerms);
                    break;
                case "Delete":
                    PaymentTermsRequestParms paymentTermRequestDelete = new PaymentTermsRequestParms();
                    paymentTermRequestDelete.QuoteId = QuoteIdValue;
                    var resultDelete = paymentTermService.DeletePaymentTerms(paymentTermRequestDelete);
                    break;
            }
            return null;
        }

        public JsonResult TestPolicyDataRequestsAPI(string message)
        {
            switch (message)
            {
                case "Get":
                    PolicyDataRequestParms policyDataRequest = new PolicyDataRequestParms();
                    policyDataRequest.QuoteId = QuoteIdValue;
                    var resultGet = policyDataService.GetPolicyData(policyDataRequest);
                    break;
                case "Post":
                    // init DTO
                    PolicyData policyData = new PolicyData();
                    // current quote id
                    policyData.QuoteId = QuoteIdValue;
                    // set policy start date
                    policyData.InceptionDate = DateTime.Now.AddDays(10);
                    var resultPost = policyDataService.AddPolicyData(policyData);
                    break;
                case "Delete":
                    PolicyDataRequestParms policyDataRequestDelete = new PolicyDataRequestParms();
                    policyDataRequestDelete.QuoteId = QuoteIdValue;
                    var resultDelete = policyDataService.DeletePolicyData(policyDataRequestDelete);
                    break;
            }
            return null;
        }
    }
}