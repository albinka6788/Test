using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Policy
{
	public class OfficerResponse
	{
		// ----------------------------------------
		// constructor
		// ----------------------------------------

		public OfficerResponse()
		{
			// init objects to help avoid issues related to null reference exceptions
			Officers = new List<Officer>();
			OperationStatus = new OperationStatus();
		}

		// ----------------------------------------
		// properties
		// ----------------------------------------

		public List<Officer> Officers { get; set; }
		public OperationStatus OperationStatus { get; set; }
	}
}
