using Core.Entities.LodgingEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Lodgings.Configurations
{
    public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.ConfigureBaseEntityColumns();

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(10);

            builder.EnableSoftDeletion();

            builder.HasIndex(i => i.Name)
                .IsUnique()
                .HasName("Currency_Name_UQ");
        }
    }
}
