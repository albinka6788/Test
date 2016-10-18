using BHIC.Domain.CommercialAuto;
using BHIC.ViewDomain;
using System.Web.Mvc;

namespace BHIC.Portal.WC.Areas.PurchasePath.Controllers
{
    public class CAQuoteController : BaseController
    {
        //
        // GET: /PurchasePath/CAQuote/

        public ActionResult Index()
        {
            CustomSession customSession = GetCustomSession();
            CAModel caModel = new CAModel();
            if (customSession!=null && customSession.ZipCode != null && IsZipExists(customSession.ZipCode))
            {
                caModel.ZipCode = customSession.ZipCode;
                return View(caModel);
            }
            else
            {
                return RedirectToAction("Index", "Home", null);
            }
            
        }
    }
}
