using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Dashboard
{
    public class ReportClaim
    {
        // public Int32 Id { get; set; }
        [Required(ErrorMessage = "Name Of Business is required")]
        [MaxLength(200)]
        //[RegularExpression(@"^([A-Za-z\s]{1,}[\.]{0,1}[\']{0,1}[A-Za-z\s]{0,}){0,200}$", ErrorMessage = "Business name is not valid.")]
        public string NameOfBusiness { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [RegularExpression(@"^.{10,}$", ErrorMessage = "Phone Number is not valid")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "PolicyCode is required")]
        //[MaxLength(10)]
        //[RegularExpression(@"^[a-zA-Z0-9]*$")] 
        public string policyCode { get; set; }

        [Required(ErrorMessage = "Name Of Injured Worker is required")]
        [RegularExpression(@"^([A-Za-z\s]{1,}[\.]{0,1}[\']{0,1}[A-Za-z\s]{0,}){0,100}$", ErrorMessage = "Injured worker name is not valid.")]
        public string NameOfInjuredWorker { get; set; }

        [Required(ErrorMessage = "Date Of Illness is required")]
        public DateTime dateOfIllness { get; set; }

        [Required(ErrorMessage = "Date Of Illness is required")]
        public string EffectiveDate { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public string location { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string description { get; set; }

        public string YourName { get; set; }
        public string UserEmail { get; set; }
        public long? UserPhone { get; set; }
        public string ClaimType { get; set; }

    }
}
