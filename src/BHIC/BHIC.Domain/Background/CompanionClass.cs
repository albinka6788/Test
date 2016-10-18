using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Background
{
    /// <summary>
    /// Companion Class associated with a given ClassDescription / State.  There may be multiple CompanionClasses returned.
    /// </summary>
    public class CompanionClass
    {
        /// <summary>
        /// ID of the CompanionClass.<br />
        /// Example: 123<br />
        /// </summary>
        public int CompanionClassId { get; set; }

        /// <summary>
        /// User-friendly name for the CompanionClass.<br />
        /// Example: Clerical<br />
        /// </summary>
        [StringLength(80)]
        public string FriendlyLabel { get; set; }

        /// <summary>
        /// ClassCode associated with the ClassDescription
        /// </summary>
        [StringLength(4)]
        public string ClassCode { get; set; }

        /// <summary>
        /// ClassSuffix associated with the ClassClassDescription
        /// </summary>
        [StringLength(2)]
        public string ClassSuffix { get; set; }

        /// <summary>
        /// Read-only State-specific help associated with the companion class<br />
        /// NOTE: This property is populated as a result of calling CompanionClasses GET, or ClassDescriptions GET.  Although this DTO can be returned as a result of calling the Quotes GET service, the HelpText property is not populated as a result of calling Quotes GET due to the added expense, and because HelpText would normally not be useful for Quotes GET requests (since Quotes GET is typically called later in the flow, after the point in time that this property would be useful).
        /// </summary>
        [StringLength(2000)]
        public string HelpText { get; set; }

        /// <summary>
        /// Read-only State-specific text that can be used to ask a question about a specific companion class.<br />
        /// <br />
        /// Example: "Are there any clerical office employees or real estate/leasing agents?"<br />
        /// <br />
        /// Instances of the text “[Friendly Label]” that exist within PromptText are replaced by the friendly label for the primary Class Description if defined.  (If a friendly label is not defined for the primary Class Description, the primary Class Description’s Descrip value is used, instead.) <br />
        /// <br />
        /// NOTE: This property is populated as a result of calling CompanionClasses GET, or ClassDescriptions GET.  Although this DTO can be returned as a result of calling the Quotes GET service, the PromptText property is not populated as a result of calling Quotes GET due to the added expense, and because PromptText would normally not be useful for Quotes GET requests (since Quotes GET is typically called later in the flow, after the point in time that this property would be useful).
        /// </summary>
        [StringLength(2000)]
        public string PromptText { get; set; }
    }
}
