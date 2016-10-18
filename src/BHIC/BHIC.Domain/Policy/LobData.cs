using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    /// <summary>
    /// Provide access to LOB type (WC, BP, CA...), CoverageStates<br />
    /// </summary>
    public class LobData
    {
        // ----------------------------------------
        // constructor
        // ----------------------------------------

        public LobData()
        {
            // init lists to help avoid issues related to null reference exceptions
            CoverageStates = new List<CoverageState>();
        }

        // ----------------------------------------
        // properties
        // ----------------------------------------

        public int? LobDataId { get; set; }

        [Required]
        public int? QuoteId { get; set; }

        /// <summary>
        /// Identifier that specifies the Line of Business associated with the operation<br />
        /// Examples:<br />
        /// WC = Workers' Comp<br />
        /// BP = Businessowner's Policy<br />
        /// CA = Commercial Auto<br />
        /// </summary>
        [Required]
        [StringLength(2)]
        public string Lob { get; set; }								// from spec: {ie. WC, BP, CA, etc.}

        /// <summary>
        /// Read-only friendly name for the Line of Business. <br />
        /// Example:  "Workers' Compensation"
        /// </summary>
        public string LobFriendlyName { get; set; }

        /// <summary>
        /// List of associated CoverageStates
        /// </summary>
        public List<CoverageState> CoverageStates { get; set; }
    }
}
