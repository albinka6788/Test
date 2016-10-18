using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BHIC.Portal.Dashboard
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { area = "PC", controller = "Login", action = "LoginIndex", id = UrlParameter.Optional }, // Parameter defaults
                null,
                new[] { "BHIC.Portal.Dashboard.Areas.PC.Controllers" }
            ).DataTokens.Add("area", "PC");

        }
    }
}