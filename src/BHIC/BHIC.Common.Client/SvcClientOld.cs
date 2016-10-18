using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using BHIC.Common.Logging;
using BHIC.Domain.Service;

namespace BHIC.Common.Client
{
    public class SvcClientOld
    {
        // return the InsuranceSystemUrl value from the web.config
        private static ServiceConnectionElement insuranceApi { get { var services = new ServiceConnections(); return services["InsuranceApi"]; } }
        private static ILoggingService logger = LoggingService.Instance;
        static DateTime startTime;
        static DateTime endTime;

        #region ReturnType CallService<ReturnType>
        public static ReturnType CallService<ReturnType>(string serviceName)
        {
            try
            {
                // call the specified serviceName
                var client = new WebClient();
                client.Headers.Add("Authorization", "Bearer " + ObtainOAuthToken().AccessToken);

                startTime = DateTime.Now;

                var svcResponse = client.DownloadString(insuranceApi.Url + serviceName);

                endTime = DateTime.Now;

                //logger.Trace(serviceName, string.Empty, svcResponse, startTime, endTime);

                // deserialize / return the data
                return JsonConvert.DeserializeObject<ReturnType>(svcResponse);
            }
            catch (Exception ex)
            {
                logger.Fatal(string.Format("Service {0} Call with error message : {1}", serviceName, ex.ToString()));
                throw;
            }
        }

        public static ReturnType CallService<ReturnType>(string serviceName, ref OAuthModel oauthRequest)
        {
            try
            {
                // call the specified serviceName
                var client = new WebClient();

                if (String.IsNullOrEmpty(oauthRequest.AccessToken))
                    oauthRequest = ObtainOAuthToken(oauthRequest);

                client.Headers.Add("Authorization", "Bearer " + oauthRequest.AccessToken);

                startTime = DateTime.Now;

                var svcResponse = client.DownloadString(insuranceApi.Url + serviceName);

                endTime = DateTime.Now;

                //logger.Trace(serviceName, string.Empty, svcResponse, startTime, endTime);

                // deserialize / return the data
                return JsonConvert.DeserializeObject<ReturnType>(svcResponse);
            }
            catch (Exception ex)
            {
                logger.Fatal(string.Format("Service {0} Call with error message : {1}", serviceName, ex.ToString()));
                throw;
            }
        }

