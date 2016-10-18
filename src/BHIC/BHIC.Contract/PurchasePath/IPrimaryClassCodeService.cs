#region Using directives

using BHIC.DML.WC.DTO;
using System;
using System.Collections.Generic;

#endregion

namespace BHIC.Contract.PurchasePath
{
    public interface IPrimaryClassCodeService
    {
        /// <summary>
        /// Get List of all Line of business
        /// </summary>
        /// <returns>Returns list of LineOfBusiness</returns>
        List<PrimaryClassCodeDTO> GetPrimaryClassCodeDataList();
    }
}
