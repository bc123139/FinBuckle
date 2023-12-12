using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Strategies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.IO;

namespace FinBuckleApi
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
            //services.AddAuthentication("Bearer")
            //    .AddJwtBearer("Bearer", opt =>
            //    {
            //        opt.RequireHttpsMetadata = false;
            //        //opt.Authority = "https://localhost:5001";//Auth Server
            //        opt.Audience = "finbuckleapi";

            //    });
            services.AddControllers();
            services.AddMultiTenant<AppTenantInfo>()
                .WithConfigurationStore()
                .WithRouteStrategy("tenant")
                .WithPerTenantAuthentication();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FinBuckleApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FinBuckle v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseMultiTenant();

            //app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
