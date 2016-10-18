#region Using directives

#endregion

using System.ComponentModel.DataAnnotations;
namespace BHIC.Domain.Policy
{
    public class UserPolicyCode
    {
        /// <summary>
        ///ID of the UserPolicyCode associated with the operation.
        ///Read-Only
        ///Example: 123
        /// </summary>
        public int UserPolicyCodeId { get; set; }

        /// <summary>
        ///User ID (Example: email address currently serves as User ID for the Cover Your Business site) 
        /// </summary>
        [Required]
        [StringLength(150)]
        public string UserId { get; set; }

        /// <summary>
        /// Policy Code associated with the user
        /// </summary>

        [Required]
        [StringLength(10)]
        public string PolicyCode { get; set; }
    }
}
