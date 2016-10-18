using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Common.SchedulerService
{
    public class WcServiceConfigLoader : ConfigurationSection
    {
        /// <summary>
        /// The name of this section in the app.config.
        /// </summary>
        public const string SectionName = "WcServices";

        private const string EndpointCollectionName = "WcServiceList";

        [ConfigurationProperty(EndpointCollectionName)]
        [ConfigurationCollection(typeof(ConnectionManagerEndpointsCollection), AddItemName = "WcService")]
        public ConnectionManagerEndpointsCollection ConnectionManagerEndpoints { get { return (ConnectionManagerEndpointsCollection)base[EndpointCollectionName]; } }
    }

    public class ConnectionManagerEndpointsCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ConnectionManagerEndpointElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ConnectionManagerEndpointElement)element).ServiceName;
        }
    }

    public class ConnectionManagerEndpointElement : ConfigurationElement
    {
        private static object _lockObject = new object();
        bool isServiceRun = false;

        [ConfigurationProperty("serviceName")]
        public string ServiceName
        {
            get { return ((string)(base["serviceName"])); }
            set { base["serviceName"] = value; }
        }

        [ConfigurationProperty("isServiceNeedToRun")]
        public bool IsServiceNeedToRun
        {
            get
            {
                bool retValue = false;
                bool.TryParse(base["isServiceNeedToRun"].ToString(), out retValue);
                return retValue;
            }
            set { base["isServiceNeedToRun"] = value; }
        }

        [ConfigurationProperty("repeatTaskInterval")]
        public int RepeatTaskInterval
        {
            get
            {
                int retValue = 30;
                int.TryParse(base["repeatTaskInterval"].ToString(), out retValue);
                return retValue;
            }
            set { base["repeatTaskInterval"] = value; }
        }

        [ConfigurationProperty("lastRunStartDateTime")]
        public DateTime LastRunStartDateTime
        {
            get
            {
                DateTime retValue = DateTime.Now;
                DateTime.TryParse(base["lastRunStartDateTime"].ToString(), out retValue);
                return retValue;
            }
            set
            {
                lock (_lockObject)
                {
                    base["lastRunStartDateTime"] = value;
                }
            }
        }

        [ConfigurationProperty("lastRunEndDateTime")]
        public DateTime LastRunEndDateTime
        {
            get
            {
                DateTime retValue = DateTime.Now;
                DateTime.TryParse(base["lastRunEndDateTime"].ToString(), out retValue);
                return retValue;
            }
            set
            {
                lock (_lockObject)
                {
                    base["lastRunEndDateTime"] = value;
                }
            }
        }

        [ConfigurationProperty("isMultipleRun")]
        public bool IsMultipleRun
        {
            get
            {
                bool retValue = true;
                bool.TryParse(base["isMultipleRun"].ToString(), out retValue);
                return retValue;
            }
            set
            {
                lock (_lockObject)
                {
                    base["isMultipleRun"] = value;
                }
            }
        }

        public bool IsServiceRun
        {
            get
            {
                return isServiceRun;
            }
            set
            {
                lock (_lockObject)
                {
                    isServiceRun = value;
                }
            }
        }
        public override bool IsReadOnly()
        {
            return false;
        }

        [ConfigurationProperty("isSpecificTime")]
        public bool IsSpecificTime
        {
            get
            {
                bool retValue = false;
                bool.TryParse(base["isSpecificTime"].ToString(), out retValue);
                return retValue;
            }
            set
            {
                lock (_lockObject)
                {
                    base["isSpecificTime"] = value;
                }
            }
        }
        [ConfigurationProperty("scheduledTime")]
        public string ScheduledTime
        {
            get
            {
                string retValue = "";
                retValue=base["scheduledTime"].ToString();
                return retValue;
            }
            set
            {
                lock (_lockObject)
                {
                    base["scheduledTime"] = value;
                }
            }
        }
        
       
    }
}
