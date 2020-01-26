﻿using Core.Entities.LodgingEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations.LodgingConfigurations
{
    public class UserReservationConfiguration : IEntityTypeConfiguration<UserReservation>, ILodgingConfigurationAggregate
    {
        public void Configure(EntityTypeBuilder<UserReservation> builder)
        {
            builder.HasKey(k => k.Id)
                .HasName("UserReservation_PK");

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

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
