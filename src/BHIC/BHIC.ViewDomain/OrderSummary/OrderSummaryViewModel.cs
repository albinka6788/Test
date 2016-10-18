#region Using directives

using System;

#endregion

namespace BHIC.ViewDomain.OrderSummary
{
    public class OrderSummaryViewModel
    {
        public string WcQuoteId { get; set; }
        public string MgaCode { get; set; }
        public string PaymentConfirmationNumber { get; set; }
        public int IsPaymentSuccess { get; set; }
        public string PaymentErrorMessage { get; set; }
        public string PolicyCentreURL { get; set; }
        public string ProductName { get; set; }
        public string PhoneNumber { get; set; }
        public string CompanyDomain { get; set; }
    }
}
