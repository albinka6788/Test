using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Common.LobStatusService
{
    public class LobStatusDTO
    {
        public string StateCode { get; set; }
        public string LineOfBusinessName { get; set; }
        public string Status { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime ExpiryOn { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
    }
}
