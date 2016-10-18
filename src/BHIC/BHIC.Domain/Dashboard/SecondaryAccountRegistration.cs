using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Dashboard
{
    public class SecondaryAccountRegistration
    {
        public Int32 Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string OrganizationName { get; set; }
        [Required]
        [MaxLength(100)]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Email is not valid")]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^(?=.{8,})(?=.*?[A-Z])(?=.*?[a-z])(?=.*?\d)(?=.*?\W).*$", ErrorMessage = "Password is not valid")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public bool isActive { get; set; }
        public Int32 CreatedBy { get; set; }
        public Int32 ModifiedBy { get; set; }
        [Required]
        [MaxLength(50)]
        [RegularExpression(@"^([A-Za-z\s]{1,}[\.]{0,1}[\']{0,1}[A-Za-z\s]{0,}){0,50}$", ErrorMessage = "First Name is not valid.")]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        [RegularExpression(@"^([A-Za-z\s]{1,}[\.]{0,1}[\']{0,1}[A-Za-z\s]{0,}){0,50}$", ErrorMessage = "Last Name is not valid.")]
        public string LastName { get; set; }
        
        [MaxLength(10)]
        [RegularExpression(@"^[a-zA-Z0-9]*$")]
        public string PolicyCode { get; set; }
        [Required(ErrorMessage = "Phone Number is required")]
        [RegularExpression(@"^.{10,}$", ErrorMessage = "Phone Number is not valid")]
        public long PhoneNumber { get; set; }

        [Required(ErrorMessage = "isEmailVerified is required.")]
        public bool isEmailVerified { get; set; }
        public Byte UserRoleId { get; set; }
    }
}
