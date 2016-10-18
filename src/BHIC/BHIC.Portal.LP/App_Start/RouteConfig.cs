using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BHIC.Portal.LP
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //"Area",
            //"",
            //new { area = "LandingPage", controller = "Login", action = "Index" }
            //);

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { area = "LandingPage", controller = "Login", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
                null,
                new[] { "BHIC.Portal.LP.Areas.LandingPage.Controllers" }
            ).DataTokens.Add("area", "LandingPage");

            //routes.MapRoute(
            //"Area",
            //"",
            //new { area = "LandingPage", controller = "Home", action = "LandingPageAccount" }
            //);

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}
