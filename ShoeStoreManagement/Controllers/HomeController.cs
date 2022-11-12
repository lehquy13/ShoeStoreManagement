using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.CRUD.Interfaces;
using ShoeStoreManagement.Data;
using ShoeStoreManagement.Models;
using System.Diagnostics;

namespace ShoeStoreManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductCRUD _productCRUD;

        public HomeController(IProductCRUD productCRUD)
        {
            _productCRUD = productCRUD;
        }

        public IActionResult Index()
        {
            ViewBag.Home = true;

            List<Product> list = _productCRUD.GetAllAsync().Result;

            return View(list);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult WishList()
        {
            ViewBag.WishList = true;

            List<Product> list = _productCRUD.GetAllAsync().Result;

            return View(list);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}