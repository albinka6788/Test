using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Common.Payment
{
    public class PaymentResponseViewModel
    {
        public string PaymentErrorMessage { get; set; }
        public int IsPaymentSuccess { get; set; }
        public string TransactionCode { get; set; }
        public string PaymentURL { get; set; }
    }
}
