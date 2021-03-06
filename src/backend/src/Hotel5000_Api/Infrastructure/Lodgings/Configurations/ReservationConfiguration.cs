﻿using Core.Entities.LodgingEntities;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Lodgings.Configurations
{
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>,
        ILodgingConfigurationAggregate
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.ConfigureBaseEntityColumns();

            builder.Property(p => p.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.EnableSoftDeletion();

            builder.HasOne(p => p.PaymentType)
                .WithMany(p => p.UserReservations)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("UserReservation_PaymentType_FK")
                .IsRequired();
        }
    }
}