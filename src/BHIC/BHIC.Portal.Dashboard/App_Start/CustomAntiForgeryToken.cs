using System;
using System.Linq;
using System.Net.Http;
using System.Web.Helpers;
//using System.Web.Http.Filters;
using System.Web.Mvc;
using System.Web.Routing;

namespace BHIC.Portal.Dashboard.App_Start
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
            var cookie = httpContext.Request.Cookies[AntiForgeryConfig.CookieName].Value;
            var form = httpContext.Request.Headers["X-XSRF-Token"];

            try
            {
                AntiForgery.Validate(cookie != null ? cookie : null, form);
            }
            catch (HttpAntiForgeryException)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "LoginIndex" }));
            }
        }

        public System.Threading.Tasks.Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(System.Web.Http.Controllers.HttpActionContext actionContext, System.Threading.CancellationToken cancellationToken, Func<System.Threading.Tasks.Task<HttpResponseMessage>> continuation)
        {
            throw new NotImplementedException();
        }
    }
}