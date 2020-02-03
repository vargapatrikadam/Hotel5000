﻿using Core.Entities.LoggingEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Helpers;

namespace Infrastructure.Logging
{
    public class LogConfiguration : IEntityTypeConfiguration<Log>, ILoggingConfigurationAggregate
    {
        public void Configure(EntityTypeBuilder<Log> builder)
        {
            builder.HasKey(k => k.Id);

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.ConfigureBaseEntityColumns();

            builder.EnableSoftDeletion();

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