using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    public class BillingDetails
    {
        public BillingSummary BillingSummary { get; set; }
        public List<FutureBill> FutureBills { get; set; }
        public List<Payment> Payments { get; set; }

        public BillingDetails()
        {
            BillingSummary = new BillingSummary();
            FutureBills = new List<FutureBill>();
            Payments = new List<Payment>();
        }
    }
}
