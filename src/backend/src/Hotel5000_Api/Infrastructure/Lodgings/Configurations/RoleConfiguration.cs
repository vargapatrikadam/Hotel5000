using Core.Entities.LodgingEntities;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Lodgings.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>, ILodgingConfigurationAggregate
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ConfigureBaseEntityColumns();

            builder.EnableSoftDeletion();

            builder.Property(p => p.Name)
                .IsRequired()
                .HasConversion<string>();

            builder.HasIndex(i => i.Name)
                .IsUnique()
                .IsSoftDeleteable()
                .HasName("Role_Name_UQ");
        }
    }
}