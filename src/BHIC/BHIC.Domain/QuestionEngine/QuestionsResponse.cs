using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

using BHIC.Domain.Service;

namespace BHIC.Domain.QuestionEngine
{
    public class QuestionsResponse
    {
        // ----------------------------------------
        // constructor
        // ----------------------------------------

        public QuestionsResponse()
        {
            // init objects to help avoid issues related to null reference exceptions
            OperationStatus = new OperationStatus();
            QuestionsRequest = new QuestionsRequest();
            Questions = new List<Question>();
            QuestionRules = new List<QuestionRule>();
            PricingMods = new List<PricingMod>();
            PricingMatrix = new List<QuestionRulePricing>();
            PricingParameters = new List<CarrierPricing>();
        }

        // ----------------------------------------
        // properties
        // ----------------------------------------

        /// <summary>
        /// ID of the Quote associated with the questions.<br />
        /// Example: 123<br />
        /// Validation:<br />
        /// 1) Required.<br />
        /// 2) Related: for those Quotes that contain Exposures within the state of California, one (and only one) CA Mailing address Location must be exist prior to calling this service; see additional detail in the help text for the Locations POST service.<br />
        /// </summary>
        [Required]
        public int? QuoteId { get; set; }

        /// <summary>
        /// Read-only response/status information; populated by the service.
        /// </summary>
        public OperationStatus OperationStatus { get; set; }

        /// <summary>
        /// Read-only text description of overall quote status, resulting from execution of the Questions GET or POST service.
        /// Valid values:<br />
        /// <ul>
        ///		<li>'Quote': The service was able to calculate a valid quote (premium amount), and the Quote is a candidate for issuance, based on information provided. Indicates that Decision Engine / Question Engine processing was successful.</li>
        ///		<li>'Decline': The service was unable to calculate a valid quote (premium amount), based on information provided.  A policy can't be issued for Quotes with this status.  The reason for this status will be contained in RatingData.QuoteStatusText.</li>
        ///		<li>'Refer': The service was able to calculate a valid quote (premium amount), based on information provided.  A policy can't be issued for Quotes with this status.  The reason for this status will be contained in RatingData.QuoteStatusText.</li>
        /// </ul>
        /// </summary>			
        public string QuoteStatus { get; set; }

        /// <summary>
        /// Read-only text message, resulting from execution of the Questions GET or POST service.<br />
        /// Describes the reason that a valid quote (premium amount) can't be calculated for the Quote, if applicable.
        /// </summary>
        public string ResultMessages { get; set; }

        //This will be Y:Yes, D: Hard Decline, S: Soft Referral., H: Hard Referral, N: Not Validated.  It will be set based off of the answers to the questions.
        /// <summary>
        /// Read-only code reflecting Question Engine status, resulting from execution of the Questions POST service.<br />
        /// Valid Values<br />
        /// <ul>
        ///		<li>'Y': Passed Question Engine rules.</li>
        ///		<li>'S': The service was unable to calculate a valid premium, based on information provided, resulting in a Soft Referral.  A policy can't be issued for Quotes with this status.  The reason for this status will be contained in RatingData.QuoteStatusText.</li>
        /// </ul>
        /// </summary>
        public string QuestionsResponsesValid { get; set; }

        /// <summary>
        /// Read-only code reflecting Decision Engine status, resulting from execution of the Questions GET or POST service.<br />
        /// Valid Values<br />
        /// <ul>
        ///		<li>'Y': Passed Decision Engine Rules.</li>
        ///		<li>'S': The service was unable to calculate a valid premium, based on information provided, resulting in a Soft Referral.  A policy can't be issued for Quotes with this status.  The reason for this status will be contained in RatingData.QuoteStatusText.</li>
        /// </ul>
        /// </summary>
        //This will be Y:Yes, D: Hard Decline, S: Soft Referral, H: Hard Referral.  It will be set based off of the call to the Decision Engine.
        public string DecisionEngineResponsesValid { get; set; }

        // prospect-provided parameters
        /// <summary>
        /// Read-only; for internal use by the Insurance Service only.
        /// </summary>
        public QuestionsRequest QuestionsRequest { get; set; }

        // questions (and responses) list
        /// <summary>
        /// Questions to be displayed to the user, along with rules for conditional display, along with previously-saved user responses.
        /// </summary>
        public List<Question> Questions { get; set; }

        // QuestionRule list
        /// <summary>
        /// Read-only; for internal use by the Insurance Service only.
        /// List of rules, used to determine whether or not responses provided by the user are aligned with current risk appetite.<br />
        /// Populated as a result of executing the Questions Post service.
        /// </summary>
        public List<QuestionRule> QuestionRules { get; set; }

        // per installment added to the premium amount to show the total cost
        /// <summary>
        /// Read-only installment charge. Calculated automatically when performing Questions GET or POST 
        /// </summary>
        public decimal PerInstallmentCharge { get; set; }

        /// <summary>
        /// Read-only annual premium amount associated with the policy. Calculated automatically when performing Questions GET or POST 
        /// PCO-This is no longer readonly.  Inside of the Questions Post, the value of premium may be updated based on the response to questions.
        /// </summary>
        public decimal Premium { get; set; }

        /// <summary>
        /// Read-only abbreviation for the Governing State for the policy. Calculated automatically when performing Questions GET or POST 
        /// </summary>
        public string GovStateAbbr { get; set; }

        // pricing mods (C32317-044 - Support sched mods for internet credits)
        /// <summary>
        /// For Internal Insurance Service use only.
        /// </summary>
        public List<PricingMod> PricingMods { get; set; }

        /// <summary>
        /// Question Rule Pricing information that will be used to get UWRating and UWSchedMods records for Schedule Rating Question Rules.
        /// </summary>
        public List<QuestionRulePricing> PricingMatrix { get; set; }

        /// <summary>
        /// Pricing Parameters that are used by the pricing matrix in order to determine Schedule Rating.  Values come from CarrierThresholds and SchedModEligibilty tables.
        /// </summary>
        public List<CarrierPricing> PricingParameters { get; set; }

    }
}
