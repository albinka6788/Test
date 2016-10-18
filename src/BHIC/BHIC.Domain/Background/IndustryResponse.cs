using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Background
{
	public class IndustryResponse
	{
		// ----------------------------------------
		// constructor
		// ----------------------------------------
		
		public IndustryResponse()
		{
			// init objects to help avoid issues related to null reference exceptions
			Industries = new List<Industry>();
			OperationStatus = new OperationStatus();
		}

		// ----------------------------------------
		// properties
		// ----------------------------------------
		
		public List<Industry> Industries { get; set; }
		public OperationStatus OperationStatus { get; set; }
	}
}
