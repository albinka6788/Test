using BHIC.Common;
using BHIC.Common.Client;
using BHIC.Common.Config;
using BHIC.Common.DataAccess;
using BHIC.Contract.Background;
using BHIC.Contract.Policy;
using BHIC.Contract.Provider;
using BHIC.Core.Policy;
using BHIC.Domain.Dashboard;
using BHIC.Domain.Policy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BHIC.Contract.PolicyCentre;
using BHIC.Core.PolicyCentre;
using BHIC.Domain.Account;
using BHIC.Common.Logging;
using BHIC.Common.XmlHelper;
using System.Security;
using System.Runtime.InteropServices;
using BHIC.Contract.PurchasePath;
using BHIC.Core.PurchasePath;

namespace BHIC.Core.Background
{
    public class AccountRegistrationService : IServiceProviders, IAccountRegistrationService
    {
        private static ILoggingService logger = LoggingService.Instance;

        #region Public Methods

        public AccountRegistrationService(ServiceProvider provider)
        {
            base.ServiceProvider = provider;
        }

        /// <summary>
        /// Returns true for success otherwise false for fail.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public UserRegistration GetCredentials(UserRegistration user)
        {
            try
            {
                using (IBHICDBBase dbConnector = new BHICDBBase())
                {
                    user.ResponseMessage = string.Format("Please provide valid login credentials.");

                    var dsGetCredentials = dbConnector.LoadDataSet("GetCredentials", QueryCommandType.StoredProcedure,
                       new List<System.Data.IDbDataParameter> { new SqlParameter() { ParameterName = "@Email", Value = user.Email, SqlDbType = SqlDbType.VarChar } });
                    if (dsGetCredentials.Tables[0].Rows.Count > 0)
                    {
                        user.PhoneNumber = null;
                        user.Email = dsGetCredentials.Tables[0].Rows[0]["EmailID"].ToString();
                        user.Id = Convert.ToInt32(dsGetCredentials.Tables[0].Rows[0]["Id"]);
                        user.FirstName = dsGetCredentials.Tables[0].Rows[0]["FirstName"].ToString();
                        user.LastName = dsGetCredentials.Tables[0].Rows[0]["LastName"].ToString();
                        if (dsGetCredentials.Tables[0].Rows[0]["PhoneNumber"] != DBNull.Value)
                        {
                            user.PhoneNumber = Convert.ToInt64(dsGetCredentials.Tables[0].Rows[0]["PhoneNumber"]);
                        }
                        user.isEmailVerified = Convert.ToBoolean(dsGetCredentials.Tables[0].Rows[0]["IsEmailVerified"]);
                        user.OrganizationName = dsGetCredentials.Tables[0].Rows[0]["OrganizationName"].ToString();
                        user.Password = dsGetCredentials.Tables[0].Rows[0]["password"].ToString();

                        bool isValidPassword = false;
                        SecureString decryptedpassword = new SecureString();
                        decryptedpassword = Encryption.DecryptTextSecure(dsGetCredentials.Tables[0].Rows[0]["password"].ToString(), user.Email.ToUpper());

                        if (UtilityFunctions.SecureStringCompare(decryptedpassword, user.SecurePassword))
                        {
                            isValidPassword = true;
                        }

                        user.ResponseMessage = dbConnector.ExecuteScalar("LockUser", QueryCommandType.StoredProcedure, (
                                               new List<System.Data.IDbDataParameter> {                                                    
                                                    new SqlParameter() { ParameterName = "@Email",Value = user.Email, SqlDbType = SqlDbType.VarChar },
                                                    new SqlParameter() { ParameterName = "@IsValidPassword",Value = isValidPassword, SqlDbType = SqlDbType.Bit },
                                                    new SqlParameter() { ParameterName = "@MaxLoginAttempt",Value = ConfigCommonKeyReader.LoginAttempt, SqlDbType = SqlDbType.Int },
                                                    new SqlParameter() { ParameterName = "@MaxTimeToUnlock",Value = ConfigCommonKeyReader.UnlockAccountTime, SqlDbType = SqlDbType.Int }
                                                    })).ToString();
                        if (user.ResponseMessage.ToUpper() != "OK")
                        {
                            user.Id = 0;
                        }
                    }
                    else
                    {
                        user.ResponseMessage = string.Format("Please provide valid login credentials.");
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return user;

        }



        /// <summary>
        /// Returns OK if Registration Successfull otherwise return the error
        /// </summary>
        /// <returns></returns>
        public string CreateAccount(AccountRegistration user)
        {
            string rMessage = string.Empty;
            BHICDBBase dbConnector = new BHICDBBase();
            try
            {
                dbConnector.OpenConnection();
                dbConnector.BeginTransaction();
                rMessage = CreateUserQuoteAndPolicy(user, dbConnector);
                if (rMessage == "OK")
                    dbConnector.CommitTransaction();
                else
                    dbConnector.RollbackTransaction();
            }
            catch (Exception ex)
            {
                rMessage = Constants.RegistrationFailed;
                dbConnector.RollbackTransaction();
                logger.Fatal(string.Format("Database Error with error message : {0}", ex.ToString()));
            }
            finally
            {
                dbConnector.CloseConnection();
            }
            return rMessage;
        }


        public bool ChangePassword(string email, SecureString oldpassword, SecureString newpassword)
        {
            bool retVal = false;
            try
            {
                using (IBHICDBBase dbConnector = new BHICDBBase())
                {
                    var dsGetCredentials = dbConnector.LoadDataSet("GetCredentials", QueryCommandType.StoredProcedure,
                      new List<System.Data.IDbDataParameter> { new SqlParameter() { ParameterName = "@Email", Value = email, SqlDbType = SqlDbType.VarChar } });
                    if (dsGetCredentials.Tables[0].Rows.Count > 0)
                    {
                        SecureString decryptedpassword = new SecureString();

                        decryptedpassword = Encryption.DecryptTextSecure(dsGetCredentials.Tables[0].Rows[0]["password"].ToString(), email.ToUpper());
                        if (UtilityFunctions.SecureStringCompare(decryptedpassword, oldpassword))
                        {
                            dbConnector.ExecuteNonQuery("ChangePassword", QueryCommandType.StoredProcedure,
                                new List<System.Data.IDbDataParameter> {                                
                                new SqlParameter() { ParameterName = "@email",Value = email, SqlDbType = SqlDbType.VarChar },
                                new SqlParameter() { ParameterName = "@newpassword", Value = UtilityFunctions.ConvertToString(newpassword), SqlDbType = SqlDbType.VarChar }
                            });
                            retVal = true;
                        }
                        else
                        {
                            retVal = false;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return retVal;
        }

        public bool ValidUser(string email)
        {
            bool retVal = false;
            try
            {
                using (IBHICDBBase dbConnector = new BHICDBBase())
                {
                    var dsGetCredentials = dbConnector.LoadDataSet("GetCredentials", QueryCommandType.StoredProcedure,
                      new List<System.Data.IDbDataParameter> { new SqlParameter() { ParameterName = "@Email", Value = email, SqlDbType = SqlDbType.VarChar } });
              
                    if (dsGetCredentials.Tables[0].Rows.Count > 0)
                    {                      
                        retVal = true;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return retVal;
        }

        public bool IsUserEmailExists(string email)
        {
            bool retVal = false;
            try
            {
                using (IBHICDBBase dbConnector = new BHICDBBase())
                {
                    var dsGetCredentials = dbConnector.LoadDataSet("IsUserEmailExists", QueryCommandType.StoredProcedure,
                      new List<System.Data.IDbDataParameter> { new SqlParameter() { ParameterName = "@Email", Value = email, SqlDbType = SqlDbType.VarChar } });
              
                    if (dsGetCredentials.Tables[0].Rows.Count > 0)
                    {                      
                        retVal = true;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return retVal;
        }

        public bool ResetPassword(string email, SecureString password)
        {
            bool retVal = false;
            try
            {
                using (IBHICDBBase dbConnector = new BHICDBBase())
                {
                    if (dbConnector.ExecuteNonQuery("ChangePassword", QueryCommandType.StoredProcedure,
                                new List<System.Data.IDbDataParameter> {                                
                                new SqlParameter() { ParameterName = "@email",Value = email, SqlDbType = SqlDbType.VarChar },
                                new SqlParameter() { ParameterName = "@newpassword", Value = Encryption.EncryptText(UtilityFunctions.ConvertToString(password), email.ToUpper()), SqlDbType = SqlDbType.VarChar }
                            }) > 0)
                    {
                        retVal = true;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return retVal;
        }

        //public bool UpadateContactInfo(ContactInformation contactInfo)
        //{
        //    bool retVal = false;
        //    try
        //    {
        //        using (IBHICDBBase dbConnector = new BHICDBBase())
        //        {
        //            var updatedInfo = dbConnector.ExecuteNonQuery("UpdateOraganisationUserDetail", QueryCommandType.StoredProcedure,
        //                new List<System.Data.IDbDataParameter> {
        //               new SqlParameter() { ParameterName = "@Firstname",Value = contactInfo.FirstName, SqlDbType = SqlDbType.VarChar },
        //               new SqlParameter() { ParameterName = "@Lastname",Value = contactInfo.LastName, SqlDbType = SqlDbType.VarChar },
        //               new SqlParameter() { ParameterName = "@Phone",Value = contactInfo.PhoneNumber , SqlDbType = SqlDbType.BigInt },
        //               new SqlParameter() { ParameterName = "@Email",Value = contactInfo.Email, SqlDbType = SqlDbType.VarChar }
        //           });
        //            if (updatedInfo > 0)
        //            {
        //                retVal = true;
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }

        //    return retVal;
        //}

        public bool EmailVerification(string email)
        {
            bool retVal = false;
            try
            {
                using (IBHICDBBase dbConnector = new BHICDBBase())
                {
                    var updatedInfo = dbConnector.ExecuteNonQuery("UpdateVerificationEmail", QueryCommandType.StoredProcedure,
                        new List<System.Data.IDbDataParameter> {
                       new SqlParameter() { ParameterName = "@Email",Value = email, SqlDbType = SqlDbType.VarChar },
                       new SqlParameter() { ParameterName = "@IsEmailverified",Value = 1, SqlDbType = SqlDbType.Bit }
                   });
                    if (updatedInfo > 0)
                    {
                        retVal = true;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return retVal;
        }

        public UserRegistration GetUserDetails(UserRegistration user)
        {
            try
            {
                using (IBHICDBBase dbConnector = new BHICDBBase())
                {
                    var dsGetCredentials = dbConnector.LoadDataSet("GetCredentials", QueryCommandType.StoredProcedure,
                       new List<System.Data.IDbDataParameter> { new SqlParameter() { ParameterName = "@Email", Value = user.Email, SqlDbType = SqlDbType.VarChar } });
                    if (dsGetCredentials.Tables[0].Rows.Count > 0)
                    {
                        user.Email = dsGetCredentials.Tables[0].Rows[0]["EmailID"].ToString();
                        user.Id = Convert.ToInt32(dsGetCredentials.Tables[0].Rows[0]["Id"]);
                        user.FirstName = dsGetCredentials.Tables[0].Rows[0]["FirstName"].ToString();
                        user.LastName = dsGetCredentials.Tables[0].Rows[0]["LastName"].ToString();
                        user.Password = dsGetCredentials.Tables[0].Rows[0]["password"].ToString();
                        user.PhoneNumber = Convert.ToInt64(dsGetCredentials.Tables[0].Rows[0]["PhoneNumber"]);
                        user.isEmailVerified = Convert.ToBoolean(dsGetCredentials.Tables[0].Rows[0]["IsEmailVerified"]);
                        user.OrganizationName = dsGetCredentials.Tables[0].Rows[0]["OrganizationName"].ToString();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return user;

        }


        public bool ForgotPwdRequestedDateTime(string type, string email, string inputMMddyyyyhhmmss)
        {
            Boolean retVal = false;
            try
            {
                using (IBHICDBBase dbConnector = new BHICDBBase())
                {

                    var changePassword = dbConnector.LoadDataSet("ForgotPwdRequstedDateTime", QueryCommandType.StoredProcedure,
                           new List<System.Data.IDbDataParameter> {                                
                               new SqlParameter() { ParameterName = "@Type",Value = type, SqlDbType = SqlDbType.VarChar },
                                new SqlParameter() { ParameterName = "@email",Value = email, SqlDbType = SqlDbType.VarChar },
                                new SqlParameter() { ParameterName = "@forgotPwdRequestedDateTime", Value = inputMMddyyyyhhmmss, SqlDbType = SqlDbType.VarChar }
                            });
                    retVal = (changePassword.Tables[0].Rows.Count > 0) ? true : false;

                }
            }
            catch (Exception)
            {
                throw;
            }
            return retVal;
        }

        /// <summary>
        /// Link user policy with specified user email id
        /// </summary>
        /// <param name="userEmailID"></param>
        /// <param name="policyNumber"></param>
        /// <returns></returns>
        public string LinkPolicyWithUser(string userEmailID, string policyNumber)
        {
            string rMessage = string.Empty;
            BHICDBBase dbConnector = new BHICDBBase();
            try
            {
                AccountRegistration user = new AccountRegistration();
                user.Email = userEmailID;
                user.PolicyCode = policyNumber;

                dbConnector.OpenConnection();
                dbConnector.BeginTransaction();
                rMessage = CreateUserQuoteAndPolicy(user, dbConnector, false);
                if (rMessage == "OK")
                {
                    dbConnector.CommitTransaction();
                }                
            }
            catch
            {
                throw;
            }
            finally
            {
                dbConnector.CloseConnection();
            }
            return rMessage;
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Account Registration process for Policy Centre. 
        /// Return OK if success otherwise custom error message.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>       
        private string CreateUserQuoteAndPolicy(AccountRegistration user, BHICDBBase dbConnector, bool createUserRequired = true)
        {
            IVUserPoliciesValidPolicyCodeService vUserPoliciesValidPolicyCodeService = new VUserPoliciesValidPolicyCodeService(ServiceProvider);
            var vUserPoliciesValidPolicyCode = vUserPoliciesValidPolicyCodeService.ValidateUserPolicy(new UserPolicyRequestParms { PolicyCode = user.PolicyCode.Trim() });

            if (!vUserPoliciesValidPolicyCode.OperationStatus.RequestSuccessful)
            {
                return "Policy Number does not exist. Please verify your Policy Number.";
            }

            //Checking for the Policy Exists or Not 
            bool policyExists = IsPolicyExists(user, dbConnector);
            if (policyExists)
            {
                return "Policy Number already exist. Please verify your Policy Number.";
            }

            if (createUserRequired)
            {
                // Encrypts the user input Password....
                user.Password = Encryption.EncryptText(user.Password, user.Email.ToUpper());

                // Inserts the account registration details of the user....
                user.isActive = true;
                user = CreateUser(user, dbConnector);

                if (user.Id <= 0)
                {
                    return "Email-Id already exist. Please verify your Email-Id.";
                }
            }
            else
            {
                user.Id = GetUserDetails(new UserRegistration { Email = user.Email }).Id;
            }

            // Calls the Quotes API Service for the policy code if user created successfully...
            IQuoteService quoteService = new QuoteService(ServiceProvider);
            var quote = quoteService.GetQuote(new QuoteRequestParms
            {
                PolicyId = user.PolicyCode.Trim(),
                IncludeRelatedPolicyData = true,
                IncludeRelatedRatingData = true,
                IncludeRelatedPaymentTerms = true,
                IncludeRelatedInsuredNames = false,
                IncludeRelatedExposuresGraph = true,
                IncludeRelatedOfficers = false,
                IncludeRelatedLocations = false,
                IncludeRelatedContactsGraph = false,
                IncludeRelatedQuestions = false,
                IncludeRelatedQuoteStatus = false
            });

            // If Quote API call is successfull, inserts the  API entry for Quote and Policy w.r.t the user
            if (quote.PolicyData.QuoteId > 0)
            {
                CreateQuote(user, dbConnector, quote);
                CreatePolicy(user, dbConnector, quote);

                // Add UserEmail and Policy Code to Guard Service
                if (!AddUserPolicyCodeService(user, ServiceProvider))
                {
                    return Constants.RegistrationFailed;
                }
            }
            else
            {
                return Constants.RegistrationFailed;
            }

            return "OK";
        }

        private AccountRegistration CreateUser(AccountRegistration user, BHICDBBase dbConnector)
        {
            var retResult = dbConnector.LoadDataSet("InsertOrUpdateOrganisationUserDetail", QueryCommandType.StoredProcedure,
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
                       new SqlParameter() { ParameterName = "@phoneNumber",     Value = user.PhoneNumber, SqlDbType = SqlDbType.BigInt }
                   });

            user.Id = Convert.ToInt32(retResult.Tables[0].Rows[0]["id"]);
            return user;
        }

        private void CreateQuote(AccountRegistration user, BHICDBBase dbConnector, Quote quote)
        {
            string policyType = user.PolicyCode.Trim().Substring(2, 2);
            int lineofBusiness = (policyType.Equals("WC", StringComparison.OrdinalIgnoreCase) ? 1 : policyType.Equals("BP", StringComparison.OrdinalIgnoreCase) ? 2 : 0);
            string quoteNumber = Convert.ToString(quote.PolicyData.QuoteId);
            decimal? premiumAmount = quote.RatingData.Premium;
            DateTime? inceptionDate = quote.PolicyData.InceptionDate;
            DateTime? expirationDate = quote.PolicyData.InceptionDate.HasValue ? quote.PolicyData.InceptionDate.Value.AddYears(1) : (DateTime?)null;
            int? paymentOptionID = quote.PaymentTerms.PaymentPlanId;

            var retResult = dbConnector.LoadDataSet("CreateQuote", QueryCommandType.StoredProcedure,
                new List<System.Data.IDbDataParameter> {
                       new SqlParameter() { ParameterName = "@QuoteNumber",     Value = quoteNumber , SqlDbType = SqlDbType.VarChar },
                       new SqlParameter() { ParameterName = "@OrganizationUserDetailID", Value = user.Id , SqlDbType = SqlDbType.Int },
                       new SqlParameter() { ParameterName = "@OrganizationAddressID",     Value = null , SqlDbType = SqlDbType.Int },
                       new SqlParameter() { ParameterName = "@LineOfBusinessId",Value = lineofBusiness, SqlDbType = SqlDbType.Int },
                       new SqlParameter() { ParameterName = "@ExternalSystemID",Value = null, SqlDbType = SqlDbType.Int },
                       new SqlParameter() { ParameterName = "@RequestDate",     Value = inceptionDate, SqlDbType = SqlDbType.DateTime },
                       new SqlParameter() { ParameterName = "@ExpiryDate",      Value = expirationDate, SqlDbType = SqlDbType.DateTime },
                       new SqlParameter() { ParameterName = "@PremiumAmount",   Value = premiumAmount, SqlDbType = SqlDbType.Decimal },                     
                       new SqlParameter() { ParameterName = "@PaymentoptionId",   Value = paymentOptionID, SqlDbType = SqlDbType.Int },                       
                       new SqlParameter() { ParameterName = "@AgencyCode",   Value = string.Empty, SqlDbType = SqlDbType.VarChar }, 
                       new SqlParameter() { ParameterName = "@IsActive",   Value = true, SqlDbType = SqlDbType.Bit }, 
                       new SqlParameter() { ParameterName = "@CreatedDate",   Value = DateTime.Now, SqlDbType = SqlDbType.DateTime }, 
                       new SqlParameter() { ParameterName = "@CreatedBy",   Value = user.Id, SqlDbType = SqlDbType.Int },
                       new SqlParameter() { ParameterName = "@ModifiedDate",   Value = DateTime.Now, SqlDbType = SqlDbType.DateTime },
                       new SqlParameter() { ParameterName = "@ModifiedBy",   Value = user.Id, SqlDbType = SqlDbType.Int },                    
                       
                   });
        }

        private void CreatePolicy(AccountRegistration user, BHICDBBase dbConnector, Quote quote)
        {
            string quoteNumber = Convert.ToString(quote.PolicyData.QuoteId);
            decimal? premiumAmount = quote.RatingData.Premium;
            DateTime? inceptionDate = quote.PolicyData.InceptionDate;
            DateTime? expirationDate = quote.PolicyData.InceptionDate.HasValue ? quote.PolicyData.InceptionDate.Value.AddYears(1) : (DateTime?)null;
            int? paymentOptionID = quote.PaymentTerms.PaymentPlanId;

            var retResult = dbConnector.LoadDataSet("CreatePolicy", QueryCommandType.StoredProcedure,
                   new List<System.Data.IDbDataParameter> {
                       new SqlParameter() { ParameterName = "@QuoteNumber",  Value = quoteNumber , SqlDbType = SqlDbType.VarChar },
                       new SqlParameter() { ParameterName = "@PolicyNumber", Value = user.PolicyCode.Trim(), SqlDbType = SqlDbType.VarChar },
                       new SqlParameter() { ParameterName = "@EffectiveDate",Value = inceptionDate, SqlDbType = SqlDbType.DateTime },
                       new SqlParameter() { ParameterName = "@ExpiryDate",     Value = expirationDate, SqlDbType = SqlDbType.DateTime },
                       new SqlParameter() { ParameterName = "@PremiumAmount",   Value =premiumAmount , SqlDbType = SqlDbType.Decimal },                     
                       new SqlParameter() { ParameterName = "@PaymentOptionID",   Value = paymentOptionID, SqlDbType = SqlDbType.Int },                    
                       new SqlParameter() { ParameterName = "@IsActive",   Value = true, SqlDbType = SqlDbType.Bit }, 
                       new SqlParameter() { ParameterName = "@CreatedDate",   Value = DateTime.Now, SqlDbType = SqlDbType.DateTime }, 
                       new SqlParameter() { ParameterName = "@CreatedBy",   Value = user.Id, SqlDbType = SqlDbType.Int },
                       new SqlParameter() { ParameterName = "@ModifiedDate",   Value = DateTime.Now, SqlDbType = SqlDbType.DateTime },
                       new SqlParameter() { ParameterName = "@ModifiedBy",   Value = user.Id, SqlDbType = SqlDbType.Int },
                       new SqlParameter() { ParameterName = "@policy_identity",Value=0, SqlDbType = SqlDbType.Int,Direction= ParameterDirection.InputOutput }  
                    });
        }

        private bool IsPolicyExists(AccountRegistration user, BHICDBBase dbConnector)
        {
            bool policyExists = false;
            int retResult = Convert.ToInt32(dbConnector.ExecuteScalar("IsPolicyExists", QueryCommandType.StoredProcedure,
                   new List<System.Data.IDbDataParameter> {
                       new SqlParameter() { ParameterName = "@Option",  Value = 1 , SqlDbType = SqlDbType.VarChar } ,    
                       new SqlParameter() { ParameterName = "@Value",  Value = user.PolicyCode , SqlDbType = SqlDbType.VarChar }     
                    }));

            if (retResult > 0)
            {
                policyExists = true;
            }
            return policyExists;
        }

        //Call UserPolicyCode POST service
        private bool AddUserPolicyCodeService(AccountRegistration user, ServiceProvider provider)
        {
            IUserPolicyCodesService userPolicyCodeService = new UserPolicyCodesService(provider);

            var userPolicyCodeResponse = userPolicyCodeService.AddUserPolicyCode(new UserPolicyCode { UserId = user.Email, PolicyCode = user.PolicyCode });

            //if request successful is false,log error details
            if (!userPolicyCodeResponse.RequestSuccessful)
            {
                logger.Fatal(string.Format("Error occurred while processsing UserPolicyCode API in Account Registration for Policy Center, please see log for more detail : {0}", userPolicyCodeResponse));
            }

            return userPolicyCodeResponse.RequestSuccessful;
        }

        #endregion
        
    }
}
