using BHIC.Common;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;
using BHIC.Domain;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using BHIC.Common.XmlHelper;
using BHIC.Contract.APIService;
using BHIC.Core;
using BHIC.Domain.Service;
using System.Security;
namespace BHIC.APIService.App_Start
{
    /// <summary>
    /// To validate the input token string
    /// Logic
    /// 1. Extract the token string
    /// 2. Decrypt the token string with password as client ip address. If decrypt success
    /// 3. Split the token string with delimeter '~'
    /// 4. If the splitted token string length is 4
    /// 5. Extract the user name and password and validate it. if it is success
    /// 6. Check for expiry, if currenttime-tokengenerated time less than configured minutes
    /// 7. then token string is valid
    /// 8. In case of any failure in the above steps, do the request cancel.
    /// </summary>
    public class BasicAuthentication : ActionFilterAttribute
    {

        public override void OnActionExecuting(HttpActionContext filterContext)
        {

            HttpRequestContext request = filterContext.RequestContext;
            HttpRequestHeaders headers = filterContext.Request.Headers;
            IUserPolicyService service = new APIUserPolicy();

            int exipreMinutes = ConfigCommonKeyReader.APIAuthorizeExpiryMinutes;
            if (headers.Contains("Authorization") && headers.GetValues("Authorization") != null && headers.GetValues("Authorization").ToString() != "")
            {
                IEnumerable<string> headerValues = headers.GetValues("Authorization");

                string token = headerValues.FirstOrDefault();
                //string ipAddress = HttpContext.Current.Request.UserHostAddress;   //Old line
                string ipAddress = UtilityFunctions.GetUserIPAddress(HttpContext.Current.ApplicationInstance.Context); 
                string decryptTokenKey = "";
                try
                {
                    decryptTokenKey = Encryption.DecryptText(token, ipAddress);
                }
                catch // Commented by guru as it throwing warning for unwanted variable declaration (Exception ex)
                {
                    CancelRequest(filterContext);
                }


                string[] splTokenKey = decryptTokenKey.Split('~');
                if (splTokenKey.Length == 4)
                {
                    string userName = splTokenKey[0].ToString();
                    string password = splTokenKey[1].ToString();
                    string receivedIPAddress = splTokenKey[2].ToString();
                    string receivedTimeStr = splTokenKey[3].ToString();
                    DateTime receivedTime = Convert.ToDateTime(receivedTimeStr);
                    DateTime curentTime = System.DateTime.Now;
                    TimeSpan span = curentTime.Subtract(receivedTime);
                    int spanMinutes = span.Minutes;
                    SecureString securePassword = UtilityFunctions.ConvertToSecureString(password);

                    if (service.IsValidAPICredentials(userName, securePassword))
                    {
                        if (spanMinutes > exipreMinutes)
                        {
                            CancelRequest(filterContext, "You are not authorized to access this api.");

                        }

                    }
                    else
                    {
                        CancelRequest(filterContext);
                    }


                }
                else
                {
                    CancelRequest(filterContext);

                }

            }
            else
            {
                CancelRequest(filterContext);
            }


        }

        /// <summary>
        /// Cancels the Athorization and adds the custom tenforce header to the response to
        /// inform the caller that his call has been denied.
        /// </summary>
        /// <param name="authContext">The authorizationContxt that needs to be canceled.</param>        
        /// <param name="message"></param>
        private static void CancelRequest(HttpActionContext authContext, string message = "You are not authorized to access this api.")
        {
            OperationStatus requestStatus = new OperationStatus();
            requestStatus.ServiceName = authContext.ControllerContext.ControllerDescriptor.ControllerName;
            requestStatus.ServiceMethod = authContext.ControllerContext.Request.Method.Method;
            Message item = new Message();
            item.MessageType = MessageType.UserError;
            item.Text = message;
            requestStatus.Messages.Add(item);
            authContext.Response = authContext.Request.CreateResponse(requestStatus);// .CreateErrorResponse(System.Net.HttpStatusCode.Forbidden, message);
        }

    }
}