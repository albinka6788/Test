using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

using BHIC.Common.XmlHelper;

namespace BHIC.Common.Configuration
{
    public static partial class CdnManager
    {
        public static string RootCdnFolder
        {
            get
            {
                var path = HostingEnvironment.MapPath("~/Content");

                if (!string.IsNullOrEmpty(ConfigCommonKeyReader.CdnPath))
                {
                    path = string.Concat(HttpContext.Current.Request.Url.Scheme, "://", HttpContext.Current.Request.Url.Host,
                        ConfigCommonKeyReader.CdnPath);
                    return (path.EndsWith("/") ? path.Substring(0, path.Length - 1) : path) + "/Content/";
                }

                return path.EndsWith("/") ? path.Substring(0, path.Length - 1) : path;
            }
        }

        private static string DefaultCommonFolder()
        {
            return ConfigCommonKeyReader.CdnDefaultCommonFolder;
        }

        public static string DefaultDashboardFolder()
        {
            return ConfigCommonKeyReader.CdnDefaultDashboardFolder;
        }

    }
}