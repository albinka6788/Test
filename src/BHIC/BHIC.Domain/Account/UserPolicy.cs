using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Account
{
    public class UserPolicy
    {
        public int UserPolicyId { get; set; }
        public string EmailAddress { get; set; }
        public string PolicyCode { get; set; }
        public bool IsDefault { get; set; }
    }
}
