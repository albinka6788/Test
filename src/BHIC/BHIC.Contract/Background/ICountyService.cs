#region Using directives

using BHIC.Common.Client;
using BHIC.Domain.Background;
using System;
using System.Collections.Generic;

#endregion

namespace BHIC.Contract.Background
{
    public interface ICountyService
    {
        /// <summary>
        /// Returns county list from cache
        /// </summary>
        /// <returns>List of county</returns>
        IList<County> GetCounty(bool isThreadMode);
        
        /// <summary>
        /// Set county list into cache
        /// </summary>
        /// <returns></returns>
        bool SetCountyCache();
    }
}
