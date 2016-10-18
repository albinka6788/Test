using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

using BHIC.Common.Logging;
using BHIC.Common.Mailing;
using BHIC.Common.Reattempt;
using BHIC.Common.XmlHelper;
using BHIC.Domain.Service;
using BHIC.Domain.TransactionTrace;
using Newtonsoft.Json;

namespace BHIC.Common.Client
{
    public class SvcClient
    {
        #region Comment : Here class level variable/property

        #endregion

        #region Comment : Here api/service call logging variables

        private static ILoggingService logger = LoggingService.Instance;
        static DateTime startTime;
        static DateTime endTime;
        static int reAttemptNumber = 0;

        #endregion

        #region Public Static Method

        #region ReturnType CallService<ReturnType>

        public static ReturnType CallService<ReturnType>(string serviceName, ServiceProvider provider)
        {
            // call the specified serviceName
            WebClient client = null;
            reAttemptNumber = 0;
            do
            {
                try
                {
                    client = new WebClient();

                    #region Comment : Here Only on "Development" environmnet check for local stored copy to reduce app loading time

                    //In case want to reload local stored data then set 
                    if (ConfigCommonKeyReader.IsDevEnvironment && !ConfigCommonKeyReader.RelaodLocalData &&
                        !string.IsNullOrEmpty(ConfigCommonKeyReader.LocalStoredDataPath))
                    {
                        var getLocalData = GetLocalStoredData(serviceName);

                        //Comment : Here app able to get local stored data then by-pass further below processing 
                        if (!string.IsNullOrEmpty(getLocalData))
                            return JsonConvert.DeserializeObject<ReturnType>(getLocalData);
                    }

                    #endregion

                    //Comment : Here based on provider set headers to be communicated 
                    provider.SetWebClientHeaders(ServiceProvider.OperationType.GET, ref client, reAttemptNumber);

                    startTime = DateTime.Now;

                    //Comment : Here API Url prefix based on supplied Provider+Category
                    var insuranceApiUrl = provider.ServiceUrl();
                    var svcResponse = client.DownloadData(insuranceApiUrl + serviceName);

                    endTime = DateTime.Now;

                    string jsonResponse = Encoding.UTF8.GetString(svcResponse);

                    LogTrace("GET", serviceName, startTime, endTime, jsonResponse);

                    #region Comment : Here Only on "Development" environmnet check for local stored copy to reduce app loading time

                    if (ConfigCommonKeyReader.IsDevEnvironment && !ConfigCommonKeyReader.RelaodLocalData &&
                        !string.IsNullOrEmpty(ConfigCommonKeyReader.LocalStoredDataPath))
                    {
                        //Comment : Here app able to get local stored data then by-pass further below processing 
                        if (!string.IsNullOrEmpty(jsonResponse))
                            SetLocalStoredData(serviceName, jsonResponse);
                    }

                    #endregion

                    // deserialize / return the data
                    return JsonConvert.DeserializeObject<ReturnType>(jsonResponse);
                }
                catch (Exception ex)
                {
                    reAttemptNumber++;
                    logger.Fatal(string.Format("Service '{0}' Call Failed with below error message :{1}{2}", serviceName, Environment.NewLine, ex.ToString()));

                    if (!(ex is WebException) || 
                        ((ex is WebException) && (((WebException)ex).Response as HttpWebResponse).StatusCode != HttpStatusCode.Unauthorized) || 
                        reAttemptNumber > 2)
                    {
                        throw;
                    }
                }
                finally
                {
                    if (client != null)
                    {
                        client.Dispose();
                    }
                }
            } while (reAttemptNumber <= 2);

            return default(ReturnType);
        }

