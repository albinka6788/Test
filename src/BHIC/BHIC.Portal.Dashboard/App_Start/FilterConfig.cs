using System;
using System.Linq;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;

using BHIC.Common.CommonUtilities;
using BHIC.Common.Trace;

namespace BHIC.Portal.Dashboard
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new PageDisableCacheAttribute());
            filters.Add(new HandleErrorAttribute());
            filters.Add(new TransactionLogAttribute());
        }
    }

    public class CustomAuthorize : AuthorizeAttribute
    {
        public override void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
            else if ((filterContext.HttpContext.Session["user"] == null || !System.Web.HttpContext.Current.User.Identity.IsAuthenticated))// && filterContext.ActionDescriptor.ActionName.ToLower() != "index")
            {
                filterContext.HttpContext.Response.Redirect("~/SessionExpired/PPPCSessionExpired");
            }
            base.OnAuthorization(filterContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "LoginIndex" }));

        }
    }
}