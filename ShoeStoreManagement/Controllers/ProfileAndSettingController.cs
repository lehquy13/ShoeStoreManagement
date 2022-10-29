using Microsoft.AspNetCore.Mvc;

namespace ShoeStoreManagement.Controllers
{
    public class ProfileAndSettingController : Controller
    {
        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult Setting()
        {
            return View();
        }
    }
}
