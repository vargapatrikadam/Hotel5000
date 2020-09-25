using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
            foreach (EntityEntry entry in context.ChangeTracker.Entries())
                if (entry.IsSoftDeletable())
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            HandleDeletion(entry, false);
                            break;
                        case EntityState.Deleted:
                            entry.State = EntityState.Modified;
                            HandleDeletion(entry);
                            HandleCascade(entry);
                            break;
                    }
                }
        }
        private static void HandleCascade(EntityEntry entry)
        {
            DbContext context = entry.Context;
            foreach (NavigationEntry navigationEntry in
                entry.Navigations.Where(n => !n.Metadata.IsDependentToPrincipal()))
            {
                navigationEntry.Load();
                if (navigationEntry is CollectionEntry collectionEntry)
                {
                    foreach (var dependentEntry in collectionEntry.CurrentValue)
                    {
                        HandleDeletion(context.Entry(dependentEntry));
                    }
                }
                else
                {
                    var dependentEntry = navigationEntry.EntityEntry;
                    if (dependentEntry != null)
                    {
                        HandleDeletion(context.Entry(dependentEntry));
                    }
                }
            }
        }
        private static void HandleDeletion(EntityEntry entry, bool deleted = true)
        {
            entry.CurrentValues["IsDeleted"] = deleted;
        }
        public static bool IsSoftDeletable(this EntityEntry entry)
        {
            if (entry.Metadata.FindProperty("IsDeleted") == null)
                return false;
            return true;
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
        public static void ApplyConfigurationsDerivedFromInterface<TInterface>(this ModelBuilder builder)
        {
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
                         .Where(t => t.GetInterfaces().Any(gi => gi.IsGenericType && gi.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
                         && t.GetInterfaces().Contains(typeof(TInterface))).ToList();

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