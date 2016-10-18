#region Using directives

using System;

#endregion

namespace BHIC.ViewDomain.Mailing
{
    public class ScheduleCallMailViewModel : MailTemplatesBaseViewModel
    {
        public string RequestTimeHeader { get; set; }
        public string RequestTime { get; set; }
        public string IpAddressHeader { get; set; }
        public string IpAddress { get; set; }
        public string RequesterNameHeader { get; set; }
        public string RequesterName { get; set; }
        public string PhoneNumberHeader { get; set; }
        public string PhoneNumber { get; set; }
        public string QuoteIdHeader { get; set; }
        public string RequestedCallTimeHeader { get; set; }
        public string RequestedCallTime { get; set; }
    }
}
