using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Policy
{
	public class PaymentTermsResponse
	{
		// ----------------------------------------
		// constructor
		// ----------------------------------------

		public PaymentTermsResponse()
		{
			// init objects to help avoid issues related to null reference exceptions
			PaymentTerms = new PaymentTerms();
			OperationStatus = new OperationStatus();
		}

		// ----------------------------------------
		// properties
		// ----------------------------------------

		public PaymentTerms PaymentTerms { get; set; }
		public OperationStatus OperationStatus { get; set; }
	}
}
