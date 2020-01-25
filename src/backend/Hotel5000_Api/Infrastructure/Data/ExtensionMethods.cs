using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Core.Entities;

namespace Infrastructure.Data
{
    public static class ExtensionMethods
    {
        public static void UpdateSoftDeleteStatuses(this DbContext context)
        {
            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry.Entity is BaseEntity)
                {
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
            }
        }
        public static void UpdateBaseEntityDateFields(this DbContext context)
        {
            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry.Entity is BaseEntity)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entry.CurrentValues["AddedAt"] = DateTime.Now;
                            break;
                        case EntityState.Deleted:
                            entry.CurrentValues["ModifiedAt"] = DateTime.Now;
                            break;
                        case EntityState.Modified:
                            entry.CurrentValues["ModifiedAt"] = DateTime.Now;
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// Adds new property & query filter to a configuration which enables soft deletion on it.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="builder"></param>
        public static void EnableSoftDeletion<TEntity>(this EntityTypeBuilder<TEntity> builder)
            where TEntity : class
        {
            builder.Property<bool>("IsDeleted");

            builder.HasQueryFilter(f => EF.Property<bool>(f, "IsDeleted") == false);
        }
        /// <summary>
        /// Enables soft deletion filtering on indexes.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="builder"></param>
        public static void IsSoftDeleteable<TEntity>(this IndexBuilder<TEntity> builder)
        {
            builder.HasFilter("IsDeleted = 0");
        }
    }
}
