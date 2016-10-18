using BHIC.Common;
using BHIC.Common.Client;
using BHIC.Common.Config;
using BHIC.Contract.Dashboard;
using BHIC.Contract.Policy;
using BHIC.Contract.PolicyCentre;
using BHIC.Core.Dashboard;
using BHIC.Core.Policy;
using BHIC.Core.PolicyCentre;
using BHIC.Domain.Dashboard;
using BHIC.Domain.Policy;
using BHIC.Portal.Dashboard.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using BHIC.Common.Logging;
using System.Text;
using System.Reflection;

namespace BHIC.Portal.Dashboard.Areas.PC.Controllers
{
    [CustomAuthorize]
    [CustomAntiForgeryToken]
    public class RequestCertificateController : BaseController
    {
        //
        // GET: /PolicyCentre/RequestCertificate/       
        ServiceProvider _provider = new GuardServiceProvider() { ProviderName = ProviderNames.Guard, ServiceCategory = LineOfBusiness };
        private readonly IRequestCertificateService _requestCertificateService;

        public RequestCertificateController()
        {
            _requestCertificateService = new RequestCertificateService(guardServiceProvider);
        }

        public ActionResult RequestCertificate()
        {

            return PartialView("RequestCertificate");
        }

        /// <summary>        
        /// This method gets all the Certificates for the given policy number.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetCertificateofInsurance(string CYBKey)
        {
            try
            {
                PolicyInformation decryptedKey = DecryptedCYBKey(CYBKey);

                string policyCode = decryptedKey.PolicyCode;
                string state = string.Empty;
                if (!string.IsNullOrWhiteSpace(policyCode))
                {
                    List<CertificateOfInsurance> certificates = new List<CertificateOfInsurance>();

                    // Fetch the list of certificates from API for the given policy number
                    var certificateOfInsuranceResponse = _requestCertificateService.GetRequestCertificate(
                            new CertificateOfInsuranceRequestParms { PolicyCode = policyCode, SessionId = Session.SessionID, UserId = UserSession().Email });
                    if (certificateOfInsuranceResponse.OperationStatus.RequestSuccessful)
                    {
                        foreach (var item in certificateOfInsuranceResponse.CertificatesOfInsurance)
                        {
                            if (item.Document.DocumentId > 0)
                            {
                                item.Document.EncryptedDocumentId = Server.UrlEncode(Encryption.EncryptText(Convert.ToString(item.Document.DocumentId)));
                                certificates.Add(item);
                            }
                        }
                    }

                    //For getting State
                    IPolicyDetailsService policyDetailsService = new PolicyDetailsService(guardServiceProvider);
                    PolicyDetailsResponse policyDetailsResponse = policyDetailsService.GetPolicyDetails(new PolicyDetailsRequestParms() { PolicyCode = policyCode, UserId = UserSession().Email });
                    if (policyDetailsResponse.OperationStatus.RequestProcessed)
                    {
                        var cityStateZip = policyDetailsResponse.PolicyDetails.Insured.CityStateZip;
                        if (cityStateZip.Contains(","))
                        {
                            state = (cityStateZip.Split(',')[1]).Split(' ')[1];
                        }
                    }

                    //return the final list of certificates as json data
                    return Json(new
                    {
                        status = true,
                        errorMessage = "",
                        certificates,
                        user = new
                        {
                            Name = decryptedKey.PolicyUserContact.UserName,
                            PhoneNumber = decryptedKey.PolicyUserContact.PolicyContactNumber,
                            Email = decryptedKey.PolicyUserContact.PolicyEmail
                        },
                        state
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                loggingService.Fatal(string.Format("Method {0} executed with error message : {1}", MethodBase.GetCurrentMethod().Name, ex.ToString()));
            }
            return Json(new { status = false, redirectStatus = true, errorMessage = Constants.ExceptionMessage }, JsonRequestBehavior.AllowGet);
        }

        //Post: /RequestCertificate/GetCertificate/
        [HttpPost]
        public ActionResult RequestCertificate(CertificateOfInsurance postData)
        {
            try
            {
                PolicyInformation decryptedKey = DecryptedCYBKey(postData.PolicyCode);
                string policyCode = decryptedKey.PolicyCode;
                if (!string.IsNullOrWhiteSpace(policyCode))
                {
                    postData.EmailTo = UserSession().Email;
                    postData.PolicyCode = policyCode;
                    postData.RequestDate = DateTime.Now;
                    postData.UserId = UserSession().Email;
                    IPolicyDetailsService policyDetailsService = new PolicyDetailsService(guardServiceProvider);
                    PolicyDetailsResponse policyDetailsResponse = policyDetailsService.GetPolicyDetails(new PolicyDetailsRequestParms() { PolicyCode = policyCode, UserId = UserSession().Email });

                    if (!policyDetailsResponse.OperationStatus.RequestProcessed)
                    {
                        return Content(policyDetailsResponse.OperationStatus.Messages.FirstOrDefault().Text);
                    }
                    postData.StartDate = policyDetailsResponse.PolicyDetails.PolicyBegin;
                    postData.EndDate = policyDetailsResponse.PolicyDetails.PolicyExpires;

                    //Comment: Validate the model before posting the data to Guard API
                    ModelState.Clear();
                    TryValidateModel(postData);
                    if (ModelState.IsValid)
                    {
                        //Comment: Calling the Guard API for Request Certificate of Insurance
                        var operation = _requestCertificateService.AddRequestCertificate(postData);
                        if (operation.RequestSuccessful)
                        {
                            var certificateOfInsuranceResponse = _requestCertificateService.GetRequestCertificate(
                                new CertificateOfInsuranceRequestParms { PolicyCode = policyCode, SessionId = Session.SessionID, UserId = UserSession().Email });

                            var requestId = operation.AffectedIds.FirstOrDefault().IdValue;

                            var documentId = certificateOfInsuranceResponse.CertificatesOfInsurance.FirstOrDefault(t => t.RequestId == requestId).Document.DocumentId;
                            return Json(new { success = true, documentId }, JsonRequestBehavior.AllowGet);
                        }

                        //Comment: Raise the API error returned from Guard for Invalid city, state and zip code
                        string apiError = operation.Messages.FirstOrDefault().Text;
                        if (string.IsNullOrWhiteSpace(apiError))
                            return Json(new { status = false, errorMessage = apiError }, JsonRequestBehavior.AllowGet);
                        if (apiError.Contains("<!"))
                        {
                            apiError = apiError.Split('<')[0];
                        }
                        return Json(new { status = false, errorMessage = apiError }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { success = false, errorMessage = GetModelError() }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { status = false, errorMessage = Constants.SessionExpired }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                loggingService.Fatal(string.Format("Method {0} executed with error message : {1}", MethodBase.GetCurrentMethod().Name, ex));
                return Json(new { status = false, redirectStatus = true, errorMessage = Constants.ExceptionMessage }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
