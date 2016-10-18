#region Using directives

using BHIC.Common;
using BHIC.Common.Client;
using BHIC.Common.Configuration;
using BHIC.Common.Mailing;
using BHIC.Common.XmlHelper;
using BHIC.Contract.Background;
using BHIC.Contract.Mailing;
using BHIC.Contract.Template;
using BHIC.Core.Background;
using BHIC.Core.PurchasePath;
using BHIC.Core.Template;
using BHIC.ViewDomain.Landing;
using BHIC.ViewDomain.Mailing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

#endregion

namespace BHIC.Core.Mailing
{
    public class MailingService : IMailingService
    {
        #region Methods

        #region Interface Implementation

        /// <summary>
        /// Send confirmation mail to user, after successful registration
        /// </summary>
        /// <returns>return true on success,false otherwise</returns>
        bool IMailingService.UserRegistrationMail(string userEmail, PolicyRegistrationMailViewModel registrationMail)
        {
            Dictionary<string, object> mailContentModel = new Dictionary<string, object>();
            mailContentModel.Add("policyRegistrationMailBody", ThemeManager.ThemeSharedContent("user-account-registration-mail-body") ?? null);
            mailContentModel.Add("policyRegistrationMailSubject", ThemeManager.ThemeSharedContent("user-account-registration-mail-subject") ?? null);

            //get anchor text 
            var anchorText = ThemeManager.ThemeSharedContent("user-account-registration-anchor") ?? string.Empty;

            var model = new PolicyRegistrationMailViewModel
            {
                //Basic communication/contact details
                WebsiteUrlText = registrationMail.WebsiteUrlText,
                WebsiteUrlHref = registrationMail.WebsiteUrlHref,
                SupportPhoneNumber = registrationMail.SupportPhoneNumber,
                SupportPhoneNumberHref = registrationMail.SupportPhoneNumberHref,
                CompanyName = registrationMail.CompanyName,
                RegisterEmailText = registrationMail.RegisterEmailText,
                RegisterEmailHref = registrationMail.RegisterEmailHref,
                LinkText = anchorText,
                PolicyCenterWebsiteUrlHref = registrationMail.TargetUrl,
                UserName = registrationMail.UserName,
                AbsoulteURL = registrationMail.AbsoulteURL
            };

            MailTemplateBuilder buildTemplate = new MailTemplateBuilder();
            var mailMsg = buildTemplate.MailRegisterDetail(model, mailContentModel);

            mailMsg.RecipEmailAddr = new List<string>() { userEmail };

            mailMsg.Cc = (ConfigCommonKeyReader.RegistrationEmailCc);
            mailMsg.Bcc = (ConfigCommonKeyReader.RegistrationEmailBcc);
            mailMsg.SenderEmailAddr = ConfigCommonKeyReader.RegistrationEmailFrom;

            return mailMsg != null ? buildTemplate.SendMail(mailMsg) : false;
        }

        /// <summary>
        /// Send welcome mail to user
        /// </summary>
        /// <returns></returns>
        bool IMailingService.WelcomeMail(string userEmail, PolicyWelcomeMailViewModel welcomeMail)
        {
            Dictionary<string, object> mailContentModel = new Dictionary<string, object>();
            mailContentModel.Add("policyWelcomeMailBody", ThemeManager.ThemeSharedContent("policy-welcome-mail-body") ?? null);
            mailContentModel.Add("policyWelcomeMailSubject", ThemeManager.ThemeSharedContent("policy-welcome-mail-subject") ?? null);

            var model = new PolicyWelcomeMailViewModel
            {
                //Basic communication/contact details
                CompanyName = welcomeMail.CompanyName,
                WebsiteUrlText = welcomeMail.WebsiteUrlText,
                WebsiteUrlHref = welcomeMail.WebsiteUrlHref,
                SupportEmailText = welcomeMail.SupportEmailText,
                SupportEmailHref = welcomeMail.SupportEmailHref,
                SupportPhoneNumber = welcomeMail.SupportPhoneNumber,
                SupportPhoneNumberHref = welcomeMail.SupportPhoneNumberHref,
                PolicyCenterWebsiteUrlHref = welcomeMail.TargetUrl,
                AbsoulteURL = welcomeMail.AbsoulteURL,

                //Basic policy related details
                InsuredBusinessName = welcomeMail.InsuredBusinessName,
                PolicyNumber = welcomeMail.PolicyNumber,
                PolicyEffectiveDate = welcomeMail.PolicyEffectiveDate,
                PolicyEffectiveDateString = welcomeMail.PolicyEffectiveDateString,

                //Policy billing related details
                TotalPremiumAmount = welcomeMail.TotalPremiumAmount,
                PremiumAmountPaid = welcomeMail.PremiumAmountPaid,
                PolicyInstalments = welcomeMail.PolicyInstalments,
                PolicyNextInstallmentAmount = welcomeMail.PolicyNextInstallmentAmount,
                PolicyNextDueDate = welcomeMail.PolicyNextDueDate,
                PolicyNextDueDateString = welcomeMail.PolicyNextDueDateString,
                NextInstallmentAmount = welcomeMail.NextInstallmentAmount,
                Physical_Address2 = welcomeMail.Physical_Address2,
                Physical_AddressCSZ = welcomeMail.Physical_AddressCSZ,
                StyleSingleInstallment = (welcomeMail.PolicyInstalments > 0) ? "" : "none"
            };

            MailTemplateBuilder buildTemplate = new MailTemplateBuilder();
            var mailMsg = buildTemplate.MailUserNewPolicyDetail(model, mailContentModel);

            mailMsg.RecipEmailAddr = new List<string>() { userEmail };

            mailMsg.Cc = (ConfigCommonKeyReader.WelcomeEmailCc);
            mailMsg.Bcc = (ConfigCommonKeyReader.WelcomeEmailBcc);
            mailMsg.SenderEmailAddr = ConfigCommonKeyReader.WelcomeEmailFrom;

            return mailMsg != null ? buildTemplate.SendMail(mailMsg) : false;
        }

