using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    public class PCQuoteInformation
    {
        /// <summary>
        /// Quote ID
        /// </summary>
        public int QuoteId { get; set; }

        /// <summary>
        /// Class Description
        /// </summary>
        public string ClassDescription { get; set; }

        /// <summary>
        /// Zip Code
        /// </summary>
        public string ZipCode { get; set; }
    }
}
