using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Background
{
	/// <summary>
	/// Parameters associated with the States service<br />
	/// Filters the response as indicated by the comments for each parameter
	/// </summary>
	public class ClassCodeRequestParms
	{
		/// <summary>
		/// Return data for the specified state
		/// </summary>
		public string StateAbbr { get; set; }

		/// <summary>
		/// Return data for the specified ClassCode
		/// </summary>
		public string Code { get; set; }

		/// <summary>
		/// Return data for the specified ClassSuffix
		/// </summary>
		public string ClassSuffix { get; set; }

	}
}
