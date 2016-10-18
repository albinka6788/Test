using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    /// <summary>
    /// Parameters associated with the Exposures service<br />
    /// Filters the response as indicated by the comments for each parameter
    /// </summary>
    public class ExposureRequestParms
    {
        /// <summary>
        /// Return data for specified Quote<br />
        /// Validation:  Please see the following for examples of valid combinations of parameters: <br />
        /// a) QuoteId: Returns all Exposures for the specified Quote  <br />
        /// b) QuoteId and Lob (WC, CA, BP...): Returns all Exposures associated with the specified Line of Business  <br />
        /// c) LobDataId: Returns all Exposures associated with the specified Line of Business  <br />
        /// d) LobDataId and State (AL, KS, WI): Returns all Exposures associated with the specified Coverage State  <br />
        /// e) CoverageStateId:  Returns all Exposures associated with the specified Coverage State  <br />
        /// f) ExposureId: Returns a specific Exposure. <br />		
        /// </summary>
        public int? QuoteId { get; set; }

        /// <summary>
        /// Return Exposures under the specified LobData within the object hierarchy.<br />
        /// Validation<br />
        /// 1) See QuoteId for valid combinations of parameters for the Exposures GET request.<br />
        /// </summary>
        public int? LobDataId { get; set; }

        /// <summary>
        /// Return Exposures under the LobData within the object hierarchy which is associated with the specified Lob. (WC, BP, CA....)<br />
        /// Validation<br />
        /// 1) See QuoteId for valid combinations of parameters for the Exposures GET request.<br />
        /// </summary>
        [StringLength(2)]
        public string Lob { get; set; }

        /// <summary>
        /// Return Exposures under the specified CoverageState, within the object hierarchy<br />
        /// Validation<br />
        /// 1) See QuoteId for valid combinations of parameters for the Exposures GET request.<br />
        /// </summary>
        public int? CoverageStateId { get; set; }

        /// <summary>
        /// Return Exposures under the CoverageState within the object hierarchy which is associated with the specified State. (AL, KS, WI....)<br />
        /// Validation<br />
        /// 1) See QuoteId for valid combinations of parameters for the Exposures GET request.<br />
        /// </summary>
        [StringLength(2)]
        public string State { get; set; }

        /// <summary>
        /// Return the specified Exposure<br />
        /// Validation<br />
        /// 1) See QuoteId for valid combinations of parameters for the Exposures GET request.<br />
        /// </summary>
        public int? ExposureId { get; set; }

    }
}
