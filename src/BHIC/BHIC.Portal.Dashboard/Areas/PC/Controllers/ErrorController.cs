using System.Web.Mvc;

namespace BHIC.Portal.Dashboard.Areas.PC.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Return user application error/exception page in case something un-expected happned
        /// </summary>
        /// <returns></returns>
        //[CustomAntiForgeryToken]
        public ActionResult OnExceptionError()
        {
            return PartialView("_OnExceptionError");
        }

        public ActionResult OnRestrictedAccess()
        {
            return PartialView("_OnRestrictedAccess");
        }

    }
}
