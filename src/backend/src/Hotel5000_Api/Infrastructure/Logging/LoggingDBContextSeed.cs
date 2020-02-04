using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Logging
{
    public class LoggingDBContextSeed
    {
        public static async Task Seed(LoggingDbContext context)
        {
            context.Database.Migrate();
        }
    }
}
