#region Using directives

using BHIC.DML.WC.DTO;
using System;
using System.Collections.Generic;

#endregion

namespace BHIC.Contract.PurchasePath
{
    public interface ILineOfBusinessService
    {
        /// <summary>
        /// Get List of all Line of business
        /// </summary>
        /// <returns>Returns list of LineOfBusiness</returns>
        List<LineOfBusiness> GetLineOfBusiness();

        /// <summary>
        /// Set LineOfBusiness list into cache
        /// </summary>
        /// <returns></returns>
        bool SetLineOfBusinessCache();
    }
}
