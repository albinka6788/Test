#region Using directives

using BHIC.Common;
using BHIC.Common.CommonUtilities;
using BHIC.Common.Config;
using BHIC.Common.Quote;
using BHIC.Common.XmlHelper;
using BHIC.Contract.Policy;
using BHIC.Contract.PurchasePath;
using BHIC.Core.Policy;
using BHIC.Core.PurchasePath;
using BHIC.DML.WC.DataContract;
using BHIC.DML.WC.DataService;
using BHIC.DML.WC.DTO;
using BHIC.Domain.Policy;
using BHIC.Domain.PurchasePath;
using BHIC.Domain.Service;
using BHIC.Portal.WC.App_Start;
using BHIC.ViewDomain;
using BHIC.ViewDomain.Landing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

#endregion

namespace BHIC.Portal.WC.Areas.PurchasePath.Controllers
{
    [ValidateSession]
    [CustomAntiForgeryToken]
    public class BusinessInfoController : BaseController
    {
        #region Variables : Page Level Local Variables Decalration

        private static string BusinessInfoPage = "BusinessInfoPage";

        BHIC.ViewDomain.CustomSession appSession = null;

        #endregion

        #region Public Methods

        #region Methods : public methods

        #region MethodType : GET

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Get lading home page business information
        /// </summary>
        /// <param name="quoteId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetBusinessInfo(string quoteId)
        {
            #region Comment : Here if request query string is found(means request came from mail embedded link) then do following action

            if (quoteId != null)
            {
                var quoteQueryString = BHIC.Common.Encryption.DecryptText(quoteId);

                if (quoteQueryString == "-1")
                {
                    #region Comment : Here in case when user requested for "Get New Quote" then do following
                    if (!IsCustomSessionNull())
                    {
                        SetCustomSession(new BHIC.ViewDomain.CustomSession());
                    }
                    #endregion
                }
                else
                {
                    #region Comment : Here Retrieve Quote code section

                    //get quote-id in int format for future references
                    int intQuoteId = 0;

                    ICommonFunctionality commonFunctionality = GetCommonFunctionalityProvider();
                    appSession = RetrieveSavedQuote(quoteId, commonFunctionality, out intQuoteId);

                    //Comment : Here if able to retrieve saved session from DB layer then do following
                    if (appSession != null)
                    {
                        //Comment : Here finlly update this into session
                        SetCustomSession(appSession);

                        //Save/Update QuoteId
                        HttpContextBase context = this.ControllerContext.HttpContext;
                        QuoteCookieHelper.Cookie_SaveQuoteId(context, intQuoteId);

                        #region For Old Quotes where BusinessInfo Vm is not available make API call for extracting LOB and set their mapping in customSession
                        if (appSession.BusinessInfoVM.IsNull())
                        {
                            appSession.BusinessInfoVM = new BusinessInfoViewModel();
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(appSession.BusinessInfoVM.LobId))
                            {
                                appSession.BusinessInfoVM.LobId = GetLobId(intQuoteId);
                            }
                            if (string.IsNullOrWhiteSpace(appSession.BusinessInfoVM.StateCode) || string.IsNullOrWhiteSpace(appSession.BusinessInfoVM.ZipCode))
                            {
                                appSession.BusinessInfoVM.StateCode = appSession.StateAbbr;
                                appSession.BusinessInfoVM.ZipCode = appSession.ZipCode;
                            }
                        }
                        #endregion

                        SetCustomSession(appSession);
                    }
                    else
                    {
                        //return PartialView("_GetBusinessInfo", new BusinessInfoViewModel());
                        this.ControllerContext.HttpContext.Response.Redirect("~/Error/QuoteExpired");
                        //RedirectToAction("QuoteExpired", "Error");
                        //throw new ApplicationException(string.Format("{0} : {1} ", BusinessInfoPage, Constants.QuoteDeleted));
                    }

                    #endregion
                }
            }

            #endregion

            #region Comment : Here get all custom session information reuired for this page processing

            GetSession();

            #endregion

            #region Comment : Here set VM object pass it to view

            BusinessInfoViewModel businessInfoViewModel =
                (!appSession.IsNull() && appSession.IsLanding) ?            //Here must check is user navigated from LandingPages then don't create new VM
                appSession.BusinessInfoVM : new BusinessInfoViewModel();

            var loggedUserDetails = GetLoggedInUserDetails();

            #endregion

            #region Comment : Here BusinessInfo interface refernce to do/make process all business logic

            IProspectInfoService prospectInfoBLL = GetProspectInfoProvider();

            //stored in a cookie on the user's machine
            int wcQuoteId = GetCookieQuoteId();

