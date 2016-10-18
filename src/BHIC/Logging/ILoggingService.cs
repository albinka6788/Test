using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using BHIC.Domain.Service;
using System.Diagnostics;

namespace BHIC.Common.Logging
{
    public interface ILoggingService
    {
        #region Public Methods

        #region Traces Methods

        void Trace(string message, [CallerFilePathAttribute] string callerPath = "",
                    [CallerMemberName] string callerMember = "",
                    [CallerLineNumber] int callerLine = 0);
        void Trace(Exception exception, [CallerFilePathAttribute] string callerPath = "",
                   [CallerMemberName] string callerMember = "",
                   [CallerLineNumber] int callerLine = 0);
        void Trace(string message, Exception exception, [CallerFilePathAttribute] string callerPath = "",
                   [CallerMemberName] string callerMember = "",
                   [CallerLineNumber] int callerLine = 0);

        void Trace<T>(string serviceName, List<T> requestList, string response, DateTime startTime, DateTime endTime);

        void Trace<T>(string serviceName, T request, string response, DateTime startTime, DateTime endTime);

        void Trace(string serviceName, string request, string response, DateTime startTime, DateTime endTime);

        void Trace(BatchActionList batchRequest, string response, DateTime startTime, DateTime endTime);

        void Trace(string serviceName, string request, string response, DateTime startTime, DateTime endTime, StackFrame stackFrame);

        #endregion

        #region Debug Methods

        void Debug(string message, [CallerFilePathAttribute] string callerPath = "",
                    [CallerMemberName] string callerMember = "",
                    [CallerLineNumber] int callerLine = 0);
        void Debug(Exception exception, [CallerFilePathAttribute] string callerPath = "",
                   [CallerMemberName] string callerMember = "",
                   [CallerLineNumber] int callerLine = 0);
        void Debug(string message, Exception exception, [CallerFilePathAttribute] string callerPath = "",
                   [CallerMemberName] string callerMember = "",
                   [CallerLineNumber] int callerLine = 0);

        void Debug<T>(string serviceName, List<T> requestList, string response, DateTime startTime, DateTime endTime);

        void Debug<T>(string serviceName, T request, string response, DateTime startTime, DateTime endTime);

        void Debug(string serviceName, string request, string response, DateTime startTime, DateTime endTime);

        void Debug(BatchActionList batchRequest, string response, DateTime startTime, DateTime endTime);

        void Debug(string serviceName, string request, string response, DateTime startTime, DateTime endTime, StackFrame stackFrame);

        #endregion

        #region Info Methods

        void Info(string message, [CallerFilePathAttribute] string callerPath = "",
                    [CallerMemberName] string callerMember = "",
                    [CallerLineNumber] int callerLine = 0);
        void Info(Exception exception, [CallerFilePathAttribute] string callerPath = "",
                   [CallerMemberName] string callerMember = "",
                   [CallerLineNumber] int callerLine = 0);
        void Info(string message, Exception exception, [CallerFilePathAttribute] string callerPath = "",
                   [CallerMemberName] string callerMember = "",
                   [CallerLineNumber] int callerLine = 0);

        void Info<T>(string serviceName, List<T> requestList, string response, DateTime startTime, DateTime endTime);

        void Info<T>(string serviceName, T request, string response, DateTime startTime, DateTime endTime);

        void Info(string serviceName, string request, string response, DateTime startTime, DateTime endTime);

        void Info(BatchActionList batchRequest, string response, DateTime startTime, DateTime endTime);

        void Info(string serviceName, string request, string response, DateTime startTime, DateTime endTime, StackFrame stackFrame);

        #endregion

        #region Warn Methods

        void Warn(string message, [CallerFilePathAttribute] string callerPath = "",
                    [CallerMemberName] string callerMember = "",
                    [CallerLineNumber] int callerLine = 0);
        void Warn(Exception exception, [CallerFilePathAttribute] string callerPath = "",
                   [CallerMemberName] string callerMember = "",
                   [CallerLineNumber] int callerLine = 0);
        void Warn(string message, Exception exception, [CallerFilePathAttribute] string callerPath = "",
                   [CallerMemberName] string callerMember = "",
                   [CallerLineNumber] int callerLine = 0);

        void Warn<T>(string serviceName, List<T> requestList, string response, DateTime startTime, DateTime endTime);

        void Warn<T>(string serviceName, T request, string response, DateTime startTime, DateTime endTime);

        void Warn(string serviceName, string request, string response, DateTime startTime, DateTime endTime);

        void Warn(BatchActionList batchRequest, string response, DateTime startTime, DateTime endTime);

        void Warn(string serviceName, string request, string response, DateTime startTime, DateTime endTime, StackFrame stackFrame);

        #endregion

        #region Error Methods

        void Error(string message, [CallerFilePathAttribute] string callerPath = "",
                   [CallerMemberName] string callerMember = "",
                   [CallerLineNumber] int callerLine = 0);
        void Error(Exception exception, [CallerFilePathAttribute] string callerPath = "",
                   [CallerMemberName] string callerMember = "",
                   [CallerLineNumber] int callerLine = 0);
        void Error(string message, Exception exception, [CallerFilePathAttribute] string callerPath = "",
                   [CallerMemberName] string callerMember = "",
                   [CallerLineNumber] int callerLine = 0);

        void Error<T>(string serviceName, List<T> requestList, string response, DateTime startTime, DateTime endTime);

        void Error<T>(string serviceName, T request, string response, DateTime startTime, DateTime endTime);

        void Error(string serviceName, string request, string response, DateTime startTime, DateTime endTime);

        void Error(BatchActionList batchRequest, string response, DateTime startTime, DateTime endTime);

        void Error(string serviceName, string request, string response, DateTime startTime, DateTime endTime, StackFrame stackFrame);

        #endregion

        #region Fatal Methods

        void Fatal(string message, [CallerFilePathAttribute] string callerPath = "",
                   [CallerMemberName] string callerMember = "",
                   [CallerLineNumber] int callerLine = 0);
        void Fatal(Exception exception, [CallerFilePathAttribute] string callerPath = "",
                   [CallerMemberName] string callerMember = "",
                   [CallerLineNumber] int callerLine = 0);

        void Fatal(string message, Exception exception, [CallerFilePathAttribute] string callerPath = "",
                   [CallerMemberName] string callerMember = "",
                   [CallerLineNumber] int callerLine = 0);

        void Fatal<T>(string serviceName, List<T> requestList, string response, DateTime startTime, DateTime endTime);

        void Fatal<T>(string serviceName, T request, string response, DateTime startTime, DateTime endTime);

        void Fatal(string serviceName, string request, string response, DateTime startTime, DateTime endTime);

        void Fatal(BatchActionList batchRequest, string response, DateTime startTime, DateTime endTime);

        void Fatal(string serviceName, string request, string response, DateTime startTime, DateTime endTime, StackFrame stackFrame);

        #endregion

        #endregion
    }
}
