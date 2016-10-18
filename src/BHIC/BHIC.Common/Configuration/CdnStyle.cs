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
        public static string CdnStyle(HttpContextBase context, string property)
        {
            var httpContext = context.ApplicationInstance.Context;
            return CdnStyle(CdnManager.DefaultCommonFolder(), property, "");
        }

        public static string CdnStyle(HttpContextBase context, string folderName, string property)
        {
            var httpContext = context.ApplicationInstance.Context;
            return CdnStyle(folderName, property, "");
        }

        public static string CdnStyle(HttpContext context, string property)
        {
            var httpContext = context.ApplicationInstance.Context;
            return CdnStyle(CdnManager.DefaultCommonFolder(), property, "");
        }

        public static string CdnStyle(HttpContext context, string folderName, string property)
        {
            var httpContext = context.ApplicationInstance.Context;
            return CdnStyle(folderName, property, "");
        }

        public static string CdnStyle(string folderName, string property, object htmlAttributes)
        {
            //if (!Directory.Exists(RootCdnFolder + folderName))
            //    folderName = DefaultCommonFolder();

            var url = RootCdnFolder + folderName + "/styles/" + property + ".css";
            var rtn = "";

            var typeString = " rel=\"stylesheet\"";

            //if (URL.Exists(url))
            rtn = "<link href=\"" + url + "\"" + typeString + "/>";

            return rtn;
        }
    }
}