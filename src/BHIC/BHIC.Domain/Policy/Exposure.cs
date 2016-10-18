using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Background;

namespace BHIC.Domain.Policy
{
    /// <summary>
    /// Exposure associated with a given Quote, including payroll, location, and class information.  There may be multiple Exposures associated with a given Quote.
    /// </summary>
    public class Exposure
    {

        /// <summary>
        /// ID of the Exposure associated with the operation.<br />
        /// If provided, specifies the ID of the Exposure to update, otherwise the Exposure will be inserted, and the ID returned in OperationStatus.AffectedIds.<br /> 
        /// Example: 123<br />
        /// </summary>
        public int? ExposureId { get; set; }

        /// <summary>
        /// ID of the CoverageState that the Exposure is assigned to.<br />
        /// If CoverageStateId is provided, QuoteId is ignored; the Exposure will be assigned to the specified CoverageState.<br />
        /// If CoverageStateId is not provided, and if a CoverageState matching the passed StateAbbr or ZipCode exists, the Exposure will be assigned to the existing CoverageState.<br />
        /// If CoverageStateId is not provided, both LobData and CoverageState will be created if they don't already exist, and the Exposure will be assigned to the new CoverageState.<br />
        /// Example: 123<br />
        /// Validation:<br />
        /// 1) ExposureId, CoverageStateId, or QuoteId is required.<br />
        /// 2) Related: for those Quotes that contain Exposures within the state of California, one (and only one) CA Mailing address Location must be exist prior to calling Questions GET or Questions POST; see additional detail in the help text for the Locations POST service.<br />
        /// </summary>
        public int? CoverageStateId { get; set; }

        /// <summary>
        /// ID of the Quote that the Exposure is assigned to.<br />
        /// <ul>
        /// <li>If CoverageStateId is provided, QuoteId is ignored; the Exposure will be assigned to the specified CoverageState.</li>
        /// <li>If CoverageStateId is not provided, and if a CoverageState matching the passed StateAbbr or ZipCode exists, the Exposure will be assigned to the existing CoverageState.</li>
        /// <li>If CoverageStateId is not provided, both LobData and CoverageState will be created if they don't already exist, and the Exposure will be assigned to the new CoverageState.</li>
        /// </ul>
        /// QuoteId is initially obtained by calling the QuoteState POST service.<br />
        /// Example: 123<br />
        /// Validation:<br />
        /// 1) ExposureId, CoverageStateId, or QuoteId is required.<br />
        /// </summary>
        public int? QuoteId { get; set; }

        /// <summary>
        /// Identifier that specifies the Line of Business associated with the operation<br />
        /// Validation:<br />
        /// 1) Required<br />
        /// 2) See the LobData service documentation for examples of valid values:<br />
        /// </summary>
        [StringLength(2)]
        [Required]
        public string LOB { get; set; }

        /// <summary>
        /// ID of the Industry assigned to the Exposure. Associated with use of Industry/Subindustry/ClassDescription for class selection.<br /><br />
        /// Valid values are available via the Industries service.<br /><br />
        /// <b>To properly identify a specific class</b> for an exposure, one of the following combinations is required:<br /><br />
        /// a) A valid IndustryId/SubIndustryId/ClassDescriptionId combination<br />
        /// b) ... or, ClassDescriptionKeywordId (which is translated by the service into a valid IndustryId/SubIndustryId/ClassDescriptionId combination).<br />
        /// c) ... or, CompanionClassId.<br /><br />
        /// <b>Note: </b>If parameters are specified for more than one use case listed above, only the first satisfied case is processed by the service.<br />		
        /// <br />
        /// Alternately, to add an Exposure without identifying a specific Class, see OtherClassDesc><br />
        /// <br />
        /// Example: 123<br />
        /// Validation:<br />
        /// 1) Tested against valid values that are available via the Industries service.<br />
        /// </summary>
        public int? IndustryId { get; set; }

        /// <summary>
        /// ID of the SubIndustry assigned to the Exposure. Associated with use of Industry/Subindustry/ClassDescription for class selection.<br />
        /// Valid values are available via the SubIndustries service.<br />
        /// See IndustryId for additional information regarding valid values / validation.<br />
        /// Example: 123<br />
        /// Validation:<br />
        /// 1) Tested against valid values that are available via the SubIndustries service.<br />		
        /// </summary>
        public int? SubIndustryId { get; set; }

        /// <summary>
        /// ID of the ClassDescription assigned to the Exposure. Associated with use of Industry/Subindustry/ClassDescription for class selection.<br />
        /// See IndustryId for additional information regarding valid values / validation.<br />
        /// Example: 123<br />
        /// Validation:<br />
        /// 1) Tested against valid values that are available via the ClassDescriptions service.<br />
        /// </summary>
        public int? ClassDescriptionId { get; set; }

