using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Account
{
    public class Login
    {
        public string Email { get; set; }
        public string Password { get; set; } // The existing password will never be returned.
    }
}