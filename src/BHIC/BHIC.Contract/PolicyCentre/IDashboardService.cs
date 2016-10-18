using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHIC.Domain.Background;
using BHIC.Domain.Dashboard;
using BHIC.Domain.Policy;
using BHIC.Domain.PolicyCentre;

namespace BHIC.Contract.PolicyCentre
{
    public interface IDashboardService
    {
        string GetPolicyIDFromDB(string email);
        List<string> GetPoliciesFromDB(string email);
        List<PolicyInformation> GetUserPoliciesFromDB(string email);
        string GetPolicyStatus(PolicyDetails policyDetails);
        UserRegistration GetCredentials(UserRegistration user);
        List<DropDownOptions> GetDropdownOptions(int lineOfBusinessID, string TableKey);
        DropDownOptions GetDropdownOptions(int lineOfBusinessID, string TableKey, int Id);
    }
}
