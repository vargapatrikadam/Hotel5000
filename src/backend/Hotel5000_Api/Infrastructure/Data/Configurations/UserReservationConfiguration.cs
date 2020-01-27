using Core.Entities.LodgingEntities;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations
{
    public class UserReservationConfiguration : IEntityTypeConfiguration<UserReservation>, ILodgingConfigurationAggregate
    {
        public void Configure(EntityTypeBuilder<UserReservation> builder)
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
