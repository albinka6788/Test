using System.Web.Mvc;

namespace BHIC.Portal.LP.Areas.LandingPage
{
    public class LandingPageAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "LandingPage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "LandingPage_default",
                "LandingPage/{controller}/{action}/{id}",
                new { action = "LandingPageAccount", id = UrlParameter.Optional }
            );
        }
    }
}