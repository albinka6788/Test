using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.QuestionEngine;
using BHIC.Contract.Provider;
using BHIC.Common.Client;

namespace BHIC.Contract.QuestionEngine
{
    public interface IQuestionService 
    {
        /// <summary>
        /// Returns list of Questions list based on QuoteId that belongs to particular Exposure
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        QuestionsResponse GetQuestionList(QuestionRequestParms args);
        
        /// <summary>
        /// Get the historical questions and user responses associated with the request parameters like QuoteId
        /// </summary>
        /// <param name="quoteId"></param>
        /// <returns></returns>
        QuestionsHistoryResponse GetQuestionsHistory(QuestionsHistoryRequestParms args);

        /// <summary>
        /// Post questionnaire details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        QuestionsResponse PostQuestionResponse(QuestionsResponse args);
        
    }
}
