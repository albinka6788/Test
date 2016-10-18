using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Background
{
    /// <summary>
    /// Parameters associated with the CompanionClasses service<br />
    /// Filters the response as indicated by the comments for each parameter
    /// </summary>
    public class CompanionClassRequestParms
    {
        /// <summary>
        /// Return companion classes for the specified ClassDescriptionId
        /// Validation: Required.
        /// </summary>
        [Required]
        public int? ClassDescriptionId { get; set; }

        /// <summary>
        /// Return companion classes for the specified State<br />
        /// Validation: Either State or Zip code is required.<br />
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Return data for the State associated with the specified ZipCode<br />
        /// Validation: Either State or Zip code is required.<br />
        /// </summary>
        public string ZipCode { get; set; }
    }
}
