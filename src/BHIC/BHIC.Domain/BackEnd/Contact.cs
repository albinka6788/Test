#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace BHIC.Domain.BackEnd
{
    /// <summary>
    /// This DTO was added to support the PolicyDetails service, which returns policy data from back-end systems.<br />
    /// Specifically, this defines the properties returned by PolicyDetails.BackEndContacts.
    /// </summary>
    public class Contact
    {
        // NOTE: some commented properties added below for easier future expansion.

        //public int CONTACTID { get; set; }

        //[StringLength(1)]
        //public string CATEGORY { get; set; }

        //[StringLength(64)]
        //public string CATEGORYVALUE { get; set; }

        //public string CATEGORYDESC { get; set; }

        //[StringLength(128)]
        //public string JOBTITLE { get; set; }

        //[StringLength(128)]
        //public string COMPANY { get; set; }

        /// <summary>
        /// Contact Name
        /// </summary>
        [StringLength(256)]
        public string NAME { get; set; }

        //public string CONTACTNAME { get; set; }

        /// <summary>
        /// Home Phone Number
        /// </summary>
        [StringLength(11)]
        public string HOME { get; set; }

        //[StringLength(4)]
        //public string HOMEEXT { get; set; }

        /// <summary>
        /// Business Phone Number
        /// </summary>
        [StringLength(11)]
        public string BUSINESS { get; set; }

        /// <summary>
        /// Business Extension
        /// </summary>
        [StringLength(4)]
        public string BUSINESSEXT { get; set; }

        //[StringLength(11)]
        //public string FAX { get; set; }

        //[StringLength(4)]
        //public string FAXEXT { get; set; }

        /// <summary>
        /// Mobile Number
        /// </summary>
        [StringLength(11)]
        public string MOBILE { get; set; }

        //[StringLength(11)]
        //public string PAGER { get; set; }

        //[StringLength(256)]
        //public string ADDRESS1 { get; set; }

        //[StringLength(64)]
        //public string CITY1 { get; set; }

        //[StringLength(2)]
        //public string STATE1 { get; set; }

        //[StringLength(50)]
        //public string ZIPCODE1 { get; set; }

        //public int? ADDRESS1TYPE { get; set; }

        //[StringLength(256)]
        //public string ADDRESS2 { get; set; }

        //[StringLength(64)]
        //public string CITY2 { get; set; }

        //[StringLength(2)]
        //public string STATE2 { get; set; }

        //[StringLength(50)]
        //public string ZIPCODE2 { get; set; }

        //public int? ADDRESS2TYPE { get; set; }

        //public bool? MAILINGADDRESS { get; set; }

        /// <summary>
        /// Contact Email Address
        /// </summary>
        [StringLength(128)]
        public string EMAIL { get; set; }

        //[StringLength(128)]
        //public string WEBADDR { get; set; }

        //public string NOTES { get; set; }

        //public DateTime CREATED { get; set; }

        //[StringLength(20)]
        //public string CREATEDBY { get; set; }

        //public DateTime? MODIFIED { get; set; }

        //[StringLength(20)]
        //public string MODIFIEDBY { get; set; }

        /// <summary>
        /// Equivalent to ContactType.Description (examples: Executive, Billing, Risk Manager... )
        /// </summary>
        public string Description { get; set; }
    }
}