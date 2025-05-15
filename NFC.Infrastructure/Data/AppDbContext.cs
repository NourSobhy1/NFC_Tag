using Microsoft.EntityFrameworkCore;
using NFC.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NFC.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
           
    

		}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        public DbSet<ChemicalItem> ChemicalItems { get; set; }
        public DbSet<SafetyInstruction> SafetyInstructions { get; set; }
        public DbSet<NfcTag> NfcTags { get; set; }


    }
}
