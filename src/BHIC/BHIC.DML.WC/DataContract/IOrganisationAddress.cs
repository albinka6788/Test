#region Using directives

using BHIC.DML.WC.DTO;
using System;

#endregion

namespace BHIC.DML.WC.DataContract
{
    public interface IOrganisationAddress
    {
        /// <summary>
        /// Add organization address details 
        /// </summary>
        /// <param name="organizationAddress"></param>
        /// <returns></returns>
        bool AddOrganisationAddressDetail(OrganisationAddress organizationAddress, out Int64? organisationAddressId);

        /// <summary>
        /// Method to create as well as update existing organisation address details
        /// </summary>
        /// <param name="organizationAddress"></param>
        /// <returns></returns>
        bool MaintainOrganisationAddressDetail(OrganisationAddress organizationAddress, out Int64? organisationAddressId);
    }
}
