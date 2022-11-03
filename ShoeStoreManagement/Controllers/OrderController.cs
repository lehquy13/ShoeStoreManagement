using Microsoft.AspNetCore.Mvc;
using ShoeStoreManagement.Data;
using ShoeStoreManagement.Core.Models;
using System.Data.Entity;

namespace ShoeStoreManagement.Controllers
{
    public class OrderController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        public OrderController(ApplicationDbContext db)
        {
            
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Order = true;

            //List<Order> orders = await _db.Orders.ToListAsync();

            //if(orders == null)
            //{
            //    return NotFound();
            //}

            return View();
        }

        public IActionResult MakeAnOrder()
        {
            return View();
        }
    }
}
