using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Demo
{
    public class User
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Phonenumber { get; set; }
        public string Policycode { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int UserID { get; set; }
    }
}
