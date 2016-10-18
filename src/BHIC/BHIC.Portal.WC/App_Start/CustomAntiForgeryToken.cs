#region Using directives

using System;
using System.Linq;
using System.Net.Http;
using System.Web.Helpers;
//using System.Web.Http.Filters;
using System.Web.Mvc;
using System.Web.Routing;

#endregion

namespace BHIC.Portal.WC.App_Start
{
    public class CustomAntiForgeryToken : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            var httpContext = filterContext.HttpContext;
            string cookie = string.Empty;
            if (httpContext.Request != null && httpContext.Request.Cookies != null && httpContext.Request.Cookies.Count > 0 &&
                httpContext.Request.Cookies[AntiForgeryConfig.CookieName] != null &&
                !string.IsNullOrWhiteSpace(httpContext.Request.Cookies[AntiForgeryConfig.CookieName].Value))
            {
                cookie = httpContext.Request.Cookies[AntiForgeryConfig.CookieName].Value;
            }
            var form = httpContext.Request.Headers["X-XSRF-Token"];

            try
            {
                AntiForgery.Validate(!string.IsNullOrWhiteSpace(cookie) ? cookie : null, form);
            }
            catch (HttpAntiForgeryException)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "SessionExpired" }));
            }
        }

        public System.Threading.Tasks.Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(System.Web.Http.Controllers.HttpActionContext actionContext, System.Threading.CancellationToken cancellationToken, Func<System.Threading.Tasks.Task<HttpResponseMessage>> continuation)
        {
            throw new NotImplementedException();
        }
    }
}