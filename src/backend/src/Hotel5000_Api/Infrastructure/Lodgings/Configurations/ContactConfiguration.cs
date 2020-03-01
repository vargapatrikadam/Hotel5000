using Core.Entities.LodgingEntities;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Lodgings.Configurations
{
    public class ContactConfiguration : IEntityTypeConfiguration<Contact>, ILodgingConfigurationAggregate
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.ConfigureBaseEntityColumns();

            builder.Property(p => p.MobileNumber)
                .IsRequired()
                .HasMaxLength(13);

            builder.HasIndex(i => i.MobileNumber)
                .IsUnique()
                .HasName("Contact_MobileNumber_UQ")
                .IsSoftDeleteable();

            builder.HasOne(p => p.User)
                .WithMany(p => p.Contacts)
                .HasConstraintName("Contact_User_FK")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.EnableSoftDeletion();
        }
    }
}