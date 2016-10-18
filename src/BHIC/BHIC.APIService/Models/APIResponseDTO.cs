using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.APIService.Models
{
    public class APIResponse
    {
        public ResponseStatus Status { get; set; }
        public string Message { get; set; }
    }
    public class APIAuthorizationResponse
    {
        public string Authorize { get; set; }
    }

    public enum ResponseStatus
    {
        Success=1,
        Failure=2
    }
}
