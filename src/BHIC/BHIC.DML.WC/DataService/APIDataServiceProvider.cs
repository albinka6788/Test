using BHIC.Common;
using BHIC.Common.DataAccess;
using BHIC.DML.WC.DataContract;
using BHIC.DML.WC.DTO;
using BHIC.Domain.Dashboard;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security;

namespace BHIC.DML.WC.DataService
{
    public class APIDataServiceProvider : BaseDataProvider, IAPIDataServiceProvider
    {

        public bool CreateUserPolicy(DTO.UserPolicyDTO UserPolicy, out Int64 policyId)
        {
            try
            {
                policyId = 0;
                //Comment : Here add return parameter 
                var returnParameter = new SqlParameter() { ParameterName = "@ReturnInsertedRowId", SqlDbType = SqlDbType.BigInt, Direction = ParameterDirection.ReturnValue };



                if (UserPolicy.OrganizationName == null)
                    UserPolicy.OrganizationName = string.Empty;

                var Address1 = string.Empty;
                var City = string.Empty;
                var StateCode = string.Empty;
                var ZipCode = 0;
                var PhoneNumber = 0; // DBNull.Value; Guru : Change to 0 otherwise we have to change the value to 0 in Get query.
                var PolicyExpirationDate = System.DateTime.Now;
                var PaymentOptionID = 0;
                var AgencyCode = string.Empty;
                var OrganizationName = String.Empty;
                var TaxType = 0;
                var TaxId = string.Empty;
                var QuoteCreatedDate = System.DateTime.Now;
                //execute stored procedure
                var rowEffeted = GetDbConnector().ExecuteNonQuery("CreateUserPolicy", QueryCommandType.StoredProcedure,
                                new List<System.Data.IDbDataParameter> 
                                    { 
                                        new SqlParameter() { ParameterName = "@OrganizationName", Value = UserPolicy.OrganizationName, SqlDbType = SqlDbType.VarChar,Size=200 } ,
                                        new SqlParameter() { ParameterName = "@EmailID", Value = UserPolicy.EmailID, SqlDbType = SqlDbType.VarChar,Size=150  } ,
                                        new SqlParameter() { ParameterName = "@Password", Value = UserPolicy.Password, SqlDbType = SqlDbType.VarChar,Size=256 } ,
                                        new SqlParameter() { ParameterName = "@TaxType", Value = TaxType, SqlDbType = SqlDbType.Int} ,
                                        new SqlParameter() { ParameterName = "@TaxId", Value = TaxId, SqlDbType = SqlDbType.VarChar,Size=256 } ,
                                        new SqlParameter() { ParameterName = "@FirstName", Value = UserPolicy.FirstName, SqlDbType = SqlDbType.VarChar,Size=255 } ,
                                        new SqlParameter() { ParameterName = "@LastName", Value = UserPolicy.LastName, SqlDbType = SqlDbType.VarChar,Size=255 } ,
                                        new SqlParameter() { ParameterName = "@PolicyCode", Value = UserPolicy.PolicyNumber, SqlDbType = SqlDbType.VarChar,Size=50 } ,
                                        new SqlParameter() { ParameterName = "@PhoneNumber", Value = PhoneNumber, SqlDbType = SqlDbType.BigInt } ,
                                        new SqlParameter() { ParameterName = "@Address1",Value =  Address1,SqlDbType = SqlDbType.VarChar, Size = 200 },
	 	                                new SqlParameter() { ParameterName = "@City",Value =  City,SqlDbType = SqlDbType.VarChar, Size = 200 },
		                                new SqlParameter() { ParameterName = "@StateCode",Value =  StateCode,SqlDbType = SqlDbType.VarChar, Size = 200 },
		                                new SqlParameter() { ParameterName = "@ZipCode",Value =  ZipCode,SqlDbType = SqlDbType.Int },
		                                new SqlParameter() { ParameterName = "@QuoteNumber", Value = UserPolicy.QuoteNumber, SqlDbType = SqlDbType.VarChar,Size=20 } ,
                                        new SqlParameter() { ParameterName = "@RequestDate", Value = QuoteCreatedDate, SqlDbType = SqlDbType.DateTime  } ,
                                        new SqlParameter() { ParameterName = "@PremiumAmount", Value = UserPolicy.TotalPremiumAmount, SqlDbType = SqlDbType.Decimal,Precision=18,Scale=2 } ,
                                        new SqlParameter() { ParameterName = "@PaymentOptionId", Value = PaymentOptionID, SqlDbType = SqlDbType.Int } ,
                                        new SqlParameter() { ParameterName = "@AgencyCode", Value = AgencyCode, SqlDbType = SqlDbType.VarChar, Size = 20 } ,
                                        new SqlParameter() { ParameterName = "@PolicyNumber", Value = UserPolicy.PolicyNumber, SqlDbType = SqlDbType.VarChar,Size=20 } ,
                                        new SqlParameter() { ParameterName = "@EffectiveDate", Value =UserPolicy.InceptionDate, SqlDbType = SqlDbType.DateTime } ,
                                        new SqlParameter() { ParameterName = "@PolicyExpiryDate", Value =PolicyExpirationDate, SqlDbType = SqlDbType.DateTime } ,
                                        returnParameter
                                       
                                    });

                if (rowEffeted > 0)
                {
                    if (returnParameter.Value != null)
                    {
                        policyId = Convert.ToInt64(returnParameter.Value);
                    }

                    return true;
                }
                return false;

            }
            catch
            {
                //Comment : Here in case of DB updation failure log it into XML for future re-trying by schedular service
                throw;
            }
        }

