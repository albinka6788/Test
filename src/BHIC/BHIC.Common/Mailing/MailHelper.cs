#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Linq;

using BHIC.Common.Logging;
using BHIC.Common.XmlHelper;
using BHIC.Common.Config;
using System.Threading;

#endregion

namespace BHIC.Common.Mailing
{
    public class MailHelper
    {
        #region Comment : Here api/service call logging variables

        private static ILoggingService logger = LoggingService.Instance;

        #endregion

        #region Public Methods

        /// <summary>
        ///  This helper class sends an email message using the System.Net.Mail namespace
        /// </summary>
        /// <param name="fromEmail">Sender email address</param>
        /// <param name="toEmail">List of recipient email address</param>
        /// <param name="cc">List of carbon copy email address</param>
        /// <param name="bcc">List of blind carbon copy email address</param>
        /// <param name="subject">Subject of the email message</param>
        /// <param name="body">Body of the email message</param>
        /// <param name="attachmentFullPath">List of File to attach</param>
        public void SendMailMessage(string fromEmail, List<string> toEmail, List<string> cc, List<string> bcc, string subject, string body,
            List<string> attachmentFullPath, bool isEncryptedFile = false, string password = null)
        {
            password = (password == null ? ConfigCommonKeyReader.DefaultPassword : password);
            if (toEmail.Count == 0 || string.IsNullOrEmpty(subject))
            {
                throw new Exception(Constants.InvalidEmailParams);
            }

            //create the MailMessage object
            MailMessage mailMessage = new MailMessage();
            if (attachmentFullPath != null)
            {
                //add any attachments from the filesystem
                if (!isEncryptedFile)
                {
                    attachmentFullPath.ForEach(res => mailMessage.Attachments.Add(new Attachment(res)));
                }
                else
                {
                    attachmentFullPath.ForEach(res => mailMessage.Attachments.Add(GetNewAttachment(res, password)));
                }
            }
            SendMail(mailMessage, fromEmail, toEmail, cc, bcc, subject, body);
        }

        /// <summary>
        /// This helper class sends an email message using the System.Net.Mail namespace
        /// </summary>
        /// <param name="fromEmail"></param>
        /// <param name="toEmail"></param>
        /// <param name="cc"></param>
        /// <param name="bcc"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="messageStream"></param>
        /// <param name="fileName"></param>
        public void SendMailMessage(string fromEmail, List<string> toEmail, List<string> cc, List<string> bcc, string subject, string body, Stream messageStream,
            string fileName = null)
        {
            if (toEmail.Count == 0 || string.IsNullOrEmpty(subject))
            {
                throw new Exception(Constants.InvalidEmailParams);
            }

            if (string.IsNullOrEmpty(fileName))
            {
                fileName = "LogFile.log";
            }

            //create the MailMessage object
            MailMessage mailMessage = new MailMessage();
            mailMessage.Attachments.Add(new Attachment(messageStream, fileName));
            SendMail(mailMessage, fromEmail, toEmail, cc, bcc, subject, body);
        }

        /// <summary>
        ///  This helper class sends an email message using the System.Net.Mail namespace
        /// </summary>
        /// <param name="fromEmail">Sender email address</param>
        /// <param name="toEmail">List of recipient email address</param>
        /// <param name="cc">List of carbon copy email address</param>
        /// <param name="bcc">List of blind carbon copy email address</param>
        /// <param name="subject">Subject of the email message</param>
        /// <param name="body">Body of the email message</param>
        public void SendMailMessage(string fromEmail, List<string> toEmail, List<string> cc, List<string> bcc, string subject, string body)
        {
            SendMailMessage(fromEmail, toEmail, cc, bcc, subject, body, new List<string>());
        }

        /// <summary>
        ///  This helper class sends an email message using the System.Net.Mail namespace
        /// </summary>
        /// <param name="fromEmail">Sender email address</param>
        /// <param name="toEmail">List of recipient email address</param>
        /// <param name="cc">List of carbon copy email address</param>
        /// <param name="bcc">List of blind carbon copy email address</param>
        /// <param name="subject">Subject of the email message</param>
        public void SendMailMessage(string fromEmail, List<string> toEmail, List<string> cc, List<string> bcc, string subject)
        {
            SendMailMessage(fromEmail, toEmail, cc, bcc, subject, string.Empty, new List<string>());
        }

        /// <summary>
        ///  This helper class sends an email message using the System.Net.Mail namespace
        /// </summary>
        /// <param name="fromEmail">Sender email address</param>
        /// <param name="toEmail">List of recipient email address</param>
        /// <param name="cc">List of carbon copy email address</param>
        /// <param name="subject">Subject of the email message</param>
        public void SendMailMessage(string fromEmail, List<string> toEmail, List<string> cc, string subject)
        {
            SendMailMessage(fromEmail, toEmail, cc, new List<string>(), subject, string.Empty, new List<string>());
        }

