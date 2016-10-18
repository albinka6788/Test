using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BHIC.Domain.Policy
{
    /// <summary>
    /// CoverageState associated with a given quote.  There may be multiple CoverageStates associated with a given LobData.
    /// </summary>
    public class CoverageState
    {
        // ----------------------------------------
        // constructor
        // ----------------------------------------

        public CoverageState()
        {
            // init lists to help avoid issues related to null reference exceptions
            Exposures = new List<Exposure>();
            Modifiers = new List<Modifier>();
        }

        // ----------------------------------------
        // properties
        // ----------------------------------------

        // from spec: StateEmployerCode					   

        /// <summary>
        /// ID of the CoverageState associated with the operation.<br />
        /// If provided, specifies the ID of the CoverageState to update, otherwise the CoverageState will be inserted, and the ID returned in OperationStatus.AffectedIds.<br /> 
        /// Example: 123<br />
        /// </summary>
        public int? CoverageStateId { get; set; }

        [Required]
        public int LobDataId { get; set; }

        /// <summary>
        /// State associated with the CoverageState.<br />
        /// Example: NY<br />
        /// Validation:<br />
        /// 1) Either State or ZipCode or both may be provided when submitting a CoverageState.<br />
        /// 2) If only State is provided, the specified State is persisted.<br />
        /// 3) If only ZipCode is provided, the associated State will be derived from ZipCode, and State will be persisted.<br />
        /// 4) If both State and Zipcode are provided, the specified State will be persisted, and ZipCode will be ignored.<br />
        /// 5) Zip code length must be 5 characters.<br />
        /// 6) State abbreviation must be 2 characters.<br />
        /// 7) Only unique states may be persisted for a given LobData > CoverageState; attempts to POST a new LobData > CoverageState that already exists will result in an error.<br />
        /// </summary>
        [StringLength(2)]
        public string State { get; set; }

        /// <summary>
        /// 5 digit Zip code associated with the CoverageState.<br />
        /// Example: 71263<br />
        /// Validation:<br />
        /// 1) Either State or ZipCode or both may be provided when submitting an CoverageState.<br />
        /// 2) If only State is provided, the specified State is persisted.<br />
        /// 3) If only ZipCode is provided, the associated State will be derived from ZipCode, and State will be persisted.<br />
        /// 4) If both State and Zipcode are provided, the specified State will be persisted, and ZipCode will be ignored.<br />
        /// 5) Zip code length must be 5 characters.<br />
        /// 6) State abbreviation must be 2 characters.<br />
        /// 7) Only unique states may be persisted for a given LobData > CoverageState; attempts to POST a new LobData > CoverageState that already exists will result in an error.<br />
        /// </summary>
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Invalid Zip")]
        [StringLength(10)]
        public string ZipCode { get; set; }

        /// <summary>
        /// Employer Code associated with the employer for the specified state
        /// </summary>
        [StringLength(12)]
        public string EmployerCode { get; set; }

        /// <summary>
        /// Exposures associated with the CoverageState
        /// </summary>
        public List<Exposure> Exposures { get; set; }

        /// <summary>
        /// Modifiers associated with the CoverageState
        /// </summary>
        public List<Modifier> Modifiers { get; set; }
    }
}