        public bool IsValidCredientials(string UserName, string Password)
        {
            BHICDBBase dbConnector = new BHICDBBase();
            var dsGetCredentials = dbConnector.LoadDataSet("GetAPICredentials", QueryCommandType.StoredProcedure,
               new List<System.Data.IDbDataParameter> { new SqlParameter() { ParameterName = "@Email", Value = UserName, SqlDbType = SqlDbType.VarChar } });
            if (dsGetCredentials.Tables[0].Rows.Count > 0)
            {
                var decryptedpassword = dsGetCredentials.Tables[0].Rows[0]["password"].ToString();
                if (decryptedpassword.ToUpper().Trim().Equals(Password.ToUpper().Trim()))
                {
                    return true;
                }

            }
            return false;
        }

        public int GetUserDetailId(string UserName)
        {
            BHICDBBase dbConnector = new BHICDBBase();
            var dsGetCredentials = dbConnector.LoadDataSet("GetCredentials", QueryCommandType.StoredProcedure,
               new List<System.Data.IDbDataParameter> { new SqlParameter() { ParameterName = "@Email", Value = UserName, SqlDbType = SqlDbType.VarChar } });
            if (dsGetCredentials.Tables[0].Rows.Count > 0)
            {
                return Convert.ToInt32(dsGetCredentials.Tables[0].Rows[0]["Id"]);

            }
            return 0;
        }

        public Boolean IsValidUserCredentials(string UserName, SecureString Password)
        {
            BHICDBBase dbConnector = new BHICDBBase();
            var dsGetCredentials = dbConnector.LoadDataSet("GetCredentials", QueryCommandType.StoredProcedure,
               new List<System.Data.IDbDataParameter> { new SqlParameter() { ParameterName = "@Email", Value = UserName, SqlDbType = SqlDbType.VarChar } });
            if (dsGetCredentials.Tables[0].Rows.Count > 0)
            {
                bool isValidPassword = false;
                SecureString decryptedpassword = new SecureString();
                decryptedpassword = UtilityFunctions.ConvertToSecureString(Encryption.DecryptText(dsGetCredentials.Tables[0].Rows[0]["password"].ToString(), UserName.ToUpper()));

                if (UtilityFunctions.SecureStringCompare(decryptedpassword, Password))
                {
                    isValidPassword = true;
                }

                return isValidPassword;

            }
            return false;
        }

        /// <summary>
        /// To check if the policy is exists. It can be checked either with policy number or quote number
        /// </summary>
        /// <param name="Option">1->Policy Number;2->Quote Number</param>
        /// <param name="Value"> Policy Number / Quote Number</param>
        /// <returns></returns>
        public bool IsPolicyNumberExists(int Option, string Value)
        {
            BHICDBBase dbConnector = new BHICDBBase();
            var dsGetCredentials = dbConnector.LoadDataSet("IsPolicyExists", QueryCommandType.StoredProcedure,
               new List<System.Data.IDbDataParameter> { 
                   new SqlParameter() { ParameterName = "@Option", Value = Option, SqlDbType = SqlDbType.Int } ,
                   new SqlParameter() { ParameterName = "@Value", Value = Value, SqlDbType = SqlDbType.VarChar } 

               });
            if (dsGetCredentials.Tables[0].Rows.Count > 0)
            {
                return true;

            }
            return false;
        }

        /// <summary>
        /// To get the quote details
        /// </summary>
        /// <param name="quoteNumber"></param>
        /// <returns></returns>
        public QuoteDTO GetQuoteInfo(string quoteNumber)
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
                if (rdr["OrganizationUserDetailID"] != DBNull.Value)
                    quote.OrganizationUserDetailID = Convert.ToInt32(rdr["OrganizationUserDetailID"]);
            }

