using Core.Entities.LodgingEntities;
using Infrastructure.Lodgings.Configurations;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Lodgings
{
    public class LodgingDbContext : DbContext
    {
        public LodgingDbContext(DbContextOptions<LodgingDbContext> options) : base(options)
        {
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ApprovingData> ApprovingData { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<Lodging> Lodgings { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<LodgingAddress> LodgingAddresses { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<ReservationWindow> ReservationWindows { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<UserReservation> UserReservations { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(ILodgingConfigurationAggregate).Assembly);
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new ApprovingDataConfiguration());
            modelBuilder.ApplyConfiguration(new ContactConfiguration());
            modelBuilder.ApplyConfiguration(new TokenConfiguration());
            modelBuilder.ApplyConfiguration(new LodgingConfiguration());
            modelBuilder.ApplyConfiguration(new CountryConfiguration());
            modelBuilder.ApplyConfiguration(new LodgingAddressConfiguration());
            modelBuilder.ApplyConfiguration(new RoomConfiguration());
            modelBuilder.ApplyConfiguration(new ReservationWindowConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserReservationConfiguration());
            modelBuilder.ApplyConfiguration(new ReservationConfiguration());

        }

        public override int SaveChanges()
        {
            this.UpdateSoftDeleteStatuses();
            //this.DetachAllEntries();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            this.UpdateSoftDeleteStatuses();
            //this.DetachAllEntries();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}