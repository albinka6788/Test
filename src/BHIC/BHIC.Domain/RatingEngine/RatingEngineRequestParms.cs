#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace BHIC.Domain.RatingEngine
{
    /// <summary>
    /// Parameters associated with Rating Engine Requests<br />
    /// Filters the rating action as indicated by the comments for each parameter
    /// </summary>
    public class RatingEngineRequestParms
    {
        public RatingEngineRequestParms()
        {
            ClassItems = new List<QuestionEngine.ClassItem>();
            Modifiers = new List<Policy.Modifier>();
        }

        /// <summary>
        /// Identifier used to associate the rating request with the source entity (could be MGA Code, Quote ID, etc...) <br />
        /// Captured in rating engine event logging.
        /// </summary>
        public string RequestIdentifier { get; set; }

        /// <summary>
        /// Carrier associated with the rating
        /// </summary>
        public string Carrier { get; set; }

        /// <summary>
        /// Agency associated with the rating
        /// </summary>
        public string Agency { get; set; }

        /// <summary>
        /// Effective Date associated with the rating (typically today's date)
        /// </summary>
        public DateTime? EffDate { get; set; }

        /// <summary>
        /// If a California exposure is present, the zip code for the primary CA location Mailing Address is required (used for territorial rating)
        /// </summary>
        public string PrimaryCaMailingZip { get; set; }

        /// <summary>
        /// List of classes associated with the rating, including state, classcode, class suffix, exposure...
        /// </summary>
        public List<QuestionEngine.ClassItem> ClassItems { get; set; }

        /// <summary>
        /// List of Modifiers associated with the rating
        /// </summary>
        public List<Policy.Modifier> Modifiers { get; set; }

    }
}