using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BHIC.Domain.Background
{
    public class ClassDescriptionKeyword
    {
        /// <summary>
        /// ID of the ClassDescriptionKeyword DTO
        /// </summary>
        public int ClassDescKeywordId { get; set; }

        /// <summary>
        /// Keyword that can be compared against search string for the ClassDescriptionKeywords GET service; strings partially matching this keyword will be returned by the service.
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// Id of the ClassDescription DTO in the Industry > SubIndustry > ClassDescription hierarchy
        /// </summary>
        public int ClassDescriptionId { get; set; }

        /// <summary>
        /// ClassCode associated with the ClassDescription
        /// </summary>
        public string ClassCode { get; set; }

        /// <summary>
        /// ClassSuffix associated with the ClassClassDescription
        /// </summary>
        public string ClassSuffix { get; set; }

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
