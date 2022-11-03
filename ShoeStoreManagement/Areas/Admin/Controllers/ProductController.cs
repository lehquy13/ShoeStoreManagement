using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.Data;
using System.Data;

namespace ShoeStoreManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
	{
        private readonly ApplicationDbContext _applicationDbContext;
        public ProductController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public IActionResult Index()
		{
			ViewBag.Product = true;
            List<Product> obj = _applicationDbContext.Products.ToList<Product>();
            ViewData["products"] = obj; 
			return View();
		}

        public IActionResult Create()
        {
            //ViewBag.Product = true;
            return View();
        }
    }
}
