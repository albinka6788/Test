using BHIC.Domain.TransactionTrace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.DML.WC.DataContract
{
    public interface ITransactionLogDataService
    {
        long AddTransactionLog(TransactionLog transactionlog);
        
    }
}
