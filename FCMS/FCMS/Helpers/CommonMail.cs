using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FCMS.Helpers
{
    public class CommonMail
    {

        public static bool SendMail(string SMTPServer, int SMTPPort, bool SMTPSSL, string From, string password, string To, string Subject, string Body, string CC, string Attachment)
        {

            MailMessage message = new MailMessage();
            SmtpClient smtpClient = new SmtpClient();
            string Password = string.Empty;
            string msg = string.Empty;
            try
            {
                SmtpClient SmtpServer = new SmtpClient(SMTPServer);
                message.From = new MailAddress(From, "FCMS Admin");
                message.To.Add(To);
                //message.CC.Add(CC);
                message.Subject = Subject;
                message.IsBodyHtml = true;
                message.Body = Body;

                if (Attachment != "" && File.Exists(Attachment))
                {
                    var fileStream = new FileStream(Attachment, FileMode.Open, FileAccess.Read);
                    Attachment attachment = new Attachment(fileStream, Path.GetFileName(Attachment));
                    message.Attachments.Add(attachment);
                }
                SmtpServer.Port = SMTPPort;
                SmtpServer.UseDefaultCredentials = true;
                SmtpServer.Credentials = new System.Net.NetworkCredential(From, password);
                SmtpServer.EnableSsl = SMTPSSL;
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.Send(message);
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }

        }

    }
}
