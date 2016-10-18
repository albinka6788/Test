using BHIC.Contract.PolicyCentre;
using BHIC.Core.PolicyCentre;
using BHIC.Domain.Policy;
using System;
using System.Web.Mvc;
using BHIC.Common;
using System.IO;
using BHIC.Common.XmlHelper;
using System.Reflection;

namespace BHIC.Portal.Dashboard.Areas.PC.Controllers
{
    public class DocumentController : BaseController
    {
        [CustomAuthorize]
        public ActionResult DownloadApiDocument(string docId, string CYBKey)
        {
            try
            {
                string policyCode = DecryptedCYBKey(CYBKey).PolicyCode;
                int documentId = Convert.ToInt32(Server.UrlDecode(Encryption.DecryptText(docId)));
                IViewDocumentService viewDocumentService = new ViewDocumentService(guardServiceProvider);
                var viewDocument = viewDocumentService.ViewDocument(
                    new ViewDocumentRequestParms { PolicyCode = policyCode, SessionId = Session.SessionID, DocumentId = documentId});

                byte[] fileBytes = viewDocument.Contents;
                string fileName = viewDocument.Filename + viewDocument.FileExt;
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            }
            catch (ApplicationException appEx)
            {
                loggingService.Trace(appEx);
            }
            catch (Exception ex)
            {
                loggingService.Fatal(string.Format("Method {0} executed with error message : {1}", MethodBase.GetCurrentMethod().Name, ex.ToString()));                
            }

            return RedirectToAction("OnExceptionError", "Error");
        }

        [HttpGet]
        public ActionResult DownloadStaticDocument(string filename)
        {
            try
            {
                filename = Server.UrlDecode(Encryption.DecryptText(filename));
                string fullFileLogicalPath = Path.Combine(ConfigCommonKeyReader.StaticCommonFilePath, filename);
                return File(fullFileLogicalPath, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
            }
            catch (Exception ex)
            {
                loggingService.Fatal(string.Format("Method {0} executed with error message : {1}", MethodBase.GetCurrentMethod().Name, ex.ToString()));
                return RedirectToAction("OnExceptionError", "Error");
            }           
        }
    }
}
