using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    public class BillingSummary
    {
        public decimal CurrentDue { get; set; }
        public string CurrentDueDate { get; set; }
        public decimal PastDue { get; set; }
        public string PastDueDate { get; set; }
        public decimal AccountBalance { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal LastPaidAmount { get; set; }

        [Key]
        public string LastPaidDate { get; set; }
        public decimal ConvenienceFee { get; set; }

        public List<BHIC.Domain.Document.Document> DeductibleInvoices { get; set; }
        public List<BHIC.Domain.Document.Document> BillingStatements { get; set; }

        public List<FutureBill> FutureBills { get; set; }
        public List<Payment> Payments { get; set; }

        public BillingSummary()
        {
            DeductibleInvoices = new List<BHIC.Domain.Document.Document>();
            BillingStatements = new List<BHIC.Domain.Document.Document>();

            FutureBills = new List<FutureBill>();
            Payments = new List<Payment>();
        }

        public BillingSummary(decimal currentDue, string currentDueDate, decimal pastDue, string pastDueDate, decimal accountBalance, decimal totalPaid, decimal lastPaidAmount, string lastPaidDate, decimal convenienceFee)
        {
            DeductibleInvoices = new List<BHIC.Domain.Document.Document>();
            BillingStatements = new List<BHIC.Domain.Document.Document>();

            FutureBills = new List<FutureBill>();
            Payments = new List<Payment>();

            CurrentDue = currentDue;
            CurrentDueDate = currentDueDate;
            PastDue = pastDue;
            PastDueDate = pastDueDate;
            AccountBalance = accountBalance;
            TotalPaid = totalPaid;
            LastPaidAmount = lastPaidAmount;
            LastPaidDate = lastPaidDate;
            ConvenienceFee = convenienceFee;
        }
    }
}
