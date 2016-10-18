using BHIC.APIService.App_Start;
using BHIC.APIService.Controllers;
using BHIC.Common;
using BHIC.Common.Client;
using BHIC.Common.Config;
using BHIC.Common.DataAccess;
using BHIC.Common.Logging;
using BHIC.Common.Mailing;
using BHIC.Contract.APIService;
using BHIC.Contract.Background;
using BHIC.Contract.Policy;
using BHIC.Core;
using BHIC.Core.Background;
using BHIC.Core.Policy;
using BHIC.DML.WC.DTO;
using BHIC.Domain;
using BHIC.Domain.Dashboard;
using BHIC.Domain.Policy;
using BHIC.Domain.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Web;
using System.Web.Http;

namespace BHIC.APIService.Areas.UserService.Controllers
{
    /// <summary>
    /// Provides User Service
    /// </summary>
    public class UserController : BaseController
    {
        IUserPolicyService service = new APIUserPolicy();
        static DateTime startTime;
        static DateTime endTime;
        private bool isUserExists = false;
        /// <summary>
        /// To create User Policy
        /// </summary>
        /// /******** Logic***********/
        /// 1. First BasicAuthentication triggers to validate the token. If token is valid, continues the below step
        /// 2. Validate the model, if is valid
        /// 2. Validate the email id is exists, if not exists
        /// 3. Call the stored procedure "CreateUserPolicy" through dataserviceprovider to store the data in all the below tables.
        ///            a. OrganisationUserDetail
        ///            b. OraganisationAddress
        ///            c. Quote
        ///            d. Policy
        /// <param name="UserPolicy"></param>
        /// <returns></returns>

