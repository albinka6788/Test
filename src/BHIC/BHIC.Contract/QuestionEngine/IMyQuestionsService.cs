using System.Collections.Generic;
using System.Linq;
using BHIC.Domain.QuestionEngine;

namespace BHIC.Contract.QuestionEngine
{
    public interface IMyQuestionsService
    {
        #region Comment : Here property and instance variable declaration

        string SeriveUrl { get; set; }
        IQueryable<MyQuestion> Questions { get; }
        MyQuestion GetQuestion(int QuestionId);

        #endregion        

        #region Comment : Here check service endpoint running

        bool IsServiceRunning(string seriveUrl);

        #endregion

        #region Comment : Here return Questions data in "Format:{List}"

        IQueryable<MyQuestion> GetQuestionsList();

        IQueryable<MyQuestion> GetQuestionsList(MyQuestionParameters searchParams);        
        
        #endregion

        #region Comment : Here return Questions data in "Format:{JSON-string}"

        string GetQuestionsJson();

        string GetQuestionsJson(MyQuestionParameters searchParams);

        #endregion
                
    }
}
