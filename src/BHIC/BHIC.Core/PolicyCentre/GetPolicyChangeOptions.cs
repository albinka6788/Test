using BHIC.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHIC.Contract.PolicyCentre;
using BHIC.Domain.PolicyCentre;

namespace BHIC.Core.PolicyCentre
{
    public class GetPolicyChangeOptions : IGetPolicyChangeOptions
    {
        #region Public function
        public List<DropDownOptions> GetLineOfBusinessPolicyChangeOptions(int lineOfBusinessID)
        {
            try
            {
                BHICDBBase dbConnector = new BHICDBBase();
                //var dataSet = dbConnector.LoadDataSet("GetPolicyChangeOptions", QueryCommandType.StoredProcedure,
                //   new List<System.Data.IDbDataParameter> { new SqlParameter() { ParameterName = "@LineOfBusinessID", Value = lineOfBusinessID, SqlDbType = SqlDbType.Int } });
                var dataSet = dbConnector.LoadDataSet("GetOptions", QueryCommandType.StoredProcedure,
                  new List<System.Data.IDbDataParameter> { new SqlParameter() { ParameterName = "@TableName", Value = "POLICYCHANGEOPTIONS", SqlDbType = SqlDbType.Int },
                   new SqlParameter() { ParameterName = "@LineOfBusinessID", Value = lineOfBusinessID, SqlDbType = SqlDbType.Int }});

                return ConvertDatatableToDictionary(dataSet.Tables[0]);
            }
            catch
            {
                throw;
            }
        }
        #endregion

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

        #endregion
    }
}
