using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using TestSystem.API.Configuration;
using TestSystem.API.Core.Entities;
using TestSystem.API.Helpers;
using TestSystem.API.Repositories;
using TestSystem.API.Repositories.Interfaces;
using TestSystem.API.Core.Services;
using TestSystem.API.Core.Services.Interfaces;

namespace TestSystem.API.Extensions
{
    internal static class AppServiceCollectionExtensions
    {
        public static IServiceCollection AddSqlServerContext(this IServiceCollection services, string connectionString)
            => services.AddDbContext<ApplicationContext>(opt => opt.UseSqlServer(connectionString));

        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IRepository<Test>, Repository<Test>>()
                    .AddScoped<IRepository<Question>, Repository<Question>>()
                    .AddScoped<IRepository<Answer>, Repository<Answer>>()
                    .AddScoped<IRepository<Attempt>, Repository<Attempt>>()
                    .AddScoped<UserManager<User>>()
                    .AddScoped<RoleManager<IdentityRole>>()
                    .AddScoped<SignInManager<User>>()
                    .AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static IServiceCollection AddIdentityAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<User, IdentityRole>(config =>
            {
                config.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
            })
            .AddEntityFrameworkStores<ApplicationContext>()
            .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.AllowedForNewUsers = false;

                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            var key = configuration.GetValue<string>(ConfigurationNames.TokenGenerationSecret);
            var keyBytes = Encoding.ASCII.GetBytes(key);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddAuthorization();

            var adminDefaultsSection = configuration.GetSection(ConfigurationNames.DefaultAdminSeedingData);
            services.Configure<DefaultAdminOptions>(adminDefaultsSection);

            services.AddScoped<IUserClaimsPrincipalFactory<User>, NamedUserClaimsPrincipalFactory>();
            return services;
        }

        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            services.AddScoped<ITestService, TestService>()
                    .AddScoped<IIdentityService, IdentityService>()
                    .AddScoped<ICompletionService, CompletionService>();

            return services;
        }
    }
}
