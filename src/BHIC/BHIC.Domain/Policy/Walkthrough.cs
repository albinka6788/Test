using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    public class WalkThrough
    {
        /// <summary>
        /// The walkthrough code
        /// </summary>
        /// This is the temporary MGA Code
        public string WalkThroughCode { get; set; }

        /// <summary>
        /// The quote ID
        /// </summary>
        public int QuoteId { get; set; }

        /// <summary>
        /// The URL to be used to proxy all ajax requests
        /// </summary>
        public string AjaxProxyUrl { get; set; }

        /// <summary>
        /// The URL to be used to proxy all ajax requests
        /// </summary>
        public string DebugUrl { get; set; }

        /// <summary>
        /// The encoded URL needed to access the walkthrough
        /// </summary>
        public string EncodedUrl { get; set; }

        /// <summary>
        /// The current step in the walkthrough
        /// </summary>
        public string StepId { get; set; }

        /// <summary>
        /// The HTML/JSON request to be sent for the BOP WalkThrough to the back end
        /// </summary>
        public string Request { get; set; }

        /// <summary>
        /// The HTML/JSON results of the BOP WalkThrough screens to be used on the front end
        /// </summary>
        public string Response { get; set; }

        /// <summary>
        /// The response type of the BOP WalkThrough screens to be used on the front end
        /// </summary>
        public string ResponseType { get; set; }

        /// <summary>
        /// The status of the current walkthrough instance.
        /// </summary>
        public WalkThroughStatus WalkThroughStatus { get; set; }

        /// <summary>
        /// Contains the ZipCode which started the WalkThrough
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        /// The state resolved from the ZipCode which started the WalkThrough
        /// </summary>
        public string State { get; set; }

    }
}
