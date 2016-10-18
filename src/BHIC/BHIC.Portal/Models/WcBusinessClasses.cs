using BHIC.Domain.Background;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace BHIC.Portal.Models
{
    /// <summary>  
    /// This class is being serialized to XML.  
    /// </summary>  
    [Serializable]
    [XmlRoot("WcBusinessClasses"), XmlType("WcBusinessClasses")]  
    public class WcBusinessClasses
    {
        public string HtmlsRootFolder { get; set; }

        [XmlElement("WcBusinessClass")]
        public WcBusinessClass WcBusinessClass { get; set; }

    }

    public class WcBusinessClass
    {
        [XmlAttribute("KeywordId")]
        public int BusinessClassKeywordId { get; set; }

        [XmlAttribute("KeywordDesc")]
        public string BusinessClassKeywordDesc { get; set; }

        [XmlElement("Industry")]
        public WcBcIndustry Industry { get; set; }

        [XmlElement("SubIndustry")]
        public WcBcSubIndustry SubIndustry { get; set; }

        [XmlElement("ClassDescription")]
        public WcBcClassDescription ClassDescription { get; set; }

        [XmlElement("HtmlFileName")]
        public string HtmlFileName { get; set; }

        [XmlElement("HtmlRootfolder",IsNullable = false)]
        public string HtmlRootfolder { get; set; }
    }

    /// <summary>
    /// WorkerCompnesation "Business Class" Industry details
    /// </summary>
    public class WcBcIndustry
    {
        [XmlAttribute("Id")]
        public int IndustryId { get; set; }

        [XmlAttribute("Value")]
        public string Description { get; set; }
    }

    /// <summary>
    /// WorkerCompnesation "Business Class" SubIndustry details
    /// </summary>
    public class WcBcSubIndustry
    {
        [XmlAttribute("Id")]
        public int SubIndustryId { get; set; }

        [XmlAttribute("Value")]
        public string Description { get; set; }
    }

    /// <summary>
    /// WorkerCompnesation "Business Class" ClassDescription details
    /// </summary>
    public class WcBcClassDescription
    {
        [XmlAttribute("Id")]
        public int ClassDescriptionId { get; set; }

        [XmlAttribute("Value")]
        public string Description { get; set; }
    }
}