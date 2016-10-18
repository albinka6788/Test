using BHIC.APIService.Controllers;
using BHIC.Common;
using BHIC.Common.Logging;
using BHIC.Common.XmlHelper;
using BHIC.Contract.APIService;
using BHIC.Core;
using BHIC.DML.WC.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Web;
using System.Web.Http;

namespace BHIC.APIService.Areas.UserService.Controllers
{
    /// <summary>
    /// To Generate Authorization
    /// </summary>
    public class AuthController : ApiController
    {
        private ILoggingService loggingService = LoggingService.Instance;
        static DateTime startTime;
        static DateTime endTime;
        
        /// <summary>
        /// Generate Authorization 
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        /// Logic
        ///   1. First validate the usernmae and password
        ///   2. If validation successfull, concatenate all the parameters(username, password, client ip address, currenttime)
        ///   3. Encrypt the string with password as (client ip address)
        ///   4. return the string as Token
        [Route("UsrSVC/Auth")]
        public OAuthDTO GetKey(string UserName, string Password)
        {
            startTime = System.DateTime.Now;
            endTime = System.DateTime.Now;
            OAuthDTO authResponse = new OAuthDTO();

            try
            {
                IUserPolicyService service = new APIUserPolicy();

                //string ipAddress = HttpContext.Current.Request.UserHostAddress;   //Old line
                string ipAddress = UtilityFunctions.GetUserIPAddress(HttpContext.Current.ApplicationInstance.Context); 
                SecureString encryptedPassword = UtilityFunctions.ConvertToSecureString(Password); // Encryption.EncryptText(Password, UserName);
                
                if (service.IsValidAPICredentials(UserName,encryptedPassword))
                {
                    int exipreMinutes = ConfigCommonKeyReader.APIAuthorizeExpiryMinutes;
                    DateTime currentTime = System.DateTime.Now;
                    DateTime expireTime = currentTime.AddMinutes(exipreMinutes);
                    string token = FormTokenString(UserName, Password, ipAddress);
                    authResponse.AccessToken = token;
                    authResponse.ExpiresIn = (exipreMinutes * 60)-1;
                    authResponse.Expires = expireTime.ToString();
                    authResponse.Issued = currentTime.ToString();
                    authResponse.Status = "Success";
                    authResponse.Username = UserName;
                    endTime = System.DateTime.Now;
                    loggingService.Trace(UtilityFunctions.GetCompleteMessage("APIGetKey", UserName + "~" + Password, JsonConvert.SerializeObject(authResponse), startTime, endTime));
                    return authResponse;
                }
                else
                {
                    authResponse.AccessToken = null;
                    authResponse.ExpiresIn = 0;
                    authResponse.Expires = null;
                    authResponse.Issued = null;
                    authResponse.Status = "Failure";
                    authResponse.Username = UserName;

                    endTime = System.DateTime.Now;
                    loggingService.Trace(UtilityFunctions.GetCompleteMessage("APIGetKey", UserName + "~" + Password, JsonConvert.SerializeObject(authResponse), startTime, endTime));
                    
                    return authResponse;
                }
                

            }
            catch (Exception ex)
            {
                loggingService.Fatal(string.Format("Service '{0}' Call Failed with below error message :{1}{2}", "APIGetKey", Environment.NewLine, ex.ToString()));
                var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                throw new HttpResponseException(response);
            }

        }

        /// <summary>
        /// Generate token string
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <param name="IPAddress"></param>
        /// <returns></returns>

        private string FormTokenString(string UserName, string Password, string IPAddress)
        {

            string tokenInput = UserName + "~" + Password + "~" + IPAddress + "~" + System.DateTime.Now.ToString();
            return Encryption.EncryptText(tokenInput, IPAddress);

        }



    }
}
