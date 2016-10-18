#region Using directives
using System;
using System.Linq;

using BHIC.Common;
using BHIC.Common.Client;
using BHIC.Contract.Policy;
using BHIC.Contract.PurchasePath;
using BHIC.Core.Policy;
using BHIC.Domain.Policy;
using BHIC.Domain.Service;
using BHIC.Common.Logging;
using BHIC.Common.XmlHelper;

#endregion

namespace BHIC.Core.PurchasePath
{
    public class GeneratePolicy : IGeneratePolicy
    {
        internal ILoggingService loggingService = LoggingService.Instance;

        #region Methods

        #region Public Methods

        /// <summary>
        /// It will create new policy using PolicyCreate api
        /// </summary>
        /// <param name="lob">Line of business</param>
        /// <param name="quoteId">Quote Id</param>
        /// <param name="serviceProvider">Service Provider Name</param>
        /// <returns>Return true if successfully created, false otherwise</returns>
        string IGeneratePolicy.CreatePolicy(string lob, string userEmail, int quoteId, ServiceProvider serviceProvider)
        {
            //Call PolicyCreateService 
            IPolicyCreateService policyCreateService = new PolicyCreateService(serviceProvider);
            OperationStatus createPolicyResponse = policyCreateService.AddPolicy(new PolicyCreate { LOB = lob.Trim(), QuoteId = quoteId, UserId = userEmail });

            //If policy created successfully call GeneratePolicy
            if (createPolicyResponse.RequestSuccessful)
            {
                //fetch policy code from createPolicyResponse
                var policyCode = createPolicyResponse.AffectedIds.Where(x => x.DTOName.Equals(ConfigCommonKeyReader.PolicyCreateDTOName.ToString())
                    && x.DTOProperty.Equals(ConfigCommonKeyReader.PolicyCreateDTOProperty.ToString())).FirstOrDefault().IdValue;

                // return GetPolicyCode(quoteId, serviceProvider);
                // Commented below line of code, as now Guard taking care of attaching policy with User Email, to do that we passed User Email ID in policy create API call
                //if (!string.IsNullOrEmpty(policyCode))
                //{
                //    //Call UserPolicyCode POST service
                //    GetUserPolicyCodes(userEmail, policyCode, serviceProvider);
                //}

                return policyCode;
            }
            else
            {
                throw new ApplicationException((createPolicyResponse.Messages != null && createPolicyResponse.Messages.Count > 0 ?
                    UtilityFunctions.ConvertMessagesToString(createPolicyResponse.Messages) :
                    "Error occurred while processsing Guard API, please see log for more detail"));
            }
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
                return policyResp.PolicyData.MgaCode;
            }

            return null;
        }

        /// <summary>
        /// Store UserPolicyCode related information
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="PolicyCode"></param>
        /// <param name="serviceProvider"></param>
        public bool GetUserPolicyCodes(string UserId, string PolicyCode, ServiceProvider serviceProvider)
        {
            IUserPolicyCodesService userPolicyCodeService = new UserPolicyCodesService(serviceProvider);

            var userPolicyCodeResponse = userPolicyCodeService.AddUserPolicyCode(new UserPolicyCode { UserId = UserId, PolicyCode = PolicyCode });

            //if request successful is false,log error details
            if (!userPolicyCodeResponse.RequestSuccessful)
            {
                loggingService.Fatal(string.Format("Error occurred while processsing UserPolicyCode API, please see log for more detail : {0}", userPolicyCodeResponse));
            }

            return userPolicyCodeResponse.RequestSuccessful;
        }

        #endregion

        #endregion
    }
}
