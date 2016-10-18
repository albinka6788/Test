using BHIC.Common;
using BHIC.Common.Config;
using BHIC.Common.Logging;
using BHIC.Contract.PolicyCentre;
using BHIC.Core.PolicyCentre;
using BHIC.Domain.Dashboard;
using BHIC.Domain.Policy;
using BHIC.Portal.Dashboard.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace BHIC.Portal.Dashboard.Areas.PC.Controllers
{
    [CustomAuthorize]
    [CustomAntiForgeryToken]
    public class PhysicianPanelController : BaseController
    {
        public ActionResult PhysicianPanel()
        {
            return PartialView("PhysicianPanel");
        }

        /// <summary>        
        /// This method gets all the Physician Documents 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetPhysicianDocument(string CYBKey)
        {
            try
            {
                string policyCode = DecryptedCYBKey(CYBKey).PolicyCode;
                IPhysicianPanelService physicianPanelService = new PhysicianPanelService(guardServiceProvider);
                var physicianDocuments = physicianPanelService.PhysicianPanelGet(
                     new PhysicianPanelRequestParms { PolicyCode = policyCode, SessionId = Session.SessionID, UserId = UserSession().Email });

                foreach (Document t in physicianDocuments)
                {
                    t.EncryptedDocumentId = Server.UrlEncode(Encryption.EncryptText(Convert.ToString(t.DocumentId)));
                }

                //return json data
                return Json(new { status = true, errorMessage = "", physicianDocuments }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                loggingService.Fatal(string.Format("Method {0} executed with error message : {1}", MethodBase.GetCurrentMethod().Name, ex.ToString()));
                return Json(new { status = false, redirectStatus = true, errorMessage = Constants.ExceptionMessage }, JsonRequestBehavior.AllowGet);
            }
        }


    }
}