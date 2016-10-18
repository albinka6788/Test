//Comment : Here Other than default library
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BHIC.Domain.QuestionEngine
{
    //public class QuestionType
    //{
    //    #region Comment : Here class main properies

    //    public string TypeNumeric { get; set; }
    //    public string TypeText { get; set; }
    //    public string TypeTextParagraph { get; set; }
    //    public string TypeRadio { get; set; }
    //    public string TypeCheckbox { get; set; }
    //    public string TypePercentage { get; set; }
    //    public string TypeList { get; set; }

    //    #endregion
    //}

    public enum MyQuestionType
    {
         Numeric
        ,Text
        ,TextParagraph
        ,Radio
        ,Checkbox
        ,Percentage
        ,List
    }
}
