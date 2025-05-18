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
                    var basePath = AppDomain.CurrentDomain.BaseDirectory;
                    var ChemicalsPath = Path.Combine(basePath, "chemicals.json");
                    //var chemicalsData = await File.ReadAllTextAsync(@"..\NFC.Infrastructure\Data\DataSeeding\chemicals.json");
                    var chemicalsData = File.ReadAllText(ChemicalsPath);
                    var Chemical = JsonSerializer.Deserialize<List<ChemicalItem>>(chemicalsData, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    //var Chemical = JsonSerializer.Deserialize<List<ChemicalItem>>(chemicalsData, new JsonSerializerOptions
                    //{
                    //    PropertyNameCaseInsensitive = true
                    //});
                    if(Chemical is not null && Chemical.Any())
                    {
                        await _dbContext.AddRangeAsync(Chemical);
                        await _dbContext.SaveChangesAsync();
                    }
                }

                if (!_dbContext.NfcTags.Any())
                {
                    var tag1 = new NfcTag
                    {
                        Id = Guid.NewGuid(),
                        UId = "UID123456789",
                        ChemicalItemId = null
                    };

                    var tag2 = new NfcTag
                    {
                        Id = Guid.NewGuid(),
                        UId = "UID987654321",
                        ChemicalItemId = Guid.Parse("bce5e54b-d3d7-4c63-10eb-08dd93db614e")
                    };

                    await _dbContext.NfcTags.AddRangeAsync(tag1, tag2);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"seeding error: {ex.Message}");
            }
        }
    }
}
