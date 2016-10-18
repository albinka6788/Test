using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.BackEnd
{
    /// <summary>
    /// WCLimit modifier values returned by WCLimits GET can be submitted to the Modifiers POST service.<br />
    /// NOTE: The system automatically uses default values (where DefValue = "Y") that are configured; it is therefore not neccesary to submit a defined default value.  Default values that are submitted will not be saved.)'
    /// </summary>
    public class WCLimit
    {
        /// <summary>
        /// State associated with the limit
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Identifying code associated with the limit
        /// </summary>
        [StringLength(4)]
        public string ClassCode { get; set; }

        /// <summary>
        /// Either Y or N.  If Y, the limit is the default value for the policy.
        /// </summary>
        [StringLength(1)]
        public string DefValue { get; set; }

        /// <summary>
        /// Bodily Injury by Accident - each employee  
        /// </summary>
        public decimal BIAE { get; set; }

        /// <summary>
        /// Bodly Injury by Disease - each employee  
        /// </summary>
        public decimal BIDE { get; set; }

        /// <summary>
        /// Bodily Injury by Disease - policy limit  
        /// </summary>
        public decimal BIDL { get; set; }
    }
}