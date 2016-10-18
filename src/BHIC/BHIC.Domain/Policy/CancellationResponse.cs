using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Policy
{
	public class CancellationRequestResponse
	{
		// ----------------------------------------
		// constructor
		// ----------------------------------------

		public CancellationRequestResponse()
		{
			// init objects to help avoid issues related to null reference exceptions
			CancellationRequests = new List<CancellationRequest>();
			OperationStatus = new OperationStatus();
		}

		// ----------------------------------------
		// properties
		// ----------------------------------------

		public List<CancellationRequest> CancellationRequests { get; set; }
		public OperationStatus OperationStatus { get; set; }
	}
}
