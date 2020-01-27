using Microsoft.EntityFrameworkCore;
using Core.Entities.LodgingEntities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Infrastructure.Helpers;

namespace Infrastructure.Data.Configurations
{
    public class LodgingConfiguration : IEntityTypeConfiguration<Lodging>, ILodgingConfigurationAggregate
    {
        public void Configure(EntityTypeBuilder<Lodging> builder)
        {
            builder.ConfigureBaseEntityColumns();

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.EnableSoftDeletion();

            builder.HasOne(p => p.User)
                .WithMany(p => p.Lodgings)
                .HasConstraintName("Lodgind_User_FK")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}
