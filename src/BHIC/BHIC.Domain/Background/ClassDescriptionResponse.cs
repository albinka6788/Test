using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Background
{
	public class ClassDescriptionResponse
	{
		// ----------------------------------------
		// constructor
		// ----------------------------------------

		public ClassDescriptionResponse()
		{
			// init objects to help avoid issues related to null reference exceptions
			ClassDescriptions = new List<ClassDescription>();
			OperationStatus = new OperationStatus();
		}

		// ----------------------------------------
		// properties
		// ----------------------------------------

		public List<ClassDescription> ClassDescriptions { get; set; }
		public OperationStatus OperationStatus { get; set; }
	}
}
