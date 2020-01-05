using Core.Entities.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations.Logging
{
    public class LogConfiguration : IEntityTypeConfiguration<LogEntity>
    {
        public void Configure(EntityTypeBuilder<LogEntity> builder)
        {
            builder.HasKey(k => k.Id);

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Timestamp)
                .IsRequired();

            builder.Property(p => p.Message)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(p => p.Type)
                .IsRequired()
                .HasConversion<string>();
        }
    }
}
