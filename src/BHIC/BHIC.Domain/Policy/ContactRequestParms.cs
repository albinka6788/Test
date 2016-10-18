using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    /// <summary>
    /// Parameters associated with the Contacts service<br />
    /// Filters the response as indicated by the comments for each parameter
    /// </summary>
    public class ContactRequestParms
    {
        /// <summary>
        /// return contacts for specified quote
        /// </summary>
        public int? QuoteId { get; set; }

        /// <summary>
        /// return data for specified contact
        /// </summary>
        public int? ContactId { get; set; }

        /// <summary>
        /// If true, the response will include related child objects (Phones, Addresses)<br />
        /// If false, the response will include only the requested Contact object(s).<br />
        /// </summary>
        public bool IncludeRelated { get; set; }
    }
}
