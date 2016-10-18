using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Policy
{
	public class LocationResponse
	{
		// ----------------------------------------
		// constructor
		// ----------------------------------------

		public LocationResponse()
		{
			// init objects to help avoid issues related to null reference exceptions
			Locations = new List<Location>();
			OperationStatus = new OperationStatus();
		}

		// ----------------------------------------
		// properties
		// ----------------------------------------

		public List<Location> Locations { get; set; }
		public OperationStatus OperationStatus { get; set; }
	}
}
