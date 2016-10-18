using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    /// <summary>
    /// Parameters associated with the WalkThrough service (BOP)<br />
    /// Filters the response as indicated by the comments for each parameter
    /// </summary>
    public class WalkThroughRequestParms
    {
        /// <summary>
        /// return Addresses for the specified Quote
        /// </summary>
        public int? QuoteId { get; set; }

        /// <summary>
        /// temporary property only used for now in testing
        /// </summary>
        public string mgacode { get; set; }

        /// <summary>
        /// State
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// ZipCode
        /// </summary>
        public string ZipCode { get; set; }
    }
}