        /// <summary>
        /// Send reset password link to user
        /// </summary>
        /// <param name="forgotPasswordVM"></param>
        /// <returns>return true on success, false otherwise</returns>
        bool IMailingService.ForgotPassword(ForgotPasswordViewModel forgotPasswordVM)
        {
            Dictionary<string, object> mailContentModel = new Dictionary<string, object>();
            mailContentModel.Add("forgotPasswordMailBody", ThemeManager.ThemeSharedContent("useremail-passwordreset-body") ?? null);
            mailContentModel.Add("forgotPasswordMailSubject", ThemeManager.ThemeSharedContent("useremail-passwordreset-subject") ?? null);

            MailTemplateBuilder buildTemplate = new MailTemplateBuilder();
            var mailMsg = buildTemplate.MailUserResetPassword(forgotPasswordVM, mailContentModel);

            mailMsg.RecipEmailAddr = new List<string>() { forgotPasswordVM.RecipEmailAddr };
            mailMsg.Cc = (ConfigCommonKeyReader.ForgotPasswordEmailCc);
            mailMsg.Bcc = (ConfigCommonKeyReader.ForgotPasswordEmailBcc);
            mailMsg.SenderEmailAddr = ConfigCommonKeyReader.ForgotPasswordEmailFrom;

            return mailMsg != null ? buildTemplate.SendMail(mailMsg) : false;
        }

        /// <summary>
        /// send failed payment details in case of failure
        /// </summary>
        /// <param name="recipEmail"></param>
        /// <param name="pfMailVm"></param>
        /// <returns></returns>
        bool IMailingService.PaymentFailure(PaymentFailureViewModel pfMailVm)
        {
            Dictionary<string, object> mailContentModel = new Dictionary<string, object>();
            mailContentModel.Add("paymentFailureMailBody", ThemeManager.ThemeSharedContent("payment-failure-mail-body") ?? null);
            mailContentModel.Add("paymentFailureMailSubject", ThemeManager.ThemeSharedContent("payment-failure-mail-subject") ?? null);

            MailTemplateBuilder buildTemplate = new MailTemplateBuilder();
            var mailMsg = buildTemplate.MailPaymentFailureDetails(pfMailVm, mailContentModel);

            mailMsg.RecipEmailAddr = (ConfigCommonKeyReader.PolicyCreationFailureIntimationTo);

            mailMsg.Cc = (ConfigCommonKeyReader.PolicyCreationFailureIntimationCc);
            mailMsg.Bcc = (ConfigCommonKeyReader.PolicyCreationFailureIntimationBcc);
            mailMsg.SenderEmailAddr = ConfigCommonKeyReader.PolicyCreationFailureIntimationFrom;

            return mailMsg != null ? buildTemplate.SendMail(mailMsg) : false;
        }

