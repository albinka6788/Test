using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

using Newtonsoft.Json;
using NLog;
using BHIC.Domain.Service;
using System.Globalization;
using System.Diagnostics;
using System.Net.Mail;
using BHIC.Common.Mailing;

namespace BHIC.Common.Logging
{
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

        public void Trace<T>(string serviceName, List<T> requestList, string response, DateTime startTime, DateTime endTime)
        {
            Log(LogLevel.Trace, GetCompleteMessageWithList(serviceName, requestList, response, startTime, endTime));
        }

        public void Trace<T>(string serviceName, T request, string response, DateTime startTime, DateTime endTime)
        {
            Log(LogLevel.Trace, GetCompleteMessageWithSingleItem(serviceName, request, response, startTime, endTime));
        }

        public void Trace(string serviceName, string request, string response, DateTime startTime, DateTime endTime)
        {
            Log(LogLevel.Trace, GetCompleteMessage(serviceName, request, response, startTime, endTime));
            //try
            //{
            //    MailHelper mail = new MailHelper();
            //    mail.SendMailMessage("anuj_singh2@yahoo.com", new List<string> { "prem.pratap@xceedance.com" }, "Logger Mail Sending", "Test mail from NLog logger mail sending implementation");
            //    //SmtpClient mailServer = new SmtpClient("smtp.yahoo.com", 587);
            //    //mailServer.EnableSsl = true;

            //    //mailServer.Credentials = new System.Net.NetworkCredential("anuj_singh2@yahoo.com", "abc_2015");

            //    //string from = "anuj_singh2@yahoo.com";
            //    //string to = "prem.pratap@xceedance.com";
            //    //MailMessage msg = new MailMessage(from, to);
            //    //msg.Subject = "Logger Mail Sending";
            //    //msg.Body = "Test mail from NLog logger mail sending implementation";
            //    //mailServer.Send(msg);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("Unable to send email. Error : " + ex);
            //}
        }

        public void Trace(BatchActionList batchRequest, string response, DateTime startTime, DateTime endTime)
        {
            Log(LogLevel.Trace, GetCompleteBatchMessage(batchRequest, response, startTime, endTime));
        }

