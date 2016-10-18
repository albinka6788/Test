using System;
using System.Web.Mvc;
using BHIC.Common;
using BHIC.Common.XmlHelper;
using BHIC.Core.Background;
using BHIC.Domain.Dashboard;
using BHIC.Portal.Dashboard.App_Start;
using BHIC.Common.Config;
using System.Reflection;
using System.Collections.Generic;
using BHIC.Core.PolicyCentre;
using BHIC.Common.Client;
using BHIC.Contract.Background;

namespace BHIC.Portal.Dashboard.Areas.PC.Controllers
{
    [AllowAnonymous]
    [CustomAntiForgeryToken]
    public class ForgotPasswordController : BaseController
    {
        //
        // GET: /PC/ForgotPassword/
        #region Public methods
        public ActionResult ForgotPassword()
        {
            return PartialView("ForgotPassword");
        }

        [HttpPost]
        public JsonResult ForgotPasswordRequest(ResetPasswordDTO postData)
        {
            try
            {
                AccountRegistrationService accountRegistrationService = new AccountRegistrationService(guardServiceProvider);
                bool valid = accountRegistrationService.ValidUser(postData.EmailId);
                UserRegistration user = new UserRegistration();
                if (valid)
                {
                    user.Email = postData.EmailId;
                    user = accountRegistrationService.GetUserDetails(user);
                    string url = GetSchemeAndHostURLPart() + ":" + this.HttpContext.Request.Url.Port + "/" + ConfigCommonKeyReader.BaseUrlPath + "#/ResetPassword/";
                    string currentDateTimeMMddyyyyhhmmss = Encryption.EncryptText(DateTime.Now.ToString("MMddyyyyhhmmss"));
                    var encryptedUrl = Encryption.EncryptText(postData.EmailId + "|:|" + currentDateTimeMMddyyyyhhmmss);
                    string requiredUrl = url + "?queryKey=" + encryptedUrl;
                    string fullname = user.FirstName + ' ' + user.LastName;


                    if (accountRegistrationService.ForgotPwdRequestedDateTime("F", postData.EmailId, currentDateTimeMMddyyyyhhmmss))
                    {
                        if (MailTemplateBuilder.SendMail(MailTemplateBuilder.MailForgotPassword(GetEmailModel(postData.EmailId, fullname, requiredUrl))))
                        {
                            return Json(new { success = true, successMessage = "Forgot Password mail sent" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { success = false, errorMessage = "Error occurred while sending mail." }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { success = false, errorMessage = "Error occurred." }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { success = false, errorMessage = "Email does not exist. Please Register." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                loggingService.Fatal(string.Format("Method {0} executed with error message : {1}", MethodBase.GetCurrentMethod().Name, ex.ToString()));
                return Json(new { success = false, redirectStatus = true, errorMessage = Constants.ExceptionMessage }, JsonRequestBehavior.AllowGet);
            }
        }

        private Dictionary<string, string> GetEmailModel(string emailId, string fullName, string requiredUrl)
        {
            Dictionary<string, string> model = new Dictionary<string, string>();
            ServiceProvider guardServiceProvider = new GuardServiceProvider() { ServiceCategory = ServiceProviderConstants.GuardServiceCategoryWC };
            ISystemVariableService systemVariable = new SystemVariableService(guardServiceProvider);

            model.Add("ClientEmailID", String.Join(";", emailId));
            model.Add("MailSubject", "useremail-passwordreset-subject");
            model.Add("MailBody", "useremail-passwordreset-body");
            model.Add("BaseUrl", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_WebsiteShortURL"]));
            model.Add("AbsoulteURL", GetEmailImageUrl());
            model.Add("IPAddress", GetUser_IP(this.ControllerContext.HttpContext));
            model.Add("Name", fullName);
            model.Add("ResetPasswordLink", requiredUrl);
            model.Add("SupportPhoneNumber", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["PhoneNumber_CSR"]));
            model.Add("SupportPhoneNumberHref", string.Concat("tel:", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["PhoneNumber_CSR"])));
            model.Add("WebsiteUrlText", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_Domain"]));
            model.Add("PasswordResetEmailText", ConfigCommonKeyReader.ApplicationContactInfo["PasswordResetEmailText"]);
            model.Add("PasswordResetEmailHref", ConfigCommonKeyReader.ApplicationContactInfo["PasswordResetEmailHref"]);

            return model;
        }
        #endregion


    }

}
