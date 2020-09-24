using Core.Entities.Authentication;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Auth.Configurations
{
    class BaseRoleConfiguration : IEntityTypeConfiguration<BaseRole>, IAuthConfigurationAggregate
    {
        public void Configure(EntityTypeBuilder<BaseRole> builder)
        {
            builder.ConfigureBaseEntityColumns();

            builder.EnableSoftDeletion();

            builder.Property(p => p.CanEditOthers)
                .IsRequired();

            builder.Property(p => p.Name)
                .IsRequired()
                .HasConversion<string>();

            builder.HasIndex(i => i.Name)
                .IsUnique()
                .IsSoftDeleteable()
                .HasName("BaseRole_Name_UQ");
        }
    }
}
