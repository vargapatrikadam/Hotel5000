using Auth.Identity;
using AutoMapper;
using Core.Interfaces.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using System.Security.Claims;
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

            //if (!_env.IsEnvironment("TESTING"))
            //    services.RegisterDbContexts(Configuration);

            services.AddHttpContextAccessor();

            services.AddTransient<IUserIdentity>(provider =>
                {
                    HttpContext httpContext = provider.GetService<IHttpContextAccessor>().HttpContext;

                    ClaimsIdentity claimsIdentity = httpContext.User.Identity as ClaimsIdentity;

                    Claim role = claimsIdentity.FindFirst(ClaimTypes.Role);
                    Claim username = claimsIdentity.FindFirst(ClaimTypes.Name);
                    Claim email = claimsIdentity.FindFirst(ClaimTypes.Email);

                    IUserIdentity userIdentity = new UserIdentity(role != null ? role.Value : string.Empty,
                                                                  username != null ? username.Value : string.Empty,
                                                                  email != null ? email.Value : string.Empty);

                    return userIdentity;
                }
            );

            services.RegisterInMemoryDbContexts();

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
    }
}