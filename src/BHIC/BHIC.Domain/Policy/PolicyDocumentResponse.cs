using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Policy
{
    public class PolicyDocumentResponse
	{
		// ----------------------------------------
		// constructor
		// ----------------------------------------

        public PolicyDocumentResponse()
		{
			// init objects to help avoid issues related to null reference exceptions
            PolicyDocuments = new List<Document>();
			OperationStatus = new OperationStatus();
		}

		// ----------------------------------------
		// properties
		// ----------------------------------------

        public List<Document> PolicyDocuments { get; set; }
		public OperationStatus OperationStatus { get; set; }
	}
}
