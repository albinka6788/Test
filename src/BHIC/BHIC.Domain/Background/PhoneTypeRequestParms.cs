using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Background
{
	/// <summary>
	/// Parameters associated with the PhoneTypes service<br />
	/// Filters the response as indicated by the comments for each parameter
	/// </summary>
	public class PhoneTypeRequestParms
	{
		/// <summary>
		/// Specifies a specific PhoneType to return.<br />
		/// Return all PhoneTypes if not specfied<br />
		/// </summary>
		public string PhoneTypeId { get; set; }
	}
}
