#region Using directives

using BHIC.DML.WC.DTO;
using BHIC.Common.DataAccess;
using System;
using System.Data;

#endregion

namespace BHIC.DML.WC.DataContract
{
    public interface IOrganisationUserDetailDataProvider
    {
        /// <summary>
        /// Add user new account details
        /// </summary>
        /// <param name="organisation"></param>
        /// <param name="organisationUserId"></param>
        /// <returns></returns>
        bool AddOrganisationUserDetail(OrganisationUserDetailDTO organisation, out Int64? organisationUserId);

        /// <summary>
        /// Method to create as well as update existing organization user details
        /// </summary>
        /// <param name="organisation"></param>
        /// <param name="organisationUserId"></param>
        /// <returns></returns>
        bool MaintainUserAccountDetail(OrganisationUserDetailDTO organisation, out Int64? organisationUserId);

        /// <summary>
        /// Get user credential details based on email-id
        /// </summary>
        /// <param name="organisationUser"></param>
        /// <returns></returns>
        OrganisationUserDetailDTO GetUserCredentialDetails(OrganisationUserDetailDTO organisationUser);

        /// <summary>
        /// Activate/Deactivate exsiting organization user
        /// </summary>
        /// <param name="organisation"></param>
        /// <returns></returns>
        bool ActivateDeactivateOrganisationUser(OrganisationUserDetailDTO organisation, IBHICDBBase bhicDBBase = null);
    }
}
