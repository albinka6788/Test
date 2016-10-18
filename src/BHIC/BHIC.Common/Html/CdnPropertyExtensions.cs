using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using BHIC.Common.Configuration;

namespace BHIC.Common.Html
{
    public static class CdnPropertyExtensions
    {
        public static MvcHtmlString CdnScript(this HtmlHelper htmlHelper, string property)
        {
            return MvcHtmlString.Create(CdnManager.CdnScript(HttpContext.Current, property));
        }

        public static MvcHtmlString CdnScript(this HtmlHelper htmlHelper,string folderName, string property)
        {
            return MvcHtmlString.Create(CdnManager.CdnScript(HttpContext.Current, folderName, property));
        }

        public static MvcHtmlString CdnStyle(this HtmlHelper htmlHelper, string property)
        {
            return MvcHtmlString.Create(CdnManager.CdnStyle(HttpContext.Current, property));
        }

        public static MvcHtmlString CdnStyle(this HtmlHelper htmlHelper, string folderName, string property)
        {
            return MvcHtmlString.Create(CdnManager.CdnStyle(HttpContext.Current, folderName, property));
        }
    }
}