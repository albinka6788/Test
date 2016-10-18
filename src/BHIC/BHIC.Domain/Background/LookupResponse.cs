using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Background
{
	public class LookupResponse
	{
		// ----------------------------------------
		// constructor
		// ----------------------------------------

        public LookupResponse()
		{
			// init objects to help avoid issues related to null reference exceptions
            Lookups = new List<Lookup>();
			OperationStatus = new OperationStatus();
		}

		// ----------------------------------------
		// properties
		// ----------------------------------------

        public List<Lookup> Lookups { get; set; }
		public OperationStatus OperationStatus { get; set; }
	}
}
