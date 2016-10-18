using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Account
{
    public class UserPolicyResponse
    {
        // ----------------------------------------
        // constructor
        // ----------------------------------------

        public UserPolicyResponse()
        {
            // init objects to help avoid issues related to null reference exceptions
            UserPolicies = new List<UserPolicy>();
            OperationStatus = new OperationStatus();
        }

        // ----------------------------------------
        // properties
        // ----------------------------------------

        public List<UserPolicy> UserPolicies { get; set; }
        public OperationStatus OperationStatus { get; set; }
    }
}
