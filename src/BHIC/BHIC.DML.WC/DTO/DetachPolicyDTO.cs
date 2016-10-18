using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BHIC.DML.WC.DTO
{
    /// <summary>
    /// DTO to Attach policy with an email id for impersonation
    /// </summary>
    public class DetachPolicyDTO
    {
        /// <summary>
        /// User Email Address
        /// </summary>
        [StringLength(150), Required]
        [EmailAddress]
        public string EmailID { get; set; }

        /// <summary>
        /// Policy Numbers
        /// </summary>
        [Required]
        public List<string> PolicyNumbers { get; set; }
    }
}
