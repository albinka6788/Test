using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Policy
{
    public class PolicyResponse
	{
		// ----------------------------------------
		// constructor
		// ----------------------------------------

        public PolicyResponse()
		{
			// init objects to help avoid issues related to null reference exceptions
            Policy = new Policy();
			OperationStatus = new OperationStatus();
		}

		// ----------------------------------------
		// properties
		// ----------------------------------------

        public Policy Policy { get; set; }
		public OperationStatus OperationStatus { get; set; }
	}
}
