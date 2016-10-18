#region Using directives

using System;

#endregion

namespace BHIC.ViewDomain.QuestionEngine
{
    public class QuoteResultViewModel
    {
        public string DsQuoteId { get; set; }
        public string LowestDownPayment { get; set; }
        public string Premium { get; set; }
        public string CurrentDue { get; set; }
        public string FutureInstallmentAmount { get; set; }
        public string NoOfInstallment { get; set; }
        public string InstallmentFee { get; set; }
        public int? PaymentPlanId { get; set; }
        public string Frequency { get; set; }					// E,A,Q,W,M,S,B,N,
        public string FrequencyCode { get; set; }
    }
}
