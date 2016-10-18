using BHIC.Common.DataAccess;
using BHIC.Contract.Dashboard;
using BHIC.Domain.Dashboard;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Core.Dashboard
{
    public class CertificateOfInsuranceService : ICertificateOfInsuranceService
    {
        public bool CreateCertificateofInsurance(CertificateOfInsuranceDTO certificate)
        {

            try
            {
                //execute stored procedure
                using (IBHICDBBase dbConnector = new BHICDBBase())
                {
                    int rowsAffected = dbConnector.ExecuteNonQuery("CreateCertificateOfInsurance", QueryCommandType.StoredProcedure,
                                    new List<System.Data.IDbDataParameter> 
                                    { 
                                        new SqlParameter() { ParameterName = "@PolicyNumber", Value = certificate.PolicyId, SqlDbType = SqlDbType.VarChar },
                                        new SqlParameter() { ParameterName = "@CertificateRequestId", Value = certificate.CertificateRequestId, SqlDbType = SqlDbType.VarChar },
                                        new SqlParameter() { ParameterName = "@EmailId", Value = certificate.EmailId, SqlDbType = SqlDbType.VarChar }                                        
                                    });

                    if (rowsAffected > 0)
                    {
                        return true;
                    }
                }
            }
            catch
            {
                //Comment : Here in case of DB updation failure log it into XML for future re-trying by schedular service
                throw;
            }

            return false;
        }

        public List<string> GetCertificateofInsurance(string policyNumber)
        {
            List<string> certificateId = new List<string>();
            try
            {
                //execute stored procedure
                using (IBHICDBBase dbConnector = new BHICDBBase())
                {
                    var dataSet = dbConnector.LoadDataSet("GetCertificateOfInsurance", QueryCommandType.StoredProcedure,
                                    new List<System.Data.IDbDataParameter> 
                                    { 
                                        new SqlParameter() { ParameterName = "@PolicyNumber", Value = policyNumber, SqlDbType = SqlDbType.VarChar }                                                     
                                    });

                    if (dataSet.Tables[0].Rows.Count > 0)
                    {
                        // On all tables' rows
                        foreach (DataRow dtRow in dataSet.Tables[0].Rows)
                        {
                            certificateId.Add(dtRow["CertificateRequestId"].ToString());
                        }
                    }
                }
            }
            catch
            {
                //Comment : Here in case of DB updation failure log it into XML for future re-trying by schedular service
                throw;
            }

            return certificateId;
        }
    }
}
