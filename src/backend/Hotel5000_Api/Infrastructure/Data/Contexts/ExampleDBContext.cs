using Core.Entities.Example;
using Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
