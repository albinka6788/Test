using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Background
{
	/// <summary>
	/// Parameters associated with the LocationTypes service<br />
	/// Filters the response as indicated by the comments for each parameter
	/// </summary>
	public class LocationTypeRequestParms
	{
		/// <summary>
		/// Specifies a specific LocationType to return.<br />
		/// Return all LocationTypes if not specfied<br />
		/// </summary>
		public string LocationTypeId { get; set; }
	}
}
