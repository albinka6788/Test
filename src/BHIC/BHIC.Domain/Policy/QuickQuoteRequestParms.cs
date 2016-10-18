using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    /// <summary>
    /// Parameters associated with the QuickQuote service<br />
    /// Filters the response as indicated by the comments for each parameter
    /// </summary>
    public class QuickQuoteRequestParms
    {
        /// <summary>
        /// QuoteId associated with the rating request
        /// </summary>
        [Required]
        public int? QuoteId { get; set; }

        /// <summary>
        /// return data for specified Effective Date
        /// </summary>
        [Required]
        public DateTime EffDate { get; set; }

        /// <summary>
        /// <strong>For internal use by the Insurance Service only</strong><br/>
        /// return data for the default carrier
        /// </summary>
        [StringLength(8)]
        public string Carrier { get; set; }

        /// <summary>
        /// <strong>For internal use by the Insurance Service only</strong><br/>
        /// return data for the default carrier
        /// </summary>
        [StringLength(8)]
        public string Agency { get; set; }

        /// <summary>
        /// return data for specified Exposure(s)
        /// </summary>
        public List<Exposure> Exposures { get; set; }

        /// <summary>
        /// Normally set to false. <br />
        /// - If true, the service won't peform premium calculations, and won't return a Premium value. <br />
        /// - If false, a Premium value will be returned.  <br />
        /// - InitialCarrier will be returned, regardless of the setting of this value. 
        /// </summary>
        public bool SkipRating { get; set; }
    }
}
