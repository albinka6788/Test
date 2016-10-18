using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Service
{
	public class ServiceEventLogResponse
	{
		// ----------------------------------------
		// constructor
		// ----------------------------------------

		public ServiceEventLogResponse()
		{
			// init objects to help avoid issues related to null reference exceptions
			ServiceEventLogs = new List<ServiceEventLog>();
			OperationStatus = new OperationStatus();
		}

		// ----------------------------------------
		// properties
		// ----------------------------------------

		public List<ServiceEventLog> ServiceEventLogs { get; set; }
		public OperationStatus OperationStatus { get; set; }
	}
}
