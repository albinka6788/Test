using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    /// <summary>
    /// Parameters associated with the PolicyDetails service<br />
    /// Filters the response as indicated by the comments for each parameter
    /// </summary>
    public class PolicyDetailsRequestParms
    {
        /// <summary>
        /// Return data for specified Policy<br />
        /// Specify only PolicyCode or PolicyCodeList, not both.<br /> 
        /// </summary>
        [StringLength(10)]
        public string PolicyCode { get; set; }

        /// <summary>
        /// Return data for specified list of Policies<br />
        /// Specify only PolicyCode or PolicyCodeList, not both.<br />
        /// </summary>
        public string PolicyCodeList { get; set; }

        /// <summary>
        /// User ID  (Example: email address currently serves as User ID for the Cover Your Business site)<br />
        /// Validation Performed: Required. The user must have permissions to access the specified PolicyCode(s).
        /// </summary>
        [StringLength(150)]
        [Required]
        public string UserId { get; set; }

        /// <summary>
        /// If true, PolicyDetails.BackEndContacts is populated with contact information from the back-end policy system.
        /// </summary>
        public bool IncludeRelatedBackEndContacts { get; set; }
    }
}