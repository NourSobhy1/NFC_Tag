using NFC.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC.Application.Interfaces
{
   public interface INfcTagRepository : IGenericRepository<NfcTag>
    {
        Task<NfcTag?> GetByUIdAsync(string uid);
    }
}
