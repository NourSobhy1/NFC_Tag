using Microsoft.Extensions.Configuration;
using NFC.Application.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NFC.Application.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;

        public EmailSender(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var emailSettings = _config.GetSection("EmailSettings");

            var client = new SmtpClient(emailSettings["SmtpServer"], int.Parse(emailSettings["Port"]))
            {
                Credentials = new NetworkCredential(emailSettings["SenderEmail"], emailSettings["SenderPassword"]),
                EnableSsl = bool.Parse(emailSettings["EnableSsl"])
            };

            var mail = new MailMessage(emailSettings["SenderEmail"], toEmail, subject, body)
            {
                IsBodyHtml = true
            };

            await client.SendMailAsync(mail);
        }
    }
    
}
