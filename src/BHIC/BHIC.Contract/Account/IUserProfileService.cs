using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Account;
using BHIC.Domain.Service;
using BHIC.Common.Client;

namespace BHIC.Contract.Account
{
    public interface IUserProfileService
    {
        /// <summary>
        /// Return list of UserProfiles based on Email
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        List<UserProfile> GetUserProfileList(UserProfileRequestParms args, ref OAuthModel oauthRequest);

        /// <summary>
        /// Save new UserProfile details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        OperationStatus AddUserProfile(UserProfile args);
    }
}
