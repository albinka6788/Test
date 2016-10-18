using BHIC.Common.Logging;
using BHIC.Contract.PolicyCentre;
using BHIC.Core.PolicyCentre;
using BHIC.Domain.Dashboard;
using BHIC.Domain.Policy;
using BHIC.Portal.Dashboard.App_Start;
using System;
using System.Web.Mvc;
using BHIC.Common;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Reflection;
using BHIC.Common.Config;

namespace BHIC.Portal.Dashboard.Areas.PC.Controllers
{
    [CustomAuthorize]
    [CustomAntiForgeryToken]
    public class PolicyDocumentController : BaseController
    {
        /// <summary>        
        /// This method gets all the PolicyDocuments 
        /// </summary>
        /// <returns></returns>
        public ActionResult PolicyDocument()
        {
            return PartialView("PolicyDocument");
        }

        /// <summary>        
        /// This method gets all the PolicyDocuments 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetPolicyDocument(string CYBKey)
        {
            try
            {
                string policyCode = DecryptedCYBKey(CYBKey).PolicyCode;
                IPolicyDocumentService policyDocumentService = new PolicyDocumentService(guardServiceProvider);
                var policyDocuments = policyDocumentService.PolicyDocumentGet(
                    new PolicyDocumentRequestParms { PolicyCode = policyCode, SessionId = Session.SessionID, UserId = UserSession().Email });

                foreach (Document t in policyDocuments)
                {
                    t.EncryptedDocumentId = Server.UrlEncode(Encryption.EncryptText(Convert.ToString(t.DocumentId)));
                }

                //return json data
                return Json(new { status = true, errorMessage = "", policyDocuments }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                loggingService.Fatal(string.Format("Method {0} executed with error message : {1}", MethodBase.GetCurrentMethod().Name, ex.ToString()));
                return Json(new { status = false, redirectStatus = true, errorMessage = Constants.ExceptionMessage }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
