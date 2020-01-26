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
            builder.HasKey(k => k.Id)
                .HasName("PaymentType_PK");

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Name)
                .IsRequired();

            //builder.HasData(
            //    );
        }
    }
}