        public static ReturnType CallService<ReturnType>(string serviceConnectionName, string serviceName, ServiceProvider provider)
        {
            // call the specified serviceName
            WebClient client = null;
            reAttemptNumber = 0;
            do
            {
                try
                {
                    client = new WebClient();

                    //Comment : Here based on provider set headers to be communicated 
                    provider.SetWebClientHeaders(ServiceProvider.OperationType.GET, ref client, reAttemptNumber);

                    // get the URL to the Insurance Service
                    var services = new ServiceConnections();
                    var api = services[serviceConnectionName];

                    startTime = DateTime.Now;

                    //Comment : Here API Url prefix based on supplied Provider+Category
                    var insuranceApiUrl = provider.ServiceUrl();

                    // perform the request
                    var svcResponse = client.DownloadData(insuranceApiUrl + serviceName);

                    endTime = DateTime.Now;

                    // string jsonResponse = Encoding.ASCII.GetString(svcResponse);
                    string jsonResponse = Encoding.UTF8.GetString(svcResponse);

                    LogTrace("GET", serviceName, startTime, endTime, jsonResponse);

                    // deserialize / return the data
                    return JsonConvert.DeserializeObject<ReturnType>(jsonResponse);
                }
                catch (Exception ex)
                {
                    reAttemptNumber++;
                    logger.Fatal(string.Format("Service '{0}' Call Failed with below error message :{1}{2}", serviceName, Environment.NewLine, ex.ToString()));

                    if (!(ex is WebException) ||
                        ((ex is WebException) && (((WebException)ex).Response as HttpWebResponse).StatusCode != HttpStatusCode.Unauthorized) ||
                        reAttemptNumber > 2)
                    {
                        throw;
                    }
                }
                finally
                {
                    if (client != null)
                    {
                        client.Dispose();
                    }
                }
            } while (reAttemptNumber <= 2);

            return default(ReturnType);
        }

        #endregion

        #region ReturnType CallService<PostType, ReturnType>

        public static ReturnType CallService<PostType, ReturnType>(string serviceName, string serviceMethod, PostType postData, ServiceProvider provider)
        {
            WebClient client = null;
            var jsonPostData = string.Empty;
            try
            {
                // serialize the data to be posted
                jsonPostData = JsonConvert.SerializeObject(postData);
                var bytesPostData = Encoding.UTF8.GetBytes(jsonPostData);
                reAttemptNumber = 0;

                do
                {
                    try
                    {
                        using (client = new WebClient())
                        {
                            //Comment : Here based on provider set headers to be communicated 
                            provider.SetWebClientHeaders(ServiceProvider.OperationType.GET, ref client, reAttemptNumber);

                            startTime = DateTime.Now;

                            //Comment : Here API Url prefix based on supplied Provider+Category
                            var insuranceApiUrl = provider.ServiceUrl();
                            var svcResponse = client.UploadData(insuranceApiUrl + serviceName, serviceMethod, bytesPostData);

                            endTime = DateTime.Now;

                            // string jsonResponse = Encoding.ASCII.GetString(svcResponse);
                            string jsonResponse = Encoding.UTF8.GetString(svcResponse);

                            LogTrace("GET", serviceName, startTime, endTime, jsonResponse, jsonPostData);

                            // deserialize / return the data
                            return JsonConvert.DeserializeObject<ReturnType>(jsonResponse);
                        }
                    }
                    catch (Exception ex)
                    {
                        reAttemptNumber++;
                        if (!(ex is WebException) ||
                            ((ex is WebException) && (((WebException)ex).Response as HttpWebResponse).StatusCode != HttpStatusCode.Unauthorized) ||
                            reAttemptNumber > 2)
                        {
                            throw;
                        }
                        logger.Fatal(string.Format("Service '{0}' Call with below request parameter {1}{2}{3}{4}Failed with below error message :{5}{6}",
                            serviceName, Environment.NewLine, jsonPostData, Environment.NewLine, Environment.NewLine, Environment.NewLine, ex.ToString()));
                    }

                } while (reAttemptNumber <= 2);
            }
            catch (Exception ex)
            {
                logger.Fatal(string.Format("Service '{0}' Call with below request parameter {1}{2}{3}{4}Failed with below error message :{5}{6}",
                    serviceName, Environment.NewLine, jsonPostData, Environment.NewLine, Environment.NewLine, Environment.NewLine, ex.ToString()));
                throw;
            }
            finally
            {
                if (client != null)
                {
                    client.Dispose();
                }
            }

            return default(ReturnType);
        }

        #endregion

        #region ReturnType CallServicePost<PostType, ReturnType>

