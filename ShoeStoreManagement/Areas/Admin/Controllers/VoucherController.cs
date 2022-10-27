using Microsoft.AspNetCore.Mvc;

namespace ShoeStoreManagement.Areas.Admin.Controllers
{
	public class VoucherController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
