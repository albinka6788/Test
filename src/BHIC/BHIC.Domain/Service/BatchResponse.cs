using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Service
{
	public class BatchResponse
	{
		// ----------------------------------------
		// constructor
		// ----------------------------------------

		public BatchResponse()
		{
			RequestSuccessful = true;
		}

		// ----------------------------------------
		// properties
		// ----------------------------------------

		/// <summary>
		/// JSON reponse, representing arbitrary data type (Industry List, Quote Data, Payment Plan, etc...)
		/// </summary>
		public string JsonResponse { get; set; }

		/// <summary>
		/// Arbitrary value set by and returned to the client; can be used to identify a specific BatchResponse within the BatchResponseList.
		/// </summary>
 
		public string RequestIdentifier { get; set; }

		/// <summary>
		/// True if the action associated with the response was successful; otherwise false<br />
		/// If false, the response can be parsed to get the OperationStatus and error messages associated with the request.
		/// </summary>
		public bool RequestSuccessful { get; set; }

	}
}