        /// <summary>
        /// Send welcome mail to user on purchase of BOP POlicy. This will be triggered from BOP API
        /// </summary>
        /// <returns></returns>
        bool IMailingService.WelcomeMailBOP(string userEmail, PolicyWelcomeMailViewModel welcomeMail)
        {
            Dictionary<string, object> mailContentModel = new Dictionary<string, object>();
            mailContentModel.Add("policyWelcomeMailBody", ThemeManager.ThemeSharedContent("bop-policy-welcome-mail-body") ?? null);
            mailContentModel.Add("policyWelcomeMailSubject", ThemeManager.ThemeSharedContent("bop-policy-welcome-mail-subject") ?? null);

            var model = new PolicyWelcomeMailViewModel
            {
                //Basic communication/contact details
                CompanyName = welcomeMail.CompanyName,
                WebsiteUrlText = welcomeMail.WebsiteUrlText,
                WebsiteUrlHref = welcomeMail.WebsiteUrlHref,
                SupportEmailText = welcomeMail.SupportEmailText,
                SupportEmailHref = welcomeMail.SupportEmailHref,
                SupportPhoneNumber = welcomeMail.SupportPhoneNumber,
                SupportPhoneNumberHref = welcomeMail.SupportPhoneNumberHref,
                PolicyCenterWebsiteUrlHref = welcomeMail.TargetUrl,
                AbsoulteURL = welcomeMail.AbsoulteURL,
                Physical_Address2 = welcomeMail.Physical_Address2,
                Physical_AddressCSZ = welcomeMail.Physical_AddressCSZ,

                //Basic policy related details
                InsuredBusinessName = welcomeMail.InsuredBusinessName,
                PolicyNumber = welcomeMail.PolicyNumber,
                PolicyEffectiveDate = welcomeMail.PolicyEffectiveDate,
                PolicyEffectiveDateString = welcomeMail.PolicyEffectiveDateString,

                //Policy billing related details
                TotalPremiumAmount = welcomeMail.TotalPremiumAmount,
                PremiumAmountPaid = welcomeMail.PremiumAmountPaid,
                PolicyInstalments = welcomeMail.PolicyInstalments,
                PolicyNextInstallmentAmount = welcomeMail.PolicyNextInstallmentAmount,
                PolicyNextDueDate = welcomeMail.PolicyNextDueDate,
                PolicyNextDueDateString = welcomeMail.PolicyNextDueDateString,
                NextInstallmentAmount = welcomeMail.NextInstallmentAmount,
                StyleSingleInstallment = (welcomeMail.PolicyInstalments > 0) ? "" : "none"
            };

            MailTemplateBuilder buildTemplate = new MailTemplateBuilder();
            var mailMsg = buildTemplate.MailUserNewPolicyDetail(model, mailContentModel);

            mailMsg.RecipEmailAddr = new List<string>() { userEmail };
            mailMsg.Cc = (ConfigCommonKeyReader.WelcomeEmailCc);
            mailMsg.Bcc = (ConfigCommonKeyReader.WelcomeEmailBcc);
            mailMsg.SenderEmailAddr = ConfigCommonKeyReader.WelcomeEmailFrom;
            // Commented by Guru, as not required CC for direct client email.
            //mailMsg.Cc = (ConfigCommonKeyReader.ClientEmailID);

            return mailMsg != null ? buildTemplate.SendMail(mailMsg) : false;
        }

        /// <summary>
        /// Send mail(having details of scheduled call) to internal team
        /// </summary>
        /// <param name="scheduleCallViewModel">Contains mail details</param>
        /// <returns>return true on success, false otherwise</returns>
        bool IMailingService.ScheduleCall(ScheduleCallViewModel model)
        {
            Dictionary<string, object> mailContentModel = new Dictionary<string, object>();

            ServiceProvider guardServiceProvider = new GuardServiceProvider() { ServiceCategory = ServiceProviderConstants.GuardServiceCategoryWC };
            ISystemVariableService systemVariable = new SystemVariableService(guardServiceProvider);

            mailContentModel.Add("scheduleCallBody", ThemeManager.ThemeSharedContent("schedule-call-body") ?? null);
            mailContentModel.Add("scheduleCallSubject", ThemeManager.ThemeSharedContent("schedule-call-subject") ?? null);

            ScheduleCallMailViewModel scheduleCallViewModel = new ScheduleCallMailViewModel();

            scheduleCallViewModel.WebsiteUrlText = systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_Domain"]);
            scheduleCallViewModel.WebsiteUrlHref = systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_WebsiteShortURL"]);
            scheduleCallViewModel.SupportEmailText = ConfigCommonKeyReader.ApplicationContactInfo["SupportEmailTextSalesSupport"];
            scheduleCallViewModel.SupportEmailHref = ConfigCommonKeyReader.ApplicationContactInfo["SupportEmailHrefSalesSupport"];
            scheduleCallViewModel.SupportPhoneNumber = systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["PhoneNumber_CSR"]);
            scheduleCallViewModel.SupportPhoneNumberHref = string.Concat("tel:", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["PhoneNumber_CSR"]));
            scheduleCallViewModel.RequestTimeHeader = "Request Date/Time";
            scheduleCallViewModel.RequestTime = Convert.ToString(model.RequestTime);
            scheduleCallViewModel.IpAddressHeader = "IP Address";
            //scheduleCallViewModel.IpAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] == null ? HttpContext.Current.Request.UserHostAddress : HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            scheduleCallViewModel.IpAddress = UtilityFunctions.GetUserIPAddress(HttpContext.Current);
            scheduleCallViewModel.RequesterNameHeader = "Requester Name";
            scheduleCallViewModel.RequesterName = model.RequesterName;
            scheduleCallViewModel.PhoneNumberHeader = "Phone Number";
            scheduleCallViewModel.PhoneNumber = model.PhoneNumber;
            scheduleCallViewModel.QuoteIdHeader = "Quote ID (if any)";
            scheduleCallViewModel.QuoteId = (model.QuoteId > 0) ? Convert.ToString(model.QuoteId) : string.Empty;
            scheduleCallViewModel.RequestedCallTimeHeader = "Requested time for call";
            scheduleCallViewModel.RequestedCallTime = Convert.ToString(model.RequestedCallTime);
            scheduleCallViewModel.AbsoulteURL = model.AbsoulteURL;

