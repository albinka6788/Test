using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Policy
{
    public class VExposuresMinPayrollResponse
    {
        // ----------------------------------------
        // constructor
        // ----------------------------------------

        public VExposuresMinPayrollResponse()
        {
            // init objects to help avoid issues related to null reference exceptions
            OperationStatus = new OperationStatus();
        }

        // ----------------------------------------
        // properties
        // ----------------------------------------

        public OperationStatus OperationStatus { get; set; }
        public decimal MinimumExposure { get; set; }
    }
}
