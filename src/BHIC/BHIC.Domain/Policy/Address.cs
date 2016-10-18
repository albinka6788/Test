using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace BHIC.Domain.Policy
{
    /// <summary>
    /// Address associated with a given Contact.  There may be multiple Addresses associated with a given Contact.
    /// </summary>
    public class Address
    {

        /// <summary>
        /// ID of the Address associated with the operation.<br />
        /// If provided, specifies the ID of the Address to update, otherwise the Address will be inserted, and the ID returned in OperationStatus.AffectedIds.<br /> 
        /// Example: 123<br />
        /// </summary>
        public int? AddressId { get; set; }

        /// <summary>
        /// ID of the Contact that the Address is assigned to.<br />
        /// Example: 123<br />
        /// Validation:<br />
        /// 1) Required.<br />
        /// </summary>
        public int? ContactId { get; set; }

        [StringLength(50)]
        public string Addr1 { get; set; }

        [StringLength(50)]
        public string Addr2 { get; set; }

        [StringLength(30)]
        public string City { get; set; }

        [StringLength(2)]
        public string State { get; set; }

        [StringLength(5)]
        public string Zip { get; set; }
    }
}
