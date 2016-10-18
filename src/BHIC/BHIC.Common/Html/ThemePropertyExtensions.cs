using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

using System.Web.Hosting;
using System.Web.Mvc;
using Newtonsoft.Json;
using BHIC.Common.Configuration;

namespace BHIC.Common.Html
{
    public static class ThemePropertyExtensions
    {
        public static MvcHtmlString ThemeImage(this HtmlHelper htmlHelper, string property)
        {
            return MvcHtmlString.Create(ThemeManager.ThemeImage(HttpContext.Current, property));
        }

        public static MvcHtmlString ThemeImage(this HtmlHelper htmlHelper, string property, object htmlAttributes)
        {
            return MvcHtmlString.Create(ThemeManager.ThemeImage(HttpContext.Current, property, htmlAttributes));
        }

        public static MvcHtmlString ThemeImage(this HtmlHelper htmlHelper, string property, string classNames)
        {
            return MvcHtmlString.Create(ThemeManager.ThemeImage(HttpContext.Current, property, classNames));
        }

        public static MvcHtmlString ThemeImage(this HtmlHelper htmlHelper, string property, string classNames, object htmlAttributes)
        {
            return MvcHtmlString.Create(ThemeManager.ThemeImage(HttpContext.Current, property, classNames, htmlAttributes));
        }

        public static MvcHtmlString ThemeImage(this HtmlHelper htmlHelper, string theme, string property, string classNames)
        {
            return MvcHtmlString.Create(ThemeManager.ThemeImage(theme, property, classNames));
        }

        public static MvcHtmlString ThemeImage(this HtmlHelper htmlHelper, string theme, string property, string classNames, object htmlAttributes)
        {
            return MvcHtmlString.Create(ThemeManager.ThemeImage(theme, property, classNames, htmlAttributes));
        }

        public static MvcHtmlString ThemeResource(this HtmlHelper htmlHelper, string property)
        {
            return MvcHtmlString.Create(ThemeManager.ThemeResource(HttpContext.Current, property));
        }

        public static MvcHtmlString ThemeResource(this HtmlHelper htmlHelper, string theme, string property)
        {
            return MvcHtmlString.Create(ThemeManager.ThemeResource(theme, property));
        }

        public static MvcHtmlString ThemeSharedContent(this HtmlHelper htmlHelper, string filename)
        {
            return MvcHtmlString.Create(ThemeManager.ThemeSharedContent(filename));
        }

        public static MvcHtmlString ThemeSharedContent(this HtmlHelper htmlHelper, string filename, dynamic model)
        {
            return MvcHtmlString.Create(ThemeManager.ThemeSharedContent(filename, model));
        }

        public static MvcHtmlString ThemeSharedContentFileUrl(this HtmlHelper htmlHelper, string filename)
        {
            return MvcHtmlString.Create(ThemeManager.ThemeSharedContentFileUrl(filename));
        }

        public static MvcHtmlString ThemeSharedContentFileUrl(this HtmlHelper htmlHelper, string filename, string extention)
        {
            return MvcHtmlString.Create(ThemeManager.ThemeSharedContentFileUrl(filename,extention));
        }

        public static MvcHtmlString ThemeSharedContentFileBaseUrl(this HtmlHelper htmlHelper)
        {
            return MvcHtmlString.Create(ThemeManager.ThemeSharedContentFileBaseUrl());
        }

        public static MvcHtmlString ThemeSharedCommonImagesBaseUrl(this HtmlHelper htmlHelper)
        {
            return MvcHtmlString.Create(ThemeManager.ThemeSharedCommonImagesBaseUrl());
        }

        public static IHtmlString ToJsonRaw(this HtmlHelper htmlHelper, object value)
        {
            var settings = new Newtonsoft.Json.JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            };

            return htmlHelper.Raw(Newtonsoft.Json.JsonConvert.SerializeObject(value, Newtonsoft.Json.Formatting.None, settings));
        }
    }
}