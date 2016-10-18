using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.XMod
{
    public class RiskXmodFactorResponse
    {
        // ----------------------------------------
        // constructor
        // ----------------------------------------

        public RiskXmodFactorResponse()
        {
            // init lists to help avoid issues related to null reference exceptions
            RiskHeaderInformation = new RiskHeaderInformation();
            RatingValuesInformation = new List<RatingValuesInformation>();
        }

        public RiskHeaderInformation RiskHeaderInformation { get; set; }
        public List<RatingValuesInformation> RatingValuesInformation { get; set; }
    }
}
