using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Common;

namespace BHIC.Common.Client
{
    public class ServiceConnectionsConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
        public ServiceConnectionCollection Services
        {
            get { return (ServiceConnectionCollection)this[""]; }
            set { this[""] = value; }
        }
    }

    public class ServiceConnections : Dictionary<string, ServiceConnectionElement>
    {
        public ServiceConnections(string configCustomServiceSectionName = "serviceConnectionsGuard")
        {
            var config = ConfigurationManager.GetSection(configCustomServiceSectionName) as ServiceConnectionsConfigSection;

            foreach (ServiceConnectionElement s in config.Services)
            {
                this.Add(s.Name, s);
            }
        }
    }

    public class ServiceConnectionCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ServiceConnectionElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ServiceConnectionElement)element).Name;
        }
    }

    public class ServiceConnectionElement : ConfigurationElement
    {
        bool isUserNameDecrypted = false;
        bool isPasswordDecrypted = false;
        bool isUserNameFieldDecrypted = false;
        bool isPasswordFieldDecrypted = false;
        bool isClientIDDecrypted = false;
        bool isSiteNumberDecrypted = false;
        string userName = string.Empty;
        string password = string.Empty;
        string userNameField = string.Empty;
        string passwordField = string.Empty;
        string clientID = string.Empty;
        string siteNumber = string.Empty;

        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }

        [ConfigurationProperty("url", IsRequired = true)]
        public string Url
        {
            get
            {
                string apiURL = (string)base["url"];
                if (!apiURL.EndsWith("/"))
                {
                    apiURL = apiURL + "/";
                }
                return apiURL;
            }
            set { base["url"] =  value; }
        }

        [ConfigurationProperty("authUrl")]
        public string AuthUrl
        {
            get { return (string)base["authUrl"]; }
            set { base["authUrl"] = value; }
        }

        [ConfigurationProperty("username")]
        public string Username
        {
            get
            {
                if (!isUserNameDecrypted)
                {
                    userName = DecryptKeyValue("username");
                    isUserNameDecrypted = true;
                }
                return userName;
            }
            set { base["username"] = EncryptKeyValue(value); }
        }

        [ConfigurationProperty("password")]
        public string Password
        {
            get
            {
                if (!isPasswordDecrypted)
                {
                    password = DecryptKeyValue("password");
                    isPasswordDecrypted = true;
                }
                return password;
            }
            set { base["password"] = EncryptKeyValue(value); }
        }

        [ConfigurationProperty("usernameField")]
        public string UsernameField
        {
            get
            {
                if (!isUserNameFieldDecrypted)
                {
                    userNameField = DecryptKeyValue("usernameField");
                    isUserNameFieldDecrypted = true;
                }
                return userNameField;
            }
            set { base["usernameField"] = EncryptKeyValue(value); }
        }

        [ConfigurationProperty("passwordField")]
        public string PasswordField
        {
            get
            {
                if (!isPasswordFieldDecrypted)
                {
                    passwordField = DecryptKeyValue("passwordField");
                    isPasswordFieldDecrypted = true;
                }
                return passwordField;
            }
            set { base["passwordField"] = EncryptKeyValue(value); }
        }

        [ConfigurationProperty("clientId")]
        public string ClientId
        {
            get
            {
                if (!isClientIDDecrypted)
                {
                    clientID = DecryptKeyValue("clientId");
                    isClientIDDecrypted = true;
                }
                return clientID;
            }
            set { base["clientId"] = EncryptKeyValue(value); }
        }

        [ConfigurationProperty("secret")]
        public string Secret
        {
            get { return (string)base["secret"]; }
            set { base["secret"] = value; }
        }

        [ConfigurationProperty("authType", IsRequired = true)]
        public ServiceAuthType AuthType
        {
            get { return (ServiceAuthType)this["authType"]; }
            set { this["authType"] = value; }
        }

        [ConfigurationProperty("siteNumber")]
        public string SiteNumber
        {
            get
            {
                if (!isSiteNumberDecrypted)
                {
                    siteNumber = DecryptKeyValue("siteNumber");
                    isSiteNumberDecrypted = true;
                }
                return siteNumber;
            }
            set { base["siteNumber"] = EncryptKeyValue(value); }
        }

        [ConfigurationProperty("testCall")]
        public string TestCall
        {
            get { return (string)base["testCall"]; }
            set { this["testCall"] = value; }
        }

        private string EncryptKeyValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            else
            {
                return Encryption.EncryptText(value, this.Name);
            }
        }

        private string DecryptKeyValue(string keyName)
        {
            if (string.IsNullOrEmpty(Convert.ToString(base[keyName])))
            {
                return string.Empty;
            }
            else
            {
                return Encryption.DecryptText(Convert.ToString(base[keyName]), this.Name);
            }
        }
    }

    [Flags]
    public enum ServiceAuthType
    {
        None = 0,
        OAuth = 1,
        HeaderCredentials = 2
    }
}
