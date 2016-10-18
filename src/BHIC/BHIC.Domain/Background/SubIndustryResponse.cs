using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Background
{
	public class SubIndustryResponse
	{
		// ----------------------------------------
		// constructor
		// ----------------------------------------
		
		public SubIndustryResponse()
		{
			// init objects to help avoid issues related to null reference exceptions
			SubIndustries = new List<SubIndustry>();
			OperationStatus = new OperationStatus();
		}

		// ----------------------------------------
		// properties
		// ----------------------------------------
		
		public List<SubIndustry> SubIndustries { get; set; }
		public OperationStatus OperationStatus { get; set; }
	}
}
