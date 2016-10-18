using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Policy
{
    public class VInsuredNameFEINResponse
    {
        // ----------------------------------------
        // constructor
        // ----------------------------------------

        public VInsuredNameFEINResponse()
        {
            // init objects to help avoid issues related to null reference exceptions
            OperationStatus = new OperationStatus();
        }

        // ----------------------------------------
        // properties
        // ----------------------------------------

        public OperationStatus OperationStatus { get; set; }
    }
}
