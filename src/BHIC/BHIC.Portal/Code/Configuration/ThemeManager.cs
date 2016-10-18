using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

using System.Web.Hosting;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace BHIC.Portal.Code.Configuration
{
    public static partial class ThemeManager
    {
        public static string RootThemeFolder
        {
            get
            {
                var path = HostingEnvironment.MapPath("~/Content/Themes/");

                if (ConfigurationManager.AppSettings["CdnPath"] != null)
                {
                    //path = ConfigurationManager.AppSettings["CdnPath"];

                    path = string.Concat(HttpContext.Current.Request.Url.Scheme, "://", HttpContext.Current.Request.Url.Host,
                        ConfigurationManager.AppSettings["CdnPath"]);
                    return (path.EndsWith("/") ? path.Substring(0, path.Length - 1) : path) + "/Content/Themes/";
                }

                return path.EndsWith("/") ? path.Substring(0, path.Length - 1) : path;
            }
        }

        private static string DefaultTheme()
        {
            return ConfigurationManager.AppSettings["DefaultThemeId"];
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