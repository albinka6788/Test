using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BHIC.Domain.Service
{
	public class ServiceStatusModel
	{
		// ----------------------------------------
		// properties
		// ----------------------------------------

		// identity of ServiceTestEvents row (master record to ServiceResults rows logged by the test)
		public int ServiceTestEventId { get; set; }

		// short name for the test, typically from ServiceTests.ShortName
		public string TestShortName { get; set; }

		// number of iterations that the test is configured for
		public int Iterations { get; set; }

		// status of tests performed
		public bool AllTestsSucceeded { get; set; }
	}
}