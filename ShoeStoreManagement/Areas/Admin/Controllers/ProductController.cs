using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ShoeStoreManagement.Areas.Admin.Controllers
{
	public class ProductController : Controller
	{
        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
		{
			return View();
		}
	}
}
