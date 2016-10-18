#region Using directives

using System;

using BHIC.Common.DataAccess;
using BHIC.DML.WC.DataService;

#endregion

namespace BHIC.DML.WC.DataContract
{
    public interface IPolicyPaymentDetailProvider
    {
        /// <summary>
        /// Store PolicyPaymentDetail into database
        /// </summary>
        /// <param name="policyPaymentDetail">PolicyPaymentDetail DTO</param>
        /// <param name="bhicDBBase"></param>
        /// <returns>Returns true on success, false otherwise</returns>
        bool AddPolicyPaymentDetail(PolicyPaymentDetailDTO policyPaymentDetail, IBHICDBBase bhicDBBase = null);
    }
}
