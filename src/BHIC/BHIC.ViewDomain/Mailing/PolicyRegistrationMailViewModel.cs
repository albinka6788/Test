#region Using directives

using System;

#endregion

namespace BHIC.ViewDomain.Mailing
{
    public class PolicyRegistrationMailViewModel : MailTemplatesBaseViewModel
    {
        public string EmailVarificationUrlHref { get; set; }
        public string RegisterEmailText { get; set; }
        public string RegisterEmailHref { get; set; }
        public string UserName { get; set; }
        public string LinkText { get; set; }
        public string PolicyCenterWebsiteUrlHref { get; set; }
    }
}


