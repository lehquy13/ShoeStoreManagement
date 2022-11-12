using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.CRUD.Interfaces;
using RouteAttribute = Microsoft.AspNetCore.Components.RouteAttribute;

namespace ShoeStoreManagement.Controllers
{
    public class DetailProductController : Controller
    {
        private readonly IProductCRUD _productCRUD;

        public DetailProductController(IProductCRUD productCRUD)
        {
            _productCRUD = productCRUD;
        }

        [HttpGet]
        public IActionResult Index(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product? product = _productCRUD.GetByIdAsync(id).Result;

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public IActionResult AddToCart()
        {
            return View();
        }
    }
}
