using Microsoft.AspNetCore.Mvc;
using ShoeStoreManagement.Data;
using ShoeStoreManagement.Core.Models;
using System.Data.Entity;

namespace ShoeStoreManagement.Controllers
{
    public class OrderController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public OrderController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Order = true;

            List<Order> orders = await _db.Orders.ToListAsync();

            if(orders == null)
            {
                return NotFound();
            }

            return View(orders);
        }

        public IActionResult MakeAnOrder()
        {
            return View();
        }
    }
}
