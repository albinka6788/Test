using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Policy
{
	public class ExposureResponse
	{
		// ----------------------------------------
		// constructor
		// ----------------------------------------

		public ExposureResponse()
		{
			// init objects to help avoid issues related to null reference exceptions
			Exposures = new List<Exposure>();
			OperationStatus = new OperationStatus();
		}

		// ----------------------------------------
		// properties
		// ----------------------------------------

		public List<Exposure> Exposures { get; set; }
		public OperationStatus OperationStatus { get; set; }
	}
}