        /// <summary>
        /// Available for use instead of ClassDescriptionId.<br />
        /// When searching by Industry/Subindustry/ClassDescription, if the user is unable to locate a relevant ClassDescription, they could be prompted to enter a brief text description in place of a specific ClassDescription; this value could be reviewed by an underwriter to determine next steps.<br />
        /// The brief text description would be stored here, for follow-up and reporting purposes.<br />
        /// <br />
        /// <b>IMPORTANT:</b>
        /// <br />
        /// The use of OtherClassDesc precludes the execution of services that require a valid ClassDescription, including the following, .<br />
        /// <ul>
        /// <li>PolicyCreate POST (because a policy can't be processed or created if any Exposures don't specify ClassDescription)</li>
        /// <li>VExposuresMinPayroll GET (because minimum exposure amounts can't be validated without specifying a ClassDescription)</li>
        /// </ul>
        /// In cases where OtherClassDesc is used, because a policy can't be created, an alternate quote flow might be to format an email message that contains all relevant quote and contact information, and forward it to an underwriter for follow-up.<br />
        /// </summary>
        [StringLength(50)]
        public string OtherClassDesc { get; set; }

        /// <summary>
        /// ID of the ClassDescriptionKeyword assigned to the Exposure. Associated with use of Keyword Search for class selection.<br />
        /// Valid values are available via the ClassDescriptionKeywords service<br />
        /// See IndustryId for additional information regarding valid values / validation.<br />
        /// Example: 123<br />
        /// Validation:<br />
        /// 1) Must be a valid existing value.<br />
        /// </summary>
        public int? ClassDescriptionKeywordId { get; set; }

        /// <summary>
        /// ID of the CompanionClass assigned to the Exposure.<br />
        /// Valid values are available via the CompanionClasses service.
        /// See IndustryId for additional information regarding valid values / validation.<br />
        /// Example: 123<br />
        /// Validation:<br />
        /// 1) Must be a valid existing value.<br />
        /// </summary>
        public int? CompanionClassId { get; set; }

        /// <summary>
        /// Read-only user-friendly class name/description.
        /// </summary>
        public string FriendlyLabel { get; set; }

        /// <summary>
        /// State associated with the Exposure.<br />
        /// Example: NY<br />
        /// Validation:<br />
        /// 1) Either StateAbbr or ZipCode or both may be provided when submitting an exposure.<br />
        /// 2) If only state is provided, only state is persisted.<br />
        /// 3) If only zip code is provided, state will be derived from zip code, and both will be persisted.<br />
        /// 4) When both are persisted, the zip/state combination will be validated.<br />
        /// 5) Zip code length must be 5 characters.<br />
        /// 6) State abbreviation must be 2 characters.<br />
        /// </summary>
        [StringLength(2)]
        public string StateAbbr { get; set; }

        /// <summary>
        /// 5 digit Zip code associated with the Exposure.<br />
        /// Example: 71263<br />
        /// Validation:<br />
        /// 1) See StateAbbr validation rules.
        /// </summary>
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Invalid Zip")]
        [StringLength(10)]
        public string ZipCode { get; set; }

        /// <summary>
        /// LocationId associated with the Exposure.<br />
        /// Each Exposure on a policy is required to be associated with an existing Location at the time that the policy is created on back-end systems via the PolicyCreate POST service.<br />
        /// If LocationId is specified, the Exposure will be associated with that Location.<br />
        /// If LocationId is not specified, the Exposure will be associated with the first existing Location whose State matches the Exposure's StateAbbr.<br />
        /// Validation:<br />
        /// 1) If specified, must match an existing Location.LocationId<br />
        /// 2) If not specified, when PolicyCreate POST is called for the Quote, a Location whose State matches the Exposure's StateAbbr must exist.
        /// </summary>
        public int? LocationId { get; set; }

        // ----------------------------------------
        // Standard Class Info
        // ----------------------------------------

        /// <summary>
        /// Read-only ClassCode associated with the exposure.<br />
        /// Example: 8803<br />
        /// </summary>
        [StringLength(4)]
        public string ClassCode { get; set; }

        /// <summary>
        /// Read-only ClassSuffix associated with the Exposure.<br />
        /// Example: 01<br />
        /// </summary>
        [StringLength(2)]
        public string ClassSuffix { get; set; }

        /// <summary>
        /// Annual Payroll associated with the Exposure.  Required.<br />
        /// Example: 150000<br />
        /// Validation:<br />
        /// 1) Required.<br />
        /// 2) Amount specified must be >= the configurable minimum amount established for the class / state.<br />
        /// </summary>
        [Required(ErrorMessage = "The annual exposure amount is required.")]
        public decimal? ExposureAmt { get; set; }


        /// <summary>
        /// Read-only Class Description.<br />
        /// Populated only for persisted Exposures that have previously been successfully saved, and associated with a specific ClassDescription.<br />
        /// Populated only by Quotes GET service.<br />
        /// </summary>
        public ClassDescription ClassDescription { get; set; }

        /// <summary>
        /// Read-only Companion Class.<br />
        /// Populated only for persisted Exposures that have previously been successfully saved, and associated with a specific CompanionClass.<br />
        /// Populated only by Quotes GET service.<br />
        /// </summary>
        public CompanionClass CompanionClass { get; set; }

        /// <summary>
        /// Read-only ClassDescriptionKeyword.<br />
        /// Populated only for persisted Exposures that have previously been successfully saved, and associated with a specific ClassDescriptionKeyword.<br />
        /// Example: This property will be populated if the Exposure POST service was passed a valid ClassDescriptionKeywordId (...which represents the user specifying a class by selecting a ClassDescriptionKeyword.)<br />
        /// Populated by the following services: Quotes GET, LobData GET, CoverageStates GET, Exposures GET.<br />
        /// </summary>
        public ClassDescriptionKeyword ClassDescriptionKeyword { get; set; }
    }
}
