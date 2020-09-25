using Core.Entities.Authentication;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Auth.Configurations
{
    class EntityConfiguration : IEntityTypeConfiguration<Entity>, IAuthConfigurationAggregate
    {
        public void Configure(EntityTypeBuilder<Entity> builder)
        {
            builder.ConfigureBaseEntityColumns();

            builder.EnableSoftDeletion();

            builder.Property(p => p.Name)
                .IsRequired();

            builder.HasIndex(i => i.Name)
                .IsUnique()
                .IsSoftDeleteable();
        }
    }
}
