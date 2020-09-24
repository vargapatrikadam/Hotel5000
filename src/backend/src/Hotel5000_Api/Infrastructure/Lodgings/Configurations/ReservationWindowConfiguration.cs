using Core.Entities.Domain;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Lodgings.Configurations
{
    public class ReservationWindowConfiguration : IEntityTypeConfiguration<ReservationWindow>,
        ILodgingConfigurationAggregate
    {
        public void Configure(EntityTypeBuilder<ReservationWindow> builder)
        {
            builder.ConfigureBaseEntityColumns();

            builder.Property(p => p.From)
                .IsRequired();

            builder.Property(p => p.To)
                .IsRequired();

            builder.EnableSoftDeletion();

            builder.HasOne(p => p.Lodging)
                .WithMany(p => p.ReservationWindows)
                .IsRequired()
                .HasConstraintName("ReservationWindow_Lodging_FK");
        }
    }
}