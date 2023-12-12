using AuthServer.Data;
using AuthServer.Models;
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Strategies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace AuthServer
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        public IWebHostEnvironment Environment { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("IdentityDb")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
                options.EmitStaticAudienceClaim = true;
            })
            .AddConfigurationStore<MultiTenantConfigurationDbContext>(options =>
            {
                options.ConfigureDbContext = optionsBuilder =>
                    optionsBuilder.UseSqlServer(Configuration.GetConnectionString("AuthDb"));
            })
            .AddAspNetIdentity<ApplicationUser>();

            // not recommended for production - you need to store your key material somewhere secure
            builder.AddDeveloperSigningCredential();

            services.AddMultiTenant<TenantInfo>()
                .WithConfigurationStore()
                .WithBasePathStrategy()
                .WithPerTenantAuthentication();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMultiTenant();
            app.Use(async (context, next) =>
            {
                var mtc = context.GetMultiTenantContext<TenantInfo>();
                var tenant = mtc?.TenantInfo;
                if (tenant != null && mtc.StrategyInfo.StrategyType == typeof(BasePathStrategy))
                {
                    context.Request.Path.StartsWithSegments("/" + tenant.Identifier, out var matched, out var newPath);
                    context.Request.PathBase = Path.Join(context.Request.PathBase, matched);
                    context.Request.Path = newPath;
                }

                await next.Invoke();
            });

            app.UseStaticFiles();
            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
