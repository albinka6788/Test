using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Background
{
    public class FieldData
    {
        public string Table { get; set; }
        public string Field { get; set; }
        public string FriendlyName { get; set; }
        public List<FieldLabel> FieldLabels { get; set; }

        public FieldData()
        {
            FieldLabels = new List<FieldLabel>();
        }
    }
}