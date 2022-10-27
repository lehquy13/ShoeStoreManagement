using Microsoft.AspNetCore.Mvc;

namespace ShoeStoreManagement.Areas.Admin.Controllers
{
	public class ReportController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
