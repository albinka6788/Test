using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace BHIC.Common.XmlHelper
{
    public static class ConfigCommonKeyReader
    {
        #region Variables

        #region Common

        static string clientEmailID;
        static bool isLiveEnvironment;
        static bool useSystemVariable;
        static string systemVariableCacheDuration;
        static NameValueCollection applicationContactInfo;
        static string staticCommonFilePath;
        static string staticCommonFilePathForLossControlEnglish;
        static string staticCommonFilePathForLossControlSpanish;
        static string inspectletWid;
        static string refreshCache;
        static double reduceAPICacheDurationInSeconds;

        #region Api Failure Mail

        static List<string> apiFailureEmailTo;
        static List<string> apiFailureEmailCc;
        static List<string> apiFailureEmailBcc;
        static string apiFailureEmailFrom;

        #endregion

        #region Forgot Password Mail

        static List<string> forgotPasswordEmailTo;
        static List<string> forgotPasswordEmailCc;
        static List<string> forgotPasswordEmailBcc;
        static string forgotPasswordEmailFrom;

        #endregion

        #region Schedule Call Mail

        static List<string> scheduleCallEmailTo;
        static List<string> scheduleCallEmailCc;
        static List<string> scheduleCallEmailBcc;
        static string scheduleCallEmailFrom;

        #endregion

        #endregion

        #region PP

        static bool isDevEnvironment;
        static string environmentName;
        static bool requireSecureCookies;
        static string cdnPath;
        static string cdnDefaultCommonFolder;
        static string cdnDefaultWcFolder;
        static string wcEmailTemplateImgPath;
        static bool enableCdn;
        static string cdnVersion;
        static string schemeAndHostURL;
        static string hostURL;
        static string defaultThemeId;
        static int countyCacheInterval;
        static string applicationHomeUrl;
        static string purchasePathAppBaseURL;
        static string appName;
        static string policyCentreURL;
        static string policyCentreDashboardURL;
        static string dbKeyName;
        static string dbSchemaName;
        static int dbCommandTimeOut;
        static bool isTestingPaymentGateway;
        static bool isPaymentConfigEntry;
        static string paymentAPIURL;
        static string paymentURLIP;
        static string paymentURL;
        static string paymentLogin;
        static string paymentTransactionKey;
        static bool enableNLogTracing;
        static bool enableSessionObjectLogging;
        static bool enableContentLogging;
        static int howLongBeenInBusinessQuestionId;
        static bool Istransactionlog;
        static string transactionlogpath;
        static int authroizeAPIExpiryMinutes;
        static int totalNumbeOfClaimInPastThreeYears;
        static string reattemptFolderPath;
        static string reattemptFailureFolderPath;
        static string reattemptArchiveFolderPath;
        static string defaultPassword;
        static string defaultSecureKey;
        static string apiLogFileName;
        static int industryCacheInterval;
        static int subIndustryCacheInterval;
        static int lineOfBusinessCacheInterval;
        static int goodAndBadStateCacheInterval;
        static int multipleStatesCacheInterval;
        static string multipleRiskIdErrorMessage;
        static int addNumberofMonthinXModExpiryDate;
        static string cloudTypographyUrl;
        static string policyCreateDTOName;
        static string policyCreateDTOProperty;
        static bool relaodLocalData;
        static string localStoredDataPath;
        static string appBaseUrl;

        #region Welcome Mail

        static List<string> welcomeEmailTo;
        static List<string> welcomeEmailCc;
        static List<string> welcomeEmailBcc;
        static string welcomeEmailFrom;

        #endregion

        #region Registration Mail

        static List<string> registrationEmailTo;
        static List<string> registrationEmailCc;
        static List<string> registrationEmailBcc;
        static string registrationEmailFrom;

        #endregion

        #region Payment Failure Mail

        static List<string> policyCreationFailureIntimationTo;
        static List<string> policyCreationFailureIntimationCc;
        static List<string> policyCreationFailureIntimationBcc;
        static string policyCreationFailureIntimationFrom;

        #endregion

        #region Password Reset Mail

        static List<string> resetPasswordEmailTo;
        static List<string> resetPasswordEmailCc;
        static List<string> resetPasswordEmailBcc;
        static string resetPasswordEmailFrom;

        #endregion

        #region Referral Mail

        static List<string> referralEmailTo;
        static List<string> referralEmailCc;
        static List<string> referralEmailBcc;
        static string referralEmailFrom;

        #endregion

        #region Retreive Quote Mail

        static List<string> retreiveQuoteEmailTo;
        static List<string> retreiveQuoteEmailCc;
        static List<string> retreiveQuoteEmailBcc;
        static string retreiveQuoteEmailFrom;

        #endregion

        #region Comment : Here application all different ClientEmailId's

        static List<string> clientEmailIdCreatePolicyFailure;
        static List<string> clientEmailIdCancelPolicy;
        static List<string> clientEmailIdClaimRequest;
        static List<string> clientEmailIdPolicyChangeRequest;
        static List<string> clientEmailIdAddressChangeRequest;

        #endregion

        static string bopUrl;
        static List<string> allowedFileTypes;

        #endregion

        #region PC

        static string baseUrlPath;
        static string wcBaseUrlPath;
        static string cdnDefaultDashboardFolder;
        static string staticFilePath;
        static string uploadFiles;
        static string wcUrl;
        static string wcAngularBaseModuleUrl;
        static string captchaSiteKey;
        static string captchaSecretKey;
        static int quoteExpiryDays;
        static int maxFileSize;
        static int minFileSize;
        static int maxFileNameChar;
        static int maxFileCount;
        static int loginAttempt;
        static int unlockAccountTime;

        #region Policy Change Mail

        static List<string> policyChangeEmailTo;
        static List<string> policyChangeEmailCc;
        static List<string> policyChangeEmailBcc;
        static string policyChangeEmailFrom;

        #endregion

        #region Policy Cancellation Mail

        static List<string> policyCancellationEmailTo;
        static List<string> policyCancellationEmailCc;
        static List<string> policyCancellationEmailBcc;
        static string policyCancellationEmailFrom;

        #endregion

        #region Address Change Mail

        static List<string> addressChangeEmailTo;
        static List<string> addressChangeEmailCc;
        static List<string> addressChangeEmailBcc;
        static string addressChangeEmailFrom;

        #endregion

        #region Claim Request Mail

        static List<string> claimRequestEmailTo;
        static List<string> claimRequestEmailCc;
        static List<string> claimRequestEmailBcc;
        static string claimRequestEmailFrom;

        #endregion

        #region Upload Documents Mail

        static List<string> uploadDocumentsEmailTo;
        static List<string> uploadDocumentsEmailCc;
        static List<string> uploadDocumentsEmailBcc;
        static string uploadDocumentsEmailFrom;

        #endregion

        #region Edit Contact Information Mail

        static List<string> contactInformationEmailTo;
        static List<string> contactInformationEmailCc;
        static List<string> contactInformationBcc;
        static string contactInformationEmailFrom;

        #endregion

        #endregion

        #region Live Chat

        static string lcLicense = "";
        static string lcGroup = "";
        static string lcServerName = "";
        static string lcServerValue = "";
        static string lcSrc = "";

        #endregion

        #region CommercialAuto
        static bool enableCommercialAuto;
        static string commercialAutoURL;
        static string polSOACode;
        #endregion

        #region Landing Page
        static string landingPageAppBaseURL;
        #endregion

        #region BOP
        static bool enableBOP;
        #endregion

        #region Payment Gateway

        static string creditCardPayments_AuthorizeNET_InTesting;
        static string creditCardPayments_AuthorizeNET_LoginID;
        static string creditCardPayments_AuthorizeNET_transactionKey;

        #endregion

        #region Google Analytics

        static string gaCode;

        #endregion

        #region NotificationTemplate
        static string emailTemplatePath;
        #endregion

        #endregion

        #region Constructor

        static ConfigCommonKeyReader()
        {
            #region Common

            clientEmailID = (ConfigurationManager.AppSettings["ClientEmailID"] ?? string.Empty);
            isLiveEnvironment = Convert.ToBoolean(ConfigurationManager.AppSettings["IsLiveEnvironment"] ?? "true");
            systemVariableCacheDuration = ConfigurationManager.AppSettings["SystemVariableCacheDuration"] ?? string.Empty;
            useSystemVariable = Convert.ToBoolean(ConfigurationManager.AppSettings["UseSystemVariable"]);
            applicationContactInfo = ConfigurationManager.GetSection("ApplicationContactInfo") as NameValueCollection;
            staticCommonFilePath = (ConfigurationManager.AppSettings["StaticCommonFilePath"] ?? string.Empty);
            staticCommonFilePathForLossControlEnglish = (ConfigurationManager.AppSettings["StaticCommonFilePathForLossControlEnglish"] ?? string.Empty);
            staticCommonFilePathForLossControlSpanish = (ConfigurationManager.AppSettings["StaticCommonFilePathForLossControlSpanish"] ?? string.Empty);
            inspectletWid = (ConfigurationManager.AppSettings["InspectletWid"] ?? string.Empty);
            hostURL = (ConfigurationManager.AppSettings["HostURL"] ?? string.Empty);
            refreshCache = (ConfigurationManager.AppSettings["refreshCache"] ?? string.Empty);
            reduceAPICacheDurationInSeconds = Convert.ToDouble((ConfigurationManager.AppSettings["ReduceAPICacheDurationInSeconds"] ?? "120"));
            
            #region Api Failure Mail

            apiFailureEmailTo = (ConfigurationManager.AppSettings["ApiFailureEmailTo"] ?? string.Empty).GetListOfSplitedValues();
            apiFailureEmailCc = (ConfigurationManager.AppSettings["ApiFailureEmailCc"] ?? string.Empty).GetListOfSplitedValues();
            apiFailureEmailBcc = (ConfigurationManager.AppSettings["ApiFailureEmailBcc"] ?? string.Empty).GetListOfSplitedValues();
            apiFailureEmailFrom = (ConfigurationManager.AppSettings["ApiFailureEmailFrom"] ?? string.Empty);

            #endregion

            #region Forgot Password Mail

            forgotPasswordEmailTo = (ConfigurationManager.AppSettings["ForgotPasswordEmailTo"] ?? string.Empty).GetListOfSplitedValues();
            forgotPasswordEmailCc = (ConfigurationManager.AppSettings["ForgotPasswordEmailCc"] ?? string.Empty).GetListOfSplitedValues();
            forgotPasswordEmailBcc = (ConfigurationManager.AppSettings["ForgotPasswordEmailBcc"] ?? string.Empty).GetListOfSplitedValues();
            forgotPasswordEmailFrom = (ConfigurationManager.AppSettings["ForgotPasswordEmailFrom"] ?? string.Empty);

            #endregion

            #region Schedule Call Mail

            scheduleCallEmailTo = (ConfigurationManager.AppSettings["ScheduleCallEmailTo"] ?? string.Empty).GetListOfSplitedValues();
            scheduleCallEmailCc = (ConfigurationManager.AppSettings["ScheduleCallEmailCc"] ?? string.Empty).GetListOfSplitedValues();
            scheduleCallEmailBcc = (ConfigurationManager.AppSettings["ScheduleCallEmailBcc"] ?? string.Empty).GetListOfSplitedValues();
            scheduleCallEmailFrom = (ConfigurationManager.AppSettings["ScheduleCallEmailFrom"] ?? string.Empty);

            #endregion

            


            #endregion

            #region PP

            environmentName = (ConfigurationManager.AppSettings["Environment"] ?? "dev");
            isDevEnvironment = (ConfigurationManager.AppSettings["Environment"] ?? "dev").Equals("dev", StringComparison.OrdinalIgnoreCase);
            requireSecureCookies = Convert.ToBoolean(ConfigurationManager.AppSettings["RequireSecureCookies"] ?? "true");
            cdnPath = (ConfigurationManager.AppSettings["CdnPath"] ?? string.Empty);
            cdnDefaultCommonFolder = (ConfigurationManager.AppSettings["CdnDefaultCommonFolder"] ?? string.Empty);
            cdnDefaultWcFolder = (ConfigurationManager.AppSettings["CdnDefaultWcFolder"] ?? string.Empty);
            wcEmailTemplateImgPath = (ConfigurationManager.AppSettings["WcEmailTemplateImgPath"] ?? string.Empty);
            enableCdn = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableCdn"] ?? "true");
            cdnVersion = string.Empty;
            defaultThemeId = (ConfigurationManager.AppSettings["DefaultThemeId"] ?? string.Empty);
            countyCacheInterval = Convert.ToInt32(ConfigurationManager.AppSettings["CountyCacheInterval"] ?? "1");
            applicationHomeUrl = (ConfigurationManager.AppSettings["ApplicationHomeUrl"] ?? string.Empty);
            purchasePathAppBaseURL = (ConfigurationManager.AppSettings["PurchasePathAppBaseURL"] ?? string.Empty);
            appName = (ConfigurationManager.AppSettings["AppName"] ?? string.Empty);
            policyCentreURL = (ConfigurationManager.AppSettings["PolicyCentreURL"] ?? string.Empty);
            policyCentreDashboardURL = (ConfigurationManager.AppSettings["PolicyCentreDashboardURL"] ?? string.Empty);
            dbKeyName = (ConfigurationManager.AppSettings["DBKeyName"] ?? "GuinnessDB");
            dbSchemaName = (ConfigurationManager.AppSettings["DBSchemaName"] ?? string.Empty);
            dbCommandTimeOut = Convert.ToInt32(ConfigurationManager.AppSettings["DBCommandTimeOut"] ?? "0");
            isTestingPaymentGateway = Convert.ToBoolean(ConfigurationManager.AppSettings["IsTestingPaymentGateway"] ?? "false");
            isPaymentConfigEntry = Convert.ToBoolean(ConfigurationManager.AppSettings["IsPaymentConfigEntry"] ?? "false");
            paymentAPIURL = ConfigurationManager.AppSettings["PaymentAPIURL"];
            paymentURLIP = (ConfigurationManager.AppSettings["PaymentURLIP"] ?? string.Empty);
            paymentURL = (ConfigurationManager.AppSettings["PaymentURL"] ?? string.Empty);
            paymentLogin = (ConfigurationManager.AppSettings["PaymentLogin"] ?? string.Empty);
            paymentTransactionKey = (ConfigurationManager.AppSettings["PaymentTransactionKey"] ?? string.Empty);
            enableNLogTracing = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableNLogTracing"] ?? "false");
            enableSessionObjectLogging = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSessionObjectLogging"] ?? "false");
            enableContentLogging = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableContentLogging"] ?? "false");
            howLongBeenInBusinessQuestionId = Convert.ToInt16(ConfigurationManager.AppSettings["HowLongBeenInBusinessQuestionId"]);
            Istransactionlog = Convert.ToBoolean(ConfigurationManager.AppSettings["IsTransactionLog"] ?? "true");
            transactionlogpath = (ConfigurationManager.AppSettings["transactionlogpath"]);
            totalNumbeOfClaimInPastThreeYears = Convert.ToInt16(ConfigurationManager.AppSettings["TotalNumbeOfClaimInPastThreeYears"]);
            bopUrl = ConfigurationManager.AppSettings["BopUrl"] ?? string.Empty;
            allowedFileTypes = (ConfigurationManager.AppSettings["AllowedFileTypes"] ?? ".exe").GetListOfSplitedValues(",");
            defaultPassword = "NCJKP$4552G";
            defaultSecureKey = "!0V#4y0()B(sIn#s";// "!5663a#KN%2S4^9s";
            apiLogFileName = ConfigurationManager.AppSettings["APILogFileName"] ?? string.Empty;
            industryCacheInterval = Convert.ToInt32(ConfigurationManager.AppSettings["IndustryCacheInterval"]);
            subIndustryCacheInterval = Convert.ToInt32(ConfigurationManager.AppSettings["SubIndustryCacheInterval"]);
            lineOfBusinessCacheInterval = Convert.ToInt32(ConfigurationManager.AppSettings["LineOfBusinessCacheInterval"]);
            goodAndBadStateCacheInterval = Convert.ToInt32(ConfigurationManager.AppSettings["GoodAndBadStateCacheInterval"]);
            multipleStatesCacheInterval = Convert.ToInt32(ConfigurationManager.AppSettings["MultipleStatesCacheInterval"]);
            multipleRiskIdErrorMessage = (ConfigurationManager.AppSettings["MultipleRiskIdErrorMessage"] ?? string.Empty);
            addNumberofMonthinXModExpiryDate = Convert.ToInt32((ConfigurationManager.AppSettings["AddNumberofMonthinXModExpiryDate"] ?? "0"));

            cloudTypographyUrl = (ConfigurationManager.AppSettings["CloudTypographyUrl"] ?? string.Empty);
            policyCreateDTOName = (ConfigurationManager.AppSettings["PolicyCreateDTOName"] ?? string.Empty);
            policyCreateDTOProperty = (ConfigurationManager.AppSettings["PolicyCreateDTOProperty"] ?? string.Empty);
            relaodLocalData = Convert.ToBoolean(ConfigurationManager.AppSettings["RelaodLocalData"] ?? "false");
            localStoredDataPath = (ConfigurationManager.AppSettings["LocalStoredDataPath"] ?? string.Empty);

            #region Welcome Mail

            welcomeEmailTo = (ConfigurationManager.AppSettings["WelcomeEmailTo"] ?? string.Empty).GetListOfSplitedValues();
            welcomeEmailCc = (ConfigurationManager.AppSettings["WelcomeEmailCc"] ?? string.Empty).GetListOfSplitedValues();
            welcomeEmailBcc = (ConfigurationManager.AppSettings["WelcomeEmailBcc"] ?? string.Empty).GetListOfSplitedValues();
            welcomeEmailFrom = (ConfigurationManager.AppSettings["WelcomeEmailFrom"] ?? string.Empty);

            #endregion

            #region Registration Mail

            registrationEmailTo = (ConfigurationManager.AppSettings["RegistrationEmailTo"] ?? string.Empty).GetListOfSplitedValues();
            registrationEmailCc = (ConfigurationManager.AppSettings["RegistrationEmailCc"] ?? string.Empty).GetListOfSplitedValues();
            registrationEmailBcc = (ConfigurationManager.AppSettings["RegistrationEmailBcc"] ?? string.Empty).GetListOfSplitedValues();
            registrationEmailFrom = (ConfigurationManager.AppSettings["RegistrationEmailFrom"] ?? string.Empty);

            #endregion

            #region Payment Failure Mail

            policyCreationFailureIntimationTo = (ConfigurationManager.AppSettings["PolicyCreationFailureIntimationTo"] ?? string.Empty).GetListOfSplitedValues();
            policyCreationFailureIntimationCc = (ConfigurationManager.AppSettings["PolicyCreationFailureIntimationCc"] ?? string.Empty).GetListOfSplitedValues();
            policyCreationFailureIntimationBcc = (ConfigurationManager.AppSettings["PolicyCreationFailureIntimationBcc"] ?? string.Empty).GetListOfSplitedValues();
            policyCreationFailureIntimationFrom = (ConfigurationManager.AppSettings["PolicyCreationFailureIntimationFrom"] ?? string.Empty);

            #endregion

            #region Password Reset Mail

            resetPasswordEmailTo = (ConfigurationManager.AppSettings["ResetPasswordEmailTo"] ?? string.Empty).GetListOfSplitedValues();
            resetPasswordEmailCc = (ConfigurationManager.AppSettings["ResetPasswordEmailCc"] ?? string.Empty).GetListOfSplitedValues();
            resetPasswordEmailBcc = (ConfigurationManager.AppSettings["ResetPasswordEmailBcc"] ?? string.Empty).GetListOfSplitedValues();
            resetPasswordEmailFrom = (ConfigurationManager.AppSettings["ResetPasswordEmailFrom"] ?? string.Empty);

            #endregion

            #region Referral Mail

            referralEmailTo = (ConfigurationManager.AppSettings["ReferralEmailTo"] ?? string.Empty).GetListOfSplitedValues();
            referralEmailCc = (ConfigurationManager.AppSettings["ReferralEmailCc"] ?? string.Empty).GetListOfSplitedValues();
            referralEmailBcc = (ConfigurationManager.AppSettings["ReferralEmailBcc"] ?? string.Empty).GetListOfSplitedValues();
            referralEmailFrom = (ConfigurationManager.AppSettings["ReferralEmailFrom"] ?? string.Empty);

            #endregion

            #region Retreive Quote Mail

            retreiveQuoteEmailTo = (ConfigurationManager.AppSettings["RetreiveQuoteEmailTo"] ?? string.Empty).GetListOfSplitedValues();
            retreiveQuoteEmailCc = (ConfigurationManager.AppSettings["RetreiveQuoteEmailCc"] ?? string.Empty).GetListOfSplitedValues();
            retreiveQuoteEmailBcc = (ConfigurationManager.AppSettings["RetreiveQuoteEmailBcc"] ?? string.Empty).GetListOfSplitedValues();
            retreiveQuoteEmailFrom = (ConfigurationManager.AppSettings["RetreiveQuoteEmailFrom"] ?? string.Empty);

            #endregion

            #region Comment : Here application all different ClientEmailId's initialization

            clientEmailIdCreatePolicyFailure = (ConfigurationManager.AppSettings["ClientEmailIdCreatePolicyFailure"] ?? string.Empty).GetListOfSplitedValues(";");
            clientEmailIdCancelPolicy = (ConfigurationManager.AppSettings["ClientEmailIdCancelPolicy"] ?? string.Empty).GetListOfSplitedValues(";");
            clientEmailIdClaimRequest = (ConfigurationManager.AppSettings["ClientEmailIdClaimRequest"] ?? string.Empty).GetListOfSplitedValues(";");
            clientEmailIdPolicyChangeRequest = (ConfigurationManager.AppSettings["ClientEmailIdPolicyChangeRequest"] ?? string.Empty).GetListOfSplitedValues(";");
            clientEmailIdAddressChangeRequest = (ConfigurationManager.AppSettings["ClientEmailIdAddressChangeRequest"] ?? string.Empty).GetListOfSplitedValues(";");

            #endregion

            #endregion

            #region PC

            baseUrlPath = (ConfigurationManager.AppSettings["BaseUrlPath"] ?? string.Empty);
            cdnDefaultDashboardFolder = (ConfigurationManager.AppSettings["CdnDefaultDashboardFolder"] ?? string.Empty);
            staticFilePath = (ConfigurationManager.AppSettings["StaticFilePath"] ?? string.Empty);
            uploadFiles = (ConfigurationManager.AppSettings["UploadFiles"] ?? string.Empty);
            wcUrl = (ConfigurationManager.AppSettings["WCUrl"] ?? string.Empty);
            wcAngularBaseModuleUrl = (ConfigurationManager.AppSettings["WcAngularBaseModuleUrl"] ?? string.Empty);
            captchaSiteKey = (ConfigurationManager.AppSettings["CaptchaSiteKey"] ?? string.Empty);
            captchaSecretKey = (ConfigurationManager.AppSettings["CaptchaSecretKey"] ?? string.Empty);
            quoteExpiryDays = (Convert.ToInt32(ConfigurationManager.AppSettings["QuoteExpiryDays"] ?? "60"));
            minFileSize = (Convert.ToInt32(ConfigurationManager.AppSettings["MinFileSize"] ?? "1"));
            maxFileSize = (Convert.ToInt32(ConfigurationManager.AppSettings["MaxFileSize"] ?? "10485760"));
            maxFileNameChar = (Convert.ToInt32(ConfigurationManager.AppSettings["MaxFileNameChar"] ?? "255"));
            maxFileCount = (Convert.ToInt32(ConfigurationManager.AppSettings["MaxFileCount"] ?? "3"));
            loginAttempt = (Convert.ToInt32(ConfigurationManager.AppSettings["LoginAttempt"] ?? "5"));
            unlockAccountTime = (Convert.ToInt32(ConfigurationManager.AppSettings["UnlockAccountTimeinHours"] ?? "1"));
            wcBaseUrlPath = (ConfigurationManager.AppSettings["WcBaseUrlPath"] ?? string.Empty);

            #region  Policy Change Mail

            policyChangeEmailTo = (ConfigurationManager.AppSettings["PolicyChangeEmailTo"] ?? string.Empty).GetListOfSplitedValues();
            policyChangeEmailCc = (ConfigurationManager.AppSettings["PolicyChangeEmailCc"] ?? string.Empty).GetListOfSplitedValues();
            policyChangeEmailBcc = (ConfigurationManager.AppSettings["PolicyChangeEmailBcc"] ?? string.Empty).GetListOfSplitedValues();
            policyChangeEmailFrom = (ConfigurationManager.AppSettings["PolicyChangeEmailFrom"] ?? string.Empty);

            #endregion

            #region Policy Cancellation Mail

            policyCancellationEmailTo = (ConfigurationManager.AppSettings["PolicyCancellationEmailTo"] ?? string.Empty).GetListOfSplitedValues();
            policyCancellationEmailCc = (ConfigurationManager.AppSettings["PolicyCancellationEmailCc"] ?? string.Empty).GetListOfSplitedValues();
            policyCancellationEmailBcc = (ConfigurationManager.AppSettings["PolicyCancellationEmailBcc"] ?? string.Empty).GetListOfSplitedValues();
            policyCancellationEmailFrom = (ConfigurationManager.AppSettings["PolicyCancellationEmailFrom"] ?? string.Empty);

            #endregion

            #region Address Change Mail

            addressChangeEmailTo = (ConfigurationManager.AppSettings["AddressChangeEmailTo"] ?? string.Empty).GetListOfSplitedValues();
            addressChangeEmailCc = (ConfigurationManager.AppSettings["AddressChangeEmailCc"] ?? string.Empty).GetListOfSplitedValues();
            addressChangeEmailBcc = (ConfigurationManager.AppSettings["AddressChangeEmailBcc"] ?? string.Empty).GetListOfSplitedValues();
            addressChangeEmailFrom = (ConfigurationManager.AppSettings["AddressChangeEmailFrom"] ?? string.Empty);

            #endregion

            #region Claim Request Mail

            claimRequestEmailTo = (ConfigurationManager.AppSettings["ClaimRequestEmailTo"] ?? string.Empty).GetListOfSplitedValues();
            claimRequestEmailCc = (ConfigurationManager.AppSettings["ClaimRequestEmailCc"] ?? string.Empty).GetListOfSplitedValues();
            claimRequestEmailBcc = (ConfigurationManager.AppSettings["ClaimRequestEmailBcc"] ?? string.Empty).GetListOfSplitedValues();
            claimRequestEmailFrom = (ConfigurationManager.AppSettings["ClaimRequestEmailFrom"] ?? string.Empty);

            #endregion

            #region Upload Documents Mail

            uploadDocumentsEmailTo = (ConfigurationManager.AppSettings["UploadDocumentsEmailTo"] ?? string.Empty).GetListOfSplitedValues();
            uploadDocumentsEmailCc = (ConfigurationManager.AppSettings["UploadDocumentsEmailCc"] ?? string.Empty).GetListOfSplitedValues();
            uploadDocumentsEmailBcc = (ConfigurationManager.AppSettings["UploadDocumentsEmailBcc"] ?? string.Empty).GetListOfSplitedValues();
            uploadDocumentsEmailFrom = (ConfigurationManager.AppSettings["UploadDocumentsEmailFrom"] ?? string.Empty);

            #endregion

            #region Edit Contact Information Mail

            contactInformationEmailTo = (ConfigurationManager.AppSettings["ContactInformationEmailTo"] ?? string.Empty).GetListOfSplitedValues();
            contactInformationEmailCc = (ConfigurationManager.AppSettings["ContactInformationEmailCc"] ?? string.Empty).GetListOfSplitedValues();
            contactInformationBcc = (ConfigurationManager.AppSettings["ContactInformationEmailBcc"] ?? string.Empty).GetListOfSplitedValues();
            contactInformationEmailFrom = (ConfigurationManager.AppSettings["ContactInformationEmailFrom"] ?? string.Empty);

            #endregion

            #endregion

            #region Live Chat

            lcLicense = Convert.ToString(ConfigurationManager.AppSettings["LCLicense"]);
            lcGroup = Convert.ToString(ConfigurationManager.AppSettings["LCgroup"]);
            lcServerName = Convert.ToString(ConfigurationManager.AppSettings["LCServerName"]);
            lcServerValue = Convert.ToString(ConfigurationManager.AppSettings["LCServerValue"]);
            lcSrc = Convert.ToString(ConfigurationManager.AppSettings["LCSrc"]);

            #endregion

            #region Reattempt
            reattemptFolderPath = ConfigurationManager.AppSettings["ReattemptFolderPath"] ?? string.Empty;
            reattemptArchiveFolderPath = ConfigurationManager.AppSettings["ReattemptArchiveFolderPath"] ?? string.Empty;
            reattemptFailureFolderPath = ConfigurationManager.AppSettings["ReattemptFailureFolderPath"] ?? string.Empty;
            #endregion

            #region APIParameters
            authroizeAPIExpiryMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["ExpireMinutes"] ?? "5");
            #endregion

            #region Commercial Auto
            enableCommercialAuto = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableCommercialAuto"] ?? "false");
            commercialAutoURL = ConfigurationManager.AppSettings["CommercialAutoURL"] ?? string.Empty;
            polSOACode = ConfigurationManager.AppSettings["SOACode"] ?? string.Empty;
            #endregion

            #region Landing Page
            landingPageAppBaseURL = ConfigurationManager.AppSettings["LandingPageAppBaseURL"] ?? string.Empty;
            #endregion

            #region BOP
            enableBOP = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableBOP"] ?? "false");
            #endregion

            #region Payment Gateway

            creditCardPayments_AuthorizeNET_InTesting = ConfigurationManager.AppSettings["CreditCardPayments_AuthorizeNET_InTesting"] ?? string.Empty;
            creditCardPayments_AuthorizeNET_LoginID = ConfigurationManager.AppSettings["CreditCardPayments_AuthorizeNET_LoginID"] ?? string.Empty;
            creditCardPayments_AuthorizeNET_transactionKey = ConfigurationManager.AppSettings["CreditCardPayments_AuthorizeNET_transactionKey"] ?? string.Empty;

            #endregion

            #region Google Analytics

            gaCode = (Convert.ToString(ConfigurationManager.AppSettings["GACode"]));

            #endregion
            #region NotificationTemplate
            emailTemplatePath = (ConfigurationManager.AppSettings["EmailTemplatePath"] ?? string.Empty);
             
            #endregion
        }

        #endregion

        #region Properties

        #region Common

        public static string ClientEmailID
        {
            get
            {
                return clientEmailID;
            }
        }

        public static bool IsLiveEnvironment
        {
            get
            {
                return isLiveEnvironment;
            }
        }

        public static string SystemVariableCacheDuration
        {
            get
            {
                return systemVariableCacheDuration;
            }
        }

        public static bool UseSystemVariable
        {
            get
            {
                return useSystemVariable;
            }
        }

        public static NameValueCollection ApplicationContactInfo
        {
            get
            {
                return applicationContactInfo;
            }
        }
        public static string StaticCommonFilePath
        {
            get
            {
                return staticCommonFilePath;
            }
        }

        public static string StaticCommonFilePathForLossControlEnglish
        {
            get
            {
                return staticCommonFilePathForLossControlEnglish;
            }
        }

        public static string StaticCommonFilePathForLossControlSpanish
        {
            get
            {
                return staticCommonFilePathForLossControlSpanish;
            }
        }


        public static string InspectletWid
        {
            get
            {
                return inspectletWid;
            }
        }

        public static string RefreshCache
        {
            get
            {
                return refreshCache;
            }
        }

        public static double ReduceAPICacheDurationInSeconds
        {
            get
            {
                return reduceAPICacheDurationInSeconds;
            }
        }
        #region Api Failure Mail

        public static List<string> ApiFailureEmailTo
        {
            get
            {
                return apiFailureEmailTo;
            }
        }

        public static List<string> ApiFailureEmailCc
        {
            get
            {
                return apiFailureEmailCc;
            }
        }

        public static List<string> ApiFailureEmailBcc
        {
            get
            {
                return apiFailureEmailBcc;
            }
        }

        public static string ApiFailureEmailFrom
        {
            get
            {
                return apiFailureEmailFrom;
            }
        }

        #endregion

        #region Forgot Password Mail

        public static List<string> ForgotPasswordEmailTo
        {
            get
            {
                return forgotPasswordEmailTo;
            }
        }

        public static List<string> ForgotPasswordEmailCc
        {
            get
            {
                return forgotPasswordEmailCc;
            }
        }

        public static List<string> ForgotPasswordEmailBcc
        {
            get
            {
                return forgotPasswordEmailBcc;
            }
        }

        public static string ForgotPasswordEmailFrom
        {
            get
            {
                return forgotPasswordEmailFrom;
            }
        }

        #endregion

        #region Schedule Call Mail

        public static List<string> ScheduleCallEmailTo
        {
            get
            {
                return scheduleCallEmailTo;
            }
        }

        public static List<string> ScheduleCallEmailCc
        {
            get
            {
                return scheduleCallEmailCc;
            }
        }

        public static List<string> ScheduleCallEmailBcc
        {
            get
            {
                return scheduleCallEmailBcc;
            }
        }

        public static string ScheduleCallEmailFrom
        {
            get
            {
                return scheduleCallEmailFrom;
            }
        }

        #endregion

        #endregion

        #region PP

        public static string EnvironmentName
        {
            get
            {
                return environmentName;
            }
        }

        public static bool IsDevEnvironment
        {
            get
            {
                return isDevEnvironment;
            }
        }

        public static bool RequireSecureCookies
        {
            get
            {
                return requireSecureCookies;
            }
        }

        public static string CdnPath
        {
            get
            {
                return cdnPath;
            }
        }

        public static string CdnDefaultCommonFolder
        {
            get
            {
                return cdnDefaultCommonFolder;
            }
        }

        public static string WcEmailTemplateImgPath
        {
            get
            {
                return wcEmailTemplateImgPath;
            }
        }

        public static string CdnDefaultWcFolder
        {
            get
            {
                return cdnDefaultWcFolder;
            }
        }

        public static bool EnableCdn
        {
            get
            {
                return enableCdn;
            }
        }

        public static string CdnVersion
        {
            get
            {
                if (string.IsNullOrEmpty(cdnVersion))
                {
                    cdnVersion = GetCDNVersion();
                }
                return cdnVersion;
            }
        }

        public static string SchemeAndHostURL
        {
            get
            {
                if (string.IsNullOrEmpty(schemeAndHostURL))
                {
                    schemeAndHostURL = GetSchemeAndHostURL();
                }
                return schemeAndHostURL;
            }
        }

        public static string AppBaseUrl
        {
            get
            {
                if (string.IsNullOrEmpty(appBaseUrl))
                {
                    appBaseUrl = GetAppBaseURL();
                }
                return appBaseUrl;
            }
        }

        public static string HostURL
        {
            get
            {
                if (string.IsNullOrEmpty(hostURL))
                {
                    hostURL = GetHostURL();
                }
                return hostURL;
            }
        }

        public static string DefaultThemeId
        {
            get
            {
                return defaultThemeId;
            }
        }

        public static int CountyCacheInterval
        {
            get
            {
                return countyCacheInterval;
            }
        }

        public static string ApplicationHomeUrl
        {
            get
            {
                return applicationHomeUrl;
            }
        }

        public static string AppName
        {
            get
            {
                return appName;
            }
        }

        public static string PurchasePathAppBaseURL
        {
            get
            {
                return purchasePathAppBaseURL;
            }
        }

        public static string PolicyCentreURL
        {
            get
            {
                return policyCentreURL;
            }
        }

        public static string PolicyCentreDashboardURL
        {
            get
            {
                return policyCentreDashboardURL;
            }
        }

        public static string DbKeyName
        {
            get
            {
                return dbKeyName;
            }
        }

        public static string DBSchemaName
        {
            get
            {
                return dbSchemaName;
            }
        }

        public static int DBCommandTimeOut
        {
            get
            {
                return dbCommandTimeOut;
            }
        }

        public static bool IsTestingPaymentGateway
        {
            get
            {
                return isTestingPaymentGateway;
            }
        }

        public static bool IsPaymentConfigEntry
        {
            get
            {
                return isPaymentConfigEntry;
            }
        }

        public static string PaymentAPIURL
        {
            get
            {
                return paymentAPIURL;
            }
        }

        public static string PaymentURLIP
        {
            get
            {
                return paymentURLIP;
            }
        }

        public static string PaymentURL
        {
            get
            {
                return paymentURL;
            }
        }

        public static string PaymentLogin
        {
            get
            {
                return paymentLogin;
            }
        }

        public static string PaymentTransactionKey
        {
            get
            {
                return paymentTransactionKey;
            }
        }

        public static bool EnableNLogTracing
        {
            get
            {
                return enableNLogTracing;
            }
        }

        public static bool EnableSessionObjectLogging
        {
            get
            {
                return enableSessionObjectLogging;
            }
        }

        public static bool EnableContentLogging
        {
            get
            {
                return enableContentLogging;
            }
        }

        public static int HowLongBeenInBusinessQuestionId
        {
            get
            {
                return howLongBeenInBusinessQuestionId;
            }
        }

        public static int IndustryCacheInterval
        {
            get
            {
                return industryCacheInterval;
            }
        }

        public static int SubIndustryCacheInterval
        {
            get
            {
                return subIndustryCacheInterval;
            }
        }

        public static int LineOfBusinessCacheInterval
        {
            get
            {
                return lineOfBusinessCacheInterval;
            }
        }

        public static int GoodAndBadStateCacheInterval
        {
            get
            {
                return goodAndBadStateCacheInterval;
            }
        }

        public static int MultipleStatesCacheInterval
        {
            get
            {
                return multipleStatesCacheInterval;
            }
        }

        public static string MultipleRiskIdErrorMessage
        {
            get
            {
                return multipleRiskIdErrorMessage;
            }
        }

        public static int AddNumberofMonthinXModExpiryDate
        {
            get
            {
                return addNumberofMonthinXModExpiryDate;
            }
        }

        public static string CloudTypographyUrl
        {
            get
            {
                return cloudTypographyUrl;
            }
        }

        public static string PolicyCreateDTOName
        {
            get
            {
                return policyCreateDTOName;
            }
        }

        public static string PolicyCreateDTOProperty
        {
            get
            {
                return policyCreateDTOProperty;
            }
        }

        public static bool RelaodLocalData
        {
            get
            {
                return relaodLocalData;
            }
        }

        public static string LocalStoredDataPath
        {
            get
            {
                return localStoredDataPath;
            }
        }

        #region Welcome Mail

        public static List<string> WelcomeEmailTo
        {
            get
            {
                return welcomeEmailTo;
            }
        }

        public static List<string> WelcomeEmailCc
        {
            get
            {
                return welcomeEmailCc;
            }
        }

        public static List<string> WelcomeEmailBcc
        {
            get
            {
                return welcomeEmailBcc;
            }
        }

        public static string WelcomeEmailFrom
        {
            get
            {
                return welcomeEmailFrom;
            }
        }

        #endregion

        #region Registration Mail

        public static List<string> RegistrationEmailTo
        {
            get
            {
                return registrationEmailTo;
            }
        }

        public static List<string> RegistrationEmailCc
        {
            get
            {
                return registrationEmailCc;
            }
        }

        public static List<string> RegistrationEmailBcc
        {
            get
            {
                return registrationEmailBcc;
            }
        }

        public static string RegistrationEmailFrom
        {
            get
            {
                return registrationEmailFrom;
            }
        }

        #endregion

        #region Payment Failure Mail

        public static List<string> PolicyCreationFailureIntimationTo
        {
            get
            {
                return policyCreationFailureIntimationTo;
            }
        }

        public static List<string> PolicyCreationFailureIntimationCc
        {
            get
            {
                return policyCreationFailureIntimationCc;
            }
        }

        public static List<string> PolicyCreationFailureIntimationBcc
        {
            get
            {
                return policyCreationFailureIntimationBcc;
            }
        }

        public static string PolicyCreationFailureIntimationFrom
        {
            get
            {
                return policyCreationFailureIntimationFrom;
            }
        }

        #endregion

        #region Password Reset Mail

        public static List<string> ResetPasswordEmailTo
        {
            get
            {
                return resetPasswordEmailTo;
            }
        }

        public static List<string> ResetPasswordEmailCc
        {
            get
            {
                return resetPasswordEmailCc;
            }
        }

        public static List<string> ResetPasswordEmailBcc
        {
            get
            {
                return resetPasswordEmailBcc;
            }
        }

        public static string ResetPasswordEmailFrom
        {
            get
            {
                return resetPasswordEmailFrom;
            }
        }

        #endregion

        #region Referral Mail

        public static List<string> ReferralEmailTo
        {
            get
            {
                return referralEmailTo;
            }
        }

        public static List<string> ReferralEmailCc
        {
            get
            {
                return referralEmailCc;
            }
        }

        public static List<string> ReferralEmailBcc
        {
            get
            {
                return referralEmailBcc;
            }
        }

        public static string ReferralEmailFrom
        {
            get
            {
                return referralEmailFrom;
            }
        }

        #endregion

        #region Retreive Quote Mail

        public static List<string> RetreiveQuoteEmailTo
        {
            get
            {
                return retreiveQuoteEmailTo;
            }
        }

        public static List<string> RetreiveQuoteEmailCc
        {
            get
            {
                return retreiveQuoteEmailCc;
            }
        }

        public static List<string> RetreiveQuoteEmailBcc
        {
            get
            {
                return retreiveQuoteEmailBcc;
            }
        }

        public static string RetreiveQuoteEmailFrom
        {
            get
            {
                return retreiveQuoteEmailFrom;
            }
        }

        #endregion

        #region Comment : Here application all different ClientEmailId's

        public static List<string> ClientEmailIdCreatePolicyFailure
        {
            get
            {
                return clientEmailIdCreatePolicyFailure;
            }
        }

        public static List<string> ClientEmailIdCancelPolicy
        {
            get
            {
                return clientEmailIdCancelPolicy;
            }
        }

        public static List<string> ClientEmailIdClaimRequest
        {
            get
            {
                return clientEmailIdClaimRequest;
            }
        }

        public static List<string> ClientEmailIdPolicyChangeRequest
        {
            get
            {
                return clientEmailIdPolicyChangeRequest;
            }
        }

        public static List<string> ClientEmailIdAddressChangeRequest
        {
            get
            {
                return clientEmailIdAddressChangeRequest;
            }
        }



        #endregion

        #endregion

        #region PC

        public static string BaseUrlPath
        {
            get
            {
                return baseUrlPath;
            }
        }

        public static string CdnDefaultDashboardFolder
        {
            get
            {
                return cdnDefaultDashboardFolder;
            }
        }

        public static string StaticFilePath
        {
            get
            {
                return staticFilePath;
            }
        }

        public static string UploadFiles
        {
            get
            {
                return uploadFiles;
            }
        }

        public static string WCUrl
        {
            get
            {
                return wcUrl;
            }
        }

        public static string WcAngularBaseModuleUrl
        {
            get
            {
                return wcAngularBaseModuleUrl;
            }
        }

        public static string CaptchaSiteKey
        {
            get
            {
                return captchaSiteKey;
            }
        }

        public static string CaptchaSecretKey
        {
            get
            {
                return captchaSecretKey;
            }
        }

        public static int QuoteExpiryDays
        {
            get
            {
                return quoteExpiryDays;
            }
        }
        public static int MinFileSize
        {
            get
            {
                return minFileSize;
            }
        }
        public static int MaxFileSize
        {
            get
            {
                return maxFileSize;
            }
        }
        public static int MaxFileNameChar
        {
            get
            {
                return maxFileNameChar;
            }
        }
        public static int MaxFileCount
        {
            get
            {
                return maxFileCount;
            }
        }

        public static int LoginAttempt
        {
            get
            {
                return loginAttempt;
            }
        }
        public static int UnlockAccountTime
        {
            get
            {
                return unlockAccountTime;
            }
        }
        public static string WcBaseUrlPath
        {
            get
            {
                return wcBaseUrlPath;
            }
        }

        #region Policy Change Mail

        public static List<string> PolicyChangeEmailTo
        {
            get
            {
                return policyChangeEmailTo;
            }
        }

        public static List<string> PolicyChangeEmailCc
        {
            get
            {
                return policyChangeEmailCc;
            }
        }

        public static List<string> PolicyChangeEmailBcc
        {
            get
            {
                return policyChangeEmailBcc;
            }
        }

        public static string PolicyChangeEmailFrom
        {
            get
            {
                return policyChangeEmailFrom;
            }
        }

        #endregion

        #region Policy Cancellation Mail

        public static List<string> PolicyCancellationEmailTo
        {
            get
            {
                return policyCancellationEmailTo;
            }
        }

        public static List<string> PolicyCancellationEmailCc
        {
            get
            {
                return policyCancellationEmailCc;
            }
        }

        public static List<string> PolicyCancellationEmailBcc
        {
            get
            {
                return policyCancellationEmailBcc;
            }
        }

        public static string PolicyCancellationEmailFrom
        {
            get
            {
                return policyCancellationEmailFrom;
            }
        }

        #endregion

        #region Address Change Mail

        public static List<string> AddressChangeEmailTo
        {
            get
            {
                return addressChangeEmailTo;
            }
        }

        public static List<string> AddressChangeEmailCc
        {
            get
            {
                return addressChangeEmailCc;
            }
        }

        public static List<string> AddressChangeEmailBcc
        {
            get
            {
                return addressChangeEmailBcc;
            }
        }

        public static string AddressChangeEmailFrom
        {
            get
            {
                return addressChangeEmailFrom;
            }
        }

        #endregion

        #region Claim Request Mail

        public static List<string> ClaimRequestEmailTo
        {
            get
            {
                return claimRequestEmailTo;
            }
        }

        public static List<string> ClaimRequestEmailCc
        {
            get
            {
                return claimRequestEmailCc;
            }
        }

        public static List<string> ClaimRequestEmailBcc
        {
            get
            {
                return claimRequestEmailBcc;
            }
        }

        public static string ClaimRequestEmailFrom
        {
            get
            {
                return claimRequestEmailFrom;
            }
        }

        #endregion
        #region Upload Documents Mail

        public static List<string> UploadDocumentsEmailTo
        {
            get
            {
                return uploadDocumentsEmailTo;
            }
        }

        public static List<string> UploadDocumentsEmailCc
        {
            get
            {
                return uploadDocumentsEmailCc;
            }
        }

        public static List<string> UploadDocumentsEmailBcc
        {
            get
            {
                return uploadDocumentsEmailBcc;
            }
        }

        public static string UploadDocumentsEmailFrom
        {
            get
            {
                return uploadDocumentsEmailFrom;
            }
        }

        #endregion

        #region Edit Contact Information Mail

        public static List<string> ContactInformationEmailTo
        {
            get
            {
                return contactInformationEmailTo;
            }
        }

        public static List<string> ContactInformationEmailCc
        {
            get
            {
                return contactInformationEmailCc;
            }
        }

        public static List<string> ContactInformationEmailBcc
        {
            get
            {
                return contactInformationBcc;
            }
        }

        public static string ContactInformationEmailFrom
        {
            get
            {
                return contactInformationEmailFrom;
            }
        }

        #endregion

        #endregion

        #region Transaction Log Flag

        public static bool IsTransactionLog
        {
            get
            {
                return Istransactionlog;
            }
        }

        public static string Transactionlogpath
        {
            get
            {
                return transactionlogpath;
            }
        }

        public static int TotalNumbeOfClaimInPastThreeYears
        {
            get
            {
                return totalNumbeOfClaimInPastThreeYears;
            }
        }

        public static string BopUrl
        {
            get
            {
                return bopUrl;
            }
        }

        public static List<string> AllowedFileTypes
        {
            get
            {
                return allowedFileTypes;
            }
        }

        public static string DefaultPassword
        {
            get
            {
                return defaultPassword;
            }
        }

        public static string DefaultSecureKey
        {
            get
            {
                return defaultSecureKey;
            }
        }

        public static string APILogFileName
        {
            get
            {
                return apiLogFileName;
            }
        }

        #endregion

        #region Live Chat

        public static string LCLicense
        {
            get
            {
                return lcLicense;
            }
        }

        public static string LCGroup
        {
            get
            {
                return lcGroup;
            }
        }

        public static string LCServerName
        {
            get
            {
                return lcServerName;
            }
        }

        public static string LCServerValue
        {
            get
            {
                return lcServerValue;
            }
        }

        public static string LCSrc
        {
            get
            {
                return lcSrc;
            }
        }

        #endregion

        #region APIProperties
        public static int APIAuthorizeExpiryMinutes
        {
            get
            {
                return authroizeAPIExpiryMinutes;
            }
        }
        #endregion

        #region Reattempt
        public static string ReattemptFolderPath
        {
            get
            {
                return reattemptFolderPath;
            }
        }
        public static string ReattemptFailureFolderPath
        {
            get
            {
                return reattemptFailureFolderPath;
            }
        }
        public static string ReattemptArchiveFolderPath
        {
            get
            {
                return reattemptArchiveFolderPath;
            }
        }


        #endregion

        #region Commercial Auto
        public static bool EnableCommercialAuto
        {
            get
            {
                return enableCommercialAuto;
            }
        }
        public static string CommercialAutoURL
        {
            get
            {
                return commercialAutoURL;
            }
        }

        public static string PolSOACode
        {
            get
            {
                return polSOACode;
            }
        }
        #endregion

        #region Landing Page

        public static string LandingPageAppBaseURL
        {
            get { return landingPageAppBaseURL; }
        }

        #endregion

        #region BOP
        public static bool EnableBOP
        {
            get
            {
                return enableBOP;
            }
        }
        #endregion

        #region Payment Gateway

        public static string CreditCardPayments_AuthorizeNET_InTesting
        {
            get
            {
                return creditCardPayments_AuthorizeNET_InTesting;
            }
        }

        public static string CreditCardPayments_AuthorizeNET_LoginID
        {
            get
            {
                return creditCardPayments_AuthorizeNET_LoginID;
            }
        }

        public static string CreditCardPayments_AuthorizeNET_transactionKey
        {
            get
            {
                return creditCardPayments_AuthorizeNET_transactionKey;
            }
        }

        #endregion

        #region Private Methods

        private static string GetCDNVersion()
        {
            string versionNo = "1000";
            string cdnVersionURL = string.Concat(HttpContext.Current.Request.Url.Scheme, "://", HttpContext.Current.Request.Url.Host, cdnPath, "Home/GetCDNVersion");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(cdnVersionURL);
            request.Method = "GET";

            //specify other request properties
            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                {

                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        versionNo = reader.ReadToEnd();
                        versionNo = versionNo.Remove(0, ("{\"cdnVersion\":\"").Length);
                        versionNo = versionNo.Substring(0, versionNo.IndexOf("\""));
                    }
                }

            }
            catch (WebException wex)
            {
                string pageContent = new StreamReader(wex.Response.GetResponseStream()).ReadToEnd().ToString();
                versionNo = "1000";
            }
            return versionNo;
        }

        private static string GetSchemeAndHostURL()
        {
            schemeAndHostURL = string.Concat(HttpContext.Current.Request.Url.Scheme, "://", HttpContext.Current.Request.Url.Host);
            return schemeAndHostURL;
        }

        private static string GetAppBaseURL()
        {
            appBaseUrl = string.Concat(GetSchemeAndHostURL(), HttpContext.Current.Request.Url.Host == "localhost" ? ConfigCommonKeyReader.PurchasePathAppBaseURL : "");
            return appBaseUrl;
        }

        private static string GetHostURL()
        {
            hostURL = HttpContext.Current.Request.Url.Host;
            return hostURL;
        }

        #endregion

        #region Google Analytics

        public static string GACode
        {
            get
            {
                return gaCode;
            }
        }

        #endregion


        #region NotificationTemplate
        public static string EmailTemplatePath
        {
            get
            {
                return emailTemplatePath;
            }
        }
       
        #endregion
        #endregion

        #region Private method
        /// <summary>
        /// Get list of string values based on supplied text and seprator
        /// </summary>
        /// <param name="text"></param>
        /// <param name="seprator"></param>
        /// <returns></returns>
        private static List<string> GetListOfSplitedValues(this string text, string seprator = ";")
        {
            return !string.IsNullOrEmpty(text.Trim()) && !string.IsNullOrEmpty(seprator.ToString().Trim()) ?
                text.Split(new string[] { seprator }, StringSplitOptions.RemoveEmptyEntries).ToList() : new List<string>();
        }
        #endregion
    }
}
