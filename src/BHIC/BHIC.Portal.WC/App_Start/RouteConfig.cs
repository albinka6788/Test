using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BHIC.Portal.WC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");            

            //GUIN-166: Albin - SEO friendly routing for static pages
            routes.MapRoute(
                "SEOUrl", // Route name
                "{id}", // URL without controller and action, only parameters
                new { area = "PurchasePath", controller = "Home", action = "SiteContents" },
                null,
                new[] { "BHIC.Portal.WC.Areas.PurchasePath.Controllers" }
            ).DataTokens.Add("area", "PurchasePath");


            routes.MapRoute(
               "Default", // Route name
               "{controller}/{action}/{id}", // URL with parameters
               new { area = "PurchasePath", controller = "Home", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
               null,
               new[] { "BHIC.Portal.WC.Areas.PurchasePath.Controllers" }
           ).DataTokens.Add("area", "PurchasePath");


            //Added for the 404 errors which was thrown by IIS
            routes.MapRoute("NotFound", "{*url}",
          new { area = "PurchasePath", controller = "Error", action = "PageNotFound", id = UrlParameter.Optional }, // Parameter defaults
               null,
               new[] { "BHIC.Portal.WC.Areas.PurchasePath.Controllers" }
           ).DataTokens.Add("area", "PurchasePath");
        }
    }
}