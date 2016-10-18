using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Common.LobStatusService
{
   public class LobStatus
    {
       public string StateName { get; set; }
       public string Abbreviation { get; set; }
       public string BOP { get; set; }
       public string WC { get; set; }
       public string CA { get; set; }
    }
}
