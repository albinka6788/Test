#region Using Directives

using System;
using System.Collections.Generic;

using BHIC.Common;
using BHIC.Common.Mailing;
using BHIC.ViewDomain.Landing;
using BHIC.ViewDomain.Mailing;
using BHIC.ViewDomain;


#endregion

namespace BHIC.Core.PurchasePath
{
    public class MailTemplateBuilder
    {
        #region Variables : Class-Level local variables decalration

        List<string> mailRecipient = new List<string>() { "prem.pratap@xceedance.com" };

        #endregion

        #region Main methods

        /// <summary>
        /// Prepare mail-message for SaveForLater with populated data from supplied model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="mailContentModel"></param>
        /// <returns></returns>
        public MailMsg MailSaveForLater(SaveForLaterViewModel model, Dictionary<string, object> mailContentModel)
        {
            #region Comment : Here prepare templatized SaveForLater mail attributes based on theme shared content

            // build the link that the recipient will click to access their quote, and get the recipient's email address
            string anchorText = Convert.ToString(mailContentModel["anchorText"]);

            // build the link that the recipient will click to access their quote, and get the recipient's email address
            var mailMsg = BuildEmailLinkAndRecipient(model.BaseUrl, model.SubUrl, anchorText, model.RecipEmail);

            //currently we could not found appropriate values, so hard codded values are used for testing purpose
            var bodyContent = Convert.ToString(mailContentModel["saveforlaterMailBody"]);

            // get the canned message template, embed the link that the user will click on
            mailMsg.MessageBody = string.IsNullOrEmpty(bodyContent) ? "" :
                UtilityFunctions.FillTemplateWithModelValues(bodyContent,
                new
                {
                    ReturnLink = mailMsg.MessageBody,
                    CompanyName = " Cover Your Business Insurance",
                    CompanyPoBox = "113247",
                    CompanyCityStateZip = "Stamford, CT 06911-3247",
                    SupportEmail = "csr@nlf-info.com",
                    SupportPhone = "844-472-0967",
                    Logo = "",
                    WebsiteURL = "https://www.bhdwc.com/",
                    RecipientEmail = mailMsg.RecipEmailAddr
                });

            // get the subject line
            mailMsg.Subject = UtilityFunctions.FillTemplateWithModelValues(Convert.ToString(mailContentModel["saveforlaterMailSubject"]), new { CompanyName = "Berkshire Hathaway" });

            //Comment : Here assign recipeint address
            mailMsg.RecipEmailAddr = model.RecipEmail;

            #endregion

            return mailMsg;
        }

        /// <summary>
        /// Prepare mail-message for SaveForLater with populated data from supplied model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="mailContentModel"></param>
        /// <returns></returns>
        public MailMsg MailSaveForLater(SaveForLaterMailViewModel model, Dictionary<string, object> mailContentModel)
        {
            #region Comment : Here prepare templatized SaveForLater mail attributes based on theme shared content

            var mailMsg = new MailMsg();

            // get the subject line
            mailMsg.Subject = UtilityFunctions.FillTemplateWithModelValues(Convert.ToString(mailContentModel["saveforlaterMailSubject"]), new { WebsiteUrlText = model.WebsiteUrlText });

            //Comment : Here get body template content 
            var bodyContent = Convert.ToString(mailContentModel["saveforlaterMailBody"]);

            //Comment : Here In this next step fill template with model values
            mailMsg.MessageBody = string.IsNullOrEmpty(bodyContent) ? "" : UtilityFunctions.FillTemplateWithModelValues(bodyContent, model);

            #endregion

            return mailMsg;
        }

        public MailMsg MailSoftReferral(ReferralQuoteMailViewModel model, Dictionary<string, object> mailContentModel)
        {
            #region Comment : Here prepare templatized Referral mail attributes based on theme shared content

            var mailMsg = new MailMsg();

            // get the subject line
            mailMsg.Subject = UtilityFunctions.FillTemplateWithModelValues(Convert.ToString(mailContentModel["referralMailSubject"]), new { LOB = model.LOB });

            //Comment : Here get body template content 
            var bodyContent = Convert.ToString(mailContentModel["referralMailBody"]);

            //Comment : Here In this next step fill template with model values
            mailMsg.MessageBody = string.IsNullOrEmpty(bodyContent) ? "" : UtilityFunctions.FillTemplateWithModelValues(bodyContent, model);

            #endregion

            return mailMsg;
        }

        /// <summary>
        /// Prepare mail-message for Welcome mail on policy purchase
        /// </summary>
        /// <param name="model"></param>
        /// <param name="mailContentModel"></param>
        /// <returns></returns>
        public MailMsg MailUserNewPolicyDetail(PolicyWelcomeMailViewModel model, Dictionary<string, object> mailContentModel)
        {
            #region Comment : Here prepare templatized SaveForLater mail attributes based on theme shared content

            var mailMsg = new MailMsg();

            // get the subject line
            mailMsg.Subject = UtilityFunctions.FillTemplateWithModelValues(Convert.ToString(mailContentModel["policyWelcomeMailSubject"]), new { WebsiteUrlText = model.WebsiteUrlText });

            //Comment : Here get body template content 
            var bodyContent = Convert.ToString(mailContentModel["policyWelcomeMailBody"]);

            //Comment : Here In this next step fill template with model values
            mailMsg.MessageBody = string.IsNullOrEmpty(bodyContent) ? "" : UtilityFunctions.FillTemplateWithModelValues(bodyContent, model);

            #endregion

            return mailMsg;
        }

