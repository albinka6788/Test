using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Policy
{
    public class BillingDetailsResponse
    {
        // ----------------------------------------
        // constructor
        // ----------------------------------------

        public BillingDetailsResponse()
        {
            // init objects to help avoid issues related to null reference exceptions
            BillingDetails = new BillingDetails();
            OperationStatus = new OperationStatus();
        }

        // ----------------------------------------
        // properties
        // ----------------------------------------

        public BillingDetails BillingDetails { get; set; }
        public OperationStatus OperationStatus { get; set; }
    }
}