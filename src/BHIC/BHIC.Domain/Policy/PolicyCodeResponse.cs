using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Policy
{
    public class PolicyCodeResponse
    {
        // ----------------------------------------
        // constructor
        // ----------------------------------------

        public PolicyCodeResponse()
        {
            // init objects to help avoid issues related to null reference exceptions
            OperationStatus = new OperationStatus();
            PolicyCodeRequest = new PolicyCodeRequest();
        }

        // ----------------------------------------
        // properties
        // ----------------------------------------
        public OperationStatus OperationStatus { get; set; }
        public PolicyCodeRequest PolicyCodeRequest { get; set; }
    }
}
