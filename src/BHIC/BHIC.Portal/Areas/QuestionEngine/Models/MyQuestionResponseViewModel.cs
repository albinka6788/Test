using System;
using System.Collections.Generic;

//Comment : Here Other than default library
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using BHIC.Domain.QuestionEngine;


namespace BHIC.Portal.Areas.QuestionEngine.Models
{
    public class MyQuestionResponseViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int QuestionId { get; set; }

        public string QuestionResponseType { get; set; }

        public bool QustionResponsesValid { get; set; }

        [Required(ErrorMessage = "A response is required for this question.")]
        public string ResponseRadio { get; set; }

        [Required(ErrorMessage = "A response is required for this question.")]
        public string ResponseText { get; set; }

        [Required(ErrorMessage = "A response is required for this question.")]
        public int? ResponseNumeric { get; set; }

        [Required(ErrorMessage = "A response is required for this question.")]
        public int ResponsePercent { get; set; }

        [Required(ErrorMessage = "A response is required for this question.")]
        public string ResponseList { get; set; }

        //[Required(ErrorMessage = "A response is required for this question.")]
        [Required(ErrorMessage = "A response is required for this question.")]
        [RegularExpression(@"^([1-9]|0[1-9]|1[0-2])[- / .]([1-9]|0[1-9]|1[0-9]|2[0-9]|3[0-1])[- / .](1[9][0-9][0-9]|2[0][0-9][0-9])$", ErrorMessage = "The date must be in the following format: MM/DD/YYYY")]
        public DateTime? ResponseDate { get; set; }

        [Required(ErrorMessage = "Please accept terms and condition.")]
        public bool AcceptFlag { get; set; }

        public List<MyQuestion> objListQuestion { get; set; }
        public List<MyQuestionResponse> objListQuestionResponse { get; set; }
    }
}