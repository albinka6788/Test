namespace BHIC.Portal.Areas.Account.Controllers
{
    #region Using directives

    using BHIC.Portal.Areas.Account.Models;
    using System.Web.Mvc;

    #endregion

    public class WcAccountController : Controller
    {
        #region Methods : Current Policy Holder Process Flow

        // GET: Account/WcAccount
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Account/Register
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        public JsonResult Register(RegisterViewModel model)
        {
            return null;
        }

        //
        // GET: /Account/Login
        public ActionResult Login()
        {
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        public JsonResult Login(LoginViewModel model)
        {
            return null;
        }

        //
        // GET: /Account/ForgotPassword
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        public JsonResult ForgotPassword(ForgotPasswordViewModel model)
        {
            return null;
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        public ActionResult LoginSuccess()
        {
            return View();
        }

        public ActionResult DisplayEmail()
        {
            return View();
        }

        #endregion
    }
}