#region Using directives

using BHIC.Domain.QuestionEngine;
using System;
using System.Collections.Generic;

#endregion

namespace BHIC.ViewDomain.QuestionEngine
{
    public class WcPurchaseViewModel
    {
        public Policy Policy { get; set; }
        public PersonalContact PersonalContact { get; set; }
        public BusinessContact BusinessContact { get; set; }
        public Account Account { get; set; }
        public BusinessInfo BusinessInfo { get; set; }
        public MailingAddress MailingAddress { get; set; }
        public string SUIN { get; set; }
        public Dictionary<string, string> EffectedIds { get; set; }
    }
}


