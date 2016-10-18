using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BHIC.Common.XmlHelper;

namespace BHIC.Common.DataAccess
{
    public class AppSettings
    {
        public static string SchemaName
        {
            get
            {
                return ConfigCommonKeyReader.DBSchemaName;
            }
        }
    }
}