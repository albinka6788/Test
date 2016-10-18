using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    /// <summary>
    /// Parameters associated with the LobData service<br />
    /// Filters the response as indicated by the comments for each parameter
    /// </summary>
    public class LobDataRequestParms
    {
        /// <summary>
        /// return data for specified Quote
        /// </summary>
        public int? QuoteId { get; set; }

        /// <summary>
        /// return the specified LobData
        /// </summary>
        public int? LobDataId { get; set; }

        /// <summary>
        /// return data for specified LOB abbreviation (WC, BP, CA...)
        /// </summary>
        [StringLength(2)]
        public string LobAbbr { get; set; }

        /// <summary>
        /// If true, the response will include related child objects (CoverageStates, Exposures, Modifiers)<br />
        /// If false, the response will include only the requested LobData object(s).<br />
        /// </summary>
        public bool IncludeRelated { get; set; }
    }
}
