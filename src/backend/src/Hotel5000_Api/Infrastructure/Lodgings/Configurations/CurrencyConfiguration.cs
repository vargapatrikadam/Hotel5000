using Core.Entities.LodgingEntities;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Lodgings.Configurations
{
    public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>, ILodgingConfigurationAggregate
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.ConfigureBaseEntityColumns();

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(10);

            builder.EnableSoftDeletion();

            builder.HasIndex(i => i.Name)
                .IsUnique()
                .HasName("Currency_Name_UQ");
        }
    }
}
