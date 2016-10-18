#region Using directives

using BHIC.Common;
using BHIC.Common.Client;
using BHIC.Common.Config;
using BHIC.Contract.Policy;
using BHIC.Contract.Provider;
using BHIC.Domain.Policy;
using BHIC.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace BHIC.Core.Policy
{
    public class PaymentTermService : IServiceProviders,IPaymentTermService
    {
        #region Comment : Here constructor

        public PaymentTermService(){}

        public PaymentTermService(ServiceProvider provider)
        {
            base.ServiceProvider = provider;
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Returns PaymentTerms based on QuoteId and PaymentPlanId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public PaymentTerms GetPaymentTermsList(PaymentTermsRequestParms args)
        {
            var paymentTermsResponse = SvcClient.CallService<PaymentTermsResponse>(string.Concat(Constants.PaymentTerms,
                UtilityFunctions.CreateQueryString<PaymentTermsRequestParms>(args)),ServiceProvider);

            if (paymentTermsResponse.OperationStatus.RequestSuccessful)
            {
                return paymentTermsResponse.PaymentTerms;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(paymentTermsResponse.OperationStatus.Messages));
            }
        }

        /// <summary>
        /// Save new PaymentTerms details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public OperationStatus AddPaymentTerms(PaymentTerms args)
        {
            var paymentTermsResponse = SvcClient.CallServicePost<PaymentTerms, OperationStatus>(Constants.PaymentTerms,args,ServiceProvider);

            //Comment: Here return response when successfully processed and even when there are some non-system errors(functional errors)
            if (paymentTermsResponse.RequestSuccessful || !paymentTermsResponse.Messages.Any(res => res.MessageType == Domain.Service.MessageType.SystemError))
            {
                return paymentTermsResponse;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(paymentTermsResponse.Messages));
            }
        }

        /// <summary>
        /// Delete existing PaymentTerms details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public OperationStatus DeletePaymentTerms(PaymentTermsRequestParms args)
        {
            var paymentTermsResponse = SvcClient.CallService<PaymentTermsRequestParms, OperationStatus>(Constants.PaymentTerms, "DELETE", args,ServiceProvider);

            if (paymentTermsResponse.RequestSuccessful)
            {
                return paymentTermsResponse;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(paymentTermsResponse.Messages));
            }
        }

        #endregion

        #endregion
    }
}
