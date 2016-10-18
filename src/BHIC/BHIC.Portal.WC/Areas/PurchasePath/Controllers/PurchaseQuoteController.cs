using BHIC.Common;
using BHIC.Common.Client;
using BHIC.Common.CommonUtilities;
using BHIC.Common.Config;
using BHIC.Common.Configuration;
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
using BHIC.Domain.Background;
using BHIC.Domain.Dashboard;
using BHIC.Domain.Policy;
using BHIC.Domain.QuestionEngine;
using BHIC.Domain.Service;
using BHIC.Portal.WC.App_Start;
using BHIC.ViewDomain;
using BHIC.ViewDomain.Landing;
using BHIC.ViewDomain.Mailing;
using BHIC.ViewDomain.QuestionEngine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.Mvc;
using DML_DTO = BHIC.DML.WC.DTO;

namespace BHIC.Portal.WC.Areas.PurchasePath.Controllers
{
    //[CustomTransactionLogFilterAttribute]
    [ValidateSession]
    [CustomAntiForgeryToken]
    public class PurchaseQuoteController : BaseController
    {
        //
        // GET: /PurchasePath/PurchaseQuote/
        //ServiceProvider provider;
        IBusinessTypeService businessTypeService;
        CustomSession customSession;
        IVCityStateZipCodeService vCityStateZipService;
        int? loggedUserId;
        public PurchaseQuoteController()
        {
            //provider = new GuardServiceProvider() { ServiceCategory = Constants.GetLineOfBusiness(Constants.LineOfBusiness.WC) };
            businessTypeService = new BusinessTypeService();
            vCityStateZipService = new VCityStateZipCodeService();
        }

        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult Quote()
        {
            //Comment : Here New implementation by Prem on 26.05.2016 (To handle un-authorized requests)
            IsValidPageRequest();

            return PartialView("_PurchaseQuote");
        }