        /// <summary>
        ///  This helper class sends an email message using the System.Net.Mail namespace
        /// </summary>
        /// <param name="fromEmail">Sender email address</param>
        /// <param name="toEmail">List of recipient email address</param>
        /// <param name="subject">Subject of the email message</param>
        /// <param name="body">Body of the email message</param>
        public void SendMailMessage(string fromEmail, List<string> toEmail, string subject, string body)
        {
            SendMailMessage(fromEmail, toEmail, new List<string>(), new List<string>(), subject, body, new List<string>());
        }

        /// <summary>
        ///  This helper class sends an email message using the System.Net.Mail namespace
        /// </summary>
        /// <param name="fromEmail">Sender email address</param>
        /// <param name="toEmail">List of recipient email address</param>
        /// <param name="subject">Subject of the email message</param>
        public void SendMailMessage(string fromEmail, List<string> toEmail, string subject)
        {
            SendMailMessage(fromEmail, toEmail, new List<string>(), new List<string>(), subject, string.Empty, new List<string>());
        }

        /// <summary>
        /// This helper class sends an email message using the System.Net.Mail namespace
        /// </summary>
        /// <param name="toEmail">List of recipient email address</param>
        /// <param name="subject">Subject of the email message</param>
        public void SendMailMessage(List<string> toEmail, string subject)
        {
            SendMailMessage(string.Empty, toEmail, new List<string>(), new List<string>(), subject, string.Empty, new List<string>());
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Return attachment object from stream object from specified file after decrypting.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private Attachment GetNewAttachment(string fileName, string password)
        {
            var input = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            MemoryStream ms = new MemoryStream();
            input.DecryptFile(ms, password);
            return new Attachment(ms, Path.GetFileName(fileName), System.Web.MimeMapping.GetMimeMapping(fileName));
        }

        private void SendMail(MailMessage mailMessage, string fromEmail, List<string> toEmail, List<string> cc, List<string> bcc, string subject, string body)
        {
            //set the sender address of the mail message
            if (!string.IsNullOrWhiteSpace(fromEmail))
            {
                mailMessage.From = new MailAddress(fromEmail);
            }


            if (toEmail != null)
            {
                //Comment : Here Till environnment is not LIVE don't send mail to user iteself send it to configured ClientEmailID only 
                //toEmail = ConfigCommonKeyReader.IsLiveEnvironment ? toEmail : UtilityFunctions.GetListOfSplitedValues(ConfigCommonKeyReader.ClientEmailID);

                //set the recipient address of the mail message
                toEmail.ForEach(res => mailMessage.To.Add(new MailAddress(res)));
            }

            if (cc != null)
            {
                //if (ConfigCommonKeyReader.IsLiveEnvironment ||
                //        string.Join(";", cc).Equals(string.Join(";", toEmail), StringComparison.OrdinalIgnoreCase) == false)
                {
                    //set the carbon copy address
                    cc.ForEach(res => mailMessage.CC.Add(new MailAddress(res)));
                }
            }

            if (bcc != null)
            {
                //if (ConfigCommonKeyReader.IsLiveEnvironment ||
                //        string.Join(";", bcc).Equals(string.Join(";", toEmail), StringComparison.OrdinalIgnoreCase) == false)
                {
                    //set the blind carbon copy address
                    bcc.ForEach(res => mailMessage.Bcc.Add(new MailAddress(res)));
                }
            }

            //set the subject of the mail message
            mailMessage.Subject = subject;

            //set the body of the mail message
            mailMessage.Body = body;

            //set the format of the mail message body
            mailMessage.IsBodyHtml = true;

            //set the priority
            mailMessage.Priority = MailPriority.Normal;

            Thread emailThread = new Thread(delegate()
            {
                SendEmail(mailMessage);
            });

            emailThread.IsBackground = true;
            emailThread.Start();
        }

        private void SendEmail(MailMessage mailMessage)
        {
            bool isSuccess = false;

            do
            {
                using (SmtpClient smtp = new SmtpClient())
                {
                    try
                    {
                        // send the email
                        smtp.Send(mailMessage);
                        logger.Trace(string.Format("Successfully send Mail with subject '{0}' to following receipients{1}{2}{3}{4}{5}{6}", 
                            mailMessage.Subject, Environment.NewLine, mailMessage.To.ToString(), Environment.NewLine, mailMessage.CC.ToString(), 
                            Environment.NewLine, mailMessage.Bcc.ToString()));
                        isSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        //log error
                        logger.Fatal(ex);
                        Thread.Sleep(10000);
                    }
                    if (isSuccess)
                    {
                        mailMessage.Dispose();
                        mailMessage = null;
                    }
                }
            } while (!isSuccess);
        }
        #endregion

    }
}