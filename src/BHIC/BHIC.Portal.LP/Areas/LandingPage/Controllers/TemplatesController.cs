using BHIC.Common.Logging;
using BHIC.Contract.LP;
using BHIC.Core.LP;
using BHIC.Portal.LP.Areas.LandingPage.Models;
using BHIC.Portal.LP.Controllers;
using System;
using System.Web.Mvc;

namespace BHIC.Portal.LP.Areas.LandingPage.Controllers
{
    public class TemplatesController : BaseController
    {
        private static readonly ILoggingService Logger = LoggingService.Instance;
        ILandingPageService landingPageService;
        public TemplatesController()
        {
            landingPageService = new LandingPageService();
        }
        // GET: LandingPage/LandigPage
        public ActionResult Templates(string tokenId)
        {
            try
            {
                var homeViewModel = new HomeViewModel();
                homeViewModel.landingPageTransaction = landingPageService.GetLandingPage(tokenId);
                if (homeViewModel.landingPageTransaction.TemplateId > 0 && homeViewModel.landingPageTransaction.IsDeployed.Value)
                {
                    switch (homeViewModel.landingPageTransaction.TemplateId)
                    {
                        case 1: return PartialView("Template1");
                        case 2: return PartialView("Template2");
                        case 3: return PartialView("Template3");
                        case 4: return PartialView("Template4");
                        case 5: return PartialView("Template5");
                        case 6: return PartialView("Template6");
                        case 7: return PartialView("Template7");
                        default: return PartialView("DummyTemplate");
                    }
                }
                else
                    return PartialView("DummyTemplate");

            }
            catch (Exception ex)
            {
                Logger.Fatal(string.Format("Templates method error message : {0}", ex.ToString()));
                return PartialView("DummyTemplate");
            }
        }


        public ActionResult PreViewTemplate(string template)
        {
            switch (template)
            {
                case "Template1.cshtml": return PartialView("Template1");
                case "Template2.cshtml": return PartialView("Template2");
                case "Template3.cshtml": return PartialView("Template3");
                case "Template4.cshtml": return PartialView("Template4");
                case "Template5.cshtml": return PartialView("Template5");
                case "Template6.cshtml": return PartialView("Template6");
                case "Template7.cshtml": return PartialView("Template7");
                default: return PartialView("DummyTemplate");
            }

        }

    }
}