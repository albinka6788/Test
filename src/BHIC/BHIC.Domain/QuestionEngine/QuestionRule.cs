#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace BHIC.Domain.QuestionEngine
{
    /// <summary>
    /// This Class is a combination of BHDQuestionRules and BHDQuestionRule_Values.
    /// Each Question Rule may have multiple Rule Values.  In order to limit the number of
    /// calls back to the server, this model was created as a combination of both tables.
    /// </summary>
    public class QuestionRule
    {
        public QuestionRule() { }

        [Key]
        public int ruleValueId { get; set; }
        public int questionId { get; set; }
        public int ruleId { get; set; }
        public string ruleState { get; set; }
        public string ruleCondition { get; set; }
        public string ruleOperand { get; set; }
        public string ruleQuestionCategory { get; set; }
        public int rulePricing { get; set; }
        /*
         * Values for ResultType:
         * R: Referral
         * D: Decline
         * S: Schedule Rating (Only rules that will have Pricing and QuestionCategory data)
        */
        public string ruleResultType { get; set; }
        public string ruleResultText { get; set; }

        /// <summary>
        /// Current Behavior:<br />
        /// - If "Y", indicates that business rules have been defined for the question, and that the assessment of the those rules did not result in a Referral, Decline, or changes to premium.<br />
        /// - If "N", indicates that business rules have been defined for the question, and that the assessment of the those rules did result in either a Referral, Decline, or changes to premium.<br />
        /// - If not populated, indicates that no business rules that affect premium or Referral/Decline status have been defined for the question. 
        /// </summary>
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

        /// <summary>
        /// Evaluates a QuestionRule record.  Returns True/False value indicating if the
        /// rule condition was met.
        /// </summary>
        /// <param name="_response">Response to validate.</param>
        /// <returns>True or False</returns>
        public bool EvaluateRule(string _response)
        {
            bool _ret = false;

            try
            {
                //EqualTo (=) Condition
                if (ruleCondition.Equals("=") && _response.Equals(ruleOperand))
                    _ret = true;
                //GreaterThan (>) Condition
                else if (ruleCondition.Equals(">") && decimal.Parse(_response) > decimal.Parse(ruleOperand))
                    _ret = true;
                //GreaterThan or EqualTo (>=) Condition
                else if (ruleCondition.Equals(">=") && decimal.Parse(_response) >= decimal.Parse(ruleOperand))
                    _ret = true;
                //LessThan (<) Condition
                else if (ruleCondition.Equals("<") && decimal.Parse(_response) < decimal.Parse(ruleOperand))
                    _ret = true;
                //LessThan or EqualTo (<=) Condition
                else if (ruleCondition.Equals("<=") && decimal.Parse(_response) <= decimal.Parse(ruleOperand))
                    _ret = true;
                //NotEqual (!=) Condition
                else if (ruleCondition.Equals("!=") && !_response.Equals(ruleOperand))
                    _ret = true;
                //Between Condition
                else if (ruleCondition.Equals("B"))
                {
                    //Value will be pipe delimited. (_highlow[0] = low, _highlow[1] = high)
                    string[] _highlow = ruleOperand.Split('|');

                    if (decimal.Parse(_response) >= decimal.Parse(_highlow[0]) && decimal.Parse(_response) <= decimal.Parse(_response))
                        _ret = true;
                }

                return _ret;
            }
            catch (FormatException)
            {
                return false;
            }
        } //end EvaluateRule

    } //end QuestionRule
} //end namespace