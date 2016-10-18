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
    public class OrganisationAddressService : BaseDataProvider,IOrganisationAddress
    {
        /// <summary>
        /// Add organization address details 
        /// </summary>
        /// <param name="organizationAddress"></param>
        /// <returns></returns>
        public bool AddOrganisationAddressDetail(OrganisationAddress organizationAddress, out Int64? organisationAddressId)
        {
            try
            {
                //Comment : Here default set value 
                organisationAddressId = null;

                //Comment : Here add return parameter 
                var returnParameter = new SqlParameter() { ParameterName = "@ReturnInsertedRowId", SqlDbType = SqlDbType.BigInt, Direction = ParameterDirection.ReturnValue };

                //execute stored procedure
                var rowEffeted = GetDbConnector().ExecuteNonQuery("CreateOraganisationAddress", QueryCommandType.StoredProcedure,
                                new List<System.Data.IDbDataParameter> 
                                    {
                                         new SqlParameter() { ParameterName = "@OrganizationID",Value =  organizationAddress.OrganizationID,SqlDbType = SqlDbType.Int }
    	                                ,new SqlParameter() { ParameterName = "@Address1",Value =  organizationAddress.Address1,SqlDbType = SqlDbType.VarChar, Size = 200 }
	 	                                ,new SqlParameter() { ParameterName = "@Address2",Value =  organizationAddress.Address2,SqlDbType = SqlDbType.VarChar, Size = 200 }
		                                ,new SqlParameter() { ParameterName = "@Address3",Value =  organizationAddress.Address3,SqlDbType = SqlDbType.VarChar, Size = 200 }
		                                ,new SqlParameter() { ParameterName = "@City",Value =  organizationAddress.City,SqlDbType = SqlDbType.VarChar, Size = 200 }
		                                ,new SqlParameter() { ParameterName = "@County",Value =  organizationAddress.County,SqlDbType = SqlDbType.VarChar, Size = 200 }
    	                                ,new SqlParameter() { ParameterName = "@StateCode",Value =  organizationAddress.StateCode,SqlDbType = SqlDbType.VarChar, Size = 200 }
		                                ,new SqlParameter() { ParameterName = "@ZipCode",Value =  organizationAddress.ZipCode,SqlDbType = SqlDbType.Int }
		                                ,new SqlParameter() { ParameterName = "@CountryID",Value =  organizationAddress.CountryID,SqlDbType = SqlDbType.Int}
		                                ,new SqlParameter() { ParameterName = "@IsCorporateAddress",Value =  organizationAddress.IsCorporateAddress,SqlDbType = SqlDbType.Bit}
		                                ,new SqlParameter() { ParameterName = "@ContactName",Value =  organizationAddress.ContactName,SqlDbType = SqlDbType.VarChar, Size = 20 }
		                                ,new SqlParameter() { ParameterName = "@ContactNumber1",Value =  organizationAddress.ContactNumber1,SqlDbType = SqlDbType.BigInt }
		                                ,new SqlParameter() { ParameterName = "@ContactNumber2",Value =  organizationAddress.ContactNumber2,SqlDbType = SqlDbType.BigInt }
		                                ,new SqlParameter() { ParameterName = "@Fax",Value =  organizationAddress.Fax,SqlDbType = SqlDbType.BigInt }    
		                                ,new SqlParameter() { ParameterName = "@IsActive",Value =  organizationAddress.IsActive,SqlDbType = SqlDbType.Bit }
		                                ,new SqlParameter() { ParameterName = "@CreatedDate",Value =  organizationAddress.CreatedDate,SqlDbType = SqlDbType.DateTime }
		                                ,new SqlParameter() { ParameterName = "@CreatedBy",Value =  organizationAddress.CreatedBy,SqlDbType = SqlDbType.Int }
		                                ,new SqlParameter() { ParameterName = "@ModifiedDate",Value =  organizationAddress.ModifiedDate,SqlDbType = SqlDbType.DateTime }
		                                ,new SqlParameter() { ParameterName = "@ModifiedBy",Value =  organizationAddress.ModifiedBy,SqlDbType = SqlDbType.Int }
                                        ,returnParameter
                                    });

                if (rowEffeted > 0)
                {
                    if (returnParameter.Value != null)
                    {
                        organisationAddressId = Convert.ToInt64(returnParameter.Value);
                    }

                    return true;
                }
            }
            catch
            {
                //Comment : Here in case of DB updation failure log it into XML for future re-trying by schedular service
                throw;
            }

            return false;
        }

        /// <summary>
        /// Method to create as well as update existing organisation address details
        /// </summary>
        /// <param name="organizationAddress"></param>
        /// <returns></returns>
        public bool MaintainOrganisationAddressDetail(OrganisationAddress organizationAddress, out Int64? organisationAddressId)
        {
            try
            {
                //Comment : Here default set value 
                organisationAddressId = null;

                //Comment : Here add return parameter 
                var returnParameter = new SqlParameter() { ParameterName = "@ReturnRowId", SqlDbType = SqlDbType.BigInt, Direction = ParameterDirection.ReturnValue };

                //execute stored procedure
                var rowEffeted = GetDbConnector().ExecuteNonQuery("MaintainOraganisationAddress", QueryCommandType.StoredProcedure,
                                new List<System.Data.IDbDataParameter> 
                                    {
                                         new SqlParameter() { ParameterName = "@OrganizationID",Value =  organizationAddress.OrganizationID,SqlDbType = SqlDbType.Int }
    	                                ,new SqlParameter() { ParameterName = "@Address1",Value =  organizationAddress.Address1,SqlDbType = SqlDbType.VarChar, Size = 200 }
	 	                                ,new SqlParameter() { ParameterName = "@Address2",Value =  organizationAddress.Address2,SqlDbType = SqlDbType.VarChar, Size = 200 }
		                                ,new SqlParameter() { ParameterName = "@Address3",Value =  organizationAddress.Address3,SqlDbType = SqlDbType.VarChar, Size = 200 }
		                                ,new SqlParameter() { ParameterName = "@City",Value =  organizationAddress.City,SqlDbType = SqlDbType.VarChar, Size = 200 }
		                                ,new SqlParameter() { ParameterName = "@County",Value =  organizationAddress.County,SqlDbType = SqlDbType.VarChar, Size = 200 }
    	                                ,new SqlParameter() { ParameterName = "@StateCode",Value =  organizationAddress.StateCode,SqlDbType = SqlDbType.VarChar, Size = 200 }
		                                ,new SqlParameter() { ParameterName = "@ZipCode",Value =  organizationAddress.ZipCode,SqlDbType = SqlDbType.Int }
		                                ,new SqlParameter() { ParameterName = "@CountryID",Value =  organizationAddress.CountryID,SqlDbType = SqlDbType.Int}
		                                ,new SqlParameter() { ParameterName = "@IsCorporateAddress",Value =  organizationAddress.IsCorporateAddress,SqlDbType = SqlDbType.Bit}
		                                ,new SqlParameter() { ParameterName = "@ContactName",Value =  organizationAddress.ContactName,SqlDbType = SqlDbType.VarChar, Size = 20 }
		                                ,new SqlParameter() { ParameterName = "@ContactNumber1",Value =  organizationAddress.ContactNumber1,SqlDbType = SqlDbType.BigInt }
		                                ,new SqlParameter() { ParameterName = "@ContactNumber2",Value =  organizationAddress.ContactNumber2,SqlDbType = SqlDbType.BigInt }
		                                ,new SqlParameter() { ParameterName = "@Fax",Value =  organizationAddress.Fax,SqlDbType = SqlDbType.BigInt }    
		                                ,new SqlParameter() { ParameterName = "@IsActive",Value =  organizationAddress.IsActive,SqlDbType = SqlDbType.Bit }
		                                ,new SqlParameter() { ParameterName = "@CreatedDate",Value =  organizationAddress.CreatedDate,SqlDbType = SqlDbType.DateTime }
		                                ,new SqlParameter() { ParameterName = "@CreatedBy",Value =  organizationAddress.CreatedBy,SqlDbType = SqlDbType.Int }
		                                ,new SqlParameter() { ParameterName = "@ModifiedDate",Value =  organizationAddress.ModifiedDate,SqlDbType = SqlDbType.DateTime }
		                                ,new SqlParameter() { ParameterName = "@ModifiedBy",Value =  organizationAddress.ModifiedBy,SqlDbType = SqlDbType.Int }
                                        ,returnParameter
                                    });

                if (rowEffeted > 0)
                {
                    if (returnParameter.Value != null)
                    {
                        organisationAddressId = Convert.ToInt64(returnParameter.Value);
                    }

                    return true;
                }
            }
            catch
            {
                //Comment : Here in case of DB updation failure log it into XML for future re-trying by schedular service
                throw;
            }

            return false;
        }
    }
}
