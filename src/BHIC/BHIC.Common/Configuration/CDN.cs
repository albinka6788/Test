using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Hosting;
using System.Web.Optimization;

using BHIC.Common.XmlHelper;

namespace BHIC.Common.Configuration
{
    public static class CDN
    {
        public static string Path
        {
            get
            {
                var CdnPath = "~/";

                if (!string.IsNullOrEmpty(ConfigCommonKeyReader.CdnPath))
                {
                    CdnPath = string.Concat(HttpContext.Current.Request.Url.Scheme, "://", HttpContext.Current.Request.Url.Host,
                        ConfigCommonKeyReader.CdnPath);
                }
                else
                {
                    CdnPath = HostingEnvironment.MapPath(CdnPath);
                }

                return CdnPath.EndsWith("/") ? CdnPath.Substring(0, CdnPath.Length - 1) : CdnPath;
            }
        }

        public static string Version
        {
            get
            {
                WebClient client = new WebClient();
                string downloadString = client.DownloadString(Path + "/Home/Version");
                return downloadString.Replace("\"", "");
            }
        }

        public static bool EnableCdn
        {
            get
            {
                return ConfigCommonKeyReader.EnableCdn;
            }
        }

        public static IHtmlString RenderScripts(string bundlePath)
        {
            bundlePath = string.Concat((EnableCdn ? Path : string.Empty), bundlePath, "_", ConfigCommonKeyReader.CdnVersion);
            //if (EnableCdn)
            //{
            //    return Scripts.RenderFormat("<script src=\"{0}\"></script>", sourceUrl);
            //}

            return Scripts.Render(bundlePath);
        }

        public static IHtmlString RenderStyles(string bundlePath)
        {
            bundlePath = string.Concat((EnableCdn ? Path : string.Empty), bundlePath, "_", ConfigCommonKeyReader.CdnVersion);
            //if (EnableCdn)
            //{
            //    string sourceUrl = Path + Styles.Url(bundlePath);
            //    return Styles.RenderFormat("<link href=\"{0}\" type=\"text/css\" rel=\"stylesheet\" />", sourceUrl);
            //}

            return Styles.Render(bundlePath);
        }

        /// <summary>
        /// Return current email image folder path
        /// </summary>
        /// <returns></returns>
        public static string GetEmailImageUrl()
        {
            return CDN.Path + ConfigCommonKeyReader.WcEmailTemplateImgPath;
        }
    }
}