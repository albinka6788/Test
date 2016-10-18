using BHIC.APIService.App_Start;
using BHIC.APIService.Controllers;
using BHIC.Common;
using BHIC.Common.Config;
using BHIC.Contract.APIService;
using BHIC.Core;
using BHIC.Core.APIService;
using BHIC.DML.WC.DTO;
using BHIC.Domain.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace BHIC.APIService.Areas.UserService.Controllers
{
    public class ImpersonateController : BaseController
    {
        static DateTime startTime;
        IUserPolicyService service = new APIUserPolicy();
        IImpersonateService ImpService = new ImpersonateService();

        /// <summary>
        /// Attaches policies to a secondary/support user account for impersonation for a particular time period as per request during policy attachment. At a time, support team can only impersonate a user’s policies.
        /// <br/>AttachPolicy API also useful to revoke access timespan to a user account for policy/policies.  
        /// <br/>
        /// <br/><b>Validation:</b>
        /// <br/>1.	Email should be valid.
        /// <br/>Error Message for failure “Not a valid Email ID”
        /// <br/>2.	Provided EndTime must be greater than current datetime.
        /// <br/>Error Message for failure “EndTime must be greater than current time”
        /// <br/>3.	Provided EndTime must be greater than current StartTime.
        /// <br/>Error Message for failure “EndTime must be greater than StartTime”
        /// <br/>4.	No account exists in Policy Centre.
        /// <br/>Error Message for failure “No user exists with the specified email id”
        /// <br/>5.	Input policy code does not exist in the system
        /// <br/>Error Message for failure “No policy exists with the PolicyCode: {Policycode}”
        /// </summary>
        /// <param name="attachPolicyDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [BasicAuthentication]
        [Route("ImpnSVC/AttachPolicy")]
        public OperationStatus LinkUserToPolicy(AttachPolicyDTO attachPolicyDTO)
        {
            string EmailID = attachPolicyDTO.EmailID;
            List<string> PolicyCodes = attachPolicyDTO.PolicyNumbers;
            startTime = System.DateTime.Now;
            loggingService.Trace(string.Format("AttachPolicy request start at {0}", DateTime.Now));
            OperationStatus requestStatus = new OperationStatus();
            requestStatus.ServiceName = "AttachPolicy";
            requestStatus.ServiceMethod = "POST";
            requestStatus.RequestProcessed = true;
            requestStatus.RequestSuccessful = false;
            try
            {
                Message item = new Message();
                item.MessageType = MessageType.UserInfo;
                //validate email with regex
                if (UtilityFunctions.IsValidRegex(attachPolicyDTO.EmailID, Constants.EmailRegex))
                {
                    if (attachPolicyDTO.EndTime > DateTime.Now)
                    {
                        if (attachPolicyDTO.EndTime > attachPolicyDTO.StartTime)
                        {
                            int userId = service.GetUserDetailId(EmailID);
                            if (userId > 0)
                            {
                                bool AllPolicyExists = true;
                                foreach (string PolicyCode in PolicyCodes)
                                {
                                    if (service.IsPolicyExists(1, PolicyCode) == false)
                                    {
                                        AllPolicyExists = false;
                                        item.Text = string.Format("No policy exist with the PolicyCode : {0}", PolicyCode);
                                        break;
                                    }
                                }
                                if (AllPolicyExists)
                                {
                                    ImpService.AttachPolicy(EmailID, PolicyCodes, attachPolicyDTO.StartTime, attachPolicyDTO.EndTime);
                                    item.Text = string.Join(",", PolicyCodes) + " policies linked with user : " + EmailID;
                                    requestStatus.RequestSuccessful = true;
                                }
                            }
                            else
                                item.Text = string.Format("No user exist with the email id : {0}", EmailID);
                        }
                        else
                        {
                            item.Text = "EndTime must be greater than StartTime";
                        }
                    }
                    else
                    {
                        item.Text = "EndTime must be greater than current time";
                    }
                }
                else
                {
                    item.Text = string.Format("Not a valid Email ID = '{0}'", attachPolicyDTO.EmailID);
                }
                if (!requestStatus.RequestSuccessful)
                    requestStatus.Messages.Add(item);
            }
            catch (Exception ex)
            {
                Message item = new Message();
                item.MessageType = MessageType.UserInfo;
                item.Text = ex.Message;
                requestStatus.Messages.Add(item);
            }
            LogTrace("AttachPolicy", EmailID + "~" + string.Join(";", PolicyCodes), JsonConvert.SerializeObject(requestStatus).ToString(), startTime);
            return requestStatus;
        }

        /// <summary>
        /// Remove policies from the secondary/support user account for impersonation.
        /// <br/>
        /// <br/><b>Validation:</b>
        /// <br/>1.	Email should be valid.
        /// <br/>Error Message for failure “Not a valid Email ID”
        /// <br/>2.	No account exists in Policy Centre.
        /// <br/>Error Message for failure “No user exists with the specified email id”
        /// <br/>3.	Input policy code does not exist in the system
        /// <br/>Error Message for failure “No policy exists with the PolicyCode: {Policycode}”
        /// </summary>
        /// <param name="DetachPolicyDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [BasicAuthentication]
        [Route("ImpnSVC/DetachPolicy")]
        public OperationStatus DeLinkUserToPolicy(DetachPolicyDTO DetachPolicyDTO)
        {
            string EmailID = DetachPolicyDTO.EmailID;
            List<string> PolicyCodes = DetachPolicyDTO.PolicyNumbers;
            startTime = System.DateTime.Now;
            loggingService.Trace(string.Format("DetachPolicy request start at {0}", DateTime.Now));
            OperationStatus requestStatus = new OperationStatus();
            requestStatus.ServiceName = "DetachPolicy";
            requestStatus.ServiceMethod = "POST";
            requestStatus.RequestProcessed = true;
            requestStatus.RequestSuccessful = false;
            try
            {
                Message item = new Message();
                item.MessageType = MessageType.UserInfo;
                //validate email with regex
                if (UtilityFunctions.IsValidRegex(DetachPolicyDTO.EmailID, Constants.EmailRegex))
                {
                    int userId = service.GetUserDetailId(EmailID);
                    if (userId > 0)
                    {
                        bool AllPolicyExists = true;
                        foreach (string PolicyCode in PolicyCodes)
                        {
                            if (service.IsPolicyExists(1, PolicyCode) == false)
                            {
                                AllPolicyExists = false;
                                item.Text = string.Format("No policy exist with the PolicyCode : {0}", PolicyCode);
                                break;
                            }
                        }
                        if (AllPolicyExists)
                        {
                            ImpService.DetachPolicy(EmailID, PolicyCodes);
                            item.Text = string.Join(",", PolicyCodes) + " policies delinked with user : " + EmailID;
                            requestStatus.RequestSuccessful = true;
                        }
                    }
                    else
                        item.Text = string.Format("No user exist with the email id : {0}", EmailID);
                }
                else
                {
                    item.Text = string.Format("Not a valid Email ID = '{0}'", DetachPolicyDTO.EmailID);
                }
                if (!requestStatus.RequestSuccessful)
                    requestStatus.Messages.Add(item);
            }
            catch (Exception ex)
            {
                Message item = new Message();
                item.MessageType = MessageType.UserInfo;
                item.Text = ex.Message;
                requestStatus.Messages.Add(item);
            }
            LogTrace("DetachPolicy", EmailID + "~" + string.Join(";", PolicyCodes), JsonConvert.SerializeObject(requestStatus).ToString(), startTime);
            return requestStatus;
        }
    }
}