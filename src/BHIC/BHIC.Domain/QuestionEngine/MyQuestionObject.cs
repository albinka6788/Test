using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.QuestionEngine
{
    public class MyQuestionObject
    {
        
    }

    public class Numeric
    {
        public int MinVal { get; set; }
        public int MaxVal { get; set; }
    }
    public class Text
    {
        public int MinLen { get; set; }
        public int MaxLen { get; set; }
        public bool SpecialCharAllowed { get; set; }
        public string[] AllowedChars { get; set; }
        public string[] NotAllowedChars { get; set; }
        public enum TextType { Numeric, AlphaNumeric, Aplha }
        public enum TextPattern { Singleline, Multiline }
    }
    public class Radio
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public string GroupName { get; set; }
        public bool Selected { get; set; }
    }
    public class Checkbox
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public bool Selected { get; set; }
    }
    public class List
    {
        public IDictionary<string, string> KeyValue { get; set; }
        public string DefaultText { get; set; }
        public string DefaultValue { get; set; }
    }
}