        [HttpPost]
        [BasicAuthentication]
        [Route("UsrSVC/CreateUserPolicy")]
        public OperationStatus AddUserPolicy([FromBody]UserPolicyDTO UserPolicy)
        {
            loggingService.Trace(string.Format("AddUserPolicy creation start at {0}", DateTime.Now));

            if (UserPolicy.OrganizationName == null)
                UserPolicy.OrganizationName = string.Empty;

            isUserExists = false;
            startTime = System.DateTime.Now;
            endTime = System.DateTime.Now;
            OperationStatus requestStatus = new OperationStatus();
            requestStatus.ServiceName = "CreateUserPolicy";
            requestStatus.ServiceMethod = "POST";

            if (UserPolicy.InceptionDate.Year > 2000 || ModelState.IsValid)
            {
                try
                {
                    string validateRequest = validateUserPolicy(UserPolicy);
                    if (validateRequest.Length > 0)
                    {
                        requestStatus.RequestProcessed = false;
                        requestStatus.RequestSuccessful = false;
                        Message item = new Message();
                        item.MessageType = MessageType.UserError;
                        item.Text = validateRequest;
                        requestStatus.Messages.Add(item);
                    }
                    else
                    {
                        UserPolicy.Password = Encryption.EncryptText(UserPolicy.Password, UserPolicy.EmailID.ToUpper());
                        Int64 policyId = 0;
                        string url = Request.RequestUri.Host;
                        bool createStatus = service.CreateUserPolicy(UserPolicy, isUserExists, out policyId);
                        if (createStatus)
                        {
                            requestStatus.RequestProcessed = true;
                            requestStatus.RequestSuccessful = true;
                            AffectedId affectedId = new AffectedId();
                            affectedId.DTOName = "UserPolicyDTO";
                            affectedId.DTOProperty = "EmailID";
                            affectedId.IdValue = UserPolicy.EmailID;
                            affectedId.OperationType = OperationType.POST;
                            requestStatus.AffectedIds.Add(affectedId);
                        }
                        else
                        {
                            endTime = System.DateTime.Now;
                            requestStatus.RequestProcessed = true;
                            requestStatus.RequestSuccessful = false;
                        }
                    }

                }
                catch (Exception ex)
                {
                    LogFatal("AddUserPolicy", ex, startTime);
                    Message item = new Message();
                    item.MessageType = MessageType.SystemError;
                    item.Text = "User Policy Creation failed due to unexpected error";
                    requestStatus.Messages.Add(item);
                }


            }
            else
            {
                if (UserPolicy.InceptionDate.Year <= 2000)
                {
                    Message errorMessage = new Message();
                    errorMessage.DTOName = "UserPolicy";
                    errorMessage.DTOProperty = "InceptionDate";
                    errorMessage.MessageType = MessageType.ModelError;
                    errorMessage.Text = "Invalid Inception Date";
                    requestStatus.Messages.Add(errorMessage);
                }
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Message errorMessage = new Message();
                        errorMessage.DTOName = "UserPolicy";
                        errorMessage.DTOProperty = state.Key;
                        errorMessage.MessageType = MessageType.ModelError;
                        errorMessage.Text = error.ErrorMessage;
                        requestStatus.Messages.Add(errorMessage);
                    }
                }

            }
            UserPolicy.Password = string.Empty;
            LogTrace("AddUserPolicy", JsonConvert.SerializeObject(UserPolicy), JsonConvert.SerializeObject(requestStatus), startTime);
            return requestStatus;


        }
        /// <summary>
        /// This method is to validate the User Policy request data.
        /// </summary>
        /// <param name="UserPolicy"></param>
        /// <returns></returns>
        private string validateUserPolicy(UserPolicyDTO UserPolicy)
        {
            string response = "";
            SecureString password = UtilityFunctions.ConvertToSecureString(UserPolicy.Password); //service.GetUserPassword(UserPolicy.EmailID);
            int userDetailId = service.GetUserDetailId(UserPolicy.EmailID);
            isUserExists = userDetailId > 0;
            if (!isUserExists || (!service.IsValidUserCredentials(UserPolicy.EmailID, password)))
            {
                response = "Invalid credentials";
                return response;
            }
            if (service.IsPolicyExists(1, UserPolicy.PolicyNumber))
            {
                response = "Policy Number already exists";
                return response;
            }

            if (service.IsQuoteIdExists(UserPolicy.QuoteNumber, true, userDetailId))
            {
                response = "Quote ID Already Exists";
                return response;
            }
            return response;
        }

        /// <summary>
        /// Return combined Scheme and Host as single value.
        /// </summary>
        /// <returns></returns>
        protected string GetSchemeAndHostURLPart()
        {
            return string.Concat(Request.RequestUri.Scheme, "://", Request.RequestUri.Host);
        }

        /// <summary>
        /// To Check whether the user already exists
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        [HttpGet]
        [BasicAuthentication]
        [Route("UsrSVC/ValidateUserExists")]
        public OperationStatus IsUserExists(string UserName)
        {
            startTime = System.DateTime.Now;
            loggingService.Trace(string.Format("IsUserExists request start at {0}", DateTime.Now));
            OperationStatus requestStatus = new OperationStatus();
            requestStatus.ServiceName = "ValidateUserExists";
            requestStatus.ServiceMethod = "GET";
            try
            {
                bool emailExists = false;
                int userId = service.GetUserDetailId(UserName);
                if (userId > 0)
                    emailExists = true;

                requestStatus.RequestProcessed = true;
                requestStatus.RequestSuccessful = emailExists;
                endTime = System.DateTime.Now;

            }
            catch (Exception ex)
            {
                LogFatal("IsUserExists", ex, startTime);
                Message item = new Message();
                item.MessageType = MessageType.UserInfo;
                item.Text = "IsUserExists Request failed due to unexpected error";
                requestStatus.Messages.Add(item);
            }
            LogTrace("IsUserExists", UserName, JsonConvert.SerializeObject(requestStatus), startTime);
            return requestStatus;
        }

        /// <summary>
        /// To Validate Username and Password
        /// </summary>
        /// <param name="UserName">UserName</param>
        /// <param name="Password">Password</param>
        /// <returns></returns>
        [HttpGet]
        [BasicAuthentication]
        [Route("UsrSVC/ValidateLogin")]
        public OperationStatus ValidateLogin(string UserName, string Password)
        {
            startTime = System.DateTime.Now;
            loggingService.Trace(string.Format("ValidateLogin request start at {0}", DateTime.Now));
            OperationStatus requestStatus = new OperationStatus();
            requestStatus.ServiceName = "ValidateUser";
            requestStatus.ServiceMethod = "GET";
            requestStatus.RequestProcessed = true;
            requestStatus.RequestSuccessful = false;

            try
            {
                SecureString securePassword = UtilityFunctions.ConvertToSecureString(Password);
                if (service.IsValidUserCredentials(UserName, securePassword))
                {
                    requestStatus.RequestSuccessful = true;
                }
            }
            catch
            {
                Message item = new Message();
                item.MessageType = MessageType.UserInfo;
                item.Text = "ValidateLogin Request failed due to unexpected error.";
                requestStatus.Messages.Add(item);
            }
            LogTrace("ValidateLogin", UserName + "~" + Password, JsonConvert.SerializeObject(requestStatus).ToString(), startTime);
            return requestStatus;
        }

        /// <summary>
        /// Save Quote For existing user
        /// </summary>
        /// <param name="requestDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [BasicAuthentication]
        [Route("UsrSVC/SaveQuote")]
        public OperationStatus SaveQuote(SaveQuoteRequestDTO requestDTO)
        {
            startTime = System.DateTime.Now;
            loggingService.Trace(string.Format("SaveQuote request start at {0}", DateTime.Now));
            OperationStatus requestStatus = new OperationStatus();
            requestStatus.ServiceName = "SaveQuote";
            requestStatus.ServiceMethod = "POST";
            requestStatus.RequestProcessed = true;
            requestStatus.RequestSuccessful = false;
            try
            {
                if (ModelState.IsValid && UtilityFunctions.IsValidRegex(requestDTO.UserName, Constants.EmailRegex))
                {
                    bool quoteIDExists = service.IsQuoteIdExists(requestDTO.QuoteNumber, false, 0);
                    if (!quoteIDExists)
                    {
                        requestStatus.RequestSuccessful = true;
                        int userId = service.GetUserDetailId(requestDTO.UserName);
                        if (userId > 0)
                        {

                            int organizationAddressId = service.GetOrganizationAddressId(userId);
                            if (organizationAddressId > 0)
                            {
                                QuoteDTO quote = new QuoteDTO();
                                quote.QuoteNumber = requestDTO.QuoteNumber;
                                quote.OrganizationAddressID = organizationAddressId;
                                quote.OrganizationUserDetailID = userId;
                                quote.RetrieveQuoteURL = requestDTO.ReteriveQuoteURL;
                                quote.LineOfBusinessId = 2;
                                quote.RequestDate = System.DateTime.Now;
                                quote.ModifiedDate = System.DateTime.Now;
                                quote.CreatedDate = System.DateTime.Now;
                                quote.ExternalSystemID = 1;
                                quote.IsActive = true;
                                quote.CreatedBy = 1;
                                quote.ModifiedBy = 1;
                                quote.ExpiryDate = System.DateTime.Now;
                                bool status = service.SaveQuote(quote);
                                endTime = System.DateTime.Now;
                                requestStatus.RequestSuccessful = status;
                            }
                            else
                            {
                                requestStatus.RequestSuccessful = false;
                                AddErrorMessage(requestStatus, string.Empty, string.Empty, MessageType.UserError, "User does not exist");
                            }
                        }
                        else
                        {
                            requestStatus.RequestSuccessful = false;
                            AddErrorMessage(requestStatus, string.Empty, string.Empty, MessageType.UserError, "User does not exist");
                        }
                        if (requestDTO.ReteriveQuoteURL != null && requestDTO.ReteriveQuoteURL.Length > 0)
                        {
                            service.SendSaveForLaterMailNotification(requestDTO);
                        }
                        else
                        {
                            LogTrace("SaveQuote", JsonConvert.SerializeObject(requestDTO), "Email Not sent due to reterival url not present", startTime);
                        }
                    }
                    else
                    {
                        AddErrorMessage(requestStatus, string.Empty, string.Empty, MessageType.UserError, "Quote ID Already Exists");
                    }
                }
                else
                {

                    foreach (var state in ModelState)
                    {
                        if (state.Value.Errors.Count > 0)
                        {
                        foreach (var error in state.Value.Errors)
                        {
                                AddErrorMessage(requestStatus, "SaveQuoteRequestDTO", state.Key, MessageType.ModelError, error.ErrorMessage);
                            }
                        }
                        else
                        {
                            AddErrorMessage(requestStatus, "SaveQuoteRequestDTO", string.Empty, MessageType.UserError, "EmailId not Valid");
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                LogFatal("SaveQuote", ex, startTime);
                AddErrorMessage(requestStatus, string.Empty, string.Empty, MessageType.UserInfo, "Save Quote Failed due to unexpected error.");
            }
            LogTrace("SaveQuote", JsonConvert.SerializeObject(requestDTO), JsonConvert.SerializeObject(requestStatus), startTime);
            return requestStatus;

        }

        /// <summary>
        /// Deletes empty account (Account without policy).
        /// <br/>
        /// <br/><b>Validation:</b>
        /// <br/>1.	Email should be valid.
        /// <br/>Error Message for failure “Not a valid Email ID”
        /// <br/>2.	No account exists in Policy Centre.
        /// <br/>Error Message for failure “No user exists with email id”
        /// </summary>
        /// <param name="deleteAccountDTO"></param>
        /// <returns></returns>
        [HttpDelete]
        [BasicAuthentication]
        [Route("UsrSVC/DeleteAccount")]
        public OperationStatus DeleteUserAccount(DeleteAccountDTO deleteAccountDTO)
        {
            string EmailID = deleteAccountDTO.EmailID;
            startTime = System.DateTime.Now;
            loggingService.Trace(string.Format("DeleteAccount request start at {0}", DateTime.Now));
            OperationStatus requestStatus = new OperationStatus();
            requestStatus.ServiceName = "DeleteAccount";
            requestStatus.ServiceMethod = "POST";
            requestStatus.RequestProcessed = true;
            requestStatus.RequestSuccessful = false;
            try
            {
                Message item = new Message();
                item.MessageType = MessageType.UserInfo;
                if (UtilityFunctions.IsValidRegex(EmailID, Constants.EmailRegex))
                {
                    int userId = service.GetUserDetailId(EmailID);
                    if (userId > 0)
                    {
                        int RowsAffected = service.DeleteAccount(EmailID);
                        if (RowsAffected > 0)
                        {
                            //item.Text = "User account deleted successfully.";
                            requestStatus.RequestSuccessful = true;
                        }
                        else
                        {
                            item.Text = "No data affected while calling DeleteAccount for " + EmailID;
                        }
                    }
                    else
                    {
                        item.Text = string.Format("No user exists with email id '{0}'", EmailID);
                    }
                }
                else
                {
                    item.Text = string.Format("Not a valid Email ID = '{0}'", EmailID);
                }
                if (!requestStatus.RequestSuccessful)
                requestStatus.Messages.Add(item);
            }
            catch (SqlException ex)
            {
                Message item = new Message();
                item.MessageType = MessageType.SystemError;
                item.Text = ex.Message;
                requestStatus.Messages.Add(item);
            }
            catch (Exception)
            {
                Message item = new Message();
                item.MessageType = MessageType.UserInfo;
                item.Text = "MergeAccounts Request failed due to unexpected error.";
                requestStatus.Messages.Add(item);
            }

            LogTrace("DeleteAccount", EmailID, JsonConvert.SerializeObject(requestStatus).ToString(), startTime);
            return requestStatus;
        }

        /// <summary>
        /// Merges two accounts which exist in Policy Center and transfer all policies from secondary account to primary. E.g.: Account A1 has policy P1, P2 and account A2 has policy P3. If you merge A1 into A2 than P1, P2, P3 will link to A2 and A1 will be empty.
        /// <br/>
        /// <br/><b>Validation:</b>
        /// <br/>1.	Old Email should be valid.
        /// <br/>Error Message for failure “Not a valid old Email ID”
        /// <br/>2.	New Email should be valid.
        /// <br/>Error Message for failure “Not a valid new Email ID”
        /// <br/>3.	Old Email should exist in policy centre.
        /// <br/>Error Message for failure “Specified Old Email ID not exists”
        /// <br/>4.	New Email should exist in policy centre 
        /// <br/>Error Message for failure “Specified New Email ID not exists”
        /// </summary>
        /// <param name="emailUpdateDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [BasicAuthentication]
        [Route("UsrSVC/MergeAccounts")]
        public OperationStatus MergeAccounts(EmailUpdateDTO emailUpdateDTO)
        {
            string OldEmailID = emailUpdateDTO.OldEmailID;
            string NewEmailID = emailUpdateDTO.NewEmailID;
            startTime = System.DateTime.Now;
            loggingService.Trace(string.Format("MergeAccounts request start at {0}", DateTime.Now));
            OperationStatus requestStatus = new OperationStatus();
            requestStatus.ServiceName = "MergeAccounts";
            requestStatus.ServiceMethod = "POST";
            requestStatus.RequestProcessed = true;
            requestStatus.RequestSuccessful = false;

            BHICDBBase dbConnection = new BHICDBBase();
            dbConnection.DBName = "GuinnessDB";
            try
            {
                Message item = new Message();
                item.MessageType = MessageType.UserInfo;
                if (!string.IsNullOrWhiteSpace(OldEmailID) && !string.IsNullOrWhiteSpace(NewEmailID))
                {
                    if (UtilityFunctions.IsValidRegex(OldEmailID, Constants.EmailRegex))
                    {
                        if (UtilityFunctions.IsValidRegex(NewEmailID, Constants.EmailRegex))
                        {
                            dbConnection.OpenConnection();
                            dbConnection.BeginTransaction();
                            UserRegistration user = new UserRegistration { Email = OldEmailID };

                            DataSet dsGetCredentials = dbConnection.LoadDataSet("GetCredentials", QueryCommandType.StoredProcedure,
                                   new List<System.Data.IDbDataParameter> { new SqlParameter() { ParameterName = "@Email", Value = user.Email, SqlDbType = SqlDbType.VarChar } });

                            if (dsGetCredentials.Tables[0].Rows.Count > 0)
                            {
                                user.Id = Convert.ToInt32(dsGetCredentials.Tables[0].Rows[0]["Id"]);
                                user.Password = Encryption.EncryptText(Encryption.DecryptText(dsGetCredentials.Tables[0].Rows[0]["password"].ToString(), user.Email.ToUpper()), NewEmailID.ToUpper());

                                dsGetCredentials = dbConnection.LoadDataSet("GetCredentials", QueryCommandType.StoredProcedure,
                                   new List<System.Data.IDbDataParameter> { new SqlParameter() { ParameterName = "@Email", Value = NewEmailID, SqlDbType = SqlDbType.VarChar } });

                                if (dsGetCredentials.Tables[0].Rows.Count > 0)
                                {
                                    DataSet dsUserPolicies = dbConnection.LoadDataSet("SELECT p.PolicyNumber FROM OrganisationUserDetail oud INNER JOIN Quote q ON oud.id = q.OrganizationUserDetailID INNER JOIN POLICY p ON q.id = p.Quoteid WHERE oud.ID = @ID AND ISNULL(p.PolicyNumber, '') <> ''", QueryCommandType.Text,
                                        new List<System.Data.IDbDataParameter> {
                                            new SqlParameter() { ParameterName = "@ID", Value = user.Id, SqlDbType = SqlDbType.Int }});

                                    service.MergeAccounts(OldEmailID, NewEmailID);

                                    ServiceProvider provider = new GuardServiceProvider() { ServiceCategory = Constants.LOB.WC };
                                    IUserPolicyCodesService userPolicyCodeService = new UserPolicyCodesService(provider);

                                    if (dsUserPolicies.Tables.Count > 0 && dsUserPolicies.Tables[0].Rows.Count > 0)
                                    {
                                        foreach (DataRow dr in dsUserPolicies.Tables[0].Rows)
                                        {
                                            var userPolicyCodesResponse = userPolicyCodeService.GetUserPolicyCodes(new UserPolicyCodeRequestParms { UserId = user.Email, PolicyCode = Convert.ToString(dr["PolicyNumber"]) });
                                            if (userPolicyCodesResponse.OperationStatus.RequestSuccessful)
                                            {
                                                foreach (UserPolicyCode userPolicyCode in userPolicyCodesResponse.UserPolicyCodes)
                                                {
                                                    userPolicyCode.UserId = NewEmailID;
                                                    var userPolicyCodeResponse = userPolicyCodeService.AddUserPolicyCode(userPolicyCode);

                                                    //if request successful is false,log error details
                                                    if (userPolicyCodeResponse.RequestSuccessful)
                                                    {
                                                        requestStatus.RequestSuccessful = true;
                                                        //item.Text = "Email ID successfully merged";
                                                    }
                                                    else
                                                    {
                                                        item.Text = string.Format("Error occurred while processsing UserPolicyCode API in Account Registration for Policy, please see log for more detail : {0}", userPolicyCodeResponse);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    dbConnection.CommitTransaction();
                                }
                                else
                                {
                                    item.Text = string.Format("Specified New Email ID : '{0}' not exists", NewEmailID);
                                }
                            }
                            else
                            {
                                item.Text = string.Format("Specified old Email ID : '{0}' not exists", OldEmailID);
                            }
                        }
                        else
                        {
                            item.Text = string.Format("Not a valid new Email ID = '{0}'", NewEmailID);
                        }
                    }
                    else
                    {
                        item.Text = string.Format("Not a valid old Email ID = '{0}'", OldEmailID);
                    }
                }
                if (!requestStatus.RequestSuccessful)
                requestStatus.Messages.Add(item);
            }
            catch (Exception)
            {
                Message item = new Message();
                item.MessageType = MessageType.UserInfo;
                item.Text = "MergeAccounts Request failed due to unexpected error.";
                requestStatus.Messages.Add(item);
                dbConnection.RollbackTransaction();
            }
            finally
            {
                dbConnection.CloseConnection();
            }

            LogTrace("MergeAccounts", OldEmailID + "~" + NewEmailID, JsonConvert.SerializeObject(requestStatus).ToString(), startTime);
            return requestStatus;
        }

        /// <summary>
        /// Renames email in policy account in case of typo mistake during policy purchase. E.g. User bought a policy but used wrong email (abc@GUARD.com). Now want to change it to xyz@GUARD.com.
        /// <br/>
        /// <br/><b>Validation:</b>
        /// <br/>1.	Old Email should be valid. 
        /// <br/>Error Message for failure “Not a valid old Email ID”
        /// <br/>2.	New Email should be valid. 
        /// <br/>Error Message for failure “Not a valid new Email ID”
        /// <br/>3.	Old Email should exist in Policy Centre.
        /// <br/>Error Message for failure “Specified old Email ID not exists”
        /// <br/>4.	New Email should not exist in Policy Centre.
        /// <br/>Error Message for failure “Specified New Email ID already exists”
        /// </summary>
        /// <param name="emailUpdateDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [BasicAuthentication]
        [Route("UsrSVC/ChangeEmailID")]
        public OperationStatus UpdateEmailAddress(EmailUpdateDTO emailUpdateDTO)
        {
            string OldEmailID = emailUpdateDTO.OldEmailID;
            string NewEmailID = emailUpdateDTO.NewEmailID;
            startTime = System.DateTime.Now;
            loggingService.Trace(string.Format("ChangeEmailID request start at {0}", DateTime.Now));
            OperationStatus requestStatus = new OperationStatus();
            requestStatus.ServiceName = "ChangeEmailID";
            requestStatus.ServiceMethod = "POST";
            requestStatus.RequestProcessed = true;
            requestStatus.RequestSuccessful = false;

            BHICDBBase dbConnection = new BHICDBBase();
            dbConnection.DBName = "GuinnessDB";
            try
            {
                Message item = new Message();
                item.MessageType = MessageType.UserInfo;
                if (!string.IsNullOrWhiteSpace(OldEmailID) && !string.IsNullOrWhiteSpace(NewEmailID))
                {
                    if (UtilityFunctions.IsValidRegex(OldEmailID, Constants.EmailRegex))
                    {
                        if (UtilityFunctions.IsValidRegex(NewEmailID, Constants.EmailRegex))
                        {
                            dbConnection.OpenConnection();
                            dbConnection.BeginTransaction();
                            UserRegistration user = new UserRegistration { Email = OldEmailID };

                            DataSet dsGetCredentials = dbConnection.LoadDataSet("GetCredentials", QueryCommandType.StoredProcedure,
                                   new List<System.Data.IDbDataParameter> { new SqlParameter() { ParameterName = "@Email", Value = user.Email, SqlDbType = SqlDbType.VarChar } });

                            if (dsGetCredentials.Tables[0].Rows.Count > 0)
                            {
                                user.Id = Convert.ToInt32(dsGetCredentials.Tables[0].Rows[0]["Id"]);
                                user.Password = Encryption.EncryptText(Encryption.DecryptText(dsGetCredentials.Tables[0].Rows[0]["password"].ToString(), user.Email.ToUpper()), NewEmailID.ToUpper());

                                dsGetCredentials = dbConnection.LoadDataSet("GetCredentials", QueryCommandType.StoredProcedure,
                                   new List<System.Data.IDbDataParameter> { new SqlParameter() { ParameterName = "@Email", Value = NewEmailID, SqlDbType = SqlDbType.VarChar } });

                                if (dsGetCredentials.Tables[0].Rows.Count == 0)
                                {
                                    DataSet dsUserPolicies = dbConnection.LoadDataSet("SELECT p.PolicyNumber FROM OrganisationUserDetail oud INNER JOIN Quote q ON oud.id = q.OrganizationUserDetailID INNER JOIN POLICY p ON q.id = p.Quoteid WHERE oud.ID = @ID AND ISNULL(p.PolicyNumber, '') <> ''", QueryCommandType.Text,
                                        new List<System.Data.IDbDataParameter> {
                                            new SqlParameter() { ParameterName = "@ID", Value = user.Id, SqlDbType = SqlDbType.Int }});

                                    var updatedInfo = dbConnection.ExecuteNonQuery("UPDATE OrganisationUserDetail SET EmailID = @Email, Password = @Password WHERE ID = @ID", QueryCommandType.Text,
                                        new List<System.Data.IDbDataParameter> {
                                            new SqlParameter() { ParameterName = "@Email", Value = NewEmailID, SqlDbType = SqlDbType.VarChar },
                                            new SqlParameter() { ParameterName = "@Password", Value = user.Password, SqlDbType = SqlDbType.VarChar },
                                            new SqlParameter() { ParameterName = "@ID", Value = user.Id, SqlDbType = SqlDbType.Int }});


                                    ServiceProvider provider = new GuardServiceProvider() { ServiceCategory = Constants.LOB.WC };
                                    IUserPolicyCodesService userPolicyCodeService = new UserPolicyCodesService(provider);

                                    if (dsUserPolicies.Tables.Count > 0 && dsUserPolicies.Tables[0].Rows.Count > 0)
                                    {
                                        foreach (DataRow dr in dsUserPolicies.Tables[0].Rows)
                                        {
                                            var userPolicyCodesResponse = userPolicyCodeService.GetUserPolicyCodes(new UserPolicyCodeRequestParms { UserId = user.Email, PolicyCode = Convert.ToString(dr["PolicyNumber"]) });
                                            if (userPolicyCodesResponse.OperationStatus.RequestSuccessful)
                                            {
                                                foreach (UserPolicyCode userPolicyCode in userPolicyCodesResponse.UserPolicyCodes)
                                                {
                                                    userPolicyCode.UserId = NewEmailID;
                                                    var userPolicyCodeResponse = userPolicyCodeService.AddUserPolicyCode(userPolicyCode);

                                                    //if request successful is false,log error details
                                                    if (userPolicyCodeResponse.RequestSuccessful)
                                                    {
                                                        requestStatus.RequestSuccessful = true;
                                                        // item.Text = "Email ID successfully changed";
                                                    }
                                                    else
                                                    {
                                                        item.Text = string.Format("Error occurred while processsing UserPolicyCode API in Account Registration for Policy, please see log for more detail : {0}", userPolicyCodeResponse);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    dbConnection.CommitTransaction();
                                }
                                else
                                {
                                    item.Text = string.Format("Specified new Email ID : '{0}' already exists", NewEmailID);
                                }
                            }
                            else
                            {
                                item.Text = string.Format("Specified old Email ID : '{0}' not exists", OldEmailID);
                            }
                        }
                        else
                        {
                            item.Text = string.Format("Not a valid new Email ID = '{0}'", NewEmailID);
                        }
                    }
                    else
                    {
                        item.Text = string.Format("Not a valid old Email ID = '{0}'", OldEmailID);
                    }
                }
                if (!requestStatus.RequestSuccessful)
                requestStatus.Messages.Add(item);
            }
            catch (Exception)
            {
                Message item = new Message();
                item.MessageType = MessageType.UserInfo;
                item.Text = "ChangeEmailID Request failed due to unexpected error.";
                requestStatus.Messages.Add(item);
                dbConnection.RollbackTransaction();
            }
            finally
            {
                dbConnection.CloseConnection();
            }

            LogTrace("ChangeEmailID", OldEmailID + "~" + NewEmailID, JsonConvert.SerializeObject(requestStatus).ToString(), startTime);
            return requestStatus;
        }

        /// <summary>
        /// Links backend created Policy to an existing policy centre user account. Backend created policy means other than front end. For e.g. policy purchase after referral.
        /// <br/>
        /// <br/><b>Validation:</b>
        /// <br/>1.	Email should be valid.
        /// <br/>Error Message for failure “Not a valid Email ID”
        /// <br/>2.	No account exists in Policy Centre.
        /// <br/>Error Message for failure “Not a valid User Email ID”
        /// <br/>3.	Policy Number should exist in backend system.
        /// <br/>Error Message for failure “Policy Number does not exist. Please verify your Policy Number.”
        /// <br/>4.	Policy Number should not already be linked to any other policy holder account.
        /// <br/>Error Message for failure “Policy Number already exist. Please verify your Policy Number.”
        /// <br/>5.	There should not be any existing user account with the given email id.
        /// <br/>Error Message for failure “Email-Id already exist. Please verify your Email-Id.”
        /// </summary>
        /// <param name="linkPolicyDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [BasicAuthentication]
        [Route("UsrSVC/LinkPolicy")]
        public OperationStatus LinkUserPolicy(LinkPolicyDTO linkPolicyDTO)
        {
            string EmailID = linkPolicyDTO.EmailID;
            string PolicyCode = linkPolicyDTO.PolicyNumber;
            startTime = System.DateTime.Now;
            loggingService.Trace(string.Format("LinkPolicy request start at {0}", DateTime.Now));
            OperationStatus requestStatus = new OperationStatus();
            requestStatus.ServiceName = "LinkPolicy";
            requestStatus.ServiceMethod = "POST";
            requestStatus.RequestProcessed = true;
            requestStatus.RequestSuccessful = false;

            try
            {

                if (!string.IsNullOrWhiteSpace(EmailID) && !string.IsNullOrWhiteSpace(PolicyCode))
                {
                    Message item = new Message();
                    item.MessageType = MessageType.UserInfo;
                    if (UtilityFunctions.IsValidRegex(EmailID, Constants.EmailRegex))
                    {
                        ServiceProvider provider = new GuardServiceProvider() { ServiceCategory = Constants.LOB.WC };
                        IAccountRegistrationService accountRegistrationService = new AccountRegistrationService(provider);

                        if (accountRegistrationService.IsUserEmailExists(EmailID))
                        {
                        if (accountRegistrationService.ValidUser(EmailID))
                        {
                            string returnMsg = accountRegistrationService.LinkPolicyWithUser(EmailID, PolicyCode);
                            if (returnMsg.Equals("OK", StringComparison.OrdinalIgnoreCase))
                            {
                                requestStatus.RequestSuccessful = true;
                                    //item.Text = "Linking of Policy with User successfully done";
                            }
                            else
                            {
                                item.Text = returnMsg;
                            }
                        }
                        else
                        {
                                item.Text = string.Format("Email ID = '{0}' is not active.", EmailID);
                            }
                        }
                        else
                        {
                            item.Text = string.Format("No user exist with the email id = '{0}'.", EmailID);
                        }
                    }
                    else
                    {
                        item.Text = string.Format("Not a valid Email ID = '{0}'", EmailID);
                    }
                    if (!requestStatus.RequestSuccessful)
                    requestStatus.Messages.Add(item);
                }
            }
            catch (Exception ex)
            {
                Message item = new Message();
                item.MessageType = MessageType.UserInfo;
                item.Text = ex.Message;// "LinkPolicy Request failed due to unexpected error.";
                requestStatus.Messages.Add(item);
            }

            LogTrace("LinkPolicy", EmailID + "~" + PolicyCode, JsonConvert.SerializeObject(requestStatus).ToString(), startTime);
            return requestStatus;
        }

        /// <summary>
        /// Creates empty account in policy centre for secondary users (secondary user means support team user).  
        /// <br/> 
        /// <br/><b>Validation:</b>
        /// <br/>1.	Email should be valid.
        /// <br/>Error Message for failure “Not a valid Email ID”
        /// <br/>2.	Password should be valid.
        /// <br/>Error Message for failure “Not a valid Password”
        /// <br/>
        /// <br/>To satisfy your password requirements, please make sure your password meets the following requirements:
        /// <br/>
        /// <br/>Be at least 8 characters’ long
        /// <br/>Have at least one lowercase letter (a-z)
        /// <br/>Have at least one uppercase (A-Z) letter
        /// <br/>Have at least one (digit) number (0-9)
        /// <br/>Have at least one special character (!@#$%^&amp;*)
        /// <br/>
        /// <br/>3.	First Name should be valid.
        /// <br/>Error Message for failure “Not a valid User First Name”
        /// <br/>4.	Last Name should be valid.
        /// <br/>Error Message for failure “Not a valid User Last Name”
        /// <br/>5.	Phone Number should be valid.
        /// <br/>Error Message for failure “Not a valid Phone Number”
        /// <br/>6.	Email should not exist in CYB.
        /// <br/>Error Message for failure “Email id already exists”
        /// </summary>
        /// <param name="accountRegistaration"></param>
        /// <returns></returns>
        [HttpPost]
        [BasicAuthentication]
        [Route("UsrSVC/CreateAccount")]
        public OperationStatus CreateAccount(SecondaryAccountRegistration accountRegistaration)
        {
            startTime = System.DateTime.Now;

            loggingService.Trace(string.Format("CreateAccount request start at {0}", DateTime.Now));

            OperationStatus requestStatus = new OperationStatus();

            requestStatus.ServiceName = "CreateAccount";
            requestStatus.ServiceMethod = "POST";
            requestStatus.RequestProcessed = true;
            requestStatus.RequestSuccessful = false;

            accountRegistaration.UserRoleId = 2;

            try
            {
                if (!string.IsNullOrEmpty(accountRegistaration.OrganizationName) && !string.IsNullOrEmpty(accountRegistaration.Email)
                    && !string.IsNullOrEmpty(accountRegistaration.FirstName) && !string.IsNullOrEmpty(accountRegistaration.LastName)
                    //&& !string.IsNullOrEmpty(accountRegistaration.PolicyCode)
                    && accountRegistaration.PhoneNumber > 0)
                {
                    Message item = new Message();
                    item.MessageType = MessageType.UserInfo;

                    //validate email with regex
                    if (UtilityFunctions.IsValidRegex(accountRegistaration.Email, Constants.EmailRegex))
                    {
                        //validate password with regex
                        if (UtilityFunctions.IsValidPassword(accountRegistaration.Password, Constants.PasswordRegex))
                        {
                        if (UtilityFunctions.IsValidRegex(accountRegistaration.FirstName, Constants.UserNameRegex))
                        {
                            if (UtilityFunctions.IsValidRegex(accountRegistaration.LastName, Constants.UserNameRegex))
                            {
                                //if (UtilityFunctions.IsValidRegex(accountRegistaration.PolicyCode, Constants.PolicyCodeRegex))
                                //{
                                    if (UtilityFunctions.IsValidRegex(accountRegistaration.PhoneNumber.ToString(), Constants.PhoneLengthRegex))
                                    {
                                        ServiceProvider provider = new GuardServiceProvider() { ServiceCategory = Constants.LOB.WC };
                                        IAccountRegistrationService accountRegistrationService = new AccountRegistrationService(provider);

                                        //generate random password 
                                        //var randPass = service.GeneratePassword(8, 4);

                                        accountRegistaration.Password = Encryption.EncryptText(accountRegistaration.Password, accountRegistaration.Email.ToUpper());
                                        accountRegistaration.ConfirmPassword = Encryption.EncryptText(accountRegistaration.ConfirmPassword, accountRegistaration.Email.ToUpper());

                                        var returnMsg = service.CreateAccount(accountRegistaration);

                                        if (returnMsg.Id > 0)
                                        {
                                            requestStatus.RequestSuccessful = true;
                                            // item.Text = "Account created successfully";

                                            //MailHelper mailHelper = new MailHelper();

                                            //send random password to user through mail
                                            //mailHelper.SendMailMessage(null, new List<string> { accountRegistaration.Email }, "Password", randPass);
                                        }
                                        else
                                        {
                                            item.Text = string.Format("Email id already exists = {0}", accountRegistaration.Email);
                                        }
                                    }
                                    else
                                    {
                                        item.Text = string.Format("Not a valid Phone Number = '{0}'", accountRegistaration.PhoneNumber);
                                    }
                                //}
                                //else
                                //{
                                //    item.Text = string.Format("Not a valid Policy Code = '{0}'", accountRegistaration.PolicyCode);
                                //}
                            }
                            else
                            {
                                item.Text = string.Format("Not a valid User Last Name= '{0}'", accountRegistaration.LastName);
                            }
                        }
                        else
                        {
                            item.Text = string.Format("Not a valid User First Name= '{0}'", accountRegistaration.FirstName);
                        }
                        }
                        else
                        {
                            item.Text = string.Format("Not a valid Password");
                        }
                    }
                    else
                    {
                        item.Text = string.Format("Not a valid Email ID = '{0}'", accountRegistaration.Email);
                    }
                    if (!requestStatus.RequestSuccessful)
                    requestStatus.Messages.Add(item);
                }

            }
            catch (Exception)
            {
                Message item = new Message();
                item.MessageType = MessageType.UserInfo;
                item.Text = "CreateAccount Request failed due to unexpected error.";

                requestStatus.Messages.Add(item);
            }

            LogTrace("CreateAccount", accountRegistaration.Email, JsonConvert.SerializeObject(requestStatus).ToString(), startTime);

            return requestStatus;
        }

        private void AddErrorMessage(OperationStatus errorItem, string dtoName, string dtoProperty, MessageType messageType, string messageText)
        {
            Message item = new Message();
            item.MessageType = messageType;
            item.DTOName = dtoName;
            item.DTOProperty = dtoProperty;
            item.Text = messageText;
            errorItem.Messages.Add(item);
        }

    }
}