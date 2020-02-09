using Core.Entities.LodgingEntities;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Lodgings.Configurations
{
    public class LodgingTypeConfiguration : ILodgingConfigurationAggregate, IEntityTypeConfiguration<LodgingType>
    {
        public void Configure(EntityTypeBuilder<LodgingType> builder)
        {
            builder.ConfigureBaseEntityColumns();

            builder.EnableSoftDeletion();

            builder.Property(p => p.Name)
                .IsRequired()
                .HasConversion<string>();

            builder.HasIndex(i => i.Name)
                .IsUnique()
                .IsSoftDeleteable()
                .HasName("LodgingType_Name_UQ");
        }
    }
}
