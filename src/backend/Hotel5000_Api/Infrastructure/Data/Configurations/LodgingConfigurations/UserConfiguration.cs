using Core.Entities.LodgingEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations.LodgingConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>, ILodgingConfigurationAggregate
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Username)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Password)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(p => p.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(p => p.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(i => i.Username)
                .IsUnique()
                .HasName("User_Username_UQ")
                .IsSoftDeleteable();

            builder.HasIndex(i => i.Email)
                .IsUnique()
                .HasName("User_Email_UQ")
                .IsSoftDeleteable();

            builder.HasOne(p => p.Role)
                .WithMany(p => p.Users)
                .HasConstraintName("User_Role_FK")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.EnableSoftDeletion();
        }
    }
}
