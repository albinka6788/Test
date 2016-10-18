using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Background
{
    /// <summary>
    /// Parameters associated with the PhoneTypes service<br />
    /// Filters the response as indicated by the comments for each parameter
    /// </summary>
    public class FieldDataRequestParms
    {
        /// <summary>
        /// Placeholder property for future expansion.<br />
        /// Set to true, to return all FieldDaTa.<br />
        /// </summary>
        public bool ReturnAll { get; set; }
    }
}