        public static ReturnType CallServicePost<PostType, ReturnType>(string serviceName, PostType postData, ServiceProvider provider)
        {

            WebClient client = null;
            var jsonPostData = string.Empty;
            try
            {
                // serialize the data to be posted
                jsonPostData = JsonConvert.SerializeObject(postData);
                var bytesPostData = Encoding.UTF8.GetBytes(jsonPostData);
                reAttemptNumber = 0;

                do
                {
                    try
                    {
                        using (client = new WebClient())
                        {
                            //Comment : Here based on provider set headers to be communicated 
                            provider.SetWebClientHeaders(ServiceProvider.OperationType.POST, ref client, reAttemptNumber);

                            startTime = DateTime.Now;

                            //Comment : Here API Url prefix based on supplied Provider+Category
                            var insuranceApiUrl = provider.ServiceUrl();
                            var svcResponse = client.UploadData(insuranceApiUrl + serviceName, "POST", bytesPostData);

                            endTime = DateTime.Now;

                            // string jsonResponse = Encoding.ASCII.GetString(svcResponse);
                            string jsonResponse = Encoding.UTF8.GetString(svcResponse);

                            LogTrace("POST", serviceName, startTime, endTime, jsonResponse, jsonPostData);

                            // deserialize / return the data
                            return JsonConvert.DeserializeObject<ReturnType>(jsonResponse);
                        }
                    }
                    catch (Exception ex)
                    {
                        reAttemptNumber++;
                        if (!(ex is WebException) ||
                            ((ex is WebException) && (((WebException)ex).Response as HttpWebResponse).StatusCode != HttpStatusCode.Unauthorized) ||
                            reAttemptNumber > 2)
                        {
                            throw;
                        }
                        logger.Fatal(string.Format("Service '{0}' Call with below request parameter {1}{2}{3}{4}Failed with below error message :{5}{6}",
                            serviceName, Environment.NewLine, jsonPostData, Environment.NewLine, Environment.NewLine, Environment.NewLine, ex.ToString()));
                    }

                } while (reAttemptNumber <= 2);

            }
            catch (Exception ex)
            {
                logger.Fatal(string.Format("Service '{0}' Call with below request parameter {1}{2}{3}{4}Failed with below error message :{5}{6}",
                    serviceName, Environment.NewLine, jsonPostData, Environment.NewLine, Environment.NewLine, Environment.NewLine, ex.ToString()));
                throw;
            }
            finally
            {
                if (client != null)
                {
                    client.Dispose();
                }
            }

            return default(ReturnType);
        }

        public static ReturnType CallServicePost<PostType, ReturnType>(string serviceConnectionName, string serviceName, PostType postData, ServiceProvider provider)
        {

            WebClient client = null;
            var jsonPostData = string.Empty;

            try
            {
                // serialize the data to be posted
                jsonPostData = JsonConvert.SerializeObject(postData);
                var bytesPostData = Encoding.UTF8.GetBytes(jsonPostData);

                // get the URL to the Insurance Service
                var services = new ServiceConnections();
                var api = services[serviceConnectionName];
                reAttemptNumber = 0;

                do
                {

                    try
                    {
                        // perform a WebClient operation
                        using (client = new WebClient())
                        {
                            //Comment : Here based on provider set headers to be communicated 
                            provider.SetWebClientHeaders(ServiceProvider.OperationType.POST, ref client, reAttemptNumber);

                            startTime = DateTime.Now;

                            //Comment : Here API Url prefix based on supplied Provider+Category
                            var insuranceApiUrl = provider.ServiceUrl();
                            // perform the request
                            var svcResponse = client.UploadData(insuranceApiUrl + serviceName, "POST", bytesPostData);

                            endTime = DateTime.Now;

                            // string jsonResponse = Encoding.ASCII.GetString(svcResponse);
                            string jsonResponse = Encoding.UTF8.GetString(svcResponse);

                            LogTrace("POST", serviceName, startTime, endTime, jsonResponse, jsonPostData);

                            // deserialize / return the data
                            return JsonConvert.DeserializeObject<ReturnType>(jsonResponse);
                        }
                    }
                    catch (Exception ex)
                    {
                        reAttemptNumber++;
                        if (!(ex is WebException) ||
                            ((ex is WebException) && (((WebException)ex).Response as HttpWebResponse).StatusCode != HttpStatusCode.Unauthorized) ||
                            reAttemptNumber > 2)
                        {
                            throw;
                        }
                        logger.Fatal(string.Format("Service '{0}' Call with below request parameter {1}{2}{3}{4}Failed with below error message :{5}{6}",
                            serviceName, Environment.NewLine, jsonPostData, Environment.NewLine, Environment.NewLine, Environment.NewLine, ex.ToString()));
                    }

                } while (reAttemptNumber <= 2);
            }
            catch (Exception ex)
            {
                logger.Fatal(string.Format("Service '{0}' Call with below request parameter {1}{2}{3}{4}Failed with below error message :{5}{6}",
                    serviceName, Environment.NewLine, jsonPostData, Environment.NewLine, Environment.NewLine, Environment.NewLine, ex.ToString()));
                throw;
            }
            finally
            {
                if (client != null)
                {
                    client.Dispose();
                }
            }

            return default(ReturnType);
        }

