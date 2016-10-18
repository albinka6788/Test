using BHIC.Domain.TransactionTrace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Contract.TransactionTrace
{
    public interface ITransactionLogService
    {
        long AddTransactionLog(TransactionLog transactionlog);
    }
}
