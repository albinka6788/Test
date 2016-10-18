using System.Web.Mvc;

namespace BHIC.Portal.Areas.Landing
{
    public class LandingAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Landing";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
            name: "Landing_default",
            url: "Landing/{controller}/{action}/{id}",
            defaults: new
            {
                area = "Landing",
                controller = "WcHome",
                action = "Index",
                id = UrlParameter.Optional
            });
        }
    }
}