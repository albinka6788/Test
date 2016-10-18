#region Using directives

using BHIC.Common.DataAccess;
using BHIC.DML.WC.DataContract;
using BHIC.DML.WC.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

#endregion

namespace BHIC.DML.WC.DataService
{
    public class PolicyDataProvider : BaseDataProvider, IPolicyDataProvider
    {

        #region Methods

        #region Interface Implementation

        /// <summary>
        /// Store PolicyData into database
        /// </summary>
        /// <param name="policy">Policy DTO</param>
        /// <returns>Returns true on success, false otherwise</returns>
        int IPolicyDataProvider.AddPolicyData(PolicyDTO policy, PolicyPaymentDetailDTO policyPaymentDetails = null)
        {
            int policyId = 0;

            List<IDbDataParameter> dbParams = new List<IDbDataParameter>
                {
                                        new SqlParameter() { ParameterName = "@QuoteNumber", Value = policy.QuoteNumber, SqlDbType = SqlDbType.VarChar,Size=20 } ,
                                        new SqlParameter() { ParameterName = "@PolicyNumber", Value = policy.PolicyNumber, SqlDbType = SqlDbType.VarChar,Size=20 } ,
                                        new SqlParameter() { ParameterName = "@EffectiveDate", Value =policy.EffectiveDate, SqlDbType = SqlDbType.DateTime } ,
                                        new SqlParameter() { ParameterName = "@ExpiryDate", Value =policy.ExpiryDate, SqlDbType = SqlDbType.DateTime } ,
                                        new SqlParameter() { ParameterName = "@PremiumAmount", Value = policy.PremiumAmount, SqlDbType = SqlDbType.Decimal,Precision=18,Scale=2 } ,
                                        new SqlParameter() { ParameterName = "@PaymentOptionID", Value = policy.PaymentOptionID, SqlDbType = SqlDbType.Int} ,
                                        new SqlParameter() { ParameterName = "@IsActive", Value = policy.IsActive, SqlDbType = SqlDbType.Bit } ,
                                        new SqlParameter() { ParameterName = "@CreatedDate", Value = policy.CreatedDate, SqlDbType = SqlDbType.DateTime } ,
                                        new SqlParameter() { ParameterName = "@CreatedBy", Value = policy.CreatedBy, SqlDbType = SqlDbType.Int } ,
                                        new SqlParameter() { ParameterName = "@ModifiedDate", Value = policy.ModifiedDate, SqlDbType = SqlDbType.DateTime } ,
                                        new SqlParameter() { ParameterName = "@ModifiedBy", Value = policy.ModifiedBy, SqlDbType = SqlDbType.Int }, 
                                        new SqlParameter() { ParameterName = "@policy_identity",Value=0, SqlDbType = SqlDbType.Int,Direction= ParameterDirection.InputOutput } 
                                       
                };
            IBHICDBBase bhicDBBase = GetDbConnector();

            try
            {
                bhicDBBase.OpenConnection();
                bhicDBBase.BeginTransaction();
                //execute stored procedure
                bhicDBBase.ExecuteNonQuery("CreatePolicy", QueryCommandType.StoredProcedure, dbParams);
                policyId = Convert.ToInt32(dbParams[(dbParams.Count - 1)].Value);

                if (policyPaymentDetails != null)
                {
                    policyPaymentDetails.PolicyID = policyId;
                    IPolicyPaymentDetailProvider policyPaymentDetailProvider = new PolicyPaymentDetailProvider();

                    policyPaymentDetailProvider.AddPolicyPaymentDetail(policyPaymentDetails, bhicDBBase);
                }

                #region Comment : Here Activate user account

                if (policy.ModifiedBy > 0)
                {
                    BHIC.DML.WC.DataContract.IOrganisationUserDetailDataProvider orgUserService = new BHIC.DML.WC.DataService.OrganisationUserDetailDataProvider();

                    orgUserService.ActivateDeactivateOrganisationUser(new OrganisationUserDetailDTO { IsActive = policy.IsActive, QuoteNumber = policy.QuoteNumber, ModifiedBy = policy.ModifiedBy, ModifiedDate = policy.ModifiedDate }, bhicDBBase);
                }

                #endregion

                bhicDBBase.CommitTransaction();
            }
            catch
            {
                throw;
            }
            finally
            {
                bhicDBBase.Dispose();
            }

            return policyId;
        }

        /// <summary>
        /// Get PolicyId by policy number
        /// </summary>
        /// <param name="policyNumber"></param>
        /// <returns>returns required policy id on success, 0 otherwise</returns>
        int IPolicyDataProvider.GetPolicyIdByPolicyNumber(string policyNumber)
        {
            List<int> ids = new List<int>();

            try
            {
                //execute stored procedure
                using (var rdr = GetDbConnector().ExecuteReader("GetPolicyIDByPolicyNumber", QueryCommandType.StoredProcedure,
                                 new List<System.Data.IDbDataParameter> 
                                    { 
                                        new SqlParameter() { ParameterName = "@policyNumber", Value = policyNumber, SqlDbType = SqlDbType.VarChar,Size=200 } 
                                    }))
                {
                    while (rdr.Read())
                    {
                        ids.Add(Convert.ToInt32(rdr["PolicyID"]));
                    }
                }

                return ids.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// It will check if give transaction code is exists on record or not
        /// </summary>
        /// <param name="transactionCode"></param>
        /// <returns>return true if exists, false otherwise</returns>
        bool IPolicyDataProvider.GetPolicyPaymentIdByTransactionCode(string transactionCode)
        {
            int id = 0;

            try
            {
                //execute stored procedure
                using (var rdr = GetDbConnector().ExecuteReader("GetPolicyPaymentIdByTransactionCode", QueryCommandType.StoredProcedure,
                                 new List<System.Data.IDbDataParameter> 
                                    { 
                                        new SqlParameter() { ParameterName = "@transactionCode", Value = transactionCode, SqlDbType = SqlDbType.VarChar,Size=200 } 
                                    }))
                {
                    while (rdr.Read())
                    {
                        id = Convert.ToInt32(rdr["Id"]);
                    }
                }

                return (id > 0) ? true : false;
            }
            catch
            {
                throw;
            }
        }



        #endregion

        #endregion
    }
}
