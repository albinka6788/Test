using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Background
{
    public class Lookup
    {
        public string AlternateDescription { get; set; }
        public string Description { get; set; }
        public DateTime? Discontinued { get; set; }
        public DateTime? Effective { get; set; }
        public string Extended { get; set; }
        public string Extended2 { get; set; }
        public string KeyExcept { get; set; }
        public string KeyField { get; set; }
        public int? KeyOrder { get; set; }
        public string LOB { get; set; }
        public string PrimarySystem { get; set; }
        public string ProgramId { get; set; }
        public string SystemKey { get; set; }
        public string Vals { get; set; }
    }
}
