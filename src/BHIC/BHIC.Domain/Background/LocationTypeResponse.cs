using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Background
{
	public class LocationTypeResponse
	{
		// ----------------------------------------
		// constructor
		// ----------------------------------------

		public LocationTypeResponse()
		{
			// init objects to help avoid issues related to null reference exceptions
			LocationTypes = new List<LocationType>();
			OperationStatus = new OperationStatus();
		}

		// ----------------------------------------
		// properties
		// ----------------------------------------

		public List<LocationType> LocationTypes { get; set; }
		public OperationStatus OperationStatus { get; set; }
	}
}
