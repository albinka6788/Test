using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

using BHIC.Common;

namespace BHIC.Common.Configuration
{
    public static partial class CdnManager
    {
        public static string CdnScript(HttpContextBase context, string property)
        {
            var httpContext = context.ApplicationInstance.Context;
            return CdnScript(CdnManager.DefaultCommonFolder(), property, "");
        }

        public static string CdnScript(HttpContextBase context,string folderName, string property)
        {
            var httpContext = context.ApplicationInstance.Context;
            return CdnScript(folderName, property,"");
        }

        public static string CdnScript(HttpContext context, string property)
        {
            var httpContext = context.ApplicationInstance.Context;
            return CdnScript(CdnManager.DefaultCommonFolder(), property, "");
        }

        public static string CdnScript(HttpContext context, string folderName, string property)
        {
            var httpContext = context.ApplicationInstance.Context;
            return CdnScript(folderName, property, "");
        }

        public static string CdnScript(string folderName, string property,object htmlAttributes)
        {
            //if (!Directory.Exists(RootCdnFolder + folderName))
            //    folderName = DefaultCommonFolder();

            var url = RootCdnFolder + folderName + "/scripts/" + property + ".js";
            var rtn = "";

            var typeString = " type=\"text/javascript\"";

            //if (URL.Exists(url))
            rtn = "<script src=\"" + url + "\"" + typeString + "/>";

            return rtn;
        }
    }
}