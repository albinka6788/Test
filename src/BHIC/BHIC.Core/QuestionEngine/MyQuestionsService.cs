using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Comment : Here Additional Library
using BHIC.Contract.QuestionEngine;
using BHIC.Domain.QuestionEngine;
using Newtonsoft.Json;


namespace BHIC.Core.QuestionEngine
{
    public class MyQuestionsService : IMyQuestionsService
    {
        #region Comment : Here property and instance variable declaration 

        public string SeriveUrl { get; set; }
        private bool _SerivceStatus { get; set; }
        public MyQuestion Question { get; set; }
        private List<MyQuestion> QuestionsList { get; set; }
        public IQueryable<MyQuestion> Questions { get; set; }

        #endregion        

        #region Comment : Here Constructor / Initialization

        public MyQuestionsService()
        {
            //Comment : Here on class load  check for service running status (implmentation will go here)
            _SerivceStatus = true;
        }

        #endregion

        #region Comment : Here set interface question property

        IQueryable<MyQuestion> IMyQuestionsService.Questions
        {
            get
            {
                Questions = this.GetQuestionsList();
                return Questions;
            }
        }

        #endregion

        #region Comment : Here check service endpoint running

        //Comment : Here get utility class object on that call WEB API
        public bool IsServiceRunning(string UrlAPI)
        {
            return _SerivceStatus;
        }
        
        #endregion

        #region Comment : Here return Questions data in "Format:{List}"

        //Comment : Here without parameter get list of Questions
        public IQueryable<MyQuestion> GetQuestionsList()
        {
            //Comment : Here first connect with service and get data (Pending implmentation)


            //Comment : Here without parameters call result (implmentation will go here)
            GetQuestionList(0);

            return this.QuestionsList.AsQueryable();
        }

        public IQueryable<MyQuestion> GetQuestionsList(MyQuestionParameters searchParams)
        {
            //Comment : Here based on this supplied Object-Property values fetch details from service (Pending implmentation)

            
            //Comment : Here without parameters call result (implmentation will go here)
            GetQuestionList(searchParams.ZipCode);            

            //Comment : Here convert this collection/list into generic type
            Questions = this.QuestionsList.AsQueryable();

            return Questions;
        }

        public MyQuestion GetQuestion(int QuestionId)
        {
            //Comment : Here based questionId get specific Question details from service(
            Question = new MyQuestion { QuestionSrNo = 200, QuestionId = 101, QuestionText = "Question 101 text ? ", QuestionType = MyQuestionType.Checkbox, QuestionToolTip = "Q101 dummy text" };

            return Question;
        }
        
        #endregion

        #region Comment : Here return Questions data in "Format:{JSON-string}"

        public string GetQuestionsJson() 
        {
            //Comment : Here first connect with service and get data (Pending implmentation)


            //Comment : Here without parameters call result (implmentation will go here)
            GetQuestionList(0);
            
            //Comment : Here serialize this list data into Json Type
            var Json = JsonConvert.SerializeObject(QuestionsList, Formatting.Indented);

            return Json;
        }

        public string GetQuestionsJson(MyQuestionParameters searchParams)
        {
            //Comment : Here first connect with service and get data (Pending implmentation)


            //Comment : Here without parameters call result (implmentation will go here)
            GetQuestionList(searchParams.ZipCode);

            //Comment : Here serialize this list data into Json Type
            var Json = JsonConvert.SerializeObject(QuestionsList, Formatting.Indented);

            return Json;
        }

        #endregion

        #region Comment : Here Private method for Questions list generation

