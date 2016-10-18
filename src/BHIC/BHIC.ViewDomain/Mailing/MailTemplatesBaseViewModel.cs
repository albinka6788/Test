#region Using directives

using System;

#endregion

namespace BHIC.ViewDomain.Mailing
{
    public class MailTemplatesBaseViewModel
    {
        public string CompanyName { get; set; }
        public string WebsiteUrlText { get; set; }
        public string WebsiteUrlHref { get; set; }
        public string SupportPhoneNumber { get; set; }
        public string SupportPhoneNumberHref { get; set; }
        public string SupportEmailText { get; set; }
        public string SupportEmailHref { get; set; }
        public string TargetUrl { get; set; }
        public string Physical_Address2 { get; set; }
        public string Physical_AddressCSZ { get; set; }
        public string AbsoulteURL { get; set; }
        public string QuoteId { get; set; }

        public string LOB { get; set; }
    }
}


