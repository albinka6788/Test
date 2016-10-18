#region Using directives

using System;
using System.ComponentModel.DataAnnotations;

#endregion

namespace BHIC.Domain.Policy
{
    /// <summary>
    /// Prospect Information associated with a given Quote.  <br />
    /// This service can be called to save and retrieve prospect contact information related to a given Quote.
    /// Use Cases: <br />
    /// - The information captured here can be used later by the AAOC team for purposes of following up with the prospect.<br />
    /// - The information captured here can be used to populate properties that are later required by other services.<br />
    /// </summary>
    public class ProspectInfo
	{
        /// <summary>
        /// ID of the ProspectInfo associated with the operation.<br />
        /// If provided, specifies the ID of the ProspectInfo to update, otherwise the ProspectInfo will be inserted, and the ID returned in OperationStatus.AffectedIds.<br /> 
        /// Example: 123<br />
        /// </summary>
        public int? ProspectInfoId { get; set; }

		/// <summary>
		/// ID of the Quote that the ProspectInfo is assigned to.<br />
		/// Example: 123<br />
		/// Validation:<br />
		/// 1) Required.<br />
		/// </summary>
        [Required]
		public int QuoteId { get; set; }

        /// <summary>
        /// Contact Name
        /// </summary>
        [StringLength(150)]
        public string CompanyName { get; set; }

        /// <summary>
        /// Prospect's Phone Number
        /// </summary>
        [StringLength(10)]
		public string PhoneNumber { get; set; }

        /// <summary>
        /// Prospect's Phone Number Extension
        /// </summary>
		[StringLength(4)]
		public string Extension { get; set; }

        /// <summary>
        /// Contact Name
        /// </summary>
        [StringLength(256)]
        public string ContactName { get; set; }

        /// <summary>
        /// Email address
        /// </summary>
        [StringLength(128)]
        public string Email { get; set; }

        /// <summary>
        /// Prospect's address line 1
        /// </summary>
        [StringLength(50)]
        public string Addr1 { get; set; }

        /// <summary>
        /// Prospect's address line 2
        /// </summary>
        [StringLength(50)]
        public string Addr2 { get; set; }

        /// <summary>
        /// Prospect's City
        /// </summary>
        [StringLength(30)]
        public string City { get; set; }

        /// <summary>
        /// Prospect's State
        /// </summary>
        [StringLength(2)]
        public string State { get; set; }

        /// <summary>
        /// Prospect's Zip Code
        /// </summary>
        [StringLength(5)]
        public string Zip { get; set; }
    }
}
