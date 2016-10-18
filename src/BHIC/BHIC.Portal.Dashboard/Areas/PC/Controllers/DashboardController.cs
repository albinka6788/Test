using System.Web.Mvc;

namespace BHIC.Portal.Dashboard.Areas.PC.Controllers
{    
    [CustomAuthorize]
    public class DashboardController : BaseController
    {
        //
        // GET: /PC/Home/

        public ActionResult Index(string key)
        {
            return View();
        }

    }
}
