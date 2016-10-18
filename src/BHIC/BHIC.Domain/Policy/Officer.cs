using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    public class Officer
    {
        // from spec: List of {Included/Excluded; Name; Title; SSN; Address; Class; PercentageOwnership; Payroll }

        /// <summary>
        /// ID of the Officer associated with the operation.<br />
        /// If provided, specifies the ID of the Officer to update, otherwise the Officer will be inserted, and the ID returned in OperationStatus.AffectedIds.<br /> 
        /// Example: 123<br />
        /// </summary>
        public int? OfficerId { get; set; }

        /// <summary>
        /// ID of the Quote that the Officer is assigned to.<br />
        /// QuoteId is initially obtained by calling the QuoteState POST service.<br />
        /// Example: 123<br />
        /// Validation:<br />
        /// 1) QuoteId is required.<br />
        /// </summary>
        [Required]
        public int? QuoteId { get; set; }

        /// <summary>
        /// True, if coverage is to be included for the Officer. 
        /// </summary>
        [Required]
        public bool? Included { get; set; }

        /// <summary>
        /// Business Name<br />
        /// Example: Best Coffee Shop, Inc.
        /// </summary>
        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        /// <summary>
        /// Officer's Title
        /// </summary>
        [Required]
        [StringLength(128)]
        public string Title { get; set; }

        /// <summary>
        /// Officer's Social Security Number
        /// </summary>
        [Required]
        [StringLength(9)]
        public string SSN { get; set; }

        /// <summary>
        /// Officer's home address
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Addr1 { get; set; }

        /// <summary>
        /// Officer's home address
        /// </summary>
        [StringLength(50)]
        public string Addr2 { get; set; }

        /// <summary>
        /// Officer's home address
        /// </summary>
        [Required]
        [StringLength(30)]
        public string City { get; set; }

        /// <summary>
        /// Officer's home address
        /// </summary>
        [Required]
        [StringLength(2)]
        public string State { get; set; }

        /// <summary>
        /// Officer's home address
        /// </summary>
        [Required]
        [StringLength(9)]
        public string Zip { get; set; }

        /// <summary>
        /// Class Code; not required.  Class code associated with Officer's role in the business.
        /// </summary>
        [StringLength(6)]
        public string Class { get; set; }

        /// <summary>
        /// Officer's percent ownership in the business.
        /// </summary>
        [Required]
        public decimal? PercentageOwnership { get; set; }

        /// <summary>
        /// Officer's payroll
        /// </summary>
        [Required]
        public decimal? Payroll { get; set; }
    }
}
