using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.TransactionTrace
{
    public class TransactionLog
    {
            public TransactionLog()
            {
                ApiTransactions = new List<ApiTransaction>();
                Dbtransactions = new List<DbTransaction>();
            }
            public long TransactionLogId { get; set; } //(bigint, not null)
            public string UserIP { get; set; } //(varchar(20), null)
            public string RequestUrl { get; set; } //(varchar(300), null)
            public string RequestType { get; set; } //(varchar(20), null)
            public DateTime RequestDateTime { get; set; } //(datetime, null)
            public DateTime ResponseDateTime { get; set; } //(datetime, null)
            public long RequestProcessTime { get; set; } //(bigint, null)
            public long ? ResponseSize { get; set; } //(bigint, null)
            public int  ? TotalAPICalls { get; set; } //(int, null)
            public long ? TotalAPIProcessTime { get; set; } //(bigint, null)
            public int  ? TotalDBCalls { get; set; } //(int, null)
            public long ? TotalDBProcessTime { get; set; } //(bigint, null)
            public string PaymentErrorDetail { get; set; } //(varchar(max), null)
            public long ? QuoteId { get; set; }
            public string UserId { get; set; }
            public string ThreadId { get; set; }
            public string Browser { get; set; }
            public string BrowserVersion { get; set; }
            public string Lob { get; set; }
            public long AdId { get; set; }
            public List<ApiTransaction> ApiTransactions { get; set; }
            public List<DbTransaction> Dbtransactions { get; set; }
    }
}
