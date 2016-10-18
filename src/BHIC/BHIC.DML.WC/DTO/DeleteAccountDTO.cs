using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.DML.WC.DTO
{
    /// <summary>
    /// Delete User Account
    /// </summary>
   public class DeleteAccountDTO
    {
        /// <summary>
        /// User Email Address, using this Email ID, user can login into Policy Center
        /// </summary>
        [StringLength(150), Required]
        [EmailAddress]
        public string EmailID { get; set; }
    }
}
