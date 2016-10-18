using System;
using System.Web.Mvc;

using BHIC.ViewDomain;
using BHIC.Portal.WC.App_Start;

namespace BHIC.Portal.WC.Areas.PurchasePath.Controllers
{
    public class ErrorController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Return user application error/exception page in case something un-expected happned
        /// </summary>
        /// <returns></returns>
        [CustomAntiForgeryToken]
        public ActionResult OnExceptionError()
        {
            return PartialView("_OnExceptionError");
        }

        public ActionResult OnExceptionErrorLanding()
        {
            return View("OnExceptionErrorLanding");
        }

        /// <summary>
        /// Return user custom error message when user has trying to access page directly from browser url without doing subsequent application steps 
        /// </summary>
        /// <returns></returns>
        [CustomAntiForgeryToken]
        public ActionResult UnAuthorizeRequest()
        {
            UnAurhorizeRequestViewModel unAurhorizeRequestVM = new UnAurhorizeRequestViewModel();

            //Comment : Here get NVC from request query string
            var nvc = Request.QueryString;

            if(nvc != null && nvc.Count >0)
            {
                unAurhorizeRequestVM.ErrorMessage = Convert.ToString(nvc["exceptionMessage"]);
            }

            return PartialView("_UnAuthorizeRequest", unAurhorizeRequestVM);
        }

        /// <summary>
        /// Navigate/Redirect user request to home page in case application custom session has been expired
        /// </summary>
        /// <returns></returns>
        public ActionResult SessionExpiredPartial()
        {
            //Comment : Here remove all application session and cookies 
            RemoveAllAppSessionAndCookie();

            return PartialView("_SessionExpiredPartial");
        }

        /// <summary>
        /// Navigate/Redirect user request to home page in case application custom session has been expired
        /// </summary>
        /// <returns></returns>
        public ActionResult SessionExpired()
        {
            RemoveAllAppSessionAndCookie();

            //Comment : Here return HttpResponse with status-code 419 (used for SessionExpired request)
            //return Json(new { responseCode = 419, responseText = "App session has timed-out" },JsonRequestBehavior.AllowGet);
            return View();
        }

        /// <summary>
        /// Return cookie expired content to user
        /// </summary>
        /// <returns></returns>
        public ActionResult CookieExpiredPartial()
        {
            return View("_CookieExpired");
        }

        /// <summary>
        /// Return cookie expired content to user
        /// </summary>
        /// <returns></returns>
        public ActionResult CookieExpired()
        {
            //Comment : Here return HttpResponse with status-code 401 (used for Unauthorize request)
            //return Json(new { responseCode = 401, responseText = "App cookie has expired" },JsonRequestBehavior.AllowGet);

            return PartialView("_CookieExpired");
        }
        /// <summary>
        /// Custom Page Not Found Error Page
        /// </summary>
        /// <returns></returns>
        public ActionResult PageNotFound()
        {
            return PartialView("_PageNotFound");
        }
        /// <summary>
        /// Custom Page Not Found Error Page
        /// </summary>
        /// <returns></returns>
        public ActionResult PageNotFoundPartial()
        {
            return PartialView("_PageNotFoundPartial");
        }

        public ActionResult QuoteExpired()
        {
            return PartialView("_QuoteExpired");
        }
    }
}
