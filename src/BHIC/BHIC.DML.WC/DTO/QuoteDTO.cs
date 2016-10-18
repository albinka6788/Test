#region Using directives

using System;

#endregion

namespace BHIC.DML.WC.DTO
{
    public class QuoteDTO : BaseClass
    {
        public Int64? OrganizationUserDetailID { get; set; }
        public Int64? OrganizationAddressID { get; set; }
        public string QuoteNumber { get; set; }
        public int LineOfBusinessId { get; set; }
        public int ExternalSystemID { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public decimal PremiumAmount { get; set; }
        public int PaymentOptionID { get; set; }
        public string AgencyCode { get; set; }
        public string RetrieveQuoteURL { get; set; }
    }
}
