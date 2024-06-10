namespace FinBuckleMvc
{
    //public class CustomTenantStore : IMultiTenantStore<AppTenantInfo>
    //{
    //    private readonly IDictionary<string, AppTenantInfo> _tenants;

    //    public CustomTenantStore()
    //    {
    //        // Initialize your store with tenant information (replace this with your data source)
    //        _tenants = new Dictionary<string, AppTenantInfo>
    //    {
    //        { "tenant1", new AppTenantInfo { Id = "unique-id-0ff4adaf", Identifier = "tenant1", Name = "Tenant 1", OpenIdConnectAuthority="https://localhost:5001/tenant-1",
    //            OpenIdConnectClientId= "mvc-tenant1",OpenIdConnectClientSecret= "secret"} },
    //        { "tenant2", new AppTenantInfo { Id = "unique-id-ao41n44", Identifier = "tenant2", Name = "Tenant 2", OpenIdConnectAuthority="https://localhost:5001/tenant-2",
    //            OpenIdConnectClientId= "mvc-tenant2",OpenIdConnectClientSecret= "secret" } }
    //        // Add more tenants as needed
    //    };
    //    }

    //    public Task<IEnumerable<AppTenantInfo>> GetAllAsync()
    //    {
    //       var res =  _tenants.Values.AsEnumerable();
    //        return Task.FromResult(res);
    //    }

    //    public Task<bool> TryAddAsync(AppTenantInfo AppTenantInfo)
    //    {
    //        throw new System.NotImplementedException();
    //    }

    //    public Task<AppTenantInfo> TryGetAsync(string id)
    //    {
    //        throw new System.NotImplementedException();
    //    }

    //    public Task<AppTenantInfo> TryGetByIdentifierAsync(string identifier)
    //    {
    //        // Retrieve tenant by identifier
    //        _tenants.TryGetValue(identifier, out var tenant);
    //        return Task.FromResult(tenant);
    //    }

    //    public Task<bool> TryRemoveAsync(string identifier)
    //    {
    //        throw new System.NotImplementedException();
    //    }

    //    public Task<bool> TryUpdateAsync(AppTenantInfo AppTenantInfo)
    //    {
    //        throw new System.NotImplementedException();
    //    }

    //    // Implement other required methods if necessary based on your needs
    //    // For instance: TryGetAsync, GetAllAsync, etc.
    //}
}
