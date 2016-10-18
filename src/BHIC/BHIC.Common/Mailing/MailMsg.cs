#region Using Directives

using System.Collections.Generic;

#endregion

namespace BHIC.Common.Mailing
{
    public class MailMsg
    {
        public string SenderEmailAddr { get; set; }
        public List<string> RecipEmailAddr { get; set; }
        public string RecipDisplayName { get; set; }
        public List<string> Cc { get; set; }
        public List<string> Bcc { get; set; }
        public string Subject { get; set; }
        public string MessageBody { get; set; }
        public List<string> AttachmentFullPath { get; set; }
    }
}
