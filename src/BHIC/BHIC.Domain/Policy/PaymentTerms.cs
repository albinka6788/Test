using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    public class PaymentTerms
    {
        // from spec: { DownPayment, Installments, Frequency }

        /// <summary>
        /// ID of the Quote that the PaymentTerms is assigned to.<br />
        /// Example: 123<br />
        /// Validation:<br />
        /// 1) Required.<br />
        /// </summary>
        public int? QuoteId { get; set; }


        /// <summary>
        /// ID of the Payment Plan assigned to the Quote.<br />
        /// Valid values are available via the PaymentPlans service.<br />		
        /// Validation:<br />
        /// 1) Required.<br />
        /// </summary>
        public int? PaymentPlanId { get; set; }

        /// <summary>
        /// Read-only down-payment value.  Accessible via PaymentTerms GET service
        /// </summary>
        public decimal DownPayment { get; set; }

        /// <summary>
        /// Read-only number of installments value.  Accessible via PaymentTerms GET service
        /// </summary>
        public int Installments { get; set; }
        public decimal? InstallmentAmount { get; set; }

        /// <summary>
        /// Read-only down-payment value.  Accessible via PaymentTerms GET service.  Calculated by the system as a part of the Decision Engine / Question Engine logic.
        /// </summary>
        public decimal InstallmentFee { get; set; }

        /// <summary>
        /// Read-only short name for frequency of installments.  See FrequencyCode.  Accessible via PaymentTerms GET service
        /// </summary>
        [StringLength(50)]
        public string Frequency { get; set; }					// E,A,Q,W,M,S,B,N,

        /// <summary>
        /// Read-only code for frequency of installments. <br />
        /// E	Bi-Monthly Installment<br />
        /// A	Annual Billing<br />
        /// Q	Quarterly Installments<br />
        /// W	Weekly Installments<br />
        /// M	Monthly Installments<br />
        /// S	Semi-Annual Installment<br />
        /// B	Bi-Weekly Installments<br />
        /// N	Semi-Monthly Installments<br />
        /// </summary>
        [StringLength(1)]
        public string FrequencyCode { get; set; }
    }
}
