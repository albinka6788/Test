#region Using Directives

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using BHIC.Common;
using BHIC.Common.Client;
using BHIC.Common.Config;
using BHIC.Common.DataAccess;
using BHIC.Common.Logging;
using BHIC.Contract.Policy;
using BHIC.Contract.PurchasePath;
using BHIC.Contract.QuestionEngine;
using BHIC.Core.Policy;
using BHIC.Core.QuestionEngine;
using BHIC.Domain.Background;
using BHIC.Domain.Policy;
using BHIC.Domain.QuestionEngine;
using BHIC.Domain.Service;
using BHIC.Domain.XMod;
using BHIC.ViewDomain;
using BHIC.ViewDomain.Landing;
using BHIC.ViewDomain.QuestionEngine;
using BHIC.Contract.Background;
using BHIC.Core.XMod;
using BHIC.Common.XmlHelper;

#endregion

namespace BHIC.Core.PurchasePath
{
    public class Questionnaire : IQuestionnaire
    {
        #region Variables : Class-Level local variables decalration

        protected static string LineOfBusiness = "WC";

        private static string Questions = "Questions";
        private static string Quote = "Quote";
        private static string QuoteSoftReferral = "Soft Referral";
        private static string QuoteHardReferral = "Hard Referral";

        private static string QuoteAPIRefer = "Refer";
        private static string QuoteAPIDecline = "Decline";


        private static string MultipleXMods = "HasMultipleXmod";
        // private static string ErrorResponseCodeXMods = "HasErrorResponseCodeXmod";
        private static string FeinXModFactor = "FeinXmodFactor";
        private static string XModExpiryDate = "XmodExpiryDate";

        //private static string PastThreeYrClaimQuestion = "Please provide the total number of claims (Lost Time or Medical) in the past three years?";
        private static int PastThreeYrClaimQuestionId = ConfigCommonKeyReader.TotalNumbeOfClaimInPastThreeYears;

        CustomSession appSession;
        ServiceProvider serviceProvider;

        #endregion

        #region Constructors

        public Questionnaire() { }

        /// <summary>
        /// Initilize local instance of custom-session object to be used in different methods in this BLL
        /// </summary>
        /// <param name="customAppSession"></param>
        public Questionnaire(CustomSession customAppSession, ServiceProvider commonServiceProvider)
        {
            appSession = customAppSession;
            serviceProvider = commonServiceProvider;
        }

        #endregion

        #region Methods

        #region Methods : public methods

        /// <summary>
        /// Get QuestionResponse object to get all details based on exposure data &amp; policy data including list of questions
        /// </summary>
        /// <param name="quoteId"></param>
        /// <param name="serviceProvider">service provider to communicate with different 3rd party APIs</param>
        /// <returns>QuestionsResponse</returns>
        public QuestionsResponse GetQuestionsResponse(int wcQuoteId)
        {
            //Comment : Here set service provider to all API calls
            IQuestionService questionService = new QuestionService(serviceProvider);

            return wcQuoteId != 0 ? questionService.GetQuestionList(new QuestionRequestParms { QuoteId = wcQuoteId }) :
                new QuestionsResponse();
        }

        /// <summary>
        /// Get the historical questions and user responses associated with the request parameters like QuoteId for the purpose of "Question Yellow Highlighting"
        /// </summary>
        /// <param name="quoteId"></param>
        /// <param name="serviceProvider">service provider to communicate with different 3rd party APIs</param>
        /// <returns>QuestionsResponse</returns>
        public QuestionsHistoryResponse GetQuestionsHistory(QuestionsHistoryRequestParms args)
        {
            //Comment : Here set service provider to all API calls
            IQuestionService questionService = new QuestionService(serviceProvider);

            return args.QuoteId != 0 ? questionService.GetQuestionsHistory(args) :
                new QuestionsHistoryResponse();
        }

        /// <summary>
        /// Get fein-number applicability for a particular quoteId
        /// </summary>
        /// <param name="wcQuoteId"></param>
        /// <returns></returns>
        public bool IsFeinNumberApplicable(int wcQuoteId)
        {
            bool isApplicable = false;

            try
            {
                #region Comment : Here local variables declaration & initialization

                string zipCode = string.Empty, stateCode = string.Empty;
                int businessExperienceYrs = 0;
                decimal minimumStatePremiumThreshold = 0;

                #endregion

                #region Comment : Here get StateCode based on ZipCode

                if (appSession != null)
                {
                    var sesionQuoteVM = GetQuoteVM();

                    if (sesionQuoteVM != null)
                    {
                        zipCode = appSession.ZipCode ?? string.Empty;
                        stateCode = appSession.StateAbbr ?? string.Empty;

                        var businessYrs = sesionQuoteVM.PolicyData.YearsInBusiness;
                        businessExperienceYrs = businessYrs > 0 ? businessYrs : Convert.ToInt32(sesionQuoteVM.BusinessYears);
                    }

                    //zipCode = "13736"; stateAbbreviation = "NY"; businessExperienceYrs = 3;

                    #region Comment : Here if both required attributes found then take next(DB Call) action

                    if (zipCode.Length > 0 && stateCode.Length > 0)
                    {
                        //Comment : Here get DbConnector object
                        minimumStatePremiumThreshold = GetStateMinimumPremiumThreshold(stateCode);

                        #region Comment : Here Condition- 1) Is state (as per the Zip code provided) present in x-mod table?

                        //Comment :  Here if PremiumThersold exist in DB then
                        if (minimumStatePremiumThreshold > 0)
                        {
                            #region Comment : Here Condition- 2) If state minimum premium exists, then did user started business at least 2 years ago (or more)?

                            if (businessExperienceYrs >= 2)
                            {
                                #region Comment : Here Condition- 3) Make call to Service provider api to get premium for this quote using QuickQuote api.

                                //Comment : Here get mannual premium based on carrier,exposure,effective-date 
                                var quotePremium = PostQuickQuoteAndGetPremium(wcQuoteId);

                                #region Comment : Here Condition- 4) Is manual premium fetched using QuickQuote service eligible for x-mod?
                                //(i.e. greater than or equal to state threshold premium as given in x-mod table?

                                if (quotePremium >= 0 && quotePremium >= minimumStatePremiumThreshold)
                                {
                                    isApplicable = true;
                                }

                                #endregion

                                #endregion
                            }

                            #endregion
                        }

                        #endregion
                    }

                    #endregion
                }

                #endregion
            }
            catch (Exception ex) { throw ex; }

            return isApplicable;
        }

        /// <summary>
        /// Get minimum premium threshold of particular state
        /// </summary>
        /// <param name="stateCode"></param>
        /// <returns></returns>
        public decimal GetStateMinimumPremiumThreshold(string stateCode)
        {
            try
            {
                //Comment : Here get DbConnector object
                var dataSet = GetDbConnector().LoadDataSet("GetStateMinimumPremium", QueryCommandType.StoredProcedure,
                    new List<System.Data.IDbDataParameter> { new SqlParameter() { ParameterName = "@StateCode", Value = stateCode, SqlDbType = SqlDbType.Char, Size = 2 } });

                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    var minimumPremium = dataSet.Tables[0].Rows[0]["PremiumThreshold"];
                    return minimumPremium != null ? Convert.ToDecimal(minimumPremium) : 0;
                }
            }
            catch (Exception ex)
            {
                //Comment : Here log thhis error iand then throw error to calle
                LogMe(Constants.LoggingType.Fatal, (new StringBuilder()).Append(ex.Message));
            }

            return 0;
        }

