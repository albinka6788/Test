using System;
using System.Web.Mvc;
using BHIC.CDN;
using System.Collections.Generic;
using BHIC.Common;
using System.Text;
using System.IO;

namespace BHIC.CDN.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetCDNVersion()
        {
            return Json(new
            {
                cdnVersion = CDNVersion.GetVersion()
            }, JsonRequestBehavior.AllowGet);
        }

        public string ReadTemplateContent(string template, string appName)
        {
            var extAllowed = new List<string>() { "html", "htm", "txt" };
            var contents = "";
            if (appName == null || appName == "" || appName == string.Empty)
            {
                appName = "Common";
            }
            var sharedUrl =string.Format("~/Content/{0}/themes/_sharedFiles/" ,appName);
            var templateName = Encryption.DecryptText(template);
            var filePath = HttpContext.Server.MapPath(string.Concat(sharedUrl,templateName));
            var isDone = false;
            var filetype = "";
            var filename = filePath;

            if (filePath.Contains("."))
            {
                contents = fileContent(filename);
                if(contents!=string.Empty)
                {
                    isDone = true;
                }
                
            }
            if (!isDone)
            {
                foreach (var ext in extAllowed)
                {
                    filetype = ext;
                    contents = fileContent(filename + "." + ext);
                    if (System.IO.File.Exists(filename + "." + ext))
                    {
                        contents = System.IO.File.ReadAllText(filename + "." + ext);
                        if (contents != string.Empty)
                        {
                            isDone = true;
                            break;
                        }
                    }
                }

            }
            if(!isDone)
            {
                throw new FileNotFoundException("Template {0} not found",templateName);
            }

            
            // remove byte order mark (BOM), if the return value is prefixed with this value
            string _byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
            if (contents.StartsWith(_byteOrderMarkUtf8, StringComparison.OrdinalIgnoreCase))
            {
                contents = contents.Remove(0, _byteOrderMarkUtf8.Length);
            }
            
            return contents;

            
        }

        private string fileContent(string fileName)
        {
            if (System.IO.File.Exists(fileName))
            {
                return System.IO.File.ReadAllText(fileName);
            }
            return String.Empty;
        }
    }
}
