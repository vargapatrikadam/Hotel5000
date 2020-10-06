using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Logging
{
    public class LoggingDBContextSeed
    {
        public static void Seed(LoggingDbContext context, bool isProduction)
        {
            //if (isProduction)
            //    context.Database.Migrate();
            //context.Database.Migrate();
            if (context.Database.IsSqlServer())
                context.Database.Migrate();
            context.SaveChanges();
        }
    }
}
