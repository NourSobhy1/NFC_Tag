using NFC.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC.Infrastructure.Data.DataSeeding
{
    public class DbIntializer : IDbIntializer
    {
        private readonly AppDbContext _dbContext;

        public DbIntializer(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task IntializerAsync()
        {
            throw new NotImplementedException();
        }
    }
}
