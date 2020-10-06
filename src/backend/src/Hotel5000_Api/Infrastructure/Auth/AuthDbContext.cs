using Core.Entities.Authentication;
using Infrastructure.Auth.Configurations;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Auth
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }
        public DbSet<BaseRole> BaseRoles { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<Entity> Entities { get; set; }
        public DbSet<Rule> Rules { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsDerivedFromInterface<IAuthConfigurationAggregate>();
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
