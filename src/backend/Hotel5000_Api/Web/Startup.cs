using Core.Entities.LodgingEntities;
using Core.Entities.LoggingEntities;
using Core.Helpers;
using Core.Interfaces;
using Core.Interfaces.Lodging;
using Core.Interfaces.Logging;
using Core.Interfaces.PasswordHasher;
using Core.Services.Lodging;
using Core.Services.Logging;
using Core.Services.PasswordHasher;
using Infrastructure.Data;
using Infrastructure.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using System.Threading.Tasks;
using Web.Middlewares;

namespace Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            #region swagger settings
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo() { Title = "Hotel5000 Api", Version = "v0.1" });
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter JWT Bearer authorization token",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "Bearer {token}",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                s.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);
                s.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        securityScheme,
                        Array.Empty<string>()
                    }
                });
            });
            #endregion

            services.AddCors();

            AuthenticationOptions authenticationOptions = Configuration.GetSection("AuthenticationOptions").Get<AuthenticationOptions>();
            #region jwt settings
            var key = Encoding.ASCII.GetBytes(authenticationOptions.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                // restful?
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                    
                };
            });
            #endregion

            services.AddScoped<IAsyncRepository<Log>, LoggingDBRepository<Log>>();

            services.AddScoped<IAsyncRepository<Role>, LodgingDBRepository<Role>>();
            services.AddScoped<IAsyncRepository<User>, LodgingDBRepository<User>>();
            services.AddScoped<IAsyncRepository<Token>, LodgingDBRepository<Token>>();
            services.AddScoped<IAsyncRepository<ApprovingData>, LodgingDBRepository<ApprovingData>>();
            services.AddScoped<IAsyncRepository<Contact>, LodgingDBRepository<Contact>>();
            services.AddScoped<IAsyncRepository<Lodging>, LodgingDBRepository<Lodging>>();
            services.AddScoped<IAsyncRepository<Country>, LodgingDBRepository<Country>>();
            services.AddScoped<IAsyncRepository<LodgingAddress>, LodgingDBRepository<LodgingAddress>>();
            services.AddScoped<IAsyncRepository<Room>, LodgingDBRepository<Room>>();
            services.AddScoped<IAsyncRepository<ReservationWindow>, LodgingDBRepository<ReservationWindow>>();
            services.AddScoped<IAsyncRepository<PaymentType>, LodgingDBRepository<PaymentType>>();
            services.AddScoped<IAsyncRepository<UserReservation>, LodgingDBRepository<UserReservation>>();
            services.AddScoped<IAsyncRepository<Reservation>, LodgingDBRepository<Reservation>>();
            

            services.AddSingleton<IOption<HashingOptions>>(new Option<HashingOptions>
                (Configuration.GetSection("HashingOptions").Get<HashingOptions>()));
            services.AddSingleton<IPasswordHasher, PasswordHasher>();

            services.AddScoped<ILoggingService, LoggingService>();

            services.AddSingleton<IOption<AuthenticationOptions>>(new Option<AuthenticationOptions>
                (authenticationOptions));
            services.AddScoped<IAuthenticationService, AuthenticationService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "Hotel5000 Api v1");
                s.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware(typeof(ExceptionHandlingMiddleware));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        //This method is only called when the project's enviroment variable 'ASPNETCORE_ENVIRONMENT' is set to 'Development'
        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            //If you want to use in-memory database during a development enviroment, use this
            services.AddDbContext<LoggingDBContext>(options =>
                options.UseInMemoryDatabase("LoggingDatabase"));

            services.AddDbContext<LodgingDBContext>(options =>
                options.UseInMemoryDatabase("LodgingDatabase"));

            ConfigureServices(services);
        }
    }
}
