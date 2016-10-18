using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    /// <summary>
    /// Parameters associated with the CancellationRequests service<br />
    /// Filters the response as indicated by the comments for each parameter
    /// </summary>
    public class CancellationRequestParms
    {
        ///// <summary>
        ///// return data for specified Quote
        ///// </summary>
        //public int? QuoteId { get; set; }

        /// <summary>
        /// return data for specified Policy
        /// </summary>
        public string PolicyId { get; set; }

        /// <summary>
        /// return data for specified CancellationRequest
        /// </summary>
        public string CancellationRequestId { get; set; }
    }
}
