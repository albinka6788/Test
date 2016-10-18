using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    /// <summary>
    /// Parameters associated with the PCQuoteInformation service<br />
    /// Filters the response as indicated by the comments for each parameter
    /// </summary>
    public class PCQuoteInformationRequestParms
    {
        /// <summary>
        /// Return data for specified Quote<br />
        /// Validation: <br />
        /// 1) One of the following must be provided: QuoteId, QuoteIdList, or MgaCode.
        /// </summary>
        public int? QuoteId { get; set; }

        /// <summary>
        /// Return data for specified Quotes.<br />
        /// An arbitrary number of Quote IDs may be provided.<br />
        /// Example: 111,222,333,444
        /// Validation: <br />
        /// 1) One of the following must be provided: QuoteId, QuoteIdList, or MgaCode.
        /// </summary>
        public string QuoteIdList { get; set; }

        /// <summary>
        /// Return data for specified MgaCode (Policy Number)<br />
        /// Validation: <br />
        /// 1) One of the following must be provided: QuoteId, QuoteIdList, or MgaCode.
        /// </summary>
        public string MgaCode { get; set; }
    }
}
