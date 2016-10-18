using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    /// <summary>
    /// Parameters associated with the Modifiers service<br />
    /// Filters the response as indicated by the comments for each parameter
    /// </summary>
    public class ModifierRequestParms
    {
        /// <summary>
        /// return data for specified quote
        /// </summary>
        public int? QuoteId { get; set; }

        /// <summary>
        /// return data for specified Lob (WC, BP, CA....)
        /// </summary>
        [StringLength(2)]
        public string Lob { get; set; }

        /// <summary>
        /// Return Modifiers under the specified CoverageState, within the object hierarchy<br />
        /// </summary>
        public int? CoverageStateId { get; set; }

        /// <summary>
        /// return data for specified state
        /// </summary>
        [StringLength(2)]
        public string State { get; set; }

        /// <summary>
        /// return specified exposure
        /// </summary>
        public int? ModifierId { get; set; }

    }
}
