using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    /// <summary>
    /// Parameters associated with the PolicyData service<br />
    /// Filters the response as indicated by the comments for each parameter
    /// </summary>
    public class PolicyDataRequestParms
    {
        /// <summary>
        /// Return data for specified Quote<br />
        /// Validation: QuoteId, MgaCode, or PolicyDataId must be provided.
        /// </summary>
        public int? QuoteId { get; set; }

        /// <summary>
        /// Return data for specified MgaCode (Policy Number)<br />
        /// Validation: QuoteId, MgaCode, or PolicyDataId must be provided.
        /// </summary>
        public string MgaCode { get; set; }

        /// <summary>
        /// Return data for specified PolicyData<br />
        /// Validation: QuoteId, MgaCode, or PolicyDataId must be provided.
        /// </summary>
        public int? PolicyDataId { get; set; }

    }
}
