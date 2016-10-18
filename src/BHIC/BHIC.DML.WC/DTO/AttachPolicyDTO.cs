using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.DML.WC.DTO
{
   /// <summary>
   /// DTO to Detach Policies from secondary account
    public class AttachPolicyDTO
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
        public List<string> PolicyNumbers{ get; set; }

        /// <summary>
        /// Impersonation Start Time
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Impersonation End Time
        /// </summary>
        public DateTime EndTime { get; set; }
    }
}
