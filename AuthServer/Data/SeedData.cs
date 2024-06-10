using AuthServer.Models;
using Finbuckle.MultiTenant;
using IdentityModel;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.Storage;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;

namespace AuthServer.Data
{
    public class SeedData
    {
        public static void EnsureSeedData(IConfiguration config)
        {
            var services = new ServiceCollection();

            services.AddSingleton(config);

            services.AddLogging();
            var migrationAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("IdentityDb"), sql => sql.MigrationsAssembly(migrationAssembly)));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddConfigurationDbContext<MultiTenantConfigurationDbContext>(options =>
                options.ConfigureDbContext = optionsBuilder =>
                    optionsBuilder.UseSqlServer(config.GetConnectionString("AuthDb"), sql => sql.MigrationsAssembly(migrationAssembly)));

            services.AddMultiTenant<TenantInfo>()
                .WithConfigurationStore();

            using (var serviceProvider = services.BuildServiceProvider())
            {
                foreach (var tenant in serviceProvider.GetRequiredService<IMultiTenantStore<TenantInfo>>()
                    .GetAllAsync().Result)
                {
                    using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                    {
                        var multiTenantContextAccessor = scope.ServiceProvider
                            .GetRequiredService<IMultiTenantContextAccessor<TenantInfo>>();
                        multiTenantContextAccessor.MultiTenantContext = new MultiTenantContext<TenantInfo>
                        {
                            TenantInfo = tenant
                        };

                        Log.Debug($"Tenant: {tenant.Name}, {tenant.Identifier}, {tenant.Id}");

                        var applicationDbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();

                        applicationDbContext.Database.Migrate();

                        var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                        var user1 = userMgr.FindByNameAsync("utahir604@gmail.com").Result;
                        if (user1 == null)
                        {
                            user1 = new ApplicationUser
                            {
                                UserName = "utahir604@gmail.com",
                                Email = "utahir604@gmail.com",
                                EmailConfirmed = true
                            };
                            var result = userMgr.CreateAsync(user1, "usmanPass").Result;
                            if (!result.Succeeded) throw new Exception(result.Errors.First().Description);

                            result = userMgr.AddClaimsAsync(user1,
                                new[]
                                {
                                    new Claim(JwtClaimTypes.Name, "Usman Tahir"),
                                    new Claim(JwtClaimTypes.GivenName, "Usman"),
                                    new Claim(JwtClaimTypes.FamilyName, "Tahir")
                                }).Result;
                            if (!result.Succeeded) throw new Exception(result.Errors.First().Description);

                            Log.Debug("Usman created");
                        }
                        else
                        {
                            Log.Debug("Usman already exists");
                        }

                        var user2 = userMgr.FindByNameAsync("usman33651@gmail.com").Result;
                        if (user2 == null)
                        {
                            user2 = new ApplicationUser
                            {
                                UserName = "usman33651@gmail.com",
                                Email = "usman33651@gmail.com",
                                EmailConfirmed = true
                            };
                            var result = userMgr.CreateAsync(user2, "usmanPass").Result;
                            if (!result.Succeeded) throw new Exception(result.Errors.First().Description);

                            result = userMgr.AddClaimsAsync(user2,
                                new[]
                                {
                                    new Claim(JwtClaimTypes.Name, "Usman Tahir"),
                                    new Claim(JwtClaimTypes.GivenName, "Usman"),
                                    new Claim(JwtClaimTypes.FamilyName, "Tahir"),
                                    new Claim("location", "rawalpindi")
                                }).Result;
                            if (!result.Succeeded) throw new Exception(result.Errors.First().Description);

                            Log.Debug("Usman created");
                        }
                        else
                        {
                            Log.Debug("Usman already exists");
                        }


                        var configurationDbContext = scope.ServiceProvider.GetRequiredService<MultiTenantConfigurationDbContext>();

                        configurationDbContext.Database.Migrate();
                        if (!configurationDbContext.IdentityResources.Any())
                        {   
                         configurationDbContext.IdentityResources.AddRange(new IdentityResources.OpenId().ToEntity(), new IdentityResources.Profile().ToEntity()); 
                         Log.Debug("ApiResources added.");
                        }
                        else
                        {
                            Log.Debug("ApiResources already exist.");
                        }

                        var clientId = $"mvc-{tenant.Identifier}";
                        if (!configurationDbContext.Clients.Where(c => c.ClientId == clientId).Any())
                        {
                            var client = new Client
                            {
                                ClientId = clientId,
                                ClientName = clientId,
                                AllowedGrantTypes = GrantTypes.Code,
                                RedirectUris = { $"{config["FinBuckleMvc"]}/signin-oidc" },
                                AllowedScopes = { "openid", "profile", "finbuckleapi.scope" },
                                ClientSecrets = { new Secret("secret".Sha256()) }
                            };
                            configurationDbContext.Clients.Add(client.ToEntity());
                            configurationDbContext.SaveChanges();
                            Log.Debug("Clients added.");
                        }
                        else
                        {
                            Log.Debug("Clients already exist.");
                        }

                        var apiScopes = new List<ApiScope>
                             {
                                 new ApiScope("finbuckleapi.scope","FinBuckle Api")
                             };
                        if (!configurationDbContext.ApiScopes.Any())
                        {
                            foreach (var apiScope in apiScopes)
                            {
                                configurationDbContext.ApiScopes.Add(apiScope.ToEntity());
                            }
                            configurationDbContext.SaveChanges();
                        }

                        IEnumerable<ApiResource> apiResources= new List<ApiResource>
                        {
                            new ApiResource("finbuckleapi","FinBuckle Api")
                            {
                                Scopes={"finbuckleapi.scope"},
                                //UserClaims =new List<string>{"role"}
                            }
                        };
                        if (!configurationDbContext.ApiResources.Any())
                        {
                            foreach (var apiResource in apiResources)
                            {
                                configurationDbContext.ApiResources.Add(apiResource.ToEntity());
                            }
                            configurationDbContext.SaveChanges();
                        }
                    }
                }
            }
        }
    }
}
