#region Using directives

using BHIC.Domain.Policy;
using BHIC.Domain.Service;
using System;
using System.Collections.Generic;

#endregion

namespace BHIC.Contract.Policy
{
    public interface IPaymentService
    {
        /// <summary>
        /// Returns list of Payment based on QuoteId,PolicyId and PaymentId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        List<Payment> GetPaymentList(PaymentRequestParms args);

        /// <summary>
        /// Save new Payment details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        OperationStatus AddPayment(Payment args);

    }
}
