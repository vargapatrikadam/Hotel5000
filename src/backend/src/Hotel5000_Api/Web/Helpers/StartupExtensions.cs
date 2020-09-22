using Auth.Implementations;
using Core.Entities.Authentication;
using Core.Entities.LodgingEntities;
using Core.Entities.LoggingEntities;
using Core.Helpers;
using Core.Interfaces;
using Core.Interfaces.Authentication;
using Core.Interfaces.LodgingDomain;
using Core.Interfaces.LodgingDomain.LodgingManagementService;
using Core.Interfaces.LodgingDomain.UserManagementService;
using Core.Interfaces.Logging;
using Core.Interfaces.PasswordHasher;
using Core.Services.LodgingDomain;
using Core.Services.Logging;
using Core.Services.PasswordHasher;
using Infrastructure.Auth;
using Infrastructure.Lodgings;
using Infrastructure.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Web.Helpers
{
    public static class StartupExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders("Token-Expired"));
            });
        }
        public static void RegisterDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<LodgingDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("LodgingDb")));
            services.AddDbContext<LoggingDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("LoggingDb")), ServiceLifetime.Singleton);
            services.AddDbContext<AuthDbContext>(options => 
                options.UseSqlServer(configuration.GetConnectionString("AuthDb")), ServiceLifetime.Singleton);
        }
        public static void RegisterInMemoryDbContexts(this IServiceCollection services)
        {
            services.AddDbContext<LoggingDbContext>(options =>
                options.UseInMemoryDatabase("LoggingDb"), ServiceLifetime.Singleton);

            services.AddDbContext<AuthDbContext>(options =>
                options.UseInMemoryDatabase("AuthDb"), ServiceLifetime.Singleton);

            services.AddDbContext<LodgingDbContext>(options =>
                options.UseInMemoryDatabase("LodgingDb"));
        }
        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IAsyncRepository<Log>, LoggingDbRepository<Log>>();

            services.AddScoped<IAsyncRepository<Role>, LodgingDbRepository<Role>>();
            services.AddScoped<IAsyncRepository<User>, LodgingDbRepository<User>>();
            services.AddScoped<IAsyncRepository<Token>, LodgingDbRepository<Token>>();
            services.AddScoped<IAsyncRepository<ApprovingData>, LodgingDbRepository<ApprovingData>>();
            services.AddScoped<IAsyncRepository<Contact>, LodgingDbRepository<Contact>>();
            services.AddScoped<IAsyncRepository<LodgingType>, LodgingDbRepository<LodgingType>>();
            services.AddScoped<IAsyncRepository<Lodging>, LodgingDbRepository<Lodging>>();
            services.AddScoped<IAsyncRepository<Country>, LodgingDbRepository<Country>>();
            services.AddScoped<IAsyncRepository<LodgingAddress>, LodgingDbRepository<LodgingAddress>>();
            services.AddScoped<IAsyncRepository<Currency>, LodgingDbRepository<Currency>>();
            services.AddScoped<IAsyncRepository<Room>, LodgingDbRepository<Room>>();
            services.AddScoped<IAsyncRepository<ReservationWindow>, LodgingDbRepository<ReservationWindow>>();
            services.AddScoped<IAsyncRepository<PaymentType>, LodgingDbRepository<PaymentType>>();
            services.AddScoped<IAsyncRepository<Reservation>, LodgingDbRepository<Reservation>>();
            services.AddScoped<IAsyncRepository<ReservationItem>, LodgingDbRepository<ReservationItem>>();

            services.AddScoped<IAsyncRepository<BaseRole>, AuthDbRepository<BaseRole>>();
            services.AddScoped<IAsyncRepository<Operation>, AuthDbRepository<Operation>>();
            services.AddScoped<IAsyncRepository<Entity>, AuthDbRepository<Entity>>();
            services.AddScoped<IAsyncRepository<Rule>, AuthDbRepository<Rule>>();
        }
        public static void SetupSwagger(this IServiceCollection services)
        {
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
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                s.IncludeXmlComments(xmlPath);
            });
        }
        public static void SetupJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var authenticationOptions = configuration.GetSection("AuthenticationOptions").Get<AuthenticationOptions>();

            var key = Encoding.ASCII.GetBytes(authenticationOptions.Secret);
            
            
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                                context.Response.Headers.Add("Token-Expired", "true");
                            context.Response.StatusCode = 401;
                            return Task.CompletedTask;
                        }
                    };
                });
        }
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            var authenticationOptions = configuration.GetSection("AuthenticationOptions").Get<AuthenticationOptions>();
            var hashingOptions = configuration.GetSection("HashingOptions").Get<HashingOptions>();

            services.AddSingleton<ISetting<HashingOptions>>(new Setting<HashingOptions>
               (hashingOptions));
            services.AddSingleton<IPasswordHasher, PasswordHasher>();

            services.AddSingleton<ILoggingService, LoggingService>();

            services.AddScoped<IAuthentication, AuthenticationService>();
            services.AddScoped<IUserManagementService, UserManagementService>();
            services.AddScoped<IContactService, UserManagementService>();
            services.AddScoped<IApprovingDataService, UserManagementService>();
            services.AddScoped<IUserService, UserManagementService>();

            services.AddScoped<ILodgingManagementService, LodgingManagementService>();
            services.AddScoped<ILodgingService, LodgingManagementService>();
            services.AddScoped<ILodgingAddressService, LodgingManagementService>();
            services.AddScoped<IReservationWindowService, LodgingManagementService>();
            services.AddScoped<IRoomService, LodgingManagementService>();
            services.AddScoped<IReservationService, ReservationService>();

            services.AddSingleton<ISetting<AuthenticationOptions>>(new Setting<AuthenticationOptions>
                (authenticationOptions));
        }
    }
}
