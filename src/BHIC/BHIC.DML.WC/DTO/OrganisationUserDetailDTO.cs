#region Using directives

using System;

#endregion

namespace BHIC.DML.WC.DTO
{
    public class OrganisationUserDetailDTO : BaseClass
    {
        public int Id { get; set; }
        public string OrganizationName { get; set; }
        public string EmailID { get; set; }
        public string Password { get; set; }
        public string Tin { get; set; }
        public string Ssn { get; set; }
        public string Fein { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PolicyCode { get; set; }
        public long PhoneNumber { get; set; }
        public string QuoteNumber { get; set; }
        public int UserRoleId { get; set; }
    }
}
