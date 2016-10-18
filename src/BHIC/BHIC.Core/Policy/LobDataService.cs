#region Using directives

using BHIC.Common;
using BHIC.Common.Client;
using BHIC.Common.Config;
using BHIC.Contract.Policy;
using BHIC.Contract.Provider;
using BHIC.Domain.Policy;
using BHIC.Domain.Service;
using System;
using System.Collections.Generic;

#endregion

namespace BHIC.Core.Policy
{
    public class LobDataService : IServiceProviders, ILobDataService
    {
        #region Methods

        public LobDataService(ServiceProvider provider)
        {
            base.ServiceProvider = provider;
        }

        #region Public Methods

        /// <summary>
        /// Returns list of LobData based on QuoteId,LobAbbr and StateAbbr
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public List<LobData> GetLobDataList(LobDataRequestParms args)
        {
            var lobDataResponse = SvcClient.CallService<LobDataResponse>(string.Concat(Constants.LobData,
                UtilityFunctions.CreateQueryString<LobDataRequestParms>(args)), ServiceProvider);

            if (lobDataResponse.OperationStatus.RequestSuccessful)
            {
                return lobDataResponse.LobDataList;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(lobDataResponse.OperationStatus.Messages));
            }

        }

        /// <summary>
        /// Save new LobData details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public OperationStatus AddLobData(LobData args)
        {
            var lobDataResponse = SvcClient.CallService<LobData, OperationStatus>("LobData", "POST", args, ServiceProvider);

            if (lobDataResponse.RequestSuccessful)
            {
                return lobDataResponse;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(lobDataResponse.Messages));
            }
        }

        #endregion

        #endregion
    }
}
