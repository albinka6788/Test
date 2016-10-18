using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Policy
{
    public class QuoteResponse
    {
        // ----------------------------------------
        // constructor
        // ----------------------------------------

        public QuoteResponse()
        {
            // init objects to help avoid issues related to null reference exceptions
            Quote = new Quote();
            OperationStatus = new OperationStatus();
            Quotes = new List<Quote>();
        }

        // ----------------------------------------
        // properties
        // ----------------------------------------

        /// <summary>
        /// Only populated if a single QuoteId was specified by populating one of the following two request properties: QuoteRequestParms.QuoteId, or QuoteRequestParms.PolicyId
        /// </summary>
        public Quote Quote { get; set; }

        /// <summary>
        /// Only populated if a comma-delimited list of QuoteIds was specified by populating the following request property: QuoteRequestParms.QuoteIdList
        /// </summary>
        public List<Quote> Quotes { get; set; }

        public OperationStatus OperationStatus { get; set; }
    }
}
