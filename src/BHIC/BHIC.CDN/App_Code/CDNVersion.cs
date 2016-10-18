using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Compilation;

namespace BHIC.CDN
{
    public static class CDNVersion
    {
        public static string GetVersion()
        {
            Version webAssemblyVersion = BuildManager.GetGlobalAsaxType().BaseType.Assembly.GetName().Version;
            return string.Concat(webAssemblyVersion.Major, webAssemblyVersion.Minor, webAssemblyVersion.Build, webAssemblyVersion.Revision);
        }
    }
}
