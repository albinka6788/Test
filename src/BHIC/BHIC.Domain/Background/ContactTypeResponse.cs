using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Background
{
	public class ContactTypeResponse
	{
		// ----------------------------------------
		// constructor
		// ----------------------------------------

		public ContactTypeResponse()
		{
			// init objects to help avoid issues related to null reference exceptions
			ContactTypes = new List<ContactType>();
			OperationStatus = new OperationStatus();
		}

		// ----------------------------------------
		// properties
		// ----------------------------------------

		public List<ContactType> ContactTypes { get; set; }
		public OperationStatus OperationStatus { get; set; }
	}
}
