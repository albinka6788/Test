#region Using directives

using BHIC.Common.DataAccess;
using BHIC.DML.WC.DataContract;
using BHIC.DML.WC.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace BHIC.DML.WC.DataService
{
    public class LineOfBusinessProvider : BaseDataProvider, ILineOfBusinessProvider
    {
        #region Methods

        #region Interface Implementation

        /// <summary>
        /// Fetch list fo available line of business from database
        /// </summary>
        /// <returns>returns list of all lob</returns>
        List<LineOfBusiness> ILineOfBusinessProvider.GetAllLineOfBusiness()
        {
            List<LineOfBusiness> lob = new List<LineOfBusiness>();

            //execute stored procedure
            var rdr = GetDbConnector().ExecuteReader("GetLineOfBusiness", QueryCommandType.StoredProcedure);

            while (rdr.Read())
            {
                lob.Add(
                    new LineOfBusiness
                    {
                        Id = Convert.ToInt32(rdr["LobId"]),
                        Abbreviation = Convert.ToString(rdr["Abbreviation"]),
                        StateCode = Convert.ToString(rdr["StateCode"]),
                        StateLineOfBusinessId = Convert.ToInt32(rdr["Id"]),
                        LobFullName = Convert.ToString(rdr["LobFullName"])
                    });
            }

            return GetAllLineOfBusinessStatus(lob);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// It will fetch status for all existing lob
        /// </summary>
        /// <param name="lineOfBusiness"></param>
        /// <returns></returns>
        private List<LineOfBusiness> GetAllLineOfBusinessStatus(List<LineOfBusiness> lineOfBusiness)
        {
            List<LineOfBusiness> lob = new List<LineOfBusiness>();

            //execute stored procedure
            var rdr = GetDbConnector().ExecuteReader("GetlobStatusForState", QueryCommandType.StoredProcedure);

            while (rdr.Read())
            {
                lob.Add(new LineOfBusiness { StateLineOfBusinessId = Convert.ToInt32(rdr["StateLobId"]), Status = Convert.ToString(rdr["Status"]) });
            }

            //fetch status from list and append to current lineOfBusiness
            lineOfBusiness.ForEach(item =>
            {
                item.Status = (lob.Where(x => x.StateLineOfBusinessId.Equals(item.StateLineOfBusinessId)).FirstOrDefault() == null) ? string.Empty
                    : lob.Where(x => x.StateLineOfBusinessId.Equals(item.StateLineOfBusinessId)).FirstOrDefault().Status;
            });

            return lineOfBusiness;
        }

        #endregion

        #endregion
    }
}
