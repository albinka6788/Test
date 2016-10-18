using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Policy
{
	public class PolicyDataResponse
	{
		// ----------------------------------------
		// constructor
		// ----------------------------------------

		public PolicyDataResponse()
		{
			// init objects to help avoid issues related to null reference exceptions
			PolicyData = new PolicyData();
			OperationStatus = new OperationStatus();
		}

		// ----------------------------------------
		// properties
		// ----------------------------------------

		public PolicyData PolicyData { get; set; }
		public OperationStatus OperationStatus { get; set; }
	}
}
