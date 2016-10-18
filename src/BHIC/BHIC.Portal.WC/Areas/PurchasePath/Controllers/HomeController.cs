#region Using directives

using BHIC.Common;
using BHIC.Common.CommonUtilities;
using BHIC.Common.Config;
using BHIC.Common.Configuration;
using BHIC.Common.Mailing;
using BHIC.Common.Quote;
using BHIC.Common.XmlHelper;
using BHIC.Contract.Background;
using BHIC.Contract.PurchasePath;
using BHIC.Core;
using BHIC.Core.Background;
using BHIC.Core.Masters;
using BHIC.Core.PurchasePath;
using BHIC.DML.WC.DataContract;
using BHIC.DML.WC.DataService;
using BHIC.DML.WC.DTO;
using BHIC.Domain.Background;
using BHIC.Domain.Policy;
using BHIC.Portal.WC.App_Start;
using BHIC.ViewDomain.Landing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

#endregion

namespace BHIC.Portal.WC.Areas.PurchasePath.Controllers
{
    //[CustomTransactionLogFilterAttribute]
    public class HomeController : BaseController
    {
        #region Variables

        private static BHIC.Common.Logging.ILoggingService logger = BHIC.Common.Logging.LoggingService.Instance;

        #endregion

        #region Methods

        #region Public Methods

        //
        // GET: /PurchasePath/Index/
        //[CompressFilter]
        public ActionResult Index(string id)
        {
            #region Comment : Here Newly implemented to handle case where GetNewQuote clicked and session-exists but cookie does not exist

            /*
            //Comment : Here get current web-request context
            var currentHttpContext = this.ControllerContext.HttpContext;
            var isAppSessionExist = !((BHIC.ViewDomain.CustomSession)currentHttpContext.Session["CustomSession"]).IsNull();

            //clean application Custom session object
            if (QuoteCookieHelper.Cookie_GetQuoteId(currentHttpContext) == 0 && isAppSessionExist)
            {
                Session["CustomSession"] = null;
            }
            */

            //Comment : Her As per new session handling purpose create session on page load iteself
            if (IsCustomSessionNull())
            {
                SetCustomSession(new BHIC.ViewDomain.CustomSession());
            }

            #endregion

            #region Comment : Here check for PolicyCentre logged user session

            HttpContextBase context = this.ControllerContext.HttpContext;
            ValidatePolicyCenterLoggedUser(context);

            if (Session["user"] != null)
            {
            }

            #endregion

            #region Comment : Here if request query string is found(means request came from mail embedded link) then do following action

            string quoteId = id;
            if (quoteId != null)
            {
                //get quote-id in int format for future references
                int intQuoteId = 0;

                ICommonFunctionality commonFunctionality = GetCommonFunctionalityProvider();
                BHIC.ViewDomain.CustomSession appSession = RetrieveSavedQuote(quoteId, commonFunctionality, out intQuoteId);

                //Comment : Here if able to retrieve saved session from DB layer then do following
                if (appSession != null)
                {
                    //Comment : Here finlly update this into session
                    SetCustomSession(appSession);

                    //Save/Update QuoteId
                    QuoteCookieHelper.Cookie_SaveQuoteId(context, intQuoteId);
                }

            }

            #endregion

            #region Comment : Here prepare Home-Page configuration variables

            var homeVM = new HomeViewModel
            {
                LCLicense = ConfigCommonKeyReader.LCLicense
                ,
                LCGroup = ConfigCommonKeyReader.LCGroup
                ,
                LCServerName = ConfigCommonKeyReader.LCServerName
                ,
                LCServerValue = ConfigCommonKeyReader.LCServerValue
                ,
                LCSrc = ConfigCommonKeyReader.LCSrc
                ,
                EnvironmentName = ConfigCommonKeyReader.EnvironmentName
                ,
                GACode = ConfigCommonKeyReader.GACode
                ,
                SessionData = GetStateAndZipFromSession(),

                SystemIdleDuration = (int)TimeSpan.FromMinutes(Session.Timeout - 1).TotalSeconds //page should refresh before session timeout, e.g: 1 minute before
            };

            #endregion

            //SubmitEmailId("nishank.srivastava@xceedance.com");

            return View("Index", homeVM);
        }

