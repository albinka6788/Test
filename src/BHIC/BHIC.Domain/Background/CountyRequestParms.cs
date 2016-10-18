using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Background
{
    /// <summary>
    /// Parameters associated with the Counties service<br />
    /// Filters the response as indicated by the comments for each parameter
    /// </summary>
    public class CountyRequestParms
    {
        /// <summary>
        /// Return the state associated with the specified zip
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        /// Return the first County matching the specified state<br />
        /// This property was added to support an automated test process used for QA, and satisfies the need to use a valid CSZ combination for automated testing purposes.<br />
        /// IMPORTANT: This property will normally never be populated.
        /// </summary>
        public string State { get; set; }
    }
}
