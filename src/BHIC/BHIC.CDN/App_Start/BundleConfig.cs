#region Using directives

using System;
using System.Web;
using System.Web.Optimization;
using System.Linq;

using CDNConfig = BHIC.Common.Configuration;

#endregion

namespace BHIC.CDN
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region Styles Bundles Registraion

            #region Common

            RegisterStylesCommon(bundles);

            RegisterStylesFontAwesome(bundles);
            RegisterStylesGoogleFont(bundles);
            RegisterStyleUIStructure(bundles);
            RegisterStyleDropKick(bundles);
            RegisterStylesFont(bundles);

            #endregion

            #region Purchase Path

            RegisterStyleWCCustomStyle(bundles);

            //Final version
            RegisterStyleQuotePage(bundles);

            #endregion

            #region Policy Centre

            RegisterStyleLogin(bundles);
            RegisterStyleDashboard(bundles);

            #endregion

            #region Commercial Auto
            RegisterStylesCA(bundles);
            #endregion

            #endregion

            #region Scripts Bundles Registration

            #region Common

            RegisterScriptsJquery(bundles);
            RegisterScriptsJqueryUI(bundles);
            RegisterScriptAngular(bundles);

            RegisterScriptUnderScore(bundles);
            RegisterScriptRemodal(bundles);
            RegisterScriptMain(bundles);
            RegisterScriptCommonModule(bundles);
            RegisterScriptMutipleFileUpload(bundles);
            RegisterScriptSystemVariables(bundles);
            RegisterScriptNavigation(bundles);

            #endregion

            #region Purchase Path

            #region Landing(Home) Page

            RegisterScriptLandingModules(bundles);
            RegisterScriptLandingServices(bundles);
            RegisterScriptLandingData(bundles);
            RegisterScriptLanding(bundles);
            RegisterScriptCustomMethodsHome(bundles);
            RegisterScriptCustomMethodsOther(bundles);
            RegisterCdnSessionTimerBundle(bundles);
            RegisterDisableBrowserBack(bundles);
            RegisterScriptLogOut(bundles);

            //Final version
            RegisterScriptHomePage(bundles);

            #endregion

            #region Quote(SPA) Pages

            RegisterScriptQuoteModules(bundles);
            RegisterScriptQuestions(bundles);
            RegisterScriptLoading(bundles);
            RegisterScriptLogOutController(bundles);
            RegisterScriptUtil(bundles);
            RegisterScriptPayment(bundles);
            RegisterScriptOrderSummary(bundles);
            RegisterScriptReferralQuote(bundles);
            RegisterScriptQuoteSummary(bundles);

            RegisterScriptQuote(bundles);
            RegisterScriptPurchaseQuote(bundles);
            RegisterScriptSessionRecovererService(bundles);

            //Final Version
            RegisterScriptPurchasePathCommon(bundles);
            RegisterScriptPurchasePathDirectives(bundles);
            RegisterScriptPurchasePathControllersServices(bundles);

            #endregion

            #endregion

            #region Policy Centre

            // RegisterCdnBundling(bundles);

            RegisterScriptInputMask(bundles);
            RegisterScriptsLogin(bundles);
            RegisterScriptsDashboard(bundles);
            RegisterScriptsAuth(bundles);
            RegisterDashBoardMainModules(bundles);
            RegisterForgotPassword(bundles);
            RegisterCaptchaService(bundles);
            RegisterResetPassword(bundles);
            RegisterDashBoardUtil(bundles);
            RegisterAccountRegistration(bundles);
            RegisterDashboardSharedServices(bundles);
            RegisterDashboardReportClaim(bundles);
            RegisterFileUpload(bundles);
            RegisterCancelPolicy(bundles);
            RegisterContactInfo(bundles);
            RegisterPhysicianPanel(bundles);
            RegisterPolicyDocument(bundles);
            RegisterPolicyInfo(bundles);
            RegisterUnauthorisedRequest(bundles);
            RegisterAuthorisedRequest(bundles);
            RegisterReportClaim(bundles);
            RegisterRequestCertificate(bundles);
            RegisterRequestPolicyChange(bundles);
            RegisterSavedQuotes(bundles);
            RegisterYourPolicies(bundles);
            RegisterChangePassword(bundles);
            RegisterEditContact(bundles);
            RegisterPayment(bundles);
            RegisterSessionController(bundles);
            RegisterPolicyPayment(bundles);
            RegisterDashboardUtilExt(bundles);
            RegisterDashBoardModules(bundles);
            RegisterDashBoardLoginModules(bundles);
            RegisterDashBoardHeader(bundles);
            RegisterScriptsSavedQuote(bundles);
            RegisterScriptsCaptcha(bundles);
            RegisterAutoFocus(bundles);
            RegisterScriptsPolicyCentreError(bundles);
            RegisterRequestLossRun(bundles);
            RegisterHelpfulResources(bundles);
            RegisterUploadDocuments(bundles);
            RegisterLoggedInUserControllers(bundles);
            RegisterDashBoardServices(bundles);
            RegisterDashBoardDirectives(bundles);
            #endregion

            #region Commercial Auto
            RegisterScriptCA(bundles);
            #endregion

            #region Landing Page
            RegisterScriptsLPMainModules(bundles);
            RegisterScriptsLPCommonModules(bundles);
            RegisterScriptsLPModules(bundles);
            RegisterScriptsLPDirectives(bundles);
            RegisterScriptsLPServices(bundles);
            RegisterScriptsLPControllers(bundles);
            RegisterScriptsLPInspectlet(bundles);
            RegisterScriptsLPGoogleAnalytics(bundles);
            RegisterScriptsLPComplete(bundles);
            #endregion
            #endregion

            #region Comment : Here To Make Bundles Minification ON/OFF (Based On Debugging Token)

            BundleTable.EnableOptimizations = true;

            if (HttpContext.Current.IsDebuggingEnabled)
            {
                if (BundleTable.Bundles != null)
                {
                    BundleTable.Bundles.ToList().ForEach(res => res.Transforms.Clear());
                }
            }

            #endregion
        }

        #region Styles Bundles

        #region Common

        private static void RegisterStylesCommon(BundleCollection bundles)
        {
            var styleBundle = new StyleBundle(string.Format("~/bundles/styles_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/Common/styles/style.css");

            bundles.Add(styleBundle);
        }

        private static void RegisterStylesFontAwesome(BundleCollection bundles)
        {
            bundles.UseCdn = CDNConfig.CDN.EnableCdn;

            var fontAwesomeCdnPath = "https://maxcdn.bootstrapcdn.com/font-awesome/4.4.0/css/font-awesome.min.css";

            var scriptBundle = new StyleBundle(string.Format("~/bundles/font-awesome_{0}", CDNVersion.GetVersion()),
                fontAwesomeCdnPath).Include(
                "~/Content/Common/styles/font-awesome.css"
                );

            bundles.Add(scriptBundle);
        }

        private static void RegisterStylesGoogleFont(BundleCollection bundles)
        {
            bundles.UseCdn = CDNConfig.CDN.EnableCdn;

            var googleFontCdnPath = "https://fonts.googleapis.com/css?family=Raleway:300,400,700,500";

            var scriptBundle = new StyleBundle(string.Format("~/bundles/styles/google-fonts_{0}", CDNVersion.GetVersion()),
                googleFontCdnPath).Include(
                "~/Content/Common/styles/googleapis.css"
                );

            bundles.Add(scriptBundle);
        }

        private static void RegisterStylesFont(BundleCollection bundles)
        {
            bundles.UseCdn = CDNConfig.CDN.EnableCdn;

            var fontCdnPath = "//cloud.typography.com/6273674/760888/css/fonts.css";

            var scriptBundle = new StyleBundle(string.Format("~/bundles/fonts_{0}", CDNVersion.GetVersion()),
                fontCdnPath).Include(
                "~/Content/Common/styles/fonts.css"
                );

            bundles.Add(scriptBundle);
        }

        private static void RegisterStyleUIStructure(BundleCollection bundles)
        {
            var scriptBundle = new StyleBundle(string.Format("~/bundles/jquery-ui_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/Common/styles/jquery-ui.structure.min.css"
                );

            bundles.Add(scriptBundle);
        }

        private static void RegisterStyleDropKick(BundleCollection bundles)
        {
            var scriptBundle = new StyleBundle(string.Format("~/bundles/dropkick_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/Common/styles/dropkick.css"
                );

            bundles.Add(scriptBundle);
        }

        #endregion

        #region Purchase Path(SPA Pages)

        private static void RegisterStyleWCCustomStyle(BundleCollection bundles)
        {
            var styleBundle = new StyleBundle(string.Format("~/bundles/wc-custom_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/Common/styles/WcCustomStyles.css"
                );

            bundles.Add(styleBundle);
        }

        private static void RegisterStyleQuotePage(BundleCollection bundles)
        {
            var styleBundle = new StyleBundle(string.Format("~/bundles/style-quotepage_{0}", CDNVersion.GetVersion())).Include(
                //Exposure(QuotePage) Jquery UI Structure CSS
                "~/Content/Common/styles/jquery-ui.structure.min.css"
                //Exposure(QuotePage) Base Style CSS
                , "~/Content/Common/styles/style.css"
                );

            bundles.Add(styleBundle);
        }

        #endregion

        #region Policy Centre

        private static void RegisterStyleLogin(BundleCollection bundles)
        {
            var styleBundle = new StyleBundle(string.Format("~/bundles/styles/Login_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/PolicyCentre/themes/3D3FB29E-23B6-4863-A11D-863811A013D6/styles/style-login.css"
                );

            bundles.Add(styleBundle);
        }

        private static void RegisterStyleDashboard(BundleCollection bundles)
        {
            var styleBundle = new StyleBundle(string.Format("~/bundles/styles/dashboard_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/PolicyCentre/themes/3D3FB29E-23B6-4863-A11D-863811A013D6/styles/style.css"
                );

            bundles.Add(styleBundle);
        }

        #endregion

        #region Commercial Auto

        private static void RegisterStylesCA(BundleCollection bundles)
        {
            var styleBundle = new StyleBundle(string.Format("~/bundles/styles/ca_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/CA/styles/style.css");

            bundles.Add(styleBundle);
        }

        #endregion

        #endregion

        #region Scripts Bundles

        #region Common

        private static void RegisterScriptAngular(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/angular_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/Common/scripts/angular/angular.js",
                "~/Content/Common/scripts/angular/angular-sanitize.js",
                "~/Content/Common/scripts/angular/angular-messages.js",
                "~/Content/Common/scripts/angular/angular-route.js",
                "~/Content/Common/scripts/angular-resource/angular-resource.js");

            bundles.Add(scriptBundle);
        }

        private static void RegisterScriptUnderScore(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/underscore_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/Common/scripts/underscore-min.js");

            bundles.Add(scriptBundle);
        }

        private static void RegisterScriptRemodal(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/remodal_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/Common/scripts/remodal.min.js"
                );

            bundles.Add(scriptBundle);
        }

        private static void RegisterScriptMain(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/main_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/Common/scripts/main.js");

            bundles.Add(scriptBundle);
        }

        private static void RegisterScriptsJquery(BundleCollection bundles)
        {
            bundles.UseCdn = CDNConfig.CDN.EnableCdn;

            var jqueryCdnPath = "https://cdnjs.cloudflare.com/ajax/libs/jquery/1.11.3/jquery.js";

            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/jquery_{0}", CDNVersion.GetVersion()), jqueryCdnPath).Include(
               "~/Content/Common/scripts/jquery-1.11.3.js"
                );

            bundles.Add(scriptBundle);
        }

        private static void RegisterScriptsJqueryUI(BundleCollection bundles)
        {
            bundles.UseCdn = CDNConfig.CDN.EnableCdn;

            var jqueryUICdnPath = "https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.11.4/jquery-ui.min.js";

            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/jqueryui_{0}", CDNVersion.GetVersion()),
                jqueryUICdnPath).Include(
               "~/Content/Common/scripts/jquery-ui.min.js"
                );

            bundles.Add(scriptBundle);
        }

        private static void RegisterCdnBundling(BundleCollection bundles)
        {
            bundles.UseCdn = CDNConfig.CDN.EnableCdn;
            BundleTable.EnableOptimizations = false; //force optimization while debugging
        }

        private static void RegisterCdnSessionTimerBundle(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/session-countdown-timer_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/Common/scripts/session-expire-countdown-timer.js"
                );

            bundles.Add(scriptBundle);
        }

        private static void RegisterScriptCommonModule(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc_common_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/Common/scripts/modules/bhic-module.js");

            bundles.Add(scriptBundle);
        }

        private static void RegisterScriptMutipleFileUpload(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/common-mutiple-fileupload_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/Common/scripts/directives/mutiple-file-upload-control.js");

            bundles.Add(scriptBundle);
        }

        private static void RegisterScriptSystemVariables(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/common-system-variables_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/Common/scripts/system-variable-service.js");

            bundles.Add(scriptBundle);
        }

        private static void RegisterScriptNavigation(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/navigation_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/Common/scripts/navigation.js");

            bundles.Add(scriptBundle);
        }

        #endregion

        #region Purchase Path

        #region Landing Page

        private static void RegisterScriptLandingModules(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-landing-modules_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/Common/scripts/modules/bhic-services-module.js",
                "~/Content/Common/scripts/modules/bhic-controllers-module.js",
                "~/Content/Common/scripts/modules/bhic-directives-module.js",
                "~/Content/Common/scripts/modules/bhic-module.js");

            bundles.Add(scriptBundle);
        }

        private static void RegisterScriptHomePage(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-homepage_{0}", CDNVersion.GetVersion()))
                .Include(
                //Mask Input Plugin
                "~/Content/Common/scripts/jquery.inputmask.bundle.js"
                //Landing(HomePage)-Modules
                , "~/Content/Common/scripts/modules/bhic-services-module.js"
                , "~/Content/Common/scripts/modules/bhic-controllers-module.js"
                , "~/Content/Common/scripts/modules/bhic-directives-module.js"
                , "~/Content/Common/scripts/modules/bhic-module.js"
                , "~/Content/Common/scripts/services/restServiceConsumer.js"
                , "~/Content/Common/scripts/services/landing/landingPageDataProvider.js"
                , "~/Content/Common/scripts/services/core-utils.js"
                //Landing(HomePage)-Directive,Controller
                , "~/Content/Common/scripts/directives/zipCodeValidationDirective.js"
                , "~/Content/Common/scripts/controllers/landing-page-controller.js"
                //Loading(Loader)-Directive,Service
                , "~/Content/Common/scripts/services/loading-service.js"
                , "~/Content/Common/scripts/directives/loadingControl/loadingDirective.js"
                //TimePicker Input mask control directive
                , "~/Content/Common/scripts/directives/time-picker-mask-control.js"
                //Schedule Call Directive,
                , "~/Content/Common/scripts/remodal.min.js"
                , "~/Content/Common/scripts/services/schedule-call-service.js"
                , "~/Content/Common/scripts/directives/scheduleCallControl/scheduleCallDirective.js"
                //Mask input Directive
                , "~/Content/WC/scripts/directives/maskInputDirective.js"
                //Logout-Controller
                , "~/Content/Common/scripts/controllers/logout-controller.js"
                //Common-Main,Custom Scripts
                , "~/Content/Common/scripts/main.js"
                , "~/Content/Common/scripts/customMethodsHome.js"
                //Underscore
                , "~/Content/Common/scripts/underscore-min.js"
                , "~/Content/Common/scripts/system-variable-service.js"
                );

            bundles.Add(scriptBundle);
        }

        private static void RegisterScriptLandingServices(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-landing-services_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/Common/scripts/services/restServiceConsumer.js"
                );

            bundles.Add(scriptBundle);
        }

        private static void RegisterScriptSessionRecovererService(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/session-recoverer-service_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/WC/scripts/services/session-recoverer-service.js");

            bundles.Add(scriptBundle);
        }

        private static void RegisterScriptLandingData(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-landing-data_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/Common/scripts/services/landing/landingPageDataProvider.js"
               );

            bundles.Add(scriptBundle);
        }

        private static void RegisterScriptLanding(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-landing_{0}", CDNVersion.GetVersion())).Include(
                    "~/Content/Common/scripts/directives/zipCodeValidationDirective.js",
                    "~/Content/Common/scripts/controllers/landing-page-controller.js");

            bundles.Add(scriptBundle);
        }

        private static void RegisterScriptCustomMethodsHome(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-custom-home_{0}", CDNVersion.GetVersion())).Include(
                   "~/Content/Common/scripts/customMethodsHome.js"
                  );

            bundles.Add(scriptBundle);
        }

        private static void RegisterScriptCustomMethodsOther(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-custom-other_{0}", CDNVersion.GetVersion())).Include(
                   "~/Content/Common/scripts/customMethodsOther.js"
                  );

            bundles.Add(scriptBundle);
        }

        private static void RegisterDisableBrowserBack(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/disable-browser-back_{0}", CDNVersion.GetVersion())).Include(
               "~/Content/Common/scripts/disable-browser-back.js"
               );

            bundles.Add(scriptBundle);
        }

        private static void RegisterScriptLogOut(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/log-out_{0}", CDNVersion.GetVersion())).Include(
              "~/Content/Common/scripts/log-out.js"
              );

            bundles.Add(scriptBundle);
        }

        #endregion

        #region Quote(SPA) Pages

        //private static void RegisterScriptQuotePage(BundleCollection bundles)
        //{
        //    var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-quotepage_{0}", CDNVersion.GetVersion()))
        //        .Include(
        //        //Exposure(QuotePage)-CommonJs
        //        "~/Content/Common/scripts/customMethodsOther.js"
        //        , "~/Content/Common/scripts/remodal.min.js"
        //        , "~/Content/Common/scripts/main.js"
        //        );

        //    bundles.Add(scriptBundle);
        //}

        private static void RegisterScriptQuestions(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-quote-question_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/WC/scripts/controllers/questions-controller.js",
                "~/Content/WC/scripts/services/questions-service.js"
                );

            bundles.Add(scriptBundle);
        }

        private static void RegisterScriptLoading(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-loading_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/Common/scripts/services/loading-service.js",
                "~/Content/Common/scripts/directives/loadingControl/loadingDirective.js");

            //Comment : Here add PP Loading diretive
            bundles.Add(scriptBundle);

            var scriptBundlePc = new ScriptBundle(string.Format("~/bundles/scripts/wc-loading_pc_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/Common/scripts/services/loading-service.js");

            bundles.Add(scriptBundlePc);
        }

        private static void RegisterScriptLogOutController(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-logout_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/Common/scripts/controllers/logout-controller.js");

            bundles.Add(scriptBundle);
        }

        private static void RegisterScriptUtil(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-util_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/Common/scripts/jquery.inputmask.bundle.js",
                "~/Content/Common/scripts/jquery.validate.min.js",
                "~/Content/Common/scripts/autoNumeric.js",
                "~/Content/Common/scripts/services/core-utils.js",
                "~/Content/Common/scripts/directives/bhic-tooltip.js",
                "~/Content/WC/scripts/services/navigation-service.js"
                );

            bundles.Add(scriptBundle);
        }

        private static void RegisterScriptQuoteModules(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-quote-modules_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/WC/scripts/module/bhic-wc-directive-module.js",
                "~/Content/WC/scripts/module/bhic-wc-services-module.js",
                "~/Content/WC/scripts/module/bhic-wc-controllers-module.js",
                "~/Content/WC/scripts/module/bhic-wc-module.js");

            bundles.Add(scriptBundle);
        }

        private static void RegisterScriptPayment(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-quote-payment_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/WC/scripts/services/policy-payment-service.js",
                "~/Content/WC/scripts/controllers/buy-policy-controller.js",
                "~/Content/WC/scripts/directives/numericOnlyDirective.js",
                "~/Content/WC/scripts/directives/ccNumberValidatorDirective.js"
                );

            bundles.Add(scriptBundle);
        }

        private static void RegisterScriptOrderSummary(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-quote-order-summary_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/WC/scripts/services/order-summary-service.js",
                "~/Content/WC/scripts/controllers/order-summary-controller.js"
                );

            bundles.Add(scriptBundle);
        }

        private static void RegisterScriptReferralQuote(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-quote-referral_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/WC/scripts/controllers/referral-quote-controller.js",
                "~/Content/WC/scripts/services/referral-quote-service.js"
                );

            bundles.Add(scriptBundle);
        }

        private static void RegisterScriptQuoteSummary(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-quote-summary_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/WC/scripts/controllers/quote-summary-controller.js",
                "~/Content/WC/scripts/services/quote-summary-service.js"
                );

            bundles.Add(scriptBundle);
        }

        private static void RegisterScriptQuote(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-quote_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/WC/scripts/services/quote-service.js",
                "~/Content/WC/scripts/controllers/quote-controller.js"
                );

            bundles.Add(scriptBundle);
        }

        private static void RegisterScriptPurchaseQuote(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-quote-purchase_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/WC/scripts/services/purchase-quote-service.js",
                "~/Content/WC/scripts/controllers/purchase-quote-controller.js",
                "~/Content/WC/scripts/directives/cityTypeHeadDirective.js",
                "~/Content/WC/scripts/directives/check-user-existence-control.js",
                "~/Content/WC/scripts/directives/invalid-password-control.js"
                );

            bundles.Add(scriptBundle);
        }

        #endregion

        #region PurchasePath(SPA)

        private static void RegisterScriptPurchasePathCommon(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-purchasepath-common_{0}", CDNVersion.GetVersion()))
                .Include(
                //Exposure(QuotePage)-CommonJs
                "~/Content/Common/scripts/customMethodsOther.js"
                , "~/Content/Common/scripts/remodal.min.js"
                , "~/Content/Common/scripts/main.js"
                //Exposure(QuotePage)-OtherCommoJs (Undescore)
                , "~/Content/Common/scripts/underscore-min.js"
                //Exposure(QuotePage)-OtherCommoJs (Base HomePage Modules)
                , "~/Content/Common/scripts/modules/bhic-services-module.js"
                , "~/Content/Common/scripts/modules/bhic-controllers-module.js"
                , "~/Content/Common/scripts/modules/bhic-directives-module.js"
                , "~/Content/Common/scripts/modules/bhic-module.js"
                //Exposure(QuotePage)-OtherCommoJs (Base HomePage Provider)
                , "~/Content/Common/scripts/services/landing/landingPageDataProvider.js"
                //Exposure(QuotePage)-OtherCommoJs (Base HTTP Service Provider)
                , "~/Content/Common/scripts/services/restServiceConsumer.js"
                //Exposure(QuotePage)-OtherCommoJs (Basic Loaded Related JS)
                , "~/Content/Common/scripts/services/loading-service.js"
                , "~/Content/Common/scripts/directives/loadingControl/loadingDirective.js"
                //Schedule A Call Directive,
                , "~/Content/Common/scripts/services/schedule-call-service.js"
                //Exposure(QuotePage)-OtherCommoJs (Basic LoggedUser Sing-Out Related JS)
                , "~/Content/Common/scripts/controllers/logout-controller.js"
                //Exposure(QuotePage)-OtherCommoJs (Basic Utility Functionlities)
                , "~/Content/Common/scripts/jquery.inputmask.bundle.js"
                , "~/Content/Common/scripts/jquery.validate.min.js"
                , "~/Content/Common/scripts/autoNumeric.js"
                , "~/Content/Common/scripts/services/core-utils.js"
                , "~/Content/Common/scripts/directives/bhic-tooltip.js"
                , "~/Content/Common/scripts/directives/mutiple-file-upload-control.js"
                , "~/Content/WC/scripts/services/navigation-service.js"
                //Exposure(QuotePage)-OtherCommoJs (All Main Modules Needed For PurchasePath SPA)
                , "~/Content/WC/scripts/module/bhic-wc-directive-module.js"
                , "~/Content/WC/scripts/module/bhic-wc-services-module.js"
                , "~/Content/WC/scripts/module/bhic-wc-controllers-module.js"
                , "~/Content/WC/scripts/module/bhic-wc-module.js"
                , "~/Content/Common/scripts/system-variable-service.js"
                , "~/Content/Common/scripts/navigation.js"
                );

            bundles.Add(scriptBundle);
        }

        private static void RegisterScriptPurchasePathDirectives(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-purchasepath-directives_{0}", CDNVersion.GetVersion())).Include(
                "~/content/WC/scripts/directives/dateControlDirective.js",
                "~/Content/WC/scripts/directives/currencyDirective.js",
                "~/Content/WC/scripts/directives/typeAheadControlDirective.js",
                "~/Content/WC/scripts/directives/business-search-directive.js",
                "~/Content/WC/scripts/directives/multi-class-multi-state/multi-class-multi-state-directive.js",
                "~/Content/WC/scripts/directives/validate-taxid-number-control.js",
                "~/Content/WC/scripts/directives/session-expired.js",
                "~/Content/WC/scripts/directives/error-messages-control.js",
                "~/Content/Common/scripts/directives/time-picker-mask-control.js",
                "~/Content/Common/scripts/directives/scheduleCallControl/scheduleCallDirective.js",
                "~/Content/Common/scripts/directives/navigationDirective.js",
                "~/Content/WC/scripts/directives/saveForLaterDirective.js",
                "~/Content/WC/scripts/directives/maskInputDirective.js",
                "~/Content/WC/scripts/directives/focusDirective.js"
                );

            bundles.Add(scriptBundle);
        }

        private static void RegisterScriptPurchasePathControllersServices(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-purchasepath-controllers-services_{0}", CDNVersion.GetVersion())).Include(
                //PurchasePath (AllPages) Common Controllers Services
                "~/Content/WC/scripts/services/session-recoverer-service.js"
                //PurchasePath (LandingBusinessInfo) Controllers Services
                , "~/Content/WC/scripts/services/business-info-service.js"
                , "~/Content/WC/scripts/directives/business-info-zip-validation-directive.js"
                , "~/Content/WC/scripts/controllers/business-info-controller.js"                
                //PurchasePath (QuotePage) Controllers Services
                , "~/Content/WC/scripts/services/quote-service.js"
                , "~/Content/WC/scripts/services/exposure-service.js"
                , "~/Content/WC/scripts/controllers/quote-controller.js"
                , "~/Content/WC/scripts/controllers/exposure-controller.js"
                //PurchasePath (QuestionPage) Controllers Services
                , "~/Content/WC/scripts/controllers/questions-controller.js"
                , "~/Content/WC/scripts/services/questions-service.js"
                //PurchasePath (QuoteSummaryPage) Controllers Services
                , "~/Content/WC/scripts/controllers/quote-summary-controller.js"
                , "~/Content/WC/scripts/services/quote-summary-service.js"
                //PurchasePath (ReferralPage) Controllers Services
                , "~/Content/WC/scripts/controllers/referral-quote-controller.js"
                , "~/Content/WC/scripts/services/referral-quote-service.js"
                //PurchasePath (UserInfo) Controllers Services
                , "~/Content/WC/scripts/services/purchase-quote-service.js"
                , "~/Content/WC/scripts/controllers/purchase-quote-controller.js"
                , "~/Content/WC/scripts/directives/cityTypeHeadDirective.js"
                , "~/Content/WC/scripts/directives/check-user-existence-control.js"
                , "~/Content/WC/scripts/directives/invalid-password-control.js"
                //PurchasePath (PaymentPage) Controllers Services
                , "~/Content/WC/scripts/services/policy-payment-service.js"
                , "~/Content/WC/scripts/controllers/buy-policy-controller.js"
                , "~/Content/WC/scripts/directives/numericOnlyDirective.js"
                , "~/Content/WC/scripts/directives/ccNumberValidatorDirective.js"
                //PurchasePath (OrderSummaryPage) Controllers Services
                , "~/Content/WC/scripts/services/order-summary-service.js"
                , "~/Content/WC/scripts/controllers/order-summary-controller.js"
                );

            bundles.Add(scriptBundle);
        }

        #endregion

        #endregion

        #region Policy Centre

        private static void RegisterScriptInputMask(BundleCollection bundles)
        {

            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-dashboard-input-mask_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/Common/scripts/jquery.inputmask.bundle.js"
                );

            bundles.Add(scriptBundle);
        }

        private static void RegisterScriptsLogin(BundleCollection bundles)
        {

            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/Login_{0}", CDNVersion.GetVersion())).Include(
                            "~/Content/PolicyCentre/scripts/remodal.min.js",
                            "~/Content/PolicyCentre/scripts/policycentre.js",
                            "~/Content/PolicyCentre/scripts/apps.js"
                //"~/Content/Common/scripts/encryptiondecryption/encryption.js",
                // "~/Content/Common/scripts/directives/fileUploadDirective.js"
                            );

            bundles.Add(scriptBundle);
        }

        private static void RegisterScriptsDashboard(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/dashboard_{0}", CDNVersion.GetVersion())).Include(
                            "~/Content/PolicyCentre/scripts/datatable.js",
                            "~/Content/PolicyCentre/scripts/remodal.min.js",
                            "~/Content/PolicyCentre/scripts/policycentre.js",
                            "~/Content/PolicyCentre/scripts/apps.js" //,
                //"~/Content/Common/scripts/encryptiondecryption/decryption.js"
                            );

            bundles.Add(scriptBundle);
        }

        private static void RegisterScriptsAuth(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/script_auth_{0}", CDNVersion.GetVersion())).Include(
                            "~/Content/PolicyCentre/scripts/policycentre_auth.js"
                            );

            bundles.Add(scriptBundle);
        }

        private static void RegisterScriptsCaptcha(BundleCollection bundles)
        {
            bundles.UseCdn = CDNConfig.CDN.EnableCdn;

            var googleReCaptcha = "https://www.google.com/recaptcha/api.js";

            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/captcha_{0}", CDNVersion.GetVersion()), googleReCaptcha).Include(
               "~/Content/Common/scripts/api.js"
                );

            bundles.Add(scriptBundle);
        }

        private static void RegisterDashBoardMainModules(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-main-modules_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/PolicyCentre/scripts/module/bhic-dashboard-services-module.js",
                "~/Content/PolicyCentre/scripts/module/bhic-dashboard-directive-module.js",
                "~/Content/PolicyCentre/scripts/module/bhic-dashboard-controllers-module.js",
                "~/Content/PolicyCentre/scripts/directives/dateControlDirective.js"
                );

            bundles.Add(scriptBundle);
        }

        private static void RegisterDashBoardLoginModules(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-login-modules_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/PolicyCentre/scripts/module/bhic-login-module.js"
                );

            bundles.Add(scriptBundle);
        }

        private static void RegisterDashBoardModules(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-dashboard-modules_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/PolicyCentre/scripts/module/bhic-dashboard-module.js"
                );

            bundles.Add(scriptBundle);
        }

        private static void RegisterCaptchaService(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-dashboard-captcha_{0}", CDNVersion.GetVersion())).Include(
                //"~/Content/Common/scripts/api.js",
               "~/Content/Common/scripts/services/captcha-service.js"
               );

            bundles.Add(scriptBundle);
        }

        private static void RegisterForgotPassword(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-dashboard-forgot-password_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/PolicyCentre/scripts/controllers/forgot-password-controller.js"
               );

            bundles.Add(scriptBundle);
        }

        private static void RegisterResetPassword(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-dashboard-reset-password_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/PolicyCentre/scripts/controllers/reset-password-controller.js"
               );

            bundles.Add(scriptBundle);
        }

        private static void RegisterDashBoardUtil(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-dashboard-util_{0}", CDNVersion.GetVersion())).Include(
              "~/Content/PolicyCentre/scripts/directives/maskInputDirective.js",
              "~/Content/PolicyCentre/scripts/directives/onerrorfocusFormDirective.js",
                //TimePicker Input mask control directive
              "~/Content/Common/scripts/directives/time-picker-mask-control.js",
                //Schedule A Call Directive,
              "~/Content/Common/scripts/services/schedule-call-service.js",
              "~/Content/Common/scripts/directives/scheduleCallControl/scheduleCallDirective.js"
                );

            bundles.Add(scriptBundle);
        }

        private static void RegisterAccountRegistration(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-dashboard-account-registration_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/PolicyCentre/scripts/controllers/login-controller.js",
                "~/Content/PolicyCentre/scripts/controllers/account-registration-controller.js"
                );

            bundles.Add(scriptBundle);
        }

        private static void RegisterDashboardSharedServices(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-dashboard-shared-service_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/PolicyCentre/scripts/services/shared-user-service.js"
                );

            bundles.Add(scriptBundle);
        }

        private static void RegisterDashboardReportClaim(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-dashboard-report-claim_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/PolicyCentre/scripts/controllers/ReportClaimDashboardController.js",
                "~/Content/PolicyCentre/scripts/controllers/report-claim-from-home-controller.js"
               );

            bundles.Add(scriptBundle);
        }

        private static void RegisterFileUpload(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-dashboard-file-upload_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/PolicyCentre/scripts/directives/filesUploadDirective.js"
               );

            bundles.Add(scriptBundle);
        }

        private static void RegisterCancelPolicy(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-dashboard-cancel-policy_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/PolicyCentre/scripts/controllers/cancel-policy-controller.js"
               );

            bundles.Add(scriptBundle);
        }

        private static void RegisterContactInfo(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-dashboard-contact-info_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/PolicyCentre/scripts/controllers/contact-info-controller.js"
               );

            bundles.Add(scriptBundle);
        }

        private static void RegisterPhysicianPanel(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-dashboard-physician-panel_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/PolicyCentre/scripts/controllers/physician-panel-controller.js"
              );

            bundles.Add(scriptBundle);
        }

        private static void RegisterPolicyDocument(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-dashboard-policy-document_{0}", CDNVersion.GetVersion())).Include(
                 "~/Content/PolicyCentre/scripts/controllers/policy-document-controller.js"
              );

            bundles.Add(scriptBundle);
        }

        private static void RegisterPolicyInfo(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-dashboard-policy-info_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/PolicyCentre/scripts/controllers/policy-information-controller.js",
                "~/Content/PolicyCentre/scripts/controllers/policy-detail-controller.js"
              );

            bundles.Add(scriptBundle);
        }

        private static void RegisterUnauthorisedRequest(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-dashboard-unauthorised_{0}", CDNVersion.GetVersion())).Include(
               "~//Content/PolicyCentre/scripts/services/wrapper-for-unauthorised-user-service.js"
                 );

            bundles.Add(scriptBundle);
        }

        private static void RegisterAuthorisedRequest(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-dashboard-authorised_{0}", CDNVersion.GetVersion())).Include(
               "~/Content/PolicyCentre/scripts/services/wrapper-for-authorised-user-service.js"
                 );

            bundles.Add(scriptBundle);
        }

        private static void RegisterReportClaim(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-report-claim_{0}", CDNVersion.GetVersion())).Include(
                 "~/Content/PolicyCentre/scripts/controllers/report-claim-controller.js"
                );

            bundles.Add(scriptBundle);
        }

        private static void RegisterRequestCertificate(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-dashboard-request-certificate_{0}", CDNVersion.GetVersion())).Include(
                     "~/Content/PolicyCentre/scripts/controllers/request-certificate-controller.js"
               );

            bundles.Add(scriptBundle);
        }

        private static void RegisterRequestPolicyChange(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-dashboard-request-policy-change_{0}", CDNVersion.GetVersion())).Include(
                    "~/Content/PolicyCentre/scripts/controllers/request-policy-change-controller.js"
               );

            bundles.Add(scriptBundle);
        }

        private static void RegisterSavedQuotes(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-dashboard-saved-quotes_{0}", CDNVersion.GetVersion())).Include(
                    "~/Content/PolicyCentre/scripts/controllers/saved-quotes-controller.js"
               );

            bundles.Add(scriptBundle);
        }

        private static void RegisterYourPolicies(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-dashboard-your-policies_{0}", CDNVersion.GetVersion())).Include(
                   "~/Content/PolicyCentre/scripts/controllers/your-policies-controller.js"
              );

            bundles.Add(scriptBundle);
        }

        private static void RegisterChangePassword(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-dashboard-change-password_{0}", CDNVersion.GetVersion())).Include(
                      "~/Content/PolicyCentre/scripts/controllers/change-password-controller.js"
                );

            bundles.Add(scriptBundle);
        }

        private static void RegisterEditContact(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-dashboard-edit-contact_{0}", CDNVersion.GetVersion())).Include(
                      "~/Content/PolicyCentre/scripts/controllers/edit-contact-info-controller.js"
                );

            bundles.Add(scriptBundle);
        }

        private static void RegisterPayment(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-dashboard-make-payment_{0}", CDNVersion.GetVersion())).Include(
                      "~/Content/PolicyCentre/scripts/controllers/make-payment-controller.js"
                );

            bundles.Add(scriptBundle);
        }

        private static void RegisterSessionController(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-dashboard-session-controller_{0}", CDNVersion.GetVersion())).Include(
                     "~/Content/PolicyCentre/scripts/controllers/session-controller.js"
               );

            bundles.Add(scriptBundle);
        }

        private static void RegisterPolicyPayment(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-dashboard-policy-payment_{0}", CDNVersion.GetVersion())).Include(
                     "~/Content/PolicyCentre/scripts/controllers/policy-payment-controller.js"
                );

            bundles.Add(scriptBundle);
        }

        private static void RegisterDashboardUtilExt(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-dashboard-util-ext_{0}", CDNVersion.GetVersion())).Include(
                     "~/Content/PolicyCentre/scripts/directives/numericOnlyDirective.js",
                     "~/Content/PolicyCentre/scripts/directives/ccNumberValidatorDirective.js"
               );

            bundles.Add(scriptBundle);
        }

        private static void RegisterDashBoardHeader(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-dashboard-header_{0}", CDNVersion.GetVersion())).Include(
                     "~/Content/PolicyCentre/scripts/controllers/header-controller.js"
               );

            bundles.Add(scriptBundle);
        }

        private static void RegisterScriptsSavedQuote(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/savedquote_{0}", CDNVersion.GetVersion())).Include(
               "~/Content/PolicyCentre/scripts/savedquote.remodal.js"
                );

            bundles.Add(scriptBundle);
        }

        private static void RegisterAutoFocus(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-dashboard-auto-focus_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/PolicyCentre/scripts/directives/AutoFocusDirective.js"
               );

            //scriptBundle.CdnPath = CDNConfig.CDN.Path;
            bundles.Add(scriptBundle);
        }

        private static void RegisterScriptsPolicyCentreError(BundleCollection bundles)
        {

            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-dashboard-PolicyCentre-Error_{0}", CDNVersion.GetVersion())).Include(
                            "~/Content/PolicyCentre/scripts/controllers/error-controller.js"
                            );

            bundles.Add(scriptBundle);
        }

        private static void RegisterRequestLossRun(BundleCollection bundles)
        {

            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-dashboard-request-loss-run_{0}", CDNVersion.GetVersion())).Include(
                            "~/Content/PolicyCentre/scripts/controllers/request-loss-run-controller.js"
                            );

            bundles.Add(scriptBundle);
        }

        private static void RegisterHelpfulResources(BundleCollection bundles)
        {

            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-dashboard-helpful-resources_{0}", CDNVersion.GetVersion())).Include(
                            "~/Content/PolicyCentre/scripts/controllers/helpful-resources-controller.js"
                            );

            bundles.Add(scriptBundle);
        }

        private static void RegisterLoggedInUserControllers(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-dashboard-loggedInUserControllers_{0}", CDNVersion.GetVersion())).Include(
                                    "~/Content/PolicyCentre/scripts/controllers/your-policies-controller.js",
                                    "~/Content/PolicyCentre/scripts/controllers/contact-info-controller.js",
                                    "~/Content/PolicyCentre/scripts/controllers/edit-contact-info-controller.js",
                                    "~/Content/PolicyCentre/scripts/controllers/edit-contact-address-controller.js",
                                    "~/Content/PolicyCentre/scripts/controllers/saved-quotes-controller.js",
                                    "~/Content/PolicyCentre/scripts/controllers/helpful-resources-controller.js",
                                    "~/Content/PolicyCentre/scripts/controllers/policy-information-controller.js",
                                    "~/Content/PolicyCentre/scripts/controllers/policy-payment-controller.js",
                                    "~/Content/PolicyCentre/scripts/controllers/physician-panel-controller.js",
                                    "~/Content/PolicyCentre/scripts/controllers/cancel-policy-controller.js",
                                    "~/Content/PolicyCentre/scripts/controllers/policy-document-controller.js",
                                    "~/Content/PolicyCentre/scripts/controllers/report-claim-controller.js",
                                    "~/Content/PolicyCentre/scripts/controllers/request-certificate-controller.js",
                                    "~/Content/PolicyCentre/scripts/controllers/request-policy-change-controller.js",
                                    "~/Content/PolicyCentre/scripts/controllers/change-password-controller.js",
                                    "~/Content/PolicyCentre/scripts/controllers/edit-contact-info-controller.js",
                                    "~/Content/PolicyCentre/scripts/controllers/session-controller.js",
                                    "~/Content/PolicyCentre/scripts/controllers/header-controller.js",
                                    "~/Content/PolicyCentre/scripts/controllers/error-controller.js",
                                    "~/Content/PolicyCentre/scripts/controllers/request-loss-run-controller.js",
                                    "~/Content/PolicyCentre/scripts/controllers/upload-documents-controller.js"
                                );

            bundles.Add(scriptBundle);
        }

        private static void RegisterDashBoardServices(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-dashboard-services_{0}", CDNVersion.GetVersion())).Include(
                                "~/Content/PolicyCentre/scripts/services/shared-user-service.js",
                                "~/Content/PolicyCentre/scripts/services/wrapper-for-authorised-user-service.js",
                                "~/Content/PolicyCentre/scripts/services/wrapper-for-unauthorised-user-service.js",
                                "~/Content/Common/scripts/services/captcha-service.js"
                                );
            bundles.Add(scriptBundle);
        }

        private static void RegisterDashBoardDirectives(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-dashboard-directives_{0}", CDNVersion.GetVersion())).Include(
                                "~/Content/PolicyCentre/scripts/directives/filesUploadDirective.js",
                                "~/Content/PolicyCentre/scripts/directives/maskInputDirective.js",
                                "~/Content/PolicyCentre/scripts/directives/onerrorfocusFormDirective.js",
                                "~/Content/PolicyCentre/scripts/directives/numericOnlyDirective.js",
                                "~/Content/PolicyCentre/scripts/directives/ccNumberValidatorDirective.js",
                                "~/Content/PolicyCentre/scripts/directives/AutoFocusDirective.js",
                                 "~/Content/PolicyCentre/scripts/directives/focusDirective.js"
                                );
            bundles.Add(scriptBundle);
        }

        private static void RegisterUploadDocuments(BundleCollection bundles)
        {

            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/wc-dashboard-upload-documents_{0}", CDNVersion.GetVersion())).Include(
                            "~/Content/PolicyCentre/scripts/controllers/upload-documents-controller.js"
                            );

            bundles.Add(scriptBundle);
        }

        #endregion

        #region Commercial Auto

        private static void RegisterScriptCA(BundleCollection bundles)
        {

            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/ca_{0}", CDNVersion.GetVersion())).Include(
                "~/Content/CA/scripts/scripts.js",
                "~/Content/Common/scripts/system-variable-service.js"
                );

            bundles.Add(scriptBundle);
        }

        #endregion

        #region Landing Page

        private static void RegisterScriptsLPMainModules(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/lp-main_{0}", CDNVersion.GetVersion())).Include(
                            "~/Content/LandingPage/scripts/main.js",
                            "~/Content/LandingPage/scripts/landingPage.js",
                            "~/Content/LandingPage/scripts/apps.js"
                            );

            bundles.Add(scriptBundle);
        }

        private static void RegisterScriptsLPCommonModules(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/lp-common-modules_{0}", CDNVersion.GetVersion())).Include(
                            "~/Content/Common/scripts/modules/bhic-services-module.js",
                            "~/Content/Common/scripts/modules/bhic-controllers-module.js",
                            "~/Content/Common/scripts/modules/bhic-directives-module.js",
                            "~/Content/Common/scripts/services/loading-service.js");

            bundles.Add(scriptBundle);
        }

        private static void RegisterScriptsLPModules(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/lp-modules_{0}", CDNVersion.GetVersion())).Include(
                            "~/Content/LandingPage/scripts/modules/bhic-landingPage-module.js",
                            "~/Content/LandingPage/scripts/modules/bhic-landingPage-services-module.js",
                            "~/Content/LandingPage/scripts/modules/bhic-landingPage-directive-module.js",
                            "~/Content/LandingPage/scripts/modules/bhic-landingPage-controllers-module.js");

            bundles.Add(scriptBundle);
        }

        private static void RegisterScriptsLPControllers(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/lp-controllers_{0}", CDNVersion.GetVersion())).Include(
                            "~/Content/LandingPage/scripts/controllers/login-controller.js",
                            "~/Content/LandingPage/scripts/controllers/landing-Page-controller.js",
                            "~/Content/LandingPage/scripts/controllers/insertorupdate-landingpage-controller.js",
                            "~/Content/LandingPage/scripts/controllers/preview-landing-Page-controller.js",
                            "~/Content/LandingPage/scripts/controllers/template-background-controller.js",
                            "~/Content/LandingPage/scripts/controllers/template-controller.js");

            bundles.Add(scriptBundle);
        }

        private static void RegisterScriptsLPDirectives(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/lp-directives_{0}", CDNVersion.GetVersion())).Include(
                          "~/Content/LandingPage/scripts/directives/numbersOnlyDirective.js",
                            "~/Content/LandingPage/scripts/directives/filesUploadDirective.js",
                            "~/Content/LandingPage/scripts/directives/dcarDirective.js");

            bundles.Add(scriptBundle);
        }

        private static void RegisterScriptsLPServices(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/lp-services_{0}", CDNVersion.GetVersion())).Include(
                            "~/Content/LandingPage/scripts/services/shared-user-service.js",
                            "~/Content/LandingPage/scripts/services/wrapper-for-authorised-user-service.js");
            bundles.Add(scriptBundle);
        }

        private static void RegisterScriptsLPGoogleAnalytics(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/lp-GoogleAnalytics_{0}", CDNVersion.GetVersion())).Include(
                            "~/Content/LandingPage/scripts/GoogleAnalytics.js"
                            );

            bundles.Add(scriptBundle);
        }

        private static void RegisterScriptsLPInspectlet(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/lp-Inspectlet_{0}", CDNVersion.GetVersion())).Include(
                            "~/Content/LandingPage/scripts/custominspectlet.js"
                            );

            bundles.Add(scriptBundle);
        }
        private static void RegisterScriptsInspectlet(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/inspectlet_{0}", CDNVersion.GetVersion())).Include(
                            "~/Content/Common/scripts/inspectlet.js"
                            );

            bundles.Add(scriptBundle);
        }

        private static void RegisterScriptsLPComplete(BundleCollection bundles)
        {
            var scriptBundle = new ScriptBundle(string.Format("~/bundles/scripts/lp-Complete_{0}", CDNVersion.GetVersion())).Include(
                //lp-main
                            "~/Content/LandingPage/scripts/main.js",
                            "~/Content/LandingPage/scripts/landingPage.js",
                            "~/Content/LandingPage/scripts/apps.js",
                //lp-common-modules
                             "~/Content/Common/scripts/modules/bhic-services-module.js",
                            "~/Content/Common/scripts/modules/bhic-controllers-module.js",
                            "~/Content/Common/scripts/modules/bhic-directives-module.js",
                            "~/Content/Common/scripts/services/loading-service.js",
                //lp-modules
                            "~/Content/LandingPage/scripts/modules/bhic-landingPage-module.js",
                            "~/Content/LandingPage/scripts/modules/bhic-landingPage-services-module.js",
                            "~/Content/LandingPage/scripts/modules/bhic-landingPage-directive-module.js",
                            "~/Content/LandingPage/scripts/modules/bhic-landingPage-controllers-module.js",
                //lp-directives
                            "~/Content/LandingPage/scripts/directives/numbersOnlyDirective.js",
                            "~/Content/LandingPage/scripts/directives/filesUploadDirective.js",
                            "~/Content/LandingPage/scripts/directives/dcarDirective.js",
                //lp-controllers
                            "~/Content/LandingPage/scripts/controllers/login-controller.js",
                            "~/Content/LandingPage/scripts/controllers/landing-Page-controller.js",
                            "~/Content/LandingPage/scripts/controllers/insertorupdate-landingpage-controller.js",
                            "~/Content/LandingPage/scripts/controllers/preview-landing-Page-controller.js",
                            "~/Content/LandingPage/scripts/controllers/template-background-controller.js",
                            "~/Content/LandingPage/scripts/controllers/template-controller.js",
                //lp-services
                            "~/Content/LandingPage/scripts/services/shared-user-service.js",
                            "~/Content/LandingPage/scripts/services/wrapper-for-authorised-user-service.js"
                            );

            bundles.Add(scriptBundle);
        }
        #endregion

        #endregion
    }
}

