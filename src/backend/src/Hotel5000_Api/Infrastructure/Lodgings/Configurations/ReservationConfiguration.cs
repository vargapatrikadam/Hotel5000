using Core.Entities.LodgingEntities;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

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
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("UserReservation_PaymentType_FK")
                .IsRequired();
        }
    }
}