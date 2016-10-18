#region Using directives

using BHIC.Common.DataAccess;
using BHIC.DML.WC.DataContract;
using BHIC.DML.WC.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

#endregion

namespace BHIC.DML.WC.DataService
{
    public class OraganisationDataProvider : IOraganisationDataProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="organisation"></param>
        /// <returns></returns>
        bool IOraganisationDataProvider.AddOraganisationDetails(OraganisationDTO organisation)
        {
            try
            {
                //execute stored procedure
                var rowEffeted = GetDbConnector().ExecuteNonQuery("CreateOraganisationAddress", QueryCommandType.StoredProcedure,
                                new List<System.Data.IDbDataParameter> 
                                    { 
                                        new SqlParameter() { ParameterName = "@OrganizationID", Value = organisation.OrganizationID, SqlDbType = SqlDbType.Int } ,
                                        new SqlParameter() { ParameterName = "@Address1", Value = organisation.Address1, SqlDbType = SqlDbType.VarChar,Size=200 } ,
                                        new SqlParameter() { ParameterName = "@Address2", Value = organisation.Address2, SqlDbType = SqlDbType.VarChar,Size=200 } ,
                                        new SqlParameter() { ParameterName = "@Address3", Value = organisation.Address3, SqlDbType = SqlDbType.VarChar,Size=200 } ,
                                        new SqlParameter() { ParameterName = "@City", Value = organisation.City, SqlDbType = SqlDbType.VarChar,Size=200 } ,
                                        new SqlParameter() { ParameterName = "@County", Value = organisation.County, SqlDbType = SqlDbType.VarChar,Size=200 } ,
                                        new SqlParameter() { ParameterName = "@StateCode", Value = organisation.StateCode, SqlDbType = SqlDbType.Char,Size=2 } ,
                                        new SqlParameter() { ParameterName = "@ZipCode", Value = organisation.ZipCode, SqlDbType = SqlDbType.Int } ,
                                        new SqlParameter() { ParameterName = "@CountryID", Value = organisation.CountryID, SqlDbType = SqlDbType.Int } ,
                                        new SqlParameter() { ParameterName = "@IsCorporateAddress", Value = organisation.IsCorporateAddress, SqlDbType = SqlDbType.Bit } ,
                                        new SqlParameter() { ParameterName = "@ContactName", Value = organisation.ContactName, SqlDbType = SqlDbType.VarChar,Size=200 } ,
                                        new SqlParameter() { ParameterName = "@ContactNumber1", Value = organisation.ContactNumber1, SqlDbType = SqlDbType.Int } ,
                                        new SqlParameter() { ParameterName = "@ContactNumber2", Value = organisation.ContactNumber2, SqlDbType = SqlDbType.Int } ,
                                        new SqlParameter() { ParameterName = "@Fax", Value = organisation.Fax, SqlDbType = SqlDbType.Int } ,
                                        new SqlParameter() { ParameterName = "@IsActive", Value = organisation.IsActive, SqlDbType = SqlDbType.Bit } ,
                                        new SqlParameter() { ParameterName = "@CreatedDate", Value = organisation.CreatedDate, SqlDbType = SqlDbType.DateTime } ,
                                        new SqlParameter() { ParameterName = "@CreatedBy", Value = organisation.CreatedBy, SqlDbType = SqlDbType.Int } ,
                                        new SqlParameter() { ParameterName = "@ModifiedDate", Value = organisation.ModifiedDate, SqlDbType = SqlDbType.DateTime } ,
                                        new SqlParameter() { ParameterName = "@ModifiedBy", Value = organisation.ModifiedBy, SqlDbType = SqlDbType.Int } 
                                       
                                    });

                if (rowEffeted > 0)
                {
                    return true;
                }
            }
            catch
            {
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
