#region Using directives

using BHIC.Domain.Policy;
using BHIC.Domain.Service;
using System.Collections.Generic;

#endregion

namespace BHIC.Contract.Policy
{
    public interface ILobDataService
    {
        
        /// <summary>
        /// Returns list of LobData based on QuoteId,LobAbbr and StateAbbr
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        List<LobData> GetLobDataList(LobDataRequestParms args);


        /// <summary>
        /// Save new LobData details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        OperationStatus AddLobData(LobData args);
    }
}
