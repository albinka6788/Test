using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

using System.Web.Hosting;
using System.Web.Mvc;
using Newtonsoft.Json;
using BHIC.Portal.Code.Configuration;


namespace BHIC.Portal.Code.Html
{
    public static class ThemePropertyExtensions
    {
        public static MvcHtmlString ThemeSharedContent(this HtmlHelper htmlHelper, string filename)
        {
            return MvcHtmlString.Create(ThemeManager.ThemeSharedContent(filename));
        }

        public static MvcHtmlString ThemeSharedContent(this HtmlHelper htmlHelper, string filename, dynamic model)
        {
            return MvcHtmlString.Create(ThemeManager.ThemeSharedContent(filename, model));
        }
    }
}