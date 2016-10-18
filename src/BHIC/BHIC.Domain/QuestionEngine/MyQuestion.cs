using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.QuestionEngine
{
    public class MyQuestion
    {
        #region Constructor / Initialization

        public MyQuestion()
        {
            //logic for implementation
        }
 
	    #endregion

        #region Question Class Main Properties

        public int QuestionSrNo { get; set; }
        public int QuestionId { get; set; }
        //public string QuestionType { get; set; }
        public MyQuestionType QuestionType { get; set; }
        public object QuestionObject { get; set; }
        public string QuestionText { get; set; }
        public int QuestionOrder { get; set; }
        public string QuestionDescription { get; set; }
        public string QuestionToolTip { get; set; }

        #endregion

        #region Question Class Other Properties

        public int QuestionCount { get; set; }
        public bool AcceptFlag { get; set; }
        public bool Render { get; set; }
        public bool? HasParent { get; set; }
        public bool? HasChild { get; set; }
        public int? DepedentOn { get; set; }
        public int WhenQuestion { get; set; }
        public string WhenCondition { get; set; }

        #endregion        

    }
}
