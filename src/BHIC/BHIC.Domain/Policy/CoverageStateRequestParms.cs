using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    /// <summary>
    /// Parameters associated with the CoverageStates service<br />
    /// Filters the response as indicated by the comments for each parameter
    /// </summary>
    public class CoverageStateRequestParms
    {
        /// <summary>
        /// return data for specified Quote
        /// </summary>
        public int? QuoteId { get; set; }

        /// <summary>
        /// return data for specified LobData
        /// </summary>
        public int? LobDataId { get; set; }

        /// <summary>
        /// return the specific item
        /// </summary>
        public int? CoverageStateId { get; set; }

        /// <summary>
        /// If true, the response will include related child objects (Exposures, Modifiers)<br />
        /// If false, the response will include only the requested CoverageState object(s).<br />
        /// </summary>
        public bool IncludeRelated { get; set; }

    }
}
