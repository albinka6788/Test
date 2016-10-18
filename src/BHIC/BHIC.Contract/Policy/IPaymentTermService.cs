#region Using directives

using BHIC.Domain.Policy;
using BHIC.Domain.Service;
using System;
using System.Collections.Generic;

#endregion

namespace BHIC.Contract.Policy
{
    public interface IPaymentTermService
    {
        /// <summary>
        /// Returns PaymentTerms based on QuoteId and PaymentPlanId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        PaymentTerms GetPaymentTermsList(PaymentTermsRequestParms args);

        /// <summary>
        /// Save new PaymentTerms details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        OperationStatus AddPaymentTerms(PaymentTerms args);

        /// <summary>
        /// Delete existing PaymentTerms details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        OperationStatus DeletePaymentTerms(PaymentTermsRequestParms args);
    }
}
