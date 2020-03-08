using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq;
using System.Reflection;

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
                .UseIdentityColumn();

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
        public static void ApplyConfigurationsDerivedFrom<T>(this ModelBuilder builder)
        {
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
                         .Where(t => t.GetInterfaces().Any(gi => gi.IsGenericType && gi.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
                         && t.GetInterfaces().Contains(typeof(T))).ToList();

            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                builder.ApplyConfiguration(configurationInstance);
            }
        }
        public static void UpdateBaseEntityDateColumns(this DbContext context)
        {
            var entries = context.ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (
                    e.State == EntityState.Added
                    || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseEntity)entityEntry.Entity).ModifiedAt = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).AddedAt = DateTime.Now;
                }
            }
        }
    }
}