#region Using directives

using BHIC.Common.DataAccess;
using BHIC.DML.WC.DataContract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

#endregion

namespace BHIC.DML.WC.DataService
{
    public class PolicyPaymentDetailProvider : BaseDataProvider, IPolicyPaymentDetailProvider
    {
        /// <summary>
        /// Store PolicyPaymentDetail into database
        /// </summary>
        /// <param name="policyPaymentDetail">PolicyPaymentDetail DTO</param>
        /// <param name="bhicDBBase"></param>
        /// <returns>Returns true on success, false otherwise</returns>
        bool IPolicyPaymentDetailProvider.AddPolicyPaymentDetail(PolicyPaymentDetailDTO policyPaymentDetail, IBHICDBBase bhicDBBase = null)
        {
            try
            {
                if (bhicDBBase == null)
                {
                    bhicDBBase = GetDbConnector();
                }

                //execute stored procedure
                var rowEffeted = bhicDBBase.ExecuteNonQuery("CreatePolicyPaymentDetail", QueryCommandType.StoredProcedure,
                                new List<System.Data.IDbDataParameter> 
                                    { 
                                        new SqlParameter() { ParameterName = "@PolicyID", Value = policyPaymentDetail.PolicyID, SqlDbType = SqlDbType.Int } ,
                                        new SqlParameter() { ParameterName = "@TransactionCode", Value = policyPaymentDetail.TransactionCode,SqlDbType=SqlDbType.VarChar,Size=200 } ,
                                        new SqlParameter() { ParameterName = "@AmountPaid", Value = policyPaymentDetail.AmountPaid, SqlDbType = SqlDbType.Decimal,Precision=18,Scale=2 } ,
                                        new SqlParameter() { ParameterName = "@IsActive", Value = policyPaymentDetail.IsActive, SqlDbType = SqlDbType.Bit } ,
                                        new SqlParameter() { ParameterName = "@CreatedDate", Value = policyPaymentDetail.CreatedDate, SqlDbType = SqlDbType.DateTime } ,
                                        new SqlParameter() { ParameterName = "@CreatedBy", Value = policyPaymentDetail.CreatedBy, SqlDbType = SqlDbType.Int } ,
                                        new SqlParameter() { ParameterName = "@ModifiedDate", Value = policyPaymentDetail.ModifiedDate, SqlDbType = SqlDbType.DateTime } ,
                                        new SqlParameter() { ParameterName = "@ModifiedBy", Value = policyPaymentDetail.ModifiedBy, SqlDbType = SqlDbType.Int } 
                                       
                                    });

                if (rowEffeted > 0)
                {
                    return true;
                }
            }
            catch
            {
                throw;
            }

            return false;
        }

    }
}
