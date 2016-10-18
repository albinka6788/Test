using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Policy
{
	public class SavedQuoteResponse
	{
		// ----------------------------------------
		// constructor
		// ----------------------------------------

		public SavedQuoteResponse()
		{
			// init objects to help avoid issues related to null reference exceptions
			OperationStatus = new OperationStatus();
		}

		// ----------------------------------------
		// properties
		// ----------------------------------------

		/// <summary>
		/// ID of the requested Quote.  Can be used as an input parameter to other services that accept QuoteId.
		/// </summary>
		public int? QuoteId { get; set; }
		public OperationStatus OperationStatus { get; set; }
	}
}
