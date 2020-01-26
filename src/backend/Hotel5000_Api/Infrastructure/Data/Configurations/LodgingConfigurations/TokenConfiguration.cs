﻿using Core.Entities.LodgingEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations.LodgingConfigurations
{
    public class TokenConfiguration : IEntityTypeConfiguration<Token>, ILodgingConfigurationAggregate
    {
        public void Configure(EntityTypeBuilder<Token> builder)
        {
            builder.HasKey(k => k.Id)
                .HasName("Token_PK");

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.RefreshToken)
                .HasMaxLength(45);

            builder.EnableSoftDeletion();

            builder.HasOne(p => p.User)
                .WithMany(p => p.Tokens)
                .HasConstraintName("Token_User_Id_FK")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}
