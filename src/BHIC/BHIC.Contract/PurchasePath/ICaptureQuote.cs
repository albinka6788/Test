using BHIC.DML.WC.DTO;
using BHIC.Domain.Background;
using BHIC.Domain.Policy;
using BHIC.ViewDomain.Landing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Contract.PurchasePath
{
    public interface ICaptureQuote
    {
        /// <summary>
        /// This method will retrieve all good and bad states
        /// </summary>
        /// <param name="stateCode"></param>
        /// <returns></returns>
        List<StateType> GetListOfGoodAndBadState();

        /// <summary>
        /// Insert Business Class Keyword. These Keywords are the understand search options provided by users to find there business.
        /// </summary>
        /// <param name="cybClassKeyword"></param>
        void AddClassKeyword(string cybClassKeyword);

        /// <summary>
        /// Returns First County matched
        /// </summary>
        /// <param name="zipCode"></param>
        /// <returns></returns>
        County GetCountyData(string zipCode, string state);

        /// <summary>
        /// Returns business Names list based on Search String 
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="ClassDescriptionKeywordId"></param>
        /// <returns></returns>
        List<ClassDescriptionKeyword> GetBusinessNamesList(string searchString, int? classDescriptionKeywordId, string zipCode, string stateAbbr);

        /// <summary>
        /// Get Exposures List
        /// </summary>
        /// <param name="quoteId"></param>
        /// <returns></returns>
        List<Exposure> GetExposureList(int quoteId);

        /// <summary>
        /// Get Companion Classes based on parameters provided
        /// </summary>
        /// <param name="classDescId"></param>
        /// <param name="state"></param>
        /// <param name="zipCode"></param>
        /// <returns></returns>
        List<CompanionClass> FetchCompanionClasses(int classDescId, string state, string zipCode);

        /// <summary>
        /// Get Primary Class Code data threshold percentage and friendly name
        /// </summary>
        /// <param name="stateCode"></param>
        /// <param name="classDescriptionId"></param>
        /// <returns></returns>
        PrimaryClassCodeDTO GetMinimumPayrollThreshold(string stateCode, int classDescriptionId);

        /// <summary>
        /// Get Quote Details
        /// </summary>
        /// <param name="quoteId"></param>
        /// <param name="expList"></param>
        /// <param name="policyData"></param>
        void GetQuoteDetails(int quoteId, ref List<Exposure> expList, ref PolicyData policyData);

        /// <summary>
        /// Validates Exposure data submitted to Server
        /// </summary>
        /// <param name="request"></param>
        /// <param name="listOfErrors"></param>
        /// <param name="appSession"></param>
        /// <returns></returns>
        bool ValidateExposureData(WcHomePageViewModel request, ref List<string> listOfErrors, BHIC.ViewDomain.CustomSession appSession);

        /// <summary>
        /// Get Exposures List if Any for a Quote Id
        /// </summary>
        /// <param name="quoteId"></param>
        /// <returns></returns>
        List<Exposure> GetCompanionClassExposureList(int quoteId);

        /// <summary>
        /// Get specified quote User ID
        /// </summary>
        /// <param name="quoteNumber"></param>
        /// <returns></returns>
        int GetQuoteUserID(string quoteNumber);

    }
}
