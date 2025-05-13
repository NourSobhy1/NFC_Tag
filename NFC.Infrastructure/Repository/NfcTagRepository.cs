using Microsoft.EntityFrameworkCore;
using NFC.Application.Interfaces;
using NFC.Domain.Models;
using NFC.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC.Infrastructure.Repository
{
   public class NfcTagRepository : GenericRepository<NfcTag> ,INfcTagRepository
    {
        private readonly AppDbContext _context;

        public NfcTagRepository(AppDbContext Context) : base(Context)
        {
           
          _context = Context;
        }

        public async Task<NfcTag?> GetByUIdAsync(string uid)
        {
            return await _context.NfcTags
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.UId == uid);
        }
    }
}
