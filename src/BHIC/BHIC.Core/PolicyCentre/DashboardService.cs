using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using BHIC.Common;
using BHIC.Common.Client;
using BHIC.Common.Config;
using BHIC.Contract.PolicyCentre;
using BHIC.Domain.Service;
using BHIC.Domain.Dashboard;
using BHIC.Common.DataAccess;
using BHIC.Domain.Policy;
using BHIC.Domain.PolicyCentre;


namespace BHIC.Core.PolicyCentre
{
    public class DashboardService : IDashboardService
    {
        public string GetPolicyIDFromDB(string email)
        {
            string policyid = "";
            try
            {
                BHICDBBase dbConnector = new BHICDBBase();
                var dataSet = dbConnector.LoadDataSet("GetPolicyID", QueryCommandType.StoredProcedure,
                   new List<System.Data.IDbDataParameter> { new SqlParameter() { ParameterName = "@Email", Value = email, SqlDbType = SqlDbType.VarChar } });
                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    policyid = dataSet.Tables[0].Rows[0]["PolicyID"].ToString();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return policyid;

        }

        public List<string> GetPoliciesFromDB(string email)
        {
            List<string> policyNumber = new List<string>();
            try
            {
                BHICDBBase dbConnector = new BHICDBBase();
                var dataSet = dbConnector.LoadDataSet("GetPolicyID", QueryCommandType.StoredProcedure,
                   new List<System.Data.IDbDataParameter> { new SqlParameter() { ParameterName = "@Email", Value = email, SqlDbType = SqlDbType.VarChar } });

                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    // On all tables' rows
                    foreach (DataRow dtRow in dataSet.Tables[0].Rows)
                    {
                        // On all tables' columns
                        foreach (DataColumn dc in dataSet.Tables[0].Columns)
                        {
                            policyNumber.Add(dtRow["PolicyID"].ToString());
                        }
                    }
                }

            }
            catch (Exception e)
            {
                throw e;
            }

            return policyNumber;

        }

        public List<PolicyInformation> GetUserPoliciesFromDB(string email)
        {
            List<PolicyInformation> policyDetails = new List<PolicyInformation>();
            try
            {
                BHICDBBase dbConnector = new BHICDBBase();
                var dataSet = dbConnector.LoadDataSet("GetPolicyID", QueryCommandType.StoredProcedure,
                   new List<System.Data.IDbDataParameter> { new SqlParameter() { ParameterName = "@Email", Value = email, SqlDbType = SqlDbType.VarChar } });

                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    // On all tables' rows
                    foreach (DataRow dtRow in dataSet.Tables[0].Rows)
                    {
                        // On all tables' columns

                        PolicyInformation policyInfo = new PolicyInformation();
                        policyInfo.PolicyCode = dtRow["PolicyID"].ToString();
                        policyInfo.LOB = Convert.ToInt32(dtRow["LOB"]);

                        policyDetails.Add(policyInfo);
                        /*foreach (DataColumn dc in dataSet.Tables[0].Columns)
                        {
                            
                        }*/
                    }
                    //policyid = dataSet.Tables[0].Rows[0]["PolicyID"].ToString();
                }

            }
            catch (Exception e)
            {
                throw e;
            }

            return policyDetails;

        }

