using BHIC.Common;
using BHIC.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace BHIC.APIService.Controllers
{
    public class BaseController : ApiController
    {
        internal ILoggingService loggingService = LoggingService.Instance;
        
        #region Constructors

        public BaseController() { 

        }
        [ApiExplorerSettings(IgnoreApi = true)]
        public void LogTrace(string APIName, string input, string response,DateTime startTime)
        {
            DateTime endTime = System.DateTime.Now;
            loggingService.Trace(UtilityFunctions.GetCompleteMessage(APIName, input,response, startTime, endTime));
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        public void LogFatal(string APIName, Exception ex, DateTime startTime)
        {
            DateTime endTime = System.DateTime.Now;
            loggingService.Fatal(string.Format("Service '{0}', started at '{1} - {2}' Call Failed with below error message :{3}{4}", APIName,startTime.ToString(),endTime.ToString(), Environment.NewLine, ex.ToString()));
        }

        #endregion
    }
}
