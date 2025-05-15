using Microsoft.EntityFrameworkCore;
using NFC.Application.Interfaces;
using NFC.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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

        public async Task IntializerAsync()
        {
            //throw new NotImplementedException();
            try
            {
                if (_dbContext.Database.GetPendingMigrations().Any())
                    await _dbContext.Database.MigrateAsync();
                if (!_dbContext.ChemicalItems.Any())
                {
                    var chemicalsData = await File.ReadAllTextAsync(@"..\NFC.Infrastructure\Data\DataSeeding\chemicals.json");
                    var Chemical = JsonSerializer.Deserialize<List<ChemicalItem>>(chemicalsData);
                    if(Chemical is not null && Chemical.Any())
                    {
                        await _dbContext.AddRangeAsync(Chemical);
                        await _dbContext.SaveChangesAsync();
                    }
                }
            }
            catch(Exception ex)
            {

            }
        }
    }
}
