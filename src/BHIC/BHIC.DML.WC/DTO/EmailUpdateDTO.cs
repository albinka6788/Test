using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.DML.WC.DTO
{
    /// <summary>
    /// Merge and Change Policy EmailID
    /// </summary>
    public class EmailUpdateDTO
    {
        /// <summary>
        /// Old Email ID 
        /// </summary>
        [StringLength(150), Required]
        [EmailAddress]
        public string OldEmailID { get; set; }

        /// <summary>
        /// New Email ID
        /// </summary>
        [StringLength(150), Required]
        [EmailAddress]
        public string NewEmailID { get; set; }
    }

    public class EmailDTO
    {

    }
}
