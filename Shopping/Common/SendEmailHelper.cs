using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class SendEmailHelper
    {
        public static void SendEmailToCustomer(string emailAddress, string senderName, string title, string content)
        {
            try
            {
                var fromAddress = new MailAddress("ShoppingOnlineTNSF@gmail.com", "Shopping Online SF");
                var toAddress = new MailAddress(emailAddress, senderName);
                const string fromPassword = "thanhnamsf";
                string subject = title;
                string body = content;
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                {
                    smtp.Send(message);
                }
            }
            catch (Exception e)
            {

            }

        }
    }
}
