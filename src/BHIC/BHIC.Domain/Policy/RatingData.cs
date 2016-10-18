using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    public class RatingData
    {
        // from spec: { results from Rating Engine }

        /// <summary>
        /// ID of the Quote associated with the operation.
        /// </summary>
        public int QuoteId { get; set; }

        // ----------------------------------------
        // Rating Engine Status (resulting from rating that occurs during Question/Decision engine processing)
        // ----------------------------------------

        /// <summary>
        /// Abbreviation for the Governing State for the policy. Calculated automatically when performing Questions GET or POST 
        /// </summary>
        public string GovStateAbbr { get; set; }

        /// <summary>
        /// Annual premium amount associated with the policy. Calculated automatically when performing Questions GET or POST 
        /// </summary>
        public decimal? Premium { get; set; }

        /// <summary>
        /// Installment Fee associated with the policy's payment terms
        /// </summary>
        public decimal PerInstallmentCharge { get; set; }

        /// <summary>
        /// Convenience Fee associated with the policy's payment terms
        /// </summary>
        public decimal ConvenienceFee { get; set; }

        /// <summary>
        /// Read-only Carrier Value; determined by the Insurance Service (Question / Decision Engine logic)
        /// </summary>
        public string Carrier { get; set; }


        // ********** STATUS ASSOCIATED WITH QUESTIONS GET SERVICE: **********

        // ----------------------------------------
        // Decision Engine Status (resulting from requesting a list of questions)
        // ----------------------------------------

        /// <summary>
        /// Text description of overall quote status, resulting from execution of the Questions GET service.
        /// Valid values:<br />
        /// <ul>
        ///		<li>'Quote': The service was able to calculate a valid quote (premium amount), and the Quote is a candidate for issuance, based on information provided. Indicates that Decision Engine processing was successful.</li>
        ///		<li>'Soft Referral': The service was unable to calculate a valid quote (premium amount), based on information provided.  A policy can't be issued for Quotes with this status.  The reason for this status will be contained in RatingData.QuoteStatusText.</li>
        /// </ul>
        /// </summary>
        public string QuoteStatus { get; set; }					// internal notes: previously supported values included: 'Quote', Soft Referral, Hard Referral, Other Business Description, Hard Decline

        /// <summary>
        /// Text message, resulting from execution of the Questions GET service.<br />
        /// Describes the reason that a valid quote (premium amount) can't be calculated for the Quote, if applicable.
        /// </summary>
        public string QuoteStatusText { get; set; }				// internal notes: previously supported values included: OK, DirectSalesOK=N, Policy premium greater than SBU amount, etc...

        /// <summary>
        /// Code reflecting Decision Engine status, resulting from execution of the Questions GET service.<br />
        /// Valid Values<br />
        /// <ul>
        ///		<li>'Y': Passed Decision Engine Rules.</li>
        ///		<li>'S': The service was unable to calculate a valid premium, based on information provided, resulting in a Soft Referral.  A policy can't be issued for Quotes with this status.  The reason for this status will be contained in RatingData.QuoteStatusText.</li>
        /// </ul>
        /// </summary>
        public string DecisionEngineStatus { get; set; }		// internal notes: previously supported values included: 'H' (Hard Referral), 'S' (Soft Referral), 'Y' (Passed Decision Engine Rules)

        // ********** STATUS ASSOCIATED WITH QUESTIONS POST SERVICE: **********

        // ----------------------------------------
        // Decision/Question Engine Status (resulting from processing the user's responses to questions)
        // ----------------------------------------

        /// <summary>
        /// Text description of overall quote status, resulting from execution of the Questions POST service.<br />
        /// Valid values:<br />
        /// <ul>
        ///		<li>'Quote': The service was able to calculate a valid quote (premium amount), and the Quote is a candidate for issuance, based on information provided. Indicates that both Decision Engine processing and Question Engine processing were successful.</li>
        ///		<li>'Soft Referral': The service was unable to calculate a valid quote (premium amount), based on information provided.  A policy can't be issued for Quotes with this status.  The reason for this status will be contained in RatingData.QuoteStatusText.</li>
        /// </ul>
        /// </summary>
        public string QuoteStatusQ { get; set; }				// internal notes: previously supported values included: 'Quote', Soft Referral, Hard Referral, Other Business Description, Hard Decline

        /// <summary>
        /// Text message, resulting from execution of the Questions POST service.<br />
        /// Describes the reason that a valid quote (premium amount) can't be calculated for the Quote, if applicable.
        /// </summary>
        public string QuoteStatusTextQ { get; set; }			// internal notes: previously supported values included: OK, DirectSalesOK=N, Policy premium greater than SBU amount, etc...

        /// <summary>
        /// Code reflecting Decision Engine status, resulting from execution of the Questions POST service.<br />
        /// Valid Values<br />
        /// <ul>
        ///		<li>'Y': Passed Decision Engine Rules.</li>
        ///		<li>'S': The service was unable to calculate a valid premium, based on information provided, resulting in a Soft Referral.  A policy can't be issued for Quotes with this status.  The reason for this status will be contained in RatingData.QuoteStatusText.</li>
        /// </ul>
        /// </summary>
        public string DecisionEngineStatusQ { get; set; }		// internal notes: previously supported values included: 'H' (Hard Referral), 'S' (Soft Referral), 'Y' (Passed Decision Engine Rules)

        /// <summary>
        /// Code reflecting Question Engine status, resulting from execution of the Questions POST service.<br />
        /// Valid Values<br />
        /// <ul>
        ///		<li>'Y': Passed Question Engine rules.</li>
        ///		<li>'S': The service was unable to calculate a valid premium, based on information provided, resulting in a Soft Referral.  A policy can't be issued for Quotes with this status.  The reason for this status will be contained in RatingData.QuoteStatusText.</li>
        /// </ul>
        /// </summary>
        public string QuestionEngineStatusQ { get; set; }		// internal notes: previously supported values included: 'H' (Hard Referral), 'S' (Soft Referral), 'N' (Not validated), 'Y' (Passed Question Engine Rules)
    }
}
