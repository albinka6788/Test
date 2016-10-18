#region Use Directive

using BHIC.Common.DataAccess;
using BHIC.Contract.PurchasePath;
using BHIC.DML.WC.DataService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace BHIC.Core.PurchasePath
{
    public class LossControlFileService : BaseDataProvider, ILossControlFileService
    {
       /// <summary>
       /// To Fetch Loss Contol File Name from Guid
       /// </summary>
       /// <param name="Guid"></param>
       /// <returns></returns>
        public string GetLossControlFileName(string Guid)
        {
            var FileName = GetDbConnector().ExecuteScalar("GetLossControlFileName", QueryCommandType.StoredProcedure,
                                   new List<System.Data.IDbDataParameter> 
                                { 
                                    new SqlParameter() { ParameterName = "@Guid", Value = Guid, SqlDbType = SqlDbType.VarChar, Size = 50 } 
                                });

            //Comment : Here convert retuned object
            return (FileName != DBNull.Value && FileName != null ? FileName.ToString() : "");
        }
    }
}
