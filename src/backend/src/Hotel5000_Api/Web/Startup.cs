using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Web.DTOs;
using Web.Helpers;

namespace Web
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddNewtonsoftJson(x =>
                    x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.ConfigureCors();

            services.AddRouting(options => options.LowercaseUrls = true);

            if (!_env.IsEnvironment("TESTING"))
                services.RegisterDbContexts(Configuration);

            services.RegisterRepositories();

            services.SetupSwagger();

            services.SetupJwtAuthentication(Configuration);

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.RegisterServices(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (_env.IsDevelopment())
                app.UseExceptionHandler("/api/error-local-development");
            else
                app.UseExceptionHandler("/api/error");

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "Hotel5000 Api v1");
                s.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        //This method is only called when the project's enviroment variable 'ASPNETCORE_ENVIRONMENT' is set to 'Development'
        //public void ConfigureDevelopmentServices(IServiceCollection services)
        //{
        //    //If you want to use in-memory database during a development enviroment, use this
        //    services.AddDbContext<LoggingDbContext>(options =>
        //        options.UseInMemoryDatabase("LoggingDatabase"));

        //    services.AddDbContext<LodgingDbContext>(options =>
        //        options.UseInMemoryDatabase("LodgingDatabase"));

        //    ConfigureServices(services);
        //}
    }
}