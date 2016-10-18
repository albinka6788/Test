#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Common.Logging;
using BHIC.DML.WC.DataContract;
using BHIC.Common.DataAccess;
using DML = BHIC.DML.WC.DTO;
using System.Data.SqlClient;
using System.Data;

#endregion

namespace BHIC.DML.WC.DataService
{
    public class CustomSessionService : BaseDataProvider,ICustomSession
    {
        /// <summary>
        /// Add or update application current session data into DB layer
        /// </summary>
        /// <param name="customSession"></param>
        /// <returns></returns>
        public bool AddCustomSession(DML::CustomSession customSession)
        {
            try
            {
                //Comment : Here get DbConnector object
                var rowEffeted = GetDbConnector().ExecuteNonQuery("MaintainApplicationCustomSession", QueryCommandType.StoredProcedure,
                                new List<System.Data.IDbDataParameter> 
                                    { 
                                        new SqlParameter() { ParameterName = "@QuoteId", Value = customSession.QuoteID, SqlDbType = SqlDbType.BigInt } ,
                                        new SqlParameter() { ParameterName = "@SessionData", Value = customSession.SessionData, SqlDbType = SqlDbType.VarChar, Size = customSession.SessionData.Length },
                                        new SqlParameter() { ParameterName = "@IsActive", Value = customSession.IsActive, SqlDbType = SqlDbType.Bit },
                                        new SqlParameter() { ParameterName = "@CreatedDate", Value = customSession.CreatedDate, SqlDbType = SqlDbType.DateTime } ,
                                        new SqlParameter() { ParameterName = "@CreatedBy", Value = customSession.CreatedBy, SqlDbType = SqlDbType.Int } ,
                                        new SqlParameter() { ParameterName = "@ModifiedDate", Value = customSession.ModifiedDate, SqlDbType = SqlDbType.DateTime } ,
                                        new SqlParameter() { ParameterName = "@ModifiedBy", Value = customSession.ModifiedBy, SqlDbType = SqlDbType.Int },
                                        new SqlParameter() { ParameterName = "@UpdateOnly", Value = customSession.UpdateOnly, SqlDbType = SqlDbType.Bit },
                                    });

                if (rowEffeted > 0)
                {
                    return true;
                }
                else
                {
                    if (!customSession.UpdateOnly)
                    {
                        LoggingService.Instance.Fatal(string.Format("Following Custom session data not saved:{0}{1}", Environment.NewLine, customSession.SessionData));
                    }
                }
            }
            catch (Exception ex)
            {
                //Comment : Here in case of DB updation failure log it into XML for future re-trying by schedular service
                LoggingService.Instance.Fatal(ex);
            }

            return false;
        }

        /// <summary>
        /// Retrieve string data of stored quote session data
        /// </summary>
        /// <param name="quoteId"></param>
        /// <returns></returns>
        public string GetCustomSession(int quoteId,int userId)
        {
            try
            {
                //Comment : Here get DbConnector object
                var customSessionData = GetDbConnector().LoadDataSet("GetApplicationCustomSession", QueryCommandType.StoredProcedure,
                                new List<System.Data.IDbDataParameter> 
                                    { 
                                        new SqlParameter() { ParameterName = "@QuoteId", Value = quoteId, SqlDbType = SqlDbType.BigInt } ,
                                        new SqlParameter() { ParameterName = "@UserId", Value = userId, SqlDbType = SqlDbType.Int } ,
                                    });

                if (customSessionData != null && customSessionData.Tables.Count >0)
                {
                    var dataRows = customSessionData.Tables[0].Rows;
                    return dataRows.Count > 0 ? dataRows[0]["SessionData"].ToString() : string.Empty;
                }
            }
            catch (Exception ex)
            {
                //Comment : Here in case of DB updation failure log it into XML for future re-trying by schedular service
                LoggingService.Instance.Fatal(ex);
            }

            return string.Empty;
        }
    }
}
