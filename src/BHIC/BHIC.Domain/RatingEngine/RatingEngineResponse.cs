#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace BHIC.Domain.RatingEngine
{
    public class RatingEngineResponse
    {
        /// <summary>
        /// Per installment charge. 
        /// </summary>
        public decimal PerInstallmentCharge { get; set; }

        /// <summary>
        /// Annual premium amount associated with the policy. 
        /// </summary>
        public decimal Premium { get; set; }

        /// <summary>
        /// Abbreviation for the Governing State for the policy. 
        /// </summary>
        public string GovStateAbbr { get; set; }

        /// <summary>
        /// Success / Failure indicator
        /// </summary>
        public bool RequestSuccessful { get; set; }

        /// <summary>
        /// Messages returned by the rating engine
        /// </summary>
        public string Messages { get; set; }
    }
}