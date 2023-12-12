using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FinBuckleApi.Controllers
{
    [ApiController]
    [Route("{__tenant__}/[controller]")]
    // [Authorize]
    public class HomeController : ControllerBase
    {

        private readonly ILogger<HomeController> _logger;
        private readonly ITenantInfo _tenantInfo;

        public HomeController(ILogger<HomeController> logger, ITenantInfo tenantInfo)
        {
            _logger = logger;
            _tenantInfo = tenantInfo;
        }

        [HttpGet(nameof(GetResult))]
        public async Task<string> GetResult()
        {
            return $"Api response for {_tenantInfo.Name}";
        }
    }
}
