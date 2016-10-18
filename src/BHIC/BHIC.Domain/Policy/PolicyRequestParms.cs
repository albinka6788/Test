using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    /// <summary>
    /// Parameters associated with the Policy service<br />
    /// Filters the response as indicated by the comments for each parameter
    /// </summary>
    public class PolicyRequestParms
    {
        /// <summary>
        /// return data for specified Policy
        /// </summary>
        [StringLength(10)]
        public string PolicyCode { get; set; }

        /// <summary>
        /// return certificates for specified session ID. Required.
        /// </summary>
        [StringLength(255)]
        public string SessionId { get; set; }

        /// <summary>
        /// User ID  (Example: email address currently serves as User ID for the Cover Your Business site)<br />
        /// Validation Performed: Required. The user must have permissions to access the specified PolicyCode.
        /// </summary>
        [StringLength(150)]
        [Required]
        public string UserId { get; set; }
    }
}