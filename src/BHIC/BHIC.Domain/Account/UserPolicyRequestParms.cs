using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Account
{
    /// <summary>
    /// Parameters associated with the UserPolicies service<br />
    /// Filters the response as indicated by the comments for each parameter
    /// </summary>
    public class UserPolicyRequestParms
    {
        /// <summary>
        /// return data for specified user policy id
        /// </summary>
        public int UserPolicyId { get; set; }

        /// <summary>
        /// return data for specified email address (which served as userid at the time this was created)
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// policy code used to validate user access
        /// </summary>
        public string PolicyCode { get; set; }
    }
}
