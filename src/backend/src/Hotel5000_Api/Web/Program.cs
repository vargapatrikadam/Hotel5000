using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces.PasswordHasher;
using Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Web
{
    public class Program
    {
        //public static void Main(string[] args)
        //{
        //    CreateHostBuilder(args).Build().Run();
        //}

        public async static Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using(var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var enviroment = services.GetRequiredService<Microsoft.AspNetCore.Hosting.IHostingEnvironment>();
                var context = services.GetRequiredService<LodgingDBContext>();
                var hasher = services.GetRequiredService<IPasswordHasher>();

                await LodgingDBContextSeed.SeedAsync(context, hasher, enviroment.IsProduction());
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
