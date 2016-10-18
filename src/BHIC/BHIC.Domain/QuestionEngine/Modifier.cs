using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BHIC.Domain.QuestionEngine
{
	public class Modifier
	{
		public string State { get; set; }
		public decimal Rate { get; set; }

		public string PrimeSeq { get; set; }		// added 3/19/2015 in support of rating engine call, and for future support of different mod types
		public string SeqCode { get; set; }			// added 3/19/2015 in support of rating engine call, and for future support of different mod types
		public string Class { get; set; }			// added 3/19/2015 in support of rating engine call, and for future support of different mod types
		public string ClassCode { get; set; }		// added 3/19/2015 in support of rating engine call, and for future support of different mod types
		public string ClassSuffix { get; set; }		// added 3/19/2015 in support of rating engine call, and for future support of different mod types

	}
}
