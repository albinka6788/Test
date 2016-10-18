#region Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace BHIC.Domain.XMod
{
    public class XModRequestParms
    {
        //Required parameters
        public string RiskId { get; set; }
        public string Fein { get; set; }

        public string UserId { get; set; }
        public string Password { get; set; }
        public string SiteNumber { get; set; }

        //Optional parameters
        public string ModType { get; set; }
        public string Test { get; set; }
        public string Format { get; set; }
        public string RatingEffectiveStartDate { get; set; }
        public string RatingEffectiveEndDate { get; set; }
    }
}
