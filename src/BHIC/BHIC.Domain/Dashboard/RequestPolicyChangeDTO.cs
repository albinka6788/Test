using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Dashboard
{
    public class RequestPolicyChangeDTO
    {
        [Required(ErrorMessage = "Description  is required")]
        public string Description { get; set; }
               
        public string SelectedItem { get; set; }
        
        [Required(ErrorMessage = "Type of change is required")]
        public int SelectedID { get; set; }

        [Required(ErrorMessage = "Effective Date is required")]
        public string Effectivedate { get; set; }

        [Required(ErrorMessage = "Policy Number is required")]
        public string PolicyNumber { get; set; }
    }
}
