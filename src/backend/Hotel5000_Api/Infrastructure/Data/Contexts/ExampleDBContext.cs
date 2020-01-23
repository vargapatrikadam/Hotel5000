using Core.Entities.Example;
using Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Data.Contexts
{
    public class ExampleDBContext : DbContext
    {
        public ExampleDBContext(DbContextOptions<ExampleDBContext> options) : base(options) { }
        public DbSet<ExampleEntity> Examples { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(IExampleConfigurationAggregate).Assembly);
        }

        public override int SaveChanges()
        {
            this.UpdateSoftDeleteStatuses();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            this.UpdateSoftDeleteStatuses();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
