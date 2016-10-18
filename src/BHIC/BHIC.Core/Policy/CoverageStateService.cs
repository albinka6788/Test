#region Using directives

using BHIC.Common;
using BHIC.Common.Client;
using BHIC.Common.Config;
using BHIC.Contract.Policy;
using BHIC.Contract.Provider;
using BHIC.Domain.Policy;
using System;
using System.Collections.Generic;

#endregion

namespace BHIC.Core.Policy
{
    public class CoverageStateService : IServiceProviders, ICoverageStateService
    {
        #region Constructors

        public CoverageStateService(ServiceProvider provider)
        {
            base.ServiceProvider = provider;
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary> 
        /// Returns list of Coverage State based on QuoteId,LobAbbr and StateAbbr
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public List<CoverageState> GetCoverageStateList(CoverageStateRequestParms args)
        {
            var coverageStateResponse = SvcClient.CallService<CoverageStateResponse>(string.Concat(Constants.CoverageState,
                UtilityFunctions.CreateQueryString<CoverageStateRequestParms>(args)), ServiceProvider);

            if (coverageStateResponse.OperationStatus.RequestSuccessful)
            {
                return coverageStateResponse.CoverageStates;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(coverageStateResponse.OperationStatus.Messages));
            }
        }

        #endregion

        #endregion
    }
}
