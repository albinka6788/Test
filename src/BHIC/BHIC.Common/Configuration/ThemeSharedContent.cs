using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

using BHIC.Common;
using BHIC.Common.XmlHelper;

namespace BHIC.Common.Configuration
{
    public static partial class ThemeManager
    {
        public static string ThemeSharedContent(string filename)
        {
            return ThemeSharedContent(filename, null);
        }

        public static string ThemeSharedContent(string filename, object model)
        {
            var url = RootThemeFolder + @"_sharedFiles\" + filename;
            var filetype = "";
            var isDone = false;

            var extAllowed = new List<string>() { "html", "htm", "txt" };
            var contents = "";

            if (filename.Contains("."))
                if (URL.Exists(url))
                {
                    contents = URL.ReadAllText(url);
                    isDone = true;
                    LogContent(string.Format("URL Exist with content = {0}", contents));
                }

            if (!isDone)
            {
                foreach (var ext in extAllowed)
                {
                    filetype = ext;
                    if (URL.Exists(url + "." + ext))
                    {
                        contents = URL.ReadAllText(url + "." + ext);
                        LogContent(string.Format("URL extension {0} Exist with content = {1}", ext, contents));
                        break;
                    }
                }
            }

            if (model != null && contents.Contains("##"))
            {
                LogContent("Entered Model binding for content code block");
                var propDictionary = model.GetType().GetProperties().Where(w => w.GetGetMethod() != null).Select(s => new { Name = s.Name, Value = s.GetGetMethod().Invoke(model, null) });

                foreach (var prop in propDictionary)
                {
                    contents = contents.Replace("##" + prop.Name + "##", (prop.Value != null) ? prop.Value.ToString() : "");	// mjl - adjust to return an empty string if the property value is null
                }
            }

            // remove byte order mark (BOM), if the return value is prefixed with this value
            string _byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
            LogContent(string.Format("_byteOrderMarkUtf8 = {0} with length {1}", _byteOrderMarkUtf8, _byteOrderMarkUtf8.Length));
            LogContent(contents);
            if (contents.StartsWith(_byteOrderMarkUtf8, StringComparison.OrdinalIgnoreCase))
            {
                LogContent(string.Format("BOM found = {1}", contents.Substring(0, _byteOrderMarkUtf8.Length)));
                contents = contents.Remove(0, _byteOrderMarkUtf8.Length);
            }
            LogContent(string.Format("Final Content = {0}", contents));

            return contents;
        }

        public static string ThemeSharedContentFileUrl(string filename)
        {
            return ThemeSharedContentFileUrl(filename, null);
        }

        public static string ThemeSharedContentFileUrl(string filename,string extention)
        {
            var url = RootThemeFolder + @"_sharedFiles/" + filename;
            var isDone = false;

            var extAllowed = new List<string>() { "html", "htm", "txt","pdf","xls","xlsx","js","css" };

            //scenario - 1 
            if (filename.Contains("."))
                if (URL.Exists(url))
                {
                    isDone = true;
                }

            //scenario - 2 
            if (!filename.Contains(".") && extAllowed.Contains(extention))
                if (URL.Exists(url))
                {
                    url += "." + extention.Replace(".","");
                    isDone = true;
                }

            //scenario - 3 
            if (!isDone)
            {
                foreach (var ext in extAllowed)
                {
                    if (URL.Exists(url + "." + ext))
                    {
                        url += "." + ext;
                        break;
                    }
                }
            }

            return url;
        }

        public static string ThemeSharedContentFileBaseUrl()
        {
            var url = RootThemeFolder + DefaultTheme() + @"/";
            return url;
        }

        //Common : Here method will return base url for shared CDN common folder images
        public static string ThemeSharedCommonImagesBaseUrl()
        {
            var url = CDN.Path + @"/Content/Common/images/";
            return url;
        }

        /// <summary>
        /// Logging trace
        /// </summary>
        /// <param name="log"></param>
        private static void LogContent(string log)
        {
            if (ConfigCommonKeyReader.EnableContentLogging)
            {
                Logging.ILoggingService loggingService = Logging.LoggingService.Instance;
                loggingService.Trace(log);
            }
        }
    }
}