            //Comment : Here get prospectInfo/businessInfo 
            if (!appSession.BusinessInfoVM.IsNull() && !appSession.BusinessInfoVM.ProspectInfoId.IsNull() && appSession.BusinessInfoVM.ProspectInfoId.Value > 0)
            {
                var prospectInfoId =
                            (!appSession.BusinessInfoVM.IsNull() && !appSession.BusinessInfoVM.ProspectInfoId.IsNull()) ? appSession.BusinessInfoVM.ProspectInfoId.Value : 0;

                var prospectInfoResponse = prospectInfoId > 0 ? prospectInfoBLL.GetProspectInfo
                                            (
                                                new Domain.Policy.ProspectInfoRequestParms() { ProspectInfoId = prospectInfoId }
                                            ) : null;
                #region Comment : Here set error message from API response in case of any non-system error type

                if (prospectInfoResponse != null && !prospectInfoResponse.OperationStatus.RequestSuccessful)
                {
                    prospectInfoResponse.OperationStatus.Messages.ForEach(msg => businessInfoViewModel.Messages.Add(msg.Text));
                }

                if (!prospectInfoResponse.IsNull() && !prospectInfoResponse.OperationStatus.IsNull() && prospectInfoResponse.OperationStatus.RequestSuccessful && prospectInfoResponse.ProspectInfoList.Any())
                {
                    #region Comment : Here Set "Prospect" API data into returning VM for UI binding

                    var prospectInfo = prospectInfoResponse.ProspectInfoList.FirstOrDefault(res => res.ProspectInfoId == appSession.BusinessInfoVM.ProspectInfoId.Value);
                    businessInfoViewModel.CompanyName = prospectInfo.CompanyName ?? string.Empty;
                    businessInfoViewModel.ZipCode = prospectInfo.Zip ?? string.Empty;
                    businessInfoViewModel.Email = prospectInfo.Email ?? string.Empty;

                    //re-generating plian-text into mask format (with phone number + extention)
                    try
                    {

                        var phoneNumber = !string.IsNullOrEmpty(prospectInfo.PhoneNumber) ? prospectInfo.PhoneNumber : string.Empty;
                        businessInfoViewModel.PhoneNumber = !string.IsNullOrEmpty(phoneNumber) ?
                            (
                                string.Format("{0}-{1}-{2} x{3}", phoneNumber.Substring(0, 3), phoneNumber.Substring(3, 3), phoneNumber.Substring(6, 4),
                                !string.IsNullOrEmpty(prospectInfo.Extension) ? prospectInfo.Extension : string.Empty)
                            ) : string.Empty;

                    }
                    catch { loggingService.Trace("Unable to format pain text phone-number into masked text - BusinessInfo Page "); }

                    businessInfoViewModel.ContactName = prospectInfo.ContactName ?? string.Empty;
                    businessInfoViewModel.Address1 = prospectInfo.Addr1 ?? string.Empty;
                    businessInfoViewModel.City = prospectInfo.City ?? string.Empty;

                    #endregion
                }
                else if ((prospectInfoResponse.IsNull() || !prospectInfoResponse.OperationStatus.RequestSuccessful) && prospectInfoId > 0)
                {
                    //GUIN:352 : As per Jason's revert we dont have to show error page even if the response doesnt come
                    //throw new ApplicationException(string.Format("{0},{1} : ProspectInfoId exists but API response empty !", "ProspectInfoPage", Constants.UnauthorizedRequest));

                    return PartialView("_GetBusinessInfo", businessInfoViewModel);
                }

                #endregion
            }
            else if (!appSession.PurchaseVM.IsNull())
            {
                if (!appSession.PurchaseVM.MailingAddress.IsNull())
                {
                    businessInfoViewModel.Address1 = appSession.PurchaseVM.MailingAddress.AddressLine1;
                    businessInfoViewModel.City = appSession.PurchaseVM.MailingAddress.City;
                    businessInfoViewModel.StateCode = appSession.PurchaseVM.MailingAddress.State;
                    businessInfoViewModel.ZipCode = appSession.PurchaseVM.MailingAddress.Zip ?? string.Empty;
                }
                businessInfoViewModel.CompanyName = !appSession.PurchaseVM.BusinessInfo.IsNull() ? appSession.PurchaseVM.BusinessInfo.BusinessName ?? string.Empty : string.Empty;
                if (!appSession.PurchaseVM.PersonalContact.IsNull())
                {
                    businessInfoViewModel.Email = appSession.PurchaseVM.PersonalContact.Email ?? string.Empty;
                    businessInfoViewModel.PhoneNumber = !string.IsNullOrEmpty(appSession.PurchaseVM.PersonalContact.PhoneNumber) ? CommonFunctionality.RemoveOlderPhoneMask(appSession.PurchaseVM.PersonalContact.PhoneNumber) : string.Empty;
                }
                //Since there is no direct mapping of ContactName we will not prefill with user Info data
                //businessInfoViewModel.ContactName = prospectInfo.ContactName ?? string.Empty;
            }
            else if (loggedUserDetails != null)
            {
                #region Comment : Here If LoggedIn user then get PC data

                //businessInfoViewModel.Email = loggedUserDetails.Email ?? string.Empty; //OLDER Line

                IUserInfo userInfo = new UserInfo();
                UserDetail userDetail = userInfo.GetUserInfo(loggedUserDetails, guardServiceProvider);
                businessInfoViewModel.Email = userDetail.EmailID;
                businessInfoViewModel.ContactName = userDetail.FullName;

                //Comment : Here [GUIN-564-Prem] Format PhoneNumber according MaskFormat applied on UI
                //businessInfoViewModel.PhoneNumber = userDetail.Phone; //OLD Line
                var phoneNumber = userDetail.Phone;
                businessInfoViewModel.PhoneNumber = UtilityFunctions.GetMaskedPhoneNumber(userDetail.Phone, userDetail.Extension);

                #endregion
            }
            #endregion

            #region Comment : Here get previus "ProspactInfo" if any exists

            #region Comment : Here based on ZipCode return list of cities and StateCode future references

