using Core.Entities.Authentication;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Auth.Configurations
{
    class OperationConfiguration : IEntityTypeConfiguration<Operation>, IAuthConfigurationAggregate
    {
        public void Configure(EntityTypeBuilder<Operation> builder)
        {
            builder.ConfigureBaseEntityColumns();

            builder.EnableSoftDeletion();

            builder.Property(p => p.Action)
                .IsRequired()
                .HasConversion<string>();

            builder.HasIndex(i => i.Action)
                .IsUnique()
                .IsSoftDeleteable();
        }
    }
}
