using BHIC.Common;
using BHIC.Common.Client;
using BHIC.Common.Config;
using BHIC.Common.XmlHelper;
using BHIC.Contract.Background;
using BHIC.Contract.Policy;
using BHIC.Contract.PolicyCentre;
using BHIC.Core.Background;
using BHIC.Core.Policy;
using BHIC.Core.PolicyCentre;
using BHIC.Domain.Dashboard;
using BHIC.Domain.Policy;
using BHIC.Domain.PolicyCentre;
using BHIC.Domain.Service;
using BHIC.Portal.Dashboard.App_Start;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;

namespace BHIC.Portal.Dashboard.Areas.PC.Controllers
{
    [CustomAuthorize]
    [CustomAntiForgeryToken]
    public class CancelPolicyController : BaseController
    {
        private ICancellationRequestService _cancellationRequestService;

        public ActionResult CancelPolicy()
        {
            return PartialView("CancelPolicy");
        }

        [HttpPost]
        public JsonResult GetPolicyCancelOptions(string CYBKey)
        {
            try
            {
                IDashboardService dashboardService = new DashboardService();
                PolicyInformation policyInfo = DecryptedCYBKey(CYBKey);
                var ExpiryDate = policyInfo.ExpiryDate;
                List<DropDownOptions> lobPolicyChangeOptions = dashboardService.GetDropdownOptions(policyInfo.LOB, Constants.PolicyCancelDropdown);
                return Json(new { success = true, options = lobPolicyChangeOptions, ExpiryDate }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                loggingService.Fatal(string.Format("Method {0} executed with error message : {1}", MethodBase.GetCurrentMethod().Name, ex.ToString()));
                return Json(new { status = false, redirectStatus = true, errorMessage = Constants.ExceptionMessage }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult CancelPolicy(CancellationRequest postData)
        {
            try
            {
                string modelErrors = string.Empty;
                PolicyInformation policyInformation = DecryptedCYBKey(postData.PolicyId);

                if (!Helper_ValidateModel(postData, policyInformation, ref modelErrors))
                {
                    return Json(new { success = false, errorMessage = modelErrors }, JsonRequestBehavior.AllowGet);
                }

                postData.RequestDate = DateTime.Now;
                postData.PolicyId = policyInformation.PolicyCode;
                postData.ContactIpAddress = GetUser_IP(this.ControllerContext.HttpContext);
                postData.ContactEmail = policyInformation.PolicyUserContact.PolicyEmail;
                postData.ContactName = policyInformation.PolicyUserContact.UserName;
                postData.ContactPhoneNumber = policyInformation.PolicyUserContact.PolicyContactNumber;

                ModelState.Clear();
                TryValidateModel(postData);
                if (ModelState.IsValid)
                {
                    //Post Cancellation Request to Guard API
                    _cancellationRequestService = new CancellationRequestService(guardServiceProvider);
                    OperationStatus operationStatus = _cancellationRequestService.AddCancellationRequest(postData);
                    if (operationStatus.RequestSuccessful)
                    {
                        //If Request is Successful, trigger an Email to Guard
                        if (MailTemplateBuilder.SendMail(MailTemplateBuilder.MailCancelPolicy(GetEmailModel(postData))))
                        {
                            return Json(new { success = true, successMessage = "Policy cancel request sent successfully..." }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { success = false, errorMessage = "Error occurred while sending mail." }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else
                {
                    return Json(new { success = false, errorMessage = GetModelError() }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                loggingService.Fatal(string.Format("Method {0} executed with error message : {1}", MethodBase.GetCurrentMethod().Name, ex.ToString()));
            }

            return Json(new { success = false, redirectStatus = true, errorMessage = Constants.ExceptionMessage }, JsonRequestBehavior.AllowGet);
        }

        private Dictionary<string, string> GetEmailModel(CancellationRequest postData)
        {
            Dictionary<string, string> model = new Dictionary<string, string>();
            ServiceProvider guardServiceProvider = new GuardServiceProvider() { ServiceCategory = ServiceProviderConstants.GuardServiceCategoryWC };
            ISystemVariableService systemVariable = new SystemVariableService(guardServiceProvider);

            model.Add("ClientEmailID", String.Join(";", ConfigCommonKeyReader.ClientEmailIdCancelPolicy));
            model.Add("MailSubject", "useremail-policycancellation-subject");
            model.Add("MailBody", "useremail-policycancellation-body");
            model.Add("BaseUrl", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_WebsiteShortURL"]));
            model.Add("AbsoulteURL", GetEmailImageUrl());
            model.Add("IPAddress", postData.ContactIpAddress);
            model.Add("PolicyCode", postData.PolicyId);
            model.Add("CancelPolicyEffectiveDate", Convert.ToDateTime(postData.RequestedEffectiveDate).ToShortDateString());
            model.Add("CancelPolicyName", postData.ContactName);
            model.Add("CancelPolicyContactNumber", postData.ContactPhoneNumber);
            model.Add("CancelPolicyEmail", postData.ContactEmail);
            model.Add("CancelPolicyReason", postData.ReasonForCancellation);
            model.Add("CancelDescription", string.IsNullOrWhiteSpace(postData.Description) ? "----" : postData.Description);
            model.Add("SupportPhoneNumber", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["PhoneNumber_CSR"]));
            model.Add("SupportPhoneNumberHref", string.Concat("tel:", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["PhoneNumber_CSR"])));
            model.Add("WebsiteUrlText", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_Domain"]));
            model.Add("SupportEmailTextSalesSupport", ConfigCommonKeyReader.ApplicationContactInfo["SupportEmailTextSalesSupport"]);
            model.Add("SupportEmailHrefSalesSupport", ConfigCommonKeyReader.ApplicationContactInfo["SupportEmailHrefSalesSupport"]);

            return model;
        }

        private bool Helper_ValidateModel(CancellationRequest postdata, PolicyInformation policyinfo, ref string errorMessage)
        {
            bool isModelValid = true;
            DateTime requestedDate = UtilityFunctions.ConvertToDate(postdata.EffectiveDate);
            int lob = policyinfo.LOB;
            DateTime policyExpires = policyinfo.ExpiryDate;
            if (requestedDate != new DateTime())
            {
                postdata.RequestedEffectiveDate = requestedDate;
                if (Convert.ToDateTime(requestedDate.ToString("MM/dd/yyyy")) < Convert.ToDateTime(DateTime.Today.ToString("MM/dd/yyyy")) || Convert.ToDateTime(requestedDate.ToString("MM/dd/yyyy")) > Convert.ToDateTime(policyinfo.ExpiryDate.ToString("MM/dd/yyyy")))
                {
                    errorMessage = "Requested Effective Date is not valid";
                    isModelValid = false;
                }
            }

            IDashboardService dashboardService = new DashboardService();
            DropDownOptions lobPolicyChangeOptions = dashboardService.GetDropdownOptions(lob, Constants.PolicyCancelDropdown, postdata.ReasonID);

            if (lobPolicyChangeOptions.value != null)
            {
                if (lobPolicyChangeOptions.value.Equals("Other", StringComparison.OrdinalIgnoreCase)
                    && string.IsNullOrEmpty(postdata.Description))
                {
                    errorMessage = "Description is required.";
                    isModelValid = false;
                }
                else
                {
                    postdata.ReasonForCancellation = lobPolicyChangeOptions.value;
                }
            }
            else
            {
                errorMessage = "Please select valid option";
                isModelValid = false;
            }

            return isModelValid;
        }
    }
}
