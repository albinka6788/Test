using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    public class VExposuresMinPayrollRequestParms
    {
        /// <summary>
        /// Id of the ClassDescription associated with the test (e.g. - the ClassDescription selected by the user); required if ClassDescKeywordId is not specified
        /// </summary>
        public int? ClassDescriptionId { get; set; }

        /// <summary>
        /// Id of the ClassDescKeyword associated with the test (e.g. - the ClassDescKeyword selected by the user); required if ClassDescriptionId is not specified
        /// </summary>
        public int? ClassDescKeywordId { get; set; }

        /// <summary>
        /// Exposure Amount associated with the test (e.g.- annual payroll)
        /// </summary>
        [Required]
        public decimal ExposureAmt { get; set; }
    }
}
