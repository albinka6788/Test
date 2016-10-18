using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Background
{
	public class PaymentPlanResponse
	{
		// ----------------------------------------
		// constructor
		// ----------------------------------------

		public PaymentPlanResponse()
		{
			// init objects to help avoid issues related to null reference exceptions
			PaymentPlans = new List<PaymentPlan>();
			OperationStatus = new OperationStatus();
		}

		// ----------------------------------------
		// properties
		// ----------------------------------------

		public List<PaymentPlan> PaymentPlans { get; set; }
		public OperationStatus OperationStatus { get; set; }
	}
}
