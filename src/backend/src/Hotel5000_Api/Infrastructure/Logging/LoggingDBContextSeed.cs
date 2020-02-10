using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Logging
{
    public class LoggingDBContextSeed
    {
        public static async Task Seed(LoggingDbContext context, bool isProduction)
        {
            //if(isProduction)
            //    context.Database.Migrate();
            context.Database.Migrate();
        }
    }
}
