using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Background
{
    public class PaymentPlan
    {
        /// <summary>
        /// Plan ID
        /// </summary>
        public int PaymentPlanId { get; set; }

        /// <summary>
        /// The finance plan. 
        /// (A)gengy Bill
        /// (D)irect Bill
        /// (T)-Bill (PAYCHEX payroll bill)
        /// </summary>
        public string Fplan { get; set; }

        /// <summary>
        /// Description (e.g. - 25% Down + 2 Monthly Installments)
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Plan is valid for the minimum premium amount specified here.
        /// 0 for any, or a minimun premium level.
        /// </summary>
        public decimal MinPrem { get; set; }

        /// <summary>
        /// Down payment type.
        /// N - No down payment
        /// $ - Dollar amount down
        /// % - percentage down.
        /// </summary>
        public string DownType { get; set; }

        /// <summary>
        /// Percentage or dollar amount down. i.e. if DownType='%' and Down=25, then it means a 25% down payment. 
        /// </summary>
        public int Down { get; set; }

        /// <summary>
        /// The number of installments (not including the down payment)
        /// </summary>
        public int Pays { get; set; }

        /// <summary>
        /// How often to bill installments? 
        /// (M)onthly, (Q)uarterly, (A)nnualy, (S)emi-annually
        /// </summary>
        public string Freq { get; set; }

        /// <summary>
        /// Reserved for internal Insurance Service use only.
        /// </summary>
        [StringLength(10)]
        public string FplanExt { get; set; }

    }
}
