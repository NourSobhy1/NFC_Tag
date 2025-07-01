using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC.Application.Interfaces.IServices
{
   public interface IEmailSender
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