        /// <summary>
        /// Get estimated premium by calling quick-quote service
        /// </summary>
        /// <param name="wcQuoteId"></param>
        /// <returns></returns>
        public decimal PostQuickQuoteAndGetPremium(int wcQuoteId)
        {
            try
            {
                //Comment : Here check for application custom session object data existance
                if (appSession != null && appSession.QuoteVM != null)
                {
                    #region Comment : Here get all required data/info to get QuickQuote

                    if (wcQuoteId > 0)
                    {
                        var sessionQuoteVM = GetQuoteVM();

                        if (sessionQuoteVM.PolicyData != null && (sessionQuoteVM.Exposures != null && sessionQuoteVM.Exposures.Count > 0))
                        {

                            //Comment : Here get policy Data
                            var inceptionDate = sessionQuoteVM.PolicyData.InceptionDate;

                            //Comment : Here get all exposures
                            var quoteExposures = sessionQuoteVM.Exposures;

                            #region Comment : Here if QuickQuote request parameters has valid data then make API call

                            if (inceptionDate != null && quoteExposures != null)
                            {
                                //Comment : Here Get service reference to make a call
                                IQuickQuoteService quickQuoteService = new QuickQuoteService(serviceProvider);

                                var quickQuotePremium = quickQuoteService.GetQuickQuotePremium(
                                                        new QuickQuoteRequestParms
                                                        {
                                                            QuoteId = wcQuoteId,
                                                            EffDate = inceptionDate ?? DateTime.Now,
                                                            Exposures = quoteExposures
                                                        });

                                return quickQuotePremium;
                            }

                            #endregion
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex) 
            {
                //Comment : Here log thhis error iand then throw error to calle
                LogMe(Constants.LoggingType.Fatal, (new StringBuilder()).Append(ex.Message));

                throw ex; 
            }

            return 0;
        }

        /// <summary>
        /// Retruns list of all the AvailableCarriers 
        /// </summary>
        /// <param name="lineOfBusiness"></param>
        /// <returns></returns>
        public List<string> GetAvailableCarriers(string lineOfBusiness)
        {
            try
            {
                //Comment : Here check for application custom session object data existance
                if (appSession != null)
                {
                    var sessionQuoteVM = GetQuoteVM();
                    if (sessionQuoteVM != null)
                    {
                        var carriersResponse = SvcClient.CallService<AvailableCarriersResponse>(string.Format("AvailableCarriers?LOB={0}&EffectiveDate={1}&States={2}", lineOfBusiness, sessionQuoteVM.PolicyData.InceptionDate, appSession.StateAbbr), serviceProvider);

                        return (carriersResponse != null && carriersResponse.OperationStatus.RequestSuccessful) ? carriersResponse.Carriers : (new List<string>());
                    }
                }

                //private IAvailableCarriersService availableCarriersService = new AvailableCarriersService();
                //var carriers = availableCarriersService.GetAvailableCarriersList(
                //    new AvailableCarriersRequestParms() { EffectiveDate = Convert.ToDateTime("09/28/2015"), LOB = LineOfBusiness, States = new List<string> { "NY" } });

                //return (carriers != null && carriers.Count > 0) ? carriers[0] : string.Empty;
            }
            catch { }

            return (new List<string>());
        }

        /// <summary>
        /// Return xMod factor for supplied FEIN number from local DataSource/DB without any service api charges
        /// </summary>
        /// <param name="feinNumber"></param>
        /// <param name="xModExists"></param>
        /// <returns></returns>
        public decimal GetFeinXModFactorNonPaid(string feinNumber, out bool xModExists)
        {
            //Comment : Here set default
            xModExists = false;

            try
            {
                //Comment : Here get DbConnector object
                //Fein Encryption key done for the security purpose of GUIN-112
                string encryptedFeinNumber = Encryption.EncryptWithStaticKey(feinNumber);

                var dataSet = GetDbConnector().LoadDataSet("GetFeinXModFactor", QueryCommandType.StoredProcedure,
                    new List<System.Data.IDbDataParameter> { new SqlParameter() { ParameterName = "@FeinNumber", Value = encryptedFeinNumber, SqlDbType = SqlDbType.VarChar} });

                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    var premiumFactor = dataSet.Tables[0].Rows[0]["XModValue"];

                    xModExists = premiumFactor != null ? true : false;
                    return premiumFactor != null ? Convert.ToDecimal(premiumFactor) : 0;
                }
            }
            catch (Exception ex) { LogMe(Constants.LoggingType.Fatal, (new StringBuilder()).Append(ex.Message)); }

            return 0;
        }

        /// <summary>
        /// Return xMod factor for supplied FEIN number from 3rd Party vendor by paying for service api per call charges
        /// </summary>
        /// <param name="feinNumber"></param>
        /// <param name="xModErrorExists"></param>
        /// <returns></returns>
        public decimal GetFeinXModFactorPaid(string feinNumber, out bool xModErrorExists, out DateTime xModExpiryDate, out List<string> errorResponseMessage)
        {
            //Comment : Here set OUT parameter default
            xModErrorExists = false;

            //Comment : Here default set out parameter
            xModExpiryDate = DateTime.Now;
            errorResponseMessage = new List<string>();

            try
            {
                ServiceProvider ncciServiceProvider = new NcciServiceProvider() { ServiceCategory = ServiceProviderConstants.NcciServiceCategoryXMod };
                IXModService xModService = new XModService(ncciServiceProvider);

                //Comment : Here New implementation by Prem on 29.01.2016 to GetXModFactor based on "FEIN" not by "RiskId"
                //var riskXmodFactorResponse = xModService.GetModRiskId(new XModRequestParms { RiskId = feinNumber });
                var riskXmodFactorResponse = xModService.GetModFein(new XModRequestParms { Fein = feinNumber });

                if (riskXmodFactorResponse != null)
                {
                    #region Comment : Here xModFactor response found with success ResponseCode then

                    //Comment : Here details about reponse 
                    //ResponseCode – Indicates the status of the request. 0 = successful. 1 = error. Numeric max size 1.
                    //ResponseMessage – List of Messages. A successful request will contain only one message: Successful Completion. Requests resulting in errors may contain 1 to many messages.
                    if (riskXmodFactorResponse.RiskHeaderInformation.ResponseCode == 0)
                    {
                        //Comment : Here rating related informationobject
                        var ratingValueInfo = riskXmodFactorResponse.RatingValuesInformation;

                        //Comment : Here add all error messages 
                        errorResponseMessage = riskXmodFactorResponse.RiskHeaderInformation.ResponseMessage;

                        #region Comment : Here based on returned rating values decide Company/Insured background for xMod applibility

                        if (ratingValueInfo.Count > 1)
                        {
                            //Comment : Here set OUT parameter
                            xModErrorExists = true;
                        }
                        else
                        {
                            //Comment : Here default set out parameter
                            // As per story GUIN-143, we added 8 months into the received Rating Effective Date and this 8 month is configuration file based
                            xModExpiryDate = Convert.ToDateTime(ratingValueInfo[0].RatingEffectiveDate).AddMonths(ConfigCommonKeyReader.AddNumberofMonthinXModExpiryDate);

                            return Convert.ToDecimal(ratingValueInfo[0].ExperienceModificationFactor);
                        }

                        #endregion
                    }
                    else
                    {
                        //Comment : Here only come when error returned by NCCI provider
                        #region Comment : Here As per new suggestion given by Saurabh on 29.01.2016 only in case of "Multiple XMod Error Message" only take it as Error
                        
                        //get list of error reponses
                        errorResponseMessage = riskXmodFactorResponse.RiskHeaderInformation.ResponseMessage;
                        
                        if (!errorResponseMessage.IsNull() && errorResponseMessage.Count() > 0)
                        {
                            //Comment : Here in case "Multiple Risk Ids Error message found then only"
                            var isMultipleErrorResponse = errorResponseMessage.Where(errors => errors.ToUpper().Contains(ConfigCommonKeyReader.MultipleRiskIdErrorMessage)).FirstOrDefault();
                            if (!string.IsNullOrEmpty(isMultipleErrorResponse))
                            {
                                xModErrorExists = true;
                            }
                        }

                        #endregion                        
                    }

                    #endregion
                }
            }
            catch (Exception ex) { LogMe(Constants.LoggingType.Fatal, (new StringBuilder()).Append(ex.Message)); }

            return 0;
        }

        /// <summary>
        /// Returns the xMod factor applied status based on different case/scenarios along with xMod factor value for suuplied FEIN number
        /// </summary>
        /// <param name="quoteId"></param>
        /// <param name="feinNumber"></param>
        /// <param name="feinXModFactor"></param>
        /// <returns></returns>
        public bool CheckAndSetXmodFactorApplicability(int quoteId, string feinNumber, out Dictionary<string, object> dictionaryReturnedInfo)
        {
            decimal feinXModFactor = 0;
            DateTime xmodExpiryDate = DateTime.Now;
            bool xModExists = false, feinXmodError = false, xModPaid = false;

            //Comment : Here default set out parameter
            dictionaryReturnedInfo = new Dictionary<string, object>();

            try
            {

                //Comment : Here STEP - 1 First check in local DB to reduce paid service API call
                feinXModFactor = GetFeinXModFactorNonPaid(feinNumber, out xModExists);

                //Comment : Here If not exist in local DB then check for paid version service call
                if (!xModExists)
                {
                    List<string> errorResponseMessage = new List<string>();

                    //Comment : Here STEP - 2 Then check using paid service provider API call
                    feinXModFactor = GetFeinXModFactorPaid(feinNumber, out feinXmodError, out xmodExpiryDate, out errorResponseMessage);

                    //Comment : Here if it fetched(from paid call) then only
                    if (feinXModFactor > 0)
                    {
                        //Comment : Here Set Flag TRUE in case paid xMod fetched 
                        xModPaid = true;
                    }

                    dictionaryReturnedInfo.Add(MultipleXMods, feinXmodError);
                }

                #region Comment : Here STEP - 3 if xModFactor found and no error/exception found then apply this factor in

                //3.1 Update this FEIN XMod value inot local DB for future refernces
                //3.2 POST this XMod/modifier for this QuoteId to applied in final premium calculation
                if (feinXModFactor > 0 && !feinXmodError)
                {
                    //Comment : Here set out parameter
                    dictionaryReturnedInfo.Add(FeinXModFactor, feinXModFactor);
                    dictionaryReturnedInfo.Add(XModExpiryDate, xmodExpiryDate);

                    //Comment : Here STEP - 4 Update this FEIN number, XMod factor into local DB
                    //Only if paid xMod fetched 
                    if (xModPaid)
                    {
                        var isModifierUpdated = UpdateFeinXModFactorInLocalDB(feinNumber, feinXModFactor, xmodExpiryDate);
                    }

                    //Comment : Here STEP - 5 Apply/POST this FEIN number, XMod factor for current QuoteId into provider system
                    var isModifierApplied = ApplyModifierForQuote(quoteId, feinXModFactor, LineOfBusiness);

                    //Comment : Here finally if modifierApplied then return TRUE
                    return isModifierApplied;
                }

                #endregion
            }
            catch (Exception ex) { throw ex; }

            return false;
        }

        /// <summary>
        /// Return premium threshold based on supplied state-code
        /// </summary>
        /// <param name="stateCode"></param>
        /// <returns></returns>
        public bool UpdateFeinXModFactorInLocalDB(string feinNumber, decimal feinXModFactor, DateTime expiryDate)
        {
            try
            {
                if (IsValidFeinNumber(feinNumber) && feinXModFactor > 0 && expiryDate != null)
                {
                    //Comment : Here get DbConnector object

                    //Fein Encryption key done for the security purpose of GUIN-112
                    string encryptedFeinNumber = Encryption.EncryptWithStaticKey(feinNumber);

                    var rowEffeted = GetDbConnector().ExecuteNonQuery("UpdateFeinXModFactor", QueryCommandType.StoredProcedure,
                                    new List<System.Data.IDbDataParameter> 
                                    { 
                                        new SqlParameter() { ParameterName = "@FeinNumber", Value = encryptedFeinNumber, SqlDbType = SqlDbType.VarChar } ,
                                        new SqlParameter() { ParameterName = "@XModValue", Value = feinXModFactor, SqlDbType = SqlDbType.Float },
                                        new SqlParameter() { ParameterName = "@ExpiryDate", Value = expiryDate, SqlDbType = SqlDbType.DateTime }
                                    });

                    if (rowEffeted > 0)
                    {
                        return true;
                    }
                }
            }
            //Comment : Here in case of DB updation failure log it into XML for future re-trying by schedular service
            catch (Exception ex) { LogMe(Constants.LoggingType.Fatal, (new StringBuilder()).Append(ex.Message)); }

            return false;
        }

        /// <summary>
        /// POST modifier data into provider system for supplied QuoteId 
        /// </summary>
        public bool ApplyModifierForQuote(int quoteId, decimal xModFactor, string lineOfBusiness)
        {
            try
            {
                //Comment : Here check appSession object 
                if (appSession != null)
                {
                    #region Comment : Here local variables declaration & initialization

                    string zipCode = string.Empty, stateCode = string.Empty;

                    #endregion

                    #region Comment : Here if both required attributes found then take next(DB Call) action

                    var sesionQuoteVM = GetQuoteVM();
                    if (sesionQuoteVM != null)
                    {
                        zipCode = appSession.ZipCode ?? string.Empty;
                        stateCode = appSession.StateAbbr ?? string.Empty;
                    }

                    if (zipCode.Length > 0 && stateCode.Length > 0)
                    {
                        var sesionQuestionnaireVM = GetQuestionnaireVM();

                        var modifier = new BHIC.Domain.Policy.Modifier() 
                        {
                            QuoteId = quoteId,
                            ModType = "ExMod",
                            ModValue = xModFactor,
                            LOB = lineOfBusiness,
                            ZipCode = zipCode,
                            State = stateCode
                          ,
                            ModifierId = (sesionQuestionnaireVM.XModFactorModifierId ?? null)
                        };

                        var serviceResponse = SvcClient.CallServicePost<BHIC.Domain.Policy.Modifier, OperationStatus>("Modifiers", modifier, serviceProvider);

                        if (serviceResponse.RequestSuccessful)
                        {
                            //Comment : Here after modidifer applied set it's Id into session object
                            var modifierId = serviceResponse.AffectedIds != null ? serviceResponse.AffectedIds.SingleOrDefault(mod => mod.DTOProperty == "ModifierId").IdValue : null;
                            appSession.QuestionnaireVM.XModFactorModifierId = Convert.ToInt32(modifierId);

                            return true;                            
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex) { LogMe(Constants.LoggingType.Fatal, (new StringBuilder()).Append(ex.Message)); throw ex; }

            return false;
        }

        /// <summary>
        /// Return referral threshold claims value required to make decision on QuoteStatus finalization
        /// </summary>
        /// <returns></returns>
        public int GetReferralThresholdClaims()
        {
            try
            {
                //Comment : Here check appSession object 
                if (appSession != null)
                {
                    #region Comment : Here local variables declaration & initialization

                    string stateCode = string.Empty, classCode = string.Empty;
                    decimal annualPayroll = 0;

                    #endregion

                    #region Comment : Here if both required attributes found then take next(DB Call) action

                    var sesionQuoteVM = appSession.QuoteVM;
                    if (sesionQuoteVM != null)
                    {
                        stateCode = appSession.StateAbbr ?? string.Empty;                        
                        classCode = sesionQuoteVM.SelectedSearch == 0 ? sesionQuoteVM.ClassCode : sesionQuoteVM.Class.ClassCode;
                        //classCode = sesionQuoteVM.ClassCode; //Old line
                        annualPayroll = sesionQuoteVM.AnnualPayroll;
                    }

                    //Comment : Here these values must be validated before making DB call
                    if (stateCode.Length > 0 && !string.IsNullOrEmpty(classCode) && annualPayroll > 0)
                    {
                        //Comment : Here get DbConnector object
                        var retunedObject = GetDbConnector().ExecuteScalar("GetReferralThresholdClaims", QueryCommandType.StoredProcedure,
                                        new List<System.Data.IDbDataParameter> 
                                { 
                                    new SqlParameter() { ParameterName = "@StateCode", Value = stateCode, SqlDbType = SqlDbType.Char, Size = 2 } ,
                                    new SqlParameter() { ParameterName = "@ClassCode", Value = classCode.ToString(), SqlDbType = SqlDbType.VarChar, Size=10 },
                                    new SqlParameter() { ParameterName = "@AnnualPayroll", Value = annualPayroll, SqlDbType = SqlDbType.Decimal }
                                });

                        //Comment : Here convert retuned object
                        var retunedClaims = retunedObject != null ? Convert.ToInt32(retunedObject) : 0;

                        //Comment : Here if get some value the do next 
                        if (retunedClaims > 0)
                        {
                            return retunedClaims;
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex) { LogMe(Constants.LoggingType.Fatal, (new StringBuilder()).Append(ex.Message)); throw ex; }

            return 0;
        }

        /// <summary>
        /// Return user response on specific PastThreeYrClaimQuestion question to make judgement on QuoteStatus finalization
        /// </summary>
        /// <param name="questionsList"></param>
        /// <returns></returns>
        public int GetNoOfClaimInPastThreeYears(List<Question> questionsList)
        {
            try
            {
                if (questionsList != null && questionsList.Count > 0)
                {
                    //Prevoius implementation
                    //var pastThreeYrQuestionResponse = questionsList.First(question => question.questionText.Trim().ToUpper().Equals(PastThreeYrClaimQuestion.ToUpper())).UserResponse.Trim();

                    var pastThreeYrQuestionResponse = questionsList.First(question => question.questionId == PastThreeYrClaimQuestionId).UserResponse.Trim();

                    return pastThreeYrQuestionResponse != null ? Convert.ToInt32(pastThreeYrQuestionResponse) : 0;
                }
            }
            catch
            {
                throw;
            }

            return 0;
        }

        /// <summary>
        /// Return user list of question with auto-filled How many years have you been in business under the current management? question user-response
        /// </summary>
        /// <param name="questionsList"></param>
        /// <param name="quesionId"></param>
        /// <returns></returns>
        public bool SetHowLongBeenInBusinessQuestionResponse(List<Question> questionsList, int quesionId)
        {
            try
            {
                if (questionsList != null && questionsList.Count > 0)
                {
                    var sesionQuoteVM = GetQuoteVM();

                    if (sesionQuoteVM != null && sesionQuoteVM.BusinessYears >= 0)
                    {
                        questionsList.First(question => question.questionId == quesionId).UserResponse = sesionQuoteVM.BusinessYears.ToString();
                        return true;
                    }
                }
            }
            catch { }

            return false;
        }

        /// <summary>
        /// Return final redirect result for supplied quote based on Question POST response, and other case scenarios
        /// </summary>
        /// <returns></returns>
        public object GetQuoteStatusDecision(QuestionsResponse questionServiceResponse, Dictionary<string, object> xModReturnedInfo, string referralXmlFilePath, out string finalQuoteResult)
        {
            #region Comment : Here Local variables
		
            //Comment : Here set default value
            finalQuoteResult = string.Empty;
            List<int> currentReferralScenarioIds = new List<int>();

            //Comment : Here get complete ReferralHistory from session
            ReferralHistory referralDeclineHistoryNew 
                = (!appSession.QuestionnaireVM.ReferralHistory.IsNull()) ? appSession.QuestionnaireVM.ReferralHistory : new ReferralHistory();
 
	        #endregion

            try
            {
                #region Comment : Here Scenario - 1 Are multiple x-mod numbers or any error messages found?

                //Add XModValue
                var hasMultipleXmods = this.GetCustomKeyValue(xModReturnedInfo, MultipleXMods);

                if (Convert.ToBoolean(hasMultipleXmods))
                {
                    //Comment : Here log this error
                    LogMe(Constants.LoggingType.Trace, (new StringBuilder()).Append("Error response code in  NCCI XMod call custom condition found "));

                    //Comment : Here also set custom made decision about REFERRAL into session
                    appSession.QuestionnaireVM.QuoteReferralMessage = Constants.CustomQuoteReferralMessage1;

                    //Comment : Here set REFERRAL scenario id to get related details for Mail-Template
                    appSession.QuestionnaireVM.ReferralScenarioId = 6;

                    //Comment : Here incorporated latest requirement chnage with new implementation on 04.04.2016 (For Mutiple Referral Reasons)
                    appSession.QuestionnaireVM.QuoteReferralMessages.Add(Constants.CustomQuoteReferralMessage1);
                    appSession.QuestionnaireVM.ReferralScenarioIds.Add(6);
                    currentReferralScenarioIds.Add(6);
                }

                #endregion

                #region Comment : Here Scenario - 2 If the value entered by user in "Total number of claims in past three years" is >= to the value extracted from DB

                try
                {
                    //Comment : Here first get both comparison values
                    var referralThresholdClaim = GetReferralThresholdClaims();
                    var userEnteredClaims = GetNoOfClaimInPastThreeYears(questionServiceResponse.Questions);

                    if (userEnteredClaims >= referralThresholdClaim && referralThresholdClaim > 0)
                    {
                        //Comment : Here log thhis error
                        LogMe(Constants.LoggingType.Trace, (new StringBuilder())
                            .Append
                            (
                                string.Format("{0} ReferralClaims:{1}, UserClaims:{2}", "Total number of claims in past three years condition found ", referralThresholdClaim, userEnteredClaims)
                            ));

                        //Comment : Here also set custom made decision about REFERRAL into session
                        appSession.QuestionnaireVM.QuoteReferralMessage = Constants.CustomQuoteReferralMessage2;

                        //Comment : Here set REFERRAL scenario id to get related details for Mail-Template
                        appSession.QuestionnaireVM.ReferralScenarioId = 7;

                        //Comment : Here incorporated latest requirement chnage with new implementation on 04.04.2016 (For Mutiple Referral Reasons)
                        appSession.QuestionnaireVM.QuoteReferralMessages.Add(Constants.CustomQuoteReferralMessage2);
                        appSession.QuestionnaireVM.ReferralScenarioIds.Add(7);
                        currentReferralScenarioIds.Add(7);
                    }
                }
                catch (Exception ex)
                {
                    LoggingService.Instance.Fatal("Error occurred while validating 'Total number of claims in past three years condition found '", ex);
                }

                #endregion

                #region Comment : Here Scenario - 3 If it's a good state then in case questions response throws hard referral, display decline page, else display referral page.

                var sessionQuoteVM = (appSession != null && appSession.QuoteVM != null) ? appSession.QuoteVM : null;

                try
                {
                    //Comment : Here only if GoodOrBadState applicable then check at next level
                    if (sessionQuoteVM.IsGoodStateApplicable && !sessionQuoteVM.IsGoodState.IsNull() && sessionQuoteVM.IsGoodState.Value)
                    {
                        //Comment : Here log thhis error
                        LogMe(Constants.LoggingType.Trace, (new StringBuilder())
                            .Append
                            (
                            string.Format("{0} IsGoodStateApplicable:{1}, IsGoodStateValue:{2}", "Good state custom condition found ", sessionQuoteVM.IsGoodStateApplicable, sessionQuoteVM.IsGoodState.Value)
                            ));

                        //Comment : Here also set custom made decision about REFERRAL into session
                        appSession.QuestionnaireVM.QuoteReferralMessage = Constants.CustomQuoteReferralMessage3;

                        //Comment : Here set REFERRAL scenario id to get related details for Mail-Template
                        appSession.QuestionnaireVM.ReferralScenarioId = 3;

                        //Comment : Here incorporated latest requirement chnage with new implementation on 04.04.2016 (For Mutiple Referral Reasons)
                        appSession.QuestionnaireVM.QuoteReferralMessages.Add(Constants.CustomQuoteReferralMessage3);
                        appSession.QuestionnaireVM.ReferralScenarioIds.Add(3);
                        currentReferralScenarioIds.Add(3);
                    }
                }
                catch (Exception ex)
                {
                    LoggingService.Instance.Fatal("Error occurred while validating 'Good state custom condition found '", ex);
                }

                #endregion

                #region Comment : Here Scenario - 4 If MS/MC Considered then in case questions response throws hard referral, display decline page, else display referral page.

                try
                {
                    //Comment : Here only if MultiClass primary exposure INVALID or MultiState checked/selected applicable then check at next level
                    if (sessionQuoteVM.IsMultiStateApplicable || !sessionQuoteVM.IsMultiClassPrimaryExposureValid || (sessionQuoteVM.MoreClassRequired && sessionQuoteVM.IsMultiClass))
                    {
                        //Comment : Here log thhis error
                        LogMe(Constants.LoggingType.Trace, (new StringBuilder())
                            .Append
                            (
                            string.Format("{0} IsMultiStateApplicable:{1}, IsMultiClassPrimaryExposureValid:{2}, MoreClassOrMutliClass:{3}", "MS/MC custom condition found ", sessionQuoteVM.IsMultiStateApplicable, !sessionQuoteVM.IsMultiClassPrimaryExposureValid, (sessionQuoteVM.MoreClassRequired && sessionQuoteVM.IsMultiClass))
                            ));

                        //Comment : Here also set custom made decision about REFERRAL into session
                        appSession.QuestionnaireVM.QuoteReferralMessage = Constants.CustomQuoteReferralMessage4;

                        #region Comment : Here based on different condition set REFERRAL scenario id

                        //Comment : Here set REFERRAL scenario id to get related details for Mail-Template
                        appSession.QuestionnaireVM.ReferralScenarioId
                            = (sessionQuoteVM.IsMultiStateApplicable) ? 4 : ((!sessionQuoteVM.IsMultiClassPrimaryExposureValid) ? 5 : 0);

                        #endregion

                        //Comment : Here incorporated latest requirement chnage with new implementation on 04.04.2016 (For Mutiple Referral Reasons)
                        appSession.QuestionnaireVM.QuoteReferralMessages.Add(Constants.CustomQuoteReferralMessage4);
                        appSession.QuestionnaireVM.ReferralScenarioIds.Add(5);
                        currentReferralScenarioIds.Add(5);
                    }
                }
                catch (Exception ex)
                {
                    LoggingService.Instance.Fatal("Error occurred while validating 'MS/MC custom condition found '", ex);
                }

                #endregion

                #region Comment : Here Scenario - 5 If BusinessCalss DirectSales "E" then base on QR display referral page [Hard/Soft].

                try
                {
                    if (sessionQuoteVM.BusinessClassDirectSales.Equals("E", StringComparison.OrdinalIgnoreCase))
                    {
                        //Comment : Here log thhis error
                        LogMe(Constants.LoggingType.Trace, (new StringBuilder()).Append("Send to Soft Referral as Direct Sales is 'E' (Having Referral Only Class)"));


                        //Comment : Here also set custom made decision about REFERRAL into session
                        appSession.QuestionnaireVM.QuoteReferralMessage = Constants.CustomQuoteReferralMessage6;

                        //Comment : Here set REFERRAL scenario id to get related details for Mail-Template
                        appSession.QuestionnaireVM.ReferralScenarioId = 2;

                        //Comment : Here incorporated latest requirement chnage with new implementation on 04.04.2016 (For Mutiple Referral Reasons)
                        appSession.QuestionnaireVM.QuoteReferralMessages.Add(Constants.CustomQuoteReferralMessage6);
                        appSession.QuestionnaireVM.ReferralScenarioIds.Add(2);
                        currentReferralScenarioIds.Add(2);
                    }
                }
                catch (Exception ex)
                {
                    LoggingService.Instance.Fatal("Error occurred while validating 'DirectSales=E custom condition found '", ex);
                }

                #endregion

                #region Comment : Here Scenario - 6 If user response for last question (I hereby certify) is 'Y' then redirect user to decline quote

                try
                {
                    //Comment : Here log thhis error
                    LogMe(Constants.LoggingType.Trace, (new StringBuilder()).Append("Send to Hard Referral as last question is set as 'Y'"));

                    // Moved here after Saurabh comment.
                    var lastQuestion = questionServiceResponse.Questions.Last();

                    //Comment : Here skip further processing if enswer to last question is "True/Y"
                    if (
                            lastQuestion != null &&
                            (
                                lastQuestion.UserResponse.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                                lastQuestion.UserResponse.Equals("Y", StringComparison.OrdinalIgnoreCase)
                            )
                        )
                    {
                        finalQuoteResult = QuoteHardReferral;

                        //Comment : Here must update custom decision status in session object
                        appSession.QuestionnaireVM.QuoteStatus = finalQuoteResult;

                        //Comment : Here on page load reset "Referral Scenarios" list
                        appSession.QuestionnaireVM.QuoteReferralMessages.Clear();
                        appSession.QuestionnaireVM.ReferralScenarioIds.Clear();
                        currentReferralScenarioIds.Clear();

                        //Comment : Here set "Decline" reason
                        appSession.QuestionnaireVM.ReferralScenarioIds.Add(12);
                        currentReferralScenarioIds.Add(12);

                        //Comment : Here In case any ReferralScenarioId exists for this QUOTE then add this into Quote's "ReferralScenariosHistory"
                        appSession.QuestionnaireVM.ReferralScenariosHistory.Add(new List<int>(appSession.QuestionnaireVM.ReferralScenarioIds));

                        #region Comment : Here [GUIN-270-Prem] Based on all scenario ids get "All Referral Reasons & Description"

                        GetReferralProcessedData(referralXmlFilePath, currentReferralScenarioIds, referralDeclineHistoryNew);

                        //Comment : Here finally add this Referral/Decline history into Session object
                        appSession.QuestionnaireVM.ReferralHistory = referralDeclineHistoryNew;

                        #endregion

                        return (new { resultStatus = "OK", resultText = QuoteHardReferral });
                    }
                }
                catch (Exception ex)
                {
                    LoggingService.Instance.Fatal("Error occurred while validating 'DirectSales=E custom condition found '", ex);
                }

                #endregion

                #region Comment : Here Scenario - 7 If all custom validation is bypassed then take final decision on returned provider response QuestionsResponse

                // ***** questions ok: successful quote - user is presented with a payment information page
                if (questionServiceResponse.QuoteStatus == Quote)
                {
                    #region Comment : Here When guard has returned "QUOTE"

                    //Comment : Here must update custom decision status in session object
                    //New implementation on 04.04.2016 is if Guard has returned "Quote" but any custom "Referral" condition found then make this quote as "Referral"(Imp.)
                    finalQuoteResult = appSession.QuestionnaireVM.ReferralScenarioIds.Count == 0 ? Quote : QuoteSoftReferral;

                    #region Comment : Here Keep referring QuoteId if Referral/Decline has happened in Quote lifecycle

                    //Comment : Here According to new requirement if the Quote is "Referred/Declied" in lifecycle of any particular QuoteId 
                    //Then this QuoteID would be associated permanently "Not Eligible" for a QUOTE online (Imp.)                    
                    if (appSession.QuestionnaireVM.ReferralScenarioIds.Count == 0)
                    {
                        #region Comment : Here QuoteId currently eligible for QUOTE but has referral history then

                        //Comment : Here If there is no "current reason/scenario" for this referral (and currently eligible also), then we would say something like below
                        if (!appSession.QuestionnaireVM.ReferralScenariosHistory.IsNull() && appSession.QuestionnaireVM.ReferralScenariosHistory.Count >0)
                        {
                            //Comment : Here again set Quote final status
                            finalQuoteResult = QuoteSoftReferral;

                            //Comment : Here SCENARIO-9. Currently eligible but initially referral (or decline)
                            appSession.QuestionnaireVM.ReferralScenarioIds.Add(9);
                            currentReferralScenarioIds.Add(9);

                            //Comment : Here In case any ReferralScenarioId exists for this QUOTE then add this into Quote's "ReferralScenariosHistory"
                            appSession.QuestionnaireVM.ReferralScenariosHistory.Add(new List<int>(appSession.QuestionnaireVM.ReferralScenarioIds));
                            
                            #region Comment : Here [GUIN-270-Prem] Based on all scenario ids get "All Referral Reasons & Description"

                            GetReferralProcessedData(referralXmlFilePath, currentReferralScenarioIds, referralDeclineHistoryNew);

                            //Comment : Here finally add this Referral/Decline history into Session object
                            appSession.QuestionnaireVM.ReferralHistory = referralDeclineHistoryNew;

                            #endregion
                        }

                        #endregion
                    }
                    else
                    {
                        //Comment : Here In case any ReferralScenarioId exists for this QUOTE then add this into Quote's "ReferralScenariosHistory"
                        appSession.QuestionnaireVM.ReferralScenariosHistory.Add(new List<int>(appSession.QuestionnaireVM.ReferralScenarioIds));

                        #region Comment : Here [GUIN-270-Prem] Based on all scenario ids get "All Referral Reasons & Description"

                        GetReferralProcessedData(referralXmlFilePath, currentReferralScenarioIds, referralDeclineHistoryNew);

                        //Comment : Here finally add this Referral/Decline history into Session object
                        appSession.QuestionnaireVM.ReferralHistory = referralDeclineHistoryNew;

                        #endregion
                    }

                    #endregion

                    #endregion

                    // return the quote results
                    return (new { resultStatus = "OK", resultText = finalQuoteResult });
                }

                // ***** questions result in SOFT REFERRAL (no policy will be added to GUARD systems; end user sees same referral page as above; email sent to designated GUARD resource) 
                else if (questionServiceResponse.QuoteStatus == QuoteAPIRefer)
                {
                    #region Comment : Here When guard has returned "REFER"

                    //Comment : Here also set custom made decision about REFERRAL into session
                    appSession.QuestionnaireVM.QuoteReferralMessage = questionServiceResponse.ResultMessages.ToString();

                    //Comment : Here set REFERRAL scenario id to get related details for Mail-Template
                    appSession.QuestionnaireVM.ReferralScenarioId = 8;

                    //Comment : Here incorporated latest requirement chnage with new implementation on 04.04.2016 (For Mutiple Referral Reasons)
                    appSession.QuestionnaireVM.QuoteReferralMessages.Add(questionServiceResponse.ResultMessages.ToString());
                    appSession.QuestionnaireVM.ReferralScenarioIds.Add(8);
                    currentReferralScenarioIds.Add(8);

                    //Comment : Here In case any ReferralScenarioId exists for this QUOTE then add this into Quote's "ReferralScenariosHistory"
                    appSession.QuestionnaireVM.ReferralScenariosHistory.Add(new List<int>(appSession.QuestionnaireVM.ReferralScenarioIds));

                    #region Comment : Here [GUIN-270-Prem] Based on all scenario ids get "All Referral Reasons & Description"

                    GetReferralProcessedData(referralXmlFilePath, currentReferralScenarioIds, referralDeclineHistoryNew);

                    //Comment : Here finally add this Referral/Decline history into Session object
                    appSession.QuestionnaireVM.ReferralHistory = referralDeclineHistoryNew;

                    #endregion

                    //Comment : Here must update custom decision status in session object
                    finalQuoteResult = QuoteSoftReferral;

                    #endregion

                    return (new { resultStatus = "OK", resultText = QuoteSoftReferral });
                }

                // ***** questions result in HARD REFERRAL (user is presented with a referral page; policy needs to get added to GUARD systems) 
                else if (questionServiceResponse.QuoteStatus == QuoteAPIDecline)
                {
                    //Comment : Here must update custom decision status in session object
                    finalQuoteResult = QuoteHardReferral;

                    //Comment : Here on page load reset "Referral Scenarios" list
                    appSession.QuestionnaireVM.QuoteReferralMessages.Clear();
                    appSession.QuestionnaireVM.ReferralScenarioIds.Clear();
                    currentReferralScenarioIds.Clear();

                    //Comment : Here set "Decline" reason
                    appSession.QuestionnaireVM.ReferralScenarioIds.Add(13);
                    currentReferralScenarioIds.Add(13);

                    //Comment : Here In case any ReferralScenarioId exists for this QUOTE then add this into Quote's "ReferralScenariosHistory"
                    appSession.QuestionnaireVM.ReferralScenariosHistory.Add(new List<int>(appSession.QuestionnaireVM.ReferralScenarioIds));

                    #region Comment : Here [GUIN-270-Prem] Based on all scenario ids get "All Referral Reasons & Description"

                    //Comment : Here also set custom made decision about Decline into session
                    appSession.QuestionnaireVM.QuoteReferralMessage = questionServiceResponse.ResultMessages;

                    GetReferralProcessedData(referralXmlFilePath, currentReferralScenarioIds, referralDeclineHistoryNew);

                    //Comment : Here finally add this Referral/Decline history into Session object
                    appSession.QuestionnaireVM.ReferralHistory = referralDeclineHistoryNew;

                    #endregion

                    return (new { resultStatus = "OK", resultText = QuoteHardReferral });
                }

                #endregion

            }
            catch (Exception ex) { LogMe(Constants.LoggingType.Error, (new StringBuilder()).Append(ex.Message)); throw ex; }

            return (new { resultStatus = "NOK", resultText = "Something went wrong while making redirect decision !" });
        }

        /// <summary>
        /// Return final redirect result for supplied quote based on Question POST response, and other case scenarios
        /// </summary>
        /// <returns></returns>
        public object GetQuoteStatusDecisionOLD(QuestionsResponse questionServiceResponse, Dictionary<string, object> xModReturnedInfo, out string finalQuoteResult)
        {
            //Comment : Here set default value
            finalQuoteResult = string.Empty;

            try
            {
                #region Comment : Here Questionnaire interface refernce to do/make process all business logic

                //IQuestionnaire questionnaire = GetQuestionnireProvider();

                #endregion

                #region Comment : Here Scenario - 1 Are multiple x-mod numbers or any error messages found?

                //Add XModValue
                var hasMultipleXmods = this.GetCustomKeyValue(xModReturnedInfo, MultipleXMods);

                if (Convert.ToBoolean(hasMultipleXmods))
                {
                    //Comment : Here log this error
                    LogMe(Constants.LoggingType.Trace, (new StringBuilder()).Append("Error response code in  NCCI XMod call custom condition found "));

                    //Comment : Here also set custom made decision about REFERRAL into session
                    appSession.QuestionnaireVM.QuoteReferralMessage = Constants.CustomQuoteReferralMessage1;

                    //Comment : Here set REFERRAL scenario id to get related details for Mail-Template
                    appSession.QuestionnaireVM.ReferralScenarioId = 6;

                    //Comment : Here incorporated latest requirement chnage with new implementation on 18.12.2015
                    return MakeCommonQuoteStatusDecision(questionServiceResponse, out finalQuoteResult);
                }

                #endregion

                #region Comment : Here Scenario - 2 If the value entered by user in "Total number of claims in past three years" is >= to the value extracted from DB

                try
                {
                    //Comment : Here first get both comparison values
                    var referralThresholdClaim = GetReferralThresholdClaims();
                    var userEnteredClaims = GetNoOfClaimInPastThreeYears(questionServiceResponse.Questions);

                    if (userEnteredClaims >= referralThresholdClaim && referralThresholdClaim > 0)
                    {
                        //Comment : Here log thhis error
                        LogMe(Constants.LoggingType.Trace, (new StringBuilder())
                            .Append
                            (
                                string.Format("{0} ReferralClaims:{1}, UserClaims:{2}", "Total number of claims in past three years condition found ", referralThresholdClaim, userEnteredClaims)
                            ));

                        //Comment : Here also set custom made decision about REFERRAL into session
                        appSession.QuestionnaireVM.QuoteReferralMessage = Constants.CustomQuoteReferralMessage2;

                        //Comment : Here set REFERRAL scenario id to get related details for Mail-Template
                        appSession.QuestionnaireVM.ReferralScenarioId = 7;

                        //Comment : Here incorporated latest requirement chnage with new implementation on 18.12.2015
                        return MakeCommonQuoteStatusDecision(questionServiceResponse, out finalQuoteResult);
                    }
                }
                catch (Exception ex)
                {
                    LoggingService.Instance.Fatal("Error occurred while validating 'Total number of claims in past three years condition found '", ex);
                }

                #endregion

                #region Comment : Here Scenario - 3 If it's a good state then in case questions response throws hard referral, display decline page, else display referral page.

                var sessionQuoteVM = (appSession != null && appSession.QuoteVM != null) ? appSession.QuoteVM : null;

                try
                {                    
                    //Comment : Here only if GoodOrBadState applicable then check at next level
                    if (sessionQuoteVM.IsGoodStateApplicable && !sessionQuoteVM.IsGoodState.IsNull() && sessionQuoteVM.IsGoodState.Value)
                    {
                        //Comment : Here log thhis error
                        LogMe(Constants.LoggingType.Trace, (new StringBuilder())
                            .Append
                            (
                            string.Format("{0} IsGoodStateApplicable:{1}, IsGoodStateValue:{2}", "Good state custom condition found ", sessionQuoteVM.IsGoodStateApplicable, sessionQuoteVM.IsGoodState.Value)
                            ));

                        //Comment : Here also set custom made decision about REFERRAL into session
                        appSession.QuestionnaireVM.QuoteReferralMessage = Constants.CustomQuoteReferralMessage3;

                        //Comment : Here set REFERRAL scenario id to get related details for Mail-Template
                        appSession.QuestionnaireVM.ReferralScenarioId = 3;

                        //Comment : Here question QuoteStatus will also be required to make decision along with this GoodOrBadState
                        return MakeCommonQuoteStatusDecision(questionServiceResponse, out finalQuoteResult);
                    }
                }
                catch (Exception ex)
                {
                    LoggingService.Instance.Fatal("Error occurred while validating 'Good state custom condition found '", ex);
                }

                #endregion                

                #region Comment : Here Scenario - 4 If MS/MC Considered then in case questions response throws hard referral, display decline page, else display referral page.

                try
                {                    
                    //Comment : Here only if MultiClass primary exposure INVALID or MultiState checked/selected applicable then check at next level
                    if (sessionQuoteVM.IsMultiStateApplicable || !sessionQuoteVM.IsMultiClassPrimaryExposureValid || (sessionQuoteVM.MoreClassRequired && sessionQuoteVM.IsMultiClass))
                    {
                        //Comment : Here log thhis error
                        LogMe(Constants.LoggingType.Trace, (new StringBuilder())
                            .Append
                            (
                            string.Format("{0} IsMultiStateApplicable:{1}, IsMultiClassPrimaryExposureValid:{2}, MoreClassOrMutliClass:{3}", "MS/MC custom condition found ", sessionQuoteVM.IsMultiStateApplicable, !sessionQuoteVM.IsMultiClassPrimaryExposureValid, (sessionQuoteVM.MoreClassRequired && sessionQuoteVM.IsMultiClass))
                            ));

                        //Comment : Here also set custom made decision about REFERRAL into session
                        appSession.QuestionnaireVM.QuoteReferralMessage = Constants.CustomQuoteReferralMessage4;

                        #region Comment : Here based on different condition set REFERRAL scenario id

                        //Comment : Here set REFERRAL scenario id to get related details for Mail-Template
                        appSession.QuestionnaireVM.ReferralScenarioId
                            = (sessionQuoteVM.IsMultiStateApplicable) ? 4 : ((!sessionQuoteVM.IsMultiClassPrimaryExposureValid) ? 5 : 0);

                        #endregion

                        //Comment : Here question QuoteStatus will also be required to make decision along with this GoodOrBadState
                        return MakeCommonQuoteStatusDecision(questionServiceResponse, out finalQuoteResult);
                    }
                }
                catch (Exception ex)
                {
                    LoggingService.Instance.Fatal("Error occurred while validating 'MS/MC custom condition found '", ex);
                }

                #endregion                

                #region Comment : Here Scenario - 5 If BusinessCalss DirectSales "E" then base on QR display referral page [Hard/Soft].
                
                try
                {
                    if (sessionQuoteVM.BusinessClassDirectSales.Equals("E", StringComparison.OrdinalIgnoreCase))
                    {
                        //Comment : Here log thhis error
                        LogMe(Constants.LoggingType.Trace, (new StringBuilder()).Append("Send to Soft Referral as Direct Sales is 'E' (Having Referral Only Class)"));


                        //Comment : Here also set custom made decision about REFERRAL into session
                        appSession.QuestionnaireVM.QuoteReferralMessage = Constants.CustomQuoteReferralMessage6;

                        //Comment : Here set REFERRAL scenario id to get related details for Mail-Template
                        appSession.QuestionnaireVM.ReferralScenarioId = 2;

                        return MakeCommonQuoteStatusDecision(questionServiceResponse, out finalQuoteResult);                        
                    }
                }
                catch (Exception ex)
                {
                    LoggingService.Instance.Fatal("Error occurred while validating 'DirectSales=E custom condition found '", ex);
                }

                #endregion

                #region Comment : Here Scenario - 6 If all custom validation is bypassed then take final decision on returned provider response QuestionsResponse

                // ***** questions ok: successful quote - user is presented with a payment information page
                if (questionServiceResponse.QuoteStatus == Quote)
                {
                    //Comment : Here must update custom decision status in session object
                    finalQuoteResult = Quote;

                    // return the quote results
                    return (new { resultStatus = "OK", resultText = Quote });
                }

                // ***** questions result in SOFT REFERRAL (no policy will be added to GUARD systems; end user sees same referral page as above; email sent to designated GUARD resource) 
                else if (questionServiceResponse.QuoteStatus == QuoteAPIRefer)
                {
                    //Comment : Here also set custom made decision about REFERRAL into session
                    appSession.QuestionnaireVM.QuoteReferralMessage = questionServiceResponse.ResultMessages.ToString();

                    //Comment : Here set REFERRAL scenario id to get related details for Mail-Template
                    appSession.QuestionnaireVM.ReferralScenarioId = 8;

                    //Comment : Here must update custom decision status in session object
                    finalQuoteResult = QuoteSoftReferral;

                    return (new { resultStatus = "OK", resultText = QuoteSoftReferral });
                }

                // ***** questions result in HARD REFERRAL (user is presented with a referral page; policy needs to get added to GUARD systems) 
                else if (questionServiceResponse.QuoteStatus == QuoteAPIDecline)
                {
                    //Comment : Here must update custom decision status in session object
                    finalQuoteResult = QuoteHardReferral;

                    return (new { resultStatus = "OK", resultText = QuoteHardReferral });
                }

                #endregion

            }
            catch (Exception ex) { LogMe(Constants.LoggingType.Error, (new StringBuilder()).Append(ex.Message)); throw ex; }

            return (new { resultStatus = "NOK", resultText = "Something went wrong while making redirect decision !" });
        }

        public bool DeleteModifierForQuote(int quoteId, string lineOfBusiness)
        {
            try
            {
                //Comment : Here check appSession object 
                if (appSession != null)
                {
                    #region Comment : Here local variables declaration & initialization

                    string zipCode = string.Empty, stateCode = string.Empty;

                    #endregion

                    #region Comment : Here if both required attributes found then take next(DB Call) action

                    var sesionQuoteVM = GetQuoteVM();
                    if (sesionQuoteVM != null)
                    {
                        zipCode = appSession.ZipCode ?? string.Empty;
                        stateCode = appSession.StateAbbr ?? string.Empty;
                    }

                    if (zipCode.Length > 0 && stateCode.Length > 0)
                    {
                        //var modifier = new BHIC.Domain.Policy.Modifier() { QuoteId = quoteId, ModType = "ExMod", ModValue = 0, LOB = lineOfBusiness, ZipCode = zipCode, State = stateCode };
                        var modifierRequestParms = new BHIC.Domain.Policy.ModifierRequestParms() { QuoteId = quoteId, Lob = lineOfBusiness ?? LineOfBusiness };
                        var serviceResponse = SvcClient.CallService<BHIC.Domain.Policy.ModifierRequestParms, OperationStatus>("Modifiers", "DELETE", modifierRequestParms, serviceProvider);

                        if (serviceResponse.RequestSuccessful)
                        {
                            //Comment : Here reset modifier id into session object
                            appSession.QuestionnaireVM.XModFactorModifierId = null;

                            return true;
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex) { LogMe(Constants.LoggingType.Fatal, (new StringBuilder()).Append(ex.Message)); throw ex; }

            return false;
        }

        #region Utility methods

        /// <summary>
        /// Fien-number length and validation
        /// </summary>
        /// <param name="feinNumber"></param>
        /// <returns></returns>
        public bool IsValidFeinNumber(string feinNumber)
        {
            return (!string.IsNullOrEmpty(feinNumber) && feinNumber.Length == 9) ? true : false;
        }

        /// <summary>
        /// Strips extra characters from fien-number string
        /// </summary>
        /// <param name="feinNumber"></param>
        /// <returns></returns>
        public string StripFeinNumber(string feinNumber)
        {
            return !string.IsNullOrEmpty(feinNumber) ? UtilityFunctions.ToNumeric(feinNumber) : null;
        }

        private void LogMe(Constants.LoggingType loggingType, StringBuilder sb)
        {
            //Comment : Here declare class refernce
            ILoggingService loggingService = LoggingService.Instance;

            //Comment : Here based on loggingType
            switch (loggingType)
            {
                case Constants.LoggingType.Trace: loggingService.Trace(sb.ToString()); break;
                case Constants.LoggingType.Debug: loggingService.Debug(sb.ToString()); break;
                case Constants.LoggingType.Info: loggingService.Info(sb.ToString()); break;
                case Constants.LoggingType.Warn: loggingService.Warn(sb.ToString()); break;
                case Constants.LoggingType.Error: loggingService.Error(sb.ToString()); break;
                case Constants.LoggingType.Fatal: loggingService.Fatal(sb.ToString()); break;
            }
        }

        #endregion

        #region POST methods

        /// <summary>
        /// Post user response on questionnaire to get premium amount for quote
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public QuestionsResponse PostQuestionnaire(QuestionsResponse postedQuestionnaireResponse)
        {
            //TEMPORARY : posting through service implementation generating some error that is why direct SvcClient call made
            var serviceResponse = SvcClient.CallServicePost<QuestionsResponse, QuestionsResponse>(Questions, postedQuestionnaireResponse, serviceProvider);

            //Comment : Here update this posted quesionnaire to get premium amount from service provider system

            return serviceResponse ?? new QuestionsResponse();
        }

        #endregion

        #endregion

        #region Methods : additional public methods

        /// <summary>
        /// Retrun QuoteVM object for session data manipulation
        /// </summary>
        /// <returns></returns>
        public QuoteViewModel GetQuoteVM()
        {
            try
            {
                //Comment : Here check appSession object 
                if (appSession != null)
                {
                    if (appSession.QuoteVM != null)
                    {
                        return appSession.QuoteVM;
                    }
                }
            }
            catch { }

            return null;
        }

        /// <summary>
        /// Retrun QuestionnaireVM object for question page session data manipulation
        /// </summary>
        /// <returns></returns>
        public QuestionnaireViewModel GetQuestionnaireVM()
        {
            try
            {
                //Comment : Here check appSession object 
                if (appSession != null)
                {
                    if (appSession.QuestionnaireVM != null)
                    {
                        return appSession.QuestionnaireVM;
                    }
                }
            }
            catch { }

            return null;
        }

        /// <summary>
        /// Return true if this is MutilState quote
        /// </summary>
        /// <param name="appSession"></param>
        /// <returns></returns>
        public bool IsMultiStateQuote()
        {
            try
            {
                //Comment : Here check appSession object 
                if (appSession != null)
                {
                    return appSession.QuoteVM.IsMultiState;
                }
            }
            catch { }

            return false;
        }

        /// <summary>
        /// Return true if this is MutilClass quote
        /// </summary>
        /// <param name="appSession"></param>
        /// <returns></returns>
        public bool IsMultiClassQuote()
        {
            try
            {
                //Comment : Here check appSession object 
                if (appSession != null)
                {
                    return appSession.QuoteVM.IsMultiClass;
                }
            }
            catch { }

            return false;
        }

        /// <summary>
        /// Retuen generic DbConnector object to communicate with database
        /// </summary>
        /// <returns></returns>
        public BHICDBBase GetDbConnector()
        {
            BHICDBBase dbConnector = new BHICDBBase();

            #region Comment : Here using XmlReader get DB connection string

            dbConnector.DBName = "GuinnessDB";

            #endregion

            return dbConnector;
        }

        /// <summary>
        /// Get specific key's value from dictionary
        /// </summary>
        /// <param name="xModReturnedInfo"></param>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public object GetCustomKeyValue(Dictionary<string, object> xModReturnedInfo, string keyName)
        {
            try
            {
                if (xModReturnedInfo != null && xModReturnedInfo.Count > 0)
                {
                    var retKeysValue =
                                    (
                                        from nvc in xModReturnedInfo
                                        where nvc.Key.Contains(keyName)
                                        select nvc.Value
                                    ).FirstOrDefault();

                    return retKeysValue;
                }
            }
            catch { }

            return null;
        }

        /// <summary>
        /// Make common QuoteStatus decision for multiple scenarios
        /// </summary>
        /// <param name="questionServiceResponse"></param>
        /// <param name="finalQuoteResult"></param>
        /// <returns></returns>
        public object MakeCommonQuoteStatusDecision(QuestionsResponse questionServiceResponse, out string finalQuoteResult)
        {
            //Comment : Here set default value
            finalQuoteResult = string.Empty;

            if (questionServiceResponse.QuoteStatus == QuoteAPIDecline)
            {
                //Comment : Here must update custom decision status in session object
                finalQuoteResult = QuoteHardReferral;

                return (new { resultStatus = "OK", resultText = QuoteHardReferral });
            }
            else
            {
                //Comment : Here must update custom decision status in session object
                finalQuoteResult = QuoteSoftReferral;

                return (new { resultStatus = "OK", resultText = QuoteSoftReferral });
            }
        }

        /// <summary>
        /// On user save-for-later submission add running quote into PC dashboard
        /// </summary>
        /// <param name="loggedUserId"></param>
        /// <param name="wcQuoteId"></param>
        /// <param name="emailId"></param>
        public void AddQuoteToDashboard(int? loggedUserId, int wcQuoteId, string emailId)
        {
            var quoteCreatedinDB = false;

            try
            {

                #region Comment : Here QuoteSummary interface refernce to do/make process all business logic

                //Comment : Here BLL stands for "Business Logic Layer"
                IQuoteSummary quoteSummaryBLL = new QuoteSummary(appSession, serviceProvider);

                #endregion

                #region Comment : Here STEP - 1. Insert this new generated quote into database for future FK refernces

                quoteCreatedinDB = quoteSummaryBLL.AddUpdateQuoteData(new BHIC.DML.WC.DTO.QuoteDTO()
                {
                    QuoteNumber = wcQuoteId.ToString(),
                    PremiumAmount = !appSession.QuestionnaireVM.IsNull() ? appSession.QuestionnaireVM.PremiumAmt : 0,
                    LineOfBusinessId = 1, //Right now hard coded it will come in session from home page in future
                    ExternalSystemID = 1, //Right now hard coded
                    IsActive = true,
                    RequestDate = DateTime.Now,
                    AgencyCode = !appSession.QuestionnaireVM.IsNull() ? appSession.QuestionnaireVM.Agency : string.Empty,
                    CreatedBy = loggedUserId ?? 1,
                    CreatedDate = DateTime.Now,
                    ExpiryDate = DateTime.Now.AddYears(1),
                    ModifiedDate = DateTime.Now,
                    ModifiedBy = loggedUserId ?? 1
                });


                #endregion

                #region Comment : Here STEP - 2. Updated newly created Quote with logged in user detail

                if (quoteCreatedinDB && loggedUserId != null)
                {
                    PurchaseQuote purchaseQuote = new PurchaseQuote();

                    // Calling Quote linking with Logged in user.
                    purchaseQuote.UpdateQuoteUserId(new BHIC.DML.WC.DTO.OrganisationUserDetailDTO()
                    {
                        EmailID = emailId
                    },
                    new DML.WC.DTO.QuoteDTO()
                    {
                        QuoteNumber = wcQuoteId.ToString(),
                        OrganizationUserDetailID = loggedUserId,
                        ModifiedBy = loggedUserId ?? 1,
                        ModifiedDate = DateTime.Now
                    });
                }

                #endregion

                #region Comment : Here STEP - 3. Persist this quote into DB which will be used for PC "Saved Quotes" functionalities

                if (quoteCreatedinDB && loggedUserId != null)
                {
                    //Comment : Here CommonFunctionality interface refernce to do/make process all business logic
                    ICommonFunctionality commonFunctionality = new CommonFunctionality();

                    //Comment : Here call BLL to make this data stored in DB
                    bool isStoredInDB = commonFunctionality.AddApplicationCustomSession(
                        new BHIC.DML.WC.DTO.CustomSession() { QuoteID = wcQuoteId, SessionData = commonFunctionality.StringifyCustomSession(appSession), IsActive = true, CreatedDate = DateTime.Now, CreatedBy = loggedUserId ?? 1, ModifiedDate = DateTime.Now, ModifiedBy = loggedUserId ?? 1 });
                }

                #endregion                
            }
            catch (Exception ex)
            {
                LogMe(Constants.LoggingType.Fatal, (new StringBuilder()).Append(ex.Message));
            }

            #region Comment : Here in case not able to save quote details at DB layer

            //Comment : Here must check is this quote details successfull register/saved on DB layer becoz these will be used in in future while making payment details on BuyPolicy page
            if (!quoteCreatedinDB)
            {
                LogMe(Constants.LoggingType.Trace, (new StringBuilder()).Append("Failed to add session state in DB"));
            }

            #endregion

        }

        private void GetReferralProcessedData(string xmlFilePath, List<int> currentReferralScenarioIds, ReferralHistory referralHistory)
        {
            //Comment : Here get first list of master data for referral reason scenarios and then all scenarios "Reason & Description" list 
            IReferralQuote referralQuoteBLL = new ReferralQuote(appSession, serviceProvider);

            var referralProcessing = new BHIC.Domain.PurchasePath.ReferralProcessing()
            {
                ReferralScenarioIds = new List<int>(currentReferralScenarioIds),
                FilePath = xmlFilePath
            };

            //Comment : Here get all current referral reason & descriptions
            referralQuoteBLL.GetAllReferralReasonsNew(referralProcessing);

            if (currentReferralScenarioIds.Any())
            {
                referralHistory.XModValueMessage = referralProcessing.XModValueMessage;
                referralHistory.ReferralScenarioIdsList.Add(currentReferralScenarioIds);
                referralHistory.ReferralScenarioTextList.Add
                (
                    new ReferralData()
                    {
                        ReasonsList = new List<string>(referralProcessing.ReasonsList),
                        DescriptionList = new List<string>(referralProcessing.DescriptionList),
                    }
                );
            }
        }

        #endregion

        #endregion
    }
}
