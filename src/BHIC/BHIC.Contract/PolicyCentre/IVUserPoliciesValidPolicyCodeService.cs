using BHIC.Domain.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Contract.PolicyCentre
{
    public interface IVUserPoliciesValidPolicyCodeService
    {
        UserPolicyResponse ValidateUserPolicy(UserPolicyRequestParms args);
    }
}
