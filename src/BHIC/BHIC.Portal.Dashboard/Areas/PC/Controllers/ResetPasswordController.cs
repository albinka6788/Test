using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BHIC.Core.PolicyCentre;
using BHIC.Contract.PolicyCentre;
using BHIC.Common;
using BHIC.Common.Logging;
using BHIC.Domain.PolicyCentre;
using Newtonsoft.Json;
using BHIC.Common.Mailing;
using System.Net.NetworkInformation;
using System.Net;
using BHIC.Domain.Dashboard;
using BHIC.Core.Background;
using BHIC.Portal.Dashboard.App_Start;
using System.Text;
using BHIC.Contract.Background;
using System.Reflection;
using System.Security;
using BHIC.Common.Configuration;
using BHIC.Common.XmlHelper;
using BHIC.Common.Client;

namespace BHIC.Portal.Dashboard.Areas.PC.Controllers
{
    [AllowAnonymous]
    [CustomAntiForgeryToken]
    public class ResetPasswordController : BaseController
    {
        IAccountRegistrationService accountRegistrationService;
        public ResetPasswordController()
        {
            accountRegistrationService = new AccountRegistrationService(guardServiceProvider);
        }
        //
        // GET: /PC/ResetPassword/

        public ActionResult ResetPassword()
        {
            return PartialView("ResetPassword");
        }

        public ActionResult GetEmail(string key)
        {
            try
            {
                string[] split = { "|:|" };
                List<string> cybEncryptedKeys = Encryption.DecryptText(key).Split(split, StringSplitOptions.None).ToList();
                string decryptedEmail = string.Empty;
                Session["key"] = key;
                if (accountRegistrationService.ForgotPwdRequestedDateTime("R", cybEncryptedKeys[0], cybEncryptedKeys[1]))
                {
                    return Json(new { status = true, decryptedEmail = cybEncryptedKeys[0] }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = true, decryptedEmail }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                loggingService.Fatal(string.Format("Method {0} executed with error message : {1}", MethodBase.GetCurrentMethod().Name, ex.ToString()));
                return Json(new { success = false, errorMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ResetPasswordRequest(ResetPasswordDTO postData)
        {
            try
            {
                string[] split = { "|:|" };
                string key = Session["key"].ToString();
                if (!key.Equals(postData.EmailId))
                {
                    return Json(new { success = false, errorMessage = "Please provide valid information." }, JsonRequestBehavior.AllowGet);
                }

                postData.EmailId = Encryption.DecryptText(postData.EmailId).Split(split, StringSplitOptions.None).ToList()[0];
                ModelState.Clear();
                TryValidateModel(postData);
                if (ModelState.IsValid)
                {
                    postData.SecurePassword = UtilityFunctions.ConvertToSecureString(postData.NewPassword);
                    postData.NewPassword = string.Empty;
                    postData.ConfirmPassword = string.Empty;

                    if (accountRegistrationService.ResetPassword(postData.EmailId, postData.SecurePassword))
                    {
                        UserRegistration userRegistration = accountRegistrationService.GetUserDetails(new UserRegistration() { Email = postData.EmailId });
                        string name = userRegistration.FirstName + " " + userRegistration.LastName;
                        string url = GetSchemeAndHostURLPart() + ":" + this.HttpContext.Request.Url.Port + "/" + ConfigCommonKeyReader.BaseUrlPath + "#/Login";

                        if (MailTemplateBuilder.SendMail(MailTemplateBuilder.MailResetPassword(GetEmailModel(postData.EmailId, name, url))))
                        {
                            Session.Remove("key");
                            return Json(new { success = true, successMessage = "Reset Password Success" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { success = false, errorMessage = "Error occurred while sending mail." }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { success = false, errorMessage = "Password Reset has been failed. Please try again." }, JsonRequestBehavior.AllowGet);
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
                return Json(new { success = false, errorMessage = "Password Reset has been failed. Please try again." }, JsonRequestBehavior.AllowGet);
            }
        }

        private Dictionary<string, string> GetEmailModel(string emailId, string name, string url)
        {
            Dictionary<string, string> model = new Dictionary<string, string>();
            ServiceProvider guardServiceProvider = new GuardServiceProvider() { ServiceCategory = ServiceProviderConstants.GuardServiceCategoryWC };
            ISystemVariableService systemVariable = new SystemVariableService(guardServiceProvider);

            model.Add("ClientEmailID", String.Join(";", emailId));
            model.Add("MailSubject", "useremail-reset-password-confirmation-subject");
            model.Add("MailBody", "useremail-reset-password-confirmation-body");
            model.Add("BaseUrl", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_WebsiteShortURL"]));
            model.Add("AbsoulteURL", GetEmailImageUrl());
            model.Add("IPAddress", GetUser_IP(this.ControllerContext.HttpContext));
            model.Add("Name", name);
            model.Add("LoginUrl", url);
            model.Add("SupportPhoneNumber", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["PhoneNumber_CSR"]));
            model.Add("SupportPhoneNumberHref", string.Concat("tel:", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["PhoneNumber_CSR"])));
            model.Add("WebsiteUrlText", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_Domain"]));
            model.Add("PasswordResetConfirmationEmailText", ConfigCommonKeyReader.ApplicationContactInfo["PasswordResetConfirmationEmailText"]);
            model.Add("PasswordResetConfirmationEmailHref", ConfigCommonKeyReader.ApplicationContactInfo["PasswordResetConfirmationEmailHref"]);

            return model;
        }
    }
}
