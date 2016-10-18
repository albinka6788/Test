using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Background
{
    public class ClassDescription
    {
        // init objects to help avoid null exceptions
        public ClassDescription()
        {
            CompanionClasses = new List<CompanionClass>();
        }

        /// <summary>
        /// Id of the SubIndustry DTO in the Industry > SubIndustry > ClassDescription hierarchy
        /// </summary>
        public int SubIndustry { get; set; }

        /// <summary>
        /// Id of the ClassDescription DTO in the Industry > SubIndustry > ClassDescription hierarchy
        /// </summary>
        public int ClassDescriptionId { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// User-friendly name for the Class.<br />
        /// Example: Clerical<br />
        /// </summary>
        public string FriendlyLabel { get; set; }

        /// <summary>
        /// List of Companion Classes associated with the ClassDescription
        /// </summary>
        public List<CompanionClass> CompanionClasses { get; set; }

        /// <summary>
        /// ClassCode associated with the ClassDescription
        /// </summary>
        public string ClassCode { get; set; }

        /// <summary>
        /// ClassSuffix associated with the ClassClassDescription
        /// </summary>
        public string ClassSuffix { get; set; }

        /// <summary>
        /// Help Text associated with the ClassClassDescription<br />
        /// NOTE: This property is populated as a result of calling ClassDescriptions GET.  Although this DTO can be returned as a result of calling the Quotes GET service, the HelpText property is not populated as a result of calling Quotes GET due to the added expense, and because HelpText would normally be called after HelpText is useful.
        /// </summary>
        public string HelpText { get; set; }

        /// <summary>
        /// Flag that describes the availability of the ClassDescription <br />
        /// Currently Supported Values:
        /// <ul>
        /// <li>Y: Can be included in a policies sold via Direct Sales</li>
        /// <li>E: Can be included in a policies sold via Direct Sales (represents 'E'xtended appetite classes)</li>
        /// <li>N: Not valid for policies sold via Direct Sales </li>
        /// </ul>
        /// </summary>
        public string DirectOK { get; set; }
    }
}
