using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BHIC.Core.PolicyCentre;
using BHIC.Domain.Dashboard;
using BHIC.Core.Background;
using BHIC.Portal.Dashboard.App_Start;
using BHIC.Common;
using System.Reflection;
using BHIC.Common.Config;
using BHIC.Common.XmlHelper;
using BHIC.Common.Client;
using BHIC.Contract.Background;

namespace BHIC.Portal.Dashboard.Areas.PC.Controllers
{
    [CustomAuthorize]
    [CustomAntiForgeryToken]
    public class ChangePasswordController : BaseController
    {
        //
        // GET: /PC/ChangePassword/

        public ActionResult ChangePassword()
        {
            return PartialView("ChangePassword");
        }
        [HttpPost]
        public JsonResult PasswordChangeRequest(ChangePasswordDTO postData)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string email = UserSession().Email;
                    AccountRegistrationService accountRegistrationService = new AccountRegistrationService(guardServiceProvider);

                    postData.SecureNewPassword = UtilityFunctions.ConvertToSecureString(Encryption.EncryptText(postData.NewPassword, email.ToUpper()));
                    postData.SecureOldPassword = UtilityFunctions.ConvertToSecureString(postData.OldPassword); 

                    postData.NewPassword = string.Empty; 
                    postData.OldPassword = string.Empty;
                    postData.ConfirmPassword = string.Empty;

                    if (accountRegistrationService.ChangePassword(email, postData.SecureOldPassword, postData.SecureNewPassword))
                    {
                        UserSession().Password = postData.NewPassword;

                        if (MailTemplateBuilder.SendMail(MailTemplateBuilder.MailChangePassword(GetEmailModel())))
                        {
                            return Json(new { success = true, successMessage = "Password Changed Successfully" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { success = false, errorMessage = "Error occurred while sending mail." }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    return Json(new { success = false, errorMessage = "Old Password is not correct." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, errorMessage = GetModelError() }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                loggingService.Fatal(string.Format("Method {0} executed with error message : {1}", MethodBase.GetCurrentMethod().Name, ex.ToString()));
                return Json(new { success = false, errorMessage = Constants.ExceptionMessage }, JsonRequestBehavior.AllowGet);
            }
        }

        private Dictionary<string, string> GetEmailModel()
        {
            string name = UserSession().FirstName + " " + UserSession().LastName;
            string URL = GetSchemeAndHostURLPart() + ":" + this.HttpContext.Request.Url.Port + "/" + ConfigCommonKeyReader.BaseUrlPath + "#/Login";
            ServiceProvider guardServiceProvider = new GuardServiceProvider() { ServiceCategory = ServiceProviderConstants.GuardServiceCategoryWC };
            ISystemVariableService systemVariable = new SystemVariableService(guardServiceProvider);

            Dictionary<string, string> model = new Dictionary<string, string>();
            model.Add("ClientEmailID", String.Join(";", UserSession().Email));
            model.Add("MailSubject", "useremail-reset-password-confirmation-subject");
            model.Add("MailBody", "useremail-reset-password-confirmation-body");
            model.Add("BaseUrl", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_WebsiteShortURL"]));
            model.Add("AbsoulteURL", GetEmailImageUrl());
            model.Add("IPAddress", GetUser_IP(this.ControllerContext.HttpContext));
            model.Add("Name", name);
            model.Add("LoginUrl", URL);
            model.Add("SupportPhoneNumber", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["PhoneNumber_CSR"]));
            model.Add("SupportPhoneNumberHref", string.Concat("tel:", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["PhoneNumber_CSR"])));
            model.Add("WebsiteUrlText", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_Domain"]));
            model.Add("PasswordResetConfirmationEmailText", ConfigCommonKeyReader.ApplicationContactInfo["PasswordResetConfirmationEmailText"]);
            model.Add("PasswordResetConfirmationEmailHref", ConfigCommonKeyReader.ApplicationContactInfo["PasswordResetConfirmationEmailHref"]);

            return model;
        }
        
    }
}
