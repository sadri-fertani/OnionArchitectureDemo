using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CNESST.AppWebDem.Models;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Collections.Generic;
using CNESST.ZU.AppDemo.Models;
using CNESST.ZU.AppDemo.Services.ProductsAPI;
using Microsoft.AspNetCore.Http;
using CNESST.ZU.AppDemo.Services;

namespace CNESST.AppWebDem.Controllers
{
    public class HomeViewmodel
    {
        public string Token { get; set; }
    }


    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly IAuthorityService _authorityService;

        public HomeController(
            IAuthorityService authorityService,
            IProductService productService,
            ILogger<HomeController> logger)
        {
            _logger = logger;
            _authorityService = authorityService;
            _productService = productService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(new HomeViewmodel { Token = await _authorityService.GetTokenAsync() });
        }

        public async Task<IActionResult> Products()
        {
            List<ProductModel> products = await _productService.GetAllAsync("Bearer " + await _authorityService.GetTokenAsync() ?? string.Empty);
            return View(products);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
