using Core.Entities.LodgingEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations.LodgingConfigurations
{
    public class ContactConfiguration : IEntityTypeConfiguration<Contact>, ILodgingConfigurationAggregate
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.HasKey(k => k.Id)
                .HasName("Contact_PK");

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.MobileNumber)
                .IsRequired()
                .HasMaxLength(13);

            builder.HasIndex(i => i.MobileNumber)
                .IsUnique()
                .HasName("Contact_MobileNumber_UQ")
                .IsSoftDeleteable();

            builder.HasOne(p => p.User)
                .WithMany(p => p.Contacts)
                .HasConstraintName("Contact_User_FK")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.EnableSoftDeletion();
        }
    }
}
