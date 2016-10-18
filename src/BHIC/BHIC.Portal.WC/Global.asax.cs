using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using BHIC.Common.CommonUtilities;
using BHIC.Common.Client;
using BHIC.Common.Config;
using BHIC.Contract.Background;
using BHIC.Contract.PurchasePath;
using BHIC.Core;
using BHIC.Core.Background;
using BHIC.Core.Masters;
using BHIC.Core.PurchasePath;
using BHIC.DML.WC.DataContract;
using BHIC.Domain.Background;
using BHIC.Portal.WC.App_Start;
using BHIC.Contract.Provider;
using System.Web;

namespace BHIC.Portal.WC
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
            //ViewEngines.Engines.Add(new CustomViewEngine());
            ViewEngines.Engines.Add(new CSharpRazorViewEngine());

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //Comment : Here heavy data caching 

            LoadCountyCacheData();
            LoadIndustryCacheData();
            LoadSubIndustryCacheData();

            //LoadClassCacheData();

            LoadLineOfBusiness();
            LoadGoodAndBadStates();
            LoadMultipleStates();
            //LoadKeywordSearchListCache();
            LoadSystemVariables();
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

        /// <summary>
        /// Store Industry data into cache
        /// </summary>
        /// <returns></returns>
        private void LoadIndustryCacheData()
        {
            try
            {
                var provider = new GuardServiceProvider() { ServiceCategory = Constants.GetLineOfBusiness(Constants.LineOfBusiness.WC) };
                IIndustryService industryService = new IndustryService();
                industryService.GetIndustryList(new IndustryRequestParms { Lob = Constants.GetLineOfBusiness(Constants.LineOfBusiness.WC) }, provider);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Store Industry data into cache
        /// </summary>
        /// <returns></returns>
        private void LoadSubIndustryCacheData()
        {
            try
            {
                var provider = new GuardServiceProvider() { ServiceCategory = Constants.GetLineOfBusiness(Constants.LineOfBusiness.WC) };
                ISubIndustryService subIndustryService = new SubIndustryService();
                subIndustryService.GetSubIndustryList(new SubIndustryRequestParms { Lob = Constants.GetLineOfBusiness(Constants.LineOfBusiness.WC) }, provider);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Store Industry data into cache
        /// </summary>
        /// <returns></returns>
        private void LoadClassCacheData()
        {
            try
            {
                var provider = new GuardServiceProvider() { ServiceCategory = Constants.GetLineOfBusiness(Constants.LineOfBusiness.WC) };
                var classDescriptionService = new ClassDescriptionService();
                classDescriptionService.GetClassDescriptionList(new ClassDescriptionRequestParms { IncludeRelated = true, Lob = Constants.GetLineOfBusiness(Constants.LineOfBusiness.WC) }, provider);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Load Keyword Search List
        /// </summary>
        private void LoadKeywordSearchListCache()
        {
            try
            {
                IClassDescKeywordService classDescKeywordService = new ClassDescKeywordService();
                classDescKeywordService.SetClassDescKeywordListCache();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Store lob data into cache
        /// </summary>
        /// <returns></returns>
        private void LoadLineOfBusiness()
        {
            try
            {
                ILineOfBusinessService lobService = new LineOfBusinessService();

                lobService.GetLineOfBusiness();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Store good and bad states into cache
        /// </summary>
        private void LoadGoodAndBadStates()
        {
            try
            {
                IStateTypeService stateService = new StateTypeService();

                stateService.GetAllGoodAndBadState();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Store multiple states into cache
        /// </summary>
        private void LoadMultipleStates()
        {
            try
            {
                IMultiStateService multipleStates = new MultiStateService();

                multipleStates.GetStates();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Store system variables data into cache
        /// </summary>
        /// <returns></returns>
        private void LoadSystemVariables()
        {
            try
            {
                ServiceProvider guardServiceProvider = new GuardServiceProvider() { ServiceCategory = ServiceProviderConstants.GuardServiceCategoryWC };
                ISystemVariableService systemVariableService = new SystemVariableService(guardServiceProvider);

                systemVariableService.GetSystemVariables();
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        ///  Custom Page Not Found Error Page
        /// </summary>
        /// <returns></returns>
        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            var httpException = exception as HttpException;
            Response.Clear();
            Server.ClearError();
            Response.StatusCode = 500;
            if (httpException != null)
            {
                Response.StatusCode = httpException.GetHttpCode();
                switch (Response.StatusCode)
                {
                    case 403:
                    case 404:
                        Response.Redirect("~/Error/PageNotFound");
                        break;
                }
            }
            // Avoid IIS7 getting in the middle
            Response.TrySkipIisCustomErrors = true;
        }
    }
}