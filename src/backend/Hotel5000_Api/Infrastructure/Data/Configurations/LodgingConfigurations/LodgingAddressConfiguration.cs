using Core.Entities.LodgingEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations.LodgingConfigurations
{
    public class LodgingAddressConfiguration : IEntityTypeConfiguration<LodgingAddress>, ILodgingConfigurationAggregate
    {
        public void Configure(EntityTypeBuilder<LodgingAddress> builder)
        {
            builder.HasKey(k => k.Id)
                .HasName("LodgingAddress_PK");

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.County)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.City)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.PostalCode)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(p => p.Street)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.HouseNumber)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(p => p.Floor)
                .HasMaxLength(10);

            builder.Property(p => p.DoorNumber)
                .HasMaxLength(10);

            builder.EnableSoftDeletion();

            builder.HasOne(p => p.Lodging)
                .WithMany(p => p.LodgingAddresses)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired()
                .HasConstraintName("LodgingAddress_Lodging_FK");

            builder.HasOne(p => p.Country)
                .WithMany(p => p.LodgingAddresses)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired()
                .HasConstraintName("LodgingAddress_Country_FK");

            builder.HasAlternateKey(k => new
            {
                k.CountryId,
                k.County,
                k.City,
                k.PostalCode,
                k.Street,
                k.HouseNumber,
                k.Floor,
                k.DoorNumber
            });
        }
    }
}