        #endregion

        #region string CallServiceBatch

        public static string CallServiceBatch(BatchActionList batchActionList, ServiceProvider provider)
        {

            // call the specified serviceName
            WebClient client = null;

            // ********** original code follows
            // convert the batchActionList to JSON; it will be deserialized back into a BatchActionList type by the Insurance Service
            NameValueCollection inputs = new NameValueCollection();
            reAttemptNumber = 0;

            do
            {
                try
                {
                    if (reAttemptNumber == 0)
                    {
                        //Comment : Here based on provider post nameValueCollection into "Batch POST" request
                        provider.SetBatchNameValueCollection(batchActionList, ref inputs);
                    }

                    client = new WebClient();

                    //Comment : Here based on provider set headers to be communicated 
                    provider.SetBatchWebClientHeaders(ServiceProvider.OperationType.GET, ref client, reAttemptNumber);

                    startTime = DateTime.Now;

                    //Comment : Here API Url prefix based on supplied Provider+Category
                    var insuranceApiUrl = provider.ServiceUrl();
                    var svcResponse = client.UploadValues(insuranceApiUrl + "batch", inputs);

                    endTime = DateTime.Now;

                    // string jsonResponse = Encoding.ASCII.GetString(svcResponse);
                    string jsonResponse = Encoding.UTF8.GetString(svcResponse);

                    LogTrace(batchActionList, "GET", GetServiceName(batchActionList), startTime, endTime, jsonResponse);

                    // deserialize / return the data
                    return jsonResponse;
                }
                catch (Exception ex)
                {
                    // todo: add exception handling / error logging
                    reAttemptNumber++;
                    var msg = ex.Message;
                    logger.Fatal(string.Format("Below Batch Service Request Call Failled{0}{1}with below error message :{2}{3}",
                        Environment.NewLine, JsonConvert.SerializeObject(batchActionList), Environment.NewLine, ex.ToString()));
                    if (!(ex is WebException) ||
                        ((ex is WebException) && (((WebException)ex).Response as HttpWebResponse).StatusCode != HttpStatusCode.Unauthorized) ||
                        reAttemptNumber > 2)
                    {
                        throw;
                    }
                }
                finally
                {
                    if (client != null)
                    {
                        client.Dispose();
                    }
                }
            } while (reAttemptNumber <= 2);

            return string.Empty;
        }

        #endregion

        #endregion

        #region Private Method

        /// <summary>
        /// Return concatenated service name
        /// </summary>
        /// <param name="batchActionList"></param>
        /// <returns></returns>
        private static string GetServiceName(BatchActionList batchActionList)
        {
            StringBuilder sb = new StringBuilder();

            batchActionList.BatchActions.ForEach(res => sb.Append(string.Format("{0} !! ", res.ServiceName)));

            return sb.ToString().Substring(0, sb.ToString().Length - 3);
        }

        #region Local storage data

        /// <summary>
        /// Get data from local stored data
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        private static string GetLocalStoredData(string serviceName)
        {
            string jsonResponse = string.Empty;

            #region Comment : Here get local stored data from specific TEXT file

            if (ExcludeLog(serviceName))
            {
                serviceName = serviceName.Split(new char[] { '?' }, StringSplitOptions.RemoveEmptyEntries)[0];

                string filePath = Path.Combine(ConfigCommonKeyReader.LocalStoredDataPath, string.Concat(serviceName, ".txt"));

                //Comment : Here specified file exists then only call read-text method
                if (File.Exists(filePath))
                {
                    jsonResponse = File.ReadAllText(filePath);
                }
            }

            #endregion

            return jsonResponse;
        }

