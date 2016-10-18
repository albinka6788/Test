using BHIC.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    public class UserPolicyCodeResponse
    {
        // ----------------------------------------
		// constructor
		// ----------------------------------------

        public UserPolicyCodeResponse()
		{
			// init objects to help avoid issues related to null reference exceptions
            UserPolicyCodes = new List<UserPolicyCode>();
			OperationStatus = new OperationStatus();
		}

		// ----------------------------------------
		// properties
		// ----------------------------------------
        public List<UserPolicyCode> UserPolicyCodes { get; set; }
        public OperationStatus OperationStatus { get; set; }
    }
}
