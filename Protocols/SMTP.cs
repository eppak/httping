using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Mail;

namespace httping.Protocols
{
    class SMTP
    {
        public static void send(string from, string to, string sbj, string msg){
            SmtpClient smtp = new SmtpClient(xmlConfig.SMTP_Server);
            MailMessage message = new MailMessage(from, to, sbj, msg);
            NetworkCredential credentials = new NetworkCredential(xmlConfig.SMTP_Username, xmlConfig.SMTP_Password);

            if (xmlConfig.SMTP_Username != "") { smtp.Credentials = credentials; }
            message.IsBodyHtml = false;
            smtp.Send(message);
        }
    }
}
