using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    /// <summary>
    /// Phone associated with a given Contact.  There may be multiple Phones associated with a given Contact.
    /// </summary>
    public class Phone
    {
        /// <summary>
        /// ID of the Phone associated with the operation.<br />
        /// If provided, specifies the ID of the Phone to update, otherwise the Phone will be inserted, and the ID returned in OperationStatus.AffectedIds.<br /> 
        /// Example: 123<br />
        /// </summary>
        public int? PhoneId { get; set; }

        /// <summary>
        /// ID of the Contact that the Phone is assigned to.<br />
        /// Example: 123<br />
        /// Validation:<br />
        /// 1) Required.<br />
        /// </summary>
        public int? ContactId { get; set; }

        /// <summary>
        /// Type of Phone (examples: Business/Home/Mobile).<br />
        /// Valid values will be made available via the PhoneTypes service.<br />
        /// </summary>
        [StringLength(1)]
        public string PhoneType { get; set; }	// from the spec: Business, Mobile, Home, etc...

        [StringLength(10)]
        public string PhoneNumber { get; set; }

        [StringLength(4)]
        public string Extension { get; set; }
    }
}
