using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Policy
{
    public class ViewDocumentResponse
    {
        // ----------------------------------------
        // constructor
        // ----------------------------------------

        public ViewDocumentResponse()
        {
            // init objects to help avoid issues related to null reference exceptions
            Document = new ViewDocument();
            OperationStatus = new OperationStatus();
        }

        // ----------------------------------------
        // properties
        // ----------------------------------------

        public ViewDocument Document { get; set; }
        public OperationStatus OperationStatus { get; set; }
    }
}