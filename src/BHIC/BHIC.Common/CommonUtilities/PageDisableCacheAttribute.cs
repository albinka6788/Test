#region Using Directives
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using BHIC.Common.Config;
using BHIC.Common.Quote;
using BHIC.Common.XmlHelper;
using System.IO.Compression;
#endregion

namespace BHIC.Common.CommonUtilities
{
    public class PageDisableCacheAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-10));
            filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
            filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            filterContext.HttpContext.Response.Cache.SetNoStore();
            filterContext.HttpContext.Response.Headers.Remove("Server");
        }
    }

    public class ValidateSession : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContextBase httpContext = filterContext.HttpContext;

            //if (!httpContext.Session.IsNewSession)
            //{
                if (httpContext.Session["CustomSession"] == null)
                {
                    Logging.LoggingService.Instance.Trace(string.Format("Validation Session Filter failed.{0}Call Received from URL: {1}{2}User IP: {3}",
                        Environment.NewLine, httpContext.Request.Url.ToString(), Environment.NewLine, 
                        UtilityFunctions.GetUserIPAddress(httpContext.ApplicationInstance.Context)));

                    //httpContext.Response.Redirect("~/Error/SessionExpired"); //Old Line
                    throw new ApplicationException(string.Format("{0},{1} : {2} !", "Application Filter", Constants.SessionExpired, Constants.AppCustomSessionExpired));
                }
            //}
            base.OnActionExecuting(filterContext);
        }
    }
}
