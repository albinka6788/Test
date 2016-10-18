#region Using directives

using System.Collections.Generic;

using BHIC.Domain.Background;
using BHIC.Common.Client;

#endregion

namespace BHIC.Contract.Background
{
    public interface IPaymentPlanService
    {
        /// <summary>
        /// Returns list of payment plans based on LobAbbr,StateAbbr,Premium and PaymentPlanId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        List<PaymentPlan> GetPaymentPlanList(PaymentPlanRequestParms args);
    }
}
