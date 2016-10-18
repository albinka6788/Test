using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BHIC.Domain.QuestionEngine
{
	public class DSQuestionRule
	{
		public int questionId { get; set; }
		public int ruleId { get; set; }
		public string ruleCondition { get; set; }
		public string ruleOperand { get; set; }
		public string ruleResultType { get; set; }
		public string ruleResultText { get; set; }
		public string correctResponse { get; set; }
		/*
		 * Values for DecisionEngineResponsesValid are as follows: 
		 *  Y: Passes all of the validations.
		 *  D: Hard Decline.
		 *  S: Soft Referral.
		 *  H: Hard Referral.
		 */
		public string DecisionEngineResponsesValid { get; set; }
		public string ResultMessages { get; set; }
	}
}
