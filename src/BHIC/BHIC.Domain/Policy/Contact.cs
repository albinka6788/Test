using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace BHIC.Domain.Policy
{
    /// <summary>
    /// Contact associated with a given Quote.  There may be multiple Contacts associated with a given Quote.
    /// </summary>
    public class Contact
    {
        //  from spec: LossControl/Exec/Billing/Audit/RiscMan/Misc/Claims/PSC

        // ----------------------------------------
        // constructor
        // ----------------------------------------

        public Contact()
        {
            // init objects to help avoid issues related to null reference exceptions
            Phones = new List<Phone>();
            Addresses = new List<Address>();
        }

        // ----------------------------------------
        // properties
        // ----------------------------------------

        /// <summary>
        /// ID of the Contact associated with the operation.<br />
        /// If provided, specifies the ID of the Contact to update, otherwise the Contact will be inserted, and the ID returned in OperationStatus.AffectedIds.<br /> 
        /// Example: 123<br />
        /// </summary>
        public int? ContactId { get; set; }

        /// <summary>
        /// ID of the Quote that the Contact is assigned to.<br />
        /// Example: 123<br />
        /// Validation:<br />
        /// 1) Required if ContactId is not specified.<br />
        /// </summary>
        public int? QuoteId { get; set; }

        /// <summary>
        /// Type of Contact(examples: LossControl/Exec/Billing/Audit/RiscMan/Misc/Claims/PSC).<br />
        /// Valid values will be made available via the ContactTypes service.<br />
        /// </summary>
        [StringLength(15)]
        public string ContactType { get; set; }

        [StringLength(256)]
        public string Name { get; set; }

        [StringLength(128)]
        public string Title { get; set; }

        [StringLength(128)]
        public string Company { get; set; }

        [StringLength(128)]
        public string Email { get; set; }

        [StringLength(128)]
        public string WebAddr { get; set; }

        public List<Phone> Phones { get; set; }
        public List<Address> Addresses { get; set; }
    }
}
