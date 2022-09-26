using Microsoft.AspNetCore.Mvc;

namespace ShoeStoreManagement.Controllers
{
    public class StockController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
