using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Policy
{
	public class PaymentResponse
	{
		// ----------------------------------------
		// constructor
		// ----------------------------------------

		public PaymentResponse()
		{
			// init objects to help avoid issues related to null reference exceptions
			Payments = new List<Payment>();
			OperationStatus = new OperationStatus();
		}

		// ----------------------------------------
		// properties
		// ----------------------------------------

		public List<Payment> Payments { get; set; }
		public OperationStatus OperationStatus { get; set; }
	}
}
