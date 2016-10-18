using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Policy
{
	public class CertRequestResponse
	{
		// ----------------------------------------
		// constructor
		// ----------------------------------------

		public CertRequestResponse()
		{
			// init objects to help avoid issues related to null reference exceptions
			CertRequests = new List<CertRequest>();
			OperationStatus = new OperationStatus();
		}

		// ----------------------------------------
		// properties
		// ----------------------------------------

		public List<CertRequest> CertRequests { get; set; }
		public OperationStatus OperationStatus { get; set; }
	}
}
