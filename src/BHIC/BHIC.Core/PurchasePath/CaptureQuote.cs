#region Using Directives

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

using BHIC.Common;
using BHIC.Common.Caching;
using BHIC.Common.Client;
using BHIC.Common.Config;
using BHIC.Common.DataAccess;
using BHIC.Contract.Background;
using BHIC.Contract.Policy;
using BHIC.Contract.PurchasePath;
using BHIC.Core.Background;
using BHIC.Core.Masters;
using BHIC.Core.Policy;
using BHIC.DML.WC.DataContract;
using BHIC.DML.WC.DataService;
using BHIC.DML.WC.DTO;
using BHIC.Domain.Background;
using BHIC.Domain.Policy;
using BHIC.ViewDomain.Landing;
using BHIC.Common.Logging;

using DML_DC = BHIC.DML.WC.DataContract;
using DML_DS = BHIC.DML.WC.DataService;

#endregion

namespace BHIC.Core.PurchasePath
{
    public class CaptureQuote : ICaptureQuote, IDisposable
    {
        internal ServiceProvider guardServiceProvider = new GuardServiceProvider() { ServiceCategory = ServiceProviderConstants.GuardServiceCategoryWC };

        #region Variables

        CacheHelper cache;

        #endregion

        #region Constructor

        public CaptureQuote()
        {
            cache = CacheHelper.Instance;
        }

        #endregion

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
        /// Return interface refernce to make all database manipulation language(DML) logic
        /// </summary>
        /// <returns></returns>
        private DML_DC::IQuoteDataProvider GetQuoteProviderDML()
        {
            #region Comment : Here QuoteSummary interface refernce to do/make process all DML logic

            DML_DC::IQuoteDataProvider quoteData = new DML_DS::QuoteDataProvider();

            #endregion

            return quoteData;
        }

        /// <summary>
        /// This method will retrieve the state whether it is good or bad type
        /// </summary>
        /// <param name="stateCode"></param>
        /// <returns></returns>
        public List<StateType> GetListOfGoodAndBadState()
        {
            List<StateType> states = new List<StateType>();

            try
            {
                var rdr = GetDbConnector().ExecuteReader("GetStateType", QueryCommandType.StoredProcedure);

                while (rdr.Read())
                {
                    states.Add(new StateType { IsGoodState = Convert.ToBoolean(rdr["StateType"]), StateCode = Convert.ToString(rdr["StateCode"]) });
                }
            }
            catch
            {

            }

            return states;
        }


        /// <summary>
        /// Insert Business Class Keyword. These Keywords are the understand search options provided by users to find there business.
        /// </summary>
        /// <param name="cybClassKeyword"></param>
        public void AddClassKeyword(string cybClassKeyword)
        {
            try
            {
                if (System.Web.HttpContext.Current != null)
                {
                    BHIC.Common.Reattempt.ReattemptLog.Register(System.Reflection.MethodBase.GetCurrentMethod(), cybClassKeyword);
                }
                else
                {
                    DML_DC::IQuoteDataProvider quoteDataProvider = GetQuoteProviderDML();

                    quoteDataProvider.AddClassKeywords(cybClassKeyword);
                }
            }
            catch (Exception ex)
            {
                LoggingService.Instance.Fatal(ex);
            }
        }

        /// <summary>
        /// Save Exposure Data and Policy Data
        /// </summary>
        /// <param name="exposureData"></param>
        /// <param name="policyData"></param>
        /// <returns></returns>
        public bool SaveExposureData(Exposure exposureData, PolicyData policyData)
        {
            return true;
        }

