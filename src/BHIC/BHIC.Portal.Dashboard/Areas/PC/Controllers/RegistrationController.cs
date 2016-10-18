#region Using directives
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BHIC.Common;
using BHIC.Common.Client;
using BHIC.Contract.Background;
using BHIC.Core.Background;
using BHIC.Portal.Dashboard.App_Start;
using BHIC.Domain.Dashboard;
using BHIC.Portal.Dashboard;
using BHIC.Common.XmlHelper;
using System.Reflection;
using BHIC.Core.PolicyCentre;

#endregion

namespace BBHIC.Portal.Dashboard.Areas.PC.Controllers
{
    [AllowAnonymous]
    [CustomAntiForgeryToken]
    public class RegistrationController : BaseController
    {

        #region Variables

        private readonly IAccountRegistrationService _accountRegistrationService;

        #endregion

        #region Methods


        public RegistrationController()
        {
            _accountRegistrationService = new AccountRegistrationService(guardServiceProvider);
        }


        // GET: /Registration/Index/
        public ActionResult Registration()
        {
            return PartialView("Registration");
        }

        //Post: /Registration/CreateAccount/
        [HttpPost]
        public ActionResult CreateAccount(AccountRegistration postData)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var retValue = _accountRegistrationService.CreateAccount(postData);
                    if (retValue == "OK")
                    {
                        string URL = GetSchemeAndHostURLPart() + ":" + this.HttpContext.Request.Url.Port + "/" + ConfigCommonKeyReader.BaseUrlPath + "#/Login";
                        var encryptedUrl = Encryption.EncryptText(postData.Email);
                        string requiredUrl = URL + "/" + encryptedUrl;
                        
                        //Fill template and send email
                        if (MailTemplateBuilder.SendMail(MailTemplateBuilder.MailRegistration(GetEmailModel(requiredUrl, postData))))
                        {
                            return Json(new { success = true, successMessage = "Registration & Confirmation Email sent Successfully" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { success = false, errorMessage = "Confirmation Email sent is not successfully." }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { success = false, errorMessage = retValue }, JsonRequestBehavior.AllowGet);
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
                return Json(new { success = false, errorMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        private Dictionary<string, string> GetEmailModel(string requiredURL, AccountRegistration postData)
        {
            Dictionary<string, string> model = new Dictionary<string, string>();
            ServiceProvider guardServiceProvider = new GuardServiceProvider() { ServiceCategory = ServiceProviderConstants.GuardServiceCategoryWC };
            ISystemVariableService systemVariable = new SystemVariableService(guardServiceProvider);

            model.Add("ClientEmailID", postData.Email);
            model.Add("MailSubject", "useremail-registration-subject");
            model.Add("MailBody", "useremail-registration-body");
            model.Add("BaseUrl", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_WebsiteShortURL"]));
            model.Add("AbsoulteURL", GetEmailImageUrl());
            model.Add("IPAddress", GetUser_IP(this.ControllerContext.HttpContext));
            model.Add("ConfirmMailLink", requiredURL);
            model.Add("Name", postData.FirstName + ' ' + postData.LastName);
            model.Add("SupportPhoneNumber", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["PhoneNumber_CSR"]));
            model.Add("SupportPhoneNumberHref", string.Concat("tel:", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["PhoneNumber_CSR"])));
            model.Add("WebsiteUrlText", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_Domain"]));
            model.Add("SupportEmailTextRegistration", ConfigCommonKeyReader.ApplicationContactInfo["SupportEmailTextRegistration"]);
            model.Add("SupportEmailHrefRegistration", ConfigCommonKeyReader.ApplicationContactInfo["SupportEmailHrefRegistration"]);

            return model;
        }

        #endregion
    }
}
