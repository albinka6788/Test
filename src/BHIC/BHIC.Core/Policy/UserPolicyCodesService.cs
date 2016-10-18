#region Using directives

using BHIC.Common;
using BHIC.Common.Client;
using BHIC.Common.Config;
using BHIC.Contract.Policy;
using BHIC.Contract.Provider;
using BHIC.Domain.Policy;
using BHIC.Domain.Service;
using System;

#endregion

namespace BHIC.Core.Policy
{
    public class UserPolicyCodesService : IServiceProviders, IUserPolicyCodesService
    {
        public UserPolicyCodesService(ServiceProvider provider)
        {
            base.ServiceProvider = provider;
        }

        #region Methods

        #region Interface Implementation

        /// <summary>
        /// Save User Policy Code
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        OperationStatus IUserPolicyCodesService.AddUserPolicyCode(UserPolicyCode args)
        {
            var userPolicyCodeResponse = SvcClient.CallService<UserPolicyCode, OperationStatus>(Constants.UserPolicyCodes, "POST", args, ServiceProvider);

            //In case of failure only log the exception, don't throw any exception
            return userPolicyCodeResponse;
        }

        public OperationStatus DeleteUserPolicyCode(UserPolicyCode args)
        {
            var userPolicyCodeResponse = SvcClient.CallService<UserPolicyCode, OperationStatus>(Constants.UserPolicyCodes, "DELETE", args, ServiceProvider);

            //In case of failure only log the exception, don't throw any exception
            return userPolicyCodeResponse;
        }     

        public UserPolicyCodeResponse GetUserPolicyCodes(UserPolicyCodeRequestParms args)
        {
            var userPolicyCodeResponse = SvcClient.CallService<UserPolicyCodeResponse>(string.Concat(Constants.UserPolicyCodes,
                UtilityFunctions.CreateQueryString<UserPolicyCodeRequestParms>(args)), ServiceProvider);

            //In case of failure only log the exception, don't throw any exception
            return userPolicyCodeResponse;
        }
        #endregion

        #endregion     
    }
}
