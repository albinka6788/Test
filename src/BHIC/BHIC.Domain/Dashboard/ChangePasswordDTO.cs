using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Dashboard
{
    public class ChangePasswordDTO
    {   
        [Required(ErrorMessage = "Old password is required")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "New password is required")]
        [RegularExpression(@"^(?=.{8,})(?=.*?[A-Z])(?=.*?[a-z])(?=.*?\d)(?=.*?\W).*$", ErrorMessage = "New password is not valid")]        
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }

        public SecureString SecureOldPassword { get; set; }
        public SecureString SecureNewPassword { get; set; } 
    }
}
