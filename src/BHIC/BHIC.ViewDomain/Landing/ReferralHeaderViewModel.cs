using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.ViewDomain.Landing
{
    public class ReferralHeaderViewModel
    {
        public string BusinessHours { get; set; }
    }

    public class MultiStateReferralHeader
    {
        public string PhoneNumber_CSR { get; set; }
        public string PhoneNumber_CSR_Tel { get; set; }
    }
}
