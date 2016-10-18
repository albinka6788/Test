using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    /// <summary>
    /// UserPolicyCodeRequestParms
    /// </summary>
    public class UserPolicyCodeRequestParms
    {
        /// <summary>
        /// UserPolicyCodeId
        /// </summary>
        public int? UserPolicyCodeId { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// PolicyCode
        /// </summary>
        public string PolicyCode { get; set; }
    }
}
