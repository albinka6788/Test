using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
	/// <summary>
	/// Parameters associated with the PaymentTerms service<br />
	/// Filters the response as indicated by the comments for each parameter
	/// </summary>
	public class PaymentTermsRequestParms
	{
		/// <summary>
		/// return data for specified quote
		/// </summary>
		public int? QuoteId { get; set; }

		/// <summary>
		/// specify the payment plan for the quote (used for POST only; returned for GET)
		/// </summary>
		public int? PaymentPlanId { get; set; }		
	}
}