        /// <summary>
        /// Send Reset Password Link to user
        /// </summary>
        /// <param name="model"></param>
        /// <param name="mailContentModel"></param>
        /// <returns></returns>
        public MailMsg MailUserResetPassword(ForgotPasswordViewModel model, Dictionary<string, object> mailContentModel)
        {
            #region Comment : Here prepare templatized SaveForLater mail attributes based on theme shared content

            var mailMsg = new MailMsg();

            // get the subject line
            mailMsg.Subject = UtilityFunctions.FillTemplateWithModelValues(Convert.ToString(mailContentModel["forgotPasswordMailSubject"]), new { CompanyName = model.CompanyName });

            //Comment : Here get body template content 
            var bodyContent = Convert.ToString(mailContentModel["forgotPasswordMailBody"]);

            //Comment : Here In this next step fill template with model values
            mailMsg.MessageBody = string.IsNullOrEmpty(bodyContent) ? "" : UtilityFunctions.FillTemplateWithModelValues(bodyContent, model);

            #endregion

            return mailMsg;
        }

        /// <summary>
        /// Prepare mail-message for paymentFailure mail on policy purchase
        /// </summary>
        /// <param name="model"></param>
        /// <param name="mailContentModel"></param>
        /// <returns></returns>
        public MailMsg MailPaymentFailureDetails(PaymentFailureViewModel model, Dictionary<string, object> mailContentModel)
        {
            #region Comment : Here prepare templatized SaveForLater mail attributes based on theme shared content

            var mailMsg = new MailMsg();

            // get the subject line
            mailMsg.Subject = UtilityFunctions.FillTemplateWithModelValues(Convert.ToString(mailContentModel["paymentFailureMailSubject"]), new { CompanyName = model.CompanyName });

            //Comment : Here get body template content 
            var bodyContent = Convert.ToString(mailContentModel["paymentFailureMailBody"]);

            //Comment : Here In this next step fill template with model values
            mailMsg.MessageBody = string.IsNullOrEmpty(bodyContent) ? "" : UtilityFunctions.FillTemplateWithModelValues(bodyContent, model);

            #endregion

            return mailMsg;
        }

        public MailMsg MailRegisterDetail(PolicyRegistrationMailViewModel model, Dictionary<string, object> mailContentModel)
        {
            #region Comment : Here prepare templatized SaveForLater mail attributes based on theme shared content

            var mailMsg = new MailMsg();

            // get the subject line
            mailMsg.Subject = UtilityFunctions.FillTemplateWithModelValues(Convert.ToString(mailContentModel["policyRegistrationMailSubject"]), new { CompanyName = model.CompanyName });

            //Comment : Here get body template content 
            var bodyContent = Convert.ToString(mailContentModel["policyRegistrationMailBody"]);

            //Comment : Here In this next step fill template with model values
            mailMsg.MessageBody = string.IsNullOrEmpty(bodyContent) ? "" : UtilityFunctions.FillTemplateWithModelValues(bodyContent, model);

            #endregion

            return mailMsg;
        }

        /// <summary>
        /// Send Schedule call details to internal team
        /// </summary>
        /// <param name="model"></param>
        /// <param name="mailContentModel"></param>
        /// <returns></returns>
        public MailMsg MailScheduleCall(ScheduleCallMailViewModel model, Dictionary<string, object> mailContentModel)
        {
            #region Comment : Here prepare templatized Schedule call mail attributes based on theme shared content

            var mailMsg = new MailMsg();

            // get the subject line
            mailMsg.Subject = UtilityFunctions.FillTemplateWithModelValues(Convert.ToString(mailContentModel["scheduleCallSubject"]), new { CompanyName = model.CompanyName });

            //Comment : Here get body template content 
            var bodyContent = Convert.ToString(mailContentModel["scheduleCallBody"]);

            //Comment : Here In this next step fill template with model values
            mailMsg.MessageBody = string.IsNullOrEmpty(bodyContent) ? "" : UtilityFunctions.FillTemplateWithModelValues(bodyContent, model);

            #endregion

            return mailMsg;
        }

        #endregion

        #region Mail helper methods

        private MailMsg BuildEmailLinkAndRecipient(string baseUrl, string subUrl, string anchorText, List<string> recipEmail)
        {
            return new MailMsg
            {
                MessageBody = "<a href='" + baseUrl + subUrl + "'  target='_blank' >" + anchorText + "</a>",
                RecipEmailAddr = recipEmail
            };
        }

        /// <summary>
        /// Send mail with details to user
        /// </summary>
        /// <param name="mail"></param>
        public bool SendMail(MailMsg mailMsg)
        {
            try
            {
                MailHelper mailMessage = new MailHelper();
                //mailMessage.SendMailMessage(mailMsg.SenderEmailAddr, mailMsg.RecipEmailAddr, mailMsg.Subject, mailMsg.MessageBody);
                mailMessage.SendMailMessage(mailMsg.SenderEmailAddr, mailMsg.RecipEmailAddr, mailMsg.Cc, mailMsg.Bcc, mailMsg.Subject, mailMsg.MessageBody);

                //Comment : Here if successfully sent
                return true;
            }
            catch
            {
                throw;
            }

            //return false;
        }

        /// <summary>
        /// Send mail with details to user
        /// </summary>
        /// <param name="mail"></param>
        public bool SendMailWithAttachments(MailMsg mailMsg, List<string> attachmentFilesFullPath, bool isEncryptedFile = false, string password = null)
        {
            try
            {
                MailHelper mailMessage = new MailHelper();
                mailMessage.SendMailMessage(mailMsg.SenderEmailAddr, mailMsg.RecipEmailAddr, mailMsg.Cc, mailMsg.Bcc, mailMsg.Subject, mailMsg.MessageBody, attachmentFilesFullPath, isEncryptedFile, password);

                //Comment : Here if successfully sent
                return true;
            }
            catch
            {
                throw;
            }

            //return false;
        }

        #endregion
    }
}
