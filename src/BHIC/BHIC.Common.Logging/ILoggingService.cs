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

        #endregion

        #endregion
    }
}
