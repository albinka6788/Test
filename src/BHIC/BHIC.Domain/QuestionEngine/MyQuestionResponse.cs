using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.QuestionEngine
{
    public class MyQuestionResponse
    {
        #region Constructor / Initialization

        public MyQuestionResponse()
        {
            //logic for implementation
        }
 
	    #endregion

        #region QuestionResponse Class Main Properties

        public int QuestionId { get; set; }
        public string QuestionResponseType { get; set; }
        public bool QuestionResponsesValid { get; set; }        

        //Comment : Here this will check for user acceptance on quote form checkbox before submitting answers
        public bool AcceptFlag	
        {
            get { return this.ResponseRadio == "Y" ? true : false; }
            set { this.ResponseRadio = (value == true ? "Y" : "N"); }
        }

        public string ResponseRadio { get; set; }
        public string ResponseText { get; set; }
        public int? ResponseNumeric { get; set; }
        public int ResponsePercent { get; set; }
        public string ResponseList { get; set; }
        public DateTime? ResponseDate { get; set; }

        #endregion

        #region QuestionResponse Class Other Properties

        public int QuestionResponseCount { get; set; }
        public string QuoteStatus { get; set; }
        public string ResultMessages { get; set; }

        #endregion

        #region Collection objects

        public ICollection<MyQuestionResponse> objListQuestionResponse { get; set; }
        public ICollection<Question> objListQuestions { get; set; }

        #endregion
        
    }
}
