using Core.Entities.LodgingEntities;
using Core.Enums.Lodging;
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
            builder.Property(p => p.Id)
                .HasConversion<int>();

            builder.Property(p => p.Name)
                .IsRequired();

            builder.HasData(
                Enum.GetValues(typeof(PaymentTypeId))
                    .Cast<PaymentTypeId>()
                    .Select(s => new PaymentType()
                    {
                        Id = s,
                        Name = s.ToString()
                    })
                );
        }
    }
}
