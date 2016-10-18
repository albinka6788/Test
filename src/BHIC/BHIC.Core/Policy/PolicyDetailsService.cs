#region Using directives

using BHIC.Common;
using BHIC.Common.Client;
using BHIC.Contract.Policy;
using BHIC.Contract.Provider;
using BHIC.Domain.Policy;
using System;

#endregion

namespace BHIC.Core.Policy
{
    public class PolicyDetailsService : IServiceProviders, IPolicyDetailsService
    {
        public PolicyDetailsService(ServiceProvider provider)
        {
            base.ServiceProvider = provider;            
        }
        #region Methods

        #region Public Methods

        /// <summary>
        /// Return PolicyDetails object based on PolicyCode
        /// </summary>
        /// <returns></returns>
        public PolicyDetailsResponse GetPolicyDetails(PolicyDetailsRequestParms args)
        {
            var policyDataResponse = SvcClient.CallService<PolicyDetailsResponse>(string.Concat("PolicyDetails",
                UtilityFunctions.CreateQueryString<PolicyDetailsRequestParms>(args)), ServiceProvider);

            if (policyDataResponse.OperationStatus.RequestSuccessful)
            {
                return policyDataResponse;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(policyDataResponse.OperationStatus.Messages));
            }
        }
      
        #endregion

        #endregion
    }
}
