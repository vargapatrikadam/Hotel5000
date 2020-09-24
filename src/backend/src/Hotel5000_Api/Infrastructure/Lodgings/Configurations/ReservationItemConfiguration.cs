using Core.Entities.Domain;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Lodgings.Configurations
{
    public class ReservationItemConfiguration : IEntityTypeConfiguration<ReservationItem>, ILodgingConfigurationAggregate
    {
        public void Configure(EntityTypeBuilder<ReservationItem> builder)
        {
            builder.ConfigureBaseEntityColumns();

            builder.Property(p => p.ReservedFrom)
                .IsRequired();

            builder.Property(p => p.ReservedTo)
                .IsRequired();

            builder.EnableSoftDeletion();

            builder.HasOne(p => p.Room)
                .WithMany(p => p.ReservationItems)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired()
                .HasConstraintName("ReservationItem_Room_FK");

            builder.HasOne(p => p.ReservationWindow)
                .WithMany(p => p.ReservationItems)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired()
                .HasConstraintName("ReservationItem_ReservationWindow_FK");

            builder.HasOne(p => p.Reservation)
                .WithMany(p => p.ReservationItems)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired()
                .HasConstraintName("ReservationItem_Reservation_FK");
        }
    }
}