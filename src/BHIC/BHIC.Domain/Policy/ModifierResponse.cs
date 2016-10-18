using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Policy
{
	public class ModifierResponse
	{
		// ----------------------------------------
		// constructor
		// ----------------------------------------

		public ModifierResponse()
		{
			// init objects to help avoid issues related to null reference exceptions
			Modifiers = new List<Modifier>();
			OperationStatus = new OperationStatus();
		}

		// ----------------------------------------
		// properties
		// ----------------------------------------

		public List<Modifier> Modifiers { get; set; }
		public OperationStatus OperationStatus { get; set; }
	}
}
