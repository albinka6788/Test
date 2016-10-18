#region Using directives

using System;
using System.IO;
using System.Net;

#endregion

namespace BHIC.Common
{
    public static class URL
    {
        #region Methods

        #region Public Methods

        /// <summary>
        /// Return existance of url path in project cdn
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool Exists(string url) 
        {
            bool result = true;
            HttpWebRequest webRequest;

            try
            {
                webRequest = HttpWebRequest.CreateHttp(url);
                webRequest.KeepAlive = false;
                webRequest.Timeout = 5000; // miliseconds
                webRequest.Method = "HEAD";

                using (webRequest.GetResponse())
                { }
            }
            catch (Exception ex)
            {
                Logging.LoggingService.Instance.Error(string.Format("Error occurred while accessing {0} URL", url), ex);
                result = false;
            }
            webRequest = null;

            return result;
        }

        /// <summary>
        /// Read all file content as a byte array from specified URL location and return byte[]
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static byte[] ReadAllBytes(string url) 
        {
            byte[] bytes = null;

            //Comment : Here first check for file existance
            if (URL.Exists(url))
            {
                bytes = File.ReadAllBytes(url);
            }

            return bytes;            
        }

        /// <summary>
        /// Read all text content from specified URL location and return text string 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string ReadAllText(string url) 
        {
            String content = string.Empty;
            HttpWebRequest webRequest;

            try
            {
                webRequest = HttpWebRequest.CreateHttp(url);
                webRequest.KeepAlive = false;

                using (var response = webRequest.GetResponse())
                using (var responseStream = response.GetResponseStream())
                using (var reader = new StreamReader(responseStream))
                {
                    content = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Logging.LoggingService.Instance.Error(string.Format("Error reading from {0} URL", url), ex);
            }

            webRequest = null;
            return content;
        }

        #endregion

        #endregion
    }
}
