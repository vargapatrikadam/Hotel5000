﻿using Core.Entities.LodgingEntities;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Lodgings.Configurations
{
    public class ReservationWindowConfiguration : IEntityTypeConfiguration<ReservationWindow>,
        ILodgingConfigurationAggregate
    {
        public void Configure(EntityTypeBuilder<ReservationWindow> builder)
        {
            builder.ConfigureBaseEntityColumns();

            builder.Property(p => p.From)
                .IsRequired();

            builder.Property(p => p.To)
                .IsRequired();

            builder.Property(p => p.Price)
                .IsRequired();

            builder.EnableSoftDeletion();

            builder.HasOne(p => p.Room)
                .WithMany(p => p.ReservationWindows)
                .IsRequired()
                .HasConstraintName("ReservationWindow_Room_FK");
        }
    }
}