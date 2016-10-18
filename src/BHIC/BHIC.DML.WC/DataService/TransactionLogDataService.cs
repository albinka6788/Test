using BHIC.Common.DataAccess;
using BHIC.DML.WC.DataContract;
using BHIC.Domain.TransactionTrace;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BHIC.DML.WC.DataService
{
    public class TransactionLogDataService : BaseDataProvider, ITransactionLogDataService
    {
        #region Public Method 
        /*Use to insert data related to Transacttion Logging*/
        public long AddTransactionLog(TransactionLog transactionlog)
        {

            long transactionlogId = 0;

            List<IDbDataParameter> dbParams = new List<IDbDataParameter>
                {
                                        new SqlParameter() { ParameterName = "@UserIP", Value = transactionlog.UserIP, SqlDbType = SqlDbType.VarChar,Size=20 } ,
                                        new SqlParameter() { ParameterName = "@RequestUrl", Value = transactionlog.RequestUrl, SqlDbType = SqlDbType.VarChar,Size=300  } ,
                                        new SqlParameter() { ParameterName = "@RequestType", Value = transactionlog.RequestType, SqlDbType = SqlDbType.VarChar,Size=20 }, 
                                        new SqlParameter() { ParameterName = "@RequestDateTime", Value =transactionlog.RequestDateTime, SqlDbType = SqlDbType.DateTime } ,
                                        new SqlParameter() { ParameterName = "@ResponseDateTime", Value =transactionlog.ResponseDateTime, SqlDbType = SqlDbType.DateTime } ,
                                        new SqlParameter() { ParameterName = "@RequestProcessTime", Value = transactionlog.RequestProcessTime, SqlDbType = SqlDbType.BigInt } ,
                                        new SqlParameter() { ParameterName = "@ResponseSize", Value = transactionlog.ResponseSize, SqlDbType = SqlDbType.BigInt,IsNullable=true} ,
                                       
                                        new SqlParameter() { ParameterName = "@TotalAPICalls", Value = transactionlog.ApiTransactions.Count, SqlDbType = SqlDbType.Int,IsNullable=true} ,
                                        new SqlParameter() { ParameterName = "@TotalAPIProcessTime", Value = transactionlog.ApiTransactions.Sum(t=>t.ApiRequestProcessTime), SqlDbType = SqlDbType.BigInt,IsNullable=true} ,
                                       
                                        new SqlParameter() { ParameterName = "@TotalDBCalls", Value = transactionlog.Dbtransactions.Count, SqlDbType = SqlDbType.Int,IsNullable=true} ,
                                        new SqlParameter() { ParameterName = "@TotalDBProcessTime", Value = transactionlog.Dbtransactions.Sum(t=>t.DbRequestProcessTime), SqlDbType = SqlDbType.BigInt,IsNullable=true} ,

                                        new SqlParameter() { ParameterName = "@PaymentErrorDetail", Value = transactionlog.PaymentErrorDetail, SqlDbType = SqlDbType.VarChar,Size=1000  ,IsNullable=true}, 
                                        new SqlParameter() { ParameterName = "@QuoteId", Value = transactionlog.QuoteId, SqlDbType = SqlDbType.BigInt,IsNullable=true}, 
                                        new SqlParameter() { ParameterName = "@UserId", Value = transactionlog.UserId, SqlDbType = SqlDbType.VarChar,Size=50  ,IsNullable=true}, 
                                        new SqlParameter() { ParameterName = "@ThreadId", Value = transactionlog.ThreadId, SqlDbType = SqlDbType.VarChar,Size=50  ,IsNullable=true}, 
                                        new SqlParameter() { ParameterName = "@Browser", Value = transactionlog.Browser, SqlDbType = SqlDbType.VarChar,Size=50  ,IsNullable=true}, 
                                        new SqlParameter() { ParameterName = "@BrowserVersion", Value = transactionlog.BrowserVersion, SqlDbType = SqlDbType.VarChar,Size=20  ,IsNullable=true}, 
                                        new SqlParameter() { ParameterName = "@Lob", Value = transactionlog.Lob, SqlDbType = SqlDbType.VarChar,Size=2  ,IsNullable=true}, 
                                        new SqlParameter() { ParameterName = "@AdId", Value = transactionlog.AdId, SqlDbType = SqlDbType.BigInt,IsNullable=true}, 

                                        new SqlParameter() { ParameterName = "@transactionlogId",Value=0, SqlDbType = SqlDbType.Int,Direction= ParameterDirection.InputOutput } 
                                       
                };
            IBHICDBBase bhicDBBase = GetDbConnector();

            try
            {
                bhicDBBase.OpenConnection();
                bhicDBBase.BeginTransaction();
                //execute stored procedure
                bhicDBBase.ExecuteNonQuery("AddTransactionLog", QueryCommandType.StoredProcedure, dbParams);
                transactionlogId = Convert.ToInt64(dbParams[(dbParams.Count - 1)].Value);

                // Add Database transaction detail
                if (transactionlog.Dbtransactions.Any())
	            {
		            foreach (var item in transactionlog.Dbtransactions)
	                {
                        
		                    dbParams = new List<IDbDataParameter>
                            {
                                new SqlParameter() { ParameterName = "@TransactionLogId", Value = transactionlogId, SqlDbType = SqlDbType.BigInt } ,
                                new SqlParameter() { ParameterName = "@DbCallRequestTime", Value =item.DbCallRequestTime, SqlDbType = SqlDbType.DateTime } ,
                                new SqlParameter() { ParameterName = "@DbCallResponseTime", Value =item.DbCallResponseTime, SqlDbType = SqlDbType.DateTime,IsNullable=true } ,
                                new SqlParameter() { ParameterName = "@DbRequestProcessTime", Value = item.DbRequestProcessTime, SqlDbType = SqlDbType.BigInt ,IsNullable=true},
                                new SqlParameter() { ParameterName = "@DbProcName", Value = item.DbProcName, SqlDbType = SqlDbType.VarChar ,Size=100,IsNullable=true}

                                       
                            };
                            bhicDBBase.ExecuteNonQuery("AddDbTransaction", QueryCommandType.StoredProcedure, dbParams);
	                }
	            }

                // Add API transaction detail
                if (transactionlog.ApiTransactions.Any())
	            {
		            foreach (var item in transactionlog.ApiTransactions)
	                {
		                    dbParams = new List<IDbDataParameter>
                            {
                                new SqlParameter() { ParameterName = "@TransactionLogId", Value = transactionlogId, SqlDbType = SqlDbType.BigInt } ,
                                new SqlParameter() { ParameterName = "@ApiCallType", Value =item.ApiCallType, SqlDbType = SqlDbType.VarChar,Size=20 } ,
                                new SqlParameter() { ParameterName = "@ApiName", Value =item.ApiName, SqlDbType = SqlDbType.VarChar,Size=100 } ,
                                new SqlParameter() { ParameterName = "@ApiCallRequestTime", Value =item.ApiCallRequestTime, SqlDbType = SqlDbType.DateTime,IsNullable=true } ,
                                new SqlParameter() { ParameterName = "@ApiCallResponseTime", Value =item.ApiCallResponseTime, SqlDbType = SqlDbType.DateTime,IsNullable=true } ,
                                new SqlParameter() { ParameterName = "@ApiRequestProcessTime", Value = item.ApiRequestProcessTime, SqlDbType = SqlDbType.BigInt ,IsNullable=true},
                                new SqlParameter() { ParameterName = "@ApiRequestSize", Value = item.ApiRequestSize, SqlDbType = SqlDbType.BigInt ,IsNullable=true},
                                new SqlParameter() { ParameterName = "@ApiResponseSize", Value = item.ApiResponseSize, SqlDbType = SqlDbType.BigInt ,IsNullable=true}
                                       
                            };
                            bhicDBBase.ExecuteNonQuery("AddApiTransaction", QueryCommandType.StoredProcedure, dbParams);
	                }
	            }

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

            return transactionlogId;
        }
        /*Use to insert data related to Transacttion Logging*/
        #endregion
    }
}
