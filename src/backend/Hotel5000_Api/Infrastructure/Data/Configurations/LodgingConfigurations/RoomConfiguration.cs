﻿using Core.Entities.LodgingEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations.LodgingConfigurations
{
    public class RoomConfiguration : IEntityTypeConfiguration<Room>, ILodgingConfigurationAggregate
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.ConfigureBaseEntityColumns();

            builder.Property(p => p.AdultCapacity)
                .IsRequired();

            builder.Property(p => p.ChildrenCapacity)
                .IsRequired();

            builder.EnableSoftDeletion();

            builder.HasOne(p => p.Lodging)
                .WithMany(p => p.Rooms)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Room_Lodging_FK");
        }
    }
}
