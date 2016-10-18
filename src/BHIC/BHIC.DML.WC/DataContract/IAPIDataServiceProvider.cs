using BHIC.DML.WC.DTO;
using BHIC.Domain.Dashboard;
using System;
using System.Collections.Generic;
using System.Security;

namespace BHIC.DML.WC.DataContract
{
    public interface IAPIDataServiceProvider
    {
        bool CreateUserPolicy(UserPolicyDTO UserPolicy, out Int64 policyId);
        int GetUserDetailId(string UserName);
        bool IsPolicyNumberExists(int Option,string Value);   
        bool IsValidUserCredentials(string UserName, SecureString Password);
        int GetOrganizationAddressId(int organizationId);
        bool IsValidAPICredentials(string UserName, SecureString Password);
        bool SaveQuote(QuoteDTO Quote);
        QuoteDTO GetQuoteInfo(string quoteNumber);
        int DeleteUser(string EmailID);
        int MergeAccounts(string OldEmailID, string NewEmailID);
        SecondaryAccountRegistration CreateUserAccount(SecondaryAccountRegistration user);
        void DetachPolicy(string EmailID, List<string> PolicyCodes);
        void AttachPolicy(string EmailID, List<string> PolicyCodes, DateTime StartTime, DateTime EndTime);
    }
}
