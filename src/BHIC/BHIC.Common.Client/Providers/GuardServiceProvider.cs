using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;

using BHIC.Common.Caching;
using BHIC.Common.Config;
using BHIC.Common.XmlHelper;
using Newtonsoft.Json;

namespace BHIC.Common.Client
{
    public class GuardServiceProvider : ServiceProvider
    {
        #region Comment : Here variables/properties declaration

        //Comment : Here set default config section and default API for this Vendor
        //Return the InsuranceSystemUrl value from the web.config
        private static ServiceConnectionElement DefaultGuardApi
        {
            get
            {
                var services = new ServiceConnections();
                return services[ServiceProviderConstants.DefaultGuardApiWC];
            }
        }

        #endregion

        #region Comment : Here class constructor

        //Constructor
        public GuardServiceProvider() { }

        #endregion

        #region Comment : Here class public methods

        private ServiceConnections GetServices(string serviceConnectionName = ServiceProviderConstants.DefaultServiceConnectionNameGuard)
        {
            return new ServiceConnections(serviceConnectionName);
        }

        /// <summary>
        ///  Get service/api url to communicate 
        /// </summary>
        /// <returns></returns>
        public override string ServiceUrl()
        {
            string apiUrl = null;

            //Comment : here based on service category like "WC, BOP, others"
            switch (base.ServiceCategory)
            {
                case ServiceProviderConstants.GuardServiceCategoryWC:
                    apiUrl = DefaultGuardApi.Url;
                    break;

                case ServiceProviderConstants.GuardServiceCategoryBOP:
                    var services = this.GetServices();
                    apiUrl = services[ServiceProviderConstants.GuardApiBOP].Url;
                    break;
            }

            return string.IsNullOrEmpty(apiUrl) == true ? "" : apiUrl;
        }

        /// <summary>
        /// Method to add custom header based on serice method type and requirement
        /// </summary>
        /// <param name="serviceMethod"></param>
        /// <param name="client"></param>
        public override void SetWebClientHeaders(OperationType serviceMethod, ref WebClient client)
        {
            AddWebClientHeaders(serviceMethod, ref client);
        }

        /// <summary>
        /// Method to add custom header based on serice method type and requirement
        /// </summary>
        /// <param name="serviceMethod"></param>
        /// <param name="client"></param>
        /// <param name="reAttemptNumber"></param>
        public override void SetWebClientHeaders(OperationType serviceMethod, ref WebClient client, int reAttemptNumber)
            {
            AddWebClientHeaders(serviceMethod, ref client, reAttemptNumber);
        }

        /// <summary>
        /// Method will set web client headers for "Batch POST" based on provider and provider service category
        /// </summary>
        /// <param name="serviceMethod"></param>
        /// <param name="client"></param>
        public override void SetBatchWebClientHeaders(OperationType serviceMethod, ref WebClient client)
                    {
            AddWebClientHeaders(serviceMethod, ref client, isBatchRequest: true);
        }

        /// <summary>
        /// Method will set web client headers for "Batch POST" based on provider and provider service category
        /// </summary>
        /// <param name="serviceMethod"></param>
        /// <param name="client"></param>
        /// <param name="reAttemptNumber"></param>
        public override void SetBatchWebClientHeaders(OperationType serviceMethod, ref WebClient client, int reAttemptNumber)
                        {
            AddWebClientHeaders(serviceMethod, ref client, reAttemptNumber, true);
        }

        /// <summary>
        /// Method to prepare header based on serice method type and requirement
        /// </summary>
        /// <param name="serviceMethod"></param>
        /// <param name="client"></param>
        public override void SetBatchNameValueCollection(BHIC.Domain.Service.BatchActionList batchActionList, ref System.Collections.Specialized.NameValueCollection inputs)
        {
            //Comment : here based on service method like "GET/POST" set headers
            inputs.Add("batchActionList", JsonConvert.SerializeObject(batchActionList));
                                }

                        #endregion

        #region Comment : Here class private methods

        /// <summary>
        /// Setting web client header with authentication token.
        /// </summary>
        /// <param name="serviceMethod"></param>
        /// <param name="client"></param>
        /// <param name="reAttemptNumber"></param>
        /// <param name="isBatchRequest"></param>
        private void AddWebClientHeaders(OperationType serviceMethod, ref WebClient client, int reAttemptNumber = 0, bool isBatchRequest = false)
        {
            //Comment : here based on service method like "GET/POST" set headers
            switch (serviceMethod)
            {
                case OperationType.GET:
                case OperationType.POST:
                    {
                        #region Comment : Here GET method headers based on LOB

                        //Comment : here based on service category like "WC, BOP, others"
                        switch (this.ServiceCategory)
                        {
                            case ServiceProviderConstants.GuardServiceCategoryWC:
                            case ServiceProviderConstants.GuardServiceCategoryBOP:
                                {
                                    #region Comment : Here GET method headers in case of "WC"

                                    var api = DefaultGuardApi;

                                    this.SetCommonClientHeaders(api, ref client, reAttemptNumber);

                                    if (!isBatchRequest)
                                    {
                                        // set content type
                                        client.Headers.Add("Content-Type", "application/json");
                                    }

                                    #endregion
                                }
                                break;
                        }

                        #endregion
                    }
                    break;
            }
        }