        private void GetQuestionList(int? id)
        {
            if (id == null || id == 0)
            {
                #region Comment : Here default Questions list 

                //Comment : Here without parameters call result (implmentation will go here)
                QuestionsList = new List<MyQuestion>
                {
                     new MyQuestion{ QuestionSrNo=1, QuestionId=1, QuestionText="What is the delivery/service radius (in miles)? ", QuestionType = MyQuestionType.Numeric, QuestionToolTip="Q1 dummy text" }
                    ,new MyQuestion{ QuestionSrNo=2, QuestionId=2
                                                   , QuestionText="Do you engage in the sale, delivery, service, or repair of large trucks (commercial), tractor trailers, mobile homes, or boats? "
                                                   , QuestionType = MyQuestionType.Text, QuestionToolTip="Q2 dummy text" }
                    ,new MyQuestion{ QuestionSrNo=3, QuestionId=3, QuestionText="Do you provide towing for AAA or for the police?  ", QuestionType = MyQuestionType.Radio, QuestionToolTip="Q3 dummy text" }
                };

                #endregion
            }
            else
            {
                #region Switch execution based on id value

                switch (id)
                {
                    case 1:

                        //Comment : Here without parameters call result (implmentation will go here)
                        QuestionsList = new List<MyQuestion>
                        {
                             new MyQuestion{ QuestionSrNo=1, QuestionId=1, QuestionText="Question 1 text ? ", QuestionType = MyQuestionType.Numeric
                                 ,QuestionObject = new Numeric{ MinVal=0,MaxVal=300 }, QuestionToolTip="Q1 dummy text" }
                            ,new MyQuestion{ QuestionSrNo=2, QuestionId=2, QuestionText="Question 2 text ? ", QuestionType = MyQuestionType.Radio, QuestionToolTip="Q2 dummy text" }
                            ,new MyQuestion{ QuestionSrNo=3, QuestionId=3, QuestionText="Question 3 text ? ", QuestionType = MyQuestionType.Percentage, QuestionToolTip="Q3 dummy text" }
                            ,new MyQuestion{ QuestionSrNo=3, QuestionId=4, QuestionText="Question 4 text ? ", QuestionType = MyQuestionType.TextParagraph, QuestionToolTip="Q4 dummy text" }
                            ,new MyQuestion{ QuestionSrNo=3, QuestionId=5, QuestionText="Question 5 text ? ", QuestionType = MyQuestionType.Percentage, QuestionToolTip="Q5 dummy text" }
                        };

                        break;

                    case 2:

                        //Comment : Here without parameters call result (implmentation will go here)
                        QuestionsList = new List<MyQuestion>
                        {
                             new MyQuestion{ QuestionSrNo=1, QuestionId=1, QuestionText="Question 6 text ? ", QuestionType = MyQuestionType.Numeric, QuestionToolTip="Q6 dummy text" }
                            ,new MyQuestion{ QuestionSrNo=2, QuestionId=2, QuestionText="Question 7 text ? ", QuestionType = MyQuestionType.List, QuestionToolTip="Q7 dummy text" }
                            ,new MyQuestion{ QuestionSrNo=3, QuestionId=3, QuestionText="Question 8 text ? ", QuestionType = MyQuestionType.Text, QuestionToolTip="Q8 dummy text" }
                            ,new MyQuestion{ QuestionSrNo=2, QuestionId=4, QuestionText="Question 9 text ? ", QuestionType = MyQuestionType.Radio, QuestionToolTip="Q9 dummy text" }
                            ,new MyQuestion{ QuestionSrNo=3, QuestionId=5, QuestionText="Question 10 text ? ", QuestionType = MyQuestionType.TextParagraph, QuestionToolTip="Q10 dummy text" }
                        };

                        break;

                    case 3:

                        //Comment : Here without parameters call result (implmentation will go here)
                        QuestionsList = new List<MyQuestion>
                        {
                             new MyQuestion{ QuestionSrNo=1, QuestionId=1, QuestionText="Question 11 text ? ", QuestionType = MyQuestionType.Numeric, QuestionToolTip="Q11 dummy text" }
                            ,new MyQuestion{ QuestionSrNo=2, QuestionId=2, QuestionText="Question 12 text ? ", QuestionType = MyQuestionType.Text, QuestionToolTip="Q12 dummy text" }
                            ,new MyQuestion{ QuestionSrNo=3, QuestionId=3, QuestionText="Question 13 text ? ", QuestionType = MyQuestionType.Radio, QuestionToolTip="Q13 dummy text" }
                            ,new MyQuestion{ QuestionSrNo=2, QuestionId=4, QuestionText="Question 14 text ? ", QuestionType = MyQuestionType.List, QuestionToolTip="Q14 dummy text" }
                            ,new MyQuestion{ QuestionSrNo=3, QuestionId=5, QuestionText="Question 15 text ? ", QuestionType = MyQuestionType.Radio, QuestionToolTip="Q15 dummy text" }
                        };

                        break;
                }

                #endregion
            }            
        }

        #endregion

    }

}
