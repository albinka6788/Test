using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Background
{
    /// <summary>
    /// Parameters associated with the CityStateZipCodeSearch service<br />
    /// Filters the response as indicated by the comments for each parameter
    /// </summary>
    public class CityStateZipCodeSearchRequestParms : CityStateZipCode
    {
        /// <summary>
        /// The search type to use
        /// </summary>
        public CityStateZipCodeSearchType CityStateZipCodeSearchType { get; set; }
    }
}
