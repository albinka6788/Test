#region Using directives

using BHIC.Common.Client;
using BHIC.Contract.Policy;
using BHIC.Domain.Policy;
using BHIC.Domain.Service;
using System;
using System.Web.Mvc;

#endregion

namespace BHIC.Core.Policy
{
    class GeneratePolicyService
    {
        #region Methods

        #region Public Methods

        /// <summary>
        /// It will create new policy using PolicyCreate api
        /// </summary>
        /// <param name="lob">Line of business</param>
        /// <param name="quoteId">Quote Id</param>
        /// <param name="serviceProvider">Service Provider Name</param>
        /// <returns>Return true if successfully created, false otherwise</returns>
        [Obsolete("This method is depreciated, please use IGeneratePolicy.CreatePolicy method")]
        public string CreatePolicy(string lob, int quoteId, ServiceProvider serviceProvider)
        {
            //Call PolicyCreateService 
            IPolicyCreateService policyCreateService = new PolicyCreateService(serviceProvider);
            OperationStatus createPolicyResponse = policyCreateService.AddPolicy(new PolicyCreate { LOB = lob.Trim(), QuoteId = quoteId });

            //If policy created successfully call GeneratePolicy
            if (createPolicyResponse.RequestSuccessful)
            {
                return GetPolicyCode(quoteId, serviceProvider);
            }

            return null;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Return PolicyCode based on parameters
        /// </summary>
        /// <param name="quoteId">Quote Id</param>
        /// <param name="serviceProvider">Service Provider Name</param>
        /// <returns>Return policy code on success, null otherwise</returns>
        private string GetPolicyCode(int quoteId, ServiceProvider serviceProvider)
        {
            IPolicyDataService policyDataService = new PolicyDataService(serviceProvider);
            PolicyDataResponse policyResp = policyDataService.GetPolicyData(new PolicyDataRequestParms { QuoteId = quoteId });

            if (policyResp != null && policyResp.PolicyData != null && !string.IsNullOrWhiteSpace(policyResp.PolicyData.MgaCode))
            {
                IPolicyDetailsService policyDetailsService = new PolicyDetailsService(serviceProvider);
                PolicyDetailsResponse policyDetailResponse = policyDetailsService.GetPolicyDetails(new PolicyDetailsRequestParms { PolicyCode = policyResp.PolicyData.MgaCode });

                if (!(policyDetailResponse.PolicyDetails == null && !policyDetailResponse.OperationStatus.RequestSuccessful))
                {
                    return policyDetailResponse.PolicyDetails.PolicyCode;
                }
            }

            return null;
        }

        #endregion

        #endregion
    }
}
