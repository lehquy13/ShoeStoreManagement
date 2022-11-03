using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.Data;
using ShoeStoreManagement.Models;
using System.Diagnostics;

namespace ShoeStoreManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task<IActionResult> IndexAsync()
        {
            ViewBag.Home = true;

            List<Product> products = await _db.Products.ToListAsync();


            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> WishList()
        {
            ViewBag.WishList = true;

            List<Product> products = await _db.Products.ToListAsync();


            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}