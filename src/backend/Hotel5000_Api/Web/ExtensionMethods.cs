using Core.Entities.Example;
using Core.Entities.Logging;
using Core.Interfaces;
using Infrastructure.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// This methods adds repositories to the service container. 
        /// We can't use open generics, because of the inambiguity of the generics behind the implementation, so we have to add them one-by-one by hand.
        /// </summary>
        /// <param name="services"></param>
        public static void AddScopedRepositories(this IServiceCollection services)
        {
            //We cannot use this way because of the repository pattern.
            //services.AddScoped(typeof(IAsyncRepository<>), typeof(LoggingDBRepository<>));
            //services.AddScoped(typeof(IAsyncRepository<>), typeof(ExampleDBRepository<>));
            services.AddScoped<IAsyncRepository<Log>, LoggingDBRepository<Log>>();
            services.AddScoped<IAsyncRepository<ExampleEntity>, ExampleDBRepository<ExampleEntity>>();
        }
    }
}
