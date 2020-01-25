using Core.Entities.Logging;
using Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Infrastructure.Data.Contexts
{
    public class LoggingDBContext : DbContext
    {
        public LoggingDBContext(DbContextOptions<LoggingDBContext> options) : base(options) { }
        public DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ILoggingConfigurationAggregate).Assembly); 
        }
    }
}
