using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Background
{
    /// <summary>
    /// Parameters associated with the System Variable service<br />
    /// Filters the response as indicated by the comments for each parameter
    /// </summary>
    public class SystemVariableRequestParms
    {
        /// <summary>
        /// Return system variables for the specified policy code. Not required.
        /// </summary>
        public string PolicyCode { get; set; }

        /// <summary>
        /// Return system variables for the specified agency. Not required unless Carrier is supplied.
        /// </summary>
        public string Agency { get; set; }

        /// <summary>
        /// Return system variables for the specified carrier. Not required unless Agency is supplied.
        /// </summary>
        public string Carrier { get; set; }

        /// <summary>
        /// Return system variables for the specified domain if PolicyCode or Agency are not supplied. Required.
        /// </summary>
        public string Domain { get; set; }
    }
}