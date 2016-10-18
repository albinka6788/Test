#region Using directives

using System;

#endregion

namespace BHIC.DML.WC
{
    public class PolicyPaymentDetailDTO : BaseClass
    {
        public int PolicyID { get; set; }
        public string CreditCardNumber { get; set; }
        public DateTime CardExpiryDate { get; set; }
        public string TransactionCode { get; set; }
        public decimal AmountPaid { get; set; }
    }
}
