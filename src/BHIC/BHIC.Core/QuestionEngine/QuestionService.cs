using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Common;
using BHIC.Common.Config;
using BHIC.Domain.QuestionEngine;
using BHIC.Contract.QuestionEngine;
using BHIC.Common.Client;
using BHIC.Contract.Provider;

namespace BHIC.Core.QuestionEngine
{
    public class QuestionService : IServiceProviders, IQuestionService
    {
        #region Comment : Here constructor

        public QuestionService(){}

        public QuestionService(ServiceProvider provider)
        {
            base.ServiceProvider = provider;
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Returns list of Questions list based on QuoteId that belongs to particular Exposure
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public QuestionsResponse GetQuestionList(QuestionRequestParms args)
        {
            var questionResponse = SvcClient.CallService<QuestionsResponse>(string.Concat(Constants.Questions, UtilityFunctions.CreateQueryString<QuestionRequestParms>(args)), ServiceProvider);

            //Comment: Here return response when successfully processed and even when there are some non-system errors(functional errors)
            if (questionResponse.OperationStatus.RequestSuccessful || !questionResponse.OperationStatus.Messages.Any(res => res.MessageType == Domain.Service.MessageType.SystemError))
            {
                return questionResponse;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(questionResponse.OperationStatus.Messages));
            }
        }

        /// <summary>
        /// Get the historical questions and user responses associated with the request parameters like QuoteId
        /// </summary>
        /// <param name="quoteId"></param>
        /// <returns></returns>
        public QuestionsHistoryResponse GetQuestionsHistory(QuestionsHistoryRequestParms args)
        {
            var questionHistoryResponse = SvcClient.CallService<QuestionsHistoryResponse>(string.Concat(Constants.QuestionsHistory, UtilityFunctions.CreateQueryString<QuestionsHistoryRequestParms>(args)), ServiceProvider);

            //Comment: Here return response when successfully processed and even when there are some non-system errors(functional errors)
            if (questionHistoryResponse.OperationStatus.RequestSuccessful || !questionHistoryResponse.OperationStatus.Messages.Any(res => res.MessageType == Domain.Service.MessageType.SystemError))
            {
                return questionHistoryResponse;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(questionHistoryResponse.OperationStatus.Messages));
            }
        }

        /// <summary>
        /// Post questionnaire details, submit question answers to get quote details in next step
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public QuestionsResponse PostQuestionResponse(QuestionsResponse args)
        {
            var questionResponse = SvcClientOld.CallService<QuestionsResponse, QuestionsResponse>(Constants.Questions, "POST", args);

            if (questionResponse.OperationStatus.RequestSuccessful)
            {
                return questionResponse;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(questionResponse.OperationStatus.Messages));
            }
        }

        #endregion

        #endregion
    }
}
