using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Policy
{
	public class FutureBillResponse
	{
		// ----------------------------------------
		// constructor
		// ----------------------------------------

        public FutureBillResponse()
		{
			// init objects to help avoid issues related to null reference exceptions
            FutureBills = new List<FutureBill>();
			OperationStatus = new OperationStatus();
		}

		// ----------------------------------------
		// properties
		// ----------------------------------------

        public List<FutureBill> FutureBills { get; set; }
		public OperationStatus OperationStatus { get; set; }
	}
}