        public void Trace(string serviceName, string request, string response, DateTime startTime, DateTime endTime, StackFrame stackFrame)
        {
            Log(LogLevel.Trace, GetCompleteMessage(serviceName, request, response, startTime, endTime),null,stackFrame.GetFileName(),string.Empty,stackFrame.GetFileLineNumber());
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

        public void Debug<T>(string serviceName, List<T> requestList, string response, DateTime startTime, DateTime endTime)
        {
            Log(LogLevel.Debug, GetCompleteMessageWithList(serviceName, requestList, response, startTime, endTime));
        }

        public void Debug<T>(string serviceName, T request, string response, DateTime startTime, DateTime endTime)
        {
            Log(LogLevel.Debug, GetCompleteMessageWithSingleItem(serviceName, request, response, startTime, endTime));
        }

        public void Debug(string serviceName, string request, string response, DateTime startTime, DateTime endTime)
        {
            Log(LogLevel.Debug, GetCompleteMessage(serviceName, request, response, startTime, endTime));
        }

        public void Debug(BatchActionList batchRequest, string response, DateTime startTime, DateTime endTime)
        {
            Log(LogLevel.Debug, GetCompleteBatchMessage(batchRequest, response, startTime, endTime));
        }

        public void Debug(string serviceName, string request, string response, DateTime startTime, DateTime endTime, StackFrame stackFrame)
        {
            Log(LogLevel.Debug, GetCompleteMessage(serviceName, request, response, startTime, endTime),null,stackFrame.GetFileName(),string.Empty,stackFrame.GetFileLineNumber());
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

        public void Info<T>(string serviceName, List<T> requestList, string response, DateTime startTime, DateTime endTime)
        {
            Log(LogLevel.Info, GetCompleteMessageWithList(serviceName, requestList, response, startTime, endTime));
        }

        public void Info<T>(string serviceName, T request, string response, DateTime startTime, DateTime endTime)
        {
            Log(LogLevel.Info, GetCompleteMessageWithSingleItem(serviceName, request, response, startTime, endTime));
        }

        public void Info(string serviceName, string request, string response, DateTime startTime, DateTime endTime)
        {
            Log(LogLevel.Info, GetCompleteMessage(serviceName, request, response, startTime, endTime));
        }

        public void Info(BatchActionList batchRequest, string response, DateTime startTime, DateTime endTime)
        {
            Log(LogLevel.Info, GetCompleteBatchMessage(batchRequest, response, startTime, endTime));
        }

        public void Info(string serviceName, string request, string response, DateTime startTime, DateTime endTime, StackFrame stackFrame)
        {
            Log(LogLevel.Info, GetCompleteMessage(serviceName, request, response, startTime, endTime), null, stackFrame.GetFileName(), string.Empty, stackFrame.GetFileLineNumber());
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

        public void Warn<T>(string serviceName, List<T> requestList, string response, DateTime startTime, DateTime endTime)
        {
            Log(LogLevel.Warn, GetCompleteMessageWithList(serviceName, requestList, response, startTime, endTime));
        }

        public void Warn<T>(string serviceName, T request, string response, DateTime startTime, DateTime endTime)
        {
            Log(LogLevel.Warn, GetCompleteMessageWithSingleItem(serviceName, request, response, startTime, endTime));
        }

        public void Warn(string serviceName, string request, string response, DateTime startTime, DateTime endTime)
        {
            Log(LogLevel.Warn, GetCompleteMessage(serviceName, request, response, startTime, endTime));
        }

        public void Warn(BatchActionList batchRequest, string response, DateTime startTime, DateTime endTime)
        {
            Log(LogLevel.Warn, GetCompleteBatchMessage(batchRequest, response, startTime, endTime));
        }

        public void Warn(string serviceName, string request, string response, DateTime startTime, DateTime endTime, StackFrame stackFrame)
        {
            Log(LogLevel.Warn, GetCompleteMessage(serviceName, request, response, startTime, endTime), null, stackFrame.GetFileName(), string.Empty, stackFrame.GetFileLineNumber());
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

        public void Error<T>(string serviceName, List<T> requestList, string response, DateTime startTime, DateTime endTime)
        {
            Log(LogLevel.Error, GetCompleteMessageWithList(serviceName, requestList, response, startTime, endTime));
        }

        public void Error<T>(string serviceName, T request, string response, DateTime startTime, DateTime endTime)
        {
            Log(LogLevel.Error, GetCompleteMessageWithSingleItem(serviceName, request, response, startTime, endTime));
        }

        public void Error(string serviceName, string request, string response, DateTime startTime, DateTime endTime)
        {
            Log(LogLevel.Error, GetCompleteMessage(serviceName, request, response, startTime, endTime));
        }

        public void Error(BatchActionList batchRequest, string response, DateTime startTime, DateTime endTime)
        {
            Log(LogLevel.Error, GetCompleteBatchMessage(batchRequest, response, startTime, endTime));
        }

        public void Error(string serviceName, string request, string response, DateTime startTime, DateTime endTime, StackFrame stackFrame)
        {
            Log(LogLevel.Error, GetCompleteMessage(serviceName, request, response, startTime, endTime), null, stackFrame.GetFileName(), string.Empty, stackFrame.GetFileLineNumber());
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

        public void Fatal<T>(string serviceName, List<T> requestList, string response, DateTime startTime, DateTime endTime)
        {
            Log(LogLevel.Fatal, GetCompleteMessageWithList(serviceName, requestList, response, startTime, endTime));
        }

        public void Fatal<T>(string serviceName, T request, string response, DateTime startTime, DateTime endTime)
        {
            Log(LogLevel.Fatal, GetCompleteMessageWithSingleItem(serviceName, request, response, startTime, endTime));
        }

        public void Fatal(string serviceName, string request, string response, DateTime startTime, DateTime endTime)
        {
            Log(LogLevel.Fatal, GetCompleteMessage(serviceName, request, response, startTime, endTime));
        }

        public void Fatal(BatchActionList batchRequest, string response, DateTime startTime, DateTime endTime)
        {
            Log(LogLevel.Fatal, GetCompleteBatchMessage(batchRequest, response, startTime, endTime));
        }

        public void Fatal(string serviceName, string request, string response, DateTime startTime, DateTime endTime, StackFrame stackFrame)
        {
            Log(LogLevel.Fatal, GetCompleteMessage(serviceName, request, response, startTime, endTime), null, stackFrame.GetFileName(), string.Empty, stackFrame.GetFileLineNumber());
        }

        #endregion

        #endregion

        #region Private Methods

        private static void Log(LogLevel level, string message, Exception exception = null, string callerPath = "", string callerMember = "", int callerLine = 0)
        {            
            var logger = LogManager.GetLogger(callerPath);

            if (!logger.IsEnabled(level)) return;

            var logEvent = new LogEventInfo(level, callerPath, message) { Exception = exception };
            logEvent.Properties.Add("callerpath", callerPath);
            logEvent.Properties.Add("callermember", callerMember);
            logEvent.Properties.Add("callerline", callerLine);
            logger.Log(logEvent);
        }

        /// <summary>
        /// Generate complete message from request list and response with time
        /// </summary>
        /// <param name="requestList"></param>
        /// <param name="response"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        private string GetCompleteMessageWithList<T>(string serviceName, List<T> requestList, string response, DateTime startTime, DateTime endTime)
        {
            StringBuilder sb = new StringBuilder();

            bool isFirstRequest = true;

            foreach (object obj in requestList)
            {
                sb.Append(string.Format((isFirstRequest ? "{0}": " !! {0}"), JsonConvert.SerializeObject(obj)));
                isFirstRequest = false;
            }

            return GetCompleteMessage(serviceName, sb.ToString(), response, startTime, endTime);
        }

        /// <summary>
        /// Generate complete message from single request and response with time
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        private string GetCompleteMessageWithSingleItem<T>(string serviceName, T request, string response, DateTime startTime, DateTime endTime)
        {
            return GetCompleteMessage(serviceName, JsonConvert.SerializeObject(request), response, startTime, endTime);
        }

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
        private string GetCompleteMessage(string serviceName, string request, string response, DateTime startTime, DateTime endTime, bool isBatchRequest = false)
        {
            StringBuilder sb = new StringBuilder();
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

            sb.Append(string.Concat("--- Service Request Log Start ---", Environment.NewLine, 
                (isBatchRequest ? "--- Batch Request ---" : "--- Service Name ---"), Environment.NewLine, serviceName,
                Environment.NewLine, Environment.NewLine, (string.IsNullOrEmpty(request) ? string.Empty : "--- Request Message ---"),
                (string.IsNullOrEmpty(request) ? string.Empty : Environment.NewLine), request,
                (string.IsNullOrEmpty(request) ? string.Empty : Environment.NewLine), (string.IsNullOrEmpty(request) ? string.Empty : Environment.NewLine), 
                "--- Response Received ---", Environment.NewLine, response,
                Environment.NewLine, Environment.NewLine, "Start Date & Time (EST) : ", TimeZoneInfo.ConvertTime(startTime, timeZoneInfo).ToString("F"),
                Environment.NewLine, "End Date & Time (EST) : ", TimeZoneInfo.ConvertTime(endTime, timeZoneInfo).ToString("F"),
                Environment.NewLine, "Total Time taken (ms) : ", (endTime - startTime).TotalMilliseconds,
                Environment.NewLine, "--- Service Request Log End ---", Environment.NewLine, Environment.NewLine));

            return sb.ToString();
        }

        /// <summary>
        /// Generate complete batch message and response with time
        /// </summary>
        /// <param name="batchActionList"></param>
        /// <param name="response"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        private string GetCompleteBatchMessage(BatchActionList batchActionList, string response, DateTime startTime, DateTime endTime)
        {
            return GetCompleteMessage(JsonConvert.SerializeObject(batchActionList), string.Empty, response, startTime, endTime, true);
        }

        #endregion

        #endregion

    }
}
