using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Background
{
    /// <summary>
    /// Parameters associated with the Lookup service<br />
    /// Filters the response as indicated by the comments for each parameter
    /// </summary>
    public class LookupRequestParms
    {
        /// <summary>
        /// Return lookup values for the specified program ID
        /// </summary>
        public string ProgramId { get; set; }

        /// <summary>
        /// Return lookup values for the specified key field
        /// </summary>
        public string KeyField { get; set; }

        /// <summary>
        /// Return lookup values that follow the exception rule for the key field
        /// </summary>
        public string KeyExcept { get; set; }

        /// <summary>
        /// Return lookup values for the specified line of business
        /// </summary>
        public string LOB { get; set; }

        /// <summary>
        /// Return lookup values for the specified value
        /// </summary>
        public string Val { get; set; }
    }
}