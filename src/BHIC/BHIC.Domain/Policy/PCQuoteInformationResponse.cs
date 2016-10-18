using BHIC.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    public class PCQuoteInformationResponse
    {
        // ----------------------------------------
        // constructor
        // ----------------------------------------

        public PCQuoteInformationResponse()
        {
            // init objects to help avoid issues related to null reference exceptions
            PCQuoteInformation = new PCQuoteInformation();
            OperationStatus = new OperationStatus();
            PCQuoteInformationList = new List<PCQuoteInformation>();
        }

        // ----------------------------------------
        // properties
        // ----------------------------------------

        /// <summary>
        /// Only populated if a single QuoteId or MgaCode was specified by populating one of the following two request properties: PCQuoteInformationRequestParms.QuoteId, or PCQuoteInformationRequestParms.MgaCode
        /// </summary>
        public PCQuoteInformation PCQuoteInformation { get; set; }

        /// <summary>
        /// Only populated if a comma-delimited list of QuoteIds was specified by populating the following request property: PCQuoteInformationRequestParms.QuoteIdList
        /// </summary>
        public List<PCQuoteInformation> PCQuoteInformationList { get; set; }

        public OperationStatus OperationStatus { get; set; }
    }
}
