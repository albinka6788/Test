//Comment : Here Other than default library

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using BHIC.Core.QuestionEngine;
using BHIC.Domain.QuestionEngine;

namespace BHIC.Portal.Areas.QuestionEngine.Models
{
    public class MyQuestionsViewModel
    {
        #region Comment : Here Question common properties

        [DisplayName("Sr. No.")]
        public int QuestionSrNo { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int QuestionId { get; set; }

        [DisplayName("Question Type")]
        [MaxLength(100)]
        public string QuestionType { get; set; }

        [DisplayName("Question Text")]
        public string QuestionText { get; set; }

        public int QuestionOrder { get; set; }

        public string QuestionDescription { get; set; }

        [DisplayName("Question ToolTip")]
        [MaxLength(250)]
        public string QuestionToolTip { get; set; }

        #endregion

        #region Comment : Here QuestionType properies

        [DisplayName("Numeric Type")]
        [Range(1, 500, ErrorMessage = "Numeric value acceptable range.")]
        [Required(ErrorMessage = "Response is required.")]        
        public string TypeNumeric { get; set; }

        [DisplayName("Textbox Type")]
        [MaxLength(300,ErrorMessage="This field response can contains max 300 characters.")]
        [Required(ErrorMessage = "Response is required.")]
        [DataType(DataType.Text)]
        public string TypeText { get; set; }

        [DisplayName("Multiline Textbox Type")]
        [MaxLength(500)]
        [Required(ErrorMessage = "Response is required.")]
        [DataType(DataType.MultilineText)]
        public string TypeTextParagraph { get; set; }

        [DisplayName("Radio Type")]
        [Required(ErrorMessage = "Response is required.")]
        public string TypeRadio { get; set; }

        [DisplayName("Checkbox Type")]
        [Required(ErrorMessage = "Response is required.")]
        public bool TypeCheckbox { get; set; }

        [DisplayName("Percentage Type")]
        [Required(ErrorMessage = "Response is required.")]
        [Range(1, 100, ErrorMessage = "Percentage value acceptable range.")]
        //[ReadOnly(true)]
        public string TypePercentage { get; set; }

        [DisplayName("List Type")]
        [Required(ErrorMessage = "Response is required.")]
        public string TypeList { get; set; }

        #endregion

        #region Comment : Here Question other properties

        [Required(ErrorMessage = "Acceptance is required.")]

        public bool AcceptFlag { get; set; }

        #endregion

        //Comment : Here list object
        public List<MyQuestion> QuestionsList { get; set; }
    }
}
