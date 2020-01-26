using Core.Entities.LodgingEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations.LodgingConfigurations
{
    public class ReservationWindowConfiguration : IEntityTypeConfiguration<ReservationWindow>, ILodgingConfigurationAggregate
    {
        public void Configure(EntityTypeBuilder<ReservationWindow> builder)
        {
            builder.HasKey(k => k.Id)
                .HasName("ReservationWindow_PK");

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

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
