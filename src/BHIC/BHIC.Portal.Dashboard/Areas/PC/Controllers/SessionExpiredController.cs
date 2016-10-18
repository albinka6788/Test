using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BHIC.Portal.Dashboard.Areas.PC.Controllers
{
    public class SessionExpiredController : BaseController
    {
        //
        // GET: /PC/SessionExpired/

        public ActionResult SessionExpired()
        {
            Session.RemoveAll();
            //Comment : Here delet PP cookie        
            DeletePolicyCenterUserDetailCookie(this.ControllerContext.HttpContext);
            return PartialView("SessionExpired");
        }

        public ActionResult PPPCSessionExpired()
        {
            Session.RemoveAll();
            //Comment : Here delet PP cookie        
            DeletePolicyCenterUserDetailCookie(this.ControllerContext.HttpContext);
            return View("PPPCSessionExpired","_LoginLayout");
        }

        public ActionResult RedirectToLogin()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Session.RemoveAll();
            FormsAuthentication.SignOut();
            //Comment : Here delet cookie
            DeletePolicyCenterUserDetailCookie(this.ControllerContext.HttpContext);
            return RedirectToAction("LoginIndex", "Login");
        }
    }
}
