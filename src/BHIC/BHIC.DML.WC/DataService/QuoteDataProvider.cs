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
    public class QuoteDataProvider : BaseDataProvider, IQuoteDataProvider
    {
        bool IQuoteDataProvider.AddQuoteDetails(QuoteDTO quote)
        {
            try
            {
                //execute stored procedure
                var rowEffeted = GetDbConnector().ExecuteNonQuery("CreateQuote", QueryCommandType.StoredProcedure,
                                new List<System.Data.IDbDataParameter> 
                                    { 
                                        new SqlParameter() { ParameterName = "@QuoteNumber", Value = quote.QuoteNumber, SqlDbType = SqlDbType.VarChar,Size=20 } ,
                                        new SqlParameter() { ParameterName = "@LineOfBusinessId", Value = quote.LineOfBusinessId, SqlDbType = SqlDbType.Int } ,
                                        new SqlParameter() { ParameterName = "@ExternalSystemID", Value = quote.ExternalSystemID, SqlDbType = SqlDbType.Int } ,
                                        new SqlParameter() { ParameterName = "@RequestDate", Value = quote.RequestDate, SqlDbType = SqlDbType.DateTime  } ,
                                        new SqlParameter() { ParameterName = "@ExpiryDate", Value = quote.ExpiryDate, SqlDbType = SqlDbType.DateTime  } ,
                                        new SqlParameter() { ParameterName = "@PremiumAmount", Value = quote.PremiumAmount, SqlDbType = SqlDbType.Decimal,Precision=18,Scale=2 } ,
                                        new SqlParameter() { ParameterName = "@PaymentOptionId", Value = quote.PaymentOptionID, SqlDbType = SqlDbType.Int } ,
                                        new SqlParameter() { ParameterName = "@AgencyCode", Value = quote.AgencyCode, SqlDbType = SqlDbType.VarChar, Size = 20 } ,
                                        new SqlParameter() { ParameterName = "@IsActive", Value = quote.IsActive, SqlDbType = SqlDbType.Bit } ,
                                        new SqlParameter() { ParameterName = "@CreatedDate", Value = quote.CreatedDate, SqlDbType = SqlDbType.DateTime } ,
                                        new SqlParameter() { ParameterName = "@CreatedBy", Value = quote.CreatedBy, SqlDbType = SqlDbType.Int } ,
                                        new SqlParameter() { ParameterName = "@ModifiedDate", Value = quote.ModifiedDate, SqlDbType = SqlDbType.DateTime } ,
                                        new SqlParameter() { ParameterName = "@ModifiedBy", Value = quote.ModifiedBy, SqlDbType = SqlDbType.Int } 
                                       
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
            GetDbConnector().CloseConnection();

            return false;
        }

        QuoteDTO IQuoteDataProvider.GetQuoteDetails(string quoteNumber)
        {
            QuoteDTO quote = new QuoteDTO();

            //execute stored procedure
            var rdr = GetDbConnector().ExecuteReader("GetQuoteDetails", QueryCommandType.StoredProcedure,
                            new List<System.Data.IDbDataParameter> 
                                    { 
                                        new SqlParameter() { ParameterName = "@QuoteNumber", Value = quoteNumber, SqlDbType = SqlDbType.VarChar, Size=20} ,
                                    });

            while (rdr.Read())
            {
                quote.QuoteNumber = Convert.ToString(rdr["QuoteNumber"]);
                quote.LineOfBusinessId = Convert.ToInt32(rdr["LineOfBusinessId"]);
                quote.ExternalSystemID = Convert.ToInt32(rdr["ExternalSystemID"]);
                quote.RequestDate = Convert.ToDateTime(rdr["RequestDate"]);
                if (rdr["ExpiryDate"] != DBNull.Value)
                    quote.ExpiryDate = Convert.ToDateTime(rdr["ExpiryDate"]);
                quote.PremiumAmount = Convert.ToInt32(rdr["PremiumAmount"]);
                quote.PaymentOptionID = Convert.ToInt32(rdr["PaymentOptionID"]);
                quote.AgencyCode = Convert.ToString(rdr["AgencyCode"]);
            }

            return quote;
        }

        bool IQuoteDataProvider.UpdateQuoteUserId(OrganisationUserDetailDTO user, QuoteDTO quote)
        {
            try
            {
                //execute stored procedure
                var rowEffeted = GetDbConnector().ExecuteNonQuery("UpdateQuoteOrganizationUserId", QueryCommandType.StoredProcedure,
                                new List<System.Data.IDbDataParameter> 
                                    { 
                                        new SqlParameter() { ParameterName = "@QuoteNumber", Value = quote.QuoteNumber, SqlDbType = SqlDbType.VarChar,Size=20 } ,
                                        new SqlParameter() { ParameterName = "@OrgnizationUserId", Value = quote.OrganizationUserDetailID, SqlDbType = SqlDbType.Int } ,
                                        new SqlParameter() { ParameterName = "@EmailId", Value = user.EmailID, SqlDbType = SqlDbType.VarChar,Size=150 } ,
                                        new SqlParameter() { ParameterName = "@ModifiedDate", Value = quote.ModifiedDate, SqlDbType = SqlDbType.DateTime } ,
                                        new SqlParameter() { ParameterName = "@ModifiedBy", Value = quote.ModifiedBy, SqlDbType = SqlDbType.Int }
                                       
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

        bool IQuoteDataProvider.UpdateQuoteOrganizationAddressId(OrganisationUserDetailDTO user, OrganisationAddress orgAddress, QuoteDTO quote)
        {
            try
            {
                //execute stored procedure
                var rowEffeted = GetDbConnector().ExecuteNonQuery("UpdateQuoteOrganizationAddressID", QueryCommandType.StoredProcedure,
                                new List<System.Data.IDbDataParameter> 
                                    { 
                                        new SqlParameter() { ParameterName = "@QuoteNumber", Value = quote.QuoteNumber, SqlDbType = SqlDbType.VarChar,Size=20 } ,
                                        new SqlParameter() { ParameterName = "@OrganizationAddressID", Value = orgAddress.Id, SqlDbType = SqlDbType.Int } ,
                                        new SqlParameter() { ParameterName = "@EmailId", Value = user.EmailID, SqlDbType = SqlDbType.VarChar,Size=150 } ,
                                        new SqlParameter() { ParameterName = "@ModifiedDate", Value = quote.ModifiedDate, SqlDbType = SqlDbType.DateTime } ,
                                        new SqlParameter() { ParameterName = "@ModifiedBy", Value = quote.ModifiedBy, SqlDbType = SqlDbType.Int }                                       
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

        /// <summary>
        /// Get specified quote User ID
        /// </summary>
        /// <param name="quoteNumber"></param>
        /// <returns></returns>
        int IQuoteDataProvider.GetQuoteUserId(string quoteNumber)
        {
            var QuoteUserId = GetDbConnector().ExecuteScalar("GetQuoteUserId", QueryCommandType.StoredProcedure,
                                    new List<System.Data.IDbDataParameter> 
                                { 
                                    new SqlParameter() { ParameterName = "@QuoteNumber", Value = quoteNumber, SqlDbType = SqlDbType.VarChar, Size = 20 } 
                                });

            //Comment : Here convert retuned object
            return (QuoteUserId != DBNull.Value && QuoteUserId != null ? Convert.ToInt32(QuoteUserId) : 0);
        }

        /// <summary>
        /// Add ClassKeyword to CYBClassKeywords Table.
        /// </summary>
        /// <param name="classKeyword"></param>
        /// <returns></returns>
        bool IQuoteDataProvider.AddClassKeywords(string classKeyword)
        {
            try
            {
                var rowEffeted = GetDbConnector().ExecuteNonQuery("InsertCYBClassKeywords", QueryCommandType.StoredProcedure, new List<System.Data.IDbDataParameter> 
                                { 
                                    new SqlParameter() { ParameterName = "@ClassKeyword", Value = classKeyword, SqlDbType = SqlDbType.VarChar, Size = 200 }
                                });

                if (rowEffeted > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                LoggingService.Instance.Trace("Error Ocurred in Database Response Transaction Logging :" + ex.Message);
                throw;
            }
            return false;
        }
    }
}
