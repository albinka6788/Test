using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
	/// <summary>
	/// Parameters associated with the Officers service<br />
	/// Filters the response as indicated by the comments for each parameter
	/// </summary>
	public class OfficerRequestParms
	{
		/// <summary>
		/// return data for specified Quote
		/// </summary>
		public int? QuoteId { get; set; }

		/// <summary>
		/// return data for specified Officer
		/// </summary>
		public int? OfficerId { get; set; }
	}
}
