using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Account
{
	public class UserProfileResponse
	{
		// ----------------------------------------
		// constructor
		// ----------------------------------------

		public UserProfileResponse()
		{
			// init objects to help avoid issues related to null reference exceptions
			UserProfiles = new List<UserProfile>();
			OperationStatus = new OperationStatus();
		}

		// ----------------------------------------
		// properties
		// ----------------------------------------

		public List<UserProfile> UserProfiles { get; set; }
		public OperationStatus OperationStatus { get; set; }
	}
}
