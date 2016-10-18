#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


#endregion

namespace BHIC.Domain.Background
{
    public class WCDeductibles
    {
        public string DBASE { get; set; }
        public decimal DeductAmt { get; set; }
        public string ClassCode { get; set; }
        public string Vals { get; set; }
        public string Names { get; set; }
    }
}
