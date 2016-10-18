using BHIC.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BHIC.Common.LobStatusService
{
    public class LobDataProvider
    {
        public static string WC = "Workers' Compensation";
        public static string BOP = "Business Owner's Policy";
        public static string CA = "CommercialAuto";
        List<LobStatusDTO> lobStatusLst = new List<LobStatusDTO>();

        public List<LobStatusDTO> PrepareLobStatusData(List<LobStatus> lobResult)
        {
            foreach (var item in lobResult)
            {
                ///Write data for wc
                lobStatusLst.Add(new LobStatusDTO
                {
                    StateCode = item.Abbreviation,
                    LineOfBusinessName = WC,
                    Status = item.WC,
                    EffectiveFrom = DateTime.Now,
                    ExpiryOn = DateTime.Now.AddDays(5),
                    CreatedDate = DateTime.Now,
                    CreatedBy = 1,
                    ModifiedDate = DateTime.Now,
                    ModifiedBy = 1,
                    IsActive = true
                });

                ///Write data for bop
                lobStatusLst.Add(new LobStatusDTO
                {
                    StateCode = item.Abbreviation,
                    LineOfBusinessName = BOP,
                    Status = item.BOP,
                    EffectiveFrom = DateTime.Now,
                    ExpiryOn = DateTime.Now.AddDays(5),
                    CreatedDate = DateTime.Now,
                    CreatedBy = 1,
                    ModifiedDate = DateTime.Now,
                    ModifiedBy = 1,
                    IsActive = true
                });

                ///Write data for ca
                lobStatusLst.Add(new LobStatusDTO
                {
                    StateCode = item.Abbreviation,
                    LineOfBusinessName = CA,
                    Status = item.CA,
                    EffectiveFrom = DateTime.Now,
                    ExpiryOn = DateTime.Now.AddDays(5),
                    CreatedDate = DateTime.Now,
                    CreatedBy = 1,
                    ModifiedDate = DateTime.Now,
                    ModifiedBy = 1,
                    IsActive = true
                });
            }

            return lobStatusLst;
        }

        /// <summary>
        /// It will write lob status  into StateLobStatus table
        /// </summary>
        /// <param name="lobObj"></param>
        /// <returns></returns>
        public bool WriteDataIntoTable(List<LobStatusDTO> lobStatusLst)
        {
            int count = 1;
            foreach (var item in lobStatusLst)
            {
                Console.WriteLine("*****************************************************************************");
                Console.WriteLine("Writing data for :");
                Console.WriteLine(string.Format("Index:{3},StateCode :{0},Lob:{1},Status:{2}", item.StateCode, item.LineOfBusinessName, item.Status, count++));

                try
                {
                    //Comment : Here add return parameter 
                    var returnParameter = new SqlParameter() { ParameterName = "@ReturnInsertedRowId", SqlDbType = SqlDbType.BigInt, Direction = ParameterDirection.ReturnValue };

                    //execute stored procedure
                    var rowEffeted = GetDbConnector().ExecuteNonQuery("InsertOrUpdateLobStatus", QueryCommandType.StoredProcedure,
                                    new List<System.Data.IDbDataParameter> 
                                    {
		                                 new SqlParameter() { ParameterName = "@StateCode",Value =  item.StateCode,SqlDbType = SqlDbType.VarChar,Size=2 }
		                                ,new SqlParameter() { ParameterName = "@LineOfBusinessName",Value =  item.LineOfBusinessName,SqlDbType = SqlDbType.VarChar,Size=50 }
		                                ,new SqlParameter() { ParameterName = "@Status",Value =  item.Status,SqlDbType = SqlDbType.VarChar,Size=50 }
		                                ,new SqlParameter() { ParameterName = "@EffectiveFrom",Value =  item.EffectiveFrom,SqlDbType = SqlDbType.DateTime }
		                                ,new SqlParameter() { ParameterName = "@ExpiryOn",Value =  item.ExpiryOn,SqlDbType = SqlDbType.DateTime }
		                                ,new SqlParameter() { ParameterName = "@IsActive",Value =  item.IsActive,SqlDbType = SqlDbType.Bit }
		                                ,new SqlParameter() { ParameterName = "@CreatedDate",Value =  item.CreatedDate,SqlDbType = SqlDbType.DateTime }
		                                ,new SqlParameter() { ParameterName = "@CreatedBy",Value =  item.CreatedBy,SqlDbType = SqlDbType.Int }
		                                ,new SqlParameter() { ParameterName = "@ModifiedDate",Value =  item.ModifiedDate,SqlDbType = SqlDbType.DateTime }
		                                ,new SqlParameter() { ParameterName = "@ModifiedBy",Value =  item.ModifiedBy,SqlDbType = SqlDbType.Int }
                                    });

                    if (rowEffeted > 0)
                    {
                        if (returnParameter.Value != null)
                        {
                            Console.WriteLine(string.Format("Added Successfully :{0}", Convert.ToInt64(returnParameter.Value)));
                        }

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("Error:{0}", ex.Message));
                }

                Thread.Sleep(500);
                Console.WriteLine("*****************************************************************************");
            }
            return true;
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
