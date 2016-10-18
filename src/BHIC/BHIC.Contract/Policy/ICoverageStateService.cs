#region Using directives

using BHIC.Domain.Policy;
using System;
using System.Collections.Generic;

#endregion

namespace BHIC.Contract.Policy
{
    public interface ICoverageStateService
    {
        /// <summary> 
        /// Returns list of Coverage State based on QuoteId,LobAbbr and StateAbbr
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        List<CoverageState> GetCoverageStateList(CoverageStateRequestParms args);
    }
}
