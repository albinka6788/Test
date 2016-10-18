using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Background
{
    /// <summary>
    /// Parameters associated with the PaymentPlans service<br />
    /// Filters the response as indicated by the comments for each parameter
    /// </summary>
    public class PaymentPlanRequestParms
    {

        /// <summary>
        /// Used by the Insurance Service to retrieve Quote-specific data that will be used to identify and return relevant payment plans.<br />
        /// Required.
        /// </summary>
        [Required]
        public int? QuoteId { get; set; }

        /// <summary>
        /// Line of Business - plans will be returned for only the LOB specified (WC, BP, CA...)
        /// Validation:<br />
        /// 1) Required.<br />
        /// </summary>
        [Required]
        public string LobAbbr { get; set; }

        /// <summary>
        /// Return data for the specified state
        /// Validation:<br />
        /// 1) Required.<br />
        /// </summary>
        public string StateAbbr { get; set; }

        /// <summary>
        /// Return data for the specified premium
        /// Validation:<br />
        /// 1) Required.<br />
        /// </summary>
        public decimal? Premium { get; set; }

        /// <summary>
        /// Return data for a specific PaymentPlan.  Optional.
        /// </summary>
        public int? PaymentPlanId { get; set; }
    }
}
