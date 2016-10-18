#region Use Directive

using BHIC.Domain.Background;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DML_DTO = BHIC.DML.WC.DTO;
using VM = BHIC.ViewDomain;

#endregion

namespace BHIC.Contract.PurchasePath
{
    public interface ICommonFunctionality
    {
        #region Methods

        /// <summary>
        /// Add or update application current session data into DB layer
        /// </summary>
        /// <param name="customSession"></param>
        /// <returns></returns>
        bool AddApplicationCustomSession(DML_DTO::CustomSession customSession);

        /// <summary>
        /// Retrieve string data of stored quote session data
        /// </summary>
        /// <param name="quoteId"></param>
        /// <returns></returns>
        string GetApplicationCustomSession(int quoteId, int userId);

        /// <summary>
        /// Method will stringify custom session object into string 
        /// </summary>
        /// <param name="customSession"></param>
        string StringifyCustomSession(VM::CustomSession customSession);

        /// <summary>
        /// Method will return deserialize custom session object from stringified object of custom session
        /// </summary>
        /// <param name="customSession"></param>
        VM::CustomSession GetDeserializedCustomSession(string customSession);

        /// <summary>
        /// Validate FEIN/SSN/TIN number
        /// </summary>
        /// <param name="taxIdNumber"></param>
        /// <returns>return true if valid string, false otherwise</returns>
        bool ValidateTaxIdAndSSN(string taxIdNumber);

        /// <summary>
        /// Compare PaymentPlan objects
        /// </summary>
        /// <param name="selectedPaymentPlan"></param>
        /// <param name="matchedPaymentPlan"></param>
        /// <returns></returns>
        List<string> ComparePlanObjects(PaymentPlan selectedPaymentPlan, PaymentPlan matchedPaymentPlan);

        /// <summary>
        /// To Fetch Loss Contol File Name from Guid
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        string GetLossControlFileName(string Guid);

        #endregion
    }
}
