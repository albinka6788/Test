#region Using directives

using BHIC.DML.WC.DTO;
using System;
using System.Collections.Generic;

#endregion

namespace BHIC.Contract.PurchasePath
{
    public interface IMultiStateService
    {
        /// <summary>
        /// Get list of states from database
        /// </summary>
        /// <returns></returns>
        List<ZipCodeStates> GetStates();

        /// <summary>
        /// Set list of states into cache
        /// </summary>
        /// <returns></returns>
        bool SetStatesCache();
    }
}
