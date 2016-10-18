using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Background
{
    public class FieldLabel
    {
        public int LabelId { get; set; }
        public FieldLabelType LabelType { get; set; }
        public string Label { get; set; }
        public DateTime UpdateOn { get; set; }
    }
}