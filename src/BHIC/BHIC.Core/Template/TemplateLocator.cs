using BHIC.Common;
using BHIC.Common.XmlHelper;
using BHIC.Contract.Template;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BHIC.Core.Template
{
    public class TemplateLocator : ITemplateLocator
    {
        private readonly string _basePath;

        public TemplateLocator(string basePath)
        {
            if (basePath.IndexOf("{0}") > -1)
            {
                _basePath = string.Format(basePath, ConfigCommonKeyReader.HostURL);
            }
            else
            {
                _basePath = basePath;
            }

            if (_basePath == null || _basePath == string.Empty) throw new ArgumentNullException("NotificationTemplatePath");
        }

        string ITemplateLocator.Locate(string TemplateName)
        {
            var cdnUrl = string.Empty;
            var templateName = Encryption.EncryptText(TemplateName);
            var requestParam =new {
                                 template=templateName,
                                 appName=ConfigCommonKeyReader.AppName
                             };

            if (!_basePath.ToLower().StartsWith("http"))
            {
                cdnUrl = GetSchemeAndHostURLPart();
            }

            cdnUrl = string.Concat(cdnUrl, _basePath, UtilityFunctions.CreateQueryString(requestParam));
            return cdnUrl;
        }

        protected string GetSchemeAndHostURLPart()
        {
            return string.Concat(HttpContext.Current.Request.Url.Scheme, "://", HttpContext.Current.Request.Url.Host);
        }
    }
}
