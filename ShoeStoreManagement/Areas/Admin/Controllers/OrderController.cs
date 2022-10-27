using Microsoft.AspNetCore.Mvc;

namespace ShoeStoreManagement.Areas.Admin.Controllers
{
	public class OrderController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
