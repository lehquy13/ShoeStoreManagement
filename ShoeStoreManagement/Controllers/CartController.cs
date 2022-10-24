using Microsoft.AspNetCore.Mvc;

namespace ShoeStoreManagement.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Orders = true;
            return View();
        }
    }
}
