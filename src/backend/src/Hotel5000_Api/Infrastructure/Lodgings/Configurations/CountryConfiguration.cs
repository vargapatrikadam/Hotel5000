using Core.Entities.LodgingEntities;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Lodgings.Configurations
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>, ILodgingConfigurationAggregate
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.ConfigureBaseEntityColumns();

            builder.Property(p => p.Code)
                .HasMaxLength(2)
                .IsRequired();

            builder.Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.EnableSoftDeletion();

            builder.HasIndex(i => i.Code)
                .IsUnique()
                .HasName("Country_CountyCode_UQ")
                .IsSoftDeleteable();

            builder.HasIndex(i => i.Name)
                .IsUnique()
                .HasName("Country_CountryName_UQ")
                .IsSoftDeleteable();
        }
    }
}