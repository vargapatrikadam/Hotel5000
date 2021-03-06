using Core.Interfaces.PasswordHasher;
using Infrastructure.Lodgings;
using Infrastructure.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Web
{
    public class Program
    {
        //public static void Main(string[] args)
        //{
        //    CreateHostBuilder(args).Build().Run();
        //}

        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var environment = services.GetRequiredService<IWebHostEnvironment>();
                var lodgingContext = services.GetRequiredService<LodgingDbContext>();
                var hasher = services.GetRequiredService<IPasswordHasher>();

                //TODO: log exceptions?
                await LodgingDbContextSeed.SeedAsync(lodgingContext, hasher, true);

                var loggingContext = services.GetRequiredService<LoggingDbContext>();
                LoggingDBContextSeed.Seed(loggingContext, true);
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}