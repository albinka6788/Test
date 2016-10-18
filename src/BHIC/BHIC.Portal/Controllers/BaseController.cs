using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;
using BHIC.Portal;
using BHIC.Common;

namespace BHIC.Portal
{
    public class BaseController : Controller
    {
        ILoggingService loggingService = new LoggingService();

        #region Properties

        public string UserName { get { return CurrentUser.FirstName; } }
        public ApplicationUser CurrentUser { get; set; }

        #endregion

        #region Constructors

        public BaseController() { }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Get user/visitor IP address
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected string GetUser_IP(HttpContextBase context)
        {
            string VisitorsIPAddr = string.Empty;
            if (context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                VisitorsIPAddr = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else if (context.Request.UserHostAddress.Length != 0)
            {
                VisitorsIPAddr = context.Request.UserHostAddress;
            }
            return VisitorsIPAddr;
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

        #endregion

        #region OnException In Controller Context

        // suppress 500 errors; exceptions will be logged via Elmah
        protected override void OnException(ExceptionContext filterContext)
        {
            Exception e = filterContext.Exception;

            //Comment : Based on error type log it 
            loggingService.Error(e);

            filterContext.ExceptionHandled = true;

            // if cookies are disabled, the anti-forgery logic will throw exceptions; provide instructions to the user that might help them
            // (even if this is truly an an anti-forgery cookie exception, the error is still logged, and a potentially useful message is displayed to users without cookie support)
            if (e.Message.Contains("The required anti-forgery cookie"))
            {
                filterContext.Result = RedirectToAction("CookieTest_Save", "Home");
            }
            else if (filterContext.RouteData.Values.ContainsValue("Error"))
            {
                filterContext.Result = RedirectToAction("NotFound", "Page");
            }
            else
            {
                filterContext.Result = RedirectToAction("Error", "WcHome");
            }

        }

        #endregion

    }
}