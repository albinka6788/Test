using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Background
{
	public class ClassCodeResponse
	{
		// ----------------------------------------
		// constructor
		// ----------------------------------------

		public ClassCodeResponse()
		{
			// init objects to help avoid issues related to null reference exceptions
			ClassCodes = new List<ClassCode>();
			OperationStatus = new OperationStatus();
		}

		// ----------------------------------------
		// properties
		// ----------------------------------------

		public List<ClassCode> ClassCodes { get; set; }
		public OperationStatus OperationStatus { get; set; }
	}
}