        //
        //Post: /PurchasePath/Index/
        [HttpPost]
        [CustomAntiForgeryToken]
        [ValidateSession]
        public JsonResult Index(string zipCode, string state, string lobId)
        {
            BHIC.ViewDomain.CustomSession customSession = GetCustomSession();

            //remove whitespaces from input parameters
            zipCode = zipCode.Trim();
            state = state.Trim();

            if (!string.IsNullOrEmpty(zipCode) && !string.IsNullOrEmpty(state) && !string.IsNullOrEmpty(lobId))
            {
                customSession.ZipCode = (customSession.ZipCode == null ? zipCode.Trim() :
                                            (!customSession.ZipCode.Equals(zipCode) ? zipCode.Trim() : customSession.ZipCode));

                customSession.StateAbbr = (customSession.StateAbbr == null ? state.Trim() :
                                            (!customSession.StateAbbr.Equals(state) ? state.Trim() : customSession.StateAbbr));

                customSession.LobId = (customSession.LobId == null ? lobId.Trim() :
                                            (!customSession.LobId.Equals(lobId) ? lobId.Trim() : customSession.LobId));
                //if (!customSession.QuoteVM.IsNull())
                //{
                //    customSession.QuoteVM.IsMultiClassApplicable = false;
                //}
                //Store zip code and state into session
                SetCustomSession(customSession);
            }

            //if given parameter value saved/updated successfully, return true
            return Json(new
            {
                isSuccess = true
            }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Validate and fetch zip code detail
        /// </summary>
        /// <param name="zip"></param>
        /// <returns>return true if exists, false otherwise</returns>
        //public JsonResult IsValidZip(string zipCode)

        [HttpGet]
        [CustomAntiForgeryToken]
        public JsonResult GetValidZipDetail(string zipCode)
        {
            try
            {
                zipCode = zipCode.Trim();

                //if zipCode exists in county list return true, false otherwise
                if (!string.IsNullOrEmpty(zipCode) && IsZipExists(zipCode))
                {
                    return GetStateListByZipCode(zipCode);
                }
            }
            catch
            {
                throw;
            }

            return Json(new
            {
                isSuccess = false,
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Fetch zip code value from session
        /// </summary>
        /// <returns>returns zip code value from session</returns>
        public JsonResult GetStateAndZipFromSession()
        {
            if (!IsCustomSessionNull())
            {
                var customSession = GetCustomSession();

                var zipCode = customSession.ZipCode ?? string.Empty;
                var stateCode = customSession.StateAbbr ?? string.Empty;
                var selectedLobId = customSession.LobId ?? string.Empty;

                return GetStateListByZipCode(zipCode, stateCode, selectedLobId);
            }

            //on error, send empty data
            return Json(new
            {
                isSuccess = false
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Name of lob based on state code
        /// </summary>
        /// <param name="stateCode">State code</param>
        /// <returns>Returns lob filterd by state</returns>
        public JsonResult GetLobList(string stateCode)
        {
            ILineOfBusinessProvider lob = new LineOfBusinessProvider();
            List<LineOfBusiness> lobList = new List<LineOfBusiness>();

            if (!string.IsNullOrEmpty(stateCode))
            {
                ILineOfBusinessService lobService = new LineOfBusinessService();

                lobList = lobService.GetLineOfBusiness().Where(x => x.StateCode.Equals(stateCode)).ToList();
            }

            //if given parameter value exists return true,false otherwise
            return Json(new
            {
                isSuccess = lobList.Count > 0 ? true : false,
                lob = lobList
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Send mail with current saved quote revocation link
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[ValidateSession]
        public ActionResult SignOut()
        {
            try
            {
                //Comment : Here STEP - 1. Clear application session "user"
                RemoveAllAppSessionAndCookie();

                return Json(new { resultStatus = "OK", resultText = "User successfully logged-out" }, JsonRequestBehavior.AllowGet);
            }
            catch { }

            return Json(new { resultStatus = "NOK", resultText = "Something went wrong while processing application sign-out !" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// It will clear all session data, on session timeout
        /// </summary>
        /// <returns></returns>
        public JsonResult ClearSession()
        {
            //Comment : Here STEP - 1. Clear application session "user"
            RemoveAllAppSessionAndCookie();

            return Json(new { resultStatus = "OK", resultText = "All Session has been cleared successfully" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Redirect to bop site,when user selects bop option
        /// </summary>
        /// <returns></returns>
        public JsonResult GetBOPPath(string zipCode, string state)
        {
            WalkThroughRequestParms walkThroughRequest = new WalkThroughRequestParms
            {
                //QuoteId = QuoteCookieHelper.Cookie_GetQuoteId(this.ControllerContext.HttpContext),
                //mgacode = string.Empty,
                ZipCode = zipCode,
                State = state
            };

            //append params to url
            var bopUrl = string.Concat(GetSchemeAndHostURLPart(), ConfigCommonKeyReader.BopUrl, UtilityFunctions.CreateQueryString(walkThroughRequest));

            return Json(new
            {
                isSuccess = true,
                bopPath = bopUrl
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Save ZipCode and Redirect to CA site, when user selects Commerical Auto product
        /// </summary>
        /// <param name="zipCode"></param>
        /// <returns></returns>
        public JsonResult SaveCASession(string zipCode)
        {
            BHIC.ViewDomain.CustomSession customSession = GetCustomSession();
            customSession.ZipCode = (customSession.ZipCode == null ? zipCode.Trim() :
                                            (!customSession.ZipCode.Equals(zipCode) ? zipCode.Trim() : customSession.ZipCode));
            SetCustomSession(customSession);
            return Json(new
            {
                isSuccess = true,
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// It call  SendMailScheduleCall to perform scheduled call request
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="contact"></param>
        /// <param name="selectedRequestTime"></param>
        /// <returns></returns>
        public JsonResult SaveScheduleCallData(string fullName, string contact, string selectedRequestTime, string scheduleCallTime)
        {
            try
            {
                BHIC.Common.Logging.LoggingService.Instance.Trace(string.Format("Schedule Call Data: Name:{0},Contact:{1},ReqeustedTime:{2},scheduleCallTime:{3}", fullName, contact, selectedRequestTime, scheduleCallTime));

                var listOfErrors = new List<string>();

                if (!string.IsNullOrEmpty(fullName) && !string.IsNullOrEmpty(contact) && !string.IsNullOrEmpty(selectedRequestTime) && UtilityFunctions.IsValidRegex(contact, Constants.PhoneRegex))
                {
                    List<string> scheduleCallTimeList = new List<string>();
                    bool isValidCallTime = true;
                    DateTime requestedCallTime = DateTime.Now;

                    if (Convert.ToInt16(selectedRequestTime) == 5)
                    {
                        if (!string.IsNullOrEmpty(scheduleCallTime))
                        {
                            scheduleCallTimeList = scheduleCallTime.Split(':').ToList();

                            if (scheduleCallTimeList.Count != 2)
                            {
                                isValidCallTime = false;
                            }
                        }
                        else
                        {
                            isValidCallTime = false;
                        }
                    }

                    if (isValidCallTime)
                    {
                        switch (selectedRequestTime)
                        {
                            case "1":
                                requestedCallTime = DateTime.Now;
                                break;
                            case "2":
                                requestedCallTime = DateTime.Now.AddMinutes(15);
                                break;
                            case "3":
                                requestedCallTime = DateTime.Now.AddMinutes(30);
                                break;
                            case "4":
                                requestedCallTime = DateTime.Now.AddHours(1);
                                break;
                            case "5":
                                requestedCallTime = new DateTime(requestedCallTime.Year, requestedCallTime.Month, requestedCallTime.Day, Convert.ToInt16(scheduleCallTimeList[0]), Convert.ToInt16(scheduleCallTimeList[1]), 0);
                                break;
                        }

                        int hh = requestedCallTime.Hour;
                        int mm = requestedCallTime.Minute;

                        if (hh >= 8 && hh <= 19)
                        {
                            if (hh == 19 && mm > 30)
                            {
                                isValidCallTime = false;
                            }
                        }
                        else
                        {
                            isValidCallTime = false;
                        }

                    }

                    if (!isValidCallTime)
                    {
                        listOfErrors.Add("Please enter time between 8.00am - 7.30 pm EST");
                    }
                    else
                    {
                        var quoteId = QuoteCookieHelper.Cookie_GetQuoteId(this.ControllerContext.HttpContext);

                        BHIC.Common.Logging.LoggingService.Instance.Trace(string.Format("Schedule Call Data: Name:{0},Contact:{1},ReqeustedTime:{2},scheduleCallTime:{3}", fullName, contact, selectedRequestTime, scheduleCallTime));

                        //call SendMailScheduleCall to send  mail for requested call
                        //return true on success,false otherwise
                        return Json(new
                        {
                            isSuccess = SendMailScheduleCall(fullName, DateTime.Now, requestedCallTime, contact, quoteId),
                        }, JsonRequestBehavior.AllowGet);
                    }

                    //if model has error return error list
                    if (listOfErrors.Count > 0)
                    {
                        return Json(new
                        {
                            isSuccess = false,
                            resultMessages = listOfErrors
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(fullName))
                    {
                        listOfErrors.Add("Please provide full name");
                    }

                    if (string.IsNullOrEmpty(contact))
                    {
                        listOfErrors.Add("Please provide contact number");
                    }

                    if (string.IsNullOrEmpty(selectedRequestTime))
                    {
                        listOfErrors.Add("Please provide request time");
                    }

                    if (UtilityFunctions.IsValidRegex(contact, Constants.PhoneRegex))
                    {
                        listOfErrors.Add("Contact number format is not valid");
                    }
                }

                //if model has error return error list
                if (listOfErrors.Count > 0)
                {
                    return Json(new
                    {
                        isSuccess = false,
                        resultMessages = listOfErrors
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex);
            }

            ///return false if given parameters does not have valid values
            return Json(new
            {
                isSuccess = false,
            }, JsonRequestBehavior.AllowGet);


        }

        /// <summary>
        /// Return client side related system variable key and value
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllSystemVariables()
        {
            List<string> clientSystemVariables = new List<string> { ConfigCommonKeyReader.ApplicationContactInfo["PhoneNumber_CSR"], 
                ConfigCommonKeyReader.ApplicationContactInfo["Company_Domain"], 
                ConfigCommonKeyReader.ApplicationContactInfo["SysVarCompanyName"],
                ConfigCommonKeyReader.ApplicationContactInfo["CompanyName_NENATI20"], 
                ConfigCommonKeyReader.ApplicationContactInfo["Physical_Address1"], 
                ConfigCommonKeyReader.ApplicationContactInfo["Physical_Address2"], 
                ConfigCommonKeyReader.ApplicationContactInfo["Physical_AddressCSZ"],
                ConfigCommonKeyReader.ApplicationContactInfo["MailingClaims_NewClaimPhone"],
                ConfigCommonKeyReader.ApplicationContactInfo["FaxNumber_Claims"],
                ConfigCommonKeyReader.ApplicationContactInfo["Company_WebsiteURL"],
                ConfigCommonKeyReader.ApplicationContactInfo["BusinessHours"]
            };

            ISystemVariableService systemVariableService = new SystemVariableService(guardServiceProvider);
            List<SystemVariable> systemVariableList = systemVariableService.GetSystemVariables().Where(res =>
                                clientSystemVariables.Contains(res.Key, StringComparer.OrdinalIgnoreCase)).ToList();

            return Json(new
            {
                isSuccess = false,
                data = systemVariableList
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Reload all existing cache list on particular interval
        /// </summary>
        /// <param name="args">Args will have cache service name and expiration duration</param>
        /// <returns>Returns JsonResult with true on success, false otherwise</returns>
        public JsonResult RefreshCache(string args)
        {
            bool isSuccess = false;

            // In case passed argument is empty do nothing
            if (!string.IsNullOrEmpty(args))
            {
                try
                {
                    //decrypt query string parameters
                    string decryptedQueryString = Encryption.DecryptText(args);

                    logger.Trace(string.Format("decrypted String :{0}", decryptedQueryString));

                    //fetch cache service name from encrypted string
                    var cacheServiceName = decryptedQueryString.Split('=')[1].Split('&')[0];

                    DateTime cacheExpiryDuration = Convert.ToDateTime(decryptedQueryString.Split('=')[2]);

                    if (cacheExpiryDuration >= DateTime.Now)
                    {
                        switch (cacheServiceName)
                        {
                            case Constants.CountyCache:

                                //Reset county cache
                                ICountyService countyService = new CountyService(guardServiceProvider);
                                isSuccess = countyService.SetCountyCache();
                                break;

                            case Constants.IndustryCache:

                                //Reset industry cache
                                IIndustryService industryService = new IndustryService();
                                isSuccess = industryService.SetIndustryCache(
                                    new IndustryRequestParms
                                    {
                                        Lob = Constants.GetLineOfBusiness(Constants.LineOfBusiness.WC)
                                    }, guardServiceProvider);

                                break;

                            case Constants.SubIndustryCache:

                                //Reset sub-industry cache
                                ISubIndustryService subIndustryService = new SubIndustryService();
                                isSuccess = subIndustryService.SetSubIndustryCache(
                                    new SubIndustryRequestParms
                                    {
                                        Lob = Constants.GetLineOfBusiness(Constants.LineOfBusiness.WC)
                                    }, guardServiceProvider);

                                break;

                            case Constants.LineOfBusinessCache:

                                //Reset line of business cache
                                ILineOfBusinessService lobService = new LineOfBusinessService();
                                isSuccess = lobService.SetLineOfBusinessCache();

                                break;

                            case Constants.StateTypeCache:

                                //Reset good and bad state cache
                                IStateTypeService stateService = new StateTypeService();
                                isSuccess = stateService.SetAllGoodAndBadStateCache();

                                break;

                            case Constants.MultipleStates:

                                //Reset multiple state cache
                                IMultiStateService multipleStates = new MultiStateService();
                                isSuccess = multipleStates.SetStatesCache();

                                break;

                            default:
                                break;
                        }

                        logger.Trace(string.Format("{0} Cache Service has been executed at : {1}, with result :{2}", cacheServiceName, DateTime.Now.ToString(), isSuccess));
                    }
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                    logger.Fatal(string.Format("Cache refresh service failed with error{0}{1}", Environment.NewLine, ex.ToString()));
                }
            }

            return Json(new
            {
                success = isSuccess
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomAntiForgeryToken]
        //[ValidateSession]
        public JsonResult GetNewQuote(string quoteId)
        {
            SetCustomSession(new BHIC.ViewDomain.CustomSession());
            BHIC.ViewDomain.CustomSession customSession = GetCustomSession();

            if (!IsCustomSessionNull() && customSession.QuoteID <= 0)
            {
                #region Generate New QuoteId

                try
                {
                    customSession.QuoteID = GenerateQuoteId(customSession);

                    //Set QuoTeId in Session State
                    SetCustomSession(customSession);
                }
                catch
                {
                    return Json(new
                    {
                        isSuccess = false
                    }, JsonRequestBehavior.AllowGet);
                }

                #endregion
            }

            //if given parameter value saved/updated successfully, return true
            return Json(new
            {
                isSuccess = true
            }, JsonRequestBehavior.AllowGet);

        }

        #region Comment : Here bind static pages

        [HttpGet]
        public ActionResult SiteContents(string id)
        {
            #region Comment : Here check for PolicyCentre logged user session

            HttpContextBase context = this.ControllerContext.HttpContext;
            ValidatePolicyCenterLoggedUser(context);

            #endregion

            #region Comment : Here get routing parameter value from id variable into local variable and also set page title based on id variable

            string pageName = id;
            ViewBag.Title = GetStaticPageTitle(pageName);

            #endregion

            #region Comment : Here if request query string is found(means get requested static page name) then do following action

            string staticContentViewModel = string.Empty;

            //GUIN-166 - Albin - Added a condition for Home page to bypass Home page from SEO routing
            if (pageName.HasValidString() && !pageName.Equals("Home", StringComparison.OrdinalIgnoreCase))
            {
                #region Comment : Here Dynamic Href Internal/External link,Domain name binding & other handling - OLDER
                /*
                //Comment : Here get supplied static page html content from CDN
                staticContentViewModel = ThemeManager.ThemeSharedContent(pageName);

                //Comment : STEP - 1 Here alter images url with cdn path 
                staticContentViewModel = staticContentViewModel.Replace("src=\"images/", string.Format("src=\"{0}images/", ThemeManager.ThemeSharedContentFileBaseUrl()));

                //Comment : STEP - 2 Here alter href urls mwith mvc routing 
                staticContentViewModel = AppendHrefPath(staticContentViewModel);

                //Comment : STEP - 3 Here remove .html/htm extension from template string for internal links
                staticContentViewModel = ReplaceOnlyInternalLinks(staticContentViewModel);

                //Comment : only for Loss Control to Change content dynamically.
                if (pageName == "loss-control")
                    staticContentViewModel = DynamicLossControlContentReplace(staticContentViewModel);

                //Comment : Here in case no content found then send user back to default index view
                if (!staticContentViewModel.HasValidString())
                {
                    return RedirectToAction("PageNotFound", "Error");
                }
                */
                #endregion

                //Comment : Here get supplied static page html content from CDN
                staticContentViewModel = ThemeManager.ThemeSharedContent(pageName);

                if (pageName != "educational-flyers")
                {
                    //Comment : STEP - 1 Here alter images url with cdn path 
                    staticContentViewModel = staticContentViewModel.Replace("src=\"images/", string.Format("src=\"{0}images/", ThemeManager.ThemeSharedContentFileBaseUrl()));

                    //Comment : STEP - 2 Here alter href urls mwith mvc routing 
                    staticContentViewModel = AppendHrefPath(staticContentViewModel);

                    //Comment : STEP - 3 Here remove .html/htm extension from template string for internal links
                    staticContentViewModel = ReplaceOnlyInternalLinks(staticContentViewModel);
                }

                //Comment : only for Loss Control to Change content dynamically.
                if (pageName == "loss-control")
                    staticContentViewModel = DynamicLossControlContentReplace(staticContentViewModel);

                //Comment : Here in case no content found then send user back to default index view
                if (!staticContentViewModel.HasValidString())
                {
                    return RedirectToAction("PageNotFound", "Error");
                }
            }
            else
            {
                return View("Index");
            }

            #endregion

            //Create object
            object pageContent = new object();
            pageContent = "<input type=\"hidden\" id=\"hdnViewId\" value=\"" + id + "\" />" + staticContentViewModel;

            HomeViewModel homeVM = new HomeViewModel
            {
                SessionData = pageContent
            };

            //return View("_StaticPages", pageContent);
            return View("_StaticPages", homeVM);
        }

        #endregion

        #region Comment : Here download the documents from CDN

        [HttpGet]
        public ActionResult DownloadStaticDocument(string filename)
        {
            try
            {
                #region Comment : Here combining the file name to actual path and then returning the file
                string _filename = Encryption.DecryptText(filename);
                if (_filename.Contains("\\"))
                {
                    _filename = _filename.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries).Last();
                }
                string FullFileLogicalPath = Path.Combine(Server.MapPath(ConfigCommonKeyReader.StaticCommonFilePath), _filename);
                if (System.IO.File.Exists(FullFileLogicalPath))
                {
                    return File(FullFileLogicalPath, System.Net.Mime.MediaTypeNames.Application.Octet, _filename);
                }
                else
                {
                    throw new ApplicationException(string.Format("Specified file i.e. '{0}' is not a valid file", filename));
                }
                #endregion
            }
            catch (Exception ex)
            {
                loggingService.Fatal(string.Format("Method DownloadStaticDocument executed with error message : {0}", ex.ToString()));
                return RedirectToAction("Index", "Home", null);
            }
        }

        //Comment : Method is used to download the English Loss Control documents.
        [HttpGet]
        public ActionResult DownloadLossControlEnglishDocument(string filename)
        {
            return DownloadLossContolFile(filename, BHIC.Common.Config.Constants.LossControlLanguage.English);
        }

        //Comment : Method is used to download the Spanish Loss Control documents.
        [HttpGet]
        public ActionResult DownloadLossControlSpanishDocument(string filename)
        {
            return DownloadLossContolFile(filename, BHIC.Common.Config.Constants.LossControlLanguage.Spanish);
        }

        private ActionResult DownloadLossContolFile(string filename, Constants.LossControlLanguage lossControlLanguage)
        {
            try
            {
                #region Comment : Here combining the file name to actual path and then returning the file

                ICommonFunctionality iCommonFunctionality = new CommonFunctionality();

                string _filename = iCommonFunctionality.GetLossControlFileName(filename);
                if (_filename.Contains("\\"))
                {
                    _filename = _filename.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries).Last();
                }
                string FullFileLogicalPath = Path.Combine(Server.MapPath(GetLossControlFilePath(lossControlLanguage)), _filename);
                if (System.IO.File.Exists(FullFileLogicalPath))
                {
                    return File(FullFileLogicalPath, System.Net.Mime.MediaTypeNames.Application.Octet, _filename);
                }
                else
                {
                    throw new ApplicationException(string.Format("Specified file i.e. '{0}' is not a valid file", filename));
                }

                #endregion
            }
            catch (Exception ex)
            {
                loggingService.Fatal(string.Format("Method DownloadStaticDocument executed with error message : {0}", ex.ToString()));
                return RedirectToAction("Index", "Home", null);
            }
        }

        private static string GetLossControlFilePath(Constants.LossControlLanguage language)
        {
            string FilePath = string.Empty;
            switch (language)
            {
                case Constants.LossControlLanguage.English:
                    FilePath = ConfigCommonKeyReader.StaticCommonFilePathForLossControlEnglish;
                    break;
                case Constants.LossControlLanguage.Spanish:
                    FilePath = ConfigCommonKeyReader.StaticCommonFilePathForLossControlSpanish;
                    break;
            }
            return FilePath;
        }

        #endregion

        #region Comment : Here download the documents from CDN - OLDER
        /*
        [HttpGet]
        public ActionResult DownloadStaticDocument(string filename)
        {
            try
            {
                #region Comment : Here combining the file name to actual path and then returning the file
                string _filename = Encryption.DecryptText(filename);
                if (_filename.Contains("\\"))
                {
                    _filename = _filename.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries).Last();
                }
                string FullFileLogicalPath = Path.Combine(Server.MapPath(ConfigCommonKeyReader.StaticCommonFilePath), _filename);
                if (System.IO.File.Exists(FullFileLogicalPath))
                {
                    return File(FullFileLogicalPath, System.Net.Mime.MediaTypeNames.Application.Octet, _filename);
                }
                else
                {
                    throw new ApplicationException(string.Format("Specified file i.e. '{0}' is not a valid file", filename));
                }
                #endregion
            }
            catch (Exception ex)
            {
                loggingService.Fatal(string.Format("Method DownloadStaticDocument executed with error message : {0}", ex.ToString()));
                return RedirectToAction("Index", "Home", null);
            }
        }

        //Comment : Method is used to download the English Loss Control documents.
        [HttpGet]
        public ActionResult DownloadLossControlEnglishDocument(string filename)
        {
            try
            {
                #region Comment : Here combining the file name to actual path and then returning the file
                string _filename = Encryption.DecryptText(filename);
                if (_filename.Contains("\\"))
                {
                    _filename = _filename.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries).Last();
                }
                string FullFileLogicalPath = Path.Combine(Server.MapPath(ConfigCommonKeyReader.StaticCommonFilePathForLossControlEnglish), _filename);
                if (System.IO.File.Exists(FullFileLogicalPath))
                {
                    return File(FullFileLogicalPath, System.Net.Mime.MediaTypeNames.Application.Octet, _filename);
                }
                else
                {
                    throw new ApplicationException(string.Format("Specified file i.e. '{0}' is not a valid file", filename));
                }
                #endregion
            }
            catch (Exception ex)
            {
                loggingService.Fatal(string.Format("Method DownloadStaticDocument executed with error message : {0}", ex.ToString()));
                return RedirectToAction("Index", "Home", null);
            }
        }

        //Comment : Method is used to download the Spanish Loss Control documents.
        [HttpGet]
        public ActionResult DownloadLossControlSpanishDocument(string filename)
        {
            try
            {
                #region Comment : Here combining the file name to actual path and then returning the file
                string _filename = Encryption.DecryptText(filename);
                if (_filename.Contains("\\"))
                {
                    _filename = _filename.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries).Last();
                }
                string FullFileLogicalPath = Path.Combine(Server.MapPath(ConfigCommonKeyReader.StaticCommonFilePathForLossControlSpanish), _filename);
                if (System.IO.File.Exists(FullFileLogicalPath))
                {
                    return File(FullFileLogicalPath, System.Net.Mime.MediaTypeNames.Application.Octet, _filename);
                }
                else
                {
                    throw new ApplicationException(string.Format("Specified file i.e. '{0}' is not a valid file", filename));
                }
                #endregion
            }
            catch (Exception ex)
            {
                loggingService.Fatal(string.Format("Method DownloadStaticDocument executed with error message : {0}", ex.ToString()));
                return RedirectToAction("Index", "Home", null);
            }
        }
        */
        #endregion

        /// <summary>
        /// Submit Email Id and Generate password and send password to emailid provided
        /// </summary>
        /// <param name="emailId"></param>
        /// <returns></returns>
        public JsonResult SubmitEmailId(string emailId)
        {
            emailId = emailId.Trim();
            List<string> errorList = new List<string>();
            if (ValidateEmailId(emailId, errorList) && errorList.Count == 0)
            {
                string generatedPassword = GeneratePassword(emailId);
                if (!string.IsNullOrWhiteSpace(generatedPassword))
                {
                    SendPasswordToMail(emailId, generatedPassword);
                    return Json(new
                    {
                        result = true,
                        errorList = errorList
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new
            {
                result = false,
                errorList = errorList
            }, JsonRequestBehavior.AllowGet);

        }

        public bool ValidatePrepProdUser(string emailId, string password)
        {
            List<string> errorList = new List<string>();
            if (ValidateEmailId(emailId, errorList) && !string.IsNullOrWhiteSpace(password))
            {
                emailId = emailId.Trim();
                password = password.Trim();
                var currentTime = DateTime.Now;
                var decryptedPaswwordParts = DecryptPassword(password).Split(',');
                if (string.Equals(decryptedPaswwordParts[0], emailId))
                {
                    long ticks = 0;
                    bool parsed = long.TryParse(decryptedPaswwordParts[1], out ticks);
                    DateTime retrivedDateTime = new DateTime(ticks);
                    if (DateTime.Now.Subtract(retrivedDateTime).TotalMinutes > 15)
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        #endregion

        #region Private methods

        private string AppendHrefPath(string staticContentViewModel)
        {
            int pos = 0;
            string restString = string.Empty;

            //GUIN-166: Albin - Prepare SEO url by add trailing "/" during deployment and blank for localhost
            var appendSlash = (ConfigCommonKeyReader.AppBaseUrl.EndsWith("/") ? string.Empty : "/");

            //Prepare PP Base url
            var WcAppBaseUrl = string.Concat(ConfigCommonKeyReader.AppBaseUrl, appendSlash);

            do
            {
                pos = staticContentViewModel.IndexOf("href=\"", pos, StringComparison.OrdinalIgnoreCase);

                if (pos != -1)
                {
                    restString = staticContentViewModel.Substring((pos + 6));

                    //Comment : Here if INDEX then navigate to home page 
                    //comment : if href= DownloadStaticDocument?filename set the url to purchase/home/...
                    if (restString.StartsWith("index", StringComparison.OrdinalIgnoreCase) ||
                        restString.StartsWith("DownloadStaticDocument?filename", StringComparison.OrdinalIgnoreCase)||
                        restString.StartsWith("DownloadLossControlEnglishDocument?filename", StringComparison.OrdinalIgnoreCase) ||
                        restString.StartsWith("DownloadLossControlSpanishDocument?filename", StringComparison.OrdinalIgnoreCase))
                    {
                        if (restString.StartsWith("index", StringComparison.OrdinalIgnoreCase))
                        {
                            string subUrl = string.Format("{0}/{1}", "/PurchasePath/Quote/Index#/GetBusinessInfo", Server.UrlEncode(BHIC.Common.Encryption.EncryptText("-1")));
                            staticContentViewModel = staticContentViewModel.Substring(0, (pos + 6)) + subUrl + restString.Substring(5, (restString.Length - 5));
                        }
                        else
                        {
                            staticContentViewModel = staticContentViewModel.Substring(0, (pos + 6)) + "/PurchasePath/Home/" + restString; //temporarly hard coded pick it from Web.config
                        }
                    }
                    //Comment : Here if not followwing(tel:,http:,https:,mailto:,#) then only add static content URL prefix url path
                    else if
                        (
                            !restString.StartsWith("tel:", StringComparison.OrdinalIgnoreCase) &&
                            !restString.StartsWith("http:", StringComparison.OrdinalIgnoreCase) &&
                            !restString.StartsWith("https:", StringComparison.OrdinalIgnoreCase) &&
                            !restString.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase) &&
                            !restString.StartsWith("#\"", StringComparison.OrdinalIgnoreCase)
                        )
                    {
                        //staticContentViewModel = staticContentViewModel.Substring(0, (pos + 6)) + "/Home/SiteContents/" + restString; //Old line till 15.01.2016

                        //staticContentViewModel = staticContentViewModel.Substring(0, (pos + 6)) + "/PurchasePath/Home/SiteContents/" + restString;

                        //GUIN-166: Albin - Added SEO friendly path for static page links
                        staticContentViewModel = string.Concat(staticContentViewModel.Substring(0, (pos + 6)), WcAppBaseUrl, restString);
                    }

                    pos = pos + 6;
                }
            }
            while (pos > -1 && pos < staticContentViewModel.Length);

            return staticContentViewModel;

        }

        /// <summary>
        /// Remove HTML from internal links 
        /// </summary>
        /// <param name="staticContentViewModel"></param>
        /// <returns></returns>
        private string ReplaceOnlyInternalLinks(string staticContentViewModel)
        {
            int pos = 0;
            string hrefString = string.Empty;
            int hrefClosingQuote = 0;

            do
            {
                pos = staticContentViewModel.IndexOf("href=\"", pos, StringComparison.OrdinalIgnoreCase);

                if (pos != -1)
                {
                    if ((pos + 6) < staticContentViewModel.Length)
                    {
                        //Get href closing " index
                        hrefClosingQuote = staticContentViewModel.IndexOf("\"", (pos + 6));

                        // href closing found
                        if (hrefClosingQuote != -1)
                        {
                            //if poistion is reachable
                            if (((pos + 6) + (hrefClosingQuote - (pos + 6))) < staticContentViewModel.Length)
                            {
                                hrefString = staticContentViewModel.Substring((pos + 6), (hrefClosingQuote - (pos + 6)));

                                if (!string.IsNullOrEmpty(hrefString))
                                {
                                    //getting the scheme and Domain
                                    string schemeAndDoamin = GetSchemeDomain(hrefString);

                                    // Is href link is internal link
                                    if (string.IsNullOrEmpty(schemeAndDoamin) || GetSchemeAndHostURLPart().Equals(schemeAndDoamin, StringComparison.OrdinalIgnoreCase))
                                    {
                                        //only when link having .HTML path/url
                                        if (hrefString.EndsWith(".html", StringComparison.OrdinalIgnoreCase) ||
                                            hrefString.EndsWith(".htm", StringComparison.OrdinalIgnoreCase) ||
                                            hrefString.IndexOf(".html#", StringComparison.OrdinalIgnoreCase) > -1 ||
                                                hrefString.IndexOf(".htm#", StringComparison.OrdinalIgnoreCase) > -1)
                                        {
                                            //Comment : Here if Internal domain URL location then dont replace .html
                                            hrefString = string.Concat("href=\"", hrefString, "\"");
                                            string hrefStringReplace = hrefString.Replace(".html", "").Replace(".htm", "").Replace(".HTML", "").Replace(".HTM", "");
                                            staticContentViewModel = staticContentViewModel.Replace(hrefString, hrefStringReplace);
                                        }
                                        else
                                        {
                                            //Comment : Here if Internal static file names will be encrypted
                                            string filename = hrefString.Split(new string[] { "filename=" }, StringSplitOptions.RemoveEmptyEntries).Last();
                                            string encodedfilename = Encryption.EncryptText(filename);
                                            hrefString = string.Concat("href=\"", hrefString, "\"");
                                            string hrefStringReplace = hrefString.Replace(filename, encodedfilename);
                                            var regex = new Regex(Regex.Escape(hrefString));
                                            staticContentViewModel = regex.Replace(staticContentViewModel, hrefStringReplace, 1);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    pos = hrefClosingQuote;
                }
            }
            while (pos > -1 && pos < staticContentViewModel.Length);
            return staticContentViewModel;
        }

        /// <summary>
        /// Get Scheme and Host based on the input url
        /// </summary>
        /// <param name="zipCode"></param>
        /// <param name="stateCode"></param>
        /// <returns></returns>
        private string GetSchemeDomain(string hrefString)
        {
            //Becuase relative path may throw error in URI formation
            string schemeAndDoamin = string.Empty;
            try
            {
                Uri originalUrl = new Uri(hrefString);
                schemeAndDoamin = String.Concat(originalUrl.Scheme, Uri.SchemeDelimiter, originalUrl.Host);
            }
            catch
            {
            }
            return schemeAndDoamin;
        }

        /// <summary>
        /// Get State list by LOB detail for current state or specified state
        /// </summary>
        /// <param name="zipCode"></param>
        /// <param name="stateCode"></param>
        /// <returns></returns>
        private JsonResult GetStateListByZipCode(string zipCode, string stateCode = "", string selectedLobId = "")
        {
            if (!string.IsNullOrEmpty(zipCode))
            {
                var county = GetAllStateByZip(zipCode);

                //if county service is not available, throw error
                if (county == null || county.Count <= 0)
                {
                    throw new Exception("Unable to fetch specified zipcode state detail due to some technical issue in DB or Guard API");
                }

                //return detail for zip code, state and lob list
                return Json(new
                {
                    isSuccess = true,
                    county = county,
                    lobResult = ((county.Count == 1) ? GetLobList(county.FirstOrDefault().StateCode) : (string.IsNullOrEmpty(stateCode) ? null : GetLobList(stateCode))),
                    selectedState = (string.IsNullOrEmpty(stateCode)) ? string.Empty : stateCode,
                    selectedLob = (string.IsNullOrEmpty(selectedLobId)) ? string.Empty : selectedLobId
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                isSuccess = false,
            });
        }

        /// <summary>
        /// Get the Page titles of static pages based on pageid
        /// </summary>
        /// <param name="zipCode"></param>
        /// <param name="stateCode"></param>
        /// <returns></returns>
        private string GetStaticPageTitle(string pageName)
        {
            return
                pageName == "about-us" ? "About Us" :
                pageName == "privacy-policy" ? "Privacy Policy" :
                pageName == "terms-conditions" ? "Terms & Conditions" :
                pageName == "other-disclosures" ? "Other Disclosures" :
                pageName == "resources" ? "Resources" :
                pageName == "commercial-auto" ? "Commercial Auto" :
                pageName == "workers-comp" ? "Workers' Compensation" :
                pageName == "business-owners-policy" ? "Business Owner's Policy" :
                pageName == "bop-major-coverage" ? "BOP Major Coverages" :
                pageName == "faq" ? "FAQs" :
                pageName == "connecticut-mcp" ? "Connecticut MCP" :
                pageName == "texas-mpn" ? "Texas MPN" :
                pageName == "epn" ? "Employer Posting Notices" :
                pageName == "ca-claim-info" ? "CA Claims Information" : 
                pageName == "loss-control" ? "Loss Control" : 
                pageName == "educational-flyers" ? "Educational Flyers" : "Products";

        }

        private bool ValidateEmailId(string email, List<string> errorList)
        {
            var emailParts = email.Split('@');
            if (emailParts.Length < 2)
            {
                errorList.Add("EmailId not valid");
                return false;
            }
            if (!Constants.AllowedDomains.Any(x => String.Equals(x, emailParts[1])))
            {
                errorList.Add("Domain specified is not allowed");
                return false;
            }
            return true;
        }

        private string GeneratePassword(string email)
        {
            return Encryption.EncryptText(string.Concat(email, ',', DateTime.Now.Ticks, ',', new Random().Next()));
        }

        private bool SendPasswordToMail(string emailId, string password)
        {
            MailHelper mailHelper = new MailHelper();
            mailHelper.SendMailMessage(string.Empty, new List<string> { emailId }, "One Time Password", string.Format("{0} {1}", "your one time password for CoverYourBusiness website is ", password));
            return false;
        }

        private string DecryptPassword(string password)
        {
            return Encryption.DecryptText(password);
        }


        /// <summary>
        /// Method is used to change the dynamic content in the loss control page
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private string DynamicLossControlContentReplace(string content)
        {
            StringBuilder sb = new StringBuilder();

            if (Session["user"] != null)
            {
                sb.Append("<div class='msgWithLogin'>");
                sb.Append("<p class='mb0'> *To access the <a href='https://www.trainingnetworknow.com/' target='_blank'>online video library</a>, please use the listed username and password. </p>");
                sb.Append("<p class='mb0'> Username : <span class='text-bold'>policyholder@bhins.com</span></p>");
                sb.Append("<p> Password : <span class='text-bold'>safety</span></p>");
                sb.Append("<p>");
                sb.Append("<a href='" + ConfigCommonKeyReader.PurchasePathAppBaseURL + "Home/DownloadLossControlEnglishDocument?filename=" + Encryption.EncryptText("CYB_Safety_Video_List--6-7-2016.pdf") + "'> <svg xmlns='http://www.w3.org/2000/svg' version='1.1' x='0' y='0' viewBox='0 0 40.7 49.1' xml:space='preserve' width='22' height='22' style='margin-right:5px;float:left;'>");
                sb.Append("<polyline class='stroke1' points='28.3 9.6 39.5 9.6 39.5 48 1.1 48 1.1 9.6 12.3 9.6 ' />");
                sb.Append("<line class='stroke1' x1='20.3' y1='0' x2='20.3' y2='33.6' />");
                sb.Append("<polyline class='stroke1' points='11.6 24 20.3 34.3 29.1 24 ' />");
                sb.Append("</svg> Video library sample list </a>");
                sb.Append("</p>");
                sb.Append("</div>");
            }
            else
            {
                sb.Append("<div class='msgWithoutLogin'>");
                sb.Append("<p> *To access the online video library, please <a href='" + ConfigCommonKeyReader.SchemeAndHostURL + ConfigCommonKeyReader.PolicyCentreDashboardURL + "'>login</a> to your policy center. </p> ");
                sb.Append("</div>");
            }
            content = content.Replace("dynamicContent", sb.ToString());

            return content;
        }

        #endregion

        #endregion
    }
}
