using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ShoeStoreManagement.Areas.Admin.Controllers
{
	public class ReportController : Controller
	{
        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
		{
            ViewBag.Report = true;

            return View();
		}
	}
}
