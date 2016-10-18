using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace BHIC.Domain.Policy
{
    /// <summary>
    /// InsuredName associated with a given Quote.  There may be multiple InsuredNames associated with a given Quote.
    /// </summary>
    public class InsuredName
    {
        // From spec: List of {Type: NamedInsured/DBA/AdditionalName;  Name; BizType; FEINType; FEIN }

        /// <summary>
        /// ID of the InsuredName associated with the operation.<br />
        /// If provided, specifies the ID of the InsuredName to update, otherwise the InsuredName will be inserted, and the ID returned in OperationStatus.AffectedIds.<br /> 
        /// Example: 123<br />
        /// </summary>
        public int? InsuredNameId { get; set; }

        /// <summary>
        /// ID of the Quote that the Contact is assigned to.<br />
        /// Example: 123<br />
        /// Validation:<br />
        /// 1) Required.<br />
        /// </summary>
        public int? QuoteId { get; set; }

        /// <summary>
        /// Set to true to identify the primary insured name that will appear on the policy.<br />
        /// Set to false to identify additional insured names covered by the policy.<br />
        /// Validation:<br />
        /// 1) Required
        /// 2) One primary name must be identified for a given policy.<br />
        /// 3) Only one primary insured name is permitted for a given policy.<br />
        /// </summary>
        public bool? PrimaryInsuredName { get; set; }

        /// <summary>
        /// Type of InsuredName (examples: NamedInsured/DBA/AdditionalName).<br />
        /// Valid values will be made available via the BusinessTypes service.<br />
        /// </summary>
        [StringLength(1)]
        public string NameType { get; set; }		// from spec: NamedInsured/DBA/AdditionalName	

        /// <summary>
        /// If NameType = "Other", this field can be used to capture a short description of the NameType.<br />
        /// Optional.
        /// </summary>
        [StringLength(35)]
        public string NameTypeDesc { get; set; }

        /// <summary>
        /// Name of the Business<br />
        /// Validation:<br />
        /// 1) Required unless NameType = Individual or Partner.<br />
        /// 2) Any individual word making up InsuredName.Name must be no longer than 50 characters.
        /// </summary>
        [StringLength(150)]
        public string Name { get; set; }

        /// <summary>
        /// First name of the person associated with the business, if required by the NameType.<br />
        /// Validation: <br />
        /// 1) Required if NameType = "Individual" or "Partner"
        /// </summary>
        [StringLength(50)]
        public string FirstName { get; set; }

        /// <summary>
        /// Middle name of the person associated with the business.<br />
        /// Relevant only if NameType = "Individual" or "Partner"<br />
        /// Optional.
        /// </summary>
        [StringLength(50)]
        public string MiddleName { get; set; }

        /// <summary>
        /// Last name of the person associated with the business, if required by the NameType.<br />
        /// Validation: <br />
        /// 1) Required if NameType = "Individual" or "Partner"
        /// </summary>
        [StringLength(50)]
        public string LastName { get; set; }

        // C36030 - updated help text
        /// <summary>
        /// Federal Tax ID Number<br />
        /// Optional.<br />
        /// Validation:  Must be a valid SSN or Tax Id number.<br />
        /// <b>Important:</b> <br />
        /// <ul>
        /// <li>For security and data integrity reasons, specific tax id validation rules should not be displayed to the end user; a user with a valid tax id should be able to proceed without issue.  Those users that provide an invalid tax id should be presented with a generic error message.</li>
        /// <li> The rules published below are for intended for use in development support and testing, only.</li>
        /// </ul>
        /// 
        /// 1) Must include 9 numeric characters (ie: 123123123 is valid, but 123abc1232 is not) <br />
        /// 2) Cannot increment or decrement 7 or more times (ie: 124567800 is valid, but 123467890 is not)<br />
        /// 3) There must be at least 2 distinct numbers (ie: 121212121)<br />
        /// 4) The number must change twice to validate (ie: 111221111 is valid, but 111112222 is not)<br />
        /// </summary>
        [StringLength(9)]
        public string FEIN { get; set; }

        /// <summary>
        /// Type of FEIN (examples: Corporation/Partnership/Individual).<br />
        /// Valid values will be made available via the FEINTypes service.<br />
        /// Optional.
        /// </summary>
        [StringLength(1)]
        public string FEINType { get; set; }
    }
}
