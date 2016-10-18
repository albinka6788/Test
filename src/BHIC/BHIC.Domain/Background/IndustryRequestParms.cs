using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Background
{
	/// <summary>
	/// Parameters associated with the Industries service<br />
	/// Filters the response as indicated by the comments for each parameter
	/// </summary>
	public class IndustryRequestParms
	{
		/// <summary>
		/// return data for specified Lob (WC, BP, CA....)
		/// </summary>
		public string Lob { get; set; }

		/// <summary>
		/// return the specified Industry
		/// </summary>
		public int? IndustryId { get; set; }
	}
}
