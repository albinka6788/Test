using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace BHIC.Domain.Policy
{
    /// <summary>
    /// Location associated with a given Quote.  There may be multiple Locations associated with a given Quote.
    /// </summary>
    public class Location
    {
        // from spec: List of {Type:Mailing/Physical/Billing, Address, City, ST, ZIP, FullTime, PartTime}

        /// <summary>
        /// ID of the Location associated with the operation.<br />
        /// If provided, specifies the ID of the Location to update, otherwise the Location will be inserted, and the ID returned in OperationStatus.AffectedIds.<br /> 
        /// Example: 123<br />
        /// </summary>
        public int? LocationId { get; set; }

        /// <summary>
        /// ID of the Quote that the Location is assigned to.<br />
        /// Example: 123<br />
        /// Validation:<br />
        /// 1) Required.<br />
        /// </summary>
        public int? QuoteId { get; set; }

        /// <summary>
        /// Type of Location(examples: Mailing/Physical/Billing).<br />
        /// Valid values will be made available via the LocationTypes service.<br />
        /// </summary>
        [StringLength(1)]
        public string LocationType { get; set; }	// from the spec: Mailing/Physical/Billing

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

        /// <summary>
        /// Number of full time employees at this location
        /// </summary>
        public int FullTime { get; set; }

        /// <summary>
        /// Number of part time employees at this location
        /// </summary>
        public int PartTime { get; set; }

        /// <summary>
        /// Set to true if the Location represents the Primary California Mailing address, for those Quotes that contain Exposures within the state of California.<br />
        /// Validation:<br />
        /// 1) Required<br />
        /// 2) For those Quotes that contain Exposures within the state of California, one (and only one) CA Mailing address Location must be added, where:<br />
        /// <ul>
        ///		<li>LocationType = M</li>
        ///		<li>State = CA</li>
        ///		<li>Zip = valid CA zip code</li>
        /// </ul> 
        /// 3) For those Quotes that contain Exposures within the state of California, the above CA Mailing Location must be added prior to calling Questions GET or Questions POST, since the resulting premium calculations rely on the presence of this data.
        /// </summary>
        [Required]
        public bool? PrimaryCaMailAddr { get; set; }
    }
}
