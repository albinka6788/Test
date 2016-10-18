#region Using directives

using System;

#endregion

namespace BHIC.DML.WC.DataContract
{
    public interface IPolicyDataProvider
    {
        /// <summary>
        /// Store PolicyData into database
        /// </summary>
        /// <param name="policy">Policy DTO</param>
        /// <returns>Returns true on success, false otherwise</returns>
        int AddPolicyData(PolicyDTO policy, PolicyPaymentDetailDTO policyPaymentDetails = null);

        /// <summary>
        /// Get PolicyId by policy number
        /// </summary>
        /// <param name="policyNumber"></param>
        /// <returns>returns required policy id on success, 0 otherwise</returns>
        int GetPolicyIdByPolicyNumber(string policyNumber);

        /// <summary>
        /// It will check if give transaction code is exists on record or not
        /// </summary>
        /// <param name="transactionCode"></param>
        /// <returns>return true if exists, false otherwise</returns>
        bool GetPolicyPaymentIdByTransactionCode(string transactionCode);
    }
}
