using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Dashboard
{
    public class CancellationRequestcustom
    {
        public DateTime RequestEffectiveDate { get; set; }
        public DateTime RequestDate { get; set; }
        public string  ContactIpAddress { get; set; }
        public string ContactEmail { get; set; }
        public string Name { get; set; }       
        public string Phone { get; set; }
        public string ReasonForCancellation { get; set; }
    }
}
