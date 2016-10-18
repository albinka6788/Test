using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.PolicyCentre
{
    public class UserQuote
    {
        public int ID { get; set; }
        public string QuoteID { get; set; }
        public string EncryptedQuoteID { get; set; }
        public decimal? PremiumAmt { get; set; }
        public string LineOfBusiness { get; set; }
        public string ZipCode { get; set; }
        public DateTime? InceptionDate { get; set; }
        public DateTime? QuotedDate { get; set; }
        public string BusinessType { get; set; }
        public string RetrieveQuoteURL { get; set; }
        public int LineOfBusinessId { get; set; }
        public int? ClassDescriptionKeywordId { get; set; }

        public UserQuote()
        {
            PremiumAmt = 0;
            ZipCode = "-";
            BusinessType = "-";
        }
    }
}
