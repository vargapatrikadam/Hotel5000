using Core.Entities.LoggingEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Helpers;

namespace Infrastructure.Logging
{
    public class LoggingDbContext : DbContext
    {
        public LoggingDbContext(DbContextOptions<LoggingDbContext> options) : base(options)
        {
        }

        public DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsDerivedFrom<ILoggingConfigurationAggregate>();
        }
    }
}