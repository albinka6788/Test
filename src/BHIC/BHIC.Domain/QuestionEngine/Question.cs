using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace BHIC.Domain.QuestionEngine
{
    public class Question
    {
        // constructor / initialization
        public Question()
        {
            // init objects to help avoid issues related to null reference exceptions
            QuestionResponseLimitList = new List<QuestionResponseLimit>();

            // default render status to true
            RenderFlag = true;
        }

        // ----------------------------------------
        // question definition 
        // ----------------------------------------

        /// <summary>
        /// Read-only id of the related question
        /// </summary>
        public int questionId { get; set; }

        /// <summary>
        /// Read-only Question Text
        /// </summary>
        public string questionText { get; set; }

        /// <summary>
        /// Read-only display order of the question.
        /// </summary>
        public int sortOrder { get; set; }

        /// <summary>
        /// Read-only type of question.<br />
        /// R - Radio Button<br />
        /// T - Text<br />
        /// N - Numeric<br />
        /// P - Percent<br />
        /// L - List<br />
        /// D - Date<br />
        /// </summary>
        public string QuestionType { get; set; }

        /// <summary>
        /// Range of valid responses available for the question (if defined)<br />
        /// Delimited string, containing available responses.<br />
        /// Represents valid responses for the question. Contains the same information as QuestionResponseLimitList, in character-delimited format.<br />
        /// </summary>
        public string ResponseLimits { get; set; }

        /// <summary>
        /// Range of valid responses available for the question (if defined)<br />
        /// List of QuestionResponseLimit objects.  Contains the same information as ResponseLimits, in object-oriented list format.
        /// </summary>
        // Response dimit definition list (parsed from db's ResponseLimits delimited column )
        public List<QuestionResponseLimit> QuestionResponseLimitList { get; set; }

        /// <summary>
        /// Read-only id of a related question, whose response value will be tested to determine the conditional display of this question.
        /// </summary>
        public int WhenQuestion { get; set; }

        /// <summary>
        /// Read-only condition to test for, when testing the value of the related question.
        /// </summary>
        public string WhenCondition { get; set; }

        /// <summary>
        /// Read-only value to test, when testing the value of the related question.
        /// </summary>
        public string WhenResponse { get; set; }

        /// <summary>
        /// Read-only values representing the results of processing user-responses to questions. Valid values: <br />
        ///  Y: Passes all of the validations.<br />
        ///  D: Hard Decline.<br />
        ///  S: Soft Referral.<br />
        ///  H: Hard Referral.<br />
        /// </summary>
        public string DecisionEngineResponsesValid { get; set; }

        /// <summary>
        /// Read-only message(s) returned as a result of processing the user response.
        /// </summary>
        public string ResultMessages { get; set; }

        /// <summary>
        /// Some questions are hidden under certain circumstances; the value of this flag represents whether or not the question should be visible
        /// </summary>
        public bool RenderFlag { get; set; }

        /// <summary>
        /// User's response to the question.
        /// </summary>
        public string UserResponse { get; set; }

        /// <summary>
        /// Current Behavior:<br />
        /// - If "Y", indicates that business rules have been defined for the question, and that the assessment of the those rules did not result in a Referral, Decline, or changes to premium.<br />
        /// - If "N", indicates that business rules have been defined for the question, and that the assessment of the those rules did result in either a Referral, Decline, or changes to premium.<br />
        /// - If not populated, indicates that no business rules that affect premium or Referral/Decline status have been defined for the question. 
        /// </summary>
        public string CorrectResponse { get; set; }
    }
}
