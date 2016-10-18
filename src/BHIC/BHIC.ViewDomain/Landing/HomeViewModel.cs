using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.ViewDomain.Landing
{
    public class HomeViewModel
    {
        #region Home Page Live Chat Configurations

        public string LCLicense { get; set; }

        public string LCGroup { get; set; }

        public string LCServerName { get; set; }

        public string LCServerValue { get; set; }

        public string LCSrc { get; set; }

        public string EnvironmentName { get; set; }

        //Google analytics
        public string GACode { get; set; }

        public object SessionData { get; set; }

        public int SystemIdleDuration { get; set; }

        #endregion
    }
}
