using Core.Entities.Example;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations.Example
{
    public class ExampleConfiguration : IEntityTypeConfiguration<ExampleEntity>, IExampleConfigurationAggregate
    {
        public void Configure(EntityTypeBuilder<ExampleEntity> builder)
        {
            builder.HasKey(k => k.Id);

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Sum)
                .IsRequired();

            builder.Property(p => p.Added)
                .IsRequired();

            builder.Property<bool>("IsDeleted");

            builder.HasQueryFilter(f => EF.Property<bool>(f, "IsDeleted") == false);

            builder.HasIndex(i => i.Sum)
                .IsUnique()
                .HasFilter("IsDeleted = 0");
        }
    }
}
