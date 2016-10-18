#region Using directives

using BHIC.Common;
using BHIC.Common.Client;
using BHIC.Common.Config;
using BHIC.Contract.Policy;
using BHIC.Contract.Provider;
using BHIC.Domain.Policy;
using BHIC.Domain.Service;
using System;

#endregion

namespace BHIC.Core.Policy
{
    public class PolicyCreateService : IServiceProviders, IPolicyCreateService
    {
        public PolicyCreateService(ServiceProvider provider)
        {
            base.ServiceProvider = provider;
        }
        
        #region Methods

        #region Public Methods

        /// <summary>
        /// Create new policy
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public OperationStatus AddPolicy(PolicyCreate args)
        {
            var policyDataResponse = SvcClient.CallService<PolicyCreate, OperationStatus>(Constants.PolicyCreate, "POST", args, ServiceProvider);

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
