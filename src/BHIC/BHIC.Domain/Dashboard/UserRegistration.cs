using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script;
using System.Web.Script.Serialization;

namespace BHIC.Domain.Dashboard
{
    public class UserRegistration
    {
        public Int32 Id { get; set; }
        public string OrganizationName { get; set; }
        public string Email { get; set; }       
        public string Password { get; set; }
        public bool isActive { get; set; }
        [ScriptIgnore]
        public Int32 CreatedBy { get; set; }
        [ScriptIgnore]
        public Int32 ModifiedBy { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [ScriptIgnore]
        public string PolicyCode { get; set; }
        public long? PhoneNumber { get; set; }
        public bool isEmailVerified { get; set; }        
        public string ResponseMessage { get; set; }
        public SecureString SecurePassword { get; set; } 
    }
}
