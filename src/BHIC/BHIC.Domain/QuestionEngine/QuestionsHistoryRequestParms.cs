#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace BHIC.Domain.QuestionEngine
{
    /// <summary>
    /// Parameters associated with the QuestionsHistory service<br />
    /// Filters the response as indicated by the comments for each parameter
    /// </summary>
    public class QuestionsHistoryRequestParms
    {
        /// <summary>
        /// ID of the Quote associated with the desired historical questions.<br />
        /// Example: 123<br />
        /// Validation:<br />
        /// 1) Required.<br />
        /// </summary>
        [Required]
        public int? QuoteId { get; set; }

        /// <summary>
        /// Specify the type of history to be returned.
        /// </summary>
        public HistoryType HistoryType { get; set; }

        /// <summary>
        /// Optional.  If set to true, historical questions that don't exist in the current question set will be suppressed from the response.<br />
        /// Example Use Case: <br />
        /// - Customer initially specifies a Florist exposure, and responds to Florist-related questions.<br />
        /// - Customer then clicks back, and changes exposure to Flooring, and responds to Flooring-related questions.<br />
        /// - Setting the SuppressNonCurrent property to true will cause Florist-specific questions to be removed from the results.<br />
        /// </summary>
        public bool SuppressNonCurrent { get; set; }
    }

    /// <summary>
    /// Types of history available with the QuestionsHistory GET request
    /// </summary>
    public enum HistoryType
    {

        /// <summary>
        /// Return the most recent set of historical questions and responses (prior to the current / active questions).
        /// </summary>
        MostRecent,

        /// <summary>
        /// Return the most recent set of historical questions and responses (prior to the current / active questions) that resulted in a decline.
        /// </summary>
        MostRecentDecline,

        /// <summary>
        /// Return the most recent set of historical questions and responses (prior to the current / active questions)  that resulted in a referral.
        /// </summary>
        MostRecentReferral
    }
}