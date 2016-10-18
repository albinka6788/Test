using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.BackEnd
{
    public class WCLimitsListRequestParms
    {
        /// <summary>
        /// QuoteID to return the limits for.<br />
        /// Required.<br />
        /// Validation:<br />
        /// - Because limits definitions are state-specific, at least one valid CoverageState must exist for the Quote prior to calling this service.<br />
        /// - Because limits definitions are specific to policy inception date, PolicyData POST must be successfully executed to set InceptionDate, prior to calling this service.<br />
        /// - Because limits definitions are carrier-specific, Questions POST must be successfully executed to set Carrier, prior to calling this service.<br />
        /// </summary>
        [Required]
        public int? QuoteId { get; set; }

        /// <summary>
        /// If true, returns a single default Limits definition row.<br />
        /// Default behavior if not specified: returns all Limits definitions rows.
        /// </summary>
        public bool? DefaultLimitsOnly { get; set; }
    }
}