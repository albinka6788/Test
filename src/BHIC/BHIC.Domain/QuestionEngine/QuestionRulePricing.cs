using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.QuestionEngine
{
    /// <summary>
    /// Represents the Pricing Matrix that will be used to determine the 
    /// pricing added to the quote.
    /// </summary>
    public class QuestionRulePricing
    {
        public QuestionRulePricing() { }

        [Key]
        public int id { get; set; }
        public string state { get; set; }
        public string questionCategory { get; set; }
        public string questionCategoryDescrip { get; set; }
        public int allowedMin { get; set; }
        public int allowedMax { get; set; }
        public int questionKey { get; set; }
        public int desiredValue { get; set; }
        public int cappedValue { get; set; }
        public int adjustedValue { get; set; }
        public string primeseq { get; set; }
        public string seqcode { get; set; }
        public string reason { get; set; }

    } //end QuestionRulePricing
} //end namespace
