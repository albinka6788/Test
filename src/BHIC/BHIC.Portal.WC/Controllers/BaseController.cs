using BHIC.Common;
using BHIC.Common.Client;
using BHIC.Common.Config;
using BHIC.Common.Configuration;
using BHIC.Common.DataAccess;
using BHIC.Common.Logging;
using BHIC.Common.Quote;
using BHIC.Common.XmlHelper;
using BHIC.Contract.Background;
using BHIC.Contract.Mailing;
using BHIC.Contract.Policy;
using BHIC.Contract.PurchasePath;
using BHIC.Core.Background;
using BHIC.Core.Mailing;
using BHIC.Core.Policy;
using BHIC.Core.PurchasePath;
using BHIC.DML.WC.DataContract;
using BHIC.DML.WC.DataService;
using BHIC.DML.WC.DTO;
using BHIC.Domain.Background;
using BHIC.Domain.Dashboard;
using BHIC.Domain.Policy;
using BHIC.Domain.Service;
using BHIC.ViewDomain.Landing;
using BHIC.ViewDomain.Mailing;
using BHIC.ViewDomain.QuestionEngine;
using MoreLinq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BHIC.Portal.WC
{
    public class BaseController : Controller
    {
        internal ILoggingService loggingService = LoggingService.Instance;
        internal ServiceProvider guardServiceProvider = new GuardServiceProvider() { ServiceCategory = ServiceProviderConstants.GuardServiceCategoryWC };

        protected static string LineOfBusiness = "WC";
        internal List<string> mailRecipient = new List<string>() { "prem.pratap@xceedance.com" };
        internal static string angularBaseModuleUrl = "PurchasePath/Quote/Index#";

        #region Constructors

        public BaseController() { }

        #endregion

        #region Helper Methods

        public static string GetAssemblyVersion()
        {
            //Version webAssemblyVersion = BuildManager.GetGlobalAsaxType().BaseType.Assembly.GetName().Version;
            //return string.Concat("?v=", webAssemblyVersion.Major, webAssemblyVersion.Minor, webAssemblyVersion.Build, webAssemblyVersion.MinorRevision);
            return string.Concat("_", ConfigCommonKeyReader.CdnVersion);
        }

        /// <summary>
        /// Get user/visitor IP address
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected string GetUser_IP(HttpContextBase context)
        {
            return UtilityFunctions.GetUserIPAddress(context.ApplicationInstance.Context);

            //string VisitorsIPAddr = string.Empty;
            //if (context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            //{
            //    VisitorsIPAddr = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            //}
            //else if (context.Request.UserHostAddress.Length != 0)
            //{
            //    VisitorsIPAddr = context.Request.UserHostAddress;
            //}
            //return VisitorsIPAddr;
        }

        /// <summary>
        /// Return list of all errors of ViewModel
        /// Generating all ViewModel errors collection
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        protected object GetModelAllErrors(ModelStateDictionary modelState)
        {

            var errorList = modelState.ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );


            return errorList;
        }

        /// <summary>
        /// Get request base complete url path details
        /// </summary>
        /// <returns></returns>
        protected string Helper_GetBaseUrl()
        {
            string baseUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            return baseUrl;
        }

        /// <summary>
        /// Indicates whether CustomSession is null 
        /// </summary>
        /// <returns>returns true if current CustomSession object is null, false otherwise</returns>
        protected bool IsCustomSessionNull()
        {
            return Session["CustomSession"].IsNull();
        }

        /// <summary>
        /// It will return CustomSession object
        /// </summary>
        /// <returns>returns CustomSession object if exists, otherwise null object</returns>
        protected BHIC.ViewDomain.CustomSession GetCustomSession()
        {
            BHIC.ViewDomain.CustomSession customSession = null;

            //if CustomSession is not null assign value in object
            if (!IsCustomSessionNull())
            {
                customSession = (BHIC.ViewDomain.CustomSession)Session["CustomSession"];

                if (ConfigCommonKeyReader.EnableSessionObjectLogging)
                {
                    loggingService.Trace(string.Format("Custom Session called from '{0}'{1}Custom Session valuue '{2}'",
                                HttpContext.Request.Url.ToString(), Environment.NewLine, Newtonsoft.Json.JsonConvert.SerializeObject(customSession)));
                }

                return customSession;
            }
            else
            {
                if (ConfigCommonKeyReader.EnableSessionObjectLogging)
                {
                    loggingService.Trace("Custom session not available");
                }
                customSession = new ViewDomain.CustomSession();
            }

            return customSession;
        }

        /// <summary>
        /// It will set Custom Session object in session
        /// </summary>
        /// <returns>returns true when successfully stored in session</returns>
        protected bool SetCustomSession(BHIC.ViewDomain.CustomSession customSession)
        {
            try
            {
                Session["CustomSession"] = customSession;

                return true;
            }
            catch (Exception ex)
            {
                //create error log, in case of exception
                loggingService.Fatal(string.Format("Service {0} Call with error message : {1}", Constants.CustomSession, ex.ToString()));
                throw;
            }
        }

        /// <summary>
        /// Get Custom Session With QuoteVM data
        /// </summary>
        /// <returns></returns>
        protected BHIC.ViewDomain.CustomSession GetCustomSessionWithQuoteVM()
        {
            BHIC.ViewDomain.CustomSession customSession = GetCustomSession();

            if (customSession.QuoteVM.IsNull())
            {
                customSession.QuoteVM = new QuoteViewModel();
            }
            return customSession;
        }

        /// <summary>
        /// Get Custom Session With QuoteVM data
        /// </summary>
        /// <returns></returns>
        protected BHIC.ViewDomain.CustomSession GetLobDataAndCoverageStateIds()
        {
            BHIC.ViewDomain.CustomSession customSession = GetCustomSessionWithQuoteVM();
            if (customSession.QuoteVM.LobDataIds.IsNull())
            {
                customSession.QuoteVM.LobDataIds = new List<int>();
            }
            if (customSession.QuoteVM.CoverageStateIds.IsNull())
            {
                customSession.QuoteVM.CoverageStateIds = new List<int>();
            }
            return customSession;
        }

        /// <summary>
        /// Get Custom Session With QuoteVM policyData data
        /// </summary>
        /// <returns></returns>
        protected BHIC.ViewDomain.CustomSession GetCustomSessionWithPolicyData()
        {
            BHIC.ViewDomain.CustomSession customSession = GetCustomSessionWithQuoteVM();
            if (customSession.QuoteVM.PolicyData.IsNull())
            {
                customSession.QuoteVM.PolicyData = new PolicyData();
            }

            return customSession;
        }

        /// <summary>
        /// Get Custom Session With QuoteVM Exposures List
        /// </summary>
        /// <returns></returns>
        protected BHIC.ViewDomain.CustomSession GetCustomSessionWithQuoteVMExposuresList()
        {
            BHIC.ViewDomain.CustomSession customSession = GetCustomSessionWithQuoteVM();
            if (customSession.QuoteVM.Exposures.IsNull())
            {
                customSession.QuoteVM.Exposures = new List<Exposure>();
            }
            return customSession;
        }

        /// <summary>
        /// Get Custom Session With PurchaseVM Exposures List
        /// </summary>
        /// <returns></returns>
        protected BHIC.ViewDomain.CustomSession GetCustomSessionWithPurchaseVM()
        {
            BHIC.ViewDomain.CustomSession customSession;
            if (!IsCustomSessionNull())
            {
                customSession = GetCustomSession();
            }
            else
            {
                customSession = new BHIC.ViewDomain.CustomSession();
            }
            if (customSession.PurchaseVM.IsNull())
            {
                customSession.PurchaseVM = new WcPurchaseViewModel();
            }
            return customSession;
        }

        /// <summary>
        /// Returns decrypted QuoteId from Url
        /// </summary>
        /// <param name="quoteId"></param>
        /// <returns></returns>
        protected int GetDecryptedQuoteId(string quoteId)
        {
            if (!string.IsNullOrWhiteSpace(quoteId))
            {
                return Convert.ToInt32(Encryption.DecryptText(Server.UrlDecode(quoteId)));
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Validate zipCode using county service
        /// </summary>
        /// <param name="zip"></param>
        /// <returns></returns>
        protected bool IsZipExists(string zip)
        {
            return string.IsNullOrWhiteSpace(zip) ? false : GetCountyData(zip).Any();
        }

        /// <summary>
        /// Validate Zip and State combination using county service
        /// </summary>
        /// <param name="zip"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        protected bool IsValidZipStateCombination(string zip, string state)
        {
            return (string.IsNullOrWhiteSpace(state) || string.IsNullOrWhiteSpace(zip)) ? false : GetCountyData(zip, state).Any();
        }

        /// <summary>
        /// Validate City,State and Zip combination using county service
        /// </summary>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="zip"></param>
        /// <returns></returns>
        protected bool IsValidCityStateZipCombination(string city, string state, string zip)
        {
            return (string.IsNullOrWhiteSpace(city) || string.IsNullOrWhiteSpace(state) || string.IsNullOrWhiteSpace(zip)) ? false : GetCountyData(zip, state, city).Any();
        }

        /// <summary>
        /// Get list of cities filterd by state and zip
        /// </summary>
        /// <param name="zip"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        protected List<County> GetAllCitiesByStateAndZip(string zip, string state)
        {
            return (string.IsNullOrWhiteSpace(zip) || string.IsNullOrWhiteSpace(state)) ? new List<County>() : GetCountyData(zip, state, null, false);
        }

        /// <summary>
        /// Get All State by zip code
        /// </summary>
        /// <param name="zip"></param>
        /// <returns></returns>
        protected List<ZipCodeStates> GetAllStateByZip(string zip)
        {
            var states = new List<ZipCodeStates>();
            zip = zip.Trim();

            if (!string.IsNullOrEmpty(zip))
            {
                //fetch all states from county by zip code
                var county = GetCountyData(zip);

                if (county != null && county.Count > 0)
                {
                    states.Add(new ZipCodeStates { ZipCode = county.FirstOrDefault().ZipCode, StateCode = county.FirstOrDefault().State });
                }
            }

            return states;
        }

        /// <summary>
        /// Get County data
        /// </summary>
        /// <param name="zip"></param>
        /// <param name="state"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        private List<County> GetCountyData(string zip = null, string state = null, string city = null, bool isDistinct = true)
        {
            ICountyService countyService = new CountyService(guardServiceProvider);

            //return county list filtered by query parameter
            var countyData = countyService.GetCounty(false);

            var county = new List<County>();

            if (countyData != null && countyData.ToList().Count() > 0)
            {
                var filteredList = new List<County>();
                if (!(string.IsNullOrEmpty(zip) && string.IsNullOrEmpty(state) && string.IsNullOrEmpty(city)))
                {
                    if (!countyData.Any(x => ((string.IsNullOrEmpty(zip) || (!string.IsNullOrEmpty(zip) && x.ZipCode.Equals(zip))) &&
                                    (string.IsNullOrEmpty(state) || (!string.IsNullOrEmpty(state) && x.State.Equals(state, StringComparison.OrdinalIgnoreCase))) &&
                                    (string.IsNullOrEmpty(city) || (!string.IsNullOrEmpty(city) && x.City.Equals(city, StringComparison.OrdinalIgnoreCase))))))
                    {
                        return county;
                    }

                    filteredList = countyData.Where(x => ((string.IsNullOrEmpty(zip) || (!string.IsNullOrEmpty(zip) && x.ZipCode.Equals(zip))) &&
                                (string.IsNullOrEmpty(state) || (!string.IsNullOrEmpty(state) && x.State.Equals(state, StringComparison.OrdinalIgnoreCase))) &&
                                (string.IsNullOrEmpty(city) || (!string.IsNullOrEmpty(city) && x.City.Equals(city, StringComparison.OrdinalIgnoreCase))))).ToList();
                }
                else
                {
                    filteredList = countyData.ToList();
                }

                if (isDistinct)
                {
                    county = filteredList.DistinctBy(x => x.State).ToList();
                }
                else
                {
                    county = filteredList.ToList();
                }
            }

            return county;
        }


        /// <summary>
        /// Retuen generic DbConnector object to communicate with database
        /// </summary>
        /// <returns></returns>
        private BHICDBBase GetDbConnector()
        {
            BHICDBBase dbConnector = new BHICDBBase();

            #region Comment : Here using XmlReader get DB connection string

            dbConnector.DBName = "GuinnessDB";

            #endregion

            //dbConnector.DBConnectionString = DbConnectionString;
            //dbConnector.CreateDBObjects(Providers.SqlServer);
            //dbConnector.CreateConnection();

            return dbConnector;
        }

        protected BHIC.ViewDomain.CustomSession RetrieveSavedQuote(string encryptedMailLinkIdentifier, BHIC.Contract.PurchasePath.ICommonFunctionality commonFunctionality, out int quoteId)
        {
            //Comment : here inititlize default
            BHIC.ViewDomain.CustomSession appSession = null;
            quoteId = 0;

            #region Comment : Here if request query string is found(means request came from mail embedded link) then do following action

            try
            {
                #region Comment : Here if QuoteId found in query string then 1.decrypt and get QuoteId 2. Retrieve quote saved state

                //Based on this get saved state of quote from database
                if (!string.IsNullOrEmpty(encryptedMailLinkIdentifier))
                {
                    var encryptedString = BHIC.Common.Encryption.DecryptText(encryptedMailLinkIdentifier);

                    //var quoteId = encryptedMailLinkQuoteId;
                    if (!string.IsNullOrEmpty(encryptedString))
                    {
                        var splitedValues = encryptedString.Split(';');
                        var loggedUserDetails = GetLoggedInUserDetails();
                        var loggedUserId = GetLoggedUserId() ?? 1;

                        //CASE - 1 : if user was not logged in then simply do as followed
                        //CASE - 2 : When user will come from MailedLink then get both quote and user id from encypted string and must check if logged in user is different and user id is difference                        
                        //CASE - 3 : if user was not logged in but provided email-id which is basically "Registered/ActiveUser" in PC then check "MailedEmailId" with "LoggedUserEmailId" if while retrieving quote with "LoggedInUser" details
                        //CASE - 4 : if user was logged in while doing "SaveForLater" but during "QuoteRetrieval(From MailedLink)" user is not LoggedIn then
                        if (splitedValues.Length == 3 || (splitedValues.Length == 2))
                        {
                            var quoteIdString = string.Empty;
                            var userIdString = string.Empty;
                            string userEmailId = string.Empty;
                            int userId = 0;

                            #region Get QuoteId,UserId in INT format

                            quoteIdString = splitedValues[0];
                            userIdString = splitedValues[1];

                            //Convert values
                            quoteId = Convert.ToInt32(quoteIdString);
                            userId = Convert.ToInt32(userIdString);

                            #endregion

                            //Comment : Here For all new RetrievalLink will contain "EmailId" then get it for OLD mails dont check for this
                            if (splitedValues.Length == 3)
                            {
                                userEmailId = splitedValues[2];
                            }

                            //Comment : Here call BLL to make this data stored in DB (Eigther NotLoggedIn or LoggedIn and matching with mailed user-id)
                            if (quoteId > 0 && userId >= 1 &&
                                (
                                    userId == loggedUserId ||
                                    userEmailId.Equals(!loggedUserDetails.IsNull() ? loggedUserDetails.Email : string.Empty, StringComparison.OrdinalIgnoreCase) ||
                                    (loggedUserId == 1 && userId > 1)   //CASE - 4
                                )
                              )
                            {
                                appSession = commonFunctionality.GetDeserializedCustomSession(commonFunctionality.GetApplicationCustomSession(quoteId, userId));
                            }

                            //if (!(loggedUserId > 1) || (loggedUserId > 1 && userId == loggedUserId))
                            //{
                            //    appSession = commonFunctionality.GetDeserializedCustomSession(commonFunctionality.GetApplicationCustomSession(quoteId, userId));
                            //}
                        }
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                loggingService.Fatal(ex);
            }

            return appSession;

            #endregion
        }

        protected BHIC.ViewDomain.CustomSession RetrieveSavedQuote_OLD(string encryptedMailLinkIdentifier, BHIC.Contract.PurchasePath.ICommonFunctionality commonFunctionality, out int quoteId)
        {
            //Comment : here inititlize default
            BHIC.ViewDomain.CustomSession appSession = null;
            quoteId = 0;

            #region Comment : Here if request query string is found(means request came from mail embedded link) then do following action

            try
            {
                #region Comment : Here if QuoteId found in query string then 1.decrypt and get QuoteId 2. Retrieve quote saved state

                //Based on this get saved state of quote from database
                if (!string.IsNullOrEmpty(encryptedMailLinkIdentifier))
                {
                    var encryptedString = BHIC.Common.Encryption.DecryptText(encryptedMailLinkIdentifier);

                    //var quoteId = encryptedMailLinkQuoteId;
                    if (!string.IsNullOrEmpty(encryptedString))
                    {
                        var splitedValues = encryptedString.Split(';');
                        var loggedUserId = GetLoggedUserId() ?? 1;

                        //Comment : Here if array has values
                        //CASE - 1 : When user will come from MailedLink then get both quote and user id from encypted string and must check if logged in user is different and user id is difference
                        //CASE - 2 : if user is not logged in then simply do as followed
                        if (splitedValues.Length == 2 || (splitedValues.Length == 1 && loggedUserId > 1))
                        {
                            var quoteIdString = string.Empty;
                            var userIdString = string.Empty;
                            int userId = 0;

                            //Comment : Here If user come from mailed link then 
                            if (splitedValues.Length == 2)
                            {
                                quoteIdString = splitedValues[0];
                                userIdString = splitedValues[1];

                                //Convert values
                                quoteId = Convert.ToInt32(quoteIdString);
                                userId = Convert.ToInt32(userIdString);
                            }
                            else if (splitedValues.Length == 1 && loggedUserId > 1)
                            {
                                //Comment : Here Important and must check is the mailed link user-id is same with logged user-id then only allow user to get 
                                quoteIdString = splitedValues[0];

                                //Convert values
                                quoteId = Convert.ToInt32(quoteIdString);
                                userId = loggedUserId;
                            }

                            //Comment : Here call BLL to make this data stored in DB (Eigther NotLoggedIn or LoggedIn and matching with mailed user-id)
                            if (!(loggedUserId > 1) || (loggedUserId > 1 && userId == loggedUserId))
                            {
                                appSession = commonFunctionality.GetDeserializedCustomSession(commonFunctionality.GetApplicationCustomSession(quoteId, userId));
                            }
                        }
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                loggingService.Fatal(ex);
            }

            return appSession;

            #endregion
        }

        /// <summary>
        /// Return interface refernce to make all business logic
        /// </summary>
        /// <returns></returns>
        protected ICommonFunctionality GetCommonFunctionalityProvider()
        {
            #region Comment : Here Questionnaire interface refernce to do/make process all business logic

            ICommonFunctionality commonFunctionality = new CommonFunctionality();

            #endregion

            return commonFunctionality;
        }

        /// <summary>
        /// Return combined Scheme and Host as single value.
        /// </summary>
        /// <returns></returns>
        protected string GetSchemeAndHostURLPart()
        {
            return string.Concat(this.HttpContext.Request.Url.Scheme, "://", this.HttpContext.Request.Url.Host);
        }

        /// <summary>
        /// Removes all application current active session and cookies
        /// </summary>
        protected void RemoveAllAppSessionAndCookie(bool maintainPcSession = false)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            #region Comment : Here Handling of PC user Session (maintainnng this session except all other sessions)

            UserRegistration userSession = null;
            if (maintainPcSession && Session["user"] != null)
            {
                userSession = Session["user"] as UserRegistration;
            }

            #endregion

            Session.RemoveAll();
            Session.Clear();

            //clean application Custom session object
            Session["CustomSession"] = null;
            Session["user"] = null;

            #region Comment : Here Handling of PC user Session (maintainnng this session except all other sessions)

            if (maintainPcSession && userSession != null && userSession.Id > 0)
            {
                Session["user"] = userSession;
            }

            #endregion

            //finally clear current quote-id cookie also
            if (!maintainPcSession)
            {
                QuoteCookieHelper.Cookie_DeleteTokenId(this.ControllerContext.HttpContext);
            }
        }

        #region PolicyCentre logged user handling methods for purchase path flow

        /// <summary>
        /// Retrun polict center or dashboard logged user details
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected UserRegistration GetPolicyCenterUserDetail(HttpContextBase context)
        {
            string pcUserEncrypted = QuoteCookieHelper.Cookie_GetPcUserId(context);
            if (pcUserEncrypted != string.Empty)
            {
                UserRegistration user = JsonConvert.DeserializeObject<UserRegistration>(pcUserEncrypted);
                //QuoteCookieHelper.Cookie_DeletePcUserId(context);
                return user;
            }

            return new UserRegistration();
        }

        /// <summary>
        /// Validate user coming through PolicyCentre flow
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected bool ValidatePolicyCenterLoggedUser(HttpContextBase context)
        {
            var userDetail = GetPolicyCenterUserDetail(context);
            if (userDetail != null && userDetail.Id > 0)
            {
                AssignPolicyCenterLoggedUserInSession(userDetail);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Assign this user in running application custom session 
        /// </summary>
        /// <param name="user"></param>
        protected void AssignPolicyCenterLoggedUserInSession(UserRegistration user)
        {
            if (user != null && user.Id > 0)
            {
                //Comment : Here finally store this logged user details into application custom session for future refernces
                Session["user"] = user;
            }
        }

        protected int? GetLoggedUserId()
        {
            return (Session["user"] != null && ((UserRegistration)Session["user"]).Id > 0) ? ((UserRegistration)Session["user"]).Id : (int?)null;
        }

        /// <summary>
        /// Get Logged in User Detail
        /// </summary>
        /// <returns></returns>
        protected UserRegistration GetLoggedInUserDetails()
        {
            if (!Session["user"].IsNull())
                return (UserRegistration)Session["user"];
            else
                return null;
        }

        #endregion

        #region Mail helper methods

        /// <summary>
        /// Method will send mail to user with embedded link to re-invock the current user state in application
        /// </summary>
        /// <param name="pageMethod"></param>
        /// <param name="quoteId"></param>
        /// <returns></returns>
        public bool SendMailSaveForLater(string pageMethod, int quoteId, string emailId)
        {
            #region Comment : Here create mail embedded link

            string baseUrl = ConfigCommonKeyReader.PurchasePathAppBaseURL;
            baseUrl = baseUrl.Replace("PurchasePath/", "");

            ICaptureQuote captureQuote = new CaptureQuote();
            int userId = captureQuote.GetQuoteUserID(quoteId.ToString());
            if (userId < 1)
            {
                // userId = (userId ?? (GetLoggedUserId() ?? 1));
                userId = GetLoggedUserId() ?? 1;
            }
            //Comment : Here encrypt quoteId query-string 
            string quoteQueryString = Encryption.EncryptText(string.Format("{0};{1};{2}", quoteId.ToString(), userId, emailId));

            //Comment : Here as per new requirement raised by GURU on 18.02.2016 always create saveforlater link to eposure page
            //pageMethod = "ModifyQuote";
            //pageMethod = "PurchasePath/Quote/Index#/GetExposureDetails"; //Old Line
            pageMethod = "PurchasePath/Quote/Index#/GetBusinessInfo";

            //string subUrl = string.Format("{0}/{1}/{2}", angularBaseModuleUrl, pageMethod, Server.UrlEncode(quoteQueryString));
            string subUrl = string.Format("{0}/{1}", pageMethod, Server.UrlEncode(quoteQueryString));

            #endregion

            #region Comment : Here prepare mail object model using ThemeManager and shared pre-defined templates

            /*Dictionary<string, object> mailContentModel = new Dictionary<string, object>();

            //mailContentModel.Add("anchorText", ThemeManager.ThemeSharedContent("useremail-saveforlater-anchortext") ?? null);
            var reviewQuoteText = ThemeManager.ThemeSharedContent("useremail-saveforlater-anchortext") ?? string.Empty;
            mailContentModel.Add("saveforlaterMailBody", ThemeManager.ThemeSharedContent("useremail-saveforlater-body") ?? null);
            mailContentModel.Add("saveforlaterMailSubject", ThemeManager.ThemeSharedContent("useremail-saveforlater-subject") ?? null);
            */
            #endregion

            #region Comment : Here prepare template populated with data and then send mail to user

            ServiceProvider guardServiceProvider = new GuardServiceProvider() { ServiceCategory = ServiceProviderConstants.GuardServiceCategoryWC };
            ISystemVariableService systemVariable = new SystemVariableService(guardServiceProvider);

            var model = new SaveForLaterMailViewModel
            {
                //Basic communication/contact details
                CompanyName = ConfigCommonKeyReader.ApplicationContactInfo["CompanyName"],
                WebsiteUrlText = systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_Domain"]),
                WebsiteUrlHref = systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_WebsiteShortURL"]),
                SupportEmailText = ConfigCommonKeyReader.ApplicationContactInfo["SupportEmailTextQuotes"],
                SupportEmailHref = ConfigCommonKeyReader.ApplicationContactInfo["SupportEmailHrefQuotes"],
                SupportPhoneNumber = systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["PhoneNumber_CSR"]),
                SupportPhoneNumberHref = string.Concat("tel:", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["PhoneNumber_CSR"])),
                Physical_Address2 = systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Physical_Address2"]),
                Physical_AddressCSZ = systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Physical_AddressCSZ"]),
                AbsoulteURL = CDN.GetEmailImageUrl(),

                //SaveForLater review-quote link details
                ReviewQuoteText = "",
                ReviewQuoteHref = string.Format("{0}{1}{2}", GetSchemeAndHostURLPart(), baseUrl, subUrl),
                QuoteId = quoteId.ToString()
            };

            //Comment : Here build template and send mail
            /*MailTemplateBuilder buildTemplate = new MailTemplateBuilder();
            var mailMsg = buildTemplate.MailSaveForLater(model, mailContentModel);

            //Comment : Here assign recipeint address
            mailMsg.RecipEmailAddr = new List<string>() { emailId };

            mailMsg.Cc = (ConfigCommonKeyReader.RetreiveQuoteEmailCc);
            mailMsg.Bcc = (ConfigCommonKeyReader.RetreiveQuoteEmailBcc);
            mailMsg.SenderEmailAddr = ConfigCommonKeyReader.RetreiveQuoteEmailFrom;*/

            #endregion

            #region Comment : Here accordingly latest business requirement Clear/Expire all application session like "user, CustomSession"

            //Send user mail
            //var mailSentStatus = mailMsg != null ? buildTemplate.SendMail(mailMsg) : false;
            IMailingService mailingService = new MailingService();

            var mailSentStatus = mailingService.SaveForLater(emailId, model);

            //Background mail should not be sent to user, if user opt for save for later option on payment page
            if (mailSentStatus)
            {
                IBackgroundProcessDataProvider provider = new BackgroundProcessDataProvider();
                provider.UpdateSaveForLaterMailStatus(userId);
            }

            #region Comment : Here Expire session

            //Forcefully expire current active user session in application and redirect user to Home screen and 
            RemoveAllAppSessionAndCookie(true);

            #endregion

            #endregion

            return mailSentStatus;
        }

        /// <summary>
        /// Send info about scheduled call through mail, to internal team
        /// </summary>
        /// <param name="requestName"></param>
        /// <param name="requestTime"></param>
        /// <param name="requestedCallTime"></param>
        /// <param name="phoneNumber"></param>
        /// <returns>return true on success, false otherwise</returns>
        public bool SendMailScheduleCall(string requestName, DateTime requestTime, DateTime requestedCallTime, string phoneNumber, int quoteId)
        {
            IMailingService mailingService = new MailingService();
            return mailingService.ScheduleCall(
                new ScheduleCallViewModel
                {
                    RequesterName = requestName,
                    RequestTime = DateTime.Now,
                    RequestedCallTime = requestedCallTime,
                    PhoneNumber = phoneNumber,
                    QuoteId = quoteId,
                    AbsoulteURL = CDN.GetEmailImageUrl(),
                });
        }

        #endregion

        //Todo: code has moved to BHIC.Core.PurchasePath.CommonFunctionality ,here only for reference, need to remove

        ///// <summary>
        ///// Return tax-id validation messages
        ///// </summary>
        ///// <param name="taxId"></param>
        ///// <returns></returns>
        //public OperationStatus ValidateFeinTinSsnNumber(string taxId)
        //{
        //    IVInsuredNameFEINService vInsuredNameFEINService = new VInsuredNameFEINService(guardServiceProvider);

        //    return vInsuredNameFEINService.ValidateFeinNumber(new VInsuredNameFEINRequestParms { FEIN = taxId });
        //}

        public int GenerateQuoteId(BHIC.ViewDomain.CustomSession appSession)
        {
            #region Variable initialization

            string adId = string.Empty;
            HttpContextBase context = this.ControllerContext.HttpContext;
            IQuoteStatusService quoteStatusService = new QuoteStatusService();
            int wcQuoteId = 0;

            #endregion

            #region Create QuoteId and Save it in cookie

            var postOperationStatus = quoteStatusService.AddQuoteStatus(new QuoteStatus
            {
                AdId = null,
                EnteredOn = DateTime.Now,
                UserIP = UtilityFunctions.GetUserIPAddress(context.ApplicationInstance.Context)
            }, guardServiceProvider);
            if (postOperationStatus.RequestSuccessful)
            {
                var effectedQuoteDTO = postOperationStatus.AffectedIds
                    .SingleOrDefault(res => res.DTOProperty == "QuoteId");

                wcQuoteId = Convert.ToInt32(effectedQuoteDTO.IdValue);

                appSession.SecureQuoteId = postOperationStatus.AffectedIds
                    .SingleOrDefault(res => res.DTOProperty == "SecureQuoteId").IdValue;

                SetCustomSession(appSession);
            }

            //Set QuoteId
            QuoteCookieHelper.Cookie_SaveQuoteId(context, wcQuoteId);

            #endregion

            return wcQuoteId;
        }

        /// <summary>
        /// This method returns the requested generated Id ,in a response, for a particular batch request
        /// </summary>
        /// <param name="requestIdentifier">Name by which a request is placed for batch processing</param>
        /// <param name="batchResponseRequestedId">The generated id which is sent in response for a particular batch request</param>
        /// <param name="batchResponses">List of batch response from which the requested generated response Id has to be returned</param>
        /// <returns>Returns the requested Id</returns>
        public string GetBatchResponseId(string requestIdentifier, string batchResponseRequestedId, List<BatchResponse> batchResponses)
        {
            string responseId = null;
            if (!batchResponses.IsNull() && batchResponses.Any() && !string.IsNullOrWhiteSpace(requestIdentifier) && !string.IsNullOrWhiteSpace(batchResponseRequestedId))
            {
                if (batchResponses.Any(m => m.RequestSuccessful && m.RequestIdentifier == requestIdentifier))
                {
                    var serviceResponse = batchResponses.SingleOrDefault(m => m.RequestSuccessful && m.RequestIdentifier == requestIdentifier).JsonResponse;
                    if (!serviceResponse.IsNull())
                    {
                        var operationStatusDeserialized = JsonConvert.DeserializeObject<OperationStatus>(serviceResponse);
                        if (!operationStatusDeserialized.AffectedIds.IsNull() && operationStatusDeserialized.AffectedIds.Any(m => m.DTOProperty == batchResponseRequestedId))
                        {
                            responseId = operationStatusDeserialized.AffectedIds.SingleOrDefault(m => m.DTOProperty == batchResponseRequestedId).IdValue;
                        }
                    }
                }
            }
            return responseId;
        }

        #endregion

        #region OnException In Controller Context

        // suppress 500 errors; exceptions will be logged via Elmah
        protected override void OnException(ExceptionContext filterContext)
        {
            Exception e = filterContext.Exception;

            filterContext.ExceptionHandled = true;

            // if cookies are disabled, the anti-forgery logic will throw exceptions; provide instructions to the user that might help them
            // (even if this is truly an an anti-forgery cookie exception, the error is still logged, and a potentially useful message is displayed to users without cookie support)
            if (e.Message.Contains("The required anti-forgery cookie"))
            {
                filterContext.Result = RedirectToAction("CookieExpired", "Error");
            }
            else if (filterContext.RouteData.Values.ContainsValue("Error"))
            {
                //Comment : Based on error type log it 
                loggingService.Fatal(e);
                //filterContext.Result = RedirectToAction("NotFound", "Page");
                filterContext.Result = RedirectToAction("OnExceptionError", "Error");
            }
            else
            {
                if (e.Message.Contains(Constants.SessionExpiredPartial))
                {
                    filterContext.Result = RedirectToAction("SessionExpiredPartial", "Error");
                }
                else if (e.Message.Contains(Constants.SessionExpired))
                {
                    filterContext.Result = RedirectToAction("SessionExpired", "Error");
                }
                else if (e.Message.Contains(Constants.UnauthorizedLandingRequest))
                {
                    filterContext.Result = RedirectToAction("OnExceptionErrorLanding", "Error");
                }
                //right now commneted (don't show user any skipped step related details as alert)
                else if (e.Message.Contains(Constants.UnauthorizedRequest))
                {
                    //string exceptionTobeShown = e.Message.Split(new Char[] { ':' })[1];

                    //filterContext.Result = RedirectToAction("UnAuthorizeRequest", "Error", new { exceptionMessage = exceptionTobeShown });
                    filterContext.Result = RedirectToAction("OnExceptionError", "Error");
                }
                else
                {
                    //Comment : Based on error type log it 
                    loggingService.Fatal(e);
                    //Comment : Here in case of all other un-expected application exceptions/errors
                    filterContext.Result = RedirectToAction("OnExceptionError", "Error");
                }
            }
        }

        #endregion

    }
}