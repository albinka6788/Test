#region Using directives

using System;

#endregion

namespace BHIC.DML.WC.DTO
{
    public class OraganisationDTO :BaseClass
    {
        public int OrganizationID { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public char StateCode { get; set; }
        public int ZipCode { get; set; }
        public int CountryID { get; set; }
        public bool IsCorporateAddress { get; set; }
        public string ContactName { get; set; }
        public int ContactNumber1 { get; set; }
        public int ContactNumber2 { get; set; }
        public int Fax { get; set; }
    }
}
