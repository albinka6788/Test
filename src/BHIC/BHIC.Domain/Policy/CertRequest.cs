using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
	public class CertRequest
	{
		public int CertRequestId { get; set; }

		public string PolicyId { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public string Name { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string ZipCode { get; set; }
		public DateTime? RequestDate { get; set; }
		public string ContactIpAddress { get; set; }
		public string ContactEmail { get; set; }
		public string ContactPhoneNumber { get; set; }
		public string ContactName { get; set; }
	}
}
