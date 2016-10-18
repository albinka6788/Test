#region Using directives

using BHIC.Domain.Background;
using System.Collections.Generic;

#endregion

namespace BHIC.Contract.Background
{
    public interface ISystemVariableService
    {

        /// <summary>
        /// It will fetch Session Variable list from cache
        /// </summary>
        /// <returns></returns>
        List<SystemVariable> GetSystemVariables();

        /// <summary>
        /// Get all payement related system variables
        /// </summary>
        /// <returns>return payment related information</returns>
        List<SystemVariable> GetPGCredentials();

        /// <summary>
        /// Get system variable value filterd by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetSystemVariableByKey(string key);
    }
}
