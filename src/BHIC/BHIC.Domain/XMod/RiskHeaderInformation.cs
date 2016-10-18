using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.XMod
{
    public class RiskHeaderInformation
    {
        public int ResponseCode { get; set; }
        public List<string> ResponseMessage { get; set; }
        public string RiskIdNumber { get; set; }
        public string FederalEmployerIdentificationNumber { get; set; }
        public string NameOfInsured { get; set; }
    }
}
