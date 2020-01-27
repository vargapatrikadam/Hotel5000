using Core.Entities.LodgingEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Configurations.LodgingConfigurations
{
    public class PaymentTypeConfiguration : IEntityTypeConfiguration<PaymentType>, ILodgingConfigurationAggregate
    {
        public void Configure(EntityTypeBuilder<PaymentType> builder)
        {
            builder.ConfigureBaseEntityColumns();

            builder.EnableSoftDeletion();

            builder.Property(p => p.Name)
                .IsRequired()
                .HasConversion<string>();

            builder.HasIndex(i => i.Name)
                .IsUnique()
                .IsSoftDeleteable()
                .HasName("PaymentType_Name_UQ");
        }
    }
}
