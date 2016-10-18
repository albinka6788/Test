using BHIC.Common.XmlHelper;
using BHIC.Domain.TransactionTrace;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace BHIC.Common.Reattempt
{
    public static class TransactionLogCustomSessions
    {
        #region Public Methods
        /*For Api log information for Transaction logging*/
        public static void CustomSessionForApiRequestResponse(ApiTransaction apitransaction)
        {
            try
            {
                var transactionlog = GetTransactionSession();
                if (transactionlog != null)
                {
                    transactionlog.ApiTransactions.Add(apitransaction);
                    SetTransactionSession(transactionlog);
                }
            }
            catch (Exception ex)
            {
                Logging.LoggingService.Instance.Trace("Error Ocurred in API Transaction Logging :" + ex.Message);
                throw;
            }


        }
        /*For Database Request logging for Transaction logging*/
        public static void CustomSessionForDbRequest(string dbProcName)
        {
            try
            {
                if (HttpContext.Current.Session != null)
                {
                    if (HttpContext.Current.Session["dbtransaction"] != null)
                        HttpContext.Current.Session["dbtransaction"] = null;

                    HttpContext.Current.Session["dbtransaction"] = new DbTransaction { DbCallRequestTime = DateTime.Now, DbProcName = dbProcName };
                }
            }
            catch (Exception ex)
            {
                Logging.LoggingService.Instance.Trace("Error Ocurred in Database Request Transaction Logging :" + ex.Message);
                throw;
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
                        var transactionlog = GetTransactionSession();
                        var dbtransaction = (DbTransaction)HttpContext.Current.Session["dbtransaction"];
                        dbtransaction.DbCallResponseTime = DateTime.Now;
                        TimeSpan timespan = dbtransaction.DbCallResponseTime.Value - dbtransaction.DbCallRequestTime;
                        dbtransaction.DbRequestProcessTime = (int)timespan.TotalMilliseconds;
                        transactionlog.Dbtransactions.Add(dbtransaction);
                        SetTransactionSession(transactionlog);
                        HttpContext.Current.Session["dbtransaction"] = null;
                    }
                }
                catch(Exception ex)
                {
                    Logging.LoggingService.Instance.Trace("Error Ocurred in Database Response Transaction Logging :" + ex.Message);
                    throw;
                }
            }
        }
        /*For Payment failure for Transaction logging*/
        public static void CustomSessionForPaymentError(string error)
        {
            try
            {
                var transactionlog = GetTransactionSession();
                if (transactionlog != null)
                {
                    transactionlog.PaymentErrorDetail = error;
                    SetTransactionSession(transactionlog);
                }
            }
            catch(Exception ex)
            {
                Logging.LoggingService.Instance.Trace("Error Ocurred in Payment Error Capturing Transaction Logging :" + ex.Message);
                throw;
            }
        }

        /*For AdId to capture in Transaction logging*/
        public static void CustomSessionForAdId(long AdId)
        {
            try
            {
                var transactionlog = GetTransactionSession();
                if (transactionlog != null)
                {
                    transactionlog.AdId = AdId;
                    SetTransactionSession(transactionlog);
                }
            }
            catch (Exception ex)
            {
                Logging.LoggingService.Instance.Trace("Error Ocurred in Adid Transaction Logging :" + ex.Message);
                throw;
            }


        }
        #endregion

        #region Private
        private static TransactionLog GetTransactionSession()
        {
            if (HttpContext.Current.Session != null)
            {
                try
                {
                    if (HttpContext.Current.Session["TransactionLogging"] != null)
                        return (TransactionLog)HttpContext.Current.Session["TransactionLogging"];
                }
                catch
                {
                    throw;
                }

            }
            return null;
        }

        private static void SetTransactionSession(TransactionLog transactionLog)
        {
            HttpContext.Current.Session["TransactionLogging"] = transactionLog;
        }
        #endregion
    }
}
