using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
	/// <summary>
	/// Parameters associated with the Addresses service<br />
	/// Filters the response as indicated by the comments for each parameter
	/// </summary>
	public class AddressRequestParms
	{
		/// <summary>
		/// return Addresses for the specified Quote
		/// </summary>
		public int? QuoteId { get; set; }

		/// <summary>
		/// return Addresses for the specified Contact
		/// </summary>
		public int? ContactId { get; set; }

		/// <summary>
		/// return the specified Address
		/// </summary>
		public int? AddressId { get; set; }
	}
}
