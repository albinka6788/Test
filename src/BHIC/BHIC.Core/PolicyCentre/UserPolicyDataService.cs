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
using BHIC.Contract.PolicyCentre;
using BHIC.Domain.Dashboard;
using Newtonsoft.Json;

namespace BHIC.Core.PolicyCentre
{
    public class UserPolicyDataService : IServiceProviders, IUserPolicyDataService
    {
        public UserPolicyDataService(ServiceProvider provider)
        {
            base.ServiceProvider = provider;
        }
        public PolicyDetailsResponse GetPolicyDetails(PolicyDetailsRequestParms args, ServiceProvider provider)
        {
            PolicyDetailsResponse policyDetailResponse;

            policyDetailResponse = SvcClient.CallService<PolicyDetailsResponse>(string.Concat(Constants.PolicyDetails,
               UtilityFunctions.CreateQueryString<PolicyDetailsRequestParms>(args)), provider);

            if (policyDetailResponse.OperationStatus.RequestSuccessful)
            {
                return policyDetailResponse;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(policyDetailResponse.OperationStatus.Messages));
            }

        }

        public List<PolicyDetailsResponse> GetPolicyList(List<string> policies, string userID, ServiceProvider provider)
        {
            List<PolicyDetailsResponse> policyDetailsResponse = null;
            #region Comment : Here create batch actions

            /*----------------------------------------
                    populate a BatchActionList that contains all requests to be sent to the Gaurd API
                    ----------------------------------------*/

            var batchActionList = new BatchActionList();	// BatchActionList: list of actions routed to the Insurance Service for response
            BatchResponseList batchResponseList;			// BatchResponseList: list of responses returned from the Insurance Service
            string jsonResponse;							// JSON-formated response data returned from the Insurance Service

            // populate a BatchAction that will be used to submit the InsuredName DTO to the Insurance Service

            foreach (var item in policies)
            {
                var policyDetailsAction = new BatchAction { ServiceName = Constants.PolicyDetails, ServiceMethod = "GET", RequestIdentifier = "Get Policy Details " + item };
                policyDetailsAction.BatchActionParameters.Add(new BatchActionParameter { Name = "PolicyDetailsRequestParms", Value = JsonConvert.SerializeObject(new PolicyDetailsRequestParms { PolicyCode = item, UserId = userID }) });
                batchActionList.BatchActions.Add(policyDetailsAction);
            }


            #endregion

            // submit the BatchActionList to the Insurance Service
            jsonResponse = SvcClient.CallServiceBatch(batchActionList, ServiceProvider);

            // deserialize the results into a BatchResponseList
            batchResponseList = JsonConvert.DeserializeObject<BatchResponseList>(jsonResponse);

            // deserialize each response returned by the service, and copy the OperationStatus to the view model for demo purposes
            OperationStatus operationStatus = new OperationStatus();
            if (batchResponseList != null)
            {
                policyDetailsResponse = new List<PolicyDetailsResponse>();
                foreach (var item in policies)
                {
                    var policyDataResponse = batchResponseList.BatchResponses.SingleOrDefault(m => m.RequestIdentifier == "Get Policy Details " + item).JsonResponse;
                    var policyResponse = JsonConvert.DeserializeObject<PolicyDetailsResponse>(policyDataResponse);
                    policyDetailsResponse.Add(policyResponse);
                }
            }

            return policyDetailsResponse;
        }

    }
}