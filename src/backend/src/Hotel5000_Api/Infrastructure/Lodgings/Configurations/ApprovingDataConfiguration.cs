using Core.Entities.Domain;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Lodgings.Configurations
{
    public class ApprovingDataConfiguration : IEntityTypeConfiguration<ApprovingData>, ILodgingConfigurationAggregate
    {
        public void Configure(EntityTypeBuilder<ApprovingData> builder)
        {
            builder.ConfigureBaseEntityColumns();

            builder.Property(p => p.IdentityNumber)
                .HasMaxLength(8);

            builder.Property(p => p.TaxNumber)
                .HasMaxLength(13);

            builder.Property(p => p.RegistrationNumber)
                .HasMaxLength(12);

            builder.EnableSoftDeletion();

            builder.HasIndex(i => i.IdentityNumber)
                .IsUnique()
                .HasName("ApprovingData_IdentityNumber_UQ")
                .IsSoftDeleteable();

            builder.HasIndex(i => i.TaxNumber)
                .IsUnique()
                .HasName("ApprovingData_TaxNumber_UQ")
                .IsSoftDeleteable();

            builder.HasIndex(i => i.RegistrationNumber)
                .IsUnique()
                .HasName("ApprovingData_RegistrationNumber_UQ")
                .IsSoftDeleteable();

            builder.HasOne(p => p.User)
                .WithOne(p => p.ApprovingData)
                .HasForeignKey<ApprovingData>(k => k.UserId)
                .HasConstraintName("ApprovingData_User_FK");
        }
    }
}