using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Policy
{
	public class LobDataResponse
	{
		// ----------------------------------------
		// constructor
		// ----------------------------------------

		public LobDataResponse()
		{
			// init objects to help avoid issues related to null reference exceptions
			LobDataList = new List<LobData>();
			OperationStatus = new OperationStatus();
		}

		// ----------------------------------------
		// properties
		// ----------------------------------------

		public List<LobData> LobDataList { get; set; }
		public OperationStatus OperationStatus { get; set; }
	}
}
