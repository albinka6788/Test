using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.QuestionEngine
{
    public class ClassItem
    {
        public string StateAbbr { get; set; }
        public string ZipCode { get; set; }
        public string ClassCode { get; set; }
        public string ClasSuffix { get; set; }
        public decimal Exposure { get; set; }
        public int classDescriptionId { get; set; }
    }
}
