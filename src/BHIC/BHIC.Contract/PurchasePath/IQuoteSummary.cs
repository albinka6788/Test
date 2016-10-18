#region Using Directives

using System.Collections.Generic;

using BHIC.Domain.Background;
using BHIC.Domain.Policy;
using BHIC.Domain.Service;
using BHIC.ViewDomain;

using DML_DTO = BHIC.DML.WC.DTO;
using System;

#endregion

namespace BHIC.Contract.PurchasePath
{
    public interface IQuoteSummary
    {
        #region Main methods

        /// <summary>
        /// Get list of applicable payment plans option for particular quote
        /// </summary>
        /// <param name="premium"></param>
        /// <returns></returns>
        List<PaymentPlan> GetPaymentPlans(decimal premium, Int32 quoteId);

        /// <summary>
        /// Get lowest installment premium amount for this Quote
        /// </summary>
        /// <returns></returns>
        decimal GetLowestInstallmentPremium(List<PaymentPlan> paymentPlans, decimal quotePremiumAmt);

        /// <summary>
        /// Retrun GetQuoteSummaryVM object for quote summary page session data manipulation
        /// </summary>
        /// <returns></returns>
        QuoteSummaryViewModel GetQuoteSummaryVM();

        /// <summary>
        /// Get down payment or current due amount based on selected payment plan for this quote
        /// </summary>
        /// <param name="totalPremium"></param>
        /// <param name="selectedPaymentPlan"></param>
        /// <returns></returns>
        decimal GetDownPaymentAmount(decimal totalPremium, PaymentPlan selectedPaymentPlan);

        /// <summary>
        /// Get FutureInstallmentAmount based on selected payment plan, total premium for this quote
        /// </summary>
        /// <param name="totalPremium"></param>
        /// <param name="selectedPaymentPlan"></param>
        /// <returns></returns>
        decimal GetFutureInstallmentAmount(decimal totalPremium, PaymentPlan selectedPaymentPlan);

        /// <summary>
        /// Get default selected payment plan for this QuoteId 
        /// </summary>
        /// <param name="QuoteId"></param>
        /// <returns>List<PaymentTerms></returns>
        PaymentTerms GetPaymentTerms(int QuoteId);

        /// <summary>
        /// Post user selected payment plan into provider system
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        OperationStatus AddPaymentTerms(PaymentTerms args);

        /// <summary>
        /// Post user new quote details into database table
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        bool AddUpdateQuoteData(DML_DTO::QuoteDTO quoteData);

        /// <summary>
        /// Retrun mandatory-deductible status for supplied StateCode
        /// </summary>
        /// <param name="stateCode"></param>
        /// <returns></returns>
        bool StateHasMandatoryDeductible(string stateCode);

        #endregion
    }
}
