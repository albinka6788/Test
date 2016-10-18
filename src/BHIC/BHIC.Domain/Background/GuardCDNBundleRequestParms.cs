using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Background
{
    /// <summary>
    /// Parameters associated with the GuardCDNBundle service<br />
    /// Filters the response as indicated by the comments for each parameter
    /// </summary>
    public class GuardCDNBundleRequestParms
    {
        /// <summary>
        /// Required.
        /// Return data for the specified bundle type
        /// </summary>
        public CDNBundleType BundleType { get; set; }

        /// <summary>
        /// Include the contents of the bundle. If not passed in, the contents will be returned.
        /// </summary>
        public bool IncludeContents { get; set; }
    }
}
