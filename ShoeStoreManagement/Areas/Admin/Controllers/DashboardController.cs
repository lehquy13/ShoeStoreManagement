using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShoeStoreManagement.Data;
using System.Data;
using System.Linq;

namespace ShoeStoreManagement.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
	{
        private ApplicationDbContext _applicationDbContext;

        DashboardController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
		public IActionResult Index()
		{
            
            ViewBag.Home = true;
            return View();
		}
    }
}
