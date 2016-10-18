#region Using directives

using System;
using System.Collections.Generic;

#endregion

namespace BHIC.Common.Config
{
    public class Constants
    {
        #region LineOfBusiness
        public const string WC = "Workers' Compensation";
        public const string BOP = "Business Owner's Policy";
        #endregion

        #region API constants (Worker Compensation)

        public const string Address = "Address";
        public const string UserProfiles = "UserProfiles";
        public const string SystemVariables = "SystemVariables";
        public static class MethodType
        {
            public const string GET = "GET";
            public const string POST = "POST";
            public const string DELETE = "DELETE";
        }


        #region Landing Page constants

        public const string County = "Counties";

        #endregion

        #region Question Page constants

        public const string Questions = "Questions";
        public const string QuestionsHistory = "QuestionsHistory";

        #endregion

        #region Email Validation Constants

        public const string InvalidEmailParams = "To,From and Sender is required";

        #endregion

        #region Other

        public const string CancellationRequest = "CancellationRequests";
        public const string CertRequest = "CertRequests";
        public const string Contact = "Contacts";
        public const string CoverageState = "CoverageStates";
        public const string Document = "Documents";
        public const string Exposure = "Exposures";
        public const string InsuredName = "InsuredNames";
        public const string LobData = "LobData";
        public const string Location = "Locations";
        public const string Modifier = "Modifiers";
        public const string Officer = "Officers";
        public const string Payments = "Payments";
        public const string PaymentTerms = "PaymentTerms";
        public const string Phone = "Phones";
        public const string Quote = "Quotes";
        public const string RatingData = "RatingData";
        public const string States = "States";
        public const string PaymentPlans = "PaymentPlans";
        public const string ProductName = "Workers' Compensation";
        public const string PolicyCreate = "PolicyCreate";
        public const string UserPolicyCodes = "UserPolicyCodes";
        public const string PCQuoteInformation = "PCQuoteInformation";

        public static class ServiceNames
        {
            public const string BusinessTypes = "BusinessTypes";
            public const string Industries = "Industries";
            public const string SubIndustries = "SubIndustries";
            public const string ClassDescriptions = "ClassDescriptions";
            public const string ClassDescKeywords = "ClassDescKeywords";
            public const string ClassCodes = "ClassCodes";
            public const string ValidateExposureMinimumPayroll = "VExposuresMinPayroll";
            public const string QuoteStatus = "QuoteStatus";
            public const string PolicyData = "PolicyData";
            public const string CompanionClasses = "CompanionClasses";
        }

        public const string AvailableCarriers = "AvailableCarriers";
        public const string QuickQuote = "QuickQuote";
        public static class QuoteStatus
        {
            public static string Quote = "Quote";
            public static string HardReferral = "Hard Referral";
            public static string SoftReferral = "Soft Referral";
        }
        //Comment : Here added default sperator to split contents
        public const char DefaultMailIdsSeprator = ';';

        #endregion

        #region Messages

        public const string ServiceResponseNotRecieved = "Unable to recieve response from Service";

        #endregion

        #endregion

        #region API constant (Policy Centre Dashboard)
        public const string PolicyDocuments = "PolicyDocuments";
        public const string PolicyDetails = "PolicyDetails";
        public const string BillingSummary = "BillingSummary";
        public const string CancellationPolicy = "CancellationPolicy";
        public const string PhysicianPanels = "PhysicianPanels";
        public const string ViewDocument = "ViewDocument";
        public const string RequestCertificate = "CertificateOfInsurance";
        public const string ValidateUserPolicy = "VUserPoliciesValidPolicyCode";
        public const string Billing = "Billing";
        public const string CityStateZipcode = "VCityStateZipCode";


        //policy status constants
        public const string Active = "Active";
        public const string ActiveSoon = "Active Soon";
        public const string Expired = "Expired";
        public const string NoCoverage = "No Coverage";
        public const string Cancelled = "Cancelled";
        public const string PendingCancellation = "Pending Cancellation";


        //Table Names for dropdownoptions(any change here need to be change at stored procedure also)
        public const string PolicyChangeDropdown = "PolicyChange";
        public const string PolicyCancelDropdown = "PolicyCancel";


        #endregion

        #region API Constant (NCCI Provider)

        public const string XModRiskId = "GetModRiskId";
        public const string XModFein = "GetModFein";

        #endregion

        #region Cache Constants

        public const string CountyCache = "county";
        public const string IndustryCache = "industry";
        public const string SubIndustryCache = "subIndustry";
        public const string ClassCache = "class";
        public const string ClassDescriptionListCache = "ClassDescriptionList";
        public const int CountyCacheDuration = 1;
        public const string LineOfBusinessCache = "lineOfBusiness";
        public const string StateTypeCache = "stateType";
        public const string PrimaryClassCodeCache = "primaryClassCode";
        public const string MultipleStates = "multipleStates";
        public const string SystemVariableCache = "systemVariablesCache";
        public const string OAuthModelToken = "oAuthModelToken";

        /// <summary>
        /// Line Of Business
        /// </summary>
        public enum LineOfBusiness
        {
            WC,
            BOP
        }

