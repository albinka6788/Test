#region Using Directives

using System;
using System.Collections.Generic;

using BHIC.Common.DataAccess;
using BHIC.Domain.QuestionEngine;
using BHIC.ViewDomain.Landing;
using BHIC.ViewDomain.QuestionEngine;

#endregion

namespace BHIC.Contract.PurchasePath
{
    public interface IQuestionnaire
    {
        #region Main methods 

        /// <summary>
        /// Get QuestionResponse object to get all details including list of questions
        /// </summary>
        /// <param name="quoteId"></param>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        QuestionsResponse GetQuestionsResponse(int quoteId);
        
        /// <summary>
        /// Get the historical questions and user responses associated with the request parameters like QuoteId for the purpose of "Question Yellow Highlighting"
        /// </summary>
        /// <param name="quoteId"></param>
        /// <param name="serviceProvider">service provider to communicate with different 3rd party APIs</param>
        /// <returns>QuestionsResponse</returns>
        QuestionsHistoryResponse GetQuestionsHistory(QuestionsHistoryRequestParms args);

        /// <summary>
        /// Get fein-number applicability for a particular quoteId
        /// </summary>
        /// <param name="wcQuoteId"></param>
        /// <returns></returns>
        bool IsFeinNumberApplicable(int wcQuoteId);

        /// <summary>
        /// Get minimum premium threshold of particular state
        /// </summary>
        /// <param name="stateCode"></param>
        /// <returns></returns>
        decimal GetStateMinimumPremiumThreshold(string stateCode);

        /// <summary>
        /// Get estimated premium by calling quick-quote service
        /// </summary>
        /// <param name="wcQuoteId"></param>
        /// <returns></returns>
        decimal PostQuickQuoteAndGetPremium(int wcQuoteId);

        /// <summary>
        /// Retruns list of all the AvailableCarriers 
        /// </summary>
        /// <param name="lineOfBusiness"></param>
        /// <returns></returns>
        List<string> GetAvailableCarriers(string lineOfBusiness);

        /// <summary>
        /// Get fien-number xMod factor stored in local database
        /// </summary>
        /// <param name="feinNumber"></param>
        /// <param name="xModExists"></param>
        /// <returns></returns>
        decimal GetFeinXModFactorNonPaid(string feinNumber, out bool xModExists);

        /// <summary>
        /// Return xMod factor for supplied FEIN number from 3rd Party vendor by paying for service api per call charges
        /// </summary>
        /// <param name="feinNumber"></param>
        /// <param name="xModErrorExists"></param>
        /// <returns></returns>
        decimal GetFeinXModFactorPaid(string feinNumber, out bool xModErrorExists, out DateTime xModExpiryDate,out List<string> errorResponseMessage);

        /// <summary>
        /// Returns the xMod factor applied status based on different case/scenarios along with xMod factor value for suuplied FEIN number
        /// </summary>
        /// <param name="quoteId"></param>
        /// <param name="feinNumber"></param>
        /// <param name="feinXModFactor"></param>
        /// <returns></returns>
        bool CheckAndSetXmodFactorApplicability(int quoteId, string feinNumber, out Dictionary<string, object> dictionaryReturnedInfo);

        /// <summary>
        /// Return premium threshold based on supplied state-code
        /// </summary>
        /// <param name="stateCode"></param>
        /// <returns></returns>
        bool UpdateFeinXModFactorInLocalDB(string feinNumber, decimal feinXModFactor, DateTime expiryDate);

        /// <summary>
        /// POST modifier data into provider system for supplied QuoteId 
        /// </summary>
        bool ApplyModifierForQuote(int quoteId, decimal xModFactor, string lineOfBusiness);

        /// <summary>
        /// DELETE modifier data into provider system for supplied QuoteId 
        /// </summary>
        bool DeleteModifierForQuote(int quoteId, string lineOfBusiness);

        /// <summary>
        /// Return referral threshold claims value required to make decision on QuoteStatus finalization
        /// </summary>
        /// <returns></returns>
        int GetReferralThresholdClaims();

        /// <summary>
        /// Return user response on specific PastThreeYrClaimQuestion question to make judgement on QuoteStatus finalization
        /// </summary>
        /// <param name="questionsList"></param>
        /// <returns></returns>
        int GetNoOfClaimInPastThreeYears(List<Question> questionsList);

        /// <summary>
        /// Return final redirect result for supplied quote based on Question POST response, and other case scenarios
        /// </summary>
        /// <returns></returns>
        object GetQuoteStatusDecision(QuestionsResponse questionServiceResponse, Dictionary<string, object> xModReturnedInfo,string xmlFilePath, out string finalQuoteResult);

        /// <summary>
        /// Return user list of question with auto-filled How many years have you been in business under the current management? question user-response
        /// </summary>
        /// <param name="questionsList"></param>
        /// <param name="quesionId"></param>
        /// <returns></returns>
        bool SetHowLongBeenInBusinessQuestionResponse(List<Question> questionsList, int quesionId);

        /// <summary>
        /// On user save-for-later submission add running quote into PC dashboard
        /// </summary>
        /// <param name="loggedUserId"></param>
        /// <param name="wcQuoteId"></param>
        /// <param name="emailId"></param>
        void AddQuoteToDashboard(int? loggedUserId, int wcQuoteId, string emailId);

        #region POST methods

        /// <summary>
        /// Post user response on questionnaire to get premium amount for quote
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        QuestionsResponse PostQuestionnaire(QuestionsResponse postedQuestionnaireResponse);

        #endregion

        #endregion

        #region Utility methods

        /// <summary>
        /// Fien-number length and validation
        /// </summary>
        /// <param name="feinNumber"></param>
        /// <returns></returns>
        bool IsValidFeinNumber(string feinNumber);

        /// <summary>
        /// Strips extra characters from fien-number string
        /// </summary>
        /// <param name="feinNumber"></param>
        /// <returns></returns>
        string StripFeinNumber(string feinNumber);

        #endregion

        #region Some addtional methods
        
        /// <summary>
        /// Retrun QuoteVM object for session data manipulation
        /// </summary>
        /// <returns></returns>
        QuoteViewModel GetQuoteVM();

        /// <summary>
        /// Retrun QuestionnaireVM object for question page session data manipulation
        /// </summary>
        /// <returns></returns>
        QuestionnaireViewModel GetQuestionnaireVM();

        /// <summary>
        /// Return true if this is MutilState quote
        /// </summary>
        /// <param name="appSession"></param>
        /// <returns></returns>
        bool IsMultiStateQuote();

        /// <summary>
        /// Return true if this is MutilClass quote
        /// </summary>
        /// <param name="appSession"></param>
        /// <returns></returns>
        bool IsMultiClassQuote();

        /// <summary>
        /// Retuen generic DbConnector object to communicate with database
        /// </summary>
        /// <returns></returns>
        BHICDBBase GetDbConnector();

        /// <summary>
        /// Get specific key's value from dictionary
        /// </summary>
        /// <param name="xModReturnedInfo"></param>
        /// <param name="keyName"></param>
        /// <returns></returns>
        object GetCustomKeyValue(Dictionary<string, object> customeDictionary, string keyName);

        #endregion

    }
}
