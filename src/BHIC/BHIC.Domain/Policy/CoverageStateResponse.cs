using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Policy
{
	public class CoverageStateResponse
	{
		// ----------------------------------------
		// constructor
		// ----------------------------------------

		public CoverageStateResponse()
		{
			// init objects to help avoid issues related to null reference exceptions
			CoverageStates = new List<CoverageState>();
			OperationStatus = new OperationStatus();
		}

		// ----------------------------------------
		// properties
		// ----------------------------------------

		public List<CoverageState> CoverageStates { get; set; }
		public OperationStatus OperationStatus { get; set; }
	}
}
