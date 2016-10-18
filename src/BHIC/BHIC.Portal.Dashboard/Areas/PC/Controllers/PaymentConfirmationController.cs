using System.Web.Mvc;

namespace BHIC.Portal.Dashboard.Areas.PC.Controllers
{
    public class PaymentConfirmationController : BaseController
    {
        //
        // GET: /PC/PaymentConfirmation/

        public ActionResult Index()
        {
            loggingService.Trace("Payment Confirmation loaded");

            return View();
        }

        public ActionResult PaymentSuccess()
        {

            return View();
        }
    }
}
