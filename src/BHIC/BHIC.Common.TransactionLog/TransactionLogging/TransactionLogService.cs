using BHIC.Common.XmlHelper;
using BHIC.Domain.TransactionTrace;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace BHIC.Common.TransactionLog.TransactionLogging
{
    public static class TransactionLogCustomSessions
    {
        #region Public Methods
        /*For Api log information for Transaction logging*/
        public static void CustomSessionForApiRequestResponse(ApiTransaction apitransaction)
        {
            if (HttpContext.Current.Session != null)
            {
                try
                {
                    if (HttpContext.Current.Session["TransactionLogging"] != null)
                    {
                        var transactionlog = (TransactionLog)HttpContext.Current.Session["TransactionLogging"];
                        transactionlog.ApiTransactions.Add(apitransaction);
                        HttpContext.Current.Session["TransactionLogging"] = transactionlog;
                    }

                }
                catch
                {
                    throw;
                }
            }
        }
        /*For Database Request logging for Transaction logging*/
        public static void CustomSessionForDbRequest(string dbProcName)
        {
            if (HttpContext.Current.Session != null)
            {
                if (HttpContext.Current.Session["dbtransaction"] != null)
                {
                    HttpContext.Current.Session["dbtransaction"] = null;
                }
                HttpContext.Current.Session["dbtransaction"] = new DbTransaction { DbCallRequestTime = DateTime.Now, DbProcName = dbProcName };
            }

        }
        /*For Database Respone logging for Transaction logging*/
        public static void CustomSessionForDbResponse()
        {
            if (HttpContext.Current.Session != null)
            {
                try
                {
                    if (HttpContext.Current.Session["TransactionLogging"] != null && HttpContext.Current.Session["dbtransaction"] != null)
                    {
                        var transactionlog = (TransactionLog)HttpContext.Current.Session["TransactionLogging"];
                        var dbtransaction = (DbTransaction)HttpContext.Current.Session["dbtransaction"];
                        dbtransaction.DbCallResponseTime = DateTime.Now;
                        TimeSpan timespan = dbtransaction.DbCallResponseTime.Value - dbtransaction.DbCallRequestTime;
                        dbtransaction.DbRequestProcessTime = (int)timespan.TotalMilliseconds;
                        transactionlog.Dbtransactions.Add(dbtransaction);
                        HttpContext.Current.Session["TransactionLogging"] = transactionlog;
                        HttpContext.Current.Session["dbtransaction"] = null;
                    }
                }
                catch
                {
                    throw;
                }
            }
        }
        /*For Payment failure for Transaction logging*/
        public static void CustomSessionForPaymentError(string error)
        {
            if (HttpContext.Current.Session != null)
            {
                try
                {
                    if (HttpContext.Current.Session["TransactionLogging"] != null)
                    {
                        var transactionlog = (TransactionLog)HttpContext.Current.Session["TransactionLogging"];
                        transactionlog.PaymentErrorDetail = error;
                        HttpContext.Current.Session["TransactionLogging"] = transactionlog;
                    }
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion
    }
}
