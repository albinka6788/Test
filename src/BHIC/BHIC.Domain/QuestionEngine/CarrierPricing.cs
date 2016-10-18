using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.QuestionEngine
{
    /// <summary>
    /// Class is used to store the properties for Pricing that are used in the algorithm
    /// to determine the PricingMatrix for a quote.
    /// These properties come from the BHDSchedModEligibility and BHDCarrierThresholds tables
    /// </summary>
    public class CarrierPricing
    {
        public CarrierPricing() { }

        [Key]
        public string state { get; set; }
        
        /// <summary>
        /// BHDSchedModEligibility.manualpremium
        /// </summary>
        public decimal manualPremium { get; set; }
        
        /// <summary>
        /// BHDCarrierThresholds.minpayroll
        /// </summary>
        public decimal minpayroll { get; set; }
        
        /// <summary>
        /// BHDCarrierThresholds.moveToCarrier
        /// </summary>
        public string moveToCarrier { get; set; }
        
        /// <summary>
        /// BHDCarrierThresholds.carrierThreshold
        /// </summary>
        public int carrierThreshold { get; set; }
        
        /// <summary>
        /// BHDCarrierThresholds.declineThreshold
        /// </summary>
        public int declineThreshold { get; set; }


    } //end CarrierPricing
} //end namespace
