using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Account
{
    public class UserProfile
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string NewEmail { get; set; }
        public string PolicyCode { get; set; }
        public string Password { get; set; }	// existing password will never be returned here
        public string NewPassword { get; set; }	// existing password will never be returned here
        public UserProfilePostType UserProfilePostType { get; set; }
    }
}
