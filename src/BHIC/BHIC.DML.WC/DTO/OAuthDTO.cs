using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BHIC.DML.WC.DTO
{
    /// <summary>
    /// Authorize DTO
    /// </summary>
    public class OAuthDTO
    {
        /// <summary>
        /// Access Token for all operations
        /// </summary>
        
        public string AccessToken { get; set; }

        
        /// <summary>
        /// Expires in No. of seconds
        /// </summary>
        public int ExpiresIn { get; set; }

        /// <summary>
        /// User Name
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Token Issued Time
        /// </summary>
        public string Issued { get; set; }
        /// <summary>
        /// Token Expire Time
        /// </summary>
        public string Expires { get; set; }
        /// <summary>
        /// Success/Failure
        /// </summary>
        public string Status { get; set; }
    }
}