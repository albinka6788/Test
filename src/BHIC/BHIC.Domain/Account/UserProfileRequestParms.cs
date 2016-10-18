using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Account
{
	/// <summary>
	/// Parameters associated with the UserProfiles service<br />
	/// Filters the response as indicated by the comments for each parameter
	/// </summary>
	public class UserProfileRequestParms
	{
		/// <summary>
		/// return data for specified email address (which served as userid at the time this was created)
		/// </summary>
		public string Email { get; set; }
	}
}
