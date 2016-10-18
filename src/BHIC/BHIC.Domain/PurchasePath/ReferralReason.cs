using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BHIC.Domain.PurchasePath
{
    [Serializable]
    public class ReferralReasons
    {
        [XmlElement]
        public List<ReferralReason> ReferralReason { get; set; }
    }

    public class ReferralReason
    {
        [XmlAttribute]
        public int ScenarioId { get; set; }

        [XmlAttribute]
        public string ReferrarPage { get; set; }

        [XmlAttribute]
        public string ReferralReasonText { get; set; }

        [XmlAttribute]
        public string ReferralDescription { get; set; }

        [XmlAttribute]
        public bool IsActive { get; set; }
    }
}
