#region Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace BHIC.DML.WC.DTO
{
    public class CustomSession : BaseClass
    {
        public int QuoteID { get; set; }
        public bool UpdateOnly { get; set; }
        public string SessionData { get; set; }
    }
}
