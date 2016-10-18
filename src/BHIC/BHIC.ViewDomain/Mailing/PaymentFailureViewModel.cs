#region Using directives

using System;

#endregion

namespace BHIC.ViewDomain.Mailing
{
    public class PaymentFailureViewModel : MailTemplatesBaseViewModel
    {
        public string QuoteIdHeader { get; set; }
        public string LOBHeader { get; set; }
        public string PaymentConfirmationNoHeader { get; set; }
        public string PaymentConfirmationNo { get; set; }
        public string TransactionAmountHeader { get; set; }
        public string TransactionAmount { get; set; }
        public string FailureMessageHeader { get; set; }
        public string FailureMessage { get; set; }
        public string FailureDateTimeHeader { get; set; }
        public string FailureDateTime { get; set; }
        public string YourIPAddressHeader { get; set; }
        public string YourIPAddress { get; set; }
        public string YourEmailAddressHeader { get; set; }
        public string YourEmailAddress { get; set; }
        public string YourEmailAddressHref { get; set; }
        public string YourPhoneNumberHeader { get; set; }
        public string YourPhoneNumber { get; set; }
        public string YourNameHeader { get; set; }
        public string YourName { get; set; }
    }
}
