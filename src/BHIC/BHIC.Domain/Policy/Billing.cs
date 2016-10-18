using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    public class Billing
    {
        public BillingDetails BillingDetails { get; set; }
        public List<Document> BillingStatements { get; set; }
        public List<Document> DeductibleInvoices { get; set; }

        public Billing()
        {
            BillingDetails = new BillingDetails();
            BillingStatements = new List<Document>();
            DeductibleInvoices = new List<Document>();
        }
    }
}
