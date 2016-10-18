using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    /// <summary>
    /// Modifier associated with a given CoverageState. There may be multiple Modifiers associated with a given Quote.
    /// </summary>
    public class Modifier
    {
        // from spec: ModType, ModValue { ie. ExMod, DrugFree, 1.102, 0.885 }

        /// <summary>
        /// ID of the Modifier associated with the operation.<br />
        /// If provided, specifies the ID of the Modifier to update, otherwise the Modifier will be inserted, and the ID returned in OperationStatus.AffectedIds.<br /> 
        /// Example: 123<br />
        /// Validation:<br />
        /// 1) ModifierId, CoverageStateId, or QuoteId is required.<br />
        /// </summary>
        public int? ModifierId { get; set; }

        /// <summary>
        /// ID of the CoverageState that the Modifier is assigned to.<br />
        /// Example: 123<br />
        /// Validation:<br />
        /// 1) ModifierId, CoverageStateId, or QuoteId is required.<br />
        /// </summary>
        public int? CoverageStateId { get; set; }

        /// <summary>
        /// ID of the Quote that the Exposure is assigned to.<br />
        /// If CoverageStateId is provided, QuoteId is ignored; the Exposure will be assigned to the specified CoverageState.<br />
        /// If CoverageStateId is not provided, and if a CoverageState matching the passed StateAbbr or ZipCode exists, the Exposure will be assigned to the existing CoverageState.<br />
        /// If CoverageStateId is not provided, both LobData and CoverageState will be created if they don't already exist, and the Exposure will be assigned to the new CoverageState.<br />
        /// QuoteId is initially obtained by calling the QuoteState POST service.<br />
        /// Example: 123<br />
        /// Validation:<br />
        /// 1) MofifierId, CoverageStateId, or QuoteId is required.<br />
        /// </summary>
        public int? QuoteId { get; set; }

        /// <summary>
        /// Identifier that specifies the Line of Business associated with the operation<br />
        /// Ignored if CoverageStateId is specified.  (If CoverageState exists, LOB has been previously specified)
        /// Validation:
        /// 1) Required if CoverageStateId is not specified.  (If CoverageState exists, LOB has been previously specified, and is therefore not required)
        /// 2) See the LobData service documentation for examples of valid values:<br />
        /// </summary>
        [Required]
        [StringLength(2)]
        public string LOB { get; set; }

        /// <summary>
        /// Type of Modfier.<br />
        /// Validation:<br />
        /// 1) Required.<br />
        /// 2) Valid Values: <br />
        ///  - ExMod: specfies that the modifier is an Experience Mod.<br />
        ///  - Deduct: specfies that the modifier is a WC Deductible Mod.  Valid values are available from the WCDeductibles GET service<br />
        ///  - Limit: specfies that the modifier is a WC Increased Limits of Employers Liability Mod.  Valid values are available from the WCLimits GET service.<br />
        /// </summary>
        [StringLength(10)]
        public string ModType { get; set; }

        /// <summary>
        /// Value of modifer.  Treated as a factor (not a percent).<br />
        /// Examples: .99, 1.01<br />
        /// Not required if ModType = "Limit".<br />
        /// </summary>
        public decimal? ModValue { get; set; }

        /// <summary>
        /// State associated with the Modifier.<br />
        /// Example: NY<br />
        /// Validation:<br />
        /// 1) Either StateAbbr or ZipCode or both may be provided when submitting a Modifier.<br />
        /// 2) If only zip code is provided, state will be derived from zip code.<br />
        /// 3) State abbreviation must be 2 characters.<br />
        /// </summary>
        [StringLength(2)]
        public string State { get; set; }

        /// <summary>
        /// 5 digit Zip code associated with the Exposure.<br />
        /// Example: 71263<br />
        /// Validation:<br />
        /// 1) Either StateAbbr or ZipCode or both may be provided when submitting a Modifier.<br />
        /// 2) If only zip code is provided, state will be derived from zip code.<br />
        /// 3) Zip code length must be 5 characters.<br />
        /// </summary>
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Invalid Zip")]
        [StringLength(10)]
        public string ZipCode { get; set; }

        /// <summary>
        /// For Insurance Service internal use only.
        /// </summary>
        [StringLength(3)]
        public string PrimeSeq { get; set; }        // added 3/19/2015 in support of rating engine call, and for future support of different mod types

        /// <summary>
        /// If ModType is Deduct, SeqCode is required.
        /// </summary>
        [StringLength(3)]
        public string SeqCode { get; set; }			// added 3/19/2015 in support of rating engine call, and for future support of different mod types

        /// <summary>
        /// Not used; for potential future expansion
        /// </summary>
        [StringLength(4)]
        public string Class { get; set; }           // added 3/19/2015 in support of rating engine call, and for future support of different mod types

        /// <summary>
        /// If ModType is Deduct, or Limit, ClassCode is required.<br />
        ///  - For Deduct: Valid values are available from the WCDeductibles GET service<br />
        ///  - For Limit: Valid values are available from the WCLimits GET service.<br />
        /// </summary>
        [StringLength(4)]
        public string ClassCode { get; set; }       // added 3/19/2015 in support of rating engine call, and for future support of different mod types

        /// <summary>
        /// If ModType is Deduct: ClassSuffix is required.  Valid ClassSuffix values for this ModType are available from the WCDeductibles GET service, via the WCDeductibles.DBASE property.
        /// </summary>
        [StringLength(2)]
        public string ClassSuffix { get; set; }		// added 3/19/2015 in support of rating engine call, and for future support of different mod types
    }
}
