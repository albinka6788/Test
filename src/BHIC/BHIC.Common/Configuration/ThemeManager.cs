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
    public static partial class ThemeManager
    {
        public static string RootThemeFolder
        {
            get
            {
                string appType = ConfigCommonKeyReader.AppName;

                var path = HostingEnvironment.MapPath(string.Format("~/Content/{0}/themes/", appType));

                if (!string.IsNullOrEmpty(ConfigCommonKeyReader.CdnPath))
                {
                    path = string.Concat(HttpContext.Current.Request.Url.Scheme, "://", HttpContext.Current.Request.Url.Host,
                        ConfigCommonKeyReader.CdnPath);
                    return (path.EndsWith("/") ? path.Substring(0, path.Length - 1) : path) + string.Format("/Content/{0}/themes/", appType);
                }

                return path.EndsWith("/") ? path.Substring(0, path.Length - 1) : path;

            }
        }

        private static string DefaultTheme()
        {
            return ConfigCommonKeyReader.DefaultThemeId;
        }

        public static string CurrentTheme(HttpContext context)
        {
            var themeFromQueryString = context.Request.QueryString["theme"];

            if (String.IsNullOrEmpty(themeFromQueryString))
                themeFromQueryString = DefaultTheme();
            else if (themeFromQueryString == "bob" || themeFromQueryString == "hands")
                themeFromQueryString = "68C2585C-0525-4C36-ABCB-30F486846403";
            
            return themeFromQueryString;
        }
    }
}