using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Background
{
	public class CompanionClassResponse
	{
		// ----------------------------------------
		// constructor
		// ----------------------------------------

		public CompanionClassResponse()
		{
			// init objects to help avoid issues related to null reference exceptions
			CompanionClasses = new List<CompanionClass>();
			OperationStatus = new OperationStatus();
		}

		// ----------------------------------------
		// properties
		// ----------------------------------------

		public List<CompanionClass> CompanionClasses { get; set; }
		public OperationStatus OperationStatus { get; set; }
	}
}
