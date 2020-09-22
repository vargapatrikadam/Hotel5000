using Core.Entities.Authentication;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Auth.Configurations
{
    class RuleConfiguration : IEntityTypeConfiguration<Rule>, IAuthConfigurationAggregate
    {
        public void Configure(EntityTypeBuilder<Rule> builder)
        {
            builder.ConfigureBaseEntityColumns();

            builder.EnableSoftDeletion();

            builder.HasOne(p => p.Entity)
                .WithMany(p => p.Rules)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Rule_Entity_FK");

            builder.HasOne(p => p.Operation)
                .WithMany(p => p.Rules)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Rule_Operation_FK");

            builder.HasOne(p => p.Role)
                .WithMany(p => p.Rules)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Rule_Role_FK");
        }
    }
}
