#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace BHIC.Domain.Policy
{
    /// <summary>
    /// Parameters associated with the Rating service<br />
    /// Filters the rating action as indicated by the comments for each parameter
    /// </summary>
    public class RatingRequestParms
    {
        /// <summary>
        /// Rate the specified Quote
        /// </summary>
        public int? QuoteId { get; set; }
    }
}
