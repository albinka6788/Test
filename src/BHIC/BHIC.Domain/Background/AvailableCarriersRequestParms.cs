using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Background
{
    /// <summary>
    /// Parameters associated with the AvailableCarriers service<br />
    /// Filters the response as indicated by the comments for each parameter
    /// </summary>
    public class AvailableCarriersRequestParms
    {
        /// <summary>
        /// The effective date of the policy so the carriers can be filtered based on this date
        /// </summary>
        [Required]
        public DateTime EffectiveDate { get; set; }

        /// <summary>
        /// A list of states on the policy
        /// </summary>
        [Required]
        public List<string> States { get; set; }

        /// <summary>
        /// The line of business to search for. Each line must be searched individually
        /// </summary>
        [Required]
        [StringLength(2)]
        public string LOB { get; set; }
    }
}
