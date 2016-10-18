using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    /// <summary>
    /// Parameters used to retrieve the QuoteId associated with a 'Saved Quote' that was previously emailed to an end user, as a result of the user clicking a 'Save for Later' button.
    /// </summary>
    public class SavedQuoteRequestParms
    {
        /// <summary>
        /// GUID embedded in the link forwarded via email to the end user.<br />
        /// Allows the system to identify the associated Quote<br />
        /// The UI will need to be able to process a request that contains this GUID, as described in documentation for the SavedQuote DTO (See SavedQuotes POST help > SavedQuote DTO > ReturnToUrl parameter).<br />
        /// </summary>
        [StringLength(100)]
        public string ReturnToQuoteGUID { get; set; }
    }
}
