using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Policy
{
	public class AddressResponse
	{
		// ----------------------------------------
		// constructor
		// ----------------------------------------

		public AddressResponse()
		{
			// init objects to help avoid issues related to null reference exceptions
			Addresses = new List<Address>();
			OperationStatus = new OperationStatus();
		}

		// ----------------------------------------
		// properties
		// ----------------------------------------

		public List<Address> Addresses { get; set; }
		public OperationStatus OperationStatus { get; set; }
	}
}
