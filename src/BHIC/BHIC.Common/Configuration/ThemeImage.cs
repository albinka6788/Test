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
    public static partial class ThemeManager
    {
        public static string ThemeImage(HttpContextBase context, string property)
        {
            var httpContext = context.ApplicationInstance.Context;
            return ThemeImage(CurrentTheme(httpContext), property, "");
        }

        public static string ThemeImage(HttpContextBase context, string property, object htmlAttributes)
        {
            var httpContext = context.ApplicationInstance.Context;
            return ThemeImage(CurrentTheme(httpContext), property, "", htmlAttributes);
        }

        public static string ThemeImage(HttpContextBase context, string property, string classNames)
        {
            var httpContext = context.ApplicationInstance.Context;
            return ThemeImage(CurrentTheme(httpContext), property, classNames);
        }

        public static string ThemeImage(HttpContextBase context, string property, string classNames, object htmlAttributes)
        {
            var httpContext = context.ApplicationInstance.Context;
            return ThemeImage(CurrentTheme(httpContext), property, classNames, htmlAttributes);
        }

        public static string ThemeImage(HttpContext context, string property)
        {
            return ThemeImage(CurrentTheme(context), property, "");
        }

        public static string ThemeImage(HttpContext context, string property, object htmlAttributes)
        {
            return ThemeImage(CurrentTheme(context), property, "", htmlAttributes);
        }

        public static string ThemeImage(HttpContext context, string property, string classNames)
        {
            return ThemeImage(CurrentTheme(context), property, classNames);
        }

        public static string ThemeImage(HttpContext context, string property, string classNames, object htmlAttributes)
        {
            return ThemeImage(CurrentTheme(context), property, classNames, htmlAttributes);
        }

        public static string ThemeImage(string theme, string property)
        {
            return ThemeImage(theme, property, "", new { });
        }

        public static string ThemeImage(string theme, string property, object htmlAttributes)
        {
            return ThemeImage(theme, property, "", htmlAttributes);
        }

        public static string ThemeImage(string theme, string property, string classNames, object htmlAttributes)
        {
            if (!Directory.Exists(RootThemeFolder + theme))
                theme = DefaultTheme();
            string url = string.Empty;
            if (property.Contains('.'))
                url = RootThemeFolder + theme + "/images/" + property;            
            else
                url = RootThemeFolder + theme + "/images/" + property + ".png";
            
            var rtn = "";

            var kv = htmlAttributes.GetType().GetProperties().Where(p => p.PropertyType == typeof(string) && p.GetGetMethod() != null).Select(p => new { Name = p.Name, Value = p.GetGetMethod().Invoke(htmlAttributes, null) });

            // build properties
            var props = "";
            foreach (var prop in kv)
            {
                props += prop.Name + "=\"" + prop.Value + "\" ";
            }

            var classstring = String.IsNullOrEmpty(classNames) ? "" : " class=\"" + classNames + "\"";

            if (URL.Exists(url))
                rtn = "<img src=\"" + url + "\"" + classstring + " " + props + "/>";

            return rtn;
        }
    }
}