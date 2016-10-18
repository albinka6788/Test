#region Using directives

using BHIC.Domain.Policy;
using BHIC.Domain.Service;
using System;

#endregion

namespace BHIC.Contract.Policy
{
    public interface IUserPolicyCodesService
    {
        #region Methods

        #region Interface Implementation

        /// <summary>
        /// Save User Policy Code
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        OperationStatus AddUserPolicyCode(UserPolicyCode args);

        /// <summary>
        /// Delete a User Policy Code
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        OperationStatus DeleteUserPolicyCode(UserPolicyCode args);

        /// <summary>
        /// Get User Policy Quotes
        /// </summary>
        /// <param name="userPolicyCode"></param>
        /// <returns></returns>
        UserPolicyCodeResponse GetUserPolicyCodes(UserPolicyCodeRequestParms args);
        #endregion

        #endregion
    }
}
