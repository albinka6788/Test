using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHIC.Domain.Service;
using BHIC.Common.Client;
using BHIC.Domain.Dashboard;
using BHIC.Domain.Policy;

namespace BHIC.Contract.PolicyCentre
{
    public interface IUserPolicyDataService
    {
        /// <summary>
        /// Return PolicyData object based on QuoteId
        /// </summary>
        /// <returns></returns>
        //PolicyDetailsRequestParms GetUserPolicyData(PolicyDetailsRequestParms args);
        //void GetUserPolicyData(UserPolicyDataReqParms args);

        PolicyDetailsResponse GetPolicyDetails(PolicyDetailsRequestParms args, ServiceProvider provider);
        List<PolicyDetailsResponse> GetPolicyList(List<string> policies, string userID, ServiceProvider provider);

    }
}
