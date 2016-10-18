#region Using directives

using BHIC.Common;
using BHIC.Common.Client;
using BHIC.Common.Configuration;
using BHIC.Common.Logging;
using BHIC.Common.Quote;
using BHIC.Common.XmlHelper;
using BHIC.Contract.Background;
using BHIC.Core.Background;
using BHIC.Core.PolicyCentre;
using BHIC.Domain.Dashboard;
using BHIC.Portal.Dashboard.App_Start;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

#endregion

namespace BHIC.Portal.Dashboard.Areas.PC.Controllers
{
    public class LoginController : BaseController
    {

        ServiceProvider _provider = new GuardServiceProvider() { ProviderName = ProviderNames.Guard, ServiceCategory = LineOfBusiness };
        private static readonly ILoggingService Logger = LoggingService.Instance;

        // GET: DashBoard/Login
        [AllowAnonymous]
        public ActionResult LoginIndex()
        {
            // Added by Guru for API Log, please don't remove it
            Session["APILog"] = null;
            if (User.Identity.IsAuthenticated)
            { return RedirectToAction("Index", "Dashboard"); }
            else
            { return View("LoginIndex"); }
        }

        [AllowAnonymous]
        [CustomAntiForgeryToken]
        public ActionResult Login()
        {
            // Added by Guru for API Log, please don't remove it
            Session["APILog"] = null;
            return PartialView("Login");
        }

        [AllowAnonymous]
        [CustomAntiForgeryToken]
        public ActionResult GetEmailVerified(string key)
        {
            try
            {
                var decryptedEmail = Encryption.DecryptText(key);
                AccountRegistrationService accountRegistrationService = new AccountRegistrationService(guardServiceProvider);
                bool verified = accountRegistrationService.EmailVerification(decryptedEmail);
                if (verified)
                {
                    ViewBag.Name = "verified";
                }
                return PartialView("Login");
            }
            catch (Exception ex)
            {
                Logger.Fatal(string.Format("Email Verification with error message : {0}", ex.ToString()));
                return Json(new { success = false });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [CustomAntiForgeryToken]
        public ActionResult UserLogin(UserRegistration postData)
        {
            try
            {
                postData.SecurePassword = UtilityFunctions.ConvertToSecureString(postData.Password);
                AccountRegistrationService accountRegistrationService = new AccountRegistrationService(guardServiceProvider);
                postData = accountRegistrationService.GetCredentials(postData);
                if (postData.Id > 0)
                {
                    if (postData.isEmailVerified)
                    {
                        FormsAuthentication.SetAuthCookie(postData.Email, false);

                        //Comment : Here set cookie
                        SetPolicyCenterUserDetailCookie(this.ControllerContext.HttpContext, postData);
                        Session["user"] = postData;
                        return Json(new { success = true, isEmailverified = true, user = postData }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = true, isEmailverified = false }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { success = false, errorMessage = postData.ResponseMessage, isEmailverified = false }, JsonRequestBehavior.AllowGet);

            }

            catch (Exception ex)
            {
                loggingService.Fatal(string.Format("Method {0} executed with error message : {1}", MethodBase.GetCurrentMethod().Name, ex.ToString()));
            }
            return Json(new { success = false, isEmailverified = true });

        }

        [AllowAnonymous]
        public ActionResult GlobalSignOut()
        {
            SignOut();
            return LoginIndex();
        }

        [AllowAnonymous]
        public ActionResult IsPCSessionAlive()
        {
            if (System.Web.HttpContext.Current.Session["user"] != null)
            {
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (QuoteCookieHelper.Cookie_GetQuoteId(this.ControllerContext.HttpContext) > 0)
                {
                    if (!string.IsNullOrEmpty(QuoteCookieHelper.Cookie_GetPcUserId(this.ControllerContext.HttpContext)))
                    {
                        Session["user"] = JsonConvert.DeserializeObject<UserRegistration>(QuoteCookieHelper.Cookie_GetPcUserId(this.ControllerContext.HttpContext));
                        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                    }
                }
                DeletePolicyCenterUserDetailCookie(this.ControllerContext.HttpContext);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        [CustomAntiForgeryToken]
        public ActionResult SignOut()
        {
            try
            {
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Session.RemoveAll();
                FormsAuthentication.SignOut();
                //Comment : Here delet cookie
                DeletePolicyCenterUserDetailCookie(this.ControllerContext.HttpContext);
            }
            catch (Exception ex)
            {
                Logger.Fatal(string.Format("Login/SignOut with error message : {0}", ex.ToString()));
                return Json(new { success = false });
            }

            return Json(new { success = true });
        }

        [HttpPost]
        [AllowAnonymous]
        [CustomAntiForgeryToken]
        public ActionResult ResendMail(UserRegistration postData)
        {
            try
            {
                string URL = GetSchemeAndHostURLPart() + ":" + this.HttpContext.Request.Url.Port + "/" + ConfigCommonKeyReader.BaseUrlPath + "#/Login";
                var encryptedUrl = Encryption.EncryptText(postData.Email);
                string requiredUrl = URL + "/" + encryptedUrl;
                string absoultePath = CDN.Path + "/Content/" + ConfigCommonKeyReader.CdnDefaultDashboardFolder + "/themes/_sharedFiles/emailImages/";
                string baseUrl = string.Concat(GetSchemeAndHostURLPart(), ConfigCommonKeyReader.WCUrl);
                ServiceProvider guardServiceProvider = new GuardServiceProvider() { ServiceCategory = ServiceProviderConstants.GuardServiceCategoryWC };
                ISystemVariableService systemVariable = new SystemVariableService(guardServiceProvider);

                Dictionary<string, string> model = new Dictionary<string, string>();
                model.Add("ClientEmailID", String.Join(";", postData.Email));
                model.Add("MailSubject", "useremail-registration-subject");
                model.Add("MailBody", "useremail-registration-body");
                model.Add("BaseUrl", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_WebsiteShortURL"]));
                model.Add("AbsoulteURL", absoultePath);
                model.Add("IPAddress", Dns.GetHostAddresses(Dns.GetHostName())[1].ToString());
                model.Add("Name", postData.FirstName + ' ' + postData.LastName);
                model.Add("ConfirmMailLink", requiredUrl);
                model.Add("SupportPhoneNumber", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["PhoneNumber_CSR"]));
                model.Add("SupportPhoneNumberHref", string.Concat("tel:", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["PhoneNumber_CSR"])));
                model.Add("WebsiteUrlText", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_Domain"]));
                model.Add("SupportEmailTextRegistration", ConfigCommonKeyReader.ApplicationContactInfo["SupportEmailTextRegistration"]);
                model.Add("SupportEmailHrefRegistration", ConfigCommonKeyReader.ApplicationContactInfo["SupportEmailHrefRegistration"]);

                if (MailTemplateBuilder.SendMail(MailTemplateBuilder.MailRegistration(model)))
                {
                    return Json(new { success = true, successMessage = "Registration & Confirmation Email sent Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, errorMessage = "Error occurred while sending mail." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Logger.Fatal(string.Format("Login/SignOut with error message : {0}", ex.ToString()));
                return Json(new { success = false });
            }
        }


        public JsonResult Validate(string code)
        {
            return Json(new { status = UtilityFunctions.ValidateCaptcha(code) }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult GlobalSignOutPartial()
        {
            return PartialView("_GlobalSignOutPartial");
        }
    }
}
