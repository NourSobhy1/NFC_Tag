using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC.Application.Interfaces.IServices
{
   public interface IChatService
    {
        Task<string> AskAsync(string question);
    }
}
