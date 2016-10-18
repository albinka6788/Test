using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using BHIC.Common;

namespace BHIC.Portal
{
    public class MvcApplication : System.Web.HttpApplication
    {
        ILoggingService loggingService = new LoggingService();

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //var controllerFactory = new StructureMapControllerFactory(controllerFactoryContainer);
            //ControllerBuilder.Current.SetControllerFactory(controllerFactory);
            //Database.SetInitializer<EntityContext>(null);
        }
        void Application_Error(object sender, EventArgs e)
        {
            Exception LastError = Server.GetLastError();
            String ErrMessage = LastError.ToString();

            //Comment : Based on error type log it 
            loggingService.Error(LastError);

            /*
            
            // Log the exception and notify system operators
            ExceptionUtility.LogException(LastError, "DefaultPage");
            ExceptionUtility.NotifySystemOps(LastError);
            
            // Clear the error from the server
            Server.ClearError();
             
            String LogName = "MyLog";
            String Message = "Url " + Request.Path + " Error: " + ErrMessage;

            Response.Write(Message);

            Server.Transfer(Request.Url.AbsoluteUri);
            */

        }

        // ----------------------------------------
        // Guru - added the Session_Start method to ensure that a static session id is available for the entire session
        // ----------------------------------------
        // When using cookie-based session state, ASP.NET does not allocate storage for session data until the Session object is used. 
        // As a result, a new session ID is generated for each page request until the session object is accessed. 
        // If your application requires a static session ID for the entire session, you can either implement the Session_Start method 
        // in the application's Global.asax file and store data in the Session object to fix the session ID, or you can use code in 
        // another part of your application to explicitly store data in the Session object.
        protected void Session_Start(Object sender, EventArgs e)
        {
            Session["init"] = 0;	// unused session variable; added to allocate session storage; see above
        }

    }
}
