using Core.Interfaces.PasswordHasher;
using Infrastructure.Lodgings;
using Infrastructure.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Web;

namespace Hotel5000_Api_Tests.FunctionalTests.Web.Helpers
{
    public class WebTestFixture : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("TESTING");

            builder.ConfigureServices(services =>
            {
                var provider = services.AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                services.AddDbContext<LoggingDbContext>(options =>
                {
                    options.UseInMemoryDatabase("Test logging db");
                    options.UseInternalServiceProvider(provider);
                });

                services.AddDbContext<LodgingDbContext>(options =>
                {
                    options.UseInMemoryDatabase("Test lodging db");
                    options.UseInternalServiceProvider(provider);
                });

                var serviceProvider = services.BuildServiceProvider();

                using (var scope = serviceProvider.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var domainDb = scopedServices.GetRequiredService<LodgingDbContext>();
                    var hasher = scopedServices.GetRequiredService<IPasswordHasher>();

                    LodgingDbContextSeed.SeedAsync(domainDb, hasher, false).Wait();

                    var loggingDb = scopedServices.GetRequiredService<LoggingDbContext>();
                    LoggingDBContextSeed.Seed(loggingDb, false);
                }
            });
        }
    }
}
