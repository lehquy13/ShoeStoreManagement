using Microsoft.AspNetCore.Mvc;

namespace ShoeStoreManagement.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Confirm()
        {
            return View();
        }
    }
}
