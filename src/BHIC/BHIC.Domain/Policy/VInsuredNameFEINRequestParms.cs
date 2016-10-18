using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    public class VInsuredNameFEINRequestParms
    {
        /// <summary>
        /// SSN / Tax ID / Federal Employer Identification Number (FEIN) (<br />
        /// Required.<br />
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
        [StringLength(9, MinimumLength = 9)]
        public string FEIN { get; set; }
    }
}