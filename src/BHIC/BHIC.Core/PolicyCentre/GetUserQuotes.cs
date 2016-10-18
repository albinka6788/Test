using BHIC.Common.DataAccess;
using BHIC.Contract.PolicyCentre;
using BHIC.Domain.PolicyCentre;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BHIC.Common;
using BHIC.Domain.Dashboard;


namespace BHIC.Core.PolicyCentre
{
    public class GetUserQuotes : IGetUserQuotes
    {
        public DataSet UserQuotes(int userID)
        {
            DataSet dataSet = new DataSet();
            try
            {
                using (IBHICDBBase dbConnector = new BHICDBBase())
                {
                    dataSet = dbConnector.LoadDataSet("GetUserQuotes", QueryCommandType.StoredProcedure,
                    new List<System.Data.IDbDataParameter> { new SqlParameter() { ParameterName = "@UserID", Value = userID, SqlDbType = SqlDbType.Int } });
                }
            }
            catch
            {
                throw;
            }

            return dataSet;
        }

        public bool DeleteQuote(string quoteNumber, int userId)
        {
            bool retVal = false;
            try
            {
                using (IBHICDBBase dbConnector = new BHICDBBase())
                {
                    var updatedInfo = dbConnector.ExecuteNonQuery("DeleteQuote", QueryCommandType.StoredProcedure,
                        new List<System.Data.IDbDataParameter> {
                       new SqlParameter() { ParameterName = "@QuoteNumber",Value = quoteNumber, SqlDbType = SqlDbType.VarChar },
                       new SqlParameter() { ParameterName = "@IsQuoteDeleted",Value = 1, SqlDbType = SqlDbType.Bit },
                       new SqlParameter() { ParameterName = "@UserID",Value = userId, SqlDbType = SqlDbType.Int }});

                    if (updatedInfo > 0)
                    {
                        retVal = true;
                    }
                }
            }
            catch
            {
                throw;
            }

            return retVal;
        }
    }
}
