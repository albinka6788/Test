#region Using directives

using System;

#endregion

namespace BHIC.DML.WC.DTO
{
    public class OrganisationAddress : BaseClass
    {
        public Int64? Id { get; set; }
        public Int64? OrganizationID { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string StateCode { get; set; }
        public int ZipCode { get; set; }
        public int? CountryID { get; set; }
        public bool IsCorporateAddress { get; set; }
        public string ContactName { get; set; }
        public Int64 ContactNumber1 { get; set; }
        public Int64? ContactNumber2 { get; set; }
        public Int64? Fax { get; set; }
    }
}
