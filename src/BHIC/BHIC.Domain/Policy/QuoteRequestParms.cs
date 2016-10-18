using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    /// <summary>
    /// Parameters associated with the Policy service<br />
    /// Filters the response as indicated by the comments for each parameter
    /// </summary>
    public class QuoteRequestParms
    {
        /// <summary>
        /// Return data for specified Quote<br />
        /// Validation:<br />
        /// 1) One of the following must be provided: QuoteId, QuoteIdList, or PolicyId.
        /// </summary>
        public int? QuoteId { get; set; }

        /// <summary>
        /// Return data for specified Quotes.<br />
        /// An arbitrary number of Quote IDs may be provided.<br />
        /// Example: 111,222,333,444
        /// Validation:<br />
        /// 1) One of the following must be provided: QuoteId, QuoteIdList, or PolicyId.
        /// </summary>
        public string QuoteIdList { get; set; }

        /// <summary>
        /// Return data for specified Policy<br />
        /// Validation:<br />
        /// 1) One of the following must be provided: QuoteId, QuoteIdList, or PolicyId.
        /// </summary>
        [StringLength(10)]
        public string PolicyId { get; set; }

        /// <summary>
        /// Return data for specified Line of Business (LOB).<br />
        /// Validation:<br />
        /// 1) See the LobData service documentation for examples of valid values:<br />		
        /// </summary>
        [StringLength(2)]
        public string LOB { get; set; }

        /// <summary>
        /// If true or not specified, the response will include related InsuredNames.<br />
        /// If false, InsuredNames will not be returned.<br />
        /// Optional.<br />
        /// </summary>
        public bool? IncludeRelatedInsuredNames { get; set; }

        /// <summary>
        /// If true or not specified, the response will include related Officers.<br />
        /// If false, Officers will not be returned.<br />
        /// Optional.<br />
        /// </summary>
        public bool? IncludeRelatedOfficers { get; set; }

        /// <summary>
        /// If true or not specified, the response will include related Locations.<br />
        /// If false, Locations will not be returned.<br />
        /// Optional.<br />
        /// </summary>
        public bool? IncludeRelatedLocations { get; set; }

        /// <summary>
        /// If true or not specified, the response will include the associated graph of exposure data, including the following:<br />
        /// - LobData<br />
        /// - CoverageStates<br />
        /// - Exposures<br />
        /// - Modifiers<br />
        /// <br />
        /// If false, the above exposure data will not be returned.<br />
        /// Optional.<br />
        /// </summary>
        public bool? IncludeRelatedExposuresGraph { get; set; }

        /// <summary>
        /// If true or not specified, the response will include the associated graph of contact information, including the following:<br />
        /// - Contacts<br />
        /// - Phones<br />
        /// - Addresses<br />
        /// <br />
        /// If false, the above contact information will not be returned.<br />
        /// Optional.<br />
        /// </summary>
        public bool? IncludeRelatedContactsGraph { get; set; }

        /// <summary>
        /// If true or not specified, the response will include related PolicyData.<br />
        /// If false, PolicyData will not be returned.<br />
        /// Optional.<br />
        /// </summary>
        public bool? IncludeRelatedPolicyData { get; set; }

        /// <summary>
        /// If true or not specified, the response will include related RatingData.<br />
        /// If false, RatingData will not be returned.<br />
        /// Optional.<br />
        /// </summary>
        public bool? IncludeRelatedRatingData { get; set; }

        /// <summary>
        /// If true or not specified, the response will include related PaymentTerms.<br />
        /// If false, PaymentTerms will not be returned.<br />
        /// Optional.<br />
        /// </summary>
        public bool? IncludeRelatedPaymentTerms { get; set; }

        /// <summary>
        /// If true or not specified, the response will include related Questions.<br />
        /// If false, Questions will not be returned.<br />
        /// Optional.<br />
        /// </summary>
        public bool? IncludeRelatedQuestions { get; set; }

        /// <summary>
        /// If true or not specified, the response will include related QuoteStatus.<br />
        /// If false, QuoteStatus will not be returned.<br />
        /// Optional.<br />
        /// </summary>
        public bool? IncludeRelatedQuoteStatus { get; set; }
    }
}
