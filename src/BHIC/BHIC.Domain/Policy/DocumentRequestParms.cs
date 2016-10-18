using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    /// <summary>
    /// Parameters associated with the Documents service<br />
    /// Filters the response as indicated by the comments for each parameter
    /// </summary>
    public class DocumentRequestParms
    {
        /// <summary>
        /// return data for specified Policy
        /// </summary>
        [StringLength(10)]
        public string PolicyCode { get; set; }

        /// <summary>
        /// return certificates for specified session ID. Required.
        /// </summary>
        [StringLength(255)]
        public string SessionId { get; set; }

        /// <summary>
        /// return data based on the document grouping
        /// </summary>
        public DocumentGrouping DocumentGroup { get; set; }
    }
}
