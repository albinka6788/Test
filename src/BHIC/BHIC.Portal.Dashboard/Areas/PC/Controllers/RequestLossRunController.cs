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
    public class RequestLossRunController : Controller
    {
        //
        // GET: /PC/RequestLossRun/

        public ActionResult RequestLossRun()
        {
            return PartialView("RequestLossRun");
        }

    }
}
