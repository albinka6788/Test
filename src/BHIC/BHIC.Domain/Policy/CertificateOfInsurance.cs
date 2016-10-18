using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    public class CertificateOfInsurance
    {
        [StringLength(10)]
        [Required]
        public string PolicyCode { get; set; }          // Code

        public string RequestId { get; set; }           // LocNum
        public string Type { get; set; }                // C or Y
        public string UseLocNum { get; set; }           // Y or N
        public string CertTypeDescription { get; set; } // 
        public DateTime StartDate { get; set; }         // LowDate
        public DateTime EndDate { get; set; }           // HighDate

        [StringLength(100)]
        [Required]
        [RegularExpression(@"^([A-Za-z\s]{1,}[\.]{0,1}[\']{0,1}[A-Za-z\s]{0,}){0,200}$", ErrorMessage = "Name is not valid.")]
        public string Name { get; set; }                // Text

        [StringLength(35)]
        [Required]
        public string Address1 { get; set; }            // Address 1

        [StringLength(35)]
        public string Address2 { get; set; }            // Address 2

        [StringLength(20)]
        [Required]
        public string City { get; set; }                // City

        [StringLength(2)]
        [Required]
        public string State { get; set; }               // State

        [StringLength(10)]
        [Required]
        public string ZipCode { get; set; }             // ZipCode

        [StringLength(255)]
        public string UseLocNumDescription { get; set; }//

        public DateTime RequestDate { get; set; }       // EnteredOn
        //public int? DocumentId { get; set; }            //
        public Document Document { get; set; }          //

        [StringLength(255)]
        [Required]
        public string EmailTo { get; set; }             //

        /// <summary>
        /// User ID  (Example: email address currently serves as User ID for the Cover Your Business site)<br />
        /// Validation Performed: Required. The user must have permissions to access the specified PolicyCode.
        /// </summary>
        [StringLength(150)]
        [Required]
        public string UserId { get; set; }
    }
}
