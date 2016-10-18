using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Background
{
	public class State
	{
		public string StateAbbr { get; set; }
		public string StateName { get; set; }
        public bool EmployerCodeRequired { get; set; }
	}
}
