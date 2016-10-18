#region Using directives

using BHIC.DML.WC.DTO;
using System.Collections.Generic;

#endregion

namespace BHIC.DML.WC.DataContract
{
    public interface IStateTypeService
    {
        /// <summary>
        /// Get List of Good and bad states
        /// </summary>
        /// <returns>Returns all good and bad states</returns>
        List<StateType> GetAllGoodAndBadState();

        /// <summary>
        /// Set List of Good and bad states into cache
        /// </summary>
        /// <returns></returns>
        bool SetAllGoodAndBadStateCache();
    }
}
