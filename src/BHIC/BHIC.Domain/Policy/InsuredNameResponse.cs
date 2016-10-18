using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Policy
{
	public class InsuredNameResponse
	{
		// ----------------------------------------
		// constructor
		// ----------------------------------------

		public InsuredNameResponse()
		{
			// init objects to help avoid issues related to null reference exceptions
			InsuredNames = new List<InsuredName>();
			OperationStatus = new OperationStatus();
		}

		// ----------------------------------------
		// properties
		// ----------------------------------------

		public List<InsuredName> InsuredNames { get; set; }
		public OperationStatus OperationStatus { get; set; }
	}
}
