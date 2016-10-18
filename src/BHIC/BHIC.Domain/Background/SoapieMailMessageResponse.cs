using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Background
{
    public class SoapieMailMessageResponse
    {
        // ----------------------------------------
        // constructor
        // ----------------------------------------

        public SoapieMailMessageResponse()
        {
            // init objects to help avoid issues related to null reference exceptions
            SoapieMailMessage = new SoapieMailMessageRequestParms();
            OperationStatus = new OperationStatus();
        }

        // ----------------------------------------
        // properties
        // ----------------------------------------

        public SoapieMailMessageRequestParms SoapieMailMessage { get; set; }
        public OperationStatus OperationStatus { get; set; }
    }
}