        /// <summary>
        /// Returns First County matched
        /// </summary>
        /// <param name="zipCode"></param>
        /// <returns></returns>
        public County GetCountyData(string zipCode, string state)
        {
            ICountyService countyService = new CountyService(guardServiceProvider);
            var countyData = countyService.GetCounty(false);
            if (!countyData.IsNull() && countyData.Any(x => x.ZipCode.Equals(zipCode) && x.State.Equals(state)))
            {
                var filteredList = countyData.Where(x => x.ZipCode.Equals(zipCode) && x.State.Equals(state)).First();
                return filteredList;
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// Return Business List Based on Search String and ClassDescriptionKeywordId
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="classDescriptionId"></param>
        /// <returns></returns>
        public List<ClassDescriptionKeyword> GetBusinessNamesList(string searchString, int? classDescriptionKeywordId, string zipCode, string stateAbbr)
        {
            IClassDescKeywordService classDescKeywordService = new ClassDescKeywordService();
            return classDescKeywordService.GetClassDescKeywordList(new ClassDescKeywordRequestParms { LOB = Constants.GetLineOfBusiness(Constants.LineOfBusiness.WC), SearchString = searchString, ClassDescKeywordId = classDescriptionKeywordId, State = stateAbbr, ZipCode = zipCode }, guardServiceProvider, false);
        }

        /// <summary>
        /// Get Exposures List if Any for a Quote Id
        /// </summary>
        /// <param name="quoteId"></param>
        /// <returns></returns>
        public List<Exposure> GetExposureList(int quoteId)
        {
            IExposureService exposureService = new ExposureService();
            try
            {
                List<Exposure> exposureResponse = exposureService.GetExposureList(new ExposureRequestParms { Lob = Constants.GetLineOfBusiness(Constants.LineOfBusiness.WC), QuoteId = quoteId }, guardServiceProvider);
                if (exposureResponse != null && exposureResponse.Count > 0)
                {
                    return exposureResponse;
                }
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Get Exposures List if Any for a Quote Id
        /// </summary>
        /// <param name="quoteId"></param>
        /// <returns></returns>
        public List<Exposure> GetCompanionClassExposureList(int quoteId)
        {
            IExposureService exposureService = new ExposureService();
            try
            {
                List<Exposure> exposureResponse = GetExposureList(quoteId);
                if (exposureResponse != null && exposureResponse.Count > 0)
                {
                    return exposureResponse.ExceptWhere(x => x.CompanionClassId.IsNull()).ToList();
                }
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Get Companion Classes based on parameters provided
        /// </summary>
        /// <param name="classDescId"></param>
        /// <param name="state"></param>
        /// <param name="zipCode"></param>
        /// <returns></returns>
        public List<CompanionClass> FetchCompanionClasses(int classDescId, string state, string zipCode)
        {
            try
            {
                ICompanionClassService companionClassService = new CompanionClassService(guardServiceProvider);
                var companionClassResponse = companionClassService.GetCompanionClasses(new CompanionClassRequestParms { ClassDescriptionId = classDescId, State = state, ZipCode = zipCode });
                if (null != companionClassResponse && companionClassResponse.Count > 0)
                {
                    foreach (var a in companionClassResponse)
                    {
                        if (a.HelpText.Equals("-none-", StringComparison.OrdinalIgnoreCase) || String.IsNullOrWhiteSpace(a.HelpText))
                        {
                            a.HelpText = null;
                        }
                    }
                    return companionClassResponse;
                }
            }
            catch
            {
                return null;
            }
            return null;
        }

        /// <summary>
        /// Get Primary Class Code data threshold percentage and friendly name
        /// </summary>
        /// <param name="stateCode"></param>
        /// <param name="classDescriptionId"></param>
        /// <returns></returns>
        public PrimaryClassCodeDTO GetMinimumPayrollThreshold(string stateCode, int classDescriptionId = 1)
        {
            IPrimaryClassCodeService primaryClassCodeDataProvider;
            List<PrimaryClassCodeDTO> primaryClassCodeList;
            PrimaryClassCodeDTO selectedData;
            try
            {
                primaryClassCodeDataProvider = new PrimaryClassCodeService();
                primaryClassCodeList = primaryClassCodeDataProvider.GetPrimaryClassCodeDataList();
                bool hasData = primaryClassCodeList.Any(x => x.ClassDescriptionId == classDescriptionId);
                if (hasData)
                {

                    var filteredData = primaryClassCodeList.Where(x => x.ClassDescriptionId == classDescriptionId).ToList();
                    if ((stateCode == "CA" || stateCode == "CT" || stateCode == "LA" || stateCode == "RI" || stateCode == "TX")
                    && filteredData.Any(x => x.StateCode == "CA" || x.StateCode == "CT" || x.StateCode == "LA" || x.StateCode == "RI" || x.StateCode == "TX"))
                    {
                        selectedData = filteredData.Where(x => x.StateCode == stateCode).First();
                    }
                    else
                    {
                        selectedData = filteredData.First();
                    }
                }
                else
                {
                    selectedData = new PrimaryClassCodeDTO();
                    selectedData.MinimumPayrollThreshold = 0.65M;
                }
            }
            catch
            {
                return null;
            }
            return selectedData;
        }

        /// <summary>
        /// Gets All Information related to a particular Quote Id
        /// </summary>
        /// <param name="quoteId"></param>
        /// <param name="expList"></param>
        /// <param name="policyData"></param>
        public void GetQuoteDetails(int quoteId, ref List<Exposure> expList, ref PolicyData policyData)
        {
            IQuoteService quoteService = new QuoteService(guardServiceProvider);
            try
            {
                Quote quoteResponse = quoteService.GetQuote(new QuoteRequestParms
                {
                    LOB = Constants.GetLineOfBusiness(Constants.LineOfBusiness.WC),
                    QuoteId = quoteId,
                    IncludeRelatedPolicyData = true,
                    IncludeRelatedRatingData = false,
                    IncludeRelatedPaymentTerms = false,
                    IncludeRelatedInsuredNames = false,
                    IncludeRelatedExposuresGraph = true,
                    IncludeRelatedOfficers = false,
                    IncludeRelatedLocations = false,
                    IncludeRelatedContactsGraph = false,
                    IncludeRelatedQuestions = false,
                    IncludeRelatedQuoteStatus = false
                });
                if (quoteResponse != null && quoteResponse.LobDataList != null && quoteResponse.LobDataList.Count > 0)
                {
                    if (quoteResponse.LobDataList[0].CoverageStates != null && quoteResponse.LobDataList[0].CoverageStates.Count > 0)
                    {
                        expList = quoteResponse.LobDataList[0].CoverageStates[0].Exposures;
                        policyData = quoteResponse.PolicyData;
                    }
                }
            }
            catch (Exception ex)
            {
                BHIC.Common.Logging.ILoggingService logger = BHIC.Common.Logging.LoggingService.Instance;
                logger.Fatal(string.Format("Service {0} Call with error message : {1}", "QuoteService", ex.ToString()));
            }
        }

        /// <summary>
        /// Validates the exposure submission data recived at server side
        /// </summary>
        /// <param name="quoteId"></param>
        /// <param name="expList"></param>
        /// <param name="policyData"></param>
        public bool ValidateExposureData(WcHomePageViewModel request, ref List<string> listOfErrors, BHIC.ViewDomain.CustomSession appSession)
        {
            #region Basic Validations

            #region Date Validation
            if (String.IsNullOrWhiteSpace(request.InceptionDate))
                return false;
            else
            {
                DateTime parsed = DateTime.MinValue;
                bool valid = false;
                try
                {
                    parsed = DateTime.Parse(request.InceptionDate, new System.Globalization.CultureInfo("en-US", true));
                    valid = true;
                }
                catch (Exception ex)
                {
                    BHIC.Common.Logging.LoggingService.Instance.Trace(string.Format("Inception Date value {0}", request.InceptionDate), ex);
                }

                if (valid)
                {
                    string curDate = DateTime.Now.ToString("MM/dd/yyyy");
                    if (!(DateTime.Parse(curDate, new System.Globalization.CultureInfo("en-US", true)) < DateTime.Parse(parsed.ToShortDateString()) && DateTime.Parse(parsed.ToShortDateString()) <= DateTime.Parse(curDate, new System.Globalization.CultureInfo("en-US", true)).AddDays(60)))
                    {
                        listOfErrors.Add(Constants.ExposureDataErrors.INCORRECT_DATE);
                        return false;
                    }
                }
                else
                {
                    listOfErrors.Add(Constants.ExposureDataErrors.INVALID_DATE_FORMAT);
                    return false;
                }
            }
            #endregion Date Validation

            #region Business Selection Validation
            if (request.IndustryId != -1 && request.SubIndustryId != -1 && request.ClassDescriptionId != -1)
            {
                if (
                    //request.ClassDescriptionKeywordId != appSession.QuoteVM.ClassDescriptionKeywordId || 
                    (UtilityFunctions.IsValidInt(request.IndustryId) && request.IndustryId != appSession.QuoteVM.IndustryId)
                    || (UtilityFunctions.IsValidInt(request.SubIndustryId) && request.SubIndustryId != appSession.QuoteVM.SubIndustryId)
                    || request.ClassDescriptionId != appSession.QuoteVM.ClassDescriptionId
                    || request.BusinessClassDirectSales != appSession.QuoteVM.BusinessClassDirectSales
                    || request.ClassCode != appSession.QuoteVM.ClassCode)
                {
                    listOfErrors.Add(Constants.ExposureDataErrors.BUSINESS_INFO_ALTERED);
                    return false;
                }
                else if (!UtilityFunctions.IsValidInt(request.ClassDescriptionKeywordId) && !UtilityFunctions.IsValidInt(request.ClassDescriptionId))
                {
                    listOfErrors.Add(Constants.ExposureDataErrors.BUSINESS_INFO_EMPTY);
                    return false;
                }
                else if (UtilityFunctions.IsValidInt(request.ClassDescriptionKeywordId) && UtilityFunctions.IsValidInt(request.IndustryId))
                {
                    listOfErrors.Add(Constants.ExposureDataErrors.BOTH_SELECTION_NOT_ALLOWED);
                    return false;
                }
                else if ((request.IndustryId == -1 || request.SubIndustryId == -1 || request.ClassDescriptionId == -1) && string.IsNullOrWhiteSpace(request.OtherClassDesc))
                {
                    listOfErrors.Add(Constants.ExposureDataErrors.BUSINESS_INFO_EMPTY);
                    return false;
                }
            }
            #endregion Business Selection Validation

            #region Exposure Amount Validation

            if (String.IsNullOrWhiteSpace(request.ExposureAmt) && String.IsNullOrWhiteSpace(request.TotalPayroll) && !(Convert.ToInt64(UtilityFunctions.ToNumeric(request.TotalPayroll)) > 0))
            {
                listOfErrors.Add(Constants.ExposureDataErrors.EXPOSURE_AMOUNT_EMPTY);
                return false;
            }

            #endregion Exposure Amount Validation

            #region Business Year Validation

            if (request.BusinessYears.IsNull())
            {
                listOfErrors.Add(Constants.ExposureDataErrors.EMPTY_BUSINESSYEAR);
                return false;
            }
            if (!(0 <= request.BusinessYears && request.BusinessYears <= 10))
            {
                listOfErrors.Add(Constants.ExposureDataErrors.INVALID_BUSINESSYEAR);
                return false;
            }
            #endregion

            if (request.IndustryId != -1 && request.SubIndustryId != -1 && request.ClassDescriptionId != -1)
            {
                #region Min Exposure Amount Validation

                //Todo: check for this validation, as it not taking care of good or bad state logic
                if (Convert.ToInt64(request.MinExpValidationAmount) != Convert.ToInt64(appSession.QuoteVM.MinExpValidationAmount))
                {
                    listOfErrors.Add(Constants.ExposureDataErrors.MIN_AMOUNT_VALIDATION_MISMATCH);
                    return false;
                }

                #endregion Min Exposure Amount Validation

                #region Good State Bad State validation

                if (appSession.QuoteVM.IsGoodStateApplicable != request.IsGoodStateApplicable)
                {
                    listOfErrors.Add(Constants.ExposureDataErrors.MIN_AMOUNT_VALIDATION_MISMATCH);
                    return false;
                }

                #endregion Good State Bad State validation
            }
            #endregion Basic Validations

            #region MSMC Validation

            if (request.IndustryId != -1 && request.SubIndustryId != -1 && request.ClassDescriptionId != -1)
            {
                if (Convert.ToInt64(UtilityFunctions.ToNumeric(request.TotalPayroll)) > 50000 && !request.CompClassData.IsNull() && request.CompClassData.Count > 0 && !(String.IsNullOrWhiteSpace(request.TotalPayroll) || String.IsNullOrWhiteSpace(request.ExposureAmt)))
                {
                    if (request.ClassDescriptionId.Value > 0)
                    {
                        List<CompanionClass> reqCompClass = new List<CompanionClass>();
                        foreach (var i in request.CompClassData)
                        {
                            reqCompClass.Add(new CompanionClass { ClassCode = i.ClassCode, ClassSuffix = i.ClassSuffix, CompanionClassId = i.CompanionClassId.HasValue ? i.CompanionClassId.Value : 0, FriendlyLabel = i.FriendlyLabel, HelpText = i.HelpText });
                        }

                        if (!appSession.apiCompClassList.IsNull() && appSession.apiCompClassList.Count > 0 && ((reqCompClass.Count != appSession.apiCompClassList.Count))) //!reqCompClass.CompareList(appSession.apiCompClassList)
                        {
                            listOfErrors.Add(Constants.ExposureDataErrors.MSMC_DATA_NOT_SAME);
                            BHIC.Common.Logging.LoggingService.Instance.Trace(string.Format("CompClass Count different {0}!={1}", reqCompClass.Count, appSession.apiCompClassList.Count));
                            return false;
                        }
                        else if (Convert.ToInt64(request.MinExpValidationAmount) >= Convert.ToInt64(UtilityFunctions.ToNumeric(request.ExposureAmt)))
                        {
                            listOfErrors.Add(Constants.ExposureDataErrors.MSMC_VALIDATION_AMOUNT_GREATER);
                            BHIC.Common.Logging.LoggingService.Instance.Trace(string.Format("Min Exp Validation Failed {0}(MinExpValAmount) >={1}(Primary Class Exposure Amount)", Convert.ToInt64(request.MinExpValidationAmount), Convert.ToInt64(UtilityFunctions.ToNumeric(request.ExposureAmt))));
                            return false;
                        }
                        else
                        {
                            long totalSum = Convert.ToInt64(UtilityFunctions.ToNumeric(request.ExposureAmt));
                            foreach (var compData in request.CompClassData)
                            {
                                if (!String.IsNullOrWhiteSpace(compData.PayrollAmount))
                                {
                                    totalSum += Convert.ToInt64(UtilityFunctions.ToNumeric(compData.PayrollAmount));
                                }
                            }
                            if (totalSum != Convert.ToInt64(UtilityFunctions.ToNumeric(request.TotalPayroll)))
                            {
                                listOfErrors.Add(Constants.ExposureDataErrors.MSMC_DATA_NOT_SAME);
                                BHIC.Common.Logging.LoggingService.Instance.Trace(String.Format("Sum of comp class data not same {0}(total Sum of Exposures) != {1}", totalSum, Convert.ToInt64(UtilityFunctions.ToNumeric(request.TotalPayroll))));
                                return false;
                            }
                        }
                    }
                }
            }
            #endregion MSMC Validation

            return true;
        }

        /// <summary>
        /// Dispose the Object
        /// </summary>
        public void Dispose()
        {
            this.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Get specified quote User ID
        /// </summary>
        /// <param name="quoteNumber"></param>
        /// <returns></returns>
        public int GetQuoteUserID(string quoteNumber)
        {
            IQuoteDataProvider quoteDataProvider = new QuoteDataProvider();
            return quoteDataProvider.GetQuoteUserId(quoteNumber);
        }
    }
}
