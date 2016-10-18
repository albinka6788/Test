using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BHIC.Domain.Service
{
	public class ServiceEventLog
	{
		// logged to ServiceTestResult.TestType, and ServiceTestEvent.TestType
		public enum TestTypeVal
		{
			Individual = 1,		// Test Harness - individual service test
			UserStory = 2,		// Test Harness - user story test
			StressTest = 3,		// Test Harness - stress testing
			Available = 4,		// Test Harness - availability testing
			Activity = 5		// Activity logging... not related to test harness
		}

		// logged to ServiceTestResult.TestMessageType
		public enum TestMessageTypeVal
		{
			Ok = 1,			// no error condition
			Info = 2,		// no error condition; informational
			SvcError = 3,	// the service reported an error condition
			TstError = 4,	// the service executed without error, but ServiceTestLibrary is reporting an issue with the response
			Activity = 5	// activity log entry
		}

		[Required]
		public int? ServiceEventLogId { get; set; }

		[Required]
		public int ServiceTestEventId { get; set; }
		
		[StringLength(25)]
		[Required]
		public string TestType { get; set; }

		[StringLength(50)]
		[Required]
		public string TestShortName { get; set; }

		[StringLength(100)]
		[Required]
		public string RequestIdentifier { get; set; }

		[StringLength(100)]
		[Required]
		public string ServiceName { get; set; }

		[StringLength(10)]
		[Required]
		public string ServiceMethod { get; set; }

		[Required]
		public DateTime RequestTimestamp { get; set; }

		[Required]
		public DateTime ResponseTimestamp { get; set; }

		[Required]
		public string RequestJSON { get; set; }

		[Required]
		public string ResponseJSON { get; set; }

		[Required]
		public bool RequestProcessed { get; set; }

		[Required]
		public bool RequestSuccessful { get; set; }

		[StringLength(250)]
		public string TestMessage { get; set; }

		[StringLength(10)]
		public string TestMessageType { get; set; }

		[StringLength(50)]
		public string UserIP { get; set; }

		public int? RecordKey { get; set; }
	}
}