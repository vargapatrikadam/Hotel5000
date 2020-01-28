﻿using Core.Entities.LoggingEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Logging
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