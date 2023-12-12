using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.AspNetCore;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace FinBuckleMvc.Services
{
    public class HomeService : IHomeService
    {
        private readonly HttpClient _apiClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HomeService(HttpClient apiClient, IHttpContextAccessor httpContextAccessor)
        {
            _apiClient = apiClient;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<string> GetResult()
        {
            ITenantInfo tenantInfo = _httpContextAccessor.HttpContext?.GetMultiTenantContext<AppTenantInfo>()?.TenantInfo;
            try
            {
                var dataString = await _apiClient.GetStringAsync($"https://localhost:5102/{tenantInfo.Identifier}/Home/GetResult");
                return JsonConvert.DeserializeObject<string>(dataString);
            }
            catch (System.Exception ex)
            {

                throw;
            }
            

        }
    }
}
