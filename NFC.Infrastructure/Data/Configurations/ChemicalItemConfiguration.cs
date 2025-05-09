using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NFC.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC.Infrastructure.Data.Configurations
{
    public class ChemicalItemConfiguration : IEntityTypeConfiguration<ChemicalItem>
    {
        public void Configure(EntityTypeBuilder<ChemicalItem> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.BoilingPoint)
                .HasPrecision(10, 4);

            builder.Property(c => c.MeltingPoint)
                .HasPrecision(10, 4);

            builder.Property(c => c.Density)
               .HasPrecision(10, 4);

            builder.HasOne(c => c.NfcTag)
                .WithOne(n => n.ChemicalItem)
                .HasForeignKey<NfcTag>(n => n.ChemicalItemId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(c => c.SafetyInstructions)
                .WithOne(s => s.ChemicalItem)
                .HasForeignKey(s => s.ChemicalItemId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
