#region Using directives

using System;
using System.Collections.Generic;

#endregion

namespace BHIC.ViewDomain.Landing
{
    public class SaveForLaterViewModel
    {
        public int QuoteId { get; set; }
        public string BaseUrl { get; set; }
        public string SubUrl { get; set; }
        public List<string> RecipEmail { get; set; }
    }
}
