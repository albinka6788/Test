using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Policy
{
	public class QuoteStatusResponse
	{
		// ----------------------------------------
		// constructor
		// ----------------------------------------

		public QuoteStatusResponse()
		{
			// init objects to help avoid issues related to null reference exceptions
			QuoteStatus = new QuoteStatus();
			OperationStatus = new OperationStatus();
		}

		// ----------------------------------------
		// properties
		// ----------------------------------------

		public QuoteStatus QuoteStatus { get; set; }
		public OperationStatus OperationStatus { get; set; }
	}
}
