using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    /// <summary>
    /// PolicyData associated with a given Quote. (Policy number, policy start date, description of operations... )
    /// </summary>
    public class PolicyData
    {
        // from spec: FullTime, PartTime, PolicyLimits, YearsInBusiness, DescOfOperations

        /// <summary>
        /// ID of the PolicyData associated with the operation.<br />
        /// If provided, specifies the ID of the PolicyData to update, otherwise the PolicyData will be inserted, and the ID returned in OperationStatus.AffectedIds.<br /> 
        /// Example: 123<br />
        /// </summary>
        public int? PolicyDataId { get; set; }

        /// <summary>
        /// ID of the Quote that the PolicyData is assigned to.<br />
        /// Example: 123<br />
        /// Validation:<br />
        /// 1) Required.<br />
        /// </summary>
        [Required]
        public int? QuoteId { get; set; }

        /// <summary>
        /// Policy Number. Populated only if a policy has been issued for the Quote, or if manually updated.<br />
        /// </summary>
        [StringLength(10)]
        public string MgaCode { get; set; }

        /// <summary>
        /// Policy start date.<br />
        /// Validation:<br />
        /// 1) Required.<br />
        /// </summary>
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? InceptionDate { get; set; }

        /// <summary>
        /// Brief description of business. Example: New Housing Construction<br />
        /// Optional.
        /// </summary>
        [StringLength(250)]
        public string DescOfOperations { get; set; }

        /// <summary>
        /// Number of years the company has been in business
        /// </summary>
        public int YearsInBusiness { get; set; }

        /// <summary>
        /// Number of full time employees with the company
        /// </summary>
        public int Fulltime { get; set; }

        /// <summary>
        /// Number of part time employees with the company
        /// </summary>
        public int PartTime { get; set; }

        /// <summary>
        /// Policy Limits
        /// </summary>
        public decimal PolicyLimits { get; set; }

        /// <summary>
        /// Date last updated
        /// </summary>
        public DateTime? DateUpdated { get; set; }
    }
}
