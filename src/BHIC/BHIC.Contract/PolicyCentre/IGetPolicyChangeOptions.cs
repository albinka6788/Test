using BHIC.Domain.PolicyCentre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Contract.PolicyCentre
{
    public interface IGetPolicyChangeOptions
    {
        /// <summary>
        /// Get Policy Change options 
        /// </summary>
        /// <param name="lineOfBusinessID">Line of business</param>
        /// <returns>Dictionary</returns>
        List<DropDownOptions> GetLineOfBusinessPolicyChangeOptions(int lineOfBusinessID);
    }
}
