using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BHIC.Portal.Dashboard.App_Start;
using BHIC.Common.XmlHelper;
using BHIC.Contract.PolicyCentre;
using BHIC.Core.PolicyCentre;
using BHIC.Domain.Dashboard;
using BHIC.Domain.PolicyCentre;
using System.Reflection;
using BHIC.Common.Config;
using BHIC.Common;
using BHIC.Common.Client;
using BHIC.Contract.Background;
using BHIC.Core.Background;

namespace BHIC.Portal.Dashboard.Areas.PC.Controllers
{
    [CustomAuthorize]
    [CustomAntiForgeryToken]
    public class RequestPolicyChangeController : BaseController
    {
        #region Methods

        public ActionResult RequestPolicyChange()
        {
            return PartialView("RequestPolicyChange");
        }

        [HttpPost]
        public JsonResult GetLobPolicyChangeOptions(string CYBKey)
        {
            try
            {
                IDashboardService dashboardService = new DashboardService();
                PolicyInformation policyInfo = DecryptedCYBKey(CYBKey);
                var ExpiryDate = policyInfo.ExpiryDate;
                if (policyInfo.Status != null)
                {
                    List<DropDownOptions> lobPolicyChangeOptions = dashboardService.GetDropdownOptions(policyInfo.LOB, Constants.PolicyChangeDropdown);
                    return Json(new { success = true, options = lobPolicyChangeOptions, ExpiryDate }, JsonRequestBehavior.AllowGet);
                }
            }

            catch (Exception ex)
            {
                loggingService.Fatal(string.Format("Method {0} executed with error message : {1}", MethodBase.GetCurrentMethod().Name, ex.ToString()));
            }
            return Json(new { status = false, redirectStatus = true, errorMessage = Constants.ExceptionMessage }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult SendPolicyChangeRequest(RequestPolicyChangeDTO inputParameter)
        {
            try
            {
                //Model validation
                string modelErrors = string.Empty;
                PolicyInformation policyInformation = DecryptedCYBKey(inputParameter.PolicyNumber);
                inputParameter.PolicyNumber = policyInformation.PolicyCode;

                if (!Helper_ValidateModel(inputParameter, policyInformation, ref modelErrors))
                {
                    return Json(new { success = false, errorMessage = modelErrors }, JsonRequestBehavior.AllowGet);
                }
                if (ModelState.IsValid)
                {
                    if (MailTemplateBuilder.SendMail(MailTemplateBuilder.MailRequestPolicyChange(GetEmailModel(inputParameter, policyInformation.PolicyUserContact))))
                    {
                        return Json(new { success = true, successMessage = "Policy change request sent successfully..." }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, errorMessage = "Error occurred while sending mail." }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { success = false, errorMessage = GetModelError() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                loggingService.Fatal(string.Format("Method {0} executed with error message : {1}", MethodBase.GetCurrentMethod().Name, ex.ToString()));
                return Json(new { success = false, redirectStatus = true, errorMessage = Constants.ExceptionMessage }, JsonRequestBehavior.AllowGet);
            }
        }


        private Dictionary<string, string> GetEmailModel(RequestPolicyChangeDTO postData, PolicyUser PolicyUserContact)
        {
            string name = (UserSession().FirstName + " " + UserSession().LastName).Trim();

            ServiceProvider guardServiceProvider = new GuardServiceProvider() { ServiceCategory = ServiceProviderConstants.GuardServiceCategoryWC };
            ISystemVariableService systemVariable = new SystemVariableService(guardServiceProvider);

            Dictionary<string, string> model = new Dictionary<string, string>();
            model.Add("ClientEmailID", String.Join(";", ConfigCommonKeyReader.ClientEmailIdPolicyChangeRequest));
            model.Add("MailSubject", "useremail-policychange-subject");
            model.Add("MailBody", "useremail-policychange-body");
            model.Add("BaseUrl", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_WebsiteShortURL"]));
            model.Add("AbsoulteURL", GetEmailImageUrl());
            model.Add("IPAddress", GetUser_IP(this.ControllerContext.HttpContext));
            model.Add("PolicyCode", postData.PolicyNumber);
            model.Add("RequestEffectiveDate", postData.Effectivedate);
            model.Add("ChangePolicyName", PolicyUserContact.UserName);//  Name);
            model.Add("ChangePolicyContactNumber", PolicyUserContact.PolicyContactNumber);// UserSession().PhoneNumber.ToString());
            model.Add("ChangePolicyEmail", PolicyUserContact.PolicyEmail);// UserSession().Email);
            model.Add("ChangeType", postData.SelectedItem);
            model.Add("Description", postData.Description);
            model.Add("SupportPhoneNumber", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["PhoneNumber_CSR"]));
            model.Add("SupportPhoneNumberHref", string.Concat("tel:", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["PhoneNumber_CSR"])));
            model.Add("SupportEmailTextSalesSupport", ConfigCommonKeyReader.ApplicationContactInfo["SupportEmailTextSalesSupport"]);
            model.Add("SupportEmailHrefSalesSupport", ConfigCommonKeyReader.ApplicationContactInfo["SupportEmailHrefSalesSupport"]);

            model.Add("WebsiteUrlText", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_Domain"]));

            return model;
        }

        private bool Helper_ValidateModel(RequestPolicyChangeDTO inputParameter, PolicyInformation policyinfo, ref string errorMessage)
        {
            bool isModelValid = true;
            DateTime requestedDate = UtilityFunctions.ConvertToDate(inputParameter.Effectivedate);
            int lob = policyinfo.LOB;
            if (requestedDate != new DateTime())
            {
                if (Convert.ToDateTime(requestedDate.ToString("MM/dd/yyyy")) <= Convert.ToDateTime(DateTime.Today.ToString("MM/dd/yyyy")) || Convert.ToDateTime(requestedDate.ToString("MM/dd/yyyy")) > Convert.ToDateTime(policyinfo.ExpiryDate.ToString("MM/dd/yyyy")))
                {
                    errorMessage = "Effective Date is not valid";
                    isModelValid = false;
                }
            }

            IDashboardService dashboardService = new DashboardService();
            DropDownOptions lobPolicyChangeOptions = dashboardService.GetDropdownOptions(lob, Constants.PolicyChangeDropdown, inputParameter.SelectedID);

            if (lobPolicyChangeOptions.value != null)
            {
                inputParameter.SelectedItem = lobPolicyChangeOptions.value;
            }
            else
            {
                errorMessage = "Please select valid option";
                isModelValid = false;
            }
            return isModelValid;
        }

        #endregion
    }
}
