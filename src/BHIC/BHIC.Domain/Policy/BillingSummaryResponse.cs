using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Policy
{
    public class BillingSummaryResponse
    {
        // ----------------------------------------
        // constructor
        // ----------------------------------------

        public BillingSummaryResponse()
        {
            // init objects to help avoid issues related to null reference exceptions
            BillingSummary = new BillingSummary();
            OperationStatus = new OperationStatus();
        }

        // ----------------------------------------
        // properties
        // ----------------------------------------

        public BillingSummary BillingSummary { get; set; }
        public OperationStatus OperationStatus { get; set; }
    }
}