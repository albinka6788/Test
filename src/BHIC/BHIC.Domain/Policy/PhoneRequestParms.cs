using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
	/// <summary>
	/// Parameters associated with the Phones service<br />
	/// Filters the response as indicated by the comments for each parameter
	/// </summary>
	public class PhoneRequestParms
	{
		/// <summary>
		/// return contacts for specified Quote
		/// </summary>
		public int? QuoteId { get; set; }

		/// <summary>
		/// return data for specified Contact
		/// </summary>
		public int? ContactId { get; set; }

		/// <summary>
		/// return data for specified Phone
		/// </summary>
		public int? PhoneId { get; set; }
	}
}
