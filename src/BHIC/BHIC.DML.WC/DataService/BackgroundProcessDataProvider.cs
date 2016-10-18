using BHIC.Common.DataAccess;
using BHIC.DML.WC.DataContract;
using BHIC.DML.WC.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace BHIC.DML.WC.DataService
{
    /// <summary>
    /// This provider is for all the background process database operations
    /// </summary>
    public class BackgroundProcessDataProvider : IBackgroundProcessDataProvider
    {
        /// <summary>
        /// To get the quotes for the inactive user, who is closed the browser before performing payment transaction.
        /// This user will get an email, to reterive the quote.
        /// </summary>
        /// <returns></returns>
        List<UserQuoteDTO> IBackgroundProcessDataProvider.GetInactiveUserQuote()
        {
            List<UserQuoteDTO> quotes = new List<UserQuoteDTO>();
            //execute stored procedure
            var rdr = GetDbConnector().ExecuteReader("GetInActiveUsersAndQuote", QueryCommandType.StoredProcedure);

            while (rdr.Read())
            {
                UserQuoteDTO quote = new UserQuoteDTO();
                quote.OrganizationUserDetailID = Convert.ToInt32(rdr["OrganizationUserDetailID"]);
                quote.QuoteNumber = Convert.ToString(rdr["QuoteNumber"]);
                quote.EmailID = Convert.ToString(rdr["EmailID"]);
                quotes.Add(quote);
            }

            return quotes;
        }
        /// <summary>
        /// Once the Save for later mail sent, this method will update the status to sent.
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>

        bool IBackgroundProcessDataProvider.UpdateSaveForLaterMailStatus(int UserId)
        {
            try
            {
                //execute stored procedure
                var returnParameter = new SqlParameter() { ParameterName = "@returnId", SqlDbType = SqlDbType.BigInt, Direction = ParameterDirection.Output };

                var rowEffeted = GetDbConnector().ExecuteNonQuery("UpdateSaveForLaterMailStatus", QueryCommandType.StoredProcedure,
                                new List<System.Data.IDbDataParameter> 
                                    { 
                                        new SqlParameter() { ParameterName = "@OrganizationUserDetailID", Value =UserId, SqlDbType = SqlDbType.Int } ,
                                        returnParameter
                                    });

                if (rowEffeted > 0)
                {
                    return true;
                }
            }
            catch(Exception ex)
            {
                BHIC.Common.Logging.LoggingService.Instance.Trace(ex);
                //Comment : Here in case of DB updation failure log it into XML for future re-trying by schedular service
            }

            return false;
        }

        /// <summary>
        /// Retuen generic DbConnector object to communicate with database
        /// </summary>
        /// <returns></returns>
        private BHICDBBase GetDbConnector()
        {
            BHICDBBase dbConnector = new BHICDBBase();

            #region Comment : Here using XmlReader get DB connection string

            dbConnector.DBName = "GuinnessDB";

            #endregion

            //dbConnector.DBConnectionString = DbConnectionString;
            //dbConnector.CreateDBObjects(Providers.SqlServer);
            //dbConnector.CreateConnection();

            return dbConnector;
        }
    }

}
