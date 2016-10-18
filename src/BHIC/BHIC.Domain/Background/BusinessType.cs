using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Background
{
    public class BusinessType
    {
        /// <summary>
        /// Business Type code / identifier
        /// </summary>
        [StringLength(150)]
        public string BusinessTypeCode { get; set; }  // vals

        /// <summary>
        /// Short Description
        /// </summary>
        [StringLength(255)]
        public string Description { get; set; }  // Descrip

        /// <summary>
        /// Alternate Description
        /// </summary>
        [StringLength(255)]
        public string AlternateDescription { get; set; }  // AltDescrip

        /// <summary>
        /// If true is returned, the associated business name is expected to be a person's name..
        /// </summary>
        public bool IsPersonName { get; set; }
    }
}