        /// <summary>
        /// Save data in local storage
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="text"></param>
        private static void SetLocalStoredData(string serviceName, string text)
        {
            if (ExcludeLog(serviceName))
            {
                #region Comment : Here write data into specific file

                serviceName = serviceName.Split(new char[] { '?' }, StringSplitOptions.RemoveEmptyEntries)[0];

                string filePath = Path.Combine(ConfigCommonKeyReader.LocalStoredDataPath, string.Concat(serviceName, ".txt"));

                #region Comment : Here if directory not exist add file to folder to server location

                if (Path.GetDirectoryName(filePath).Length > 0 && !Directory.Exists(Path.GetDirectoryName(filePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                }

                #endregion

                //Comment : Here specified file exists then only call read-text method
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.Write(text);
                }

                #endregion
            }
        }

        #endregion

        #region LoggingTrace

        /// <summary>
        /// Log API activity trace
        /// </summary>
        /// <param name="callType"></param>
        /// <param name="serviceName"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="jsonResponse"></param>
        private static void LogTrace(string callType, string serviceName, DateTime startDate, DateTime endDate, string jsonResponse)
        {
            if (ConfigCommonKeyReader.IsTransactionLog)
            {
                TransactionLogTrace(callType, serviceName, startDate, endDate, jsonResponse);
            }
            if (ExcludeLog(serviceName))
            {
                jsonResponse = string.Empty;
            }
            logger.Trace(GetCompleteMessage(serviceName, string.Empty, jsonResponse, startTime, endTime));
        }

        /// <summary>
        /// Log API activity trace
        /// </summary>
        /// <param name="callType"></param>
        /// <param name="serviceName"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="jsonResponse"></param>
        /// <param name="jsonPostData"></param>
        private static void LogTrace(string callType, string serviceName, DateTime startDate, DateTime endDate, string jsonResponse, string jsonPostData)
        {
            if (ConfigCommonKeyReader.IsTransactionLog)
            {
                TransactionLogTrace(callType, serviceName, startDate, endDate, jsonResponse);
            }
            if (ExcludeLog(serviceName))
            {
                jsonResponse = string.Empty;
            }
            logger.Trace(GetCompleteMessage(serviceName, jsonPostData, jsonResponse, startTime, endTime));
        }

        /// <summary>
        /// Log API activity trace
        /// </summary>
        /// <param name="batchActionList"></param>
        /// <param name="calltype"></param>
        /// <param name="servicename"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <param name="jsonResponse"></param>
        private static void LogTrace(BatchActionList batchActionList, string calltype, string servicename, DateTime startdate, DateTime enddate, string jsonResponse)
        {
            if (ConfigCommonKeyReader.IsTransactionLog)
            {
                TransactionLogTrace(calltype, servicename, startdate, enddate, jsonResponse);
            }
            logger.Trace(GetCompleteMessage(JsonConvert.SerializeObject(batchActionList), string.Empty, jsonResponse, startTime, endTime, true));
        }

        /// <summary>
        /// Save API transaction in DB or file repository
        /// </summary>
        /// <param name="calltype"></param>
        /// <param name="servicename"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <param name="jsonResponse"></param>
        private static void TransactionLogTrace(string calltype, string servicename, DateTime startdate, DateTime enddate, string jsonResponse)
        {
            if (HttpContext.Current.Session != null && HttpContext.Current.Session["TransactionLogging"] != null)
            {
                TransactionLogCustomSessions.CustomSessionForApiRequestResponse(new ApiTransaction
                {
                    ApiCallType = calltype,
                    ApiName = servicename,
                    ApiCallRequestTime = startTime,
                    ApiCallResponseTime = endTime,
                    ApiRequestProcessTime = (int)(enddate - startdate).TotalMilliseconds,
                    ApiResponseSize = jsonResponse.Length
                });
            }
        }

        #endregion

        /// <summary>
        /// Generate complete message from request message and response with time
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="isBatchRequest"></param>
        /// <returns></returns>
        private static string GetCompleteMessage(string serviceName, string request, string response, DateTime startTime, DateTime endTime, bool isBatchRequest = false)
        {
            string completeMessage = UtilityFunctions.GetCompleteMessage(serviceName, request, response, startTime, endTime, isBatchRequest);

            RecordAPILogforSession(serviceName, completeMessage, response);

            return completeMessage;
        }

        /// <summary>
        /// Record API log for each session
        /// </summary>
        /// <param name="loggedMessage"></param>
        /// <param name="response"></param>
        private static void RecordAPILogforSession(string serviceName, string loggedMessage, string response)
        {
            try
            {
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    if (ConfigCommonKeyReader.ApiFailureEmailTo != null && ConfigCommonKeyReader.ApiFailureEmailTo.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        if (HttpContext.Current.Session["APILog"] != null)
                        {
                            sb.AppendLine(HttpContext.Current.Session["APILog"].ToString());
                        }
                        sb.AppendLine(loggedMessage);
                        string apiLog = sb.ToString();
                        if (apiLog.Length > (1024 * 1024 * 2))
                        {
                            apiLog = apiLog.Remove((1024 * 1024 * 2));
                        }
                        HttpContext.Current.Session["APILog"] = apiLog;
                        if (response.IndexOf("\"RequestSuccessful\":false", StringComparison.OrdinalIgnoreCase) >= 0 &&
                            (response.IndexOf("Messages\":[]", StringComparison.OrdinalIgnoreCase) > 0 || 
                                response.IndexOf("\"MessageType\":5", StringComparison.OrdinalIgnoreCase) >= 0)
                            )
                        {
                            string apiLogFileName = SaveAPILogFile(apiLog);
                            MailHelper mailHelper = new MailHelper();
                            if (string.IsNullOrEmpty(apiLogFileName))
                            {
                                mailHelper.SendMailMessage(ConfigCommonKeyReader.ApiFailureEmailFrom,
                                    ConfigCommonKeyReader.ApiFailureEmailTo, ConfigCommonKeyReader.ApiFailureEmailCc, ConfigCommonKeyReader.ApiFailureEmailBcc,
                                    string.Concat(serviceName, " API Return Error as response"),
                                    string.Concat("Following Error occurred in API call, see attached file for step performed by user",
                                    Environment.NewLine, response, Environment.NewLine, "Note: File not saved in Server"),
                                    new MemoryStream(Encoding.UTF8.GetBytes(apiLog)), "APIErrorLogFile.log");
                            }
                            else
                            {
                                mailHelper.SendMailMessage(ConfigCommonKeyReader.ApiFailureEmailFrom,
                                    ConfigCommonKeyReader.ApiFailureEmailTo, ConfigCommonKeyReader.ApiFailureEmailCc, ConfigCommonKeyReader.ApiFailureEmailBcc,
                                    string.Concat(serviceName, " API Return Error as response"),
                                    string.Concat("Following Error occurred in API call, see attached file for step performed by user", Environment.NewLine,
                                    response), new List<string> { apiLogFileName });

                                HttpContext.Current.Session["APILog"] = null;
                            }
                        }
                    }
                    else
                    {
                        if (HttpContext.Current.Session["APILog"] != null)
                        {
                            HttpContext.Current.Session["APILog"] = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(string.Concat(serviceName, " API Log failed"), ex);
            }
        }

        /// <summary>
        /// Save API Log File
        /// </summary>
        /// <param name="apiLog"></param>
        /// <returns></returns>
        private static string SaveAPILogFile(string apiLog)
        {
            string apiLogFileName = string.Empty;
            try
            {
                apiLogFileName = string.Format(ConfigCommonKeyReader.APILogFileName, Guid.NewGuid());
                if (!Directory.Exists(Path.GetDirectoryName(apiLogFileName)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(apiLogFileName));
                }
                File.WriteAllText(apiLogFileName, apiLog);
            }
            catch (Exception ex)
            {
                logger.Fatal("Save API Log failed", ex);
            }
            return apiLogFileName;
        }

        /// <summary>
        /// Return true logging has to be excluded for supplied service
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        private static bool ExcludeLog(string serviceName)
        {
            switch (serviceName.ToUpper())
            {
                case "COUNTIES?ZIPCODE=":
                case "INDUSTRIES?LOB=WC":
                case "SUBINDUSTRIES?LOB=WC":
                case "CLASSDESCRIPTIONS?LOB=WC":
                    return true;
                default:
                    {
                        if (serviceName.Contains("SystemVariables?"))
                        {
                            return true;
                        }

                        break;
                    }
            }

            return false;
        }

        #endregion
    }
}