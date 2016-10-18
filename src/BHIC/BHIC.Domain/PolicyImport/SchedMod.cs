using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.PolicyImport
{
	public class SchedMod
	{
		public string State { get; set; }
		public int Questionkey { get; set; }
		public double Percentage { get; set; }
		public string Reason { get; set; }
	}
}
