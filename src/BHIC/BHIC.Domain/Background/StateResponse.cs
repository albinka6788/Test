using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Background
{
	public class StateResponse
	{
		// ----------------------------------------
		// constructor
		// ----------------------------------------

		public StateResponse()
		{
			// init objects to help avoid issues related to null reference exceptions
			States = new List<State>();
			OperationStatus = new OperationStatus();
		}

		// ----------------------------------------
		// properties
		// ----------------------------------------

		public List<State> States { get; set; }
		public OperationStatus OperationStatus { get; set; }
	}
}
