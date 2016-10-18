using BHIC.Common.Logging;
using BHIC.Common.Reattempt;
using BHIC.Contract.TransactionTrace;
using BHIC.DML.WC.DataContract;
using BHIC.DML.WC.DataService;
using BHIC.Domain.TransactionTrace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BHIC.Core.TransactionTrace
{
    public class TransactionLogService :ITransactionLogService
    {

        public long AddTransactionLog(TransactionLog transactionlog)
        {
            if (HttpContext.Current != null && HttpContext.Current.Session["TransactionLogging"] != null)
            {
                ReattemptLog.Register(System.Reflection.MethodBase.GetCurrentMethod(), transactionlog);
               return 0;
            }
            ITransactionLogDataService transactionlogdataservice = new TransactionLogDataService();
            return transactionlogdataservice.AddTransactionLog(transactionlog);
           
        }
    }
}
