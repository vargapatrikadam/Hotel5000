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
    public class RoleConfiguration : IEntityTypeConfiguration<Role>, ILodgingConfigurationAggregate
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.Property(p => p.Id)
                .HasConversion<int>();

            builder.Property(p => p.Name)
                .IsRequired();

            builder.HasData(
                Enum.GetValues(typeof(RoleId))
                    .Cast<RoleId>()
                    .Select(s => new Role()
                    {
                        Id = s,
                        Name = s.ToString()
                    })
                );
        }
    }
}
