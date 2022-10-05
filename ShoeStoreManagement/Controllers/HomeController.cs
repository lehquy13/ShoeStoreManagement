using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeStoreManagement.Models;
using ShoeStoreManagement.Services;
using System.Diagnostics;

namespace ShoeStoreManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GeneralDBContext _generalDBContext;

        public HomeController(ILogger<HomeController> logger, GeneralDBContext generalDBContext)
        {
            _logger = logger;
            _generalDBContext = generalDBContext;
        }

        public async Task<IActionResult> IndexAsync()
        {
            List<Product> products = await _generalDBContext.Products.ToListAsync();
            ViewData["products"] = products;
            return View();
        }



        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Wishes()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}