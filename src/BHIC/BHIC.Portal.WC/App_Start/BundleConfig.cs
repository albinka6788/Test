#region Using directives

using BHIC.Common.Configuration;
using System.Web.Optimization;

#endregion

namespace BHIC.Portal.WC
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterScriptMain(bundles);
            RegisterScriptAngular(bundles);
            RegisterScriptModules(bundles);
            RegisterScriptRestService(bundles);
            RegisterScriptLanding(bundles);

            RegisterStylesCommon(bundles);

            RegisterCdnBundling(bundles);
        }

        /// <summary>
        /// To register bundling to use CDN 
        /// </summary>
        /// <param name="bundles"></param>
        private static void RegisterCdnBundling(BundleCollection bundles)
        {
            bundles.UseCdn = CDN.EnableCdn;
            BundleTable.EnableOptimizations = false; //force optimization while debugging
        }

        private static void RegisterScriptJquery(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle("~/bundles/jquery").Include(
                      "~/Content/Common/scripts/jquery-1.10.2.js");

            scriptBundle.CdnPath = CDN.Path;
            scriptBundle.CdnFallbackExpression = "window.jquery";
            bundles.Add(scriptBundle);
        }

        private static void RegisterScriptMain(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle("~/bundles/scripts/main").Include(
                "~/Content/Common/scripts/main.js");

            scriptBundle.CdnPath = CDN.Path;
            scriptBundle.CdnFallbackExpression = "window.main";
            bundles.Add(scriptBundle);
        }

        private static void RegisterScriptAngular(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle("~/bundles/angular").Include(
                "~/Content/Common/scripts/angular/angular.js",
                "~/Content/Common/scripts/angular-resource/angular-resource.js");
      
            scriptBundle.CdnPath = CDN.Path;
            scriptBundle.CdnFallbackExpression = "window.angular";
            bundles.Add(scriptBundle);
        }

        private static void RegisterScriptModules(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle("~/bundles/modules").Include(
                "~/Content/Common/scripts/modules/bhic-services-module.js",
                "~/Content/Common/scripts/modules/bhic-controllers-module.js",
                   "~/Content/Common/scripts/modules/bhic-directives-module.js",
                "~/Content/Common/scripts/modules/bhic-module.js");

            scriptBundle.CdnPath = CDN.Path;
            scriptBundle.CdnFallbackExpression = "window.modules";
            bundles.Add(scriptBundle);
        }

        private static void RegisterScriptRestService(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle("~/bundles/rest-service").Include(
               "~/Content/Common/scripts/services/restServiceConsumer.js"); ;
            
            scriptBundle.CdnPath = CDN.Path;
            scriptBundle.CdnFallbackExpression = "window.rest-service";
            bundles.Add(scriptBundle);
        }

        private static void RegisterScriptLanding(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle("~/bundles/landing").Include(
                "~/Content/Common/scripts/services/landing/landingPageDataProvider.js",
             "~/Content/Common/scripts/services/loading-service.js",
                "~/Content/Common/scripts/directives/zipCodeValidationDirective.js",
             "~/Content/Common/scripts/controllers/landing-page-controller.js",
             "~/Content/Common/scripts/directives/loadingControl/loadingDirective.js"); ;

            scriptBundle.CdnPath = CDN.Path;
            scriptBundle.CdnFallbackExpression = "window.landing";
            bundles.Add(scriptBundle);
        }

        private static void RegisterStylesCommon(BundleCollection bundles)
        {
            var styleBundle = new StyleBundle("~/bundles/style").Include(
                     "~/Content/Common/styles/style.css");

            styleBundle.CdnPath = CDN.Path;
            bundles.Add(styleBundle);
        }
    }
}