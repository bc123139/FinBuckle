using Finbuckle.MultiTenant;

namespace FinBuckleMvc
{
    public class AppTenantInfo : TenantInfo
    {
        public string OpenIdConnectAuthority { get; set; }
        public string OpenIdConnectClientId { get; set; }
        public string OpenIdConnectClientSecret { get; set; }
    }
}
