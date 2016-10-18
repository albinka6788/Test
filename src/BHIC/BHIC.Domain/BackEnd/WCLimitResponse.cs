using BHIC.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.BackEnd
{
    public class WCLimitsListResponse
    {
        // ----------------------------------------
        // constructor
        // ----------------------------------------

        public WCLimitsListResponse()
        {
            // init objects to help avoid issues related to null reference exceptions
            WcLimits = new List<WCLimit>();
            OperationStatus = new OperationStatus();
        }

        // ----------------------------------------
        // properties
        // ----------------------------------------

        public List<WCLimit> WcLimits { get; set; }
        public OperationStatus OperationStatus { get; set; }
    }
}