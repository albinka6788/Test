using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Background
{
	public class PhoneTypeResponse
	{
		// ----------------------------------------
		// constructor
		// ----------------------------------------

		public PhoneTypeResponse()
		{
			// init objects to help avoid issues related to null reference exceptions
			PhoneTypes = new List<PhoneType>();
			OperationStatus = new OperationStatus();
		}

		// ----------------------------------------
		// properties
		// ----------------------------------------

		public List<PhoneType> PhoneTypes { get; set; }
		public OperationStatus OperationStatus { get; set; }
	}
}
