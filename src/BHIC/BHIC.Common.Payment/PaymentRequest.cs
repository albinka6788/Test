#region Using directives

using System;

#endregion

namespace BHIC.Common.Payment
{
    /// <summary>
    /// This class will provide details of the card used for payment
    /// </summary>
    public class PaymentRequest
    {
        public string AgencyCode { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal FeeAmount { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public bool FromPolicyCenter { get; set; }
        public string PaymentCode { get; set; }
        public bool IsPaymentRequestReceivedFromPurchasePath { get; set; }
    }
}
