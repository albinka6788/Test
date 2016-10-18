using BHIC.Common.Logging;
using BHIC.Common.Quote;
using BHIC.Common.XmlHelper;
using BHIC.Contract.TransactionTrace;
using BHIC.Core.TransactionTrace;
using BHIC.Domain.Dashboard;
using BHIC.Domain.TransactionTrace;
using System;
using System.Web;
using System.Web.Mvc;

namespace BHIC.Common.Trace
{
    public class TransactionLogAttribute : ActionFilterAttribute
    {
        private readonly ILoggingService _logger = LoggingService.Instance;

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            /*For Transaction logging Commit*/
            if (!ConfigCommonKeyReader.IsTransactionLog) return;

            //DateTime start = DateTime.Now;
            
            try
            {

                if (filterContext.Exception == null)
                {
                    if (HttpContext.Current.Session["TransactionLogging"] != null)
                    {
                        if (HttpContext.Current.Session["ThreadId"] == null)
                        {
                            HttpContext.Current.Session["ThreadId"] = Guid.NewGuid().ToString("N");
                        }


                        var transactionlog = (TransactionLog)HttpContext.Current.Session["TransactionLogging"];
                        transactionlog.ResponseDateTime = DateTime.Now;
                        transactionlog.RequestProcessTime = (int)(transactionlog.ResponseDateTime - transactionlog.RequestDateTime).TotalMilliseconds;
                        transactionlog.QuoteId = QuoteCookieHelper.Cookie_GetQuoteId(filterContext.RequestContext.HttpContext);

                        var userRegistration = HttpContext.Current.Session["user"] == null ? null : HttpContext.Current.Session["user"].GetType().Name == "UserRegistration"?(UserRegistration)HttpContext.Current.Session["user"]:null;
                        if (userRegistration != null)
                        {
                            transactionlog.UserId = userRegistration.Email;
                        }
                        transactionlog.ThreadId = HttpContext.Current.Session["ThreadId"].ToString();
                        ITransactionLogService transactionlogservice = new TransactionLogService();
                        transactionlogservice.AddTransactionLog(transactionlog);
                    }

                }
                else
                {
                    _logger.Error(filterContext.Exception);
                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            finally
            {
                HttpContext.Current.Session["TransactionLogging"] = null;
            }
            //Logging.LoggingService.Instance.Trace(string.Format("Time taken by Transaction filter action executed {0} ms", (DateTime.Now - start).TotalMilliseconds));
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            /*For Transaction logging initialize*/
            if (!ConfigCommonKeyReader.IsTransactionLog) return;

            //DateTime start = DateTime.Now;

            try
            {
                var customSession = (ViewDomain.CustomSession)HttpContext.Current.Session["CustomSession"];
                var request = filterContext.HttpContext.Request;
                var transactionLog = new TransactionLog
                {
                    RequestDateTime = DateTime.Now,
                    //UserIP = request.UserHostAddress, //Old line
                    UserIP = UtilityFunctions.GetUserIPAddress(filterContext.HttpContext.ApplicationInstance.Context),
                    RequestType = request.RequestType,
                    RequestUrl = request.Url.AbsoluteUri,
                    Browser = request.Browser.Browser,
                    BrowserVersion = request.Browser.Version,
                    Lob = customSession == null ? "" : customSession.LobId ?? ""
                };
                if (HttpContext.Current.Session != null && HttpContext.Current.Session["TransactionLogging"] != null)
                {
                    HttpContext.Current.Session["TransactionLogging"] = null;
                }
                HttpContext.Current.Session["TransactionLogging"] = transactionLog;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }

            //Logging.LoggingService.Instance.Trace(string.Format("Time taken by Transaction filter action executing {0} ms", (DateTime.Now - start).TotalMilliseconds));

        }
    }
}
