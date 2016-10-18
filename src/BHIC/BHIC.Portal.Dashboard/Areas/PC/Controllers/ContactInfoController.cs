using BHIC.Portal.Dashboard.App_Start;
using System.Web.Mvc;

namespace BHIC.Portal.Dashboard.Areas.PC.Controllers
{
    [CustomAuthorize]
    [CustomAntiForgeryToken]
    public class ContactInfoController : Controller
    {
        // GET: DashBoard/ContactInfo
        public ActionResult ContactInfo()
        {
            return PartialView("ContactInfo");
        }
    }
}