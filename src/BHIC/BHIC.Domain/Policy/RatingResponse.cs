#region Using directives

using BHIC.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace BHIC.Domain.Policy
{
    public class RatingResponse
    {
        // ----------------------------------------
        // constructor
        // ----------------------------------------

        public RatingResponse()
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