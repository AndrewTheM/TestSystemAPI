using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestSystem.API.Configuration;
using TestSystem.API.Extensions;

namespace TestSystem.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            string configName = (Environment.IsDevelopment())
                                ? ConfigurationNames.LocalDbConnectionString
                                : ConfigurationNames.RemoteDbConnectionString;

            string connectionString = Configuration.GetConnectionString(configName);

            services.AddCors()
                    .AddLogging()
                    .AddAutoMapper(typeof(Startup))
                    .AddSqlServerContext(connectionString)
                    .AddIdentityAuth(Configuration)
                    .AddCoreServices()
                    .AddUnitOfWork();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseStatusCodePages();
            app.UseHttpsRedirection();
            app.UseRouting();

            // TODO: configure CORS properly
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            });

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseRequestLogging();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
