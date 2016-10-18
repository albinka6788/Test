#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace BHIC.Domain.Background
{
    public class WCDeductiblesRequestParms
    {
        public string Carrier { get; set; }

        public string State { get; set; }

        public string EffDate { get { return (DateTime.Now.ToShortDateString()); } }
    }
}
