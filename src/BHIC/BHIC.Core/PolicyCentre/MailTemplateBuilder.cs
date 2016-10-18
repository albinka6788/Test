using BHIC.Common;
using BHIC.Common.Configuration;
using BHIC.Common.Mailing;
using BHIC.Common.XmlHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BHIC.Core.PolicyCentre
{
    public class MailTemplateBuilder
    {
        #region Mail Templates

        #region Request a Policy Change

        public static MailMsg MailRequestPolicyChange(Dictionary<string, string> model)
        {
            MailMsg mailMsg = new MailMsg();
            mailMsg.RecipEmailAddr = (ConfigCommonKeyReader.PolicyChangeEmailTo);
            mailMsg.Subject = ThemeManager.ThemeSharedContent(model["MailSubject"]);
            mailMsg.MessageBody = ThemeManager.ThemeSharedContent(model["MailBody"], new
            {
                BaseUrl = model["BaseUrl"],
                AbsoulteURL = model["AbsoulteURL"],
                DateTime = DateTime.Now,
                IPAddress = model["IPAddress"],
                PolicyCode = model["PolicyCode"],
                RequestEffectiveDate = model["RequestEffectiveDate"],
                ChangePolicyName = model["ChangePolicyName"],
                ChangePolicyContactNumber = model["ChangePolicyContactNumber"],
                ChangePolicyEmail = model["ChangePolicyEmail"],
                ChangeType = model["ChangeType"],
                Description = model["Description"],
                SupportPhoneNumber = model["SupportPhoneNumber"],
                SupportPhoneNumberHref = model["SupportPhoneNumberHref"],
                WebsiteUrlText = model["WebsiteUrlText"],
                SupportEmailTextSalesSupport = model["SupportEmailTextSalesSupport"],
                SupportEmailHrefSalesSupport = model["SupportEmailHrefSalesSupport"]
            });

            mailMsg.Cc = (ConfigCommonKeyReader.PolicyChangeEmailCc);
            mailMsg.Bcc = (ConfigCommonKeyReader.PolicyChangeEmailBcc);
            mailMsg.SenderEmailAddr = ConfigCommonKeyReader.PolicyChangeEmailFrom;

            return mailMsg;
        }

        #endregion

        #region Report a Claim

        public static MailMsg MailReportClaim(Dictionary<string, string> model)
        {
            MailMsg mailMsg = new MailMsg();
            mailMsg.RecipEmailAddr = (ConfigCommonKeyReader.ClaimRequestEmailTo);
            mailMsg.Subject = ThemeManager.ThemeSharedContent(model["MailSubject"]);
            mailMsg.MessageBody = ThemeManager.ThemeSharedContent(model["MailBody"], new
            {
                BaseUrl = model["BaseUrl"],
                AbsoulteURL = model["AbsoulteURL"],
                DateTime = DateTime.Now,
                IPAddress = model["IPAddress"],
                PolicyCode = model["PolicyCode"],
                ClaimType = model["ClaimType"],
                DateOfIllness = model["DateOfIllness"],
                PolicyClaimEmail = model["PolicyClaimEmail"],
                BusinessName = model["BusinessName"],
                YourName = model["YourName"],
                BestContactNumber = model["BestContactNumber"],
                InjuredWorkerName = model["InjuredWorkerName"],
                Location = model["Location"],
                Description = model["Description"],
                SupportPhoneNumber = model["SupportPhoneNumber"],
                SupportPhoneNumberHref = model["SupportPhoneNumberHref"],
                WebsiteUrlText = model["WebsiteUrlText"],
                ClaimsProcessingEmailText = model["ClaimsProcessingEmailText"],
                ClaimsProcessingEmail = model["ClaimsProcessingEmail"]
            });

            mailMsg.Cc = (ConfigCommonKeyReader.ClaimRequestEmailCc);
            mailMsg.Bcc = (ConfigCommonKeyReader.ClaimRequestEmailBcc);
            mailMsg.SenderEmailAddr = ConfigCommonKeyReader.ClaimRequestEmailFrom;

            return mailMsg;
        }

        public static MailMsg MailBOPReportClaim(Dictionary<string, string> model)
        {
            MailMsg mailMsg = new MailMsg();
            mailMsg.RecipEmailAddr = (ConfigCommonKeyReader.ClaimRequestEmailTo);
            mailMsg.Subject = ThemeManager.ThemeSharedContent(model["MailSubject"]);
            mailMsg.MessageBody = ThemeManager.ThemeSharedContent(model["MailBody"], new
            {
                BaseUrl = model["BaseUrl"],
                AbsoulteURL = model["AbsoulteURL"],
                DateTime = DateTime.Now,
                IPAddress = model["IPAddress"],
                PolicyCode = model["PolicyCode"],
                ClaimType = model["ClaimType"],
                DateOfLoss = model["DateOfLoss"],
                BusinessName = model["BusinessName"],
                BestContactNumber = model["BestContactNumber"],
                YourName = model["YourName"],
                InjuredWorkerName = model["InjuredWorkerName"],
                Location = model["Location"],
                Description = model["Description"],
                PolicyClaimEmail = model["PolicyClaimEmail"],
                SupportPhoneNumber = model["SupportPhoneNumber"],
                SupportPhoneNumberHref = model["SupportPhoneNumberHref"],
                WebsiteUrlText = model["WebsiteUrlText"],
                ClaimsProcessingEmailText = model["ClaimsProcessingEmailText"],
                ClaimsProcessingEmail = model["ClaimsProcessingEmail"]
            });

            mailMsg.Cc = (ConfigCommonKeyReader.ClaimRequestEmailCc);
            mailMsg.Bcc = (ConfigCommonKeyReader.ClaimRequestEmailBcc);
            mailMsg.SenderEmailAddr = ConfigCommonKeyReader.ClaimRequestEmailFrom;
            return mailMsg;
        }

        #endregion

        #region Cancel Policy

        public static MailMsg MailCancelPolicy(Dictionary<string, string> model)
        {
            MailMsg mailMsg = new MailMsg();
            mailMsg.RecipEmailAddr = (ConfigCommonKeyReader.PolicyCancellationEmailTo);
            mailMsg.Subject = ThemeManager.ThemeSharedContent(model["MailSubject"]);
            mailMsg.MessageBody = ThemeManager.ThemeSharedContent(model["MailBody"], new
            {
                BaseUrl = model["BaseUrl"],
                AbsoulteURL = model["AbsoulteURL"],
                DateTime = DateTime.Now,
                IPAddress = model["IPAddress"],
                PolicyCode = model["PolicyCode"],
                CancelPolicyEffectiveDate = model["CancelPolicyEffectiveDate"],
                CancelPolicyName = model["CancelPolicyName"],
                CancelPolicyContactNumber = model["CancelPolicyContactNumber"],
                CancelPolicyEmail = model["CancelPolicyEmail"],
                CancelPolicyReason = model["CancelPolicyReason"],
                CancelDescription = model["CancelDescription"],
                SupportPhoneNumber = model["SupportPhoneNumber"],
                SupportPhoneNumberHref = model["SupportPhoneNumberHref"],
                WebsiteUrlText = model["WebsiteUrlText"],
                SupportEmailTextSalesSupport = model["SupportEmailTextSalesSupport"],
                SupportEmailHrefSalesSupport = model["SupportEmailHrefSalesSupport"]
            });

            mailMsg.Cc = (ConfigCommonKeyReader.PolicyCancellationEmailCc);
            mailMsg.Bcc = (ConfigCommonKeyReader.PolicyCancellationEmailBcc);
            mailMsg.SenderEmailAddr = ConfigCommonKeyReader.PolicyCancellationEmailFrom;

            return mailMsg;
        }

        #endregion

        #region Change Password

        public static MailMsg MailChangePassword(Dictionary<string, string> model)
        {
            MailMsg mailMsg = new MailMsg();
            mailMsg.RecipEmailAddr = UtilityFunctions.GetListOfSplitedValues(model["ClientEmailID"]);
            mailMsg.Subject = ThemeManager.ThemeSharedContent(model["MailSubject"]);
            mailMsg.MessageBody = ThemeManager.ThemeSharedContent(model["MailBody"], new
            {
                BaseUrl = model["BaseUrl"],
                AbsoulteURL = model["AbsoulteURL"],
                DateTime = DateTime.Now,
                IPAddress = model["IPAddress"],
                Name = model["Name"],
                LoginUrl = model["LoginUrl"],
                SupportPhoneNumber = model["SupportPhoneNumber"],
                SupportPhoneNumberHref = model["SupportPhoneNumberHref"],
                WebsiteUrlText = model["WebsiteUrlText"],
                PasswordResetConfirmationEmailText = model["PasswordResetConfirmationEmailText"],
                PasswordResetConfirmationEmailHref = model["PasswordResetConfirmationEmailHref"]
            });

            mailMsg.Cc = (ConfigCommonKeyReader.ResetPasswordEmailCc);
            mailMsg.Bcc = (ConfigCommonKeyReader.ResetPasswordEmailBcc);
            mailMsg.SenderEmailAddr = ConfigCommonKeyReader.ResetPasswordEmailFrom;

            return mailMsg;
        }

        #endregion

        #region Edit Contact Info

        public static MailMsg MailEditContactInfo(Dictionary<string, string> model)
        {
            MailMsg mailMsg = new MailMsg();
            mailMsg.RecipEmailAddr = (ConfigCommonKeyReader.ContactInformationEmailTo);
            mailMsg.Subject = ThemeManager.ThemeSharedContent(model["MailSubject"]);
            mailMsg.MessageBody = ThemeManager.ThemeSharedContent(model["MailBody"], new
            {
                BaseUrl = model["BaseUrl"],
                AbsoulteURL = model["AbsoulteURL"],
                DateTime = DateTime.Now,
                IPAddress = model["IPAddress"],
                ContactInfoName = model["ContactInfoName"],
                ContactInfoPhoneNumber = model["ContactInfoPhoneNumber"],
                ContactInfoEmail = model["ContactInfoEmail"],
                PolicyCode = model["PolicyCode"],
                SupportPhoneNumber = model["SupportPhoneNumber"],
                SupportPhoneNumberHref = model["SupportPhoneNumberHref"],
                WebsiteUrlText = model["WebsiteUrlText"],
                SupportEmailTextSalesSupport = model["SupportEmailTextSalesSupport"],
                SupportEmailHrefSalesSupport = model["SupportEmailHrefSalesSupport"]
            });

            mailMsg.Cc = (ConfigCommonKeyReader.ContactInformationEmailCc);
            mailMsg.Bcc = (ConfigCommonKeyReader.ContactInformationEmailBcc);
            mailMsg.SenderEmailAddr = ConfigCommonKeyReader.ContactInformationEmailFrom;

            return mailMsg;
        }

        #endregion

        #region Edit Contact Address Info

        public static MailMsg MailEditContactAddress(Dictionary<string, string> model)
        {
            MailMsg mailMsg = new MailMsg();
            mailMsg.RecipEmailAddr = (ConfigCommonKeyReader.AddressChangeEmailTo);
            mailMsg.Subject = ThemeManager.ThemeSharedContent(model["MailSubject"]);
            mailMsg.MessageBody = ThemeManager.ThemeSharedContent(model["MailBody"], new
            {
                BaseUrl = model["BaseUrl"],
                AbsoulteURL = model["AbsoulteURL"],
                DateTime = DateTime.Now,
                IPAddress = model["IPAddress"],
                ContactInfoPhoneNumber = model["ContactInfoPhoneNumber"],
                ContactInfoEmail = model["ContactInfoEmail"],
                BusinessName = model["BusinessName"],
                Policycodes = model["Policycodes"],
                Address1 = model["Address1"],
                Address2 = model["Address2"],
                City = model["City"],
                State = model["State"],
                ZipCode = model["ZipCode"],
                Additional = model["Additional"],
                SupportPhoneNumber = model["SupportPhoneNumber"],
                SupportPhoneNumberHref = model["SupportPhoneNumberHref"],
                WebsiteUrlText = model["WebsiteUrlText"],
                AddressType = model["AddressType"],
                SupportEmailTextSalesSupport = model["SupportEmailTextSalesSupport"],
                SupportEmailHrefSalesSupport = model["SupportEmailHrefSalesSupport"]
            });

            mailMsg.Cc = (ConfigCommonKeyReader.AddressChangeEmailCc);
            mailMsg.Bcc = (ConfigCommonKeyReader.AddressChangeEmailBcc);
            mailMsg.SenderEmailAddr = ConfigCommonKeyReader.AddressChangeEmailFrom;

            return mailMsg;
        }

        #endregion

        #region Forgot Password

        public static MailMsg MailForgotPassword(Dictionary<string, string> model)
        {
            MailMsg mailMsg = new MailMsg();
            mailMsg.RecipEmailAddr = UtilityFunctions.GetListOfSplitedValues(model["ClientEmailID"]);
            mailMsg.Subject = ThemeManager.ThemeSharedContent(model["MailSubject"]);
            mailMsg.MessageBody = ThemeManager.ThemeSharedContent(model["MailBody"], new
            {
                BaseUrl = model["BaseUrl"],
                AbsoulteURL = model["AbsoulteURL"],
                DateTime = DateTime.Now,
                IPAddress = model["IPAddress"],
                Name = model["Name"],
                ResetPasswordLink = model["ResetPasswordLink"],
                SupportPhoneNumber = model["SupportPhoneNumber"],
                SupportPhoneNumberHref = model["SupportPhoneNumberHref"],
                WebsiteUrlText = model["WebsiteUrlText"],
                PasswordResetEmailText = model["PasswordResetEmailText"],
                PasswordResetEmailHref = model["PasswordResetEmailHref"]
            });

            mailMsg.Cc = (ConfigCommonKeyReader.ForgotPasswordEmailCc);
            mailMsg.Bcc = (ConfigCommonKeyReader.ForgotPasswordEmailBcc);
            mailMsg.SenderEmailAddr = ConfigCommonKeyReader.ForgotPasswordEmailFrom;

            return mailMsg;
        }

        #endregion

        #region Registration

        public static MailMsg MailRegistration(Dictionary<string, string> model)
        {
            MailMsg mailMsg = new MailMsg();
            mailMsg.RecipEmailAddr = UtilityFunctions.GetListOfSplitedValues(model["ClientEmailID"]);
            mailMsg.Subject = ThemeManager.ThemeSharedContent(model["MailSubject"]);
            mailMsg.MessageBody = ThemeManager.ThemeSharedContent(model["MailBody"], new
            {
                BaseUrl = model["BaseUrl"],
                AbsoulteURL = model["AbsoulteURL"],
                DateTime = DateTime.Now,
                IPAddress = model["IPAddress"],
                ConfirmMailLink = model["ConfirmMailLink"],
                Name = model["Name"],
                SupportPhoneNumber = model["SupportPhoneNumber"],
                SupportPhoneNumberHref = model["SupportPhoneNumberHref"],
                WebsiteUrlText = model["WebsiteUrlText"],
                SupportEmailTextRegistration = model["SupportEmailTextRegistration"],
                SupportEmailHrefRegistration = model["SupportEmailHrefRegistration"]
            });

            mailMsg.Cc = (ConfigCommonKeyReader.RegistrationEmailCc);
            mailMsg.Bcc = (ConfigCommonKeyReader.RegistrationEmailBcc);
            mailMsg.SenderEmailAddr = ConfigCommonKeyReader.RegistrationEmailFrom;

            return mailMsg;
        }

        #endregion

        #region Reset Password

        public static MailMsg MailResetPassword(Dictionary<string, string> model)
        {
            MailMsg mailMsg = new MailMsg();
            mailMsg.RecipEmailAddr = UtilityFunctions.GetListOfSplitedValues(model["ClientEmailID"]);
            mailMsg.Subject = ThemeManager.ThemeSharedContent(model["MailSubject"]);
            mailMsg.MessageBody = ThemeManager.ThemeSharedContent(model["MailBody"], new
            {
                BaseUrl = model["BaseUrl"],
                AbsoulteURL = model["AbsoulteURL"],
                DateTime = DateTime.Now,
                IPAddress = model["IPAddress"],
                Name = model["Name"],
                LoginUrl = model["LoginUrl"],
                SupportPhoneNumber = model["SupportPhoneNumber"],
                SupportPhoneNumberHref = model["SupportPhoneNumberHref"],
                WebsiteUrlText = model["WebsiteUrlText"],
                PasswordResetConfirmationEmailText = model["PasswordResetConfirmationEmailText"],
                PasswordResetConfirmationEmailHref = model["PasswordResetConfirmationEmailHref"]
            });


            mailMsg.Cc = (ConfigCommonKeyReader.ResetPasswordEmailCc);
            mailMsg.Bcc = (ConfigCommonKeyReader.ResetPasswordEmailBcc);
            mailMsg.SenderEmailAddr = ConfigCommonKeyReader.ResetPasswordEmailFrom;

            return mailMsg;
        }

        #endregion

        #region  Upload Documents

        public static MailMsg MailUploadDocuments(Dictionary<string, string> model)
        {
            MailMsg mailMsg = new MailMsg();
            mailMsg.RecipEmailAddr = (ConfigCommonKeyReader.UploadDocumentsEmailTo);
            mailMsg.Subject = ThemeManager.ThemeSharedContent(model["MailSubject"]);
            mailMsg.MessageBody = ThemeManager.ThemeSharedContent(model["MailBody"], new
            {
                BaseUrl = model["BaseUrl"],
                AbsoulteURL = model["AbsoulteURL"],
                DateTime = DateTime.Now,
                IPAddress = model["IPAddress"],
                PolicyCode = model["PolicyCode"],
                ContactNumber = model["ContactNumber"],
                UserEmail = model["UserEmail"],
                Description = model["Description"],
                BusinessName = model["BusinessName"],
                SupportPhoneNumber = model["SupportPhoneNumber"],
                SupportPhoneNumberHref = model["SupportPhoneNumberHref"],
                WebsiteUrlText = model["WebsiteUrlText"],
                SupportEmailTextSalesSupport = model["SupportEmailTextSalesSupport"],
                SupportEmailHrefSalesSupport = model["SupportEmailHrefSalesSupport"]
            });

            mailMsg.Cc = (ConfigCommonKeyReader.UploadDocumentsEmailCc);
            mailMsg.Bcc = (ConfigCommonKeyReader.UploadDocumentsEmailBcc);
            mailMsg.SenderEmailAddr = ConfigCommonKeyReader.UploadDocumentsEmailFrom;

            return mailMsg;
        }

        #endregion

        #endregion

        #region Mail helper method

        /// <summary>
        /// Send mail with details to user
        /// </summary>
        /// <param name="mail"></param>
        public static bool SendMail(MailMsg mail)
        {
            try
            {
                MailHelper mailMessage = new MailHelper();
                //mailMessage.SendMailMessage(null, mail.RecipEmailAddr.Split(';').ToList(), mail.Subject, mail.MessageBody);
                mailMessage.SendMailMessage(mail.SenderEmailAddr, mail.RecipEmailAddr, mail.Cc, mail.Bcc, mail.Subject, mail.MessageBody);
            }
            catch
            {
                throw;
            }
            return true;
        }

        #endregion
    }
}
