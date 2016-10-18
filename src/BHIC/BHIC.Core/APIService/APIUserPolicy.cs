using BHIC.Common;
using BHIC.Common.Client;
using BHIC.Common.Configuration;
using BHIC.Common.XmlHelper;
using BHIC.Contract.APIService;
using BHIC.Contract.Background;
using BHIC.Contract.Mailing;
using BHIC.Core.Background;
using BHIC.Core.Mailing;
using BHIC.Core.PurchasePath;
using BHIC.DML.WC.DataContract;
using BHIC.DML.WC.DataService;
using BHIC.DML.WC.DTO;
using BHIC.Domain.Dashboard;
using BHIC.Domain.Policy;
using BHIC.ViewDomain.Mailing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BHIC.Core
{
    /// <summary>
    /// This API service is for BOP to the create user and map the policy.
    /// </summary>
    public class APIUserPolicy : IUserPolicyService
    {
        IAPIDataServiceProvider provider = new APIDataServiceProvider();
        ServiceProvider guardServiceProvider = new GuardServiceProvider() { ServiceCategory = ServiceProviderConstants.GuardServiceCategoryWC };
        int initialRequest = 0;
        /// <summary>
        /// To Create User Policy. This will create OrganizationUserDetail, OrganizationAddress, Quote and Policy Information 
        /// In case of new user, registration & welcome mail will be sent otherwise only welcome mail will be sent.
        /// </summary>
        /// <param name="UserPolicy"></param>
        /// <param name="ExistingUser"></param>
        /// <param name="targetUrl"></param>
        /// <param name="PolicyId"></param>
        /// <returns></returns>
        public bool CreateUserPolicy(UserPolicyDTO UserPolicy, bool ExistingUser, out Int64 PolicyId)
        {
            bool createStatus = provider.CreateUserPolicy(UserPolicy, out PolicyId);
            if (createStatus)
            {
                initialRequest = 0;
                SendWelcomeMailNotification(UserPolicy, ExistingUser);
            }
            return createStatus;

        }
        /// <summary>
        /// To Send Welcome & Registration Notification to user.
        /// </summary>
        /// <param name="UserPolicy"></param>
        /// <param name="ExistingUser"></param>
        /// <param name="targetUrl"></param>

        public void SendWelcomeMailNotification(UserPolicyDTO UserPolicy, bool ExistingUser)
        {

            if (initialRequest == 0)
            {
                IMailingService mailingService = new MailingService();
                ServiceProvider guardServiceProvider = new GuardServiceProvider() { ServiceCategory = ServiceProviderConstants.GuardServiceCategoryWC };
                ISystemVariableService systemVariable = new SystemVariableService(guardServiceProvider);

                if (!ExistingUser)
                {
                    var encryptedURL = Encryption.EncryptText(UserPolicy.EmailID);
                    PolicyRegistrationMailViewModel policyRegistrationMailVM = new PolicyRegistrationMailViewModel();
                    policyRegistrationMailVM.CompanyName = ConfigCommonKeyReader.ApplicationContactInfo["CompanyName"];
                    policyRegistrationMailVM.WebsiteUrlText = systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_Domain"]);
                    policyRegistrationMailVM.WebsiteUrlHref = systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_WebsiteShortURL"]);
                    policyRegistrationMailVM.RegisterEmailText = ConfigCommonKeyReader.ApplicationContactInfo["SupportEmailTextRegistration"];
                    policyRegistrationMailVM.RegisterEmailHref = ConfigCommonKeyReader.ApplicationContactInfo["SupportEmailHrefRegistration"];
                    policyRegistrationMailVM.SupportPhoneNumber = ConfigCommonKeyReader.ApplicationContactInfo["SupportPhoneNumber"];
                    policyRegistrationMailVM.SupportPhoneNumberHref = ConfigCommonKeyReader.ApplicationContactInfo["SupportPhoneNumberHref"];
                    policyRegistrationMailVM.TargetUrl = string.Format("{0}{1}#{2}/{3}", GetSchemeAndHostName(), ConfigCommonKeyReader.PolicyCentreURL, "Login", encryptedURL);
                    policyRegistrationMailVM.UserName = UserPolicy.FirstName + " " + UserPolicy.LastName;
                    policyRegistrationMailVM.AbsoulteURL = CDN.GetEmailImageUrl();
                    var mailRegStatus = mailingService.UserRegistrationMail(UserPolicy.EmailID, policyRegistrationMailVM);
                }

                PolicyWelcomeMailViewModel policyWelcomeMailVM = new PolicyWelcomeMailViewModel();
                policyWelcomeMailVM.CompanyName = ConfigCommonKeyReader.ApplicationContactInfo["CompanyName"];
                policyWelcomeMailVM.WebsiteUrlText = systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_Domain"]);
                policyWelcomeMailVM.WebsiteUrlHref = systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_WebsiteShortURL"]);
                policyWelcomeMailVM.SupportEmailText = ConfigCommonKeyReader.ApplicationContactInfo["SupportEmailTextWelcome"];
                policyWelcomeMailVM.SupportEmailHref = ConfigCommonKeyReader.ApplicationContactInfo["SupportEmailHrefWelcome"];
                policyWelcomeMailVM.SupportPhoneNumber = ConfigCommonKeyReader.ApplicationContactInfo["SupportPhoneNumber"];
                policyWelcomeMailVM.SupportPhoneNumberHref = ConfigCommonKeyReader.ApplicationContactInfo["SupportPhoneNumberHref"];
                policyWelcomeMailVM.TargetUrl = string.Format("{0}{1}#{2}", GetSchemeAndHostName(), ConfigCommonKeyReader.PolicyCentreURL, "Login");
                policyWelcomeMailVM.InsuredBusinessName = (!string.IsNullOrEmpty(UserPolicy.OrganizationName) ? UserPolicy.OrganizationName :
                                                            (!string.IsNullOrEmpty(UserPolicy.LastName) ?
                                                            string.Concat(UserPolicy.FirstName + " " + UserPolicy.LastName) : UserPolicy.FirstName));

                policyWelcomeMailVM.PolicyEffectiveDate = UserPolicy.InceptionDate;
                policyWelcomeMailVM.PolicyEffectiveDateString = UserPolicy.InceptionDate.ToString("MM/dd/yyyy");
                policyWelcomeMailVM.PolicyNumber = UserPolicy.PolicyNumber;
                policyWelcomeMailVM.TotalPremiumAmount = Convert.ToDecimal(UserPolicy.TotalPremiumAmount).ToString("#,##0.00");
                policyWelcomeMailVM.PremiumAmountPaid = Convert.ToDecimal(UserPolicy.TotalPremiumPaid).ToString("#,##0.00");
                policyWelcomeMailVM.PolicyInstalments = UserPolicy.Installments;
                if (UserPolicy.Installments > 0)
                {
                    policyWelcomeMailVM.PolicyNextInstallmentAmount = UserPolicy.InstallmentAmount;
                    policyWelcomeMailVM.NextInstallmentAmount = UserPolicy.InstallmentAmount.ToString("#,##0.00");
                }

                //var nextDueDate = CalculatePolicyNextDueDate(UserPolicy.InceptionDate, UserPolicy.Installments,UserPolicy.FrequencyCode);

                //DateTime PolicyNextDueDate = nextDueDate ?? new DateTime();
                //String PolicyNextDueDateString = nextDueDate != null ? nextDueDate.Value.ToString("MM/dd/yyyy") : "none";
                DateTime PolicyNextDueDate = UserPolicy.NextDueDate ?? new DateTime();
                String PolicyNextDueDateString = ((UserPolicy.NextDueDate != null && UserPolicy.NextDueDate != DateTime.MinValue) ?
                                                    UserPolicy.NextDueDate.Value.ToString("MM/dd/yyyy") : "none");
                policyWelcomeMailVM.PolicyNextDueDateString = PolicyNextDueDateString;
                policyWelcomeMailVM.PolicyNextDueDate = PolicyNextDueDate;
                policyWelcomeMailVM.AbsoulteURL = CDN.GetEmailImageUrl();
                policyWelcomeMailVM.Physical_Address2 = systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Physical_Address2"]);
                policyWelcomeMailVM.Physical_AddressCSZ = systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Physical_AddressCSZ"]);
                var mailStatus = mailingService.WelcomeMailBOP(UserPolicy.EmailID, policyWelcomeMailVM);
            }
            else
            {
                //BHIC.Common.Reattempt.ReattemptLog.Register(System.Reflection.MethodBase.GetCurrentMethod(), UserPolicy, ExistingUser, GetSchemeAndHostName());
            }

        }


        /// <summary>
        /// Calculate next due date based on slected payment term by user
        /// </summary>
        /// <param name="selectedPaymentTerms"></param>
        /// <returns></returns>
        private DateTime? CalculatePolicyNextDueDate(DateTime? inceptionDate, int installments, string frequencyCode)
        {
            //return value
            DateTime? nextDueDate = null;

            //Comment : Here set next payment details
            if (inceptionDate != null && installments > 0)
            {
                frequencyCode = frequencyCode ?? string.Empty;
                //Comment : Here based on ferquency code decide next date
                switch (frequencyCode)
                {
                    case "S":
                        //e.g : {"PaymentPlanId":1318,"Fplan":"D","Description":"60% Down + Balance in 6 Months","MinPrem":500.0000,"DownType":"%","Down":60,"Pays":1,"Freq":"S","FplanExt":"D,60,1,S"},
                        nextDueDate = inceptionDate.Value.AddMonths(6);
                        break;
                    case "M":
                        nextDueDate = inceptionDate.Value.AddMonths(1);
                        break;
                }
            }

            return nextDueDate;
        }

        /// <summary>
        /// To check if the policy is exists. It can be checked either with policy number or quote number
        /// </summary>
        /// <param name="Option">1->Policy Number;2->Quote Number</param>
        /// <param name="Value"> Policy Number / Quote Number</param>
        /// <returns></returns>

        public bool IsPolicyExists(int Option, string Value)
        {
            return provider.IsPolicyNumberExists(Option, Value);
        }

        /// <summary>
        /// This is to Save Quote for BOP(Save for Later functionality)
        /// </summary>
        /// <param name="quote"></param>
        /// <returns></returns>
        public bool SaveQuote(QuoteDTO quote)
        {
            bool createStatus = provider.SaveQuote(quote);
            return createStatus;
        }

        public bool SendSaveForLaterMailNotification(SaveQuoteRequestDTO Quote)
        {
            #region Comment : Here create mail embedded link

            string baseUrl = Quote.ReteriveQuoteURL;

            #endregion

            #region Comment : Here prepare mail object model using ThemeManager and shared pre-defined templates

            Dictionary<string, object> mailContentModel = new Dictionary<string, object>();
            ServiceProvider guardServiceProvider = new GuardServiceProvider() { ServiceCategory = ServiceProviderConstants.GuardServiceCategoryWC };
            ISystemVariableService systemVariable = new SystemVariableService(guardServiceProvider);

            //mailContentModel.Add("anchorText", ThemeManager.ThemeSharedContent("useremail-saveforlater-anchortext") ?? null);
            var reviewQuoteText = ThemeManager.ThemeSharedContent("useremail-saveforlater-bop-anchortext") ?? string.Empty;
            mailContentModel.Add("saveforlaterMailBody", ThemeManager.ThemeSharedContent("useremail-saveforlater-bop-body") ?? null);
            mailContentModel.Add("saveforlaterMailSubject", ThemeManager.ThemeSharedContent("useremail-saveforlater-bop-subject") ?? null);

            #endregion

            #region Comment : Here prepare template populated with data and then send mail to user

            var model = new SaveForLaterMailViewModel
            {
                //Basic communication/contact details
                CompanyName = ConfigCommonKeyReader.ApplicationContactInfo["CompanyName"],
                WebsiteUrlText = systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_Domain"]),
                WebsiteUrlHref = systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_WebsiteShortURL"]),
                SupportEmailText = ConfigCommonKeyReader.ApplicationContactInfo["SupportEmailTextQuotes"],
                SupportEmailHref = ConfigCommonKeyReader.ApplicationContactInfo["SupportEmailHrefQuotes"],
                SupportPhoneNumber = ConfigCommonKeyReader.ApplicationContactInfo["SupportPhoneNumber"],
                SupportPhoneNumberHref = ConfigCommonKeyReader.ApplicationContactInfo["SupportPhoneNumberHref"],
                AbsoulteURL = CDN.GetEmailImageUrl(),
                Physical_Address2 = systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Physical_Address2"]),
                Physical_AddressCSZ = systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Physical_AddressCSZ"]),
                //SaveForLater review-quote link details
                ReviewQuoteText = reviewQuoteText,
                ReviewQuoteHref = string.Format("{0}{1}{2}", "", baseUrl, "")
            };

            //Comment : Here build template and send mail
            MailTemplateBuilder buildTemplate = new MailTemplateBuilder();
            var mailMsg = buildTemplate.MailSaveForLater(model, mailContentModel);

            //Comment : Here assign recipeint address
            mailMsg.RecipEmailAddr = new List<string>() { Quote.UserName };
            mailMsg.Cc = (ConfigCommonKeyReader.RetreiveQuoteEmailCc);
            mailMsg.Bcc = (ConfigCommonKeyReader.RetreiveQuoteEmailBcc);
            mailMsg.SenderEmailAddr = ConfigCommonKeyReader.RetreiveQuoteEmailFrom;
            // Commented by Guru, as not required CC for direct client email.
            //mailMsg.Cc = UtilityFunctions.GetListOfSplitedValues(ConfigCommonKeyReader.ClientEmailID);

            #endregion

            #region Comment : Here accordingly latest business requirement Clear/Expire all application session like "user, CustomSession"

            //Send user mail
            var mailSentStatus = mailMsg != null ? buildTemplate.SendMail(mailMsg) : false;


            #endregion

            return mailSentStatus;
        }

        /// <summary>
        /// This is to get the user id if the user is exists
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public int GetUserDetailId(string UserName)
        {
            return provider.GetUserDetailId(UserName);
        }

        /// <summary>
        /// This is to get the organizationaddressid of the user
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public int GetOrganizationAddressId(int organizationId)
        {
            return provider.GetOrganizationAddressId(organizationId);
        }


        public bool IsQuoteIdExists(string QuoteNumber, bool validatePolicy, int UserDetailId)
        {
            QuoteDTO quote = provider.GetQuoteInfo(QuoteNumber);
            bool quoteExists = false;
            if (quote != null && quote.QuoteNumber == QuoteNumber)
            {
                quoteExists = true;
                if (validatePolicy)
                {
                    bool policyExists = IsPolicyExists(2, QuoteNumber);
                    if (!policyExists)
                    {
                        if (quote.OrganizationUserDetailID != UserDetailId)
                        {
                            quoteExists = true;
                        }
                        else
                        {
                            quoteExists = false;
                        }
                    }
                }
            }
            return quoteExists;

        }


        public bool IsValidAPICredentials(string UserName, System.Security.SecureString Password)
        {
            return provider.IsValidAPICredentials(UserName, Password);
        }


        public bool IsValidUserCredentials(string UserName, System.Security.SecureString Password)
        {
            return provider.IsValidUserCredentials(UserName, Password);
        }

        /// <summary>
        /// Return current requesting site detail
        /// </summary>
        /// <returns></returns>
        private string GetSchemeAndHostName()
        {
            return string.Concat(HttpContext.Current.Request.Url.Scheme, "://", HttpContext.Current.Request.Url.Host);
        }

        public int DeleteAccount(string Email)
        {
            return provider.DeleteUser(Email);
        }

        public int MergeAccounts(string OldEmailID, string NewEmailID)
        {
            return provider.MergeAccounts(OldEmailID, NewEmailID);
        }

        public SecondaryAccountRegistration CreateAccount(SecondaryAccountRegistration accountRegistration)
        {
            return provider.CreateUserAccount(accountRegistration);
        }

        /// <summary>
        /// Creates a pseudo-random password containing the number of character classes
        /// defined by complexity, where 2 = alpha, 3 = alpha+num, 4 = alpha+num+special.
        /// </summary>
        public string GeneratePassword(int length, int complexity)
        {
            System.Security.Cryptography.RNGCryptoServiceProvider csp =
            new System.Security.Cryptography.RNGCryptoServiceProvider();

            // Define the possible character classes where complexity defines the number
            // of classes to include in the final output.
            char[][] classes =
                            {
                            @"abcdefghijklmnopqrstuvwxyz".ToCharArray(),
                            @"ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray(),
                            @"0123456789".ToCharArray(),
                            @" !""#$%&'()*+,./:;<>?@[\]^_{|}~".ToCharArray(),
                            };

            complexity = Math.Max(1, Math.Min(classes.Length, complexity));
            if (length < complexity)
                throw new ArgumentOutOfRangeException("length");

            // Since we are taking a random number 0-255 and modulo that by the number of
            // characters, characters that appear earilier in this array will recieve a
            // heavier weight. To counter this we will then reorder the array randomly.
            // This should prevent any specific character class from recieving a priority
            // based on it's order.
            char[] allchars = classes.Take(complexity).SelectMany(c => c).ToArray();
            byte[] bytes = new byte[allchars.Length];
            csp.GetBytes(bytes);
            for (int i = 0; i < allchars.Length; i++)
            {
                char tmp = allchars[i];
                allchars[i] = allchars[bytes[i] % allchars.Length];
                allchars[bytes[i] % allchars.Length] = tmp;
            }

            // Create the random values to select the characters
            Array.Resize(ref bytes, length);
            char[] result = new char[length];

            while (true)
            {
                csp.GetBytes(bytes);
                // Obtain the character of the class for each random byte
                for (int i = 0; i < length; i++)
                    result[i] = allchars[bytes[i] % allchars.Length];

                // Verify that it does not start or end with whitespace
                if (Char.IsWhiteSpace(result[0]) || Char.IsWhiteSpace(result[(length - 1) % length]))
                    continue;

                string testResult = new string(result);
                // Verify that all character classes are represented
                if (0 != classes.Take(complexity).Count(c => testResult.IndexOfAny(c) < 0))
                    continue;

                return testResult;
            }
        }
    }
}
