using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Policy
{
	public class ContactResponse
	{
		// ----------------------------------------
		// constructor
		// ----------------------------------------

		public ContactResponse()
		{
			// init objects to help avoid issues related to null reference exceptions
			Contacts = new List<Contact>();
			OperationStatus = new OperationStatus();
		}

		// ----------------------------------------
		// properties
		// ----------------------------------------

		public List<Contact> Contacts { get; set; }
		public OperationStatus OperationStatus { get; set; }
	}
}
