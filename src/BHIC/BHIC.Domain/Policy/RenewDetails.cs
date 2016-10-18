using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    public class RenewDetails
    {
        public string RenewOfToUse { get; set; }
        public string RenewOfToShow { get; set; }
        public string RenewByToUse { get; set; }
        public string RenewByToShow { get; set; }

        public RenewDetails() { }

        public RenewDetails(string renewoftouse, string renewoftoshow, string renewbytouse, string renewbytoshow)
        {
            RenewOfToUse = renewoftouse;
            RenewOfToShow = renewoftoshow;
            RenewByToUse = renewbytouse;
            RenewByToShow = renewbytoshow;
        }
    }
}
