#region Using directives

using BHIC.Domain.Background;
using BHIC.Domain.Policy;
using BHIC.ViewDomain.Landing;
using System;
using System.Collections.Generic;

#endregion

namespace BHIC.ViewDomain.BuyPolicy
{
    public class BuyPolicyViewModel
    {
        public string UserEmailId { get; set; }
        public decimal InstallmentFee { get; set; }
        public decimal PremiumAmt { get; set; }
        public string ProductName { get; set; }

        public List<PaymentPlan> PaymentOptions { get; set; }
        public PaymentTerms PaymentTerms { get; set; }
        public List<NavigationModel> NavLinks { get; set; }
    }
}
