using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.TransactionTrace
{
    public class DbTransaction
    {
            public long DbTransactionId { get; set; } //(bigint, not null)
            public long TransactionLogId { get; set; } //(bigint, null)
            public DateTime DbCallRequestTime { get; set; } //(datetime, null)
            public DateTime ? DbCallResponseTime { get; set; } //(datetime, null)
            public long ? DbRequestProcessTime { get; set; } //(bigint, null)
            public string DbProcName { get; set; }
    }
}
