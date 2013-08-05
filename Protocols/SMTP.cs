/* The MIT License (MIT)
Copyright (c) 2013 Alessandro Cappellozza (alessandro.cappellozza@gmail.com)

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
documentation files (the "Software"), to deal in the Software without restriction, including without limitation
the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software,
and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of
the Software. THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO
EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,  WHETHER IN
AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE
OR OTHER DEALINGS IN THE SOFTWARE.*/

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
            try
            {
                SmtpClient smtp = new SmtpClient(xmlConfig.SMTP_Server, xmlConfig.SMTP_Port);
                MailMessage message = new MailMessage(from, to, sbj, msg);
                NetworkCredential credentials = new NetworkCredential(xmlConfig.SMTP_Username, xmlConfig.SMTP_Password);

                if (xmlConfig.SMTP_Username != "") { smtp.Credentials = credentials; }
                message.IsBodyHtml = false;
                smtp.Send(message);
                Program.Print("DONE ", false);
            }
            catch (Exception e) {
                Program.Print("Error {0}", false, e.Message);
            }
        }
    }
}
