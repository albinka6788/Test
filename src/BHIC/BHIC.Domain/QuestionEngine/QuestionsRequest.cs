using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Policy;

namespace BHIC.Domain.QuestionEngine
{
    public class QuestionsRequest
    {
        public QuestionsRequest()
        {
            ClassItems = new List<ClassItem>();
            Modifiers = new List<Modifier>();
        }

        //public string UserId { get; set; }
        //public string Password { get; set; }
        public string Carrier { get; set; }
        public string GovStateAbbr { get; set; }
        public string Agency { get; set; }
        //public decimal Exposure { get; set; }
        //public string IndustryCode { get; set; }
        //public string ClassCode { get; set; }
        public DateTime EffDate { get; set; }
        public decimal Premium { get; set; }
        public List<Modifier> Modifiers { get; set; }
        public List<ClassItem> ClassItems { get; set; }
        public string PrimaryCaMailingZip { get; set; }		// required for rating engine call; not used by the question engine at the time this was added
        public int? QuoteId { get; set; }
    }
}
