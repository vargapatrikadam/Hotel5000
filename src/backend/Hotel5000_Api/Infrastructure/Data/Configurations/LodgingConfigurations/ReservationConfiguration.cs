﻿using Core.Entities.LodgingEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations.LodgingConfigurations
{
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>, ILodgingConfigurationAggregate
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.HasKey(k => k.Id)
                .HasName("Reservation_PK");

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.ReservedFrom)
                .IsRequired();

            builder.Property(p => p.ReservedTo)
                .IsRequired();

            builder.EnableSoftDeletion();

            builder.HasOne(p => p.ReservationWindow)
                .WithMany(p => p.Reservations)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired()
                .HasConstraintName("Reservation_ReservationWindow_FK");

            builder.HasOne(p => p.UserReservation)
                .WithMany(p => p.Reservations)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired()
                .HasConstraintName("Reservation_UserReservation_FK");
        }
    }
}