        /// <summary>
        /// To set web-client call common headers
        /// </summary>
        private void SetCommonClientHeaders(ServiceConnectionElement api, ref WebClient client, int reAttemptNumber = 0)
        {
            // if we're using OAuth, get the OAuth bearer token and add it.
            if (api.AuthType == ServiceAuthType.OAuth)
            {
                OAuthModel oAuthRequest = ObtainOAuthToken(api, GetTokenFromCache(), reAttemptNumber);
                TraceTokenDetail(oAuthRequest);
                client.Headers.Add("Authorization", "Bearer " + oAuthRequest.AccessToken);
                SetTokenFromCache(oAuthRequest);
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
        }

        #endregion

        #region Comment : Here Get OAuthToken Calls

        public static OAuthModel ObtainOAuthToken()
        {
            return GetAuthToken(DefaultGuardApi);
        }

        public static OAuthModel ObtainOAuthToken(OAuthModel oauthRequest)
        {
            var services = new ServiceConnections();
            return ObtainOAuthToken(DefaultGuardApi, oauthRequest);
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

        public static OAuthModel ObtainOAuthToken(ServiceConnectionElement element, OAuthModel oauthRequest, int reAttemptNumber = 0)
        {
            if (oauthRequest != null)
            {
                DateTime dt;
                //var newElement = GetServiceConnection(element, oauthRequest);
                if ((DateTime.TryParse(oauthRequest.Expires, out dt) ? dt.ToUniversalTime() <= DateTime.UtcNow : true))
                {
                    Logging.LoggingService.Instance.Trace("Expired token");
                    TraceTokenDetail(oauthRequest);

                    if (!string.IsNullOrEmpty(oauthRequest.RefreshToken) && !string.IsNullOrEmpty(oauthRequest.ClientId))
                    {
                        Logging.LoggingService.Instance.Trace("Refresh Token");
                        return GetRefreshToken(element, oauthRequest.RefreshToken);
                    }
                    return GetAuthToken(element);
                }
                else
                {
                    if (reAttemptNumber == 1 && !string.IsNullOrEmpty(oauthRequest.RefreshToken) && !string.IsNullOrEmpty(oauthRequest.ClientId))
                    {
                        Logging.LoggingService.Instance.Trace("Refresh Token");
                        return GetRefreshToken(element, oauthRequest.RefreshToken);
                    }
                    else if (reAttemptNumber > 0)
                    {
                        return GetAuthToken(element);
                    }
                }
            }
            else
            {
                return GetAuthToken(element);
            }
            return oauthRequest;
        }

        private static OAuthModel GetAuthToken(ServiceConnectionElement element)
        {
            OAuthModel oauthResponse = new OAuthModel();

            try
            {
                Logging.LoggingService.Instance.Trace("New Token");
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

        private static ServiceConnectionElement GetServiceConnection(ServiceConnectionElement element, OAuthModel oauthRequest)
        {
            var newElement = ServiceConnectionFrom(element);
            newElement.Username = oauthRequest.Username ?? element.Username;
            newElement.Password = oauthRequest.Password ?? element.Password;
            newElement.ClientId = oauthRequest.ClientId ?? element.ClientId;
            newElement.Secret = oauthRequest.Secret ?? element.Secret;
            return newElement;
        }

        private static OAuthModel GetTokenFromCache()
        {
            return CacheHelper.Instance.Get<OAuthModel>(Constants.OAuthModelToken);
        }

        private static void SetTokenFromCache(OAuthModel val)
        {
            if (CacheHelper.Instance.IsExists(Constants.OAuthModelToken))
            {
                CacheHelper.Instance.Remove(Constants.OAuthModelToken);
            }
            DateTime dt;
            DateTime.TryParse(val.Expires, out dt);
            if (dt != DateTime.MinValue)
            {
                double cacheDuration = (dt.ToUniversalTime() - DateTime.UtcNow).TotalSeconds;
                if ((cacheDuration - ConfigCommonKeyReader.ReduceAPICacheDurationInSeconds) > 0)
                {
                    cacheDuration -= ConfigCommonKeyReader.ReduceAPICacheDurationInSeconds;
                    CacheHelper.Instance.Add<OAuthModel>(Constants.OAuthModelToken, val, cacheDuration);
                }
            }
        }

        /// <summary>
        /// Print authentication token detail
        /// </summary>
        /// <param name="oauthResponse"></param>
        private static void TraceTokenDetail(OAuthModel oauthResponse)
        {
            DateTime dt;
            DateTime.TryParse(oauthResponse.Expires, out dt);
            Logging.LoggingService.Instance.Trace(
                string.Format("token exp time : {0}, expires In time : {1}, Current UTC Time {2} and expires in as per current UTC time : {3} ms, have refresh token : {4}, client id : {5}", 
                oauthResponse.Expires, oauthResponse.ExpiresIn, DateTime.UtcNow, (dt.ToUniversalTime() - DateTime.UtcNow).TotalMilliseconds,
                (oauthResponse.RefreshToken ?? "No refresh token"), (oauthResponse.ClientId ?? "No Client")));
        }
        #endregion
    }
}