            MailTemplateBuilder buildTemplate = new MailTemplateBuilder();
            var mailMsg = buildTemplate.MailScheduleCall(scheduleCallViewModel, mailContentModel);

            mailMsg.RecipEmailAddr = (ConfigCommonKeyReader.ScheduleCallEmailTo);
            mailMsg.Cc = (ConfigCommonKeyReader.ScheduleCallEmailCc);
            mailMsg.Bcc = (ConfigCommonKeyReader.ScheduleCallEmailBcc);
            mailMsg.SenderEmailAddr = ConfigCommonKeyReader.ScheduleCallEmailFrom;

            return mailMsg != null ? buildTemplate.SendMail(mailMsg) : false;
        }

        /// <summary>
        /// Send mail for save for Later
        /// </summary>
        /// <param name="scheduleCallViewModel">Contains mail details</param>
        /// <returns>return true on success, false otherwise</returns>
        bool IMailingService.SaveForLater(string userEmail, SaveForLaterMailViewModel model)
        {
            Dictionary<string, object> mailContentModel = new Dictionary<string, object>();
            ITemplateLocator templateLocator = new TemplateLocator(ConfigCommonKeyReader.EmailTemplatePath);
            ITemplateContentReader templateContentReader = new TemplateContentReader();

            string reviewQuoteText = templateContentReader.GetContent(templateLocator.Locate("useremail-saveforlater-anchortext"));
            mailContentModel.Add("saveforlaterMailBody", templateContentReader.GetContent(templateLocator.Locate("useremail-saveforlater-body")) ?? null);
            mailContentModel.Add("saveforlaterMailSubject", templateContentReader.GetContent(templateLocator.Locate("useremail-saveforlater-subject")) ?? null);
            model.ReviewQuoteText = string.Format("{0} # {1}", reviewQuoteText, model.QuoteId);

            MailTemplateBuilder buildTemplate = new MailTemplateBuilder();
            var mailMsg = buildTemplate.MailSaveForLater(model, mailContentModel);

            mailMsg.RecipEmailAddr = new List<string>() { userEmail };
            mailMsg.Cc = (ConfigCommonKeyReader.RetreiveQuoteEmailCc);
            mailMsg.Bcc = (ConfigCommonKeyReader.RetreiveQuoteEmailBcc);
            mailMsg.SenderEmailAddr = ConfigCommonKeyReader.RetreiveQuoteEmailFrom;

            return mailMsg != null ? buildTemplate.SendMail(mailMsg) : false;
        }

        #endregion

        #region Private Method

        /// <summary>
        /// Send mail with details to user
        /// </summary>
        /// <param name="mail"></param>
        private static bool SendMail(MailMsg mail)
        {
            try
            {
                MailHelper mailMessage = new MailHelper();
                //mailMessage.SendMailMessage(null, mail.RecipEmailAddr.Split(';').ToList(), mail.Subject, mail.MessageBody);
                //mailMessage.SendMailMessage(null, (mail.RecipEmailAddr), mail.Subject, mail.MessageBody);
                mailMessage.SendMailMessage(null, mail.RecipEmailAddr, mail.Subject, mail.MessageBody);

            }
            catch
            {
                throw;
            }

            return true;
        }


        #endregion

        #endregion
    }
}
