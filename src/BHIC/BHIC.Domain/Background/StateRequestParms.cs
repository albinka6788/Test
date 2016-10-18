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
	public class StateRequestParms
	{
		/// <summary>
		/// Return the specified state
		/// </summary>
		public string StateAbbr { get; set; }

		/// <summary>
		/// Return the specified state
		/// </summary>
		public string StateName { get; set; }
	}
}
