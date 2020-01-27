using Core.Entities.LodgingEntities;
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
            builder.ConfigureBaseEntityColumns();

            builder.EnableSoftDeletion();

            builder.Property(p => p.Name)
                .IsRequired();

            builder.HasIndex(i => i.Name)
                .IsUnique()
                .IsSoftDeleteable()
                .HasName("Role_Name_UQ");
        }
    }
}
