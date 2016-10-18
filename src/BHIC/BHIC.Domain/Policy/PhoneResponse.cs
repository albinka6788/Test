using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Policy
{
	public class PhoneResponse
	{
		// ----------------------------------------
		// constructor
		// ----------------------------------------

		public PhoneResponse()
		{
			// init objects to help avoid issues related to null reference exceptions
			Phones = new List<Phone>();
			OperationStatus = new OperationStatus();
		}

		// ----------------------------------------
		// properties
		// ----------------------------------------

		public List<Phone> Phones { get; set; }
		public OperationStatus OperationStatus { get; set; }
	}
}