            //if has zipcode value
            var zipCode = !string.IsNullOrEmpty(businessInfoViewModel.ZipCode) ? businessInfoViewModel.ZipCode : null;
            if (zipCode != null)
            {
                #region Comment : Here Based on zip-code,county get "StateCode","LobsList","CityList"

                //get county data
                var county = GetAllStateByZip(zipCode);

                //if county service is not available, throw error
                if (county == null || !county.Any())
                {
                    throw new Exception("Unable to fetch specified zipcode state detail due to some technical issue in DB or Guard API");
                }

                //Comment : Here set StateCode
                businessInfoViewModel.StateCode = county.FirstOrDefault().StateCode;

                #region Comment : Here get LOBs for this STATE

                var lobAvailibility = (county.Any() ? GetLobList(businessInfoViewModel.StateCode) : null);
                if (lobAvailibility != null && lobAvailibility.Any())
                {
                    businessInfoViewModel.LobList = JsonConvert.SerializeObject(lobAvailibility);
                }

                #endregion

                #region Comment : Here get City list for this ZipCode+STATE
                /*
                var cityList = GetAllCitiesByStateAndZip(zipCode, businessInfoViewModel.StateCode);
                if (cityList != null && cityList.Any())
                {
                    businessInfoViewModel.CityList = JsonConvert.SerializeObject(cityList);
                }
                */
                #endregion

                #endregion
            }

            #endregion

            //Comment : Here same time set this to CustomSession object
            if (appSession != null)
            {
                //Comment : Here must check object instance existance
                var sessionBusinessVM = appSession.BusinessInfoVM;

                if (sessionBusinessVM == null)
                {
                    appSession.BusinessInfoVM = new BusinessInfoViewModel();
                }

                //Comment : Here important SET "LobId" from session if exists
                businessInfoViewModel.LobId = appSession.BusinessInfoVM.LobId ?? string.Empty;
                businessInfoViewModel.StateCode = appSession.BusinessInfoVM.StateCode;
                businessInfoViewModel.ZipCode = appSession.BusinessInfoVM.ZipCode;
                //Comment : Here finlly update this into session
                SetCustomSession(appSession);
            }

            #endregion

            return PartialView("_GetBusinessInfo", businessInfoViewModel);
        }

