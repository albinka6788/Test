using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BHIC.Domain.PolicyImport
{
	public class Question
	{
		public string MGACode { get; set; }
		public int Question_ID { get; set; }
		public string QResponse { get; set; }
		public string CorrectResponse { get; set; }
		public int DE_Class_Rules_ID { get; set; }
	}
}