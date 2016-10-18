using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Background;
using BHIC.Domain.Dashboard;
using System.Security;

namespace BHIC.Contract.Background
{
    public interface IAccountRegistrationService
    {
        /// <summary>
        /// Returns true otherwise returns false if fail
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        UserRegistration GetCredentials(UserRegistration user);


        /// <summary>
        /// Returns true otherwise returns false if fail
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        string CreateAccount(AccountRegistration user);

        bool ChangePassword(string email, SecureString oldpassword, SecureString newpassword);

        bool ValidUser(string email);

        bool IsUserEmailExists(string email);

        bool ResetPassword(string email, SecureString password);

        //bool UpadateContactInfo(ContactInformation info);

        bool EmailVerification(string email);

        UserRegistration GetUserDetails(UserRegistration user);

        bool ForgotPwdRequestedDateTime(string type, string email, string inputmmddyyyyhhmmss);

        /// <summary>
        /// Link user policy with specified user email id
        /// </summary>
        /// <param name="userEmailID"></param>
        /// <param name="policyNumber"></param>
        /// <returns></returns>
        string LinkPolicyWithUser(string userEmailID, string policyNumber);

    }
}
