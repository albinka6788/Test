using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Policy
{
	public class BillingStatementResponse
	{
		// ----------------------------------------
		// constructor
		// ----------------------------------------

        public BillingStatementResponse()
		{
			// init objects to help avoid issues related to null reference exceptions
            BillingStatements = new List<Document>();
			OperationStatus = new OperationStatus();
		}

		// ----------------------------------------
		// properties
		// ----------------------------------------

		public List<Document> BillingStatements { get; set; }
		public OperationStatus OperationStatus { get; set; }
	}
}
