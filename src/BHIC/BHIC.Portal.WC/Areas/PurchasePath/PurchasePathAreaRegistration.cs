using System.Web.Mvc;

namespace BHIC.Portal.WC.Areas.PurchasePath
{
    public class PurchasePathAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "PurchasePath";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "PurchasePath_default",
                "PurchasePath/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
