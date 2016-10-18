#region Using directives

using BHIC.Common.DataAccess;
using BHIC.DML.WC.DataContract;
using BHIC.DML.WC.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

using BHIC.Common.Logging;

#endregion

namespace BHIC.DML.WC.DataService
{
    public class OrganisationUserDetailDataProvider : BaseDataProvider, IOrganisationUserDetailDataProvider
    {
        /// <summary>
        /// Add user new account details
        /// </summary>
        /// <param name="organisation"></param>
        /// <param name="organisationUserId"></param>
        /// <returns></returns>
        bool IOrganisationUserDetailDataProvider.AddOrganisationUserDetail(OrganisationUserDetailDTO organisation, out Int64? organisationUserId)
        {
            try
            {
                //Comment : Here default set value 
                organisationUserId = null;

                //Comment : Here add return parameter 
                var returnParameter = new SqlParameter() { ParameterName = "@ReturnInsertedRowId", SqlDbType = SqlDbType.BigInt, Direction = ParameterDirection.ReturnValue };

                //execute stored procedure
                var rowEffeted = GetDbConnector().ExecuteNonQuery("CreateOraganisationUserDetail", QueryCommandType.StoredProcedure,
                                new List<System.Data.IDbDataParameter> 
                                    { 
                                        new SqlParameter() { ParameterName = "@OrganizationName", Value = organisation.OrganizationName, SqlDbType = SqlDbType.VarChar,Size=200 } ,
                                        new SqlParameter() { ParameterName = "@EmailID", Value = organisation.EmailID, SqlDbType = SqlDbType.VarChar,Size=150  } ,
                                        new SqlParameter() { ParameterName = "@Password", Value = organisation.Password, SqlDbType = SqlDbType.VarChar,Size=256 } ,
                                        new SqlParameter() { ParameterName = "@Tin", Value = organisation.Tin, SqlDbType = SqlDbType.VarChar,Size=256 } ,
                                        new SqlParameter() { ParameterName = "@Ssn", Value = organisation.Ssn, SqlDbType = SqlDbType.VarChar,Size=256 } ,
                                        new SqlParameter() { ParameterName = "@Fein", Value = organisation.Fein, SqlDbType = SqlDbType.VarChar,Size=256 } ,
                                        new SqlParameter() { ParameterName = "@IsActive", Value = organisation.IsActive, SqlDbType = SqlDbType.Bit } ,
                                        new SqlParameter() { ParameterName = "@CreatedDate", Value = organisation.CreatedDate,SqlDbType = SqlDbType.DateTime } ,
                                        new SqlParameter() { ParameterName = "@CreatedBy", Value = organisation.CreatedBy, SqlDbType = SqlDbType.Int } ,
                                        new SqlParameter() { ParameterName = "@ModifiedDate", Value = organisation.ModifiedDate, SqlDbType = SqlDbType.DateTime } ,
                                        new SqlParameter() { ParameterName = "@ModifiedBy", Value = organisation.ModifiedBy, SqlDbType = SqlDbType.Int }, 
                                        new SqlParameter() { ParameterName = "@FirstName", Value = organisation.FirstName, SqlDbType = SqlDbType.VarChar,Size=255 } ,
                                        new SqlParameter() { ParameterName = "@LastName", Value = organisation.LastName, SqlDbType = SqlDbType.VarChar,Size=255 } ,
                                        new SqlParameter() { ParameterName = "@PolicyCode", Value = organisation.PolicyCode, SqlDbType = SqlDbType.VarChar,Size=50 } ,
                                        new SqlParameter() { ParameterName = "@PhoneNumber", Value = organisation.PhoneNumber, SqlDbType = SqlDbType.BigInt } ,
                                        new SqlParameter() { ParameterName = "@PhoneNumber", Value = organisation.PhoneNumber, SqlDbType = SqlDbType.BigInt } ,
                                        new SqlParameter() { ParameterName = "@UserRoleId", Value = organisation.UserRoleId, SqlDbType = SqlDbType.BigInt } ,
                                        returnParameter
                                       
                                    });

                if (rowEffeted > 0)
                {
                    if (returnParameter.Value != null)
                    {
                        organisationUserId = Convert.ToInt64(returnParameter.Value);
                    }

                    return true;
                }
            }
            catch
            {
                //Comment : Here in case of DB updation failure log it into XML for future re-trying by schedular service
                throw;
            }

            return false;
        }

