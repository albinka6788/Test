using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Policy
{
	public class DeductibleInvoiceResponse
	{
		// ----------------------------------------
		// constructor
		// ----------------------------------------

        public DeductibleInvoiceResponse()
		{
			// init objects to help avoid issues related to null reference exceptions
            DeductibleInvoices = new List<Document>();
			OperationStatus = new OperationStatus();
		}

		// ----------------------------------------
		// properties
		// ----------------------------------------

        public List<Document> DeductibleInvoices { get; set; }
		public OperationStatus OperationStatus { get; set; }
	}
}
