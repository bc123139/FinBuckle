using FinBuckleMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using FinBuckleMvc.Services;

namespace FinBuckleMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHomeService _homeService;


        public HomeController(ILogger<HomeController> logger
             ,IHttpContextAccessor httpContextAccessor
, IHomeService homeService
            )
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _homeService = homeService;
        }

        public IActionResult Index()
        {
            //IEnumerable<AppTenantInfo> tenants = _tenantStore.GetAllAsync().Result;
            //var tenantInfo = _multiTenantContext?.TenantInfo as AppTenantInfo;
            //return RedirectToAction("Action", "Controller", new { __tenant__ = tenantInfo.Identifier });
            return View();

        }

        [Authorize]
        public async Task<IActionResult> Claims()
        {
            var homeResult = await _homeService.GetResult();
            ViewBag.Home=homeResult;
            return View();
        }

        public IActionResult Privacy()
        {
            
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> SignOutUser()
        {
            if (User != null)
                await HttpContext.SignOutAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
