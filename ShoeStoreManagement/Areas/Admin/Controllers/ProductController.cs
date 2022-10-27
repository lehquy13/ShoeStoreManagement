using Microsoft.AspNetCore.Mvc;

namespace ShoeStoreManagement.Areas.Admin.Controllers
{
	public class ProductController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
