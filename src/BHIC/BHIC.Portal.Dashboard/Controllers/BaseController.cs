using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using BHIC.Common;
using BHIC.Common.Client;
using BHIC.Common.Config;
using BHIC.Common.Logging;
using BHIC.Common.Quote;
using BHIC.Common.XmlHelper;
using BHIC.Domain.Dashboard;
using BHIC.Domain.Policy;
using BHIC.ViewDomain;
using BHIC.ViewDomain.Landing;
using BHIC.ViewDomain.PolicyCentre;
using BHIC.ViewDomain.QuestionEngine;
using Newtonsoft.Json;
using BHIC.Core.Background;
using System.Reflection;
using System.Text;
using System.Collections;
using BHIC.Common.Configuration;
using System.Net;

namespace BHIC.Portal.Dashboard
{
    public class BaseController : Controller
    {
        internal ILoggingService loggingService = LoggingService.Instance;
        internal ServiceProvider guardServiceProvider = new GuardServiceProvider() { ServiceCategory = ServiceProviderConstants.GuardServiceCategoryWC };

        protected static string LineOfBusiness = "WC";

        #region Constructors

        public BaseController() { }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Get current assembly version
        /// </summary>
        /// <returns></returns>
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
            //else if (context.Request.UserHostAddress.Length > 0)
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

        protected string GetEmailImageUrl()
        {
            return CDN.Path + "/Content/" + ConfigCommonKeyReader.CdnDefaultDashboardFolder + "/themes/_sharedFiles/emailImages/";
        }