        /// <summary>
        /// Method to create as well as update existing details
        /// </summary>
        /// <param name="organisation"></param>
        /// <param name="organisationUserId"></param>
        /// <returns></returns>
        bool IOrganisationUserDetailDataProvider.MaintainUserAccountDetail(OrganisationUserDetailDTO organisation, out Int64? organisationUserId)
        {
            try
            {
                //Comment : Here default set value 
                organisationUserId = null;

                //Comment : Here add return parameter 
                var returnParameter = new SqlParameter() { ParameterName = "@ReturnRowId", SqlDbType = SqlDbType.BigInt, Direction = ParameterDirection.ReturnValue };

                //execute stored procedure
                var rowEffeted = GetDbConnector().ExecuteNonQuery("MaintainOraganisationUserDetail", QueryCommandType.StoredProcedure,
                                new List<System.Data.IDbDataParameter> 
                                    { 
                                        new SqlParameter() { ParameterName = "@OrganizationName", Value = organisation.OrganizationName, SqlDbType = SqlDbType.VarChar,Size=200 } ,
                                        new SqlParameter() { ParameterName = "@EmailID", Value = organisation.EmailID, SqlDbType = SqlDbType.VarChar,Size=150  } ,
                                        new SqlParameter() { ParameterName = "@Password", Value = organisation.Password, SqlDbType = SqlDbType.VarChar,Size=256 } ,
                                        new SqlParameter() { ParameterName = "@Tin", Value = organisation.Tin, SqlDbType = SqlDbType.VarChar,Size=256 } ,
                                        new SqlParameter() { ParameterName = "@Ssn", Value = organisation.Ssn, SqlDbType = SqlDbType.VarChar,Size=256 } ,
                                        new SqlParameter() { ParameterName = "@Fein", Value = organisation.Fein, SqlDbType = SqlDbType.VarChar,Size=256 } ,
                                        new SqlParameter() { ParameterName = "@IsActive", Value = organisation.IsActive, SqlDbType = SqlDbType.Bit } ,
                                        new SqlParameter() { ParameterName = "@CreatedDate", Value = organisation.CreatedDate,SqlDbType = SqlDbType.DateTime } ,
                                        new SqlParameter() { ParameterName = "@CreatedBy", Value = organisation.CreatedBy, SqlDbType = SqlDbType.Int } ,
                                        new SqlParameter() { ParameterName = "@ModifiedDate", Value = organisation.ModifiedDate, SqlDbType = SqlDbType.DateTime } ,
                                        new SqlParameter() { ParameterName = "@ModifiedBy", Value = organisation.ModifiedBy, SqlDbType = SqlDbType.Int }, 
                                        new SqlParameter() { ParameterName = "@FirstName", Value = organisation.FirstName, SqlDbType = SqlDbType.VarChar,Size=255 } ,
                                        new SqlParameter() { ParameterName = "@LastName", Value = organisation.LastName, SqlDbType = SqlDbType.VarChar,Size=255 } ,
                                        new SqlParameter() { ParameterName = "@PolicyCode", Value = organisation.PolicyCode, SqlDbType = SqlDbType.VarChar,Size=50 } ,
                                        new SqlParameter() { ParameterName = "@PhoneNumber", Value = organisation.PhoneNumber, SqlDbType = SqlDbType.BigInt } ,
                                        returnParameter
                                       
                                    });

                if (rowEffeted > 0)
                {
                    if (returnParameter.Value != null)
                    {
                        organisationUserId = Convert.ToInt64(returnParameter.Value);
                    }

                    return true;
                }
            }
            catch
            {
                //Comment : Here in case of DB updation failure log it into XML for future re-trying by schedular service
                throw;
            }

            return false;
        }

        /// <summary>
        /// Get user credential details based on email-id
        /// </summary>
        /// <param name="organisationUser"></param>
        /// <returns></returns>
        OrganisationUserDetailDTO IOrganisationUserDetailDataProvider.GetUserCredentialDetails(OrganisationUserDetailDTO organisationUser)
        {
            //Comment : Here return object
            OrganisationUserDetailDTO returnOrganisationUserCredentials = new OrganisationUserDetailDTO();

            try
            {
                //execute stored procedure
                var returnedData = GetDbConnector().LoadDataSet("GetUserCredentialDetails", QueryCommandType.StoredProcedure,
                                new List<System.Data.IDbDataParameter> 
                                    { 
                                        new SqlParameter() { ParameterName = "@EmailID", Value = organisationUser.EmailID, SqlDbType = SqlDbType.VarChar,Size=150  }
                                       
                                    });

                //Comment : Here get data
                if (returnedData != null && returnedData.Tables.Count > 0)
                {
                    var dataTable = returnedData.Tables[0];
                    if (dataTable.Rows.Count > 0)
                    {
                        returnOrganisationUserCredentials.EmailID = dataTable.Rows[0]["EmailID"].ToString();
                        returnOrganisationUserCredentials.Password = dataTable.Rows[0]["Password"].ToString();
                        returnOrganisationUserCredentials.IsActive = Convert.ToBoolean(dataTable.Rows[0]["IsActive"]);
                    }
                }
            }
            catch
            {
                //Comment : Here in case of DB updation failure log it into XML for future re-trying by schedular service
                throw;
            }

            return returnOrganisationUserCredentials;
        }

        /// <summary>
        /// Activate/Deactivate exsiting organization user
        /// </summary>
        /// <param name="organisation"></param>
        /// <returns></returns>
        bool IOrganisationUserDetailDataProvider.ActivateDeactivateOrganisationUser(OrganisationUserDetailDTO organisation, IBHICDBBase bhicDBBase = null)
        {
            try
            {
                if (bhicDBBase == null)
                {
                    bhicDBBase = GetDbConnector();
                }

                //execute stored procedure
                var rowEffeted = bhicDBBase.ExecuteNonQuery("ActivateDeactivateOrganizationUser", QueryCommandType.StoredProcedure,
                                new List<System.Data.IDbDataParameter> 
                                    { 
                                        new SqlParameter() { ParameterName = "@IsActive", Value = organisation.IsActive, SqlDbType = SqlDbType.Bit } ,
                                        new SqlParameter() { ParameterName = "@QuoteNumber", Value = organisation.QuoteNumber, SqlDbType = SqlDbType.VarChar, Size = organisation.QuoteNumber.Length },                                        
                                        new SqlParameter() { ParameterName = "@ModifiedDate", Value = organisation.ModifiedDate, SqlDbType = SqlDbType.DateTime } ,
                                        new SqlParameter() { ParameterName = "@ModifiedBy", Value = organisation.ModifiedBy, SqlDbType = SqlDbType.Int }
                                       
                                    });

                if (rowEffeted > 0)
                {
                    return true;
                }
            }
            catch
            {
                //Comment : Here in case of DB updation failure log it into XML for future re-trying by schedular service
                throw;
            }

            return false;
        }
    }
}
