using System;
using BHIC.Common.CommonUtilities;
using BHIC.Common.Client;
using BHIC.Contract.Background;
using BHIC.Core.Background;
using BHIC.Portal.LP.App_Start;
using System.Web.Mvc;
using System.Web.Routing;

namespace BHIC.Portal.LP
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //Remove all view engine
            ViewEngines.Engines.Clear();

            //Add Custom view Engine Derived from Razor
            ViewEngines.Engines.Add(new CSharpRazorViewEngine());

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            //Comment : Here heavy data caching 

            LoadCountyCacheData();
        }

        /// <summary>
        /// Store county data into cache
        /// </summary>
        /// <returns></returns>
        private void LoadCountyCacheData()
        {
            try
            {
                ServiceProvider guardServiceProvider = new GuardServiceProvider() { ServiceCategory = ServiceProviderConstants.GuardServiceCategoryWC };
                ICountyService countyService = new CountyService(guardServiceProvider);

                countyService.GetCounty(false);
            }
            catch (Exception)
            {
            }
        }
        
    }
}