        protected string GetEmailBaseUrl()
        {
            return string.Concat(GetSchemeAndHostURLPart(), ConfigCommonKeyReader.WCUrl);
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
        protected CustomSession GetCustomSession()
        {
            CustomSession customSession = null;

            //if CustomSession is not null assign value in object
            if (!IsCustomSessionNull())
            {
                customSession = (CustomSession)Session["CustomSession"];
            }

            return customSession;
        }

        /// <summary>
        /// It will set Custom Session object in session
        /// </summary>
        /// <returns>returns true when successfully stored in session</returns>
        protected bool SetCustomSession(CustomSession customSession)
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
        protected CustomSession GetCustomSessionWithQuoteVM()
        {
            CustomSession customSession;
            if (!IsCustomSessionNull())
            {
                customSession = GetCustomSession();
            }
            else
            {
                customSession = new CustomSession();
            }
            if (customSession.QuoteVM.IsNull())
            {
                customSession.QuoteVM = new QuoteViewModel();
            }
            return customSession;
        }

        /// <summary>
        /// Get Custom Session With QuoteVM policyData data
        /// </summary>
        /// <returns></returns>
        protected CustomSession GetCustomSessionWithPolicyData()
        {
            CustomSession customSession = GetCustomSessionWithQuoteVM();
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
        protected CustomSession GetCustomSessionWithQuoteVMExposuresList()
        {
            CustomSession customSession = GetCustomSessionWithQuoteVM();
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
        protected CustomSession GetCustomSessionWithPurchaseVM()
        {
            CustomSession customSession;
            if (!IsCustomSessionNull())
            {
                customSession = GetCustomSession();
            }
            else
            {
                customSession = new CustomSession();
            }
            if (customSession.PurchaseVM.IsNull())
            {
                customSession.PurchaseVM = new WcPurchaseViewModel();
            }
            return customSession;
        }

        /// <summary>
        /// Get available payment options
        /// </summary>
        /// <returns></returns>
        protected List<PaymentOption> GetPayOptions()
        {
            List<PaymentOption> payOptions = new List<PaymentOption>();

            payOptions.Add(new PaymentOption { Id = 1, Value = "Due Now", IsEnabled = true });
            payOptions.Add(new PaymentOption { Id = 2, Value = "Remaining Balance", IsEnabled = false });

            return payOptions;
        }

        /// <summary>
        /// Set default pay option by id
        /// </summary>
        protected List<PaymentOption> SetDefaultPayOption(List<PaymentOption> payOptions, string selectedId)
        {
            //reset all payment options 
            payOptions.All(x => { x.IsEnabled = false; return true; });

            //set select option as default value
            payOptions.Where(x => x.Id == Convert.ToInt32(selectedId)).FirstOrDefault().IsEnabled = true;

            return payOptions;
        }

        /// <summary>
        /// Return combined Scheme and Host as single value.
        /// </summary>
        /// <returns></returns>
        protected string GetSchemeAndHostURLPart()
        {
            return string.Concat(this.HttpContext.Request.Url.Scheme, "://", this.HttpContext.Request.Url.Host);
        }


        #region PolicyCentre logged user handling methods for purchase path flow

        /// <summary>
        /// Method will add cookies of policy ceneter user details
        /// </summary>
        /// <param name="context"></param>
        /// <param name="user"></param>
        protected void SetPolicyCenterUserDetailCookie(HttpContextBase context, UserRegistration user)
        {
            QuoteCookieHelper.Cookie_SavePcUserId(context, JsonConvert.SerializeObject(user));
        }

        /// <summary>
        /// Method delete remove cookies for policy ceneter user details
        /// </summary>
        /// <param name="context"></param>
        protected void DeletePolicyCenterUserDetailCookie(HttpContextBase context)
        {
            QuoteCookieHelper.Cookie_DeleteTokenId(context);
            QuoteCookieHelper.Cookie_DeletePcUserId(context);
        }

        #endregion


        /// <summary>
        ///  Return the encrypted CYB Key from Policy Information object
        /// </summary>
        /// <param name="CYBPolicyNumber"></param>
        /// <returns></returns>
        protected string EncryptCYBKey(PolicyInformation policyInformation)
        {
            string CYBKey = string.Empty;
            try
            {
                CYBKey = Encryption.EncryptText(string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}{18}",
                  policyInformation.PolicyCode, "|:|",
                  policyInformation.Status, "|:|",
                  policyInformation.EmailId, "|:|",
                  policyInformation.LOB, "|:|",
                  policyInformation.BusinessName, "|:|",
                  policyInformation.PolicyUserContact.UserName, "|:|",
                  policyInformation.PolicyUserContact.PolicyEmail, "|:|",
                  policyInformation.PolicyUserContact.PolicyContactNumber, "|:|",
                  policyInformation.EffectiveDate, "|:|",
                  policyInformation.ExpiryDate));
            }
            catch (Exception ex)
            {
                loggingService.Fatal(string.Format("Method {0} executed with error message : {1}", MethodBase.GetCurrentMethod().Name, CYBKey + Environment.NewLine + ex.ToString()));
            }

            return CYBKey;
        }



        /// <summary>
        ///  Return the Policy Information after decrypting the encrypted key
        /// </summary>
        /// <param name="CYBPolicyNumber"></param>
        /// <returns></returns>
        protected PolicyInformation DecryptedCYBKey(string CYBKey)
        {
            PolicyInformation policyInformation = new PolicyInformation();

            try
            {
                string[] split = { "|:|" };
                List<string> cybEncryptedKeys = Encryption.DecryptText(CYBKey).Split(split, StringSplitOptions.None).ToList();
                UserRegistration user = UserSession();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                if (user.Email.Equals(cybEncryptedKeys[2]))
                {
                    if (isMenuValid(cybEncryptedKeys[1], cybEncryptedKeys[3], controllerName))
                    {

                        policyInformation.PolicyCode = cybEncryptedKeys[0];
                        policyInformation.Status = cybEncryptedKeys[1];
                        policyInformation.EmailId = cybEncryptedKeys[2];
                        policyInformation.LOB = Convert.ToInt32(cybEncryptedKeys[3]);
                        policyInformation.BusinessName = cybEncryptedKeys[4];
                        policyInformation.PolicyUserContact = new PolicyUser()
                        {
                            UserName = cybEncryptedKeys[5],
                            PolicyEmail = cybEncryptedKeys[6],
                            PolicyContactNumber = cybEncryptedKeys[7]
                        };
                        policyInformation.EffectiveDate = Convert.ToDateTime(cybEncryptedKeys[8]).Date;
                        policyInformation.ExpiryDate = Convert.ToDateTime(cybEncryptedKeys[9]).Date;
                    }
                }
                else
                {
                    loggingService.Fatal(string.Format("User '{0}' tried to manipulate our data and send us following data:{1}{2}", user.Email, Environment.NewLine, CYBKey));
                }

            }
            catch (Exception ex)
            {
                loggingService.Fatal(string.Format("Method {0} executed with error message : {1}", MethodBase.GetCurrentMethod().Name, CYBKey + Environment.NewLine + ex.ToString()));
            }

            return policyInformation;
        }

        /// <summary>
        ///  Return the Policy Information after decrypting the encrypted key
        /// </summary>
        /// <param name="CYBPolicyNumber"></param>
        /// <returns></returns>
        protected bool DecryptedCYBKey(string CYBKey, string controllerName)
        {
            try
            {
                if (!string.IsNullOrEmpty(CYBKey))
                {
                    string[] split = { "|:|" };
                    List<string> cybEncryptedKeys = Encryption.DecryptText(CYBKey).Split(split, StringSplitOptions.None).ToList();

                    UserRegistration user = UserSession();

                    if (user.Email.Equals(cybEncryptedKeys[2]) && isMenuValid(cybEncryptedKeys[1], cybEncryptedKeys[3], controllerName))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {
                loggingService.Fatal(string.Format("Method {0} executed with error message : {1}", MethodBase.GetCurrentMethod().Name, CYBKey + Environment.NewLine + ex.ToString()));
            }

            return false;
        }

        /// <summary>
        ///  Return the Model Error incase of server side validation fails
        /// </summary>
        /// <param name="CYBPolicyNumber"></param>
        /// <returns></returns>
        protected string GetModelError()
        {
            ArrayList modelErrors = new ArrayList();
            try
            {
                foreach (var error in from state in ModelState.Values
                                      from error in state.Errors
                                      select error.ErrorMessage)
                {
                    modelErrors.Add(error);
                }
            }
            catch (Exception ex)
            {
                loggingService.Fatal(string.Format("Method {0} executed with error message : {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return string.Join(",", modelErrors.ToArray());
        }

        protected bool isMenuValid(string policyStatus, string LOB, string activeTab)
       {
            bool isValid = true;
            activeTab = activeTab.ToUpper();

            //to eliminate the dynamic concatenated date or other parameters with the status
            // policyStatus = DisplayStatus(policyStatus); // Commented By Krishna Not necessory this functionality.

            switch (policyStatus)
            {
                case Constants.ActiveSoon:
                    {
                        if (activeTab.Equals("REQUESTCERTIFICATE") || activeTab.Equals("REPORTCLAIM") || activeTab.Equals("REPORTCLAIMFROMHEADER") || activeTab.Equals("CANCELPOLICY") || activeTab.Equals("REQUESTLOSSRUN"))
                        {
                            isValid = false;
                        }
                        break;
                    }
                case Constants.Expired:
                    {
                        if (activeTab.Equals("MAKEPAYMENT") || activeTab.Equals("REQUESTCERTIFICATE") || activeTab.Equals("REQUESTPOLICYCHANGE") || activeTab.Equals("EDITCONTACTADDRESS") || activeTab.Equals("CANCELPOLICY"))
                        {
                            isValid = false;
                        }
                        break;
                    }
                case Constants.PendingCancellation:
                    {
                        if (activeTab.Equals("REQUESTCERTIFICATE") || activeTab.Equals("CANCELPOLICY"))
                        {
                            isValid = false;
                        }
                        break;
                    }
                case Constants.Cancelled:
                    {
                        if (activeTab.Equals("REQUESTCERTIFICATE") || activeTab.Equals("CANCELPOLICY") || activeTab.Equals("EDITCONTACTADDRESS") || activeTab.Equals("MAKEPAYMENT") || activeTab.Equals("REQUESTPOLICYCHANGE"))
                            {
                                isValid = false;
                            }
                        break;
                    }
                case Constants.NoCoverage:
                    {
                        isValid = false;
                        break;
                    }
                case Constants.Active:
                    {
                        isValid = true;
                        break;
                    }
                default:
                    isValid = false;
                    break;
            }
            return isValid;
        }
        #endregion

        /// <summary>
        ///  Return the current Session of the user
        /// </summary>
        /// <param name="CYBPolicyNumber"></param>
        /// <returns></returns>
        public UserRegistration UserSession()
        {
            try
            {
                if (Session["user"] != null)
                    return (UserRegistration)Session["user"];
            }
            catch (Exception ex)
            {
                loggingService.Fatal(string.Format("Method {0} executed with error message : {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return new UserRegistration();
        }

        #region OnException In Controller Context

        // suppress 500 errors; exceptions will be logged via Elmah
        protected override void OnException(ExceptionContext filterContext)
        {
            Exception e = filterContext.Exception;

            //Comment : Based on error type log it 
            loggingService.Error(e);

            filterContext.ExceptionHandled = true;

            filterContext.Result = RedirectToAction("OnExceptionError", "Error");


            //// if cookies are disabled, the anti-forgery logic will throw exceptions; provide instructions to the user that might help them
            //// (even if this is truly an an anti-forgery cookie exception, the error is still logged, and a potentially useful message is displayed to users without cookie support)
            //if (e.Message.Contains("The required anti-forgery cookie"))
            //{
            //    filterContext.Result = RedirectToAction("CookieTest_Save", "Home");
            //}
            //else if (filterContext.RouteData.Values.ContainsValue("Error"))
            //{
            //    filterContext.Result = RedirectToAction("NotFound", "Page");
            //}
            //else
            //{
            //    if (e.Message.Contains(Constants.UnauthorizedRequest))
            //    {
            //        string exceptionTobeShown = e.Message.Split(new Char[] { ':' })[1];

            //        filterContext.Result = RedirectToAction("UnAuthorizeRequest", "Error", new { exceptionMessage = exceptionTobeShown });
            //    }
            //    else if (e.Message.Contains(Constants.SessionExpired))
            //    {
            //        filterContext.Result = RedirectToAction("SessionExpired", "Error");
            //    }
            //    else
            //    {
            //        //Comment : Here in case of all other un-expected application exceptions/errors
            //        filterContext.Result = RedirectToAction("OnExceptionError", "Error");
            //    }
            //}
        }

        #endregion


        /// <summary>
        ///  Return the Exact policy Status eliminating the dynamic date
        /// </summary>
        /// <param name="CYBPolicyNumber"></param>
        /// <returns></returns>
        protected string DisplayStatus(string policystatus)
        {
            if (policystatus.Contains(Constants.Expired))
            {
                return Constants.Expired;
            }
            if (policystatus.Contains(Constants.PendingCancellation))
            {
                return Constants.PendingCancellation;
            }
            if (policystatus.Contains(Constants.Cancelled))
            {
                return Constants.Cancelled;
            }
            if (policystatus.Contains(Constants.Active))
            {
                return Constants.Active;
            }
            if (policystatus.Contains(Constants.ActiveSoon))
            {
                return Constants.ActiveSoon;
            }
            if (policystatus.Contains(Constants.NoCoverage))
            {
                return Constants.NoCoverage;
            }
            return policystatus;
        }

    }
}