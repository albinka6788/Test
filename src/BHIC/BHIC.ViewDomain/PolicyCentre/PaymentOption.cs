#region Using directives

using System;
using System.Collections.Generic;

#endregion

namespace BHIC.ViewDomain.PolicyCentre
{
    public class PaymentOption
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public bool IsEnabled { get; set; }
    }
}
