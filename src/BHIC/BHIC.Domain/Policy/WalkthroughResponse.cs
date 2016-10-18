using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Policy
{
    public class WalkThroughResponse
    {
        // ----------------------------------------
        // constructor
        // ----------------------------------------

        public WalkThroughResponse()
        {
            // init objects to help avoid issues related to null reference exceptions
            WalkThrough = new WalkThrough();
            OperationStatus = new OperationStatus();
        }

        // ----------------------------------------
        // properties
        // ----------------------------------------

        public WalkThrough WalkThrough { get; set; }
        public OperationStatus OperationStatus { get; set; }
    }
}