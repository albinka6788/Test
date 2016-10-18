using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Policy
{
    public class PolicyDetailsResponse
    {
        // ----------------------------------------
        // constructor
        // ----------------------------------------

        public PolicyDetailsResponse()
        {
            // init objects to help avoid issues related to null reference exceptions
            PolicyDetails = new PolicyDetails();
            OperationStatus = new OperationStatus();
            PolicyDetailsList = new List<PolicyDetails>();
        }

        // ----------------------------------------
        // properties
        // ----------------------------------------

        /// <summary>
        /// If PolicyCode is specified in the request, this property will be populated, if the related policy exists.<br />
        /// </summary>
        public PolicyDetails PolicyDetails { get; set; }

        /// <summary>
        /// If PolicyCodeList is specified in the request, this property will be populated, for each specified policy that exists.<br />
        /// </summary>
        public List<PolicyDetails> PolicyDetailsList { get; set; }

        public OperationStatus OperationStatus { get; set; }
    }
}