        public JsonResult ValidateZipCodeLobs(string zipCode)
        {
            try
            {
                zipCode = !string.IsNullOrEmpty(zipCode) ? zipCode.Trim() : string.Empty;

                if (zipCode.Length == 5)
                {
                    #region Comment : Here Based on zip-code,county get "StateCode","LobsList","CityList"

                    //get county data
                    var county = GetAllStateByZip(zipCode);

                    //if county service is not available, throw error
                    if (county == null || !county.Any())
                    {
                        throw new Exception("Unable to fetch specified zipcode state detail due to some technical issue in DB or Guard API");
                    }

                    //Comment : Here set VM object pass it to view
                    BusinessInfoViewModel businessInfoViewModel = new BusinessInfoViewModel();

                    //Comment : Here set StateCode
                    businessInfoViewModel.StateCode = county.FirstOrDefault().StateCode;

                    #region Comment : Here get LOBs for this STATE

                    var lobAvailibility = (county.Any() ? GetLobList(businessInfoViewModel.StateCode) : null);
                    if (lobAvailibility != null && lobAvailibility.Any())
                    {
                        businessInfoViewModel.LobList = JsonConvert.SerializeObject(lobAvailibility);
                    }

                    #endregion

                    #region Comment : Here get City list for this ZipCode+STATE
                    /*
                    var cityList = GetAllCitiesByStateAndZip(zipCode, businessInfoViewModel.StateCode);
                    if (cityList != null && cityList.Any())
                    {
                        businessInfoViewModel.CityList = JsonConvert.SerializeObject(cityList);
                    }
                    */
                    #endregion

                    #endregion

                    return Json(new
                    {
                        resultStatus = "OK",
                        resultText = "Success",
                        resultMessages = new List<string>(),
                        busInfoVM = businessInfoViewModel
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                loggingService.Trace("Something went wrong validate zipcode ");
            }

            return Json(new { resultStatus = "NOK", resultText = "Something went wrong validate zipcode", resultMessages = new List<string>() }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region MethodType : POST

        /// <summary>
        /// This method will submit Business Information to generate Quote
        /// </summary>
        /// <param name="businessInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostBusinessInfo(BusinessInfoViewModel businessInfoVM)
        {
            if (businessInfoVM != null)
            {
                string zipCode, state, lobId;
                zipCode = businessInfoVM.ZipCode.Trim();
                state = businessInfoVM.StateCode;
                lobId = businessInfoVM.LobId;

                #region Comment : Here Server side validation processing

                //Todo: call ValidateProspectInfoModel for server side validation

                var listOfModelErrors = ValidateProspectInfoModel(businessInfoVM);

                // if model has error return error list
                if (listOfModelErrors.Any())
                {
                    return Json(new
                    {
                        resultStatus = "NOK",
                        resultText = "",
                        resultMessages = listOfModelErrors
                    }, JsonRequestBehavior.AllowGet);
                }

                #endregion

                #region Comment : Here BusinessInfo interface refernce to do/make process all business logic

                IProspectInfoService prospectInfoBLL = GetProspectInfoProvider();

                #endregion

                #region Comment : Here When posted BusinessInfo(PorspectInfo) data found then

                #region Comment : Here get all custom session information required for this page processing

                GetSession();

                #endregion

                try
                {

                    #region Saving LOB Data is not exist

                    var selectedLob = GetLobList(state).FirstOrDefault(x => x.Id == Convert.ToInt32(businessInfoVM.LobId));
                    string selectedLobCode = (selectedLob.Abbreviation.Equals("BOP", StringComparison.OrdinalIgnoreCase) ? "BP" : selectedLob.Abbreviation);
                    string lobFullName = selectedLob.LobFullName;
                    var prospectInfoId =
                        (!appSession.BusinessInfoVM.IsNull() && !appSession.BusinessInfoVM.ProspectInfoId.IsNull()) ? appSession.BusinessInfoVM.ProspectInfoId.Value : 0;

                    // Check is Quote ID not exist
                    if (appSession.QuoteID == 0)
                    {
                        // Call Quote ID generation
                        GenerateAndSaveQuoteID(businessInfoVM);
                        // Call LOBData Post
                        PostLobData(state, selectedLobCode, lobFullName);
                    }
                    else
                    {
                        // Check is Business Info not exist
                        if (prospectInfoId == 0)
                        {
                            SaveLOBData(businessInfoVM, selectedLobCode, lobFullName);
                        }
                        else
                        {
                            // Check whether Zip code , state or LOB
                            if (!appSession.ZipCode.Equals(businessInfoVM.ZipCode, StringComparison.OrdinalIgnoreCase) || !appSession.StateAbbr.Equals(businessInfoVM.StateCode, StringComparison.OrdinalIgnoreCase) || !appSession.LobId.Equals(businessInfoVM.LobId, StringComparison.OrdinalIgnoreCase))
                            {
                                // Call Quote ID generation
                                GenerateAndSaveQuoteID(businessInfoVM);
                                // Call LOBData Post
                                PostLobData(state, selectedLobCode, lobFullName);
                                prospectInfoId = 0;
                            }
                            else
                            {
                                SaveLOBData(businessInfoVM, selectedLobCode, lobFullName);
                            }
                        }
                    }

                    /*var wcCondition = (lobId == "1" && !appSession.QuoteVM.IsNull() && !appSession.QuoteVM.County.IsNull() && (businessInfoVM.ZipCode != appSession.QuoteVM.County.ZipCode || businessInfoVM.StateCode != appSession.QuoteVM.County.State));
                    var nonWCCondition = (lobId != "1" && !appSession.BusinessInfoVM.IsNull() && (businessInfoVM.ZipCode != appSession.BusinessInfoVM.ZipCode || businessInfoVM.StateCode != appSession.BusinessInfoVM.StateCode));
                    if (
                            wcCondition || nonWCCondition || (!appSession.BusinessInfoVM.IsNull() && (businessInfoVM.LobId != appSession.BusinessInfoVM.LobId))
                       )
                    {
                        //Comment : Here first get new "QuoteId,SecureQuoteId,BusinessInfo" objects from session and then create new session and update these values
                        int newWcQuoteID = GenerateQuoteId(appSession);
                        string secureQuoteId = appSession.SecureQuoteId;

                        //Comment : Here [GUIN-267-Prem] To maintain only "BusinessInfo/PorspactInfo" and related ProsPecInfoId to create NewSession onwards this page
                        var sessionBusinessInfoVM = appSession.BusinessInfoVM ?? null;

                        //Comment : Here CREATE new session from this step onwards
                        appSession = null;
                        BHIC.ViewDomain.CustomSession newAppSession = new BHIC.ViewDomain.CustomSession();
                        newAppSession.QuoteID = newWcQuoteID;
                        newAppSession.SecureQuoteId = secureQuoteId;
                        newAppSession.ZipCode = businessInfoVM.ZipCode;
                        newAppSession.StateAbbr = businessInfoVM.StateCode;

                        //Finally set session state
                        appSession = newAppSession;
                        SetCustomSession(appSession);

                        //try
                        //{
                        //    var lobDataResponse = PostLobData(state, lobId, newWcQuoteID);
                        //}
                        //catch (Exception ex)
                        //{
                        //    //log error in case of LobData POST api call
                        //    loggingService.Fatal(string.Format("Service {0} Call with error message : {1}", "LobData", ex.ToString()));
                        //}

                    }*/

                    #endregion

                    int wcQuoteId = GetCookieQuoteId();

                    if (wcQuoteId != -1)
                    {

                        #region Comment : Here STEP-1 collect all posted information into targeted DTOs

                        #region Comment : Here get all custom session information reuired for this page processing

                        //GetSession();

                        #endregion

                        //Comment : Here local varibles                    


                        //Comment: Here get number & extention sepratly from request data
                        var phoneNumber = !string.IsNullOrEmpty(businessInfoVM.PhoneNumber) ? businessInfoVM.PhoneNumber.Split(new[] { 'x' }) : null;

                        #endregion

                        #region Comment : Here STEP-2 POST this data trough API

                        #region If prospectInfoId does not exist POST LOB DATA

                        /*if (prospectInfoId == 0)
                        {
                            try
                            {
                                if (!GetLobData(wcQuoteId).Any())
                                {
                                    var lobDataResponse = PostLobData(state, lobId, wcQuoteId);
                                }
                            }
                            catch (Exception ex)
                            {
                                //log error in case of LobData POST api call
                                loggingService.Fatal(string.Format("Service {0} Call with error message : {1}", "LobData", ex.ToString()));
                            }
                        }*/

                        #endregion If prospectInfoId does not exist POST LOB DATA

                        //Comment : Here save prospectInfo/businessInfo   
                        var prospectInfoResponse = new OperationStatus();
                        var listOfErrors = new List<string>();

                        try
                        {
                            prospectInfoResponse = prospectInfoBLL.AddtProspectInfo(new ProspectInfo()
                            {
                                ProspectInfoId = prospectInfoId > 0 ? prospectInfoId : 0,
                                QuoteId = wcQuoteId,
                                CompanyName = businessInfoVM.CompanyName,
                                Zip = businessInfoVM.ZipCode,
                                Email = businessInfoVM.Email,
                                State = state,
                                PhoneNumber
                                        = (phoneNumber != null && phoneNumber.Length > 0) ? (UtilityFunctions.ToNumeric(phoneNumber[0]) ?? string.Empty) : string.Empty,
                                ContactName = businessInfoVM.ContactName,
                                Addr1 = businessInfoVM.Address1,
                                City = businessInfoVM.City,
                                Extension
                                        = (phoneNumber != null && phoneNumber.Length > 1) ? (UtilityFunctions.ToNumeric(phoneNumber[1]) ?? string.Empty) : string.Empty,
                            });

                        }
                        catch (Exception ex)
                        {
                            //Collect errors into string list
                            ex.Message.Split('.').ToList().ForEach(res => listOfErrors.Add(string.Concat(res, ".")));

                            return Json(new
                            {
                                resultStatus = "NOK",
                                resultText = "Business information data saved successfully",
                                resultUrl = string.Empty,
                                resultMessages = listOfErrors
                            }, JsonRequestBehavior.AllowGet);
                        }

                        #endregion

                        #region Comment : Here set error message from API response in case of any non-system error type

                        if (prospectInfoResponse != null && !prospectInfoResponse.RequestSuccessful &&
                            !prospectInfoResponse.Messages.Any(res => res.MessageType == MessageType.SystemError))
                        {
                            prospectInfoResponse.Messages.ForEach(msg => listOfErrors.Add(msg.Text));

                            //Comment : Here retrun execution of code from here itself in case of some error occured
                            if (listOfErrors.Count > 0)
                            {
                                return Json(new { resultStatus = "NOK", resultText = "Something went wrong while posting porpect info data !", resultMessages = listOfErrors }, JsonRequestBehavior.AllowGet);
                            }
                        }

                        #endregion

                        #region Comment : Here STEP - 4 If POST API request successful then get new QuoteId

                        if (prospectInfoResponse.RequestSuccessful)
                        {
                            #region Comment : Here on POST Questions success get Premium and InstallmentFee in session object for next page(Quote) usage

                            try
                            {
                                #region Comment : Here STEP-3 Set active Sessio object

                                //GetSession();

                                if (appSession != null)
                                {
                                    #region Set BusinessInfo data into session

                                    appSession.ZipCode = (appSession.ZipCode == null ? zipCode.Trim() :
                                                                (!appSession.ZipCode.Equals(zipCode) ? zipCode.Trim() : appSession.ZipCode));

                                    appSession.StateAbbr = (appSession.StateAbbr == null ? state.Trim() :
                                                                (!appSession.StateAbbr.Equals(state) ? state.Trim() : appSession.StateAbbr));

                                    appSession.LobId = (appSession.LobId == null ? lobId.Trim() :
                                                                (!appSession.LobId.Equals(lobId) ? lobId.Trim() : appSession.LobId));

                                    //Comment : Here set "ProspectInfo" into session object
                                    if (appSession.BusinessInfoVM == null)
                                    {
                                        appSession.BusinessInfoVM = new BusinessInfoViewModel();
                                    }

                                    //mandatory data
                                    appSession.BusinessInfoVM.CompanyName = businessInfoVM.CompanyName;
                                    appSession.BusinessInfoVM.ZipCode = businessInfoVM.ZipCode;
                                    appSession.BusinessInfoVM.Email = businessInfoVM.Email;
                                    appSession.BusinessInfoVM.StateCode = appSession.StateAbbr;
                                    appSession.BusinessInfoVM.PhoneNumber = businessInfoVM.PhoneNumber ?? string.Empty;
                                    appSession.BusinessInfoVM.ContactName = businessInfoVM.ContactName ?? string.Empty;
                                    appSession.BusinessInfoVM.Address1 = businessInfoVM.Address1 ?? string.Empty;
                                    appSession.BusinessInfoVM.City = businessInfoVM.City ?? string.Empty;
                                    appSession.BusinessInfoVM.LobId = lobId;
                                    if (!appSession.PurchaseVM.IsNull())
                                    {
                                        if (!appSession.PurchaseVM.BusinessInfo.IsNull())
                                        {
                                            appSession.PurchaseVM.BusinessInfo.BusinessName = businessInfoVM.CompanyName;
                                        }
                                        if (!appSession.PurchaseVM.PersonalContact.IsNull())
                                        {
                                            appSession.PurchaseVM.PersonalContact.Email = businessInfoVM.Email;
                                            appSession.PurchaseVM.PersonalContact.PhoneNumber = businessInfoVM.PhoneNumber;
                                            if (!string.IsNullOrWhiteSpace(businessInfoVM.ContactName))
                                            {
                                                var contactName = businessInfoVM.ContactName.Split(' ');
                                                if (contactName.Length > 0)
                                                {
                                                    appSession.PurchaseVM.PersonalContact.FirstName = contactName[0];
                                                    appSession.PurchaseVM.PersonalContact.LastName = contactName.Length > 1 ? contactName[contactName.Length - 1] : string.Empty;
                                                }
                                            }
                                        }
                                        if (!appSession.PurchaseVM.Account.IsNull())
                                        {
                                            appSession.PurchaseVM.Account.Email = businessInfoVM.Email;
                                        }
                                    }

                                    //Comment : Here Set ProspectInfoId for future "UPDATE"
                                    appSession.BusinessInfoVM.ProspectInfoId =
                                        Convert.ToInt32(prospectInfoResponse.AffectedIds.SingleOrDefault(res => res.DTOProperty == "ProspectInfoId").IdValue);

                                    //Comment : Here must clear LobList & CirtList in session added on PageLoad before setting SESSION state
                                    appSession.BusinessInfoVM.LobList = string.Empty;
                                    appSession.BusinessInfoVM.CityList = string.Empty;

                                    #endregion
                                }

                                #endregion

                                #region Comment : Here according to new requirement in case quote already exists then must UPDATE in DB for future consequenses

                                try
                                {
                                    ICommonFunctionality commonFunctionality = new CommonFunctionality();

                                    //Comment : Here call BLL to make this data stored in DB
                                    commonFunctionality.AddApplicationCustomSession(
                                        new DML.WC.DTO.CustomSession()
                                        {
                                            QuoteID = wcQuoteId,
                                            SessionData = commonFunctionality.StringifyCustomSession(appSession),
                                            IsActive = true,
                                            CreatedDate = DateTime.Now,
                                            CreatedBy = GetLoggedUserId() ?? 1,
                                            ModifiedDate = DateTime.Now,
                                            ModifiedBy = GetLoggedUserId() ?? 1,
                                            UpdateOnly = true
                                        });
                                }
                                catch { loggingService.Trace("Unable to save session data inot DB - BusinessInfo Page "); }

                                #endregion

                                //Comment : Here [GUIN-468-Anuj] ProgressBar Active issue fixed
                                appSession.PageFlag = 1;

                                //Comment : Here finlly update this into session
                                SetCustomSession(appSession);
                            }
                            catch (Exception ex)
                            {
                                loggingService.Trace(string.Format("Unable to post prospect information {0}"), ex.Message);
                            }

                            #endregion
                        }

                        #endregion
                    }


                    #region Comment : Here Based on user selected LOB prepare "Next Page NavigationUrl"

                    string navigationUrl = GetNextPageNavigationUrl(businessInfoVM);

                    #endregion

                    #region Comment : Here Handled [GUIN-202-SreeRam] Conversion Count(1.a.i) for "UserReports" for WC Quotes

                    if (selectedLob.Abbreviation.Equals("WC", StringComparison.OrdinalIgnoreCase))
                    {
                        //Comment : Here [GUIN-202-Prem] Check for existing values of "QuoteStatus" flags
                        var quoteStatusVM = !appSession.QuoteStatusVM.IsNull() ? appSession.QuoteStatusVM : new QuoteStatusViewModel();
                        quoteStatusVM.LandingSaved = DateTime.Now;

                        IQuoteStatusService quoteStatusService = new QuoteStatusService();
                        var postOperationStatus = quoteStatusService.AddQuoteStatus(
                            new QuoteStatus
                            {
                                QuoteId = wcQuoteId,
                                LandingSaved = quoteStatusVM.LandingSaved,
                                ClassesSelected = quoteStatusVM.ClassesSelected,
                                ExposuresSaved = quoteStatusVM.ExposuresSaved,
                                UserIP = UtilityFunctions.GetUserIPAddress(this.ControllerContext.HttpContext.ApplicationInstance.Context)
                            }, guardServiceProvider);

                        if (!postOperationStatus.RequestSuccessful)
                        {
                            loggingService.Fatal(postOperationStatus.Messages.ToString());
                        }

                        //update QuoteStatus VM in current active session state
                        appSession.QuoteStatusVM = quoteStatusVM;
                    }

                    //Comment : Here finlly update this into session                    
                    SetCustomSession(appSession);

                    #endregion

                    return Json(new { resultStatus = "OK", resultText = "Business information data saved successfully", resultUrl = navigationUrl }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    loggingService.Fatal(ex);
                }

                #endregion

            }

            return Json(new { resultStatus = "NOK", resultText = "Something went wrong while posting questionnaire data !", resultMessages = new List<string>() }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #endregion

        #region Methods : private methods

        /// <summary>
        /// Generate and save Quote ID
        /// </summary>
        /// <param name="appSession"></param>
        /// <param name="businessInfoVM"></param>
        private void GenerateAndSaveQuoteID(BusinessInfoViewModel businessInfoVM)
        {
            int newWcQuoteID = GenerateQuoteId(appSession);
            string secureQuoteId = appSession.SecureQuoteId;

            //Comment : Here CREATE new session from this step onwards
            if (!appSession.IsLanding)
            {
                appSession = new BHIC.ViewDomain.CustomSession();
            }
            appSession.QuoteID = newWcQuoteID;
            appSession.SecureQuoteId = secureQuoteId;
            appSession.ZipCode = businessInfoVM.ZipCode;
            appSession.StateAbbr = businessInfoVM.StateCode;
            SetCustomSession(appSession);
        }

        /// <summary>
        /// Save LOB Data if not exist or changed.
        /// </summary>
        /// <param name="appSession"></param>
        /// <param name="businessInfoVM"></param>
        /// <param name="selectedLobCode"></param>
        private void SaveLOBData(BusinessInfoViewModel businessInfoVM, string selectedLobCode, string lobFullName)
        {
            // Check LOBData Exist
            // If LOBData not exist post LOBData
            var lobDataList = GetLobData(appSession.QuoteID);
            if (!lobDataList.IsNull() && lobDataList.Any())
            {
                // If selected lob changed from previous selected lob
                if (!lobDataList.Any(res => res.Lob.Equals(selectedLobCode, StringComparison.OrdinalIgnoreCase)))
                {
                    // Call Quote ID generation
                    GenerateAndSaveQuoteID(businessInfoVM);
                    // Call LOBData Post
                    PostLobData(businessInfoVM.StateCode, selectedLobCode, lobFullName);
                }
            }
            else
            {
                PostLobData(businessInfoVM.StateCode, selectedLobCode, lobFullName);
            }
        }

        /// <summary>
        /// Return QuoteId stored in cookies with current Http request
        /// </summary>
        /// <returns></returns>
        private int GetCookieQuoteId()
        {
            // stored in a cookie on the user's machine
            int? wcQuoteId = QuoteCookieHelper.Cookie_GetQuoteId(this.ControllerContext.HttpContext);

            //if cookie QuoteId has been expired
            if (wcQuoteId == null)
            {
                throw new ApplicationException(string.Format("{0} : {1} !", BusinessInfoPage, Constants.QuoteIdCookieEmpty));
            }

            return wcQuoteId ?? 0;
        }

        /// <summary>
        /// Get application current session
        /// </summary>
        private void GetSession()
        {
            try
            {
                if (!IsCustomSessionNull())
                {
                    appSession = GetCustomSession();
                }
            }
            catch { }
            finally
            {
                if (IsCustomSessionNull())
                {
                    throw new ApplicationException(string.Format("{0},{1} : {2} !", BusinessInfoPage, Constants.SessionExpiredPartial, Constants.AppCustomSessionExpired));
                }
            }
        }

        /// <summary>
        /// Return interface refernce to make all business logic
        /// </summary>
        /// <returns></returns>
        private IProspectInfoService GetProspectInfoProvider()
        {
            #region Comment : Here ProspectInfo interface refernce to do/make process all business logic

            IProspectInfoService questionnaire = new ProspectInfoService(guardServiceProvider);

            #endregion

            return questionnaire;
        }

        /// <summary>
        /// It will validate BusinessInfo Model
        /// </summary>
        /// <returns>returns list or errors if found, empty otherwise </returns>
        private List<string> ValidateProspectInfoModel(BusinessInfoViewModel businessInfo)
        {
            var listOfErrors = new List<string>();

            try
            {
                if (businessInfo != null)
                {
                    //validated CompanyName for following rulesK
                    //1.company name should not empty
                    //2.length should not more than 150
                    if (string.IsNullOrEmpty(businessInfo.CompanyName))
                    {
                        listOfErrors.Add("Please enter company name");
                    }
                    else if (businessInfo.CompanyName.Length > 150)
                    {
                        listOfErrors.Add("Company name should not be more than 150 characters");
                    }

                    //validated zipCode for following rules
                    //1.Zip Code should not empty
                    //2.Zip Code should be numeric 
                    //3.It should be valid zip code
                    //4.length should not more than 5
                    if (string.IsNullOrEmpty(businessInfo.ZipCode))
                    {
                        listOfErrors.Add("Please enter zip code");
                    }
                    else if (!Regex.IsMatch(businessInfo.ZipCode, @"\d"))
                    {
                        listOfErrors.Add("Invalid zip code");
                    }
                    else if (!IsZipExists(businessInfo.ZipCode))
                    {
                        listOfErrors.Add("Zip code does not exist");
                    }
                    else if (businessInfo.ZipCode.Length > 5)
                    {
                        listOfErrors.Add("ZipCode length should not be more than 5 characters");
                    }

                    //validated Email for following rules
                    //1.Email should not empty
                    //2.It should be valid email pattern
                    //3.length should not more than 128
                    if (string.IsNullOrEmpty(businessInfo.Email))
                    {
                        listOfErrors.Add(Constants.EmptyEmail);
                    }
                    else if (!UtilityFunctions.IsValidRegex(businessInfo.Email, Constants.EmailRegex))
                    {
                        listOfErrors.Add(Constants.InvalidEmail);
                    }
                    else if (businessInfo.Email.Length > 128)
                    {
                        listOfErrors.Add("Email length should not be more than 128 characters");
                    }

                    //if PhoneNumber provided, validate for following rules
                    //1.Phone number should be in valid format
                    if (!string.IsNullOrEmpty(businessInfo.PhoneNumber) && !UtilityFunctions.IsValidRegex(businessInfo.PhoneNumber, Constants.PhoneRegex))
                    {
                        listOfErrors.Add("Please enter your phone number");
                    }

                    //if ContactName provided, validate for following rules
                    //1.Conatct Name length should not more than 256
                    if (!string.IsNullOrEmpty(businessInfo.ContactName) && (businessInfo.ContactName.Length > 256))
                    {
                        listOfErrors.Add("Contact Name should not be more than 256 characters");
                    }

                    ////if Address1 provided, validate for following rules
                    ////1.Address1 length should not more than 50
                    //if (!string.IsNullOrEmpty(businessInfo.Address1) && ( businessInfo.Address1.Length > 50))
                    //{
                    //    listOfErrors.Add("Address should not more than 50");
                    //}

                    ////if City provided, validate for following rules
                    ////1.It should be valid city and zip combination
                    //if (!string.IsNullOrEmpty(businessInfo.City) && !IsValidCityStateZipCombination(businessInfo.City, businessInfo.StateCode, businessInfo.ZipCode))
                    //{
                    //    listOfErrors.Add(Constants.InvalidCityStateZip);
                    //}

                    if (string.IsNullOrEmpty(businessInfo.LobId))
                    {
                        listOfErrors.Add("Something went wrong!!Please try again");
                    }

                    //validate state and zip combination
                    if (!string.IsNullOrEmpty(businessInfo.StateCode) && !string.IsNullOrEmpty(businessInfo.ZipCode))
                    {
                        //validate whether is it valid zip and state combination
                        if (!IsValidZipStateCombination(businessInfo.ZipCode, businessInfo.StateCode))
                        {
                            if (!listOfErrors.Contains("Something went wrong!!Please try again"))
                            {
                                listOfErrors.Add("Something went wrong!!Please try again");
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(businessInfo.LobId))
                            {
                                var validLobList = string.IsNullOrEmpty(businessInfo.StateCode) ? null : GetLobList(businessInfo.StateCode);

                                if (validLobList != null && validLobList.Count > 0)
                                {
                                    //validated lob id for following rules
                                    //1.provided lob id should exist with having "available" status
                                    if (!validLobList.Any(x => x.Id == int.Parse(businessInfo.LobId) && x.Status.Equals("Available", StringComparison.OrdinalIgnoreCase)))
                                    {
                                        listOfErrors.Add("Selected product is not available at given Zip Code");
                                    }
                                }
                                else
                                {
                                    listOfErrors.Add("Products not yet available for this zipcode");
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!listOfErrors.Contains("Something went wrong!!Please try again"))
                        {
                            listOfErrors.Add("Something went wrong!!Please try again");
                        }
                    }

                }
                else
                {
                    listOfErrors.Add(Constants.ModelStateError);
                }

            }
            catch (Exception ex)
            {
                loggingService.Fatal(ex);
                if (!listOfErrors.Contains("Something went wrong!!Please try again"))
                {
                    listOfErrors.Add("Something went wrong!!Please try again");
                }
            }

            return listOfErrors;
        }

        /// <summary>
        /// Get Name of lob based on state code
        /// </summary>
        /// <param name="stateCode">State code</param>
        /// <returns>Returns lob filterd by state</returns>
        private List<LineOfBusiness> GetLobList(string stateCode)
        {
            ILineOfBusinessProvider lob = new LineOfBusinessProvider();
            List<LineOfBusiness> lobList = new List<LineOfBusiness>();

            if (!string.IsNullOrEmpty(stateCode))
            {
                ILineOfBusinessService lobService = new LineOfBusinessService();

                lobList = lobService.GetLineOfBusiness().Where(x => x.StateCode.Equals(stateCode, StringComparison.Ordinal)).ToList();
            }

            //if given parameter value exist return true,false otherwise
            return lobList;
        }

        private string GetNextPageNavigationUrl(BusinessInfoViewModel businessInfoVM)
        {
            string lobId = businessInfoVM.LobId ?? string.Empty;
            switch (lobId)
            {
                case "1":
                    return "/GetExposureDetails/";
                case "2":
                    return string.Concat(GetSchemeAndHostURLPart(), string.Format(ConfigCommonKeyReader.BopUrl, Server.UrlEncode(appSession.SecureQuoteId)));
                case "3":
                    return "/PurchasePath/CAQuote/Index";
                default:
                    return string.Empty;
            }
        }

        private string GetBOPPath(string zipCode, string state)
        {
            WalkThroughRequestParms walkThroughRequest = new WalkThroughRequestParms
            {
                ZipCode = zipCode,
                State = state
            };

            //append params to url
            var bopUrl = string.Concat(GetSchemeAndHostURLPart() //"https://yyy3.coveryourbusiness.com" Temporary for local deployement set Hard-coded
                , ConfigCommonKeyReader.BopUrl, UtilityFunctions.CreateQueryString(walkThroughRequest));

            return bopUrl;
        }

        private OperationStatus PostLobData(string state, string selectedLob, string lobFullName)
        {
            //fetch selected lob info
            //var selectedLob = GetLobList(state).FirstOrDefault(x => x.Id == Convert.ToInt32(lobId));
            OperationStatus lobDataResponse = null;
            appSession = GetCustomSession();
            //if quote id and selected Abbreviation exist, Post info to LobData
            //if (selectedLob != null && !string.IsNullOrEmpty(selectedLob.Abbreviation) && wcQuoteId > 0)
            {
                //post info to LobData api
                //Bop Abbreviation should be send as BP for Lob Post api call
                ILobDataService lobDataService = new LobDataService(guardServiceProvider);
                lobDataResponse = lobDataService.AddLobData(new LobData
                {
                    QuoteId = appSession.QuoteID,
                    Lob = selectedLob,
                    LobFriendlyName = lobFullName
                });
            }
            return lobDataResponse;
        }

        private List<LobData> GetLobData(int intQuoteId)
        {
            ILobDataService lobService = new LobDataService(guardServiceProvider);
            var res = lobService.GetLobDataList(new LobDataRequestParms()
            {
                QuoteId = intQuoteId,
                IncludeRelated = true // This is used to read child objects for all.
            });
            return res;
        }

        private string GetLobId(int quoteId)
        {
            var res = GetLobData(quoteId);
            if (!res.IsNull() && res.Any())
            {
                switch (res[0].Lob)
                {
                    case "WC": return "1";
                    case "BP": return "2";
                    case "CA": return "3";
                    default: return null;
                }
            }
            else
                return null;
        }

        #endregion
    }
}
