using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Background
{
    /// <summary>
    /// Parameters associated with the ClassIndustries service<br />
    /// Filters the response as indicated by the comments for each parameter
    /// </summary>
    public class ClassDescriptionRequestParms
    {
        /// <summary>
        /// Return data for specified Lob (WC, BP, CA....)
        /// </summary>
        [StringLength(2)]
        [Required]
        public string Lob { get; set; }

        /// <summary>
        /// Return data for the specified State<br />
        /// Validation: Either State or Zip code is required.<br />
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Return data for the State associated with the specified ZipCode<br />
        /// Validation: Either State or Zip code is required.<br />
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        /// Return data for the specified SubIndustry
        /// </summary>
        public int? SubIndustryId { get; set; }

        /// <summary>
        /// Return the specified ClassDescription
        /// </summary>
        public int? ClassDescriptionId { get; set; }

        /// <summary>
        /// If true, the response will include related child objects (CompanionClasses)<br />
        /// If false, the response will include only the requested ClassDescription object(s).<br />
        /// </summary>
        public bool IncludeRelated { get; set; }

    }
}
