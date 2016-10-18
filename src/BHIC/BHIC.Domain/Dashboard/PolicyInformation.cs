using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Dashboard
{
    public class PolicyInformation
    {
        public string PolicyCode { get; set; }
        public string Status { get; set; }
        public string EmailId { get; set; }
        public bool IsSucess { get; set; }
        public int LOB { get; set; }
        public string BusinessName { get; set; }
        public PolicyUser PolicyUserContact { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
