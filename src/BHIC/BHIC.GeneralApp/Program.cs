using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;

using BHIC.Common;
using BHIC.Common.Client;
using BHIC.Common.Config;
using BHIC.Common.DataAccess;
using BHIC.Contract.Background;
using BHIC.Contract.Policy;
using BHIC.Contract.PurchasePath;
using BHIC.Core;
using BHIC.Core.Policy;
using BHIC.Core.PurchasePath;
using BHIC.Domain.Background;
using BHIC.Domain.Dashboard;
using BHIC.Domain.Policy;
using BHIC.ViewDomain;
using BHIC.Core.Background;


namespace BHIC.GeneralApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args != null && args.Count() > 0)
                {
                    switch (args[0])
                    {
                        case "1": // Verify API Token Caching
                            break;
                        case "2": // Encrypt Existing FEIN
                            break;
                        case "3": // Rename Email ID
                            ChangeEmailID(args);
                            break;
                        case "4": // Link policy with user
                            LinkPolicyWithUser(args);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.Read();
        }

        private static void VerifyAPITokenCaching()
        {
            ServiceProvider provider = new GuardServiceProvider() { ServiceCategory = Constants.LOB.WC };

            ISubIndustryService subIndustryService = new SubIndustryService();
            DateTime startTime = DateTime.Now;
            Int64 i = 0;
            do
            {
                var subIndustryList = subIndustryService.GetSubIndustryList(
                        new SubIndustryRequestParms { Lob = Constants.GetLineOfBusiness(Constants.LineOfBusiness.WC) },
                        provider);

                while ((DateTime.Now - startTime).TotalMinutes <= 25)
                {
                    Console.Clear();
                    Console.WriteLine((DateTime.Now - startTime).TotalMinutes);
                }
                i++;
                Console.WriteLine(i);
            } while ((DateTime.Now - startTime).TotalMinutes <= 35);

        }

        private static void EncryptExistingFEIN()
        {
            BHICDBBase dbConnection = new BHICDBBase();

            dbConnection.DBName = "GuinnessDB";

            dbConnection.OpenConnection();

            Console.WriteLine("DB Connection Successful");

            DataSet ds = dbConnection.LoadDataSet("SELECT * FROM FeinPremiumFactor", QueryCommandType.Text);

            Console.WriteLine("Loaded all FEIN/SSN records");

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string feinSSNNumber = dr["Fein"].ToString();

                if (Regex.IsMatch(feinSSNNumber, @"^\d+$"))
                {
                    Console.WriteLine(feinSSNNumber);

                    string encryptedFEIN = BHIC.Common.Encryption.EncryptWithStaticKey(feinSSNNumber);

                    dbConnection.ExecuteNonQuery(string.Format("UPDATE FeinPremiumFactor SET Fein = '{0}' WHERE ID = {1}", encryptedFEIN, dr["Id"]));
                }
            }

            Console.WriteLine("All FEIN/SSN are encrypted");
        }

        private static void UpdateExistingPPCustomSessionWithCurrentStructure()
        {
            BHICDBBase dbConnection = new BHICDBBase();

            dbConnection.DBName = "GuinnessDB";

            dbConnection.OpenConnection();

            Console.WriteLine("DB Connection Successful");

            DataSet ds = dbConnection.LoadDataSet("SELECT QuoteID, CAST(SessionData AS VARCHAR(MAX)) SessionData FROM PurchaePathCustomSession", QueryCommandType.Text);

            Console.WriteLine("Loaded all Purchase Path CustomSession records");

            ICommonFunctionality commonFunctionality = new CommonFunctionality();
            CustomSession appSession = new CustomSession();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string customSessionString = dr["SessionData"].ToString();

                try
                {
                    appSession = commonFunctionality.GetDeserializedCustomSession(customSessionString);
                    string appSessionString = commonFunctionality.StringifyCustomSession(appSession);
                    //commonFunctionality.AddApplicationCustomSession(
                    //    new DML.WC.DTO.CustomSession() { QuoteID = Convert.ToInt32(dr["QuoteId"]), 
                    //        SessionData = commonFunctionality.StringifyCustomSession(appSession), IsActive = true, CreatedDate = DateTime.Now, CreatedBy = 1, 
                    //        ModifiedDate = DateTime.Now, ModifiedBy = 1 });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            Console.WriteLine("All custom data refresh with current structure");
        }

        /// <summary>
        /// Change existing Email ID to new Email ID
        /// </summary>
        /// <param name="oldEmailID"></param>
        /// <param name="newEmailID"></param>
        private static void ChangeEmailID(string[] args)
        {
            BHICDBBase dbConnection = new BHICDBBase();
            dbConnection.DBName = "GuinnessDB";
            try
            {
                if (args.Count() == 3)
                {
                    string oldEmailID = args[1].ToString();
                    string newEmailID = args[2].ToString();
                    if (UtilityFunctions.IsValidRegex(oldEmailID, Constants.EmailRegex))
                    {
                        if (UtilityFunctions.IsValidRegex(newEmailID, Constants.EmailRegex))
                        {
                            dbConnection.OpenConnection();
                            dbConnection.BeginTransaction();
                            UserRegistration user = new UserRegistration { Email = oldEmailID };

                            DataSet dsGetCredentials = dbConnection.LoadDataSet("GetCredentials", QueryCommandType.StoredProcedure,
                                   new List<System.Data.IDbDataParameter> { new SqlParameter() { ParameterName = "@Email", Value = user.Email, SqlDbType = SqlDbType.VarChar } });

                            if (dsGetCredentials.Tables[0].Rows.Count > 0)
                            {
                                user.Id = Convert.ToInt32(dsGetCredentials.Tables[0].Rows[0]["Id"]);
                                user.Password = Encryption.EncryptText(Encryption.DecryptText(dsGetCredentials.Tables[0].Rows[0]["password"].ToString(), user.Email.ToUpper()), newEmailID.ToUpper());

                                dsGetCredentials = dbConnection.LoadDataSet("GetCredentials", QueryCommandType.StoredProcedure,
                                   new List<System.Data.IDbDataParameter> { new SqlParameter() { ParameterName = "@Email", Value = newEmailID, SqlDbType = SqlDbType.VarChar } });

                                if (dsGetCredentials.Tables[0].Rows.Count == 0)
                                {
                                    DataSet dsUserPolicies = dbConnection.LoadDataSet("SELECT p.PolicyNumber FROM OrganisationUserDetail oud INNER JOIN Quote q ON oud.id = q.OrganizationUserDetailID INNER JOIN POLICY p ON q.id = p.Quoteid WHERE oud.ID = @ID AND ISNULL(p.PolicyNumber, '') <> ''", QueryCommandType.Text,
                                        new List<System.Data.IDbDataParameter> {
                                            new SqlParameter() { ParameterName = "@ID", Value = user.Id, SqlDbType = SqlDbType.Int }});

                                    var updatedInfo = dbConnection.ExecuteNonQuery("UPDATE OrganisationUserDetail SET EmailID = @Email, Password = @Password WHERE ID = @ID", QueryCommandType.Text,
                                        new List<System.Data.IDbDataParameter> {
                                            new SqlParameter() { ParameterName = "@Email", Value = newEmailID, SqlDbType = SqlDbType.VarChar },
                                            new SqlParameter() { ParameterName = "@Password", Value = user.Password, SqlDbType = SqlDbType.VarChar },
                                            new SqlParameter() { ParameterName = "@ID", Value = user.Id, SqlDbType = SqlDbType.Int }});


                                    ServiceProvider provider = new GuardServiceProvider() { ServiceCategory = Constants.LOB.WC };
                                    IUserPolicyCodesService userPolicyCodeService = new UserPolicyCodesService(provider);

                                    if (dsUserPolicies.Tables.Count > 0 && dsUserPolicies.Tables[0].Rows.Count > 0)
                                    {
                                        foreach (DataRow dr in dsUserPolicies.Tables[0].Rows)
                                        {
                                            var userPolicyCodeResponse = userPolicyCodeService.AddUserPolicyCode(new UserPolicyCode { UserId = newEmailID, PolicyCode = Convert.ToString(dr["PolicyNumber"]) });

                                            //if request successful is false,log error details
                                            if (!userPolicyCodeResponse.RequestSuccessful)
                                            {
                                                throw new ApplicationException(string.Format("Error occurred while processsing UserPolicyCode API in Account Registration for Policy, please see log for more detail : {0}", userPolicyCodeResponse).ToString());
                                            }
                                            Console.WriteLine(Convert.ToString(dr["PolicyNumber"]));
                                        }
                                    }
                                    dbConnection.CommitTransaction();
                                }
                                else
                                {
                                    throw new ApplicationException(string.Format("Specified new Email ID : '{0}' already exists", newEmailID));
                                }
                            }
                            else
                            {
                                throw new ApplicationException(string.Format("Specified old Email ID : '{0}' not exists", oldEmailID));
                            }
                        }
                        else
                        {
                            throw new ApplicationException(string.Format("Not a valid new Email ID = '{0}'", newEmailID));
                        }
                    }
                    else
                    {
                        throw new ApplicationException(string.Format("Not a valid old Email ID = '{0}'", oldEmailID));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                dbConnection.CloseConnection();
            }
            Console.WriteLine("Email ID successfully changed");
        }

        private static void LinkPolicyWithUser(string[] args)
        {
            try
            {
                ServiceProvider provider = new GuardServiceProvider() { ServiceCategory = Constants.LOB.WC };
                IAccountRegistrationService accountRegistrationService = new AccountRegistrationService(provider);
                if (args.Count() == 3)
                {
                    string userEmailID = args[1].ToString();
                    string policyCode = args[2].ToString();
                    if (UtilityFunctions.IsValidRegex(userEmailID, Constants.EmailRegex))
                    {
                        if (accountRegistrationService.ValidUser(userEmailID))
                        {
                            string returnMsg = accountRegistrationService.LinkPolicyWithUser(userEmailID, policyCode);
                            if (!returnMsg.Equals("OK", StringComparison.OrdinalIgnoreCase))
                            {
                                throw new ApplicationException(returnMsg);
                            }
                        }
                        else
                        {
                            throw new ApplicationException(string.Format("Not a valid User Email ID = '{0}'", userEmailID));
                        }
                    }
                    else
                    {
                        throw new ApplicationException(string.Format("Not a valid Email ID = '{0}'", userEmailID));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            Console.WriteLine("Linking of Policy with User successfully done");
        }
    }
}