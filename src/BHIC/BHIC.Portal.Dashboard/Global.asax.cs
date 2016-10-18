using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

using BHIC.Common.CommonUtilities;

namespace BHIC.Portal.Dashboard
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {

        //protected void Application_BeginRequest()
        //{
        //    BundleConfig.RegisterBundles(BundleTable.Bundles);
        //}

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            MvcHandler.DisableMvcResponseHeader = true;

            //Remove all view engine
            ViewEngines.Engines.Clear();

            //Add Custom view Engine Derived from Razor
            ViewEngines.Engines.Add(new CSharpRazorViewEngine());

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            System.Web.Helpers.AntiForgeryConfig.SuppressIdentityHeuristicChecks = true;
        }

        //private void Application_Error(object sender, EventArgs e)
        //{
        //    Exception ex = Server.GetLastError();

        //    if (ex is HttpAntiForgeryException)
        //    {
        //        Response.Clear();
        //        Server.ClearError(); //make sure you log the exception first
        //        //Response.Redirect("/error/antiforgery", true);
        //    }
        //}
    }
}