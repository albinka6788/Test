using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace BHIC.Domain.QuestionEngine
{
    /// <summary>
    /// Initially created to support internet credits.
    /// Represents a UWSchedMod record.
    /// </summary>
    public class PricingMod
    {
        [StringLength(2)]
        public string State { get; set; }

        [StringLength(1)]
        public string PrimeSeq { get; set; }

        [StringLength(3)]
        public string SeqCode { get; set; }

        public int? QuestionKey { get; set; }

        [StringLength(500)]
        public string Reason { get; set; }

        public int? Percentage { get; set; }
    }
}
