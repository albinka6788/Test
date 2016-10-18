using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHIC.Domain.Policy;

namespace BHIC.Contract.PolicyCentre
{
    public interface IBillingSummaryService
    {
        /// <summary>
        /// Return PolicyDetails object based on PolicyCode
        /// </summary>
        /// <returns></returns>
        BillingSummaryResponse GetBillingSummary(BillingSummaryRequestParms args);
    }
}
