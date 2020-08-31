using BlazorWebApp.Interfaces;
using BlazorWebApp.Services;
using BlazorWebApp.Services.Domain;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorWebApp.Helpers
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<HttpService>();

            services.AddScoped<ILodgingService, LodgingService>();

            return services;
        }
    }
}
