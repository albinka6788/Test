using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Service
{
	public class BatchAction
	{
		// ----------------------------------------
		// constructor
		// ----------------------------------------

		public BatchAction()
		{
			// init lists to help avoid issues related to null reference exceptions
			BatchActionParameters = new List<BatchActionParameter>();
		}

		// ----------------------------------------
		// properties
		// ----------------------------------------

		/// <summary>
		/// name of Web API Service to execute (see Web API documentation)
		/// </summary>

		public string ServiceName { get; set; }

		/// <summary>
		/// name of specific method on the above Web API Service to execute (e.g. - GET, PUT, POST, DELETE)
		/// </summary>
		public string ServiceMethod { get; set; }

		/// <summary>
		/// name of specific method on the above Web API Service to execute (e.g. - GET, PUT, POST, DELETE)
		/// </summary>
		public List<BatchActionParameter> BatchActionParameters { get; set; }

		/// <summary>
		/// value set by and returned to the client of the Insurance Service; can be used to identify which BatchResponse is associated with the request in the BatchActionList.  (e.g. - identify which BatchResponse is an Industry list, which is Quote data, which is Payment Plan data, etc...)
		/// </summary>
		public string RequestIdentifier { get; set; }						
	}
}
