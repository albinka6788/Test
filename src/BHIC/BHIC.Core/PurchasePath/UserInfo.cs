using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHIC.Contract.PurchasePath;
using BHIC.Domain.Dashboard;
using BHIC.Domain.PurchasePath;
using BHIC.Core.PolicyCentre;
using BHIC.Common.Client;
using BHIC.Contract.Policy;
using BHIC.Domain.Policy;
using BHIC.Core.Policy;

namespace BHIC.Core.PurchasePath
{
    public class UserInfo : IUserInfo
    {
        public UserDetail GetUserInfo(UserRegistration user, ServiceProvider serviceProvider)
        {
            UserDetail userDetail = new UserDetail();
            //Comment: Get the list of policies from Database based on Email Id
            DashboardService dashboardService = new DashboardService();
            var lstPolicies = dashboardService.GetUserPoliciesFromDB(user.Email);
            List<String> lstGuardPolicies = lstPolicies.Where(a => a.LOB == 1 || a.LOB == 2).Select(a => a.PolicyCode).ToList();

            //Comment: Get the policy details from the Guard API for the fetched policies
            if (lstPolicies != null && lstPolicies.Count > 0)
            {
                IPolicyDetailsService policyDetailsService = new PolicyDetailsService(serviceProvider);
                PolicyDetailsResponse policyDetailResponse = policyDetailsService.GetPolicyDetails(new PolicyDetailsRequestParms { UserId = user.Email, PolicyCodeList = string.Join(",", lstGuardPolicies), IncludeRelatedBackEndContacts = true });

                if (!(policyDetailResponse.PolicyDetailsList == null && policyDetailResponse.PolicyDetailsList.Count > 0 && !policyDetailResponse.OperationStatus.RequestSuccessful))
                {
                    List<PolicyInfo> policies = ExtractPolicyInfo(policyDetailResponse.PolicyDetailsList);
                    PolicyInfo preferredPolicy = (from c in policies orderby c.PolicyStatusCode, c.LOB descending, c.EffectiveDate descending select c).FirstOrDefault();
                    if(preferredPolicy!=null && preferredPolicy.BackEndContacts!=null && preferredPolicy.BackEndContacts.Count>0 )
                    { 
                        Domain.BackEnd.Contact userContact=preferredPolicy.BackEndContacts[0];
                        userDetail.EmailID = userContact.EMAIL;
                        userDetail.Phone = userContact.BUSINESS;
                        userDetail.FullName = userContact.NAME;
                    }
                }
            }
            return userDetail;
        }

        private List<PolicyInfo> ExtractPolicyInfo(List<PolicyDetails> list)
        {
            List<PolicyInfo> policies = new List<PolicyInfo>();
            DashboardService dashboardService = new DashboardService();
            for (int i = 0; i < list.Count; i++)
            {
                PolicyDetails policyDetail = list[i];
                PolicyInfo policyInfo = new PolicyInfo();
                policyInfo.LOB = policyDetail.LOB;
                policyInfo.PolicyCode = policyDetail.PolicyCode;
                policyInfo.EffectiveDate = policyDetail.PolicyBegin;
                policyInfo.PolicyStatus = dashboardService.GetPolicyStatus(policyDetail);
                policyInfo.BackEndContacts = policyDetail.BackEndContacts;
                policies.Add(policyInfo);
            }
            return policies;
        }
    }
}