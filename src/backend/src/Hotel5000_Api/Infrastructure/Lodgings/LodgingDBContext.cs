using Core.Entities.LodgingEntities;
using Infrastructure.Helpers;
using Infrastructure.Lodgings.Configurations;
using Microsoft.EntityFrameworkCore;
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
        public DbSet<LodgingType> LodgingTypes { get; set; }
        public DbSet<Lodging> Lodgings { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<LodgingAddress> LodgingAddresses { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<ReservationWindow> ReservationWindows { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ReservationItem> ReservationItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsDerivedFromInterface<ILodgingConfigurationAggregate>();
            modelBuilder.ApplyConfigurationsFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());
        }

        public override int SaveChanges()
        {
            this.UpdateSoftDeleteStatuses();
            this.UpdateBaseEntityDateColumns();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            this.UpdateSoftDeleteStatuses();
            this.UpdateBaseEntityDateColumns();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}