using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Caching;
using System.Web;

using BHIC.Common.Caching;

namespace BHIC.Common.Configuration
{
    public static partial class ThemeManager
    {
        public static string ThemeResource(HttpContextBase context, string property)
        {
            var httpContext = context.ApplicationInstance.Context;
            return ThemeResource(CurrentTheme(httpContext), property);
        }

        public static string ThemeResource(HttpContext context, string property)
        {
            return ThemeResource(CurrentTheme(context), property);
        }

        /// <summary>
        /// Fetch specified property value from specified theme or default theme
        /// </summary>
        /// <param name="theme"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static string ThemeResource(string theme, string property)
        {
            Dictionary<string, string> resources = ThemeResources(theme);

            var rtn = "";

            if (resources.ContainsKey(property))
            {
                rtn = resources[property];
            }

            return rtn;
        }

        public static Dictionary<string, string> ThemeResources(HttpContextBase context)
        {
            var httpContext = context.ApplicationInstance.Context;
            return ThemeResources(CurrentTheme(httpContext));
        }

        public static Dictionary<string, string> ThemeResources(HttpContext context)
        {
            return ThemeResources(CurrentTheme(context));
        }

        /// <summary>
        /// Fetch specified property value from specified theme or default theme
        /// </summary>
        /// <param name="theme"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ThemeResources(string theme)
        {
            var defaultTheme = DefaultTheme();

            if (!Directory.Exists(RootThemeFolder + theme))
            {
                theme = defaultTheme;
            }

            Dictionary<string, string> resources = new Dictionary<string, string>();
            var defaultThemeResourceFile = RootThemeFolder + defaultTheme + "/resources.json";
            var defaultFileContents = ReadThemeFileContent(defaultThemeResourceFile);

            // Check whether any content from default theme found
            if (!string.IsNullOrEmpty(defaultFileContents))
            {
                // if yes then load resources
                resources = JsonConvert.DeserializeObject<Dictionary<string, string>>(defaultFileContents);
            }

            // Check if passed theme is same as default theme
            if (!theme.Equals(defaultTheme))
            {
                // If no then perform below activities
                var themeResourceFile = RootThemeFolder + theme + "/resources.json";
                var fileContents = ReadThemeFileContent(themeResourceFile);

                if (!string.IsNullOrEmpty(fileContents))
                {
                    Dictionary<string, string> tempResources = JsonConvert.DeserializeObject<Dictionary<string, string>>(fileContents);

                    foreach (var resource in tempResources)
                    {
                        if (!String.IsNullOrWhiteSpace(resource.Value))
                        {
                            if (resources.ContainsKey(resource.Key))
                                resources[resource.Key] = resource.Value;
                        }
                    }
                }
            }

            return resources;
        }

        public static string GetResourceKeyValue(Dictionary<string, string> resources, string keyName)
        {
            return (resources != null && resources.ContainsKey(keyName) && resources[keyName] == null ? resources[keyName] : string.Empty);
        }

        /// <summary>
        /// Read specified theme file content either from cache if available otherwise from actual file
        /// </summary>
        /// <param name="themeFileName"></param>
        /// <returns></returns>
        private static string ReadThemeFileContent(string themeFileName)
        {
            var themeFileContents = string.Empty;
            // Check whether default theme resource file content exist in cache
            if (CacheHelper.Instance.IsExists(themeFileName))
            {
                // If yes then load from cache
                themeFileContents = CacheHelper.Instance.Get<string>(themeFileName);
            }
            else
            {
                // if no then read from file and save in cache
                themeFileContents = URL.ReadAllText(themeFileName);
                CacheItemPolicy policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = DateTimeOffset.Now.AddDays(1);
                CacheHelper.Instance.Add(themeFileName, themeFileContents, policy);
            }

            return themeFileContents;
        }
    }
}
