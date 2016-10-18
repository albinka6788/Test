using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Common.TransactionLog.Common
{
    /// <summary>
    /// To store the reattempt information
    /// </summary>
    public class ReattemptInfo
    {
        public DateTime CreatedDateTime { get; set; }
        public string ServiceName { get; set; }
        public string MethodName { get; set; }
        public int RetryCount { get; set; }
        public List<string> Log { get; set; }
        public object[] InputParameters { get; set; }
        public string ClassName { get; set; }
        public List<string> InputParametersTypeName { get; set; }

    }

    
}
