using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

using System.Web.Hosting;
using System.Web.Mvc;
using Newtonsoft.Json;
using BHIC.Common;


namespace BHIC.Portal.Code.Configuration
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
                }

            if (!isDone)
            {
                foreach (var ext in extAllowed)
                {
                    filetype = ext;
                    if (URL.Exists(url + "." + ext))
                    {
                        contents = URL.ReadAllText(url + "." + ext);
                        break;
                    }
                }
            }

            if (model != null)
            {
                var propDictionary = model.GetType().GetProperties().Where(w => w.GetGetMethod() != null).Select(s => new { Name = s.Name, Value = s.GetGetMethod().Invoke(model, null) });

                foreach(var prop in propDictionary)
                {
					contents = contents.Replace("##" + prop.Name + "##", (prop.Value != null) ? prop.Value.ToString() : "");	// mjl - adjust to return an empty string if the property value is null
                }
            }

            // remove byte order mark (BOM), if the return value is prefixed with this value
            string _byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
            if (contents.StartsWith(_byteOrderMarkUtf8))
            {
                contents = contents.Remove(0, _byteOrderMarkUtf8.Length);
            }

            return contents;
        }
    }
}