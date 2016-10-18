#region Using directives

using BHIC.Common.DataAccess;
using BHIC.DML.WC.DataContract;
using BHIC.DML.WC.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

#endregion

namespace BHIC.DML.WC.DataService
{
    public class MultipleStateDataProvider : BaseDataProvider, IMultipleStateDataProvider
    {
        #region Methods

        #region Interface Implementation

        List<ZipCodeStates> IMultipleStateDataProvider.GetStatesByZipCode()
        {
            var zipStates = new List<ZipCodeStates>();

            //execute stored procedure
            var rdr = GetDbConnector().ExecuteReader("GetZipCodeStateDetails", QueryCommandType.StoredProcedure);

            while (rdr.Read())
            {
                zipStates.Add(new ZipCodeStates { Id = Convert.ToInt32(rdr["Id"]), ZipCode = Convert.ToString(rdr["ZipCode"]), StateId = Convert.ToInt32(rdr["StateId"]), StateCode = Convert.ToString(rdr["StateCode"]) });

            }

            return zipStates;
        }

        #endregion

        #endregion
    }
}
