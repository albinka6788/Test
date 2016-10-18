using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Dashboard
{
    public class AddressInformation
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Address1 is required")]
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }
        [Required(ErrorMessage = "State is required")]
        public string State { get; set; }
        [Required(ErrorMessage = "ZipCode is required")]
        public string ZipCode { get; set; }
        //[Required(ErrorMessage = "Additional Information is required")]
        public string Additional { get; set; }
        [Required(ErrorMessage = "PolicyCode is required")]
        public string[] PolicyCode { get; set; }
        public long? UserPhone { get; set; }
        [Required(ErrorMessage="Address Type is required")]
        public string AddressType { get; set; }

    }
}
