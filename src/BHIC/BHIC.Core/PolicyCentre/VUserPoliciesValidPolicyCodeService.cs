using BHIC.Common;
using BHIC.Common.Client;
using BHIC.Common.Config;
using BHIC.Contract.PolicyCentre;
using BHIC.Contract.Provider;
using BHIC.Domain.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Core.PolicyCentre
{
    public class VUserPoliciesValidPolicyCodeService : IServiceProviders, IVUserPoliciesValidPolicyCodeService
    {
        public VUserPoliciesValidPolicyCodeService(ServiceProvider provider)
        {
            base.ServiceProvider = provider;
        }
        /// <summary>
        /// Validates list of Policies based on user email and policy code
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public UserPolicyResponse ValidateUserPolicy(UserPolicyRequestParms args)
        {
            var UserPolicyResponse = SvcClient.CallService<UserPolicyResponse>(string.Concat(Constants.ValidateUserPolicy,
                UtilityFunctions.CreateQueryString<UserPolicyRequestParms>(args)), ServiceProvider);

            return UserPolicyResponse;
        }
    }
}
