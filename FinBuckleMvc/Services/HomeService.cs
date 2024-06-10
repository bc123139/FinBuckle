using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace FinBuckleMvc.Services
{
    public class HomeService : IHomeService
    {
        private readonly HttpClient _apiClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        public HomeService(HttpClient apiClient, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _apiClient = apiClient;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        public async Task<string> GetResult()
        {
            ITenantInfo tenantInfo = _httpContextAccessor.HttpContext?.GetMultiTenantContext<AppTenantInfo>()?.TenantInfo;
            try
            {
                var dataString = await _apiClient.GetStringAsync($"{_configuration["FinBuckleApi"]}/{tenantInfo.Identifier}/Home/GetResult");
                return dataString;
            }
            catch (System.Exception ex)
            {

                throw;
            }
            

        }
    }
}
