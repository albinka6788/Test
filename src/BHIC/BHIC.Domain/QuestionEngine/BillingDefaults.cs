using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BHIC.Domain.QuestionEngine
{
	// mjl - created to support the display of payment info to the user.
	// for example, FEEINSTALL represents the per installment charge we would need to add to the premium amount to show the total cost
	public class BillingDefaults
	{
		// data returned
		public decimal FEEINSTALL { get; set; }
		public decimal FEEREINS { get; set; }
		public decimal FEENSF { get; set; }
		public decimal FEELATE { get; set; }
		public decimal? feeConvenience { get; set; }
		public decimal NOBILLMIN { get; set; }
		public decimal NOBILLMAX { get; set; }
		public int DAYSDOWN { get; set; }
		public int DAYSPREINCEPT { get; set; }
		public int DAYSDUEDATE { get; set; }
		public int DAYSDUECOMP { get; set; }
		public int DAYSBILLDATE { get; set; }
		public bool FOUND { get; set; }

		// parameters passed
		public string parmLOB { get; set; }
		public string parmCarrier { get; set; }
		public string parmBillType { get; set; }
		public string parmState { get; set; }
		public DateTime parmEffDate { get; set; }
	}
}
