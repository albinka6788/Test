using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Contract.Policy;
using BHIC.Common;
using BHIC.Domain.Policy;
using BHIC.Common.Config;
using BHIC.Domain.Service;
using BHIC.Contract.Provider;
using BHIC.Common.Client;

namespace BHIC.Core.Policy
{
    public class PolicyDataService : IServiceProviders, IPolicyDataService
    {
        public PolicyDataService(ServiceProvider provider)
        {
            base.ServiceProvider = provider;
        }

        #region Methods

        #region Public Methods

        /// <summary>
        /// Return PolicyData object based on QuoteId
        /// </summary>
        /// <returns></returns>
        public PolicyDataResponse GetPolicyData(PolicyDataRequestParms args)
        {
            var policyDataResponse = SvcClient.CallService<PolicyDataResponse>(string.Concat("PolicyData",
                                        UtilityFunctions.CreateQueryString<PolicyDataRequestParms>(args)), ServiceProvider);

            if (policyDataResponse.OperationStatus.RequestSuccessful)
            {
                return policyDataResponse;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(policyDataResponse.OperationStatus.Messages));
            }
        }

        /// <summary>
        /// Save new PolicyData details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public OperationStatus AddPolicyData(PolicyData args)
        {
            var policyDataResponse = SvcClient.CallService<PolicyData, OperationStatus>("PolicyData", "POST", args, ServiceProvider);

            if (policyDataResponse.RequestSuccessful)
            {
                return policyDataResponse;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(policyDataResponse.Messages));
            }
        }

        /// <summary>
        /// Delete existing PolicyData details based on QuoteId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public OperationStatus DeletePolicyData(PolicyDataRequestParms args)
        {
            var policyDataResponse = SvcClient.CallService<PolicyDataRequestParms, OperationStatus>("PolicyData", "DELETE", args, ServiceProvider);

            if (policyDataResponse.RequestSuccessful)
            {
                return policyDataResponse;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(policyDataResponse.Messages));
            }
        }

        #endregion

        #endregion
    }
}
