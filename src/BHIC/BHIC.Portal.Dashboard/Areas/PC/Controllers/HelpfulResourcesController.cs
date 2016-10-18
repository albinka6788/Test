using BHIC.Portal.Dashboard.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BHIC.Portal.Dashboard.Areas.PC.Controllers
{
    [CustomAuthorize]
    [CustomAntiForgeryToken]
    public class HelpfulResourcesController : Controller
    {
        //
        // GET: /PC/HelpfulResources/

        public ActionResult Resource()
        {
            return PartialView("Resource");
        }
        public ActionResult EmployerNotices()
        {
            return PartialView("EmployerNotices");
        }
        public ActionResult Connecticut()
        {
            return PartialView("Connecticut");
        }
        public ActionResult TexasMPN()
        {
            return PartialView("TexasMPN");
        }
        public ActionResult CAClaims()
        {
            return PartialView("CAClaims");
        }
    }
}
