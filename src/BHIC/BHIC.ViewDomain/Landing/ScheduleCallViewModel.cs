#region Using directives

using BHIC.ViewDomain.Mailing;
using System;

#endregion

namespace BHIC.ViewDomain.Landing
{
    public class ScheduleCallViewModel
    {
        public DateTime RequestTime { get; set; }
        public string RequesterName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime RequestedCallTime { get; set; }
        public int QuoteId { get; set; }
        public string AbsoulteURL { get; set; }
    }
}
