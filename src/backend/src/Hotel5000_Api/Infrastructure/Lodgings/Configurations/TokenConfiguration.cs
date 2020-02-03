using Core.Entities.LodgingEntities;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Lodgings.Configurations
{
    public class TokenConfiguration : IEntityTypeConfiguration<Token>, ILodgingConfigurationAggregate
    {
        public void Configure(EntityTypeBuilder<Token> builder)
        {
            builder.ConfigureBaseEntityColumns();

            builder.Property(p => p.RefreshToken)
                .HasMaxLength(45);

            builder.Property(p => p.ExpiresAt)
                .IsRequired();

            builder.Property(p => p.UsableFrom)
                .IsRequired();

            builder.EnableSoftDeletion();

            builder.HasOne(p => p.User)
                .WithMany(p => p.Tokens)
                .HasConstraintName("Token_User_Id_FK")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}