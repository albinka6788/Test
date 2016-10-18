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

#endregion

namespace BHIC.Core.Policy
{
    public class PaymentService : IServiceProviders, IPaymentService
    {
        #region Constructor

        public PaymentService(ServiceProvider provider)
        {
            base.ServiceProvider = provider;
        }

        #endregion

        #region Methods

        #region  Interface implementation

        /// <summary>
        /// Returns list of Payment based on QuoteId,PolicyId and PaymentId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        List<Payment> IPaymentService.GetPaymentList(PaymentRequestParms args)
        {
            var paymentResponse = SvcClient.CallService<PaymentResponse>(string.Concat(Constants.Payments,
                UtilityFunctions.CreateQueryString<PaymentRequestParms>(args)), ServiceProvider);

            if (paymentResponse.OperationStatus.RequestSuccessful)
            {
                return paymentResponse.Payments;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(paymentResponse.OperationStatus.Messages));
            }
        }

        /// <summary>
        /// Save new Payment details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        OperationStatus IPaymentService.AddPayment(Payment args)
        {
            var paymentResponse = SvcClient.CallService<Payment, OperationStatus>(Constants.Payments,
                "POST", args, ServiceProvider);

            if (paymentResponse.RequestSuccessful)
            {
                return paymentResponse;
            }
            else
            {
                throw new ApplicationException((paymentResponse.Messages != null && paymentResponse.Messages.Count > 0 ?
                    UtilityFunctions.ConvertMessagesToString(paymentResponse.Messages) :
                    "Error occurred while processsing Guard API, please see log for more detail"));
            }
        }

        #endregion

        #endregion
    }
}
