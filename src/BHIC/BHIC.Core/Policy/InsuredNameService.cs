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
    public class InsuredNameService : IServiceProviders, IInsuredNameService
    {
        #region Methods
        public InsuredNameService(ServiceProvider provider)
        {
            base.ServiceProvider = provider;
        }
        #region Public Methods

        /// <summary>
        /// Returns list of InsuredName based on QuoteId and InsuredNameId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public List<InsuredName> GetInsuredNameList(InsuredNameRequestParms args)
        {
            var insuredNameResponse = SvcClient.CallService<InsuredNameResponse>(string.Concat(Constants.InsuredName,
                UtilityFunctions.CreateQueryString<InsuredNameRequestParms>(args)), ServiceProvider);

            if (insuredNameResponse.OperationStatus.RequestSuccessful)
            {
                return insuredNameResponse.InsuredNames;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(insuredNameResponse.OperationStatus.Messages));
            }
        }

        /// <summary>
        /// Save new InsuredName details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public OperationStatus AddInsuredName(InsuredName args)
        {
            var insuredNameResponse = SvcClient.CallService<InsuredName, OperationStatus>(Constants.InsuredName, "POST", args, ServiceProvider);

            if (insuredNameResponse.RequestSuccessful)
            {
                return insuredNameResponse;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(insuredNameResponse.Messages));
            }

        }

        /// <summary>
        /// Delete existing InsuredName details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public OperationStatus DeleteInsuredName(InsuredNameRequestParms args)
        {
            var insuredNameResponse = SvcClient.CallService<InsuredNameRequestParms, OperationStatus>(Constants.InsuredName, "DELETE", args, ServiceProvider);

            if (insuredNameResponse.RequestSuccessful)
            {
                return insuredNameResponse;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(insuredNameResponse.Messages));
            }

        }

        #endregion

        #endregion
    }
}
