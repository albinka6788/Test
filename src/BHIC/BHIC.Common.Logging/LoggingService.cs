using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;

using BHIC.Common.XmlHelper;
using BHIC.Domain.Service;

using Newtonsoft.Json;
using NLog;
using System.Linq;

namespace BHIC.Common.Logging
{
    //test commit
    public sealed class LoggingService : ILoggingService
    {
        #region Comment : Here Class property declaration and instantiation following "Singleton Design Pattern"

        //Comment : Here Instance marked as READONLY, which will force compiler to assign it only during static initialization
        private static readonly LoggingService instanceReadOnly = new LoggingService();

        //Comment : Here Volatile ensures that assignment to the instance variable COMPLETES before the instance variable can be ACCESSED
        private static volatile LoggingService instance;

        //Comment : Here Approach will uses a syncRoot instance to lock on, rather than locking on the type itself, to avoid deadlocks. 
        private static object syncMultiThread = new Object();

        private LoggingService() { }

        public static LoggingService Instance
        {
            get
            {
                //Comment : Here Multithreaded Singleton Object Instantiation
                if (instance == null)
                {
                    lock (syncMultiThread)
                    {
                        if (instance == null)
                            instance = new LoggingService();
                    }
                }

                return instance;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        #region Traces Methods

        public void Trace(string message, [CallerFilePath] string callerPath = "",
            [CallerMemberName] string callerMember = "", [CallerLineNumber] int callerLine = 0)
        {
            Log(LogLevel.Trace, message, null, callerPath, callerMember, callerLine);
        }

        public void Trace(string message, Exception exception, [CallerFilePath] string callerPath = "",
            [CallerMemberName] string callerMember = "", [CallerLineNumber] int callerLine = 0)
        {
            Log(LogLevel.Fatal, message, exception, callerPath, callerMember, callerLine);
        }

        public void Trace(Exception exception, [CallerFilePath] string callerPath = "",
            [CallerMemberName] string callerMember = "", [CallerLineNumber] int callerLine = 0)
        {
            Log(LogLevel.Fatal, string.Empty, exception, callerPath, callerMember, callerLine);
        }

        #endregion

        #region Debug Methods

        public void Debug(string message, [CallerFilePath] string callerPath = "",
            [CallerMemberName] string callerMember = "", [CallerLineNumber] int callerLine = 0)
        {
            Log(LogLevel.Debug, message, null, callerPath, callerMember, callerLine);
        }

        public void Debug(string message, Exception exception, [CallerFilePath] string callerPath = "",
            [CallerMemberName] string callerMember = "", [CallerLineNumber] int callerLine = 0)
        {
            Log(LogLevel.Debug, message, exception, callerPath, callerMember, callerLine);
        }

        public void Debug(Exception exception, [CallerFilePath] string callerPath = "",
            [CallerMemberName] string callerMember = "", [CallerLineNumber] int callerLine = 0)
        {
            Log(LogLevel.Debug, string.Empty, exception, callerPath, callerMember, callerLine);
        }

        #endregion

        #region Info Methods

        public void Info(string message, [CallerFilePath] string callerPath = "",
            [CallerMemberName] string callerMember = "", [CallerLineNumber] int callerLine = 0)
        {
            Log(LogLevel.Info, message, null, callerPath, callerMember, callerLine);
        }

        public void Info(string message, Exception exception, [CallerFilePath] string callerPath = "",
            [CallerMemberName] string callerMember = "", [CallerLineNumber] int callerLine = 0)
        {
            Log(LogLevel.Info, message, exception, callerPath, callerMember, callerLine);
        }

        public void Info(Exception exception, [CallerFilePath] string callerPath = "",
            [CallerMemberName] string callerMember = "", [CallerLineNumber] int callerLine = 0)
        {
            Log(LogLevel.Info, string.Empty, exception, callerPath, callerMember, callerLine);
        }

        #endregion

        #region Warning Methods

        public void Warn(string message, [CallerFilePath] string callerPath = "",
            [CallerMemberName] string callerMember = "", [CallerLineNumber] int callerLine = 0)
        {
            Log(LogLevel.Warn, message, null, callerPath, callerMember, callerLine);
        }

        public void Warn(string message, Exception exception, [CallerFilePath] string callerPath = "",
            [CallerMemberName] string callerMember = "", [CallerLineNumber] int callerLine = 0)
        {
            Log(LogLevel.Warn, message, exception, callerPath, callerMember, callerLine);
        }

        public void Warn(Exception exception, [CallerFilePath] string callerPath = "",
            [CallerMemberName] string callerMember = "", [CallerLineNumber] int callerLine = 0)
        {
            Log(LogLevel.Warn, string.Empty, exception, callerPath, callerMember, callerLine);
        }

        #endregion

        #region Error Methods

        public void Error(string message, [CallerFilePath] string callerPath = "",
            [CallerMemberName] string callerMember = "", [CallerLineNumber] int callerLine = 0)
        {
            Log(LogLevel.Error, message, null, callerPath, callerMember, callerLine);
        }

        public void Error(string message, Exception exception, [CallerFilePath] string callerPath = "",
            [CallerMemberName] string callerMember = "", [CallerLineNumber] int callerLine = 0)
        {
            Log(LogLevel.Error, message, exception, callerPath, callerMember, callerLine);
        }

        public void Error(Exception exception, [CallerFilePath] string callerPath = "",
            [CallerMemberName] string callerMember = "", [CallerLineNumber] int callerLine = 0)
        {
            Log(LogLevel.Error, string.Empty, exception, callerPath, callerMember, callerLine);
        }

        #endregion

        #region Fatal Methods

        public void Fatal(string message, [CallerFilePath] string callerPath = "",
            [CallerMemberName] string callerMember = "", [CallerLineNumber] int callerLine = 0)
        {
            Log(LogLevel.Fatal, message, null, callerPath, callerMember, callerLine);
        }

        public void Fatal(string message, Exception exception, [CallerFilePath] string callerPath = "",
            [CallerMemberName] string callerMember = "", [CallerLineNumber] int callerLine = 0)
        {
            Log(LogLevel.Fatal, message, exception, callerPath, callerMember, callerLine);
        }

        public void Fatal(Exception exception, [CallerFilePath] string callerPath = "",
            [CallerMemberName] string callerMember = "", [CallerLineNumber] int callerLine = 0)
        {
            Log(LogLevel.Fatal, string.Empty, exception, callerPath, callerMember, callerLine);
        }

        #endregion

        #endregion

        #region Private Methods

        private static void Log(LogLevel level, string message, Exception exception = null, string callerPath = "", string callerMember = "", int callerLine = 0)
        {
            var logger = LogManager.GetLogger(callerPath);
            LogManager.ThrowExceptions = ConfigCommonKeyReader.EnableNLogTracing;

            if (!logger.IsEnabled(level)) return;

            string completeMessage = string.Empty;

            if (level == LogLevel.Error || level == LogLevel.Fatal)
            {
                completeMessage = ((HttpContext.Current != null && HttpContext.Current.Request != null) ? 
                        string.Format("Request {0} received from IP Address : {1}", HttpContext.Current.Request.Url.ToString(),
                    GetUserIPAddress(HttpContext.Current.ApplicationInstance.Context)) : "HTTP Context not found");

                if (!string.IsNullOrEmpty(message))
                {
                    completeMessage = string.Concat(completeMessage, Environment.NewLine, Environment.NewLine,
                        "Actual Error message detail Start", Environment.NewLine,
                        "=================================", Environment.NewLine,
                        message, Environment.NewLine,
                        "=================================", Environment.NewLine,
                        "Actual Error message detail End");
                }

                //if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session["APILog"] != null)
                //{
                //    completeMessage = string.Concat((completeMessage ?? string.Empty), Environment.NewLine, Environment.NewLine,
                //        "API Called detail Start", Environment.NewLine,
                //        "======================", Environment.NewLine,
                //        HttpContext.Current.Session["APILog"], Environment.NewLine,
                //        "======================", Environment.NewLine, "API Called detail End");
                //}
            }
            else
            {
                completeMessage = message;
            }

            if (completeMessage.IndexOf("Executed API Call for Quote Number = '", StringComparison.OrdinalIgnoreCase) == -1 && 
                HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session["CustomSession"] != null &&
                ((BHIC.ViewDomain.CustomSession)HttpContext.Current.Session["CustomSession"]).QuoteID > 0)
            {
                completeMessage = string.Format("{0}{1}Log recorded for Quote Number = '{2}'",
                    completeMessage, Environment.NewLine, ((BHIC.ViewDomain.CustomSession)HttpContext.Current.Session["CustomSession"]).QuoteID);
            }

            var logEvent = new LogEventInfo(level, callerPath, completeMessage) { Exception = exception };
            logEvent.Properties.Add("callerpath", callerPath);
            logEvent.Properties.Add("callermember", callerMember);
            logEvent.Properties.Add("callerline", callerLine);
            logger.Log(logEvent);
        }

        /// <summary>
        /// Get user IP address from current http request context
        /// </summary>
        /// <param name="currentRequest"></param>
        /// <returns></returns>
        private static string GetUserIPAddress(HttpContext currentContext)
        {
            string VisitorsIPAddress = string.Empty;
            HttpRequest currentRequest = currentContext.Request;

            if (currentRequest.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                VisitorsIPAddress = currentRequest.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Last();
            }
            else if (currentRequest.ServerVariables["REMOTE_ADDR"] != null)
            {
                VisitorsIPAddress = currentRequest.ServerVariables["REMOTE_ADDR"].ToString();
            }
            else if (currentRequest.UserHostAddress.Length != 0)
            {
                VisitorsIPAddress = currentRequest.UserHostAddress;
            }

            return VisitorsIPAddress;
        }

        #endregion

        #endregion

    }
}
