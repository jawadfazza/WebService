using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace WebService.Library
{
    public class Mail 
    {
        IConfiguration configuration;
        static string fromPassword="";
        public Mail(IConfiguration configuration)
        {
              fromPassword = configuration.GetConnectionString("HotmailPasword");
        }
        

        public static void SendEmail(string emailTitle,string ToAddress,string FromAddress, string subject,string body)
        {
            var fromAddress = new MailAddress("e.jawadfazza@hotmail.com", emailTitle);
                


            var toAddress = new MailAddress(ToAddress);
            var alternativeEmailAddress = FromAddress;
          
            var smtp = new SmtpClient
            {
                Host = "smtp.office365.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                Timeout = 20000 // Set a timeout (milliseconds) for the SMTP connection
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                message.Subject += $" (Sent from: {alternativeEmailAddress})";
                try
                {
                    smtp.Send(message);
                    Console.WriteLine("Email sent successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error sending email: " + ex.Message);
                }
            }
        }
    }
}
