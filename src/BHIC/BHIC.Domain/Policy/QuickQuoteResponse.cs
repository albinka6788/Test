using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Policy
{
    public class QuickQuoteResponse
    {
        // ----------------------------------------
        // constructor
        // ----------------------------------------

        public QuickQuoteResponse()
        {
            // init objects to help avoid issues related to null reference exceptions
            Premium = 0;
            InitialCarrier = "";
            OperationStatus = new OperationStatus();
        }

        // ----------------------------------------
        // properties
        // ----------------------------------------

        public decimal Premium { get; set; }
        public string InitialCarrier { get; set; }
        public OperationStatus OperationStatus { get; set; }
    }
}
