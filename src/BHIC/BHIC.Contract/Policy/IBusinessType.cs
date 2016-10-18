using BHIC.Domain.Background;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Contract.Policy
{
    public interface IBusinessTypeService
    {
        /// <summary>
        /// Returns list of payment plans based on LobAbbr,StateAbbr,Premium and PaymentPlanId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        List<BusinessType> GetBusinessTypes(BusinessTypeRequestParms args);
    }
}
