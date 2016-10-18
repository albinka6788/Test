using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Background
{
	/// <summary>
	/// Parameters associated with the ContactTypes service<br />
	/// Filters the response as indicated by the comments for each parameter
	/// </summary>
	public class ContactTypeRequestParms
	{
		/// <summary>
		/// Specifies a specific ContactType within the ContactGroup to return.<br />
		/// Return all ContactTypes within the ContactGroup if not specfied<br />
		/// </summary>
		public string ContactTypeId { get; set; }

		/// <summary>
		/// Return the ContactTypes associated with the specified ContactGroup<br />
		/// Currently supported ContactGroup values: "PolicyHolder"
		/// </summary>
		[Required]
		public string ContactGroup { get; set; }
	}
}
