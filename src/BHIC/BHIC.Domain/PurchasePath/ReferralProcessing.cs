using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BHIC.Domain.PurchasePath
{
    public class ReferralProcessing
    {
        public List<int> ReferralScenarioIds { get; set; }
        public string FilePath { get; set; }
        public List<string> ReasonsList { get; set; }
        public List<string> DescriptionList { get; set; }
        public string XModValueMessage { get; set; }
    }
}
