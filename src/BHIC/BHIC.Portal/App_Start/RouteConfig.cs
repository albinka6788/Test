using System.Web.Mvc;
using System.Web.Routing;

namespace BHIC.Portal
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                , namespaces: new[] { "BHIC.Portal.Areas.Landing.Controllers" }
            );

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{area}/{controller}/{action}/{id}",
            //    defaults: new { area = "Landing", controller = "WcHome", action = "Index", id = UrlParameter.Optional }
            //    , namespaces: new[] { "BHIC.Portal.Areas.Landing.Controllers" }
            //);
        }
    }
}
