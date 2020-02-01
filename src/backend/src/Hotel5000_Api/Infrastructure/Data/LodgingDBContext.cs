using Core.Entities.LodgingEntities;
using Infrastructure.Data.Configurations;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class LodgingDBContext : DbContext
    {
        public LodgingDBContext(DbContextOptions<LodgingDBContext> options) : base(options)
        {}
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ApprovingData> ApprovingData { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<Lodging> Lodgings { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<LodgingAddress> LodgingAddresses { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<ReservationWindow> Reser { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<UserReservation> UserReservations { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ILodgingConfigurationAggregate).Assembly);
        }
        public override int SaveChanges()
        {
            this.UpdateSoftDeleteStatuses();
            //this.DetachAllEntries();
            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            this.UpdateSoftDeleteStatuses();
            //this.DetachAllEntries();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