        public static ReturnType CallService<ReturnType>(string serviceConnectionName, string serviceName)
        {
            try
            {
                // get the URL to the Insurance Service
                var services = new ServiceConnections();
                var api = services[serviceConnectionName];

                // perform a WebClient operation
                using (var client = new WebClient())
                {
                    // if we're using OAuth, get the OAuth bearer token and add it.
                    if (api.AuthType == ServiceAuthType.OAuth)
                    {
                        client.Headers.Add("Authorization", "Bearer " + ObtainOAuthToken(api).AccessToken);
                    }

                    // else, if we're using HeaderCredentials, add the credentials to the header
                    if (api.AuthType == ServiceAuthType.HeaderCredentials)
                    {
                        if (String.IsNullOrEmpty(api.UsernameField))
                            client.Headers.Add("username", api.Username ?? "");
                        else
                            client.Headers.Add(api.UsernameField, api.Username ?? "");


                        if (String.IsNullOrEmpty(api.PasswordField))
                            client.Headers.Add("password", api.Password ?? "");
                        else
                            client.Headers.Add(api.PasswordField, api.Password ?? "");
                    }

                    // set content type
                    client.Headers.Add("Content-Type", "application/json");

                    startTime = DateTime.Now;

                    // perform the request
                    var svcResponse = client.DownloadData(api.Url + serviceName);

                    endTime = DateTime.Now;

                    string jsonResponse = Encoding.ASCII.GetString(svcResponse);

                    //logger.Trace(serviceName, string.Empty, jsonResponse, startTime, endTime);

                    // deserialize / return the data
                    return JsonConvert.DeserializeObject<ReturnType>(jsonResponse);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(string.Format("Service {0} Call with error message : {1}", serviceName, ex.ToString()));
                throw;
            }
        }

        public static ReturnType CallService<ReturnType>(string serviceConnectionName, string serviceName, ref OAuthModel oauthRequest)
        {
            try
            {
                // get the URL to the Insurance Service
                var services = new ServiceConnections();
                var api = services[serviceConnectionName];

                // perform a WebClient operation
                using (var client = new WebClient())
                {
                    // if we're using OAuth, get the OAuth bearer token and add it.
                    if (api.AuthType == ServiceAuthType.OAuth)
                    {
                        oauthRequest = ObtainOAuthToken(api);
                        client.Headers.Add("Authorization", "Bearer " + oauthRequest.AccessToken);
                    }

                    // else, if we're using HeaderCredentials, add the credentials to the header
                    if (api.AuthType == ServiceAuthType.HeaderCredentials)
                    {
                        if (String.IsNullOrEmpty(api.UsernameField))
                            client.Headers.Add("username", api.Username ?? "");
                        else
                            client.Headers.Add(api.UsernameField, api.Username ?? "");


                        if (String.IsNullOrEmpty(api.PasswordField))
                            client.Headers.Add("password", api.Password ?? "");
                        else
                            client.Headers.Add(api.PasswordField, api.Password ?? "");
                    }

                    // set content type
                    client.Headers.Add("Content-Type", "application/json");

                    startTime = DateTime.Now;

                    // perform the request
                    var svcResponse = client.DownloadData(api.Url + serviceName);

                    endTime = DateTime.Now;

                    string jsonResponse = Encoding.ASCII.GetString(svcResponse);

                    //logger.Trace(serviceName, string.Empty, jsonResponse, startTime, endTime);

                    // deserialize / return the data
                    return JsonConvert.DeserializeObject<ReturnType>(jsonResponse);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(string.Format("Service {0} Call with error message : {1}", serviceName, ex.ToString()));
                throw;
            }
        }
        #endregion

        #region ReturnType CallService<PostType, ReturnType>

        public static ReturnType CallService<PostType, ReturnType>(string serviceName, string serviceMethod, PostType postData)
        {
            try
            {
                // serialize the data to be posted
                var jsonPostData = JsonConvert.SerializeObject(postData);
                var bytesPostData = Encoding.Default.GetBytes(jsonPostData);

                using (var client = new WebClient())
                {
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Authorization", "Bearer " + ObtainOAuthToken().AccessToken);

                    startTime = DateTime.Now;

                    var svcResponse = client.UploadData(insuranceApi.Url + serviceName, serviceMethod, bytesPostData);

                    endTime = DateTime.Now;

                    string jsonResponse = Encoding.ASCII.GetString(svcResponse);

                    //logger.Trace(serviceName, jsonPostData, jsonResponse, startTime, endTime);

                    // deserialize / return the data
                    return JsonConvert.DeserializeObject<ReturnType>(jsonResponse);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(string.Format("Service {0} Call with error message : {1}", serviceName, ex.ToString()));
                throw;
            }
        }

        public static ReturnType CallService<PostType, ReturnType>(string serviceName, string serviceMethod, PostType postData, ref OAuthModel oauthRequest)
        {

            try
            {
                // serialize the data to be posted
                var jsonPostData = JsonConvert.SerializeObject(postData);
                var bytesPostData = Encoding.Default.GetBytes(jsonPostData);

                using (var client = new WebClient())
                {
                    if (String.IsNullOrEmpty(oauthRequest.AccessToken))
                        oauthRequest = ObtainOAuthToken(oauthRequest);

                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Authorization", "Bearer " + oauthRequest.AccessToken);

                    startTime = DateTime.Now;

                    var svcResponse = client.UploadData(insuranceApi.Url + serviceName, serviceMethod, bytesPostData);

                    endTime = DateTime.Now;

                    string jsonResponse = Encoding.ASCII.GetString(svcResponse);

                    //logger.Trace(serviceName, jsonPostData, jsonResponse, startTime, endTime);

                    // deserialize / return the data
                    return JsonConvert.DeserializeObject<ReturnType>(jsonResponse);
                }

            }
            catch (Exception ex)
            {
                logger.Fatal(string.Format("Service {0} Call with error message : {1}", serviceName, ex.ToString()));
                throw;
            }
        }

        public static ReturnType CallService<PostType, ReturnType>(string serviceName, PostType postData)
        {
            #region Comment : Here create BatchAction list object

            var batchActionList = new BatchActionList();	// BatchActionList: list of actions routed to the Insurance Service for response
            BatchResponseList batchResponseList;			// BatchResponseList: list of responses returned from the Insurance Service
            string jsonResponse;							// JSON-formated response data returned from the Insurance Service

            // flag that determines how to respond after processing the response received
            bool success = true;

            // deserialize each response returned by the service, and copy the OperationStatus to the view model for demo purposes
            OperationStatus operationStatus = new OperationStatus();

            #endregion

            #region Comment : Here get details based on supplied ReturnType

            var batchAction = new BatchAction { ServiceName = serviceName, ServiceMethod = "GET", RequestIdentifier = serviceName + " Data" };
            batchAction.BatchActionParameters.Add(new BatchActionParameter { Name = postData.GetType().Name, Value = JsonConvert.SerializeObject(postData) });
            batchActionList.BatchActions.Add(batchAction);

            jsonResponse = SvcClientOld.CallServiceBatch(batchActionList);

            // deserialize the results into a BatchResponseList
            batchResponseList = JsonConvert.DeserializeObject<BatchResponseList>(jsonResponse);

            // deserialize each response returned by the service, and copy the OperationStatus to the view model for demo purposes
            operationStatus = new OperationStatus();
            foreach (var batchResponse in batchResponseList.BatchResponses)
            {
                // this flag tested later to determine whether to proceed to the next view or not
                if (!batchResponse.RequestSuccessful) { success = false; }
            }

            #region Comment : Here get Deserialize ReturnType Object

            var returnedResponse = string.Empty;

            if (batchResponseList != null && success)
            {
                returnedResponse = batchResponseList.BatchResponses[0].JsonResponse;

                //reset everything
                batchActionList = null;
                batchResponseList = null;
                jsonResponse = null;
                success = true;
                operationStatus = null;

                // deserialize / return the data
                return JsonConvert.DeserializeObject<ReturnType>(returnedResponse);
            }

            #endregion

            #endregion

            // deserialize / return the data
            return JsonConvert.DeserializeObject<ReturnType>(jsonResponse);
        }

        #endregion

        #region ReturnType CallServicePost<PostType, ReturnType>
        public static ReturnType CallServicePost<PostType, ReturnType>(string serviceName, PostType postData)
        {

            try
            {
                // serialize the data to be posted
                var jsonPostData = JsonConvert.SerializeObject(postData);
                var bytesPostData = Encoding.Default.GetBytes(jsonPostData);

                using (var client = new WebClient())
                {
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Authorization", "Bearer " + ObtainOAuthToken().AccessToken);

                    startTime = DateTime.Now;

                    var svcResponse = client.UploadData(insuranceApi.Url + serviceName, "POST", bytesPostData);

                    endTime = DateTime.Now;

                    string jsonResponse = Encoding.ASCII.GetString(svcResponse);

                    //logger.Trace(serviceName, jsonPostData, jsonResponse, startTime, endTime);

                    // deserialize / return the data
                    return JsonConvert.DeserializeObject<ReturnType>(jsonResponse);
                }

            }
            catch (Exception ex)
            {
                logger.Fatal(string.Format("Service {0} Call with error message : {1}", serviceName, ex.ToString()));
                throw;
            }
        }

        public static ReturnType CallServicePost<PostType, ReturnType>(string serviceName, PostType postData, ref OAuthModel oauthRequest)
        {

            try
            {
                // serialize the data to be posted
                var jsonPostData = JsonConvert.SerializeObject(postData);
                var bytesPostData = Encoding.Default.GetBytes(jsonPostData);

                using (var client = new WebClient())
                {
                    if (String.IsNullOrEmpty(oauthRequest.AccessToken))
                        oauthRequest = ObtainOAuthToken(oauthRequest);

                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Authorization", "Bearer " + oauthRequest.AccessToken);

                    startTime = DateTime.Now;

                    var svcResponse = client.UploadData(insuranceApi.Url + serviceName, "POST", bytesPostData);

                    endTime = DateTime.Now;

                    string jsonResponse = Encoding.ASCII.GetString(svcResponse);

                    //logger.Trace(serviceName, jsonPostData, jsonResponse, startTime, endTime);

                    // deserialize / return the data
                    return JsonConvert.DeserializeObject<ReturnType>(jsonResponse);
                }

            }
            catch (Exception ex)
            {
                logger.Fatal(string.Format("Service {0} Call with error message : {1}", serviceName, ex.ToString()));
                throw;
            }
        }

        public static ReturnType CallServicePost<PostType, ReturnType>(string serviceConnectionName, string serviceName, PostType postData)
        {

            try
            {
                // serialize the data to be posted
                var jsonPostData = JsonConvert.SerializeObject(postData);
                var bytesPostData = Encoding.Default.GetBytes(jsonPostData);

                // get the URL to the Insurance Service
                var services = new ServiceConnections();
                var api = services[serviceConnectionName];
                //string insuranceServiceUrl = ConfigurationManager.AppSettings[baseUrlAppSetting];

                // perform a WebClient operation
                using (var client = new WebClient())
                {
                    // if we're using OAuth, get the OAuth bearer token and add it.
                    if (api.AuthType == ServiceAuthType.OAuth)
                    {
                        client.Headers.Add("Authorization", "Bearer " + ObtainOAuthToken(api).AccessToken);
                    }

                    // else, if we're using HeaderCredentials, add the credentials to the header
                    if (api.AuthType == ServiceAuthType.HeaderCredentials)
                    {
                        if (String.IsNullOrEmpty(api.UsernameField))
                            client.Headers.Add("username", api.Username ?? "");
                        else
                            client.Headers.Add(api.UsernameField, api.Username ?? "");


                        if (String.IsNullOrEmpty(api.PasswordField))
                            client.Headers.Add("password", api.Password ?? "");
                        else
                            client.Headers.Add(api.PasswordField, api.Password ?? "");
                    }

                    // set content type
                    client.Headers.Add("Content-Type", "application/json");

                    startTime = DateTime.Now;

                    // perform the request
                    var svcResponse = client.UploadData(api.Url + serviceName, "POST", bytesPostData);

                    endTime = DateTime.Now;

                    string jsonResponse = Encoding.ASCII.GetString(svcResponse);

                    //logger.Trace(serviceName, jsonPostData, jsonResponse, startTime, endTime);

                    // deserialize / return the data
                    return JsonConvert.DeserializeObject<ReturnType>(jsonResponse);
                }

            }
            catch (Exception ex)
            {
                logger.Fatal(string.Format("Service {0} Call with error message : {1}", serviceName, ex.ToString()));
                throw;
            }
        }

        public static ReturnType CallServicePost<PostType, ReturnType>(string serviceConnectionName, string serviceName, PostType postData, ref OAuthModel oauthRequest)
        {

            try
            {
                // serialize the data to be posted
                var jsonPostData = JsonConvert.SerializeObject(postData);
                var bytesPostData = Encoding.Default.GetBytes(jsonPostData);

                // get the URL to the Insurance Service
                var services = new ServiceConnections();
                var api = services[serviceConnectionName];
                //string insuranceServiceUrl = ConfigurationManager.AppSettings[baseUrlAppSetting];

                // perform a WebClient operation
                using (var client = new WebClient())
                {
                    // if we're using OAuth, get the OAuth bearer token and add it.
                    if (api.AuthType == ServiceAuthType.OAuth)
                    {
                        if (String.IsNullOrEmpty(oauthRequest.AccessToken))
                            oauthRequest = ObtainOAuthToken(api);

                        client.Headers.Add("Authorization", "Bearer " + oauthRequest.AccessToken);
                    }

                    // else, if we're using HeaderCredentials, add the credentials to the header
                    if (api.AuthType == ServiceAuthType.HeaderCredentials)
                    {
                        if (String.IsNullOrEmpty(api.UsernameField))
                            client.Headers.Add("username", api.Username ?? "");
                        else
                            client.Headers.Add(api.UsernameField, api.Username ?? "");


                        if (String.IsNullOrEmpty(api.PasswordField))
                            client.Headers.Add("password", api.Password ?? "");
                        else
                            client.Headers.Add(api.PasswordField, api.Password ?? "");
                    }

                    // set content type
                    client.Headers.Add("Content-Type", "application/json");

                    startTime = DateTime.Now;

                    // perform the request
                    var svcResponse = client.UploadData(api.Url + serviceName, "POST", bytesPostData);

                    endTime = DateTime.Now;

                    string jsonResponse = Encoding.ASCII.GetString(svcResponse);

                    //logger.Trace(serviceName, jsonPostData, jsonResponse, startTime, endTime);

                    // deserialize / return the data
                    return JsonConvert.DeserializeObject<ReturnType>(jsonResponse);
                }

            }
            catch (Exception ex)
            {
                logger.Fatal(string.Format("Service {0} Call with error message : {1}", serviceName, ex.ToString()));
                throw;
            }
        }
        #endregion

        #region string CallServiceBatch
        public static string CallServiceBatch(BatchActionList batchActionList)
        {
            try
            {
                // ********** original code follows
                // convert the batchActionList to JSON; it will be deserialized back into a BatchActionList type by the Insurance Service
                NameValueCollection inputs = new NameValueCollection();
                inputs.Add("batchActionList", JsonConvert.SerializeObject(batchActionList));

                // call the specified serviceName
                var client = new WebClient();
                client.Headers.Add("Authorization", "Bearer " + ObtainOAuthToken().AccessToken);

                startTime = DateTime.Now;

                var svcResponse = client.UploadValues(insuranceApi.Url + "batch", inputs);

                endTime = DateTime.Now;

                string jsonResponse = Encoding.ASCII.GetString(svcResponse);

                //logger.Trace(batchActionList, jsonResponse, startTime, endTime);

                // deserialize / return the data
                return jsonResponse;
            }
            catch (Exception ex)
            {
                // todo: add exception handling / error logging
                var msg = ex.Message;
                logger.Fatal(string.Format("Service {0} Call with error message : {1}", GetServiceName(batchActionList), ex.ToString()));
                throw;
            }
        }

        public static string CallServiceBatch(BatchActionList batchActionList, ref OAuthModel oauthRequest)
        {
            try
            {
                // ********** original code follows
                // convert the batchActionList to JSON; it will be deserialized back into a BatchActionList type by the Insurance Service
                NameValueCollection inputs = new NameValueCollection();
                inputs.Add("batchActionList", JsonConvert.SerializeObject(batchActionList));

                // call the specified serviceName
                if (String.IsNullOrEmpty(oauthRequest.AccessToken))
                    oauthRequest = ObtainOAuthToken(oauthRequest);
                var client = new WebClient();
                client.Headers.Add("Authorization", "Bearer " + oauthRequest.AccessToken);

                startTime = DateTime.Now;

                var svcResponse = client.UploadValues(insuranceApi.Url + "batch", inputs);

                endTime = DateTime.Now;

                string jsonResponse = Encoding.ASCII.GetString(svcResponse);

                //logger.Trace(batchActionList, jsonResponse, startTime, endTime);

                // deserialize / return the data
                return jsonResponse;
            }
            catch (Exception ex)
            {
                // todo: add exception handling / error logging
                var msg = ex.Message;
                logger.Fatal(string.Format("Service {0} Call with error message : {1}", GetServiceName(batchActionList), ex.ToString()));
                throw;
            }
        }
        #endregion

        #region OAuthToken Calls
        public static OAuthModel ObtainOAuthToken()
        {
            return GetAuthToken(insuranceApi);
        }

        public static OAuthModel ObtainOAuthToken(OAuthModel oauthRequest)
        {
            var services = new ServiceConnections();
            return ObtainOAuthToken(insuranceApi, oauthRequest);
        }

        public static OAuthModel ObtainOAuthToken(string serviceConnectionName)
        {
            var services = new ServiceConnections();
            return GetAuthToken(services[serviceConnectionName]);
        }

        public static OAuthModel ObtainOAuthToken(string serviceConnectionName, OAuthModel oauthRequest)
        {
            var services = new ServiceConnections();
            return ObtainOAuthToken(services[serviceConnectionName], oauthRequest);
        }

        public static OAuthModel ObtainOAuthToken(string serviceConnectionName, string username, string password)
        {
            var services = new ServiceConnections();
            var element = services[serviceConnectionName];
            var newElement = ServiceConnectionFrom(element);
            newElement.Username = username;
            newElement.Password = password;
            return GetAuthToken(newElement);
        }

        public static OAuthModel ObtainOAuthToken(ServiceConnectionElement element)
        {
            return GetAuthToken(element);
        }

        public static OAuthModel ObtainOAuthToken(string serviceConnectionName, string refreshToken)
        {
            var services = new ServiceConnections();
            var element = services[serviceConnectionName];
            return GetRefreshToken(element, refreshToken);
        }

        public static OAuthModel ObtainOAuthToken(ServiceConnectionElement element, OAuthModel oauthRequest)
        {
            DateTime dt;
            var isExpired = DateTime.TryParse(oauthRequest.Expires, out dt) ? dt <= DateTime.UtcNow : true;

            var newElement = ServiceConnectionFrom(element);
            newElement.Username = oauthRequest.Username ?? element.Username;
            newElement.Password = oauthRequest.Password ?? element.Password;
            newElement.ClientId = oauthRequest.ClientId ?? element.ClientId;
            newElement.Secret = oauthRequest.Secret ?? element.Secret;

            if (isExpired)
            {
                if (!String.IsNullOrEmpty(oauthRequest.RefreshToken) && !String.IsNullOrEmpty(oauthRequest.ClientId))
                    return GetRefreshToken(newElement, oauthRequest.RefreshToken);

                return GetAuthToken(newElement);
            }

            return oauthRequest ?? GetAuthToken(newElement);
        }

        private static OAuthModel GetAuthToken(ServiceConnectionElement element)
        {
            OAuthModel oauthResponse = new OAuthModel();

            try
            {
                var inputs = new NameValueCollection();
                inputs.Add("grant_type", "password");
                inputs.Add("client_id", element.ClientId ?? "");
                inputs.Add("secret", element.Secret ?? "");
                inputs.Add("username", element.Username);
                inputs.Add("password", element.Password);

                var client = new WebClient();
                var tokenResponse = client.UploadValues(element.AuthUrl, inputs);
                oauthResponse = JsonConvert.DeserializeObject<OAuthModel>(Encoding.UTF8.GetString(tokenResponse));
                oauthResponse.Status = "OK";
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                oauthResponse.Status = "Could not authenticate using the credentials submitted.";
            }

            return oauthResponse;
        }

        private static OAuthModel GetRefreshToken(ServiceConnectionElement element, string refreshToken)
        {
            OAuthModel oauthResponse = new OAuthModel();

            try
            {
                var inputs = new NameValueCollection();
                inputs.Add("grant_type", "refresh_token");
                inputs.Add("client_id", element.ClientId ?? "");
                inputs.Add("secret", element.Secret ?? "");
                inputs.Add("refresh_token", refreshToken);

                var client = new WebClient();
                var tokenResponse = client.UploadValues(element.AuthUrl, inputs);
                oauthResponse = JsonConvert.DeserializeObject<OAuthModel>(Encoding.UTF8.GetString(tokenResponse));
                oauthResponse.Status = "OK";
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                oauthResponse.Status = "Could not authenticate using the credentials submitted.";
            }

            return oauthResponse;
        }

        private static ServiceConnectionElement ServiceConnectionFrom(ServiceConnectionElement element)
        {
            return new ServiceConnectionElement()
            {
                AuthType = element.AuthType,
                AuthUrl = element.AuthUrl,
                ClientId = element.ClientId,
                Name = element.Name,
                Password = element.Password,
                PasswordField = element.PasswordField,
                Secret = element.Secret,
                Url = element.Url,
                Username = element.Username,
                UsernameField = element.UsernameField
            };
        }

        private static string GetServiceName(BatchActionList batchActionList)
        {
            StringBuilder sb = new StringBuilder();

            batchActionList.BatchActions.ForEach(res => sb.Append(string.Format("{0} !! ", res.ServiceName)));

            return sb.ToString().Substring(0, sb.ToString().Length - 3);
        }

        #endregion

    }
}