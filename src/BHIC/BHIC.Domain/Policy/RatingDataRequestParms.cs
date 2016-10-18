using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    /// <summary>
    /// Parameters associated with the RatingData service<br />
    /// Filters the response as indicated by the comments for each parameter
    /// </summary>
    public class RatingDataRequestParms
    {
        /// <summary>
        /// return data for specified quote
        /// </summary>
        public int? QuoteId { get; set; }


        /// <summary>
        /// return data for specified policy code (only used for BP)
        /// </summary>
        public string PolicyCode { get; set; }
    }
}
