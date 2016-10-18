#region Using directives

using BHIC.Domain.Policy;
using System;

#endregion

namespace BHIC.Contract.PolicyCentre
{
    public interface IBillingService
    {
        BillingResponse GetBillingDetails(Domain.Policy.BillingRequestParms args);
    }
}
