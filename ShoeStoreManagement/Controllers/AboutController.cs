using Microsoft.AspNetCore.Mvc;

namespace ShoeStoreManagement.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.About = true;
            return View();
        }
    }
}
