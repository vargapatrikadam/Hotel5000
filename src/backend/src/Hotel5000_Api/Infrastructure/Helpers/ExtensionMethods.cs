using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Core.Entities;

namespace Infrastructure.Helpers
{
    public static class ExtensionMethods
    {
        public static void UpdateSoftDeleteStatuses(this DbContext context)
        {
            foreach (var entry in context.ChangeTracker.Entries())
                if (entry.Entity is BaseEntity)
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entry.CurrentValues["IsDeleted"] = false;
                            break;
                        case EntityState.Deleted:
                            entry.State = EntityState.Modified;
                            entry.CurrentValues["IsDeleted"] = true;
                            break;
                    }
        }

        /// <summary>
        /// Adds new property & query filter to a configuration which enables soft deletion on it.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="builder"></param>
        public static EntityTypeBuilder<TEntity> EnableSoftDeletion<TEntity>(this EntityTypeBuilder<TEntity> builder)
            where TEntity : class
        {
            builder.Property<bool>("IsDeleted");

            builder.HasQueryFilter(f => EF.Property<bool>(f, "IsDeleted") == false);

            return builder;
        }

        /// <summary>
        /// Enables soft deletion filtering on indexes.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="builder"></param>
        public static IndexBuilder<TEntity> IsSoftDeleteable<TEntity>(this IndexBuilder<TEntity> builder)
        {
            builder.HasFilter("IsDeleted = 0");

            return builder;
        }

        public static EntityTypeBuilder<TEntity> ConfigureBaseEntityColumns<TEntity>(
            this EntityTypeBuilder<TEntity> builder)
            where TEntity : BaseEntity
        {
            builder.HasKey(k => k.Id)
                .HasName(typeof(TEntity).Name + "_PK");

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.AddedAt)
                .ValueGeneratedOnAdd()
                .HasComputedColumnSql("getdate()");

            builder.Property(p => p.ModifiedAt)
                .ValueGeneratedOnUpdate()
                .HasComputedColumnSql("getdate()");

            return builder;
        }

        public static void DetachAllEntries(this DbContext context)
        {
            var entries = context.ChangeTracker.Entries()
                .Where(e => e.State != EntityState.Detached)
                .ToList();

            foreach (var entry in entries)
                if (entry.Entity != null)
                    entry.State = EntityState.Detached;
        }
    }
}