        public string GetPolicyStatus(PolicyDetails policyDetails)
        {
            if (policyDetails.Status != null)
            {
                var statuses = policyDetails.Status.Replace("<br>", "|").Split('|');

                var status = statuses[statuses.Length - 1].ToUpper();
                if (status.Contains("ISSUED"))
                {
                    if (policyDetails.PolicyBegin.Date <= DateTime.Now.Date && policyDetails.PolicyExpires > DateTime.Now.Date)
                    {
                        return Constants.Active;
                    }
                    else if (policyDetails.PolicyBegin > DateTime.Now.Date)
                    {
                        return Constants.ActiveSoon;
                    }
                    else if (policyDetails.PolicyExpires < DateTime.Now.Date)
                    {
                        return Constants.Expired + " " + policyDetails.PolicyExpires.ToString("MM/dd/yyyy");
                    }
                }
                else if (status.Contains("PENDING PROPOSAL"))
                {
                    return Constants.ActiveSoon;
                }
                else if (status.Contains("CANCELLED FLAT"))
                {
                    return Constants.NoCoverage;
                }
                else if (status.Contains("CANCELLED PRO RATA") || status.Contains("CANCELLED SHORT RATE"))
                {
                    var cancledStatus = status.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    return Constants.Cancelled + " " + cancledStatus[cancledStatus.Length - 1];
                }
                else if (status.Contains("PENDING CANCELLATION"))
                {
                    var pendingStatus = status.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    return Constants.PendingCancellation + " " + pendingStatus[pendingStatus.Length - 1];
                }
            }

            return policyDetails.Status;
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

        public UserRegistration GetCredentials(UserRegistration user)
        {
            try
            {
                using (IBHICDBBase dbConnector = new BHICDBBase())
                {
                    var dsGetCredentials = dbConnector.LoadDataSet("GetCredentials", QueryCommandType.StoredProcedure,
                       new List<System.Data.IDbDataParameter> { new SqlParameter() { ParameterName = "@Email", Value = user.Email, SqlDbType = SqlDbType.VarChar } });
                    if (dsGetCredentials.Tables[0].Rows.Count > 0)
                    {
                        var decryptedpassword = Encryption.DecryptText(dsGetCredentials.Tables[0].Rows[0]["password"].ToString(), user.Email);
                        if (decryptedpassword.ToUpper().Trim().Equals(user.Password.ToUpper().Trim()))
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
            }
            catch (Exception)
            {
                throw;
            }

            return user;

        }

        public List<DropDownOptions> GetDropdownOptions(int lineOfBusinessID, string TableKey)
        {
            try
            {
                BHICDBBase dbConnector = new BHICDBBase();
                var dataSet = dbConnector.LoadDataSet("GetOptions", QueryCommandType.StoredProcedure,
                  new List<System.Data.IDbDataParameter> { new SqlParameter() { ParameterName = "@TableName", Value = TableKey, SqlDbType = SqlDbType.VarChar },
                   new SqlParameter() { ParameterName = "@LineOfBusinessID", Value = lineOfBusinessID, SqlDbType = SqlDbType.Int }});

                List<DropDownOptions> options = ConvertDatatableToDictionary(dataSet.Tables[0]);
                if (lineOfBusinessID == 1 || TableKey == Constants.PolicyCancelDropdown)
                {
                    return options;
                }
                else
                {
                    return OptionsBySort(options);
                }
            }
            catch
            {
                throw;
            }
        }


        public DropDownOptions GetDropdownOptions(int lineOfBusinessID, string TableKey, int Id)
        {
            try
            {
                BHICDBBase dbConnector = new BHICDBBase();
                var dataSet = dbConnector.LoadDataSet("GetOption", QueryCommandType.StoredProcedure,
                  new List<System.Data.IDbDataParameter> { 
                      new SqlParameter() { ParameterName = "@TableName", Value = TableKey, SqlDbType = SqlDbType.VarChar },
                      new SqlParameter() { ParameterName = "@LineOfBusinessID", Value = lineOfBusinessID, SqlDbType = SqlDbType.Int },
                      new SqlParameter() { ParameterName = "@id", Value = Id, SqlDbType = SqlDbType.Int }
                  });

                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    return new DropDownOptions
                      {
                          id = Convert.ToInt32(dataSet.Tables[0].Rows[0][0]),
                          value = dataSet.Tables[0].Rows[0][1].ToString()
                      };
                }
                else
                {
                    return new DropDownOptions();
                }
            }
            catch
            {
                throw;
            }
        }


        #region Private function

        private List<DropDownOptions> ConvertDatatableToDictionary(DataTable dtInput)
        {
            List<DropDownOptions> lstOutput = new List<DropDownOptions>();

            for (int counter = 0; counter < dtInput.Rows.Count; counter++)
            {
                lstOutput.Add(new DropDownOptions
                {
                    id = Convert.ToInt32(dtInput.Rows[counter][0]),
                    value = dtInput.Rows[counter][1].ToString()
                });

            }
            return lstOutput;
        }
        private List<DropDownOptions> OptionsBySort(List<DropDownOptions> options)
        {
            DropDownOptions otherOption = options.Where(a => a.value.ToLower().Contains("other")).FirstOrDefault();
            if (otherOption != null)
            {
                options.Remove(otherOption);
                options = options.OrderBy(a => a.value).ToList();
                options.Add(otherOption);
            }
            else
            {
                options = options.OrderBy(a => a.value).ToList();
            }
            return options;
        }



        #endregion

    }
}
