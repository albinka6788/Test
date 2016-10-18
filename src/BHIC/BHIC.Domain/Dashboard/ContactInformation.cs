using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Dashboard
{
    public class ContactInformation
    {
        [Required(ErrorMessage = "Email is required.")]
        [MaxLength(100)]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Email is not valid.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [RegularExpression(@"^([A-Za-z\s]{1,}[\.]{0,1}[\']{0,1}[A-Za-z\s]{0,}){0,100}$", ErrorMessage = "Name is not valid.")]
        public string Name { get; set; }              
        [Required(ErrorMessage = "Phone Number is required.")]
        [RegularExpression(@"^.{10,}$", ErrorMessage = "Phone Number is not valid.")]
        public Int64 PhoneNumber { get; set; }
        [Required]
        public string PolicyCode { get; set; }
        [Required]
        public string ContactId { get; set; }
        [Required]
        public string QuoteId { get; set; }
        [Required]
        public string PhoneId { get; set; }
        [Required]
        public string PhoneType { get; set; }
    }
}
