using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Contract.Account;
using BHIC.Domain.Account;
using BHIC.Common;
using BHIC.Domain.Service;
using BHIC.Common.Client;
using BHIC.Common.Config;

namespace BHIC.Core.Account
{
    public class UserProfileService : IUserProfileService
    {
        #region Methods

        #region Public Methods

        /// <summary>
        /// Return list of UserProfiles based on Email
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public List<UserProfile> GetUserProfileList(UserProfileRequestParms args, ref OAuthModel oauthRequest)
        {
            var userProfileResponse = SvcClientOld.CallService<UserProfileResponse>(string.Concat(Constants.UserProfiles,
                UtilityFunctions.CreateQueryString<UserProfileRequestParms>(args)), ref oauthRequest);

            if (userProfileResponse.OperationStatus.RequestSuccessful)
            {
                return userProfileResponse.UserProfiles;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(userProfileResponse.OperationStatus.Messages));
            }
        }

        /// <summary>
        /// Save new UserProfile details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public OperationStatus AddUserProfile(UserProfile args)
        {
            var operationResponse = SvcClientOld.CallService<UserProfile, OperationStatus>(Constants.UserProfiles, "POST", args);

            if (operationResponse.RequestSuccessful)
            {
                return operationResponse;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(operationResponse.Messages));
            }
        }

        #endregion

        #endregion
    }
}
