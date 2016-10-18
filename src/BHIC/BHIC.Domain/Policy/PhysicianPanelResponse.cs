using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Policy
{
    public class PhysicianPanelResponse
	{
		// ----------------------------------------
		// constructor
		// ----------------------------------------

        public PhysicianPanelResponse()
		{
			// init objects to help avoid issues related to null reference exceptions
            PhysicianPanels = new List<Document>();
			OperationStatus = new OperationStatus();
		}

		// ----------------------------------------
		// properties
		// ----------------------------------------

        public List<Document> PhysicianPanels { get; set; }
		public OperationStatus OperationStatus { get; set; }
	}
}
