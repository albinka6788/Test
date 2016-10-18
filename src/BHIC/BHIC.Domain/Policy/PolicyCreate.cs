using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    /// <summary>
    /// Parameters associated with creation and issuance of a policy.<br />
    /// <br />
    /// <b>IMPORTANT:</b> The use of this service first requires that payment has been performed and confirmed to be successful.
    /// </summary>
    public class PolicyCreate
    {
        /// <summary>
        /// ID of the Quote to be used as a source of data for the new policy.<br />
        /// Example: 123<br />
        /// Validation:<br />
        /// 1) Required.<br />
        /// 2) Additionally, the PolicyCreate POST service requires, at minimum, the prior successful execution of services to create the following objects:<br />
        /// - PolicyData<br />
        /// - LobData<br />
        /// - CoverageState<br />
        /// - Exposures<br />
        /// - Questions<br />
        /// - PaymentTerms<br />
        /// - Contact/Address/Phone<br />
        /// - Location (specifically, a business mailing address is required; set Location.LocationType = M)<br />
        /// - Modifiers (employer liability limits modifier must exist)
        /// </summary>
        [Required]
        public int? QuoteId { get; set; }

        /// <summary>
        /// Specify the Line of Business associated with the policy to be created.<br />
        /// This method must be called once for each LOB associated with the Quote; one policy will be issued for each LOB submitted.<br />
        /// Validation:<br />
        /// 1) Required.<br />
        /// 2) See the LobData service documentation for examples of valid values:<br />		
        /// </summary>
        [Required]
        [StringLength(2)]
        public string LOB { get; set; }

        /// <summary>
        /// User ID  (Example: email address currently serves as User ID for the Cover Your Business site)<br />
        /// <br />
        /// <b>Side Effects:</b><br />
        /// The successful completion of a PolicyCreate POST request will result in the automatic execution the UserPolicyCodes POST request, using this property as the UserId property for that request. <br />
        /// <br />
        /// <b>Validation Performed:</b><br />
        /// - Must be a valid email address.<br />
        /// - Required<br />
        /// <br />
        /// 3/22/2016: This property is now required, in accordance with the instructions below.<br />
        /// <br />
        /// 3/16/2016: <span style='color:red;'><b>Code Changes Required: </b></span><br />
        /// 1) Populate this property for all existing PolicyCreate POST requests, and treat this property as required.<br />
        /// 2) Prior to the addition of this property, most client code requests to PolicyCreate POST were followed by a request to UserPolicyCodes POST.  These instances of UserPolicyCodes POST requests must be removed, since they'll now be automatically performed by the PolicyCreate POST request.<br />
        /// </summary>
        [StringLength(150), Required]
        [EmailAddress]
        public string UserId { get; set; }

        /// <summary>
        /// IMPORTANT: This flag is for internal use only, and must never be set to true.<br />
        /// This flag is to be used only by internal Insurance Service logic.<br />
        /// Setting this flag to true under normal conditions is undefined, and could result in unexpected behavior, including data corruption.
        /// </summary>
        // This property is tested by the PolicyCreateController.Post() logic to skip validation that would prevent a quote from being imported if it already has an MGA code.  
        // Skipping the above validation allows multiple requests to be submitted for the same quote id in rapid succession, without consideration of whether or not it was already imported.
        // This property is currently only set to true for stress testing.
        public bool InternalFlag1 { get; set; }
    }
}
