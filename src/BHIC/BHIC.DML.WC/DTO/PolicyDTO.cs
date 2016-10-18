#region Using directives

using System;

#endregion

namespace BHIC.DML.WC
{
    public class PolicyDTO : BaseClass
    {
        public string QuoteNumber { get; set; }
        public string PolicyNumber { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public decimal PremiumAmount { get; set; }
        public int PaymentOptionID { get; set; }
    }
}
