﻿using Core.Entities.LodgingEntities;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Lodgings.Configurations
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
                .HasConstraintName("Lodging_User_FK")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasOne(p => p.LodgingType)
                .WithMany(p => p.Lodgings)
                .HasConstraintName("Lodging_LodgingType_FK")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}