using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.QuestionEngine
{
    /// <summary>
    /// Parameters associated with the Questions service<br />
    /// Filters the response as indicated by the comments for each parameter
    /// </summary>
    public class QuestionRequestParms
    {
        /// <summary>
        /// ID of the Quote associated with the questions.<br />
        /// Example: 123<br />
        /// Validation:<br />
        /// 1) Required.<br />
        /// 2) Related: for those Quotes that contain Exposures within the state of California, one (and only one) CA Mailing address Location must be exist prior to calling this service; see additional detail in the help text for the Locations POST service.<br />
        /// </summary>
        [Required]
        public int? QuoteId { get; set; }
    }
}
