#region Using directives

using System;

#endregion

namespace BHIC.Domain.Policy
{
    /// <summary>
    /// Parameters associated with the ProspectInfo service<br />
    /// Filters the response as indicated by the comments for each parameter
    /// </summary>
    public class ProspectInfoRequestParms
    {
        /// <summary>
        /// return ProspectInfo rows for specified Quote
        /// </summary>
        public int? QuoteId { get; set; }

        /// <summary>
        /// return data for specified ProspectInfo
        /// </summary>
        public int? ProspectInfoId { get; set; }

    }
}
