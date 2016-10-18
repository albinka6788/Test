using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;


namespace BHIC.DML.WC.DTO
{
    /// <summary>
    /// Save Quote Request Parameters
    /// </summary>
    public class SaveQuoteRequestDTO
    {
        /// <summary>
        /// User Name
        /// </summary>
        [StringLength(150), Required]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Email is not valid")]
        public string UserName { get; set; }
        /// <summary>
        /// Quote ID
        /// </summary>
        [StringLength(20), Required]
        public string QuoteNumber { get; set; }

        /// <summary>
        /// Reterive Quote URL
        /// </summary>
        [StringLength(500)]
        public string ReteriveQuoteURL { get; set; }
    }
}