        public static class LOB
        {
            public static string WC = "WC";
        }

        public enum PaymentConfirmationTypeEnum
        {
            ExistingPolicy,
            NewPolicy
        }

        #endregion

        #region Error Messages

        #region Database Errors
        public static class DatabaseErrors
        {
            public static string OPEN_CONNECTION = "Please open connection before performing any Transaction Activity";
            public static string START_TRANSACTION_COMMIT = "Please Start a Transaction before submitting any changes";
            public static string START_TRANSACTION_REVERT = "Please Start a Transaction before reverting any changes";
        }
        #endregion Database Errors

        #region Controller Errors

        #region Policy Centre Dashboard Errors
        public const string RegistrationFailed = "Your attempt to register for an account has failed. " +
                            "Please check the values you have entered  and try again. if you belive this is an error, click to chat below.";
        public const string ExceptionMessage = "It seems, something went wrong. Please verify all the values and try again. Sorry, for the inconvenience caused.";
        #endregion

        #region Quote Capture Errors
        public static class QuoteCaptureErrors
        {
            public static string SESSION_EXPIRED = "User session has been expired, unable to post exposure data !";
            public static string BUSINESS_INFO_EMPTY = "Business Information not provided";
            public static string EXPOSURE_SUBMISSION_FAILED = "Unable to post exposure and policy data";
        }

        public static class ExposureDataErrors
        {
            public static string INCORRECT_DATE = "Effective dates must be at least tomorrow and not more than 60 days out";
            public static string INVALID_DATE_FORMAT = "Date Format is not valid.";
            public static string BUSINESS_INFO_EMPTY = "Business Information not provided";
            public static string BOTH_SELECTION_NOT_ALLOWED = "Search by keyword and industry not allowed, simultaneously";
            public static string BUSINESS_INFO_ALTERED = "Business Information altered";
            public static string ANNUAL_PAYROLL_ALTERED = "Annual payroll amount is altered";
            public static string EXPOSURE_AMOUNT_EMPTY = "Annual Payroll is not provided";
            public static string INVALID_BUSINESSYEAR = "Business year provided is invalid";
            public static string EMPTY_BUSINESSYEAR = "Business is not provided";
            public static string MIN_AMOUNT_VALIDATION_MISMATCH = "Minimum payroll amount validation mismatch";
            public static string MSMC_DATA_NOT_SAME = "Multi Class Multi State Data is not same";
            public static string MSMC_VALIDATION_AMOUNT_GREATER = "Minimum Exposure Validation amount is greater than Primary Class Exposure Amount";
        }
        #endregion Quote Capture Errors

        #region Other controller errors

        public const string SessionExpired = "Session Expired";
        public const string SessionExpiredPartial = "Partial Views Session Expired";
        public const string UnauthorizedRequest = "Unauthorized Request";
        public const string UnauthorizedLandingRequest = "Unauthorized Landing Request";
        public const string AppCustomSessionExpired = "Application custom session has been expired";
        public const string QuoteIdCookieEmpty = "QuoteId does not exist";
        public const string QuoteDeleted = "Sorrt we are unable to retrive quote, due to link expired";
        #endregion

        #endregion Controller Errors

        #endregion Error Messages

        #region Enum Accessor Methods

        /// <summary>
        /// Gets Line of Business on the basis of LOB provided
        /// </summary>
        /// <param name="lob"></param>
        /// <returns></returns>
        public static string GetLineOfBusiness(LineOfBusiness lob)
        {
            switch (lob)
            {
                case LineOfBusiness.WC:
                    return "WC";
                case LineOfBusiness.BOP:
                    return "BOP";
                default:
                    return "WC";
            }
        }

        //Comment : Here used to set Language type for file download
        public enum LossControlLanguage
        {
            English,
            Spanish
        }

        #endregion Enum Accessor Methods

        #region Custom Session

        public const string CustomSession = "CustomSession";

        #endregion

        #region Payment Constants

        public const string FormName = "paymentForm";
        public const string SubmitButtonText = "Pay Now";
        public const string PaymentFailure = "An Error occured while processing request. Please try again";
        public const string FailedPaymentCode = "1";

        #endregion

        #region Logging Constants

        public enum LoggingType
        {
            Trace = 1
           ,
            Debug = 2
                ,
            Info = 3
                ,
            Warn = 4
                ,
            Error = 5
                , Fatal = 6
        }


        #endregion

        #region Server side form validation text messages

        #region QuoteSummary Page Constants

        public const string PaymentPlanDoesNotExists = "Selected Plan does not exist";

        #endregion

        #region Question Page Constants

        public const string InvalidResponse = "Please enter valid response type";
        public const string EmptyReponse = "Response is required";
        public const string InvalidQType = "Invalid question type";
        public const string InvalidQNumber = "Invalid question number";
        public const string QuestionValidationException = "Some question validation processing error occured";

        public const string CustomQuoteReferralMessage1 = "Mutiple risk-id condition found";
        public const string CustomQuoteReferralMessage2 = "Total number of claims in past three years condition found";
        public const string CustomQuoteReferralMessage3 = "Good state custom condition found";
        public const string CustomQuoteReferralMessage4 = "MS/MC custom condition found";
        public const string CustomQuoteReferralMessage5 = "Other Industry, SubIndustry or class description selection condition found";
        public const string CustomQuoteReferralMessage6 = "Referral only class condition found";
        public const string CustomQuoteReferralMessage7 = "Multistate condition found.";
        public const string CustomQuoteReferralMessage8 = "Other Industry, SubIndustry or class description selection and multistate condition found.";
        #endregion

        #region PurchaseQuote Page Constants

        public const string ModelStateError = "Model State is empty";
        public const string EmptyAccount = "Account is empty";
        public const string EmptyEmail = "Please enter your contact email id";
        public const string EmptyConfirmEmail = "Please confirm email address";
        public const string EmptyPassword = "Please provide password";
        public const string EmptyConfirmPassword = "Please provide confirm password";
        public const string InvalidPasswordLength = "Minimum password length should more than 8 digits";
        public const string InvalidEmail = "Please provide valid email";
        public const string InvalidPassword = "Please provide valid password";
        public const string PasswordAndConfirmPasswordMismatch = "Password and Confirm Passowrd do not match";
        public const string EmptyMailingAddress = "Please provide mailing address";
        public const string EmptyMailingAddress1 = "Please provide Mailing address";
        public const string EmptyMailingCity = "Please provide Mailing city";
        public const string EmptyMailingState = "Please provide Mailing state";
        public const string EmptyMailingZip = "Please provide Mailing zip";
        public const string InvalidCityStateZip = "Invalid city,state and zip combination";
        public const string EmptyFirstName = "Please provide first name";
        public const string EmptyLastName = "Please provide last name";
        public const string EmptyPhoneNumber = "Please provide phone number";
        public const string InvalidPhoneNumber = "Please enter your contact phone number";
        public const string EmptyBusinessInfo = "Business info is empty";
        public const string EmptyBusinessName = "Please enter your business name";
        public const string EmptyBusinessType = "Please provide Business type";
        public const string EmptyTaxIdOrSSN = "Please provide TaxId or SSN";
        public const string InvalidTaxIdOrSSN = "Invalid TaxId or SSN";
        public const string EmptyPersonalContact = "Please provide Personal contact";
        public const string InvalidFirstNameLength = "First name length should not more than 50";
        public const string InvalidLastNameLength = "Last name length should not more than 50";
        public const string InvalidFirstName = "First name is invalid";
        public const string InvalidLastName = "Last name is invalid";
        public const string PasswordNotExists = "User password does not exist";
        public const string EmailNotExists = "Email does not exist";
        public const string EmptySUIN = "Please provide SUIN number";
        public const string InvalidSUIN = "SUIN number is not valid";
        public const string EmptyContactName = "Please enter your contact name";


        #region Common Regex

        //regex for user name validation
        public const string UserNameRegex = @"^([A-Za-z\s]{1,}[\.]{0,1}[\']{0,1}[A-Za-z\s]{0,}){0,50}$";

        //regex for email validation
        public const string EmailRegex = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

        //regex for password
        public const string PasswordRegex = @"^(?=.{8,})(?=.*?[A-Z])(?=.*?[a-z])(?=.*?\d)(?=.*?\W).*$";

        //regex for phone number
        public const string PhoneRegexOld = @"^(\(?[0-9]{3}\)?\(?[0-9]{3}\)?-?\(?[0-9]{4}\)?)+(\s?x?\(?[0-9_]{0,4}\)?)?$";
        public const string PhoneRegex = @"^([0-9]{3}?-?[0-9]{3}?-?[0-9]{4}?)+(\s?x?[0-9_]{0,4}?)?$";

        //regex for phone number without ext
        public const string PhoneRegexWithoutExt = @"^\(?([0-9]{3})\)?[-.●]?([0-9]{3})[-.●]?([0-9]{4})$";

        //regex for numeric value
        public const string NumericRegex = @"^\d+$";

        //regex for policy code
        public const string PolicyCodeRegex = @"^[a-zA-Z0-9]*$";

        public const string PhoneLengthRegex = @"^.{10,}$";

        #endregion

        #endregion

        #region Payment Page Constants

        public const string InvalidPaymentPlan = "Current plan is not available, Please try different one";
        public const string EmptyPaymentPlan = "Please select plan from list";

        #endregion

        #endregion

        #region Application/Website Communication/Contact Details

        public static class WebsiteContactDetail
        {
            public static string CompanyName = "CoverYourBusiness.com";
            public static string WebsiteUrlText = "CoverYourBusiness.com";
            public static string SupportEmailText = "Support@CoverYourBusiness.com";
            public static string SupportEmailHref = "mailto:Support@CoverYourBusiness.com";
            public static string SupportPhoneNumber = "844-472-0967";
            public static string SupportPhoneNumberHref = "tel:8444720967";
        }

        #endregion

        #region UI Constants

        public static int OtherDescriptionId = -1;
        public static string OtherDescription = "Other";
        public static List<string> AllowedDomains = new List<string>() { 
        "xceedance.com",
        "guard.com",
        "coveryourbusiness.com"
        };
        #endregion UI Constants
    }
}
