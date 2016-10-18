using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Policy
{
	public class RatingDataResponse
	{
		// ----------------------------------------
		// constructor
		// ----------------------------------------

		public RatingDataResponse()
		{
			// init objects to help avoid issues related to null reference exceptions
			RatingData = new RatingData();
			OperationStatus = new OperationStatus();
		}

		// ----------------------------------------
		// properties
		// ----------------------------------------

		public RatingData RatingData { get; set; }
		public OperationStatus OperationStatus { get; set; }
	}
}
