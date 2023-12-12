using AuthServer.Models;
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Data
{
    public class ApplicationDbContext : MultiTenantIdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(ITenantInfo tenantInfo) : base(tenantInfo)
        {
        }

        public ApplicationDbContext(ITenantInfo tenantInfo, DbContextOptions<ApplicationDbContext> options) :
            base(tenantInfo, options)
        {
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>().IsMultiTenant();
        }
    }
}
