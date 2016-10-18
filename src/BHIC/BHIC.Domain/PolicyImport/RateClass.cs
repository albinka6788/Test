using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BHIC.Domain.PolicyImport
{
	public class RateClass
	{
		public string LocID { get; set; }
		public string State { get; set; }
		public string ClassCode { get; set; }
		public double Exposure { get; set; }
	}
}