using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHIC.Contract.Background;
using BHIC.DML.WC.DataContract;
using BHIC.DML.WC.DataService;
using BHIC.DML.WC.DTO;
using BHIC.Common.XmlHelper;
using BHIC.Common;
using BHIC.Common.Configuration;
using BHIC.ViewDomain.Mailing;
using BHIC.Core.PurchasePath;
using System.Web;
using BHIC.Contract.Mailing;
using BHIC.Core.Mailing;
using BHIC.Common.Client;

namespace BHIC.Core.Background
{
    public class BackgroundSaveForLaterService : IBackgroundSaveForLaterService
    {
        IBackgroundProcessDataProvider provider = new BackgroundProcessDataProvider();
        /// <summary>
        /// Get the list of quotes for the inactive users and send save for later mail.
        /// </summary>
        void IBackgroundSaveForLaterService.TriggerSaveForLaterMail()
        {
            try
            {
                List<UserQuoteDTO> quotes = provider.GetInactiveUserQuote();
                foreach (UserQuoteDTO quote in quotes)
                {
                    int quoteNumber = Convert.ToInt32(quote.QuoteNumber);
                    int userId = Convert.ToInt32(quote.OrganizationUserDetailID);
                    string emailId = quote.EmailID;
                    bool mailSent = SendMailSaveForLater(quoteNumber, emailId, userId);
                    if (mailSent)
                    {
                        UpdateSaveForLaterMailStatus(userId);
                    }
                }
            }
            catch (Exception ex)
            {
                BHIC.Common.Logging.LoggingService.Instance.Fatal(ex);
                throw;
            }
        }
        /// <summary>
        /// Once save for later mail sent, update the status to mail sent.
        /// </summary>
        /// <param name="UserId"></param>

        private void UpdateSaveForLaterMailStatus(int UserId)
        {
            provider.UpdateSaveForLaterMailStatus(UserId);
        }

        /// <summary>
        /// Method will send mail to user with embedded link to re-invock the current user state in application
        /// </summary>
        /// <param name="pageMethod"></param>
        /// <param name="quoteId"></param>
        /// <returns></returns>
        private bool SendMailSaveForLater(int quoteId, string emailId, int? userId = null)
        {
            #region Comment : Here create mail embedded link


            //Comment : Here encrypt quoteId query-string 
            string quoteQueryString = Encryption.EncryptText(string.Format("{0};{1}", quoteId.ToString(), userId));
            string pageMethod = "PurchasePath/Quote/Index#/GetBusinessInfo";
            string subUrl = string.Format("{0}/{1}", pageMethod, HttpUtility.UrlEncode(quoteQueryString));
            #endregion

            #region Comment : Here prepare template populated with data and then send mail to user

            ServiceProvider guardServiceProvider = new GuardServiceProvider() { ServiceCategory = ServiceProviderConstants.GuardServiceCategoryWC };
            ISystemVariableService systemVariable = new SystemVariableService(guardServiceProvider);

            var saveForLaterMailmodel = new SaveForLaterMailViewModel
            {
                //Basic communication/contact details
                QuoteId = quoteId.ToString(),
                CompanyName = ConfigCommonKeyReader.ApplicationContactInfo["CompanyName"],
                WebsiteUrlText = systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_Domain"]),
                WebsiteUrlHref = systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_WebsiteShortURL"]),
                SupportEmailText = ConfigCommonKeyReader.ApplicationContactInfo["SupportEmailTextQuotes"],
                SupportEmailHref = ConfigCommonKeyReader.ApplicationContactInfo["SupportEmailHrefQuotes"],
                SupportPhoneNumber = systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["PhoneNumber_CSR"]),
                SupportPhoneNumberHref = string.Concat("tel:", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["PhoneNumber_CSR"])),
                Physical_Address2 = systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Physical_Address2"]),
                Physical_AddressCSZ = systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Physical_AddressCSZ"]),
                AbsoulteURL = string.Format(ConfigCommonKeyReader.WcEmailTemplateImgPath, ConfigCommonKeyReader.HostURL),
                //SaveForLater review-quote link details
                ReviewQuoteText = "",
                ReviewQuoteHref = string.Format("{0}://{1}{2}{3}",
                                        (ConfigCommonKeyReader.HostURL.Equals("localhost", StringComparison.OrdinalIgnoreCase) ? "http" : "https"),
                                        ConfigCommonKeyReader.HostURL, "/", subUrl)
            };

            #endregion

            IMailingService mailingService = new MailingService();

            //Send user mail
            var mailSentStatus = mailingService.SaveForLater(emailId, saveForLaterMailmodel);

            return mailSentStatus;
        }
    }
}
