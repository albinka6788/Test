using BHIC.Common.DataAccess;
using BHIC.DML.WC.DataContract;
using BHIC.DML.WC.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.DML.WC.DataService
{
    public class PrimaryClassCodeDataProvider : BaseClass, IPrimaryClassCodeDataProvider
    {
        /// <summary>
        /// Get Class Code data of All primary classes
        /// </summary>
        /// <returns></returns>
        public List<PrimaryClassCodeDTO> GetAllPrimaryClassCodeData()
        {
            List<PrimaryClassCodeDTO> classCodeDataList = new List<PrimaryClassCodeDTO>();

            //execute stored procedure
            var rdr = GetDbConnector().ExecuteReader("GetPrimaryClassCodeData", QueryCommandType.StoredProcedure);

            while (rdr.Read())
            {
                classCodeDataList.Add(new PrimaryClassCodeDTO
                {
                    StateCode = Convert.ToString(rdr["StateCode"]),
                    ClassDescriptionId = Convert.ToInt32(rdr["ClassDescriptionId"]),
                    MinimumPayrollThreshold = Convert.ToDecimal(rdr["MinimumPayrollThreshold"]),
                    FriendlyName = Convert.ToString(rdr["FriendlyName"])
                });

            }

            return classCodeDataList;
        }
        /// <summary>
        /// Retuen generic DbConnector object to communicate with database
        /// </summary>
        /// <returns></returns>
        private BHICDBBase GetDbConnector()
        {
            BHICDBBase dbConnector = new BHICDBBase();
            dbConnector.DBName = "GuinnessDB";
            return dbConnector;
        }
    }
}
