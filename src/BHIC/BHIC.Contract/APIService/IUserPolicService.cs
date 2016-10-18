using BHIC.DML.WC.DTO;
using BHIC.Domain.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Contract.APIService
{
    public interface IUserPolicyService
    {
        int GetUserDetailId(string UserName);
        //To check if the policy is exists. It can be checked either with policy number or quote number
        Boolean IsPolicyExists(int Option, string Value);
        Boolean IsValidUserCredentials(string UserName,SecureString Password);
        Boolean CreateUserPolicy(UserPolicyDTO UserPolicy,bool ExistingUser, out Int64 PolicyId);
        Boolean SaveQuote(QuoteDTO quote);
        int GetOrganizationAddressId(int organizationId);
        Boolean IsQuoteIdExists(string QuoteNumber, bool validatePolicy, int UserDetailId);
        Boolean IsValidAPICredentials(string UserName, SecureString Password);
        void SendWelcomeMailNotification(UserPolicyDTO UserPolicy, bool ExistingUser);
        bool SendSaveForLaterMailNotification(SaveQuoteRequestDTO Quote);
        int DeleteAccount(string Email);
        int MergeAccounts(string OldEmailID, string NewEmailID);
        SecondaryAccountRegistration CreateAccount(SecondaryAccountRegistration accountRegistration);
        string GeneratePassword(int length, int complexity);
    }
}