            return quote;
        }

        /// <summary>
        /// To save quote information
        /// </summary>
        /// <param name="quote"></param>
        /// <returns></returns>

        public bool SaveQuote(QuoteDTO quote)
        {
            try
            {
                //execute stored procedure
                var rowEffeted = GetDbConnector().ExecuteNonQuery("CreateQuote", QueryCommandType.StoredProcedure,
                                new List<System.Data.IDbDataParameter> 
                                    { 
                                        new SqlParameter() { ParameterName = "@QuoteNumber", Value = quote.QuoteNumber, SqlDbType = SqlDbType.VarChar,Size=20 } ,
                                        new SqlParameter() { ParameterName = "@OrganizationUserDetailID", Value = quote.OrganizationUserDetailID, SqlDbType = SqlDbType.Int } ,
                                        new SqlParameter() { ParameterName = "@OrganizationAddressID", Value = quote.OrganizationAddressID, SqlDbType = SqlDbType.Int } ,
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
                                        new SqlParameter() { ParameterName = "@ModifiedBy", Value = quote.ModifiedBy, SqlDbType = SqlDbType.Int } ,
                                        new SqlParameter() { ParameterName = "@RetrieveQuoteURL", Value = quote.RetrieveQuoteURL, SqlDbType = SqlDbType.VarChar } 
                                       
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

        public int GetOrganizationAddressId(int organizationId)
        {
            BHICDBBase dbConnector = new BHICDBBase();
            var dsGetCredentials = dbConnector.LoadDataSet("GetOrganisationAddressId", QueryCommandType.StoredProcedure,
               new List<System.Data.IDbDataParameter> { new SqlParameter() { ParameterName = "@OrganizationId", Value = organizationId, SqlDbType = SqlDbType.Int } });
            if (dsGetCredentials.Tables[0].Rows.Count > 0)
            {
                return Convert.ToInt32(dsGetCredentials.Tables[0].Rows[0]["Id"]);

            }
            return 0;
        }

        public bool IsValidAPICredentials(string UserName, SecureString Password)
        {
            BHICDBBase dbConnector = new BHICDBBase();
            var dsGetCredentials = dbConnector.LoadDataSet("GetAPICredentials", QueryCommandType.StoredProcedure,
               new List<System.Data.IDbDataParameter> { new SqlParameter() { ParameterName = "@Email", Value = UserName, SqlDbType = SqlDbType.VarChar } });
            if (dsGetCredentials.Tables[0].Rows.Count > 0)
            {
                bool isValidPassword = false;
                SecureString decryptedpassword = new SecureString();
                decryptedpassword = UtilityFunctions.ConvertToSecureString(Encryption.DecryptText(dsGetCredentials.Tables[0].Rows[0]["password"].ToString(), UserName));

                if (UtilityFunctions.SecureStringCompare(decryptedpassword, Password))
                {
                    isValidPassword = true;
                }

                return isValidPassword;

            }
            return false;
        }

        /// <summary>
        /// Delete a user account
        /// </summary>
        /// <param name="EmailID"></param>
        /// <returns></returns>
        public int DeleteUser(string EmailID)
        {
            BHICDBBase dbConnector = new BHICDBBase();
            var RowsAffected = dbConnector.ExecuteNonQuery("DeleteAccount", QueryCommandType.StoredProcedure,
               new List<System.Data.IDbDataParameter> { new SqlParameter() { ParameterName = "@EmailID", Value = EmailID, SqlDbType = SqlDbType.VarChar } });

            return RowsAffected;
        }

        /// <summary>
        /// Merge two Accounts
        /// </summary>
        /// <param name="OldEmailID"></param>
        /// <param name="NewEmailID"></param>
        /// <returns></returns>
        public int MergeAccounts(string OldEmailID, string NewEmailID)
        {
            BHICDBBase dbConnector = new BHICDBBase();
            int RowsAffected = dbConnector.ExecuteNonQuery("MergeAccounts", QueryCommandType.StoredProcedure,
               new List<System.Data.IDbDataParameter> { new SqlParameter() { ParameterName = "@OlDEmailID", Value = OldEmailID, SqlDbType = SqlDbType.VarChar },
                new SqlParameter() { ParameterName = "@NewemailID", Value = NewEmailID, SqlDbType = SqlDbType.VarChar }
               });

            return RowsAffected;
        }

        public SecondaryAccountRegistration CreateUserAccount(SecondaryAccountRegistration user)
        {
            var retResult = GetDbConnector().LoadDataSet("InsertOrUpdateOrganisationUserDetail", QueryCommandType.StoredProcedure,
                    new List<System.Data.IDbDataParameter> {
                       new SqlParameter() { ParameterName = "@id",              Value = user.Id , SqlDbType = SqlDbType.Int },
                       new SqlParameter() { ParameterName = "@fname",           Value = user.FirstName, SqlDbType = SqlDbType.VarChar },
                       new SqlParameter() { ParameterName = "@lname",           Value = user.LastName, SqlDbType = SqlDbType.VarChar },
                       new SqlParameter() { ParameterName = "@organizationName",Value = user.OrganizationName, SqlDbType = SqlDbType.VarChar },
                       new SqlParameter() { ParameterName = "@email",           Value = user.Email, SqlDbType = SqlDbType.VarChar },
                       new SqlParameter() { ParameterName = "@password",        Value = user.Password, SqlDbType = SqlDbType.VarChar },
                       new SqlParameter() { ParameterName = "@isActive",        Value = user.isActive, SqlDbType = SqlDbType.Bit},
                       new SqlParameter() { ParameterName = "@createdDate",     Value =  DateTime.Now, SqlDbType = SqlDbType.DateTime },
                       new SqlParameter() { ParameterName = "@createdBy",       Value = user.CreatedBy, SqlDbType = SqlDbType.Int },
                       new SqlParameter() { ParameterName = "@modifiedDate",    Value = DateTime.Now , SqlDbType = SqlDbType.DateTime },
                       new SqlParameter() { ParameterName = "@modifiedBy",      Value = user.ModifiedBy, SqlDbType = SqlDbType.Int},
                       new SqlParameter() { ParameterName = "@policyCode",      Value = user.PolicyCode, SqlDbType = SqlDbType.VarChar },
                       new SqlParameter() { ParameterName = "@phoneNumber",     Value = user.PhoneNumber, SqlDbType = SqlDbType.BigInt },
                       new SqlParameter() { ParameterName = "@userRoleId",      Value = user.UserRoleId, SqlDbType = SqlDbType.TinyInt },
                       new SqlParameter() { ParameterName = "@isEmailVerified", Value = user.isEmailVerified, SqlDbType = SqlDbType.Bit }
                   });

            user.Id = Convert.ToInt32(retResult.Tables[0].Rows[0]["id"]);
            return user;
        }

        public void AttachPolicy(string EmailID, List<string> PolicyCodes, DateTime StartTime, DateTime EndTime)
        {
            BHICDBBase dbConnector = new BHICDBBase();
            try
            {
                dbConnector.OpenConnection();
                dbConnector.BeginTransaction();
                foreach (string PolicyCode in PolicyCodes)
                {
                    var retResult = dbConnector.ExecuteScalar("AttachPolicyToUser", QueryCommandType.StoredProcedure,
                           new List<System.Data.IDbDataParameter> {
                       new SqlParameter() { ParameterName = "@Email", Value = EmailID, SqlDbType = SqlDbType.VarChar },
                       new SqlParameter() { ParameterName = "@PolicyCode", Value = PolicyCode, SqlDbType = SqlDbType.VarChar },
                       new SqlParameter() { ParameterName = "@StartDate", Value = StartTime, SqlDbType = SqlDbType.DateTime },
                       new SqlParameter() { ParameterName = "@EndDate", Value = EndTime, SqlDbType = SqlDbType.DateTime }
                   });
                }
                dbConnector.CommitTransaction();
            }
            catch (Exception)
            {
                dbConnector.RollbackTransaction();
                throw;
            }
            finally
            {
                dbConnector.CloseConnection();
            }
        }

        public void DetachPolicy(string EmailID, List<string> PolicyCodes)
        {
            BHICDBBase dbConnector = new BHICDBBase();
            try
            {
                dbConnector.OpenConnection();
                dbConnector.BeginTransaction();
                foreach (string PolicyCode in PolicyCodes)
                {
                    var retResult = dbConnector.ExecuteScalar("DetachPolicyFromUser", QueryCommandType.StoredProcedure,
                      new List<System.Data.IDbDataParameter> {
                       new SqlParameter() { ParameterName = "@Email", Value = EmailID, SqlDbType = SqlDbType.VarChar },
                       new SqlParameter() { ParameterName = "@PolicyCode", Value = PolicyCode, SqlDbType = SqlDbType.VarChar }                     
                   });
                }
                dbConnector.CommitTransaction();
            }
            catch (Exception)
            {
                dbConnector.RollbackTransaction();
                throw;
            }
            finally
            {
                dbConnector.CloseConnection();
            }
        }
    }
}
