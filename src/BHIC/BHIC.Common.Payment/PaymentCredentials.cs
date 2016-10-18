using System;
using System.Collections.Generic;
using System.Linq;

using BHIC.Common.XmlHelper;
using BHIC.Contract.Background;
using BHIC.Core.Background;
using BHIC.Common.Client;
using BHIC.Domain.Background;

namespace BHIC.Common.Payment
{
    /// <summary>
    /// This class collects details for the credentials
    /// of User for a payment gateway
    /// </summary>
    public class PaymentCredentials
    {
        string paymentAPIURL;

        /// <summary>
        /// User Login Id or API Login Id
        /// </summary>
        public string TransactionUserId { get; set; }

        /// <summary>
        /// User Transaction key or Password or Secure Key
        /// </summary>
        public string Password
        {
            get;
            set;
        }
        /// <summary>
        /// Version of AuthNet being used
        /// </summary>
        public string AuthNetVersion
        {
            get;
            set;
        }

        /// <summary>
        /// Check if the mode is Test Mode
        /// </summary>
        public bool IsTestMode
        {
            get;
            set;
        }

        /// <summary>
        /// Check if the mode is Test Mode
        /// </summary>
        public string TransactionKey
        {
            get;
            set;
        }

        public string BaseAddress
        {
            get
            {
                return paymentAPIURL;
            }
        }

        //public string CommonRedirectUrl { get; set; }

        public PaymentCredentials()
        {

            if (ConfigCommonKeyReader.IsPaymentConfigEntry)
            {
                TransactionUserId = ConfigCommonKeyReader.PaymentLogin;
                TransactionKey = ConfigCommonKeyReader.PaymentTransactionKey;
                IsTestMode = ConfigCommonKeyReader.IsTestingPaymentGateway;
                paymentAPIURL = ConfigCommonKeyReader.PaymentAPIURL;
            }
            else
            {
                ServiceProvider guardServiceProvider = new GuardServiceProvider() { ServiceCategory = ServiceProviderConstants.GuardServiceCategoryWC };

                ISystemVariableService systemVariableService = new SystemVariableService(guardServiceProvider);

                TransactionUserId = systemVariableService.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["CreditCardPayments_AuthorizeNET_LoginID"]);
                TransactionKey = systemVariableService.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["CreditCardPayments_AuthorizeNET_transactionKey"]);
                IsTestMode = Convert.ToBoolean(systemVariableService.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["CreditCardPayments_AuthorizeNET_InTesting"]));
                paymentAPIURL = systemVariableService.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["CreditCardPayments_AuthorizeNET_post_url"]);
            }
        }
    }
}

