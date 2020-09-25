using Core.Entities.Authentication;
using Infrastructure.Auth.Configurations;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;

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
    }
}
