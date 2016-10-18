#region Using directives

using BHIC.Domain.Policy;
using System;

#endregion

namespace BHIC.Contract.Policy
{
    public interface IPolicyDetailsService
    {
        /// <summary>
        /// Return PolicyDetails object based on PolicyCode
        /// </summary>
        /// <returns></returns>
        PolicyDetailsResponse GetPolicyDetails(PolicyDetailsRequestParms args);
    }
}
