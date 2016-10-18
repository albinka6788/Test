﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    public class Payment
    {
        [Key]
        public DateTime PaymentDate { get; set; }

        [StringLength(255)]
        public string CheckNumber { get; set; }

        public decimal Amount { get; set; }

        [StringLength(10)]
        public string AgencyCode { get; set; }

        [StringLength(255)]
        public string PolicyCode { get; set; }

        /// <summary>
        /// User ID  (Example: email address currently serves as User ID for the Cover Your Business site)<br />
        /// Validation Performed: Required. The user must have permissions to access the specified PolicyCode.
        /// </summary>
        [StringLength(150)]
        [Required]
        public string UserId { get; set; }
    }
}
