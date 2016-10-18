#region Using Directives

using BHIC.Common.Mailing;
using BHIC.Domain.PurchasePath;
using BHIC.ViewDomain;
using BHIC.ViewDomain.QuestionEngine;
using System.Collections.Generic;

#endregion

namespace BHIC.Contract.PurchasePath
{
    public interface IReferralQuote
    {
        #region Main methods

        /// <summary>
        /// Prepare mail content static and dynamically for SoftReferral quote new process
        /// </summary>
        /// <param name="referralQuoteVM"></param>
        /// <param name="referralReasons"></param>
        /// <param name="listOfUploadedFiles"></param>
        /// <returns></returns>
        bool ProcessSoftReferralMail(ReferralQuoteMailViewModel referralQuoteVM, ReferralReasons referralReasons, List<string> listOfUploadedFiles);

        /// <summary>
        /// Prepare mail content static and dynamically for SoftReferral quote new process
        /// </summary>
        /// <param name="referralQuoteVM"></param>
        /// <param name="listOfUploadedFiles"></param>
        /// <returns></returns>
        bool ProcessSoftReferralMailNew(ReferralQuoteMailViewModel referralQuoteVM, List<string> listOfUploadedFiles);

        /// <summary>
        /// Get list of referral reasons and descirption of supplied ReferralScenarioList
        /// </summary>
        /// <param name="referralProcessingData"></param>
        void GetAllReferralReasonsNew(ReferralProcessing referralProcessingData);

        #endregion
    }
}
