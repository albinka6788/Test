using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Policy
{
	public class BillingResponse
	{
		// ----------------------------------------
		// constructor
		// ----------------------------------------

        public BillingResponse()
		{
			// init objects to help avoid issues related to null reference exceptions
            Billing = new Billing();
			OperationStatus = new OperationStatus();
		}

		// ----------------------------------------
		// properties
		// ----------------------------------------

        public Billing Billing { get; set; }
		public OperationStatus OperationStatus { get; set; }
	}
}