        public JsonResult GetQuotePurchasePageDefaults(string quoteId)
        {
            #region Comment : Here newly added by Prem on 05.01.2015 to handle SaveForLater issues
            customSession = GetCustomSessionWithQuoteVM();

            if (!string.IsNullOrWhiteSpace(quoteId))
            {
                #region Comment : Here if request query string is found(means request came from mail embedded link) then do following action

                //get quote-id in int format for future references
                int intQuoteId = 0;

                ICommonFunctionality commonFunctionality = GetCommonFunctionalityProvider();
                customSession = RetrieveSavedQuote(quoteId, commonFunctionality, out intQuoteId);

                //Comment : Here if able to retrieve saved session from DB layer then do following
                if (customSession != null)
                {
                    //Comment : Here finlly update this into session
                    //SetCustomSession(appSession);

                    //Save/Update QuoteId
                    QuoteCookieHelper.Cookie_SaveQuoteId(this.ControllerContext.HttpContext, intQuoteId);
                }

                #endregion
            }

            #endregion

            //customSession = GetCustomSessionWithQuoteVM();
            PurchaseQuote purchaseQuote = new PurchaseQuote();
            dynamic cityList = null;
            StateUINumberClass stateUINDetails = null;
            var loggedInUserDetails = GetLoggedInUserDetails();

            try
            {
                if (customSession.QuoteVM.County.IsNull())
                    customSession.QuoteVM.County = new County();
                customSession.QuoteVM.County.ZipCode = customSession.ZipCode;
                customSession.QuoteVM.County.State = customSession.StateAbbr;
                stateUINDetails = purchaseQuote.GetStatesData(customSession.StateAbbr, string.Empty);

                customSession.StateUINumberRequired = !stateUINDetails.IsNull();

                cityList = GetCityList(customSession);
                ((BusinessTypeService)businessTypeService).ServiceProvider = guardServiceProvider;
                List<BHIC.Domain.Background.BusinessType> businessTypeResponse = businessTypeService.GetBusinessTypes(
                                                            new BusinessTypeRequestParms() { InsuredNameTypesOnly = true });

                if (!loggedInUserDetails.IsNull())
                {
                    //Remove the password of loggedInUser
                    loggedInUserDetails.Password = string.Empty;
                    AssignPolicyCenterLoggedUserInSession(loggedInUserDetails);

                    if (customSession.PurchaseVM.IsNull())
                        customSession.PurchaseVM = new WcPurchaseViewModel();
                    if (customSession.PurchaseVM.Account.IsNull())
                        customSession.PurchaseVM.Account = new Account();
                    customSession.PurchaseVM.Account.Email = loggedInUserDetails.Email;

                    AccountRegistrationService accountRgistrationService = new AccountRegistrationService(guardServiceProvider);
                    UserRegistration user = new UserRegistration();
                    user.Email = loggedInUserDetails.Email;
                    user = accountRgistrationService.GetUserDetails(user);
                    loggedInUserDetails.Password = user.Password;
                    customSession.PurchaseVM.Account.Password = user.Password;
                }
                SetCustomSession(customSession);

                //Comment : Progress bar nagivation binding based on flag 
                List<NavigationModel> links = new List<NavigationModel>();
                NavigationController nc = new NavigationController();
                links = nc.GetProgressBarLinks(customSession.PageFlag);

                if (!businessTypeResponse.IsNull() && businessTypeResponse.Count > 0)
                {
                    return Json(new { stateUINDetails = stateUINDetails, loggedInUserDetails = loggedInUserDetails, cityList = cityList, countyData = (!customSession.QuoteVM.IsNull() && !customSession.QuoteVM.County.IsNull()) ? customSession.QuoteVM.County : new County(), businessTypeList = businessTypeResponse, taxIdNumber = customSession.QuestionnaireVM.TaxIdNumber, taxIdType = customSession.QuestionnaireVM.TaxIdType, NavLinks = links }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { stateUINDetails = stateUINDetails, loggedInUserDetails = loggedInUserDetails, cityList = cityList, countyData = (!customSession.QuoteVM.IsNull() && !customSession.QuoteVM.County.IsNull()) ? customSession.QuoteVM.County : new County(), taxIdNumber = customSession.QuestionnaireVM.TaxIdNumber, taxIdType = customSession.QuestionnaireVM.TaxIdType, NavLinks = links }, JsonRequestBehavior.AllowGet);
                }


            }
            catch (ApplicationException ex)
            {
                BHIC.Common.Logging.ILoggingService logger = BHIC.Common.Logging.LoggingService.Instance;
                logger.Fatal(string.Format("Service {0} Call with error message : {1}", "businessTypeService", ex.ToString()));
                return Json(new { stateUINDetails = stateUINDetails, loggedInUserDetails = loggedInUserDetails, cityList = cityList, countyData = !(customSession.QuoteVM.IsNull() && customSession.QuoteVM.County.IsNull()) ? customSession.QuoteVM.County : new County() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetPurchaseQuoteData(string quoteId)
        {
            int wcQuoteId;
            customSession = GetCustomSession();
            //Comment : Here added this logic to avoid repitetive call for SaveSessionState
            if (!string.IsNullOrWhiteSpace(quoteId))
            {
                #region Comment : Here if request query string is found(means request came from mail embedded link) then do following action

                //get quote-id in int format for future references
                int intQuoteId = 0;

                ICommonFunctionality commonFunctionality = GetCommonFunctionalityProvider();
                customSession = RetrieveSavedQuote(quoteId, commonFunctionality, out intQuoteId);

                //Comment : Here if able to retrieve saved session from DB layer then do following
                if (customSession != null)
                {
                    //Comment : Here finlly update this into session
                    //SetCustomSession(appSession);

                    //Save/Update QuoteId
                    HttpContextBase context = this.ControllerContext.HttpContext;
                    QuoteCookieHelper.Cookie_SaveQuoteId(context, intQuoteId);

                }

                #endregion

                //wcQuoteId = GetDecryptedQuoteId(quoteId);
                wcQuoteId = intQuoteId;
                return Json(new { model = customSession.PurchaseVM }, JsonRequestBehavior.AllowGet);
            }
            else if (!customSession.IsNull() && !customSession.PurchaseVM.IsNull())
            {
                //Comment : Progress bar nagivation binding based on flag 
                List<NavigationModel> links = new List<NavigationModel>();
                NavigationController nc = new NavigationController();
                wcQuoteId = QuoteCookieHelper.Cookie_GetQuoteId(this.ControllerContext.HttpContext);
                PurchaseQuote purchaseQuote = new PurchaseQuote();
                var quoteData = purchaseQuote.GetPurchaseQuoteData(wcQuoteId, string.Empty);
                PopulateAffectedIds(quoteData, wcQuoteId);
                links = nc.GetProgressBarLinks(customSession.PageFlag);
                if (!customSession.BusinessInfoVM.IsNull())
                {
                    if (customSession.PurchaseVM.MailingAddress.IsNull())
                    {
                        customSession.PurchaseVM.MailingAddress = new MailingAddress();
                    }
                    //customSession.PurchaseVM.MailingAddress.AddressLine1 = customSession.BusinessInfoVM.Address1 ?? string.Empty;
                    //customSession.PurchaseVM.MailingAddress.City = customSession.BusinessInfoVM.City ?? string.Empty;
                    customSession.PurchaseVM.MailingAddress.State = customSession.StateAbbr;
                    customSession.PurchaseVM.MailingAddress.Zip = customSession.BusinessInfoVM.ZipCode;

                    if (customSession.PurchaseVM.BusinessInfo.IsNull())
                    {
                        customSession.PurchaseVM.BusinessInfo = new BusinessInfo();
                    }
                    customSession.PurchaseVM.BusinessInfo.BusinessName = string.IsNullOrWhiteSpace(customSession.PurchaseVM.BusinessInfo.BusinessName) ? customSession.BusinessInfoVM.CompanyName : customSession.PurchaseVM.BusinessInfo.BusinessName;
                    if (customSession.PurchaseVM.PersonalContact.IsNull())
                    {
                        customSession.PurchaseVM.PersonalContact = new PersonalContact();
                        customSession.PurchaseVM.PersonalContact.SameAsContact = true;
                    }
                    if (!string.IsNullOrWhiteSpace(customSession.BusinessInfoVM.ContactName))
                    {
                        var contactName = customSession.BusinessInfoVM.ContactName.Split(' ');
                        if (contactName.Length > 0)
                        {
                            customSession.PurchaseVM.PersonalContact.FirstName = string.IsNullOrWhiteSpace(customSession.PurchaseVM.PersonalContact.FirstName) ? contactName[0] : customSession.PurchaseVM.PersonalContact.FirstName;
                            customSession.PurchaseVM.PersonalContact.LastName = string.IsNullOrWhiteSpace(customSession.PurchaseVM.PersonalContact.LastName) ? (contactName.Length > 1 ? contactName[contactName.Length - 1] : string.Empty) : customSession.PurchaseVM.PersonalContact.LastName;
                        }
                    }
                    customSession.PurchaseVM.PersonalContact.Email = string.IsNullOrWhiteSpace(customSession.PurchaseVM.PersonalContact.Email) ? customSession.BusinessInfoVM.Email : customSession.PurchaseVM.PersonalContact.Email;
                    customSession.PurchaseVM.PersonalContact.PhoneNumber = string.IsNullOrWhiteSpace(customSession.PurchaseVM.PersonalContact.PhoneNumber) ? customSession.BusinessInfoVM.PhoneNumber : customSession.PurchaseVM.PersonalContact.PhoneNumber;
                    if (customSession.PurchaseVM.Account.IsNull())
                    {
                        customSession.PurchaseVM.Account = new Account();
                    }
                    customSession.PurchaseVM.Account.Email = string.IsNullOrWhiteSpace(customSession.PurchaseVM.Account.Email) ? customSession.BusinessInfoVM.Email : customSession.PurchaseVM.Account.Email;
                }

                return Json(new { model = customSession.PurchaseVM, NavLinks = links }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                wcQuoteId = QuoteCookieHelper.Cookie_GetQuoteId(this.ControllerContext.HttpContext);
                PurchaseQuote purchaseQuote = new PurchaseQuote();
                var quoteData = purchaseQuote.GetPurchaseQuoteData(wcQuoteId, string.Empty);
                PopulateAffectedIds(quoteData, wcQuoteId);
                WcPurchaseViewModel model = new WcPurchaseViewModel();
                SetQuoteData(ref model, quoteData);
                if (!customSession.BusinessInfoVM.IsNull())
                {
                    if (model.MailingAddress.IsNull())
                    {
                        model.MailingAddress = new MailingAddress();
                    }
                    // model.MailingAddress.AddressLine1 = customSession.BusinessInfoVM.Address1 ?? string.Empty;
                    // model.MailingAddress.City = customSession.BusinessInfoVM.City;
                    model.MailingAddress.State = customSession.StateAbbr;
                    model.MailingAddress.Zip = customSession.BusinessInfoVM.ZipCode;

                    if (model.BusinessInfo.IsNull())
                    {
                        model.BusinessInfo = new BusinessInfo();
                    }
                    model.BusinessInfo.BusinessName = string.IsNullOrWhiteSpace(model.BusinessInfo.BusinessName) ? customSession.BusinessInfoVM.CompanyName : model.BusinessInfo.BusinessName;
                    if (model.PersonalContact.IsNull())
                    {
                        model.PersonalContact = new PersonalContact();
                        model.PersonalContact.SameAsContact = true;
                    }
                    if (!string.IsNullOrWhiteSpace(customSession.BusinessInfoVM.ContactName))
                    {
                        var contactName = customSession.BusinessInfoVM.ContactName.Split(' ');
                        if (contactName.Length > 0)
                        {
                            model.PersonalContact.FirstName = string.IsNullOrWhiteSpace(model.PersonalContact.FirstName) ? contactName[0] : model.PersonalContact.FirstName;
                            model.PersonalContact.LastName = string.IsNullOrWhiteSpace(model.PersonalContact.LastName) ? (contactName.Length > 1 ? contactName[contactName.Length - 1] : string.Empty) : model.PersonalContact.LastName;
                        }
                    }
                    model.PersonalContact.Email = string.IsNullOrWhiteSpace(model.PersonalContact.Email) ? customSession.BusinessInfoVM.Email : model.PersonalContact.Email;
                    model.PersonalContact.PhoneNumber = string.IsNullOrWhiteSpace(model.PersonalContact.PhoneNumber) ? customSession.BusinessInfoVM.PhoneNumber : model.PersonalContact.PhoneNumber;
                    if (model.Account.IsNull())
                    {
                        model.Account = new Account();
                    }
                    model.Account.Email = string.IsNullOrWhiteSpace(model.Account.Email) ? customSession.BusinessInfoVM.Email : model.Account.Email;
                }

                return Json(new { model = model }, JsonRequestBehavior.AllowGet);
            }
        }

        private void SetQuoteData(ref WcPurchaseViewModel model, Quote quoteData)
        {
            if (!quoteData.IsNull())
            {
                model.PersonalContact = new PersonalContact();
                customSession = GetCustomSession();
                model.PersonalContact.SameAsContact = (customSession.PurchaseVM.IsNull() || customSession.PurchaseVM.PersonalContact.IsNull()) ? true : customSession.PurchaseVM.PersonalContact.SameAsContact;
                
                if (!quoteData.Contacts.IsNull() && quoteData.Contacts.Count > 0)
                {
                    Contact personalContact = quoteData.Contacts.First(x => x.ContactType == "Misc");
                    if (!personalContact.IsNull())
                    {
                        model.PersonalContact.Email = personalContact.Email;
                        model.PersonalContact.FirstName = personalContact.Name;
                        model.PersonalContact.LastName = personalContact.Name;
                        if (!personalContact.Phones.IsNull() && personalContact.Phones.Count > 0)
                        {
                            model.PersonalContact.PhoneNumber = personalContact.Phones[0].PhoneNumber + personalContact.Phones[0].Extension;
                        }
                    }

                    Contact businessContact = quoteData.Contacts.First(x => x.ContactType == "Billing");
                    if (!businessContact.IsNull())
                    {
                        model.BusinessContact = new BusinessContact();
                        model.BusinessContact.Email = businessContact.Email;
                        model.BusinessContact.FirstName = businessContact.Name;
                        model.BusinessContact.LastName = businessContact.Name;
                        if (!businessContact.Phones.IsNull() && businessContact.Phones.Count > 0)
                        {
                            model.BusinessContact.PhoneNumber = businessContact.Phones[0].PhoneNumber + businessContact.Phones[0].Extension;
                        }
                    }
                }
                if (!quoteData.Locations.IsNull() && quoteData.Locations.Count > 0)
                {
                    model.MailingAddress = new MailingAddress();
                    model.MailingAddress.AddressLine1 = quoteData.Locations[0].Addr1;
                    model.MailingAddress.AddressLine2 = quoteData.Locations[0].Addr2;
                    model.MailingAddress.City = quoteData.Locations[0].City;
                    model.MailingAddress.State = quoteData.Locations[0].State;
                    model.MailingAddress.Zip = quoteData.Locations[0].Zip;
                }
                if (!quoteData.InsuredNames.IsNull() && quoteData.InsuredNames.Count > 0)
                {
                    model.BusinessInfo = new BusinessInfo();
                    model.BusinessInfo.BusinessName = quoteData.InsuredNames[0].Name;
                    model.BusinessInfo.FirstName = quoteData.InsuredNames[0].FirstName;
                    model.BusinessInfo.MiddleName = quoteData.InsuredNames[0].MiddleName;
                    model.BusinessInfo.LastName = quoteData.InsuredNames[0].LastName;
                    model.BusinessInfo.BusinessType = quoteData.InsuredNames[0].NameType;
                    model.BusinessInfo.TaxIdType = quoteData.InsuredNames[0].FEINType;
                    model.BusinessInfo.TaxIdOrSSN = quoteData.InsuredNames[0].FEIN;
                }
            }
        }

        /// <summary>
        /// Return status whether this page resuest is valid or un-authorized
        /// </summary>
        /// <returns></returns>
        private bool IsValidPageRequest()
        {
            //Comment : Here check is this/current quote have associated quesionnaire QuoteStatus as "Quote" and  quote-summary object otherwise invalid user-info page request
            //if (GetCustomSession().QuoteSummaryVM.IsNull())
            //{
            //    throw new ApplicationException(string.Format("{0},{1} : You are not authorized to get user-info page !", "UserInfoPage", Constants.UnauthorizedRequest));
            //}

            if (GetCustomSession().PageFlag < 4)
            {
                throw new ApplicationException(string.Format("{0},{1} : You are not authorized to get user-info page !", "UserInfoPage", Constants.UnauthorizedRequest));
            }
            return true;
        }

        public bool InValidateStateCityZip(string zipCode, string city, string state)
        {
            //Comment : Here set default return status TRUE or INVALID for suplied zip,state,city combination 
            bool retStatus = true;
            customSession = GetCustomSessionWithPurchaseVM();
            if (customSession.PurchaseVM.MailingAddress.IsNull())
            {
                customSession.PurchaseVM.MailingAddress = new MailingAddress();
            }
            customSession.PurchaseVM.MailingAddress.City = city;
            customSession.PurchaseVM.MailingAddress.State = state;
            customSession.PurchaseVM.MailingAddress.Zip = zipCode;
            SetCustomSession(customSession);
            try
            {
                //return county list filtered by query parameter
                retStatus = IsValidCityStateZipCombination(city, state, zipCode);

            }
            catch (Exception ex)
            {
                BHIC.Common.Logging.ILoggingService logger = BHIC.Common.Logging.LoggingService.Instance;
                logger.Fatal(string.Format("Service {0} Call with error message : {1}", "vCityStateZipService", ex.ToString()));
            }

            return (!retStatus);
        }

        public List<County> GetCityList(CustomSession customSession)
        {
            return GetAllCitiesByStateAndZip(!String.IsNullOrWhiteSpace(customSession.ZipCode) ? customSession.ZipCode.Trim() : String.Empty, !String.IsNullOrWhiteSpace(customSession.StateAbbr) ? customSession.StateAbbr.Trim() : String.Empty);
        }

        [HttpPost]
        /// <summary>
        /// Post Purchase details
        /// </summary>
        /// <returns></returns>
        public JsonResult Purchase(WcPurchaseViewModel model)
        {
            var errorlist = ValidatePurchaseModel(model);

            if (errorlist.IsNull() || errorlist.Count == 0)
            {
                //Set Quote Purchase VM in session for back To Modify functionality
                customSession = GetCustomSession();

                var effectedIds = !customSession.PurchaseVM.IsNull() ? customSession.PurchaseVM.EffectedIds : null; ;
                customSession.PurchaseVM = model;
                customSession.PurchaseVM.EffectedIds = effectedIds;
                //SetCustomSession(customSession);

                int wcQuoteId = QuoteCookieHelper.Cookie_GetQuoteId(this.ControllerContext.HttpContext);
                // if there's no active quote (e.g. - if the session expired), throw error
                if (!UtilityFunctions.IsValidInt(wcQuoteId))
                {
                    return Json(new
                    {
                        response = false,
                        message = Constants.QuoteCaptureErrors.SESSION_EXPIRED
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    #region Comment : Here collect all posted information into targeted DTOs

                    var personalPhone = model.PersonalContact.PhoneNumber.Split('x');//(222) 222-2222 x22222 model.PersonalContact.PhoneNumber
                    var businessPhone = model.BusinessContact.PhoneNumber.Split('x');//(222) 222-2222 x22222 model.BusinessContact.PhoneNumber

                    //Comment : here collect "Additional Business Information"
                    var insuredNameData = new InsuredName();
                    IInsuredNameService insuredNameService = new InsuredNameService(guardServiceProvider);
                    var insuredNameResponse = insuredNameService.GetInsuredNameList(new InsuredNameRequestParms { QuoteId = wcQuoteId });
                    insuredNameData.QuoteId = wcQuoteId;
                    if (!insuredNameResponse.IsNull() && insuredNameResponse.Count > 0)
                    {
                        insuredNameData.InsuredNameId = insuredNameResponse[0].InsuredNameId;
                        insuredNameData.FEIN = insuredNameResponse[0].FEIN;
                        insuredNameData.PrimaryInsuredName = insuredNameResponse[0].PrimaryInsuredName;
                        insuredNameData.FirstName = insuredNameResponse[0].FirstName;
                        insuredNameData.LastName = insuredNameResponse[0].LastName;
                        insuredNameData.MiddleName = insuredNameResponse[0].MiddleName;
                    }
                    else
                    {
                        insuredNameData.PrimaryInsuredName = true;
                    }
                    if (!model.BusinessInfo.IsNull())
                    {
                        if (!model.BusinessInfo.BusinessType.IsNull())
                        {
                            insuredNameData.NameType = model.BusinessInfo.BusinessType;
                        }
                        if (model.BusinessInfo.BusinessType == "I")
                        {
                            insuredNameData.FirstName = model.BusinessInfo.FirstName;
                            insuredNameData.MiddleName = model.BusinessInfo.MiddleName;
                            insuredNameData.LastName = model.BusinessInfo.LastName;
                        }
                        else
                        {
                            insuredNameData.Name = model.BusinessInfo.BusinessName;
                        }
                        insuredNameData.FEINType = model.BusinessInfo.TaxIdType;
                        insuredNameData.FEIN = UtilityFunctions.ToNumeric(model.BusinessInfo.TaxIdOrSSN);//"333-333333";
                    }



                    //Comment : here collect "Business Mailing Address"
                    var effectedObjectId = GetItemByKey("BusinessContactAddressDataId", customSession.PurchaseVM.EffectedIds);
                    var addressData = new Address();
                    addressData.AddressId = !string.IsNullOrWhiteSpace(effectedObjectId) ? Int32.Parse(effectedObjectId) : (int?)null;
                    addressData.Addr1 = model.MailingAddress.AddressLine1;
                    addressData.Addr2 = model.MailingAddress.AddressLine2;
                    addressData.State = model.MailingAddress.State ?? String.Empty;
                    addressData.City = model.MailingAddress.City ?? String.Empty;
                    addressData.Zip = model.MailingAddress.Zip ?? String.Empty;

                    effectedObjectId = GetItemByKey("PersonalContactPhoneId", customSession.PurchaseVM.EffectedIds);
                    var personalContactPhone = new Phone();
                    personalContactPhone.PhoneId = !string.IsNullOrWhiteSpace(effectedObjectId) ? Int32.Parse(effectedObjectId) : (int?)null;
                    personalContactPhone.PhoneType = "H";
                    personalContactPhone.PhoneNumber = model.PersonalContact.PhoneNumber.Contains('x') ? UtilityFunctions.ToNumeric(personalPhone[0]) : model.PersonalContact.PhoneNumber.Substring(0, 10);
                    personalContactPhone.Extension = model.PersonalContact.PhoneNumber.Contains('x') ? UtilityFunctions.ToNumeric(personalPhone[1]) : model.PersonalContact.PhoneNumber.Substring(10, 4);

                    var businessContactPhone = new Phone();
                    businessContactPhone.PhoneType = "B";
                    effectedObjectId = GetItemByKey("BusinessContactPhoneId", customSession.PurchaseVM.EffectedIds);
                    businessContactPhone.PhoneId = !string.IsNullOrWhiteSpace(effectedObjectId) ? Int32.Parse(effectedObjectId) : (int?)null;
                    businessContactPhone.PhoneNumber = model.BusinessContact.PhoneNumber.Contains('x') ? UtilityFunctions.ToNumeric(businessPhone[0]) : model.BusinessContact.PhoneNumber.Substring(0, 10);
                    businessContactPhone.Extension = model.BusinessContact.PhoneNumber.Contains('x') ? UtilityFunctions.ToNumeric(businessPhone[1]) : model.BusinessContact.PhoneNumber.Substring(10, 4);

                    //Comment : Here create contact object 
                    var personalcontactData = new Contact();
                    personalcontactData.QuoteId = wcQuoteId;
                    effectedObjectId = GetItemByKey("PersonalContactDataId", customSession.PurchaseVM.EffectedIds);
                    personalcontactData.ContactId = !string.IsNullOrWhiteSpace(effectedObjectId) ? Int32.Parse(effectedObjectId) : (int?)null;
                    personalcontactData.Name = string.Concat(model.PersonalContact.FirstName, " ", model.PersonalContact.LastName);
                    personalcontactData.ContactType = "Misc";
                    personalcontactData.Email = model.PersonalContact.Email;
                    personalcontactData.Phones = new List<Phone>();
                    //personalcontactData.Phones.Add(personalContactPhone);

                    var businesscontactData = new Contact();
                    businesscontactData.QuoteId = wcQuoteId;
                    effectedObjectId = GetItemByKey("BusinessContactDataId", customSession.PurchaseVM.EffectedIds);
                    businesscontactData.ContactId = !string.IsNullOrWhiteSpace(effectedObjectId) ? Int32.Parse(effectedObjectId) : (int?)null;
                    businesscontactData.Name = string.Concat(model.BusinessContact.FirstName, " ", model.BusinessContact.LastName);
                    businesscontactData.Company = model.BusinessInfo.BusinessName;
                    businesscontactData.ContactType = "Billing";
                    businesscontactData.Email = model.BusinessContact.Email;
                    //businesscontactData.Addresses = new List<Address> { addressData };            //Right now child object submission is not exposed by Guard ex. Address in Contact details
                    businesscontactData.Phones = new List<Phone>();        //Right now child object submission is not exposed by Guard ex. Phones in Contact details
                    //businesscontactData.Phones.Add(businessContactPhone);

                    // populate a BatchAction that will be used to submit the InsuredName DTO to the Insurance Service
                    effectedObjectId = GetItemByKey("LocationsDataId", customSession.PurchaseVM.EffectedIds);
                    Location locationData = new Location();
                    locationData.QuoteId = wcQuoteId;
                    locationData.LocationId = !string.IsNullOrWhiteSpace(effectedObjectId) ? Int32.Parse(effectedObjectId) : (int?)null;
                    locationData.PrimaryCaMailAddr = model.MailingAddress.State.Equals("CA", StringComparison.OrdinalIgnoreCase);
                    locationData.LocationType = "M";
                    locationData.Addr1 = model.MailingAddress.AddressLine1;
                    locationData.Addr2 = model.MailingAddress.AddressLine2;
                    locationData.City = model.MailingAddress.City;
                    locationData.State = model.MailingAddress.State;
                    locationData.Zip = model.MailingAddress.Zip;

                    #endregion

                    #region Comment : Here create BatchAction list object

                    /*----------------------------------------
                populate a BatchActionList that contains all requests to be sent to the Gaurd API
                ----------------------------------------*/

                    var batchActionList = new BatchActionList();	// BatchActionList: list of actions routed to the Insurance Service for response
                    BatchResponseList batchResponseList;			// BatchResponseList: list of responses returned from the Insurance Service
                    string jsonResponse;							// JSON-formated response data returned from the Insurance Service

                    #endregion

                    #region Comment : Here First create batch action for "Contact - Personal" and "Contact - Business" API and based on EffectedIds POST related child object POST in Next Batch

                    string personalContactId = null, businessContactId = null;

                    // populate a BatchAction that will be used to submit the Contact ("Personal") DTO to the Insurance Service
                    var contactPersonalAction = new BatchAction { ServiceName = "Contacts", ServiceMethod = "POST", RequestIdentifier = "ContactsPersonal Data" };
                    contactPersonalAction.BatchActionParameters.Add(new BatchActionParameter { Name = "contact", Value = JsonConvert.SerializeObject(personalcontactData) });
                    batchActionList.BatchActions.Add(contactPersonalAction);

                    // populate a BatchAction that will be used to submit the Contact ("Business") DTO to the Insurance Service
                    var contactBusinessAction = new BatchAction { ServiceName = "Contacts", ServiceMethod = "POST", RequestIdentifier = "ContactsBusiness Data" };
                    contactBusinessAction.BatchActionParameters.Add(new BatchActionParameter { Name = "contact", Value = JsonConvert.SerializeObject(businesscontactData) });
                    batchActionList.BatchActions.Add(contactBusinessAction);

                    // submit the BatchActionList to the Insurance Service
                    jsonResponse = SvcClient.CallServiceBatch(batchActionList, guardServiceProvider);

                    // flag that determines how to respond after processing the response received
                    bool success = true;

                    // deserialize the results into a BatchResponseList
                    batchResponseList = JsonConvert.DeserializeObject<BatchResponseList>(jsonResponse);

                    try
                    {
                        //perosnal contacts details
                        var serviceResponse = batchResponseList.BatchResponses.SingleOrDefault(m => m.RequestSuccessful &&
                                                            m.RequestIdentifier == "ContactsPersonal Data").JsonResponse;

                        if (serviceResponse != null)
                        {
                            var operationStatusDeserialized = JsonConvert.DeserializeObject<OperationStatus>(serviceResponse);
                            personalContactId = operationStatusDeserialized.AffectedIds.SingleOrDefault(m => m.DTOProperty == "ContactId").IdValue;
                        }

                        //business contacts details
                        serviceResponse = batchResponseList.BatchResponses.SingleOrDefault(m => m.RequestSuccessful &&
                                                            m.RequestIdentifier == "ContactsBusiness Data").JsonResponse;

                        if (serviceResponse != null)
                        {
                            var operationStatusDeserialized = JsonConvert.DeserializeObject<OperationStatus>(serviceResponse);
                            businessContactId = operationStatusDeserialized.AffectedIds.SingleOrDefault(m => m.DTOProperty == "ContactId").IdValue;
                        }
                    }
                    catch (Exception)
                    {
                    }

                    //Comment : Here Reset batchaction list
                    batchActionList = new BatchActionList();
                    batchResponseList = null;

                    #endregion

                    #region Comment : Here other batch action "Phone" and "Address" and "InsuredNames" and "UserProfile" creation POST APIs

                    #region Comment : Here if contact data submitted then only add related child objects in next batch execution

                    if (!string.IsNullOrEmpty(personalContactId))
                    {
                        personalContactPhone.ContactId = int.Parse(personalContactId);

                        // populate a BatchAction that will be used to submit the Contact ("Personal") DTO to the Insurance Service
                        var phoneContactPersonalAction = new BatchAction { ServiceName = "Phones", ServiceMethod = "POST", RequestIdentifier = "PersonalContactPhone Data" };
                        phoneContactPersonalAction.BatchActionParameters.Add(new BatchActionParameter { Name = "phone", Value = JsonConvert.SerializeObject(personalContactPhone) });
                        batchActionList.BatchActions.Add(phoneContactPersonalAction);
                    }

                    if (!string.IsNullOrEmpty(businessContactId))
                    {
                        #region Comment : Here add related "Phones" data to CotactId

                        businessContactPhone.ContactId = int.Parse(businessContactId);

                        // populate a BatchAction that will be used to submit the Contact ("Business") DTO to the Insurance Service
                        var phoneContactBusinessAction = new BatchAction { ServiceName = "Phones", ServiceMethod = "POST", RequestIdentifier = "BusinessConatctPhone Data" };
                        phoneContactBusinessAction.BatchActionParameters.Add(new BatchActionParameter { Name = "phone", Value = JsonConvert.SerializeObject(businessContactPhone) });
                        batchActionList.BatchActions.Add(phoneContactBusinessAction);

                        #endregion

                        #region Comment : Here add related "Address" data to CotactId

                        addressData.ContactId = int.Parse(businessContactId);

                        // populate a BatchAction that will be used to submit the Contact ("Business") DTO to the Insurance Service
                        var addressContactBusinessAction = new BatchAction { ServiceName = "Addresses", ServiceMethod = "POST", RequestIdentifier = "BusinessConatctAddress Data" };
                        addressContactBusinessAction.BatchActionParameters.Add(new BatchActionParameter { Name = "address", Value = JsonConvert.SerializeObject(addressData) });
                        batchActionList.BatchActions.Add(addressContactBusinessAction);

                        #endregion
                    }

                    #endregion

                    // populate a BatchAction that will be used to submit the InsuredName DTO to the Insurance Service
                    var insuredNameAction = new BatchAction { ServiceName = "InsuredNames", ServiceMethod = "POST", RequestIdentifier = "InsuredNames Data" };
                    insuredNameAction.BatchActionParameters.Add(new BatchActionParameter { Name = "insuredName", Value = JsonConvert.SerializeObject(insuredNameData) });
                    batchActionList.BatchActions.Add(insuredNameAction);

                    var locationAction = new BatchAction { ServiceName = "Locations", ServiceMethod = "POST", RequestIdentifier = "Locations Data" };
                    locationAction.BatchActionParameters.Add(new BatchActionParameter { Name = "location", Value = JsonConvert.SerializeObject(locationData) });
                    batchActionList.BatchActions.Add(locationAction);

                    if (!string.IsNullOrWhiteSpace(model.SUIN))
                    {
                        PurchaseQuote obj = new PurchaseQuote();
                        var reqCoverageState = obj.GetCoverageStatesData(wcQuoteId);
                        if (!reqCoverageState.IsNull() && reqCoverageState.Count > 0)
                        {
                            reqCoverageState[0].EmployerCode = model.SUIN;
                            var suinAction = new BatchAction { ServiceName = "CoverageStates", ServiceMethod = "POST", RequestIdentifier = "SUIN Post" };
                            suinAction.BatchActionParameters.Add(new BatchActionParameter { Name = "CoverageState", Value = JsonConvert.SerializeObject(reqCoverageState[0]) });
                            batchActionList.BatchActions.Add(suinAction);
                        }

                    }

                    #endregion

                    #region Comment : Here Execute BatchActions list

                    // submit the BatchActionList to the Insurance Service
                    jsonResponse = SvcClient.CallServiceBatch(batchActionList, guardServiceProvider);

                    // flag that determines how to respond after processing the response received
                    success = true;

                    // deserialize the results into a BatchResponseList
                    batchResponseList = JsonConvert.DeserializeObject<BatchResponseList>(jsonResponse);

                    success = (batchResponseList != null && !batchResponseList.BatchResponses.Any(batchResponse => !batchResponse.RequestSuccessful));

                    #endregion

                    #region Comment : Here Based on result of BatchActions POST API do next level proceessing

                    #region Comment : Batch Action API processing failed and Return error to UI if any error other than system error from API
                    if (!success)
                    {
                        var listOfErrors = new List<string>();

                        #region Comment : Here set error message from API response in case of any non-system error type

                        if (batchResponseList != null &&
                            !batchResponseList.BatchResponses.Any(res => JsonConvert.DeserializeObject<OperationStatus>(res.JsonResponse).Messages.Any(msg => msg.MessageType == MessageType.SystemError)))
                        {
                            batchResponseList.BatchResponses.ForEach(res => JsonConvert.DeserializeObject<OperationStatus>(res.JsonResponse).Messages.ForEach(msg => listOfErrors.Add(msg.Text)));
                        }

                        #endregion

                        return Json(new
                        {
                            response = false,
                            message = Constants.QuoteCaptureErrors.SESSION_EXPIRED,  //This is need to be change, non-relevent message return type
                            errorList = listOfErrors
                        }, JsonRequestBehavior.AllowGet);
                    }
                    #endregion

                    #region Comment : Batch Action API processing successful, updating our own DB for User detail

                    bool result = false;

                    UserRegistration userLoggedIn = GetLoggedInUserDetails();

                    try
                    {
                        //Comment : Here get logged PC user id
                        loggedUserId = userLoggedIn.IsNull() ? (int?)null : userLoggedIn.Id;

                        #region Comment : STEP - 1. Here on successful submission Add/Register this user account in database for future refernces

                        BHIC.DML.WC.DTO.OrganisationUserDetailDTO user = new DML.WC.DTO.OrganisationUserDetailDTO();
                        user.CreatedBy = loggedUserId ?? 1;
                        user.CreatedDate = DateTime.Now;
                        user.EmailID = !model.Account.IsNull() ? model.Account.Email : string.Empty;
                        user.FirstName = ((!model.PersonalContact.IsNull() && !string.IsNullOrEmpty(model.PersonalContact.FirstName)) ?
                                        model.PersonalContact.FirstName : string.Empty);
                        user.IsActive = false;      //User will be de-activated till user will not make payment successfully
                        user.LastName = ((!model.PersonalContact.IsNull() && !string.IsNullOrEmpty(model.PersonalContact.LastName)) ?
                                        model.PersonalContact.LastName : string.Empty);
                        user.ModifiedBy = loggedUserId ?? 1;
                        user.ModifiedDate = DateTime.Now;
                        user.OrganizationName = ((!model.BusinessInfo.IsNull() && !string.IsNullOrEmpty(model.BusinessInfo.BusinessName)) ?
                                        model.BusinessInfo.BusinessName : string.Empty);
                        // Old code
                        //user.Password = !model.Account.IsNull() ? Encryption.EncryptText(model.Account.Password, user.EmailID) : string.Empty;
                        user.Password = (!model.Account.IsNull() ? (userLoggedIn == null ? Encryption.EncryptText(model.Account.Password, user.EmailID.ToUpper()) :
                            (!userLoggedIn.Email.Equals(user.EmailID, StringComparison.OrdinalIgnoreCase) ?
                            Encryption.EncryptText(model.Account.Password, user.EmailID.ToUpper()) : userLoggedIn.Password)) : string.Empty);

                        user.PhoneNumber = Convert.ToInt64(personalContactPhone.PhoneNumber +
                            (!String.IsNullOrWhiteSpace(personalContactPhone.Extension) ? personalContactPhone.Extension : string.Empty));
                        user.Tin = ((!model.BusinessInfo.IsNull() && !string.IsNullOrEmpty(model.BusinessInfo.TaxIdOrSSN)) ?
                                        Encryption.EncryptText(model.BusinessInfo.TaxIdOrSSN) : string.Empty);
                        user.Ssn = ((!model.BusinessInfo.IsNull() && !string.IsNullOrEmpty(model.BusinessInfo.TaxIdOrSSN)) ?
                                        Encryption.EncryptText(model.BusinessInfo.TaxIdOrSSN) : string.Empty);
                        user.PolicyCode = string.Empty;
                        //customSession = GetCustomSession();
                        user.Fein = Encryption.EncryptText((customSession != null && customSession.QuestionnaireVM != null && !string.IsNullOrWhiteSpace(customSession.QuestionnaireVM.TaxIdNumber) ? customSession.QuestionnaireVM.TaxIdNumber : "0"));

                        //Comment : Here Add organisation address details and get inserted record id for refernce data updation

                        PurchaseQuote purchaseQuote = new PurchaseQuote();
                        long? returnedOrganisationUserId;
                        result = purchaseQuote.MaintainUserAccount(user, out returnedOrganisationUserId);

                        #endregion

                        #region Comment : STEP - 2. Here on successful user account registration add related dependent objects like org. address, user quotes updation etc.

                        if (!result)
                        {
                            loggingService.Fatal("User Registration failed");
                        }
                        else
                        {
                            #region Comment : STEP - 3. Here on successful add user organisation address details

                            //Reset flag
                            result = false;

                            //Comment : Here add quote related details 
                            BHIC.DML.WC.DataContract.IOrganisationAddress orgAddressService = new BHIC.DML.WC.DataService.OrganisationAddressService();
                            BHIC.DML.WC.DTO.OrganisationAddress organisationAddress = new DML.WC.DTO.OrganisationAddress()
                            {
                                OrganizationID = returnedOrganisationUserId,//////////////////////////////////Hard coded
                                Address1 = model.MailingAddress.AddressLine1,
                                Address2 = model.MailingAddress.AddressLine2,
                                City = model.MailingAddress.City,
                                StateCode = customSession.StateAbbr,
                                ZipCode = Convert.ToInt32(customSession.ZipCode),
                                ContactName = model.PersonalContact.FirstName + model.PersonalContact.LastName,
                                ContactNumber1 = Convert.ToInt64(businessContactPhone.PhoneNumber),
                                CountryID = 1,
                                IsActive = true,
                                CreatedBy = loggedUserId ?? 1,
                                CreatedDate = DateTime.Now,
                                ModifiedBy = loggedUserId ?? 1,
                                ModifiedDate = DateTime.Now
                            };

                            //Comment : Here Add organisation address details and get inserted record id for refernce data updation
                            Int64? returnedOrganisationAddressId;
                            result = orgAddressService.MaintainOrganisationAddressDetail(organisationAddress, out returnedOrganisationAddressId);

                            #endregion

                            #region Comment : STEP - 4. Here on successful user account registration add related dependent objects like user quotes,org. address updation etc.

                            //Reset flag
                            result = false;

                            //Comment : Here add quote related details 
                            BHIC.DML.WC.DTO.QuoteDTO quote = new DML.WC.DTO.QuoteDTO()
                            {
                                QuoteNumber = wcQuoteId.ToString(),
                                OrganizationUserDetailID = returnedOrganisationUserId,//////////////////////////////////Hard coded
                                ModifiedBy = loggedUserId ?? 1,
                                ModifiedDate = DateTime.Now
                            };

                            result = purchaseQuote.UpdateQuoteUserId(user, quote);

                            #endregion

                            #region Comment : STEP - 5. Here on successful add of user organisation address details and then let's update reference in Quote

                            if (!returnedOrganisationAddressId.IsNull())
                            {
                                //Reset flag
                                result = false;

                                //Comment : Here add quote related details 
                                BHIC.DML.WC.DataContract.IQuoteDataProvider quoteDataProvider = new BHIC.DML.WC.DataService.QuoteDataProvider();

                                //Comment : Here update refernce id
                                organisationAddress.Id = returnedOrganisationAddressId;
                                result = quoteDataProvider.UpdateQuoteOrganizationAddressId(user, organisationAddress, quote);
                            }

                            #endregion

                            #region Comment : Here STEP - 6. Add this generated quote in application custom session persisatance which will be used for PC "Saved Quotes" functionalities

                            if (loggedUserId != null || returnedOrganisationUserId > 0)
                            {
                                loggedUserId = (loggedUserId ?? Convert.ToInt32(returnedOrganisationUserId));
                                //Comment : Here CommonFunctionality interface refernce to do/make process all business logic
                                ICommonFunctionality commonFunctionality = GetCommonFunctionalityProvider();

                                //Comment : Here call BLL to make this data stored in DB
                                bool isStoredInDB = commonFunctionality.AddApplicationCustomSession(
                                    new DML_DTO.CustomSession() { QuoteID = wcQuoteId, SessionData = commonFunctionality.StringifyCustomSession(customSession), IsActive = true, CreatedDate = DateTime.Now, CreatedBy = (int)loggedUserId, ModifiedDate = DateTime.Now, ModifiedBy = loggedUserId ?? Convert.ToInt32(returnedOrganisationUserId) });

                                //Comment : Progress bar nagivation flag handling
                                //Session["flag"] = 5;
                                customSession.PageFlag = 5;
                                SetCustomSession(customSession);
                            }

                            #endregion
                        }

                        #endregion
                    }
                    catch (Exception ex)
                    {
                        loggingService.Fatal(ex);
                    }
                    #endregion

                    #endregion
                }
            }
            else
            {
                return Json(new
                {
                    response = false,
                    errorList = errorlist
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                response = true
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveForLater(WcPurchaseViewModel request, string emailId)
        {
            #region Comment : Here get current quoteId from cookie

            //stored in a cookie on the user's machine
            int wcQuoteId = QuoteCookieHelper.Cookie_GetQuoteId(this.ControllerContext.HttpContext);

            #endregion

            #region Comment : Here when session and quote-id existance verfied then if emailId is correct then send mail to user

            if (emailId.Trim().Length > 0)
            {
                try
                {
                    //Comment : Here post this data into provider system
                    var exposureResponse = Purchase(request);

                    //Comment : Here if question post processed
                    if (!exposureResponse.IsNull())
                    {
                        JsonResult jsonResult = exposureResponse as JsonResult;
                        dynamic dResult = jsonResult.Data;
                        if (dResult.ToString().Contains("response = True"))
                        {
                            #region Comment : Here if stored in DB end then finally send user mail

                            if (wcQuoteId != 0) // && isStoredInDB)
                            {
                                string modifyPurchaseQuotePage = "ModifyPurchaseQuote";
                                bool mailSent = SendMailSaveForLater(modifyPurchaseQuotePage, wcQuoteId, emailId);

                                //Comment : Here based on mail sent status through user a message
                                if (mailSent)
                                    return Json(new { resultStatus = "OK", resultText = "Successfully submitted" }, JsonRequestBehavior.AllowGet);
                            }

                            #endregion
                        }
                    }
                }
                catch (Exception ex)
                {
                    loggingService.Fatal(ex);
                    return Json(new { resultStatus = "NOK", resultText = "Something went wrong while processing save for later !" }, JsonRequestBehavior.AllowGet);
                }
            }

            #endregion

            loggingService.Trace("Failed due to empty Email ID");
            return Json(new { resultStatus = "NOK", resultText = "Something went wrong while processing save for later !" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ForgotPasswordRequest(string emailId)
        {
            try
            {
                ServiceProvider guardServiceProvider = new GuardServiceProvider() { ServiceCategory = ServiceProviderConstants.GuardServiceCategoryWC };
                ISystemVariableService systemVariable = new SystemVariableService(guardServiceProvider);

                AccountRegistrationService accountRgistrationService = new AccountRegistrationService(guardServiceProvider);
                UserRegistration user = new UserRegistration();

                user.Email = emailId;
                user = accountRgistrationService.GetUserDetails(user);
                string str = this.HttpContext.Request.Url.Scheme;
                string URL = GetSchemeAndHostURLPart() + ConfigCommonKeyReader.PolicyCentreURL + "#/ResetPassword/";

                string currentDateTime_MMddyyyyhhmmss = Encryption.EncryptText(DateTime.Now.ToString("MMddyyyyhhmmss"));
                var encryptedURL = Encryption.EncryptText(emailId + "|:|" + currentDateTime_MMddyyyyhhmmss);
                string requiredURL = URL + "?queryKey=" + encryptedURL;

                string absoultePath = CDN.Path + "/Content/" + ConfigCommonKeyReader.CdnDefaultDashboardFolder + "/themes/_sharedFiles/emailImages/";
                string baseUrl = string.Concat(this.HttpContext.Request.Url.Scheme, "://", this.HttpContext.Request.Url.Host, ConfigCommonKeyReader.PolicyCentreURL);
                string fullname = user.FirstName + ' ' + user.LastName;

                IMailingService mailingService = new MailingService();

                if (accountRgistrationService.ForgotPwdRequestedDateTime("F", emailId, currentDateTime_MMddyyyyhhmmss) && mailingService.ForgotPassword(new ForgotPasswordViewModel
                {
                    BaseUrl = baseUrl,
                    AbsoulteURL = absoultePath,
                    Name = fullname,
                    ResetPasswordLink = requiredURL,
                    RecipEmailAddr = emailId,
                    SupportPhoneNumber = systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["PhoneNumber_CSR"]),
                    SupportPhoneNumberHref = string.Concat("tel:", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["PhoneNumber_CSR"])),
                    WebsiteUrlText = systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_Domain"]),
                    WebsiteUrlHref = systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_WebsiteShortURL"]),
                }))
                {
                    return Json(new { success = true, successMessage = "Forgot Password mail sent" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, errorMessage = "Error occurred while sending mail" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #region Validate user account and password

        /// <summary>
        /// Check user account existance based on EmailId
        /// </summary>
        /// <param name="emailId"></param>
        /// <returns></returns>
        public JsonResult IsExistingUser(string emailId)
        {
            //Comment : Here set default return status FALSE for suplied emailId
            var userExists = Json(new { resultStatus = "False", resultText = "" }, JsonRequestBehavior.AllowGet);
            customSession = GetCustomSessionWithPurchaseVM();
            if (customSession.PurchaseVM.Account.IsNull())
            {
                customSession.PurchaseVM.Account = new Account();
            }
            customSession.PurchaseVM.Account.Email = emailId;
            SetCustomSession(customSession);

            #region Comment : Here validate email and then get user details

            try
            {
                //Comment : Here trim blank spaces
                emailId = emailId.Trim();
                var loggedInUser = GetLoggedInUserDetails();
                if (!loggedInUser.IsNull() && loggedInUser.Email.Equals(emailId, StringComparison.OrdinalIgnoreCase))
                {
                    return Json(new { resultStatus = "True", resultText = "User account exists" }, JsonRequestBehavior.AllowGet);
                }
                //Comment : Here must check for valid EmailId 
                if (!(emailId.Length > 0))
                {
                    return userExists;
                }

                #region Comment : Here if email is valid

                //Comment : Here create BLL instance
                PurchaseQuote purchaseQuoteBLL = new PurchaseQuote();

                //Comment : Here get user data 

                BHIC.DML.WC.DTO.OrganisationUserDetailDTO orgUserCredential = purchaseQuoteBLL.GetUserCredentials(emailId);

                //Comment : Here if data found
                if (orgUserCredential != null && orgUserCredential.EmailID != null && orgUserCredential.EmailID.Length > 0 && orgUserCredential.IsActive)
                {

                    //Comment : Here set these value in session for future refernces
                    customSession.UserEmailId = orgUserCredential.EmailID;
                    customSession.UserPasswrod = orgUserCredential.Password;
                    SetCustomSession(customSession);

                    //return user existance status
                    return Json(new { resultStatus = "True", resultText = "User account exists" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    customSession.UserEmailId = null;
                    customSession.UserPasswrod = null;
                    SetCustomSession(customSession);
                }

                #endregion
            }
            catch (Exception ex)
            {
                BHIC.Common.Logging.ILoggingService logger = BHIC.Common.Logging.LoggingService.Instance;
                logger.Fatal(string.Format("Method {0} executed with error message : {1}", "IsExistingUser", ex.ToString()));
            }

            #endregion

            return userExists;
        }

        /// <summary>
        /// Validate user password for supplied EmailId
        /// </summary>
        /// <param name="emailId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public JsonResult HasValidPassword(string emailId, string password)
        {
            //Comment : Here set default return status FALSE for suplied emailId
            var hasValidPassword = Json(new { resultStatus = "False", resultText = "" }, JsonRequestBehavior.AllowGet);
            customSession = GetCustomSessionWithPurchaseVM();
            if (
                (!customSession.PurchaseVM.Account.IsNull() && !String.IsNullOrWhiteSpace(customSession.PurchaseVM.Account.Email)) &&
                (!customSession.PurchaseVM.Account.Email.Trim().Equals(emailId.Trim(), StringComparison.OrdinalIgnoreCase) || !UtilityFunctions.IsValidPassword(password, Constants.PasswordRegex))
                )
            {
                return hasValidPassword;
            }

            #region Comment : Here validate email and password and then get user details

            try
            {
                //Comment : Here trim blank spaces
                emailId = emailId.Trim();
                password = password.Trim();
                var loggedInUser = GetLoggedInUserDetails();
                if (!loggedInUser.IsNull() && loggedInUser.Email.Equals(emailId, StringComparison.OrdinalIgnoreCase) && loggedInUser.Password == password)
                {
                    return Json(new { resultStatus = "True", resultText = "" }, JsonRequestBehavior.AllowGet);
                }

                //Comment : Here must check for valid EmailId & Pasword
                if (!(emailId.Length > 0) || !(password.Length > 0))
                {
                    return hasValidPassword;
                }

                #region Comment : Here STEP - 1. Check user details in session and get password from it for validation

                //Comment : Here check for application custom session object data existance
                if (customSession != null && (customSession.UserEmailId != null && customSession.UserPasswrod != null))
                {
                    if (customSession.UserEmailId.Equals(emailId, StringComparison.OrdinalIgnoreCase))
                    {
                        #region Comment : Here STEP - 2. Encrypt user password and compare it with exisitng passowrd encrption text

                        #region Get Latest password in case the user has changed password

                        PurchaseQuote purchaseQuoteBLL = new PurchaseQuote();
                        BHIC.DML.WC.DTO.OrganisationUserDetailDTO orgUserCredential = purchaseQuoteBLL.GetUserCredentials(emailId);
                        customSession.UserPasswrod = orgUserCredential.Password;

                        #endregion Get Latest password in case the user has changed password

                        SecureString decyptedPassword = Encryption.DecryptTextSecure(customSession.UserPasswrod, customSession.UserEmailId.ToUpper());

                        if (UtilityFunctions.SecureStringCompare(decyptedPassword, UtilityFunctions.ConvertToSecureString(password)))
                        {
                            //Comment : Here STEP - 3. If checked and verified then return VALID/Correct password for supplied EmailId
                            if (customSession.PurchaseVM.Account.IsNull())
                            {
                                customSession.PurchaseVM.Account = new Account();
                            }
                            customSession.PurchaseVM.Account.Password = password;
                            return Json(new { resultStatus = "True", resultText = "" }, JsonRequestBehavior.AllowGet);
                        }

                        #endregion
                    }
                    else
                    {
                        if (customSession.PurchaseVM.Account.IsNull())
                        {
                            customSession.PurchaseVM.Account = new Account();
                        }
                        customSession.PurchaseVM.Account.Password = password;
                        return Json(new { resultStatus = "True", resultText = "" }, JsonRequestBehavior.AllowGet);
                    }
                }
                //Comment : Here this section will be app;icable when user account does not exists then treat this password as VALID/Correct
                else if (customSession != null && customSession.UserEmailId == null)
                {
                    if (customSession.PurchaseVM.Account.IsNull())
                    {
                        customSession.PurchaseVM.Account = new Account();
                    }
                    customSession.PurchaseVM.Account.Password = password;
                    return Json(new { resultStatus = "True", resultText = "New user account details" }, JsonRequestBehavior.AllowGet);
                }
                SetCustomSession(customSession);
                #endregion
            }
            catch (Exception ex)
            {
                BHIC.Common.Logging.ILoggingService logger = BHIC.Common.Logging.LoggingService.Instance;
                logger.Fatal(string.Format("Method {0} executed with error message : {1}", "HasValidPassword", ex.ToString()));
            }

            #endregion

            return hasValidPassword;
        }

        #endregion

        /// <summary>
        /// Validate model based on different conditions
        /// </summary>
        /// <param name="model"></param>
        /// <returns>return true if model is valid,false otherwise</returns>
        private List<string> ValidatePurchaseModel(WcPurchaseViewModel model)
        {
            #region Variable Declaration

            //Comment : Here add all model errors into list for UI display
            var listOfErrors = new List<string>();
            customSession = GetCustomSessionWithPurchaseVM();

            #endregion

            #region General Validations

            if (!ModelState.IsValid)
            {
                listOfErrors.Add(Constants.ModelStateError);
            }

            #endregion

            #region Account Validations

            //Validate Account 
            if (model.Account == null)
            {
                listOfErrors.Add(Constants.EmptyAccount);
            }
            else
            {
                if (string.IsNullOrEmpty(model.Account.Email)) // || customSession.PurchaseVM.Account.Email != model.Account.Email)
                {
                    listOfErrors.Add(Constants.EmptyEmail);
                }

                if (string.IsNullOrEmpty(model.Account.Password)) // || customSession.PurchaseVM.Account.Password != model.Account.Password)
                {
                    listOfErrors.Add(Constants.EmptyPassword);
                }

                if (model.Account.Password.Length < 8)
                {
                    listOfErrors.Add(Constants.InvalidPasswordLength);
                }

                if (!UtilityFunctions.IsValidRegex(model.Account.Email, Constants.EmailRegex))
                {
                    listOfErrors.Add(Constants.InvalidEmail);
                }

                if (!UtilityFunctions.IsValidPassword(model.Account.Password, Constants.PasswordRegex))
                {
                    listOfErrors.Add(Constants.InvalidPassword);
                }
                if (!String.Equals(model.Account.Password, model.Account.ConfirmPassword))
                {
                    listOfErrors.Add(Constants.PasswordAndConfirmPasswordMismatch);
                }

            }

            //if user id and password validation failed, return false
            var isEmailExists = (!model.IsNull() && !model.Account.IsNull() && string.IsNullOrEmpty(model.Account.Email)) ? false :
                                UtilityFunctions.ValidateInputUsingJsonResult(IsExistingUser(model.Account.Email));
            if (isEmailExists)
            {
                var isPasswordExists = UtilityFunctions.ValidateInputUsingJsonResult(HasValidPassword(model.Account.Email, model.Account.Password));

                if (!isPasswordExists)
                {
                    listOfErrors.Add(Constants.PasswordNotExists);
                }
            }

            #endregion

            #region Mailing Address Validations

            //validate Mailing Address
            if (model.MailingAddress == null)
            {
                listOfErrors.Add(Constants.EmptyMailingAddress);
            }
            else
            {
                var isValidCityStateZip = true;

                if (string.IsNullOrEmpty(model.MailingAddress.AddressLine1))
                {
                    listOfErrors.Add(Constants.EmptyMailingAddress1);
                }

                if (string.IsNullOrEmpty(model.MailingAddress.City)) // || customSession.PurchaseVM.MailingAddress.City != model.MailingAddress.City)
                {
                    isValidCityStateZip = false;
                    listOfErrors.Add(Constants.EmptyMailingCity);
                }

                if (string.IsNullOrEmpty(model.MailingAddress.State)) // || customSession.PurchaseVM.MailingAddress.State != model.MailingAddress.State)
                {
                    isValidCityStateZip = false;
                    listOfErrors.Add(Constants.EmptyMailingState);
                }

                if (string.IsNullOrEmpty(model.MailingAddress.Zip)) // || customSession.PurchaseVM.MailingAddress.Zip != model.MailingAddress.Zip)
                {
                    isValidCityStateZip = false;
                    listOfErrors.Add(Constants.EmptyMailingZip);
                }

                if (isValidCityStateZip && !IsValidCityStateZipCombination(model.MailingAddress.City, model.MailingAddress.State, model.MailingAddress.Zip))
                {
                    listOfErrors.Add(Constants.InvalidCityStateZip);
                }
            }

            #endregion

            #region Business Contact Validations

            //Validate Business Contact
            if (model.BusinessContact == null)
            {
                listOfErrors.Add(Constants.EmptyMailingAddress);
            }
            else
            {
                if (string.IsNullOrEmpty(model.BusinessContact.Email))
                {
                    listOfErrors.Add(Constants.EmptyEmail);
                }

                if (string.IsNullOrEmpty(model.BusinessContact.FirstName))
                {
                    listOfErrors.Add(Constants.EmptyFirstName);
                }

                if (string.IsNullOrEmpty(model.BusinessContact.LastName))
                {
                    listOfErrors.Add(Constants.EmptyLastName);
                }

                if (string.IsNullOrEmpty(model.BusinessContact.PhoneNumber))
                {
                    listOfErrors.Add(Constants.EmptyPhoneNumber);
                }

                if (!string.IsNullOrEmpty(model.BusinessContact.PhoneNumber) && !UtilityFunctions.IsValidRegex(model.BusinessContact.PhoneNumber, Constants.PhoneRegex))
                {
                    listOfErrors.Add(Constants.InvalidPhoneNumber);
                }
            }

            #endregion

            #region Business Info Validations

            //validate business info
            if (model.BusinessInfo == null)
            {
                listOfErrors.Add(Constants.EmptyBusinessInfo);
            }
            else
            {
                if (string.IsNullOrEmpty(model.BusinessInfo.BusinessName) && (string.IsNullOrEmpty(model.BusinessInfo.FirstName) || string.IsNullOrEmpty(model.BusinessInfo.LastName)))
                {
                    listOfErrors.Add(Constants.EmptyBusinessName);
                }

                if (string.IsNullOrEmpty(model.BusinessInfo.BusinessType))
                {
                    listOfErrors.Add(Constants.EmptyBusinessType);
                }

                if (string.IsNullOrEmpty(model.BusinessInfo.TaxIdOrSSN) || UtilityFunctions.ToNumeric(customSession.PurchaseVM.BusinessInfo.TaxIdOrSSN.Trim()) != UtilityFunctions.ToNumeric(model.BusinessInfo.TaxIdOrSSN.Trim()))
                {
                    listOfErrors.Add(Constants.EmptyTaxIdOrSSN);
                }

                ICommonFunctionality commonFunctionality = GetCommonFunctionalityProvider();

                //if FEIN/SSN/TIN is invalid, return false
                if (!string.IsNullOrEmpty(model.BusinessInfo.TaxIdOrSSN) && !commonFunctionality.ValidateTaxIdAndSSN(model.BusinessInfo.TaxIdOrSSN))
                {
                    listOfErrors.Add(Constants.InvalidTaxIdOrSSN);
                }
            }

            #endregion

            #region Personal Contact Validations

            //validate personal contact
            if (model.PersonalContact == null)
            {
                listOfErrors.Add(Constants.EmptyPersonalContact);
            }
            else
            {
                if (string.IsNullOrEmpty(model.PersonalContact.Email))
                {
                    listOfErrors.Add(Constants.EmptyEmail);
                }

                if (string.IsNullOrEmpty(model.PersonalContact.ConfirmEmail))
                {
                    listOfErrors.Add(Constants.EmptyConfirmEmail);
                }

                if (string.IsNullOrEmpty(model.PersonalContact.FirstName))
                {
                    listOfErrors.Add(Constants.EmptyFirstName);
                }

                if (string.IsNullOrEmpty(model.PersonalContact.LastName))
                {
                    listOfErrors.Add(Constants.EmptyLastName);
                }

                if (string.IsNullOrEmpty(model.PersonalContact.PhoneNumber))
                {
                    listOfErrors.Add(Constants.EmptyPhoneNumber);
                }

                if (model.PersonalContact.FirstName.Length > 50)
                {
                    listOfErrors.Add(Constants.InvalidFirstNameLength);
                }

                if (model.PersonalContact.LastName.Length > 50)
                {
                    listOfErrors.Add(Constants.InvalidLastNameLength);
                }

                if (!string.IsNullOrEmpty(model.PersonalContact.FirstName) && !UtilityFunctions.IsValidRegex(model.PersonalContact.FirstName, Constants.UserNameRegex))
                {
                    listOfErrors.Add(Constants.InvalidFirstName);
                }

                if (!string.IsNullOrEmpty(model.PersonalContact.LastName) && !UtilityFunctions.IsValidRegex(model.PersonalContact.LastName, Constants.UserNameRegex))
                {
                    listOfErrors.Add(Constants.InvalidLastName);
                }

                if (!string.IsNullOrEmpty(model.PersonalContact.ConfirmEmail) && !UtilityFunctions.IsValidRegex(model.PersonalContact.Email, Constants.EmailRegex))
                {
                    listOfErrors.Add(Constants.InvalidEmail);
                }

                if (!string.IsNullOrEmpty(model.PersonalContact.PhoneNumber) && !UtilityFunctions.IsValidRegex(model.PersonalContact.PhoneNumber, Constants.PhoneRegex))
                {
                    listOfErrors.Add(Constants.InvalidPhoneNumber);
                }
            }
            #endregion

            #region Custom Validations

            var suinRegexPatternFirst = @"^(\d)\1+$";
            var suinRegexPatternSecond = @"^0*1*2*3*4*5*6*7*8*9*0*$";
            var isValidSUIN = true;

            //validate suin
            if (customSession.StateUINumberRequired && string.IsNullOrWhiteSpace(model.SUIN))
            {
                listOfErrors.Add(Constants.EmptySUIN);
            }
            else if (!String.IsNullOrWhiteSpace(model.SUIN))
            {
                if (UtilityFunctions.IsValidRegex(model.SUIN.Trim(), suinRegexPatternFirst))
                {
                    isValidSUIN = false;
                }
                else if (UtilityFunctions.IsValidRegex(model.SUIN.Trim(), suinRegexPatternSecond))
                {
                    isValidSUIN = false;
                }
                else if (model.SUIN.Split('_').ToString().Length < 1)
                {
                    isValidSUIN = false;
                }

                if (!isValidSUIN)
                {
                    listOfErrors.Add(Constants.InvalidSUIN);
                }
            }




            #endregion

            return listOfErrors;
        }

        private void PopulateAffectedIds(Quote quoteData, int quoteId)
        {
            var effectedIds = new Dictionary<string, string>();
            if (customSession.PurchaseVM.IsNull())
            {
                customSession.PurchaseVM = new WcPurchaseViewModel();//To be considered
            }
            if (customSession.PurchaseVM.EffectedIds.IsNull())
            {
                customSession.PurchaseVM.EffectedIds = new Dictionary<string, string>();
            }
            if (!quoteData.IsNull())
            {
                if (!quoteData.Locations.IsNull() && quoteData.Locations.Any(x => x.QuoteId == quoteId && x.LocationType == "M"))
                {
                    var res = quoteData.Locations.FirstOrDefault(x => x.QuoteId == quoteId && x.LocationType == "M");
                    effectedIds["LocationsDataId"] = Convert.ToString(res.LocationId);
                }
                if (!quoteData.Contacts.IsNull() && quoteData.Contacts.Any(x => x.QuoteId == quoteId && x.ContactType == "Misc"))
                {
                    var res = quoteData.Contacts.FirstOrDefault(x => x.QuoteId == quoteId && x.ContactType == "Misc");
                    effectedIds["PersonalContactDataId"] = Convert.ToString(res.ContactId);
                    if (!res.Phones.IsNull() && res.Phones.Any(x => x.PhoneType == "H"))
                    {
                        var result = res.Phones.FirstOrDefault(x => x.PhoneType == "H");
                        effectedIds["PersonalContactPhoneId"] = Convert.ToString(result.PhoneId);
                    }
                }
                if (!quoteData.Contacts.IsNull() && quoteData.Contacts.Any(x => x.QuoteId == quoteId && x.ContactType == "Billing"))
                {
                    var res = quoteData.Contacts.FirstOrDefault(x => x.QuoteId == quoteId && x.ContactType == "Billing");
                    effectedIds["BusinessContactDataId"] = Convert.ToString(res.ContactId);
                    if (!res.Phones.IsNull() && res.Phones.Any(x => x.PhoneType == "B"))
                    {
                        var result = res.Phones.FirstOrDefault(x => x.PhoneType == "B");
                        effectedIds["BusinessContactPhoneId"] = Convert.ToString(result.PhoneId);
                    }
                    if (!res.Addresses.IsNull() && res.Addresses.Any())
                    {
                        var addressData = res.Addresses.FirstOrDefault();
                        effectedIds["BusinessContactAddressDataId"] = Convert.ToString(addressData.AddressId);
                    }//BusinessConatctAddress
                }
            }
            customSession.PurchaseVM.EffectedIds = effectedIds;
            SetCustomSession(customSession);
        }

        public string GetItemByKey(string name, Dictionary<string, string> dict)
        {
            if (!dict.IsNull() && dict.Any() && dict.ContainsKey(name))
            {
                return dict[name];
            }
            return null;
        }
    }
}


