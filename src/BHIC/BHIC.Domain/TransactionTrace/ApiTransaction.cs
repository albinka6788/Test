using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.TransactionTrace
{
    public class ApiTransaction
    {
            public long ApiTransactionId { get; set; } //(bigint, not null)
            public long TransactionLogId { get; set; } //(bigint, not null)
            public string ApiCallType { get; set; } //(varchar(20), null)
            public string ApiName { get; set; } //(varchar(100), null)
            public DateTime ApiCallRequestTime { get; set; } //(datetime, null)
            public DateTime ? ApiCallResponseTime { get; set; } //(datetime, null)
            public long ? ApiRequestProcessTime { get; set; } //(bigint, null)
            public long ? ApiRequestSize { get; set; } //(bigint, null)
            public long ? ApiResponseSize { get; set; } //(bigint, null)
    }
}
