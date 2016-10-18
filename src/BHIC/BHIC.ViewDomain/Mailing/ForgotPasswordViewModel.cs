#region Using directives

using System;

#endregion

namespace BHIC.ViewDomain.Mailing
{
    public class ForgotPasswordViewModel : MailTemplatesBaseViewModel
    {
        public string BaseUrl { get; set; }
        public string Name { get; set; }
        public string ResetPasswordLink { get; set; }
        public string RecipEmailAddr { get; set; }
    }
}
