using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Background
{
	public class CountyResponse
	{
		// ----------------------------------------
		// constructor
		// ----------------------------------------

		public CountyResponse()
		{
			// init objects to help avoid issues related to null reference exceptions
			Counties = new List<County>();
			OperationStatus = new OperationStatus();
		}

		// ----------------------------------------
		// properties
		// ----------------------------------------

		public List<County> Counties { get; set; }
		public OperationStatus OperationStatus { get; set; }
	}
}
