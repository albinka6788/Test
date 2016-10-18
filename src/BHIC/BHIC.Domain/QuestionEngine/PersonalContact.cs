using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.QuestionEngine
{
    public class PersonalContact
    {
        public bool SameAsContact { get; set; }
        public string Email { get; set; }
        public string ConfirmEmail { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
