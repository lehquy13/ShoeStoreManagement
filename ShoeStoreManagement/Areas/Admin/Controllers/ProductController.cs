using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoeStoreManagement.Controllers;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.Data;
using System.Data;
using System.Linq;

namespace ShoeStoreManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly ApplicationDbContext _applicationDBContext;
        private List<ProductCategory>? productCategories;
        private List<SizeDetail>? sizeDetails;
        private List<Product>? products;
        public ProductController(ILogger<ProductController> logger, ApplicationDbContext applicationDBContext)
        {
            _logger = logger;
            _applicationDBContext = applicationDBContext;
            Init();
        }

        private void Init()
        {
            productCategories = _applicationDBContext.ProductCategories.ToList<ProductCategory>();
            sizeDetails = _applicationDBContext.SizeDetails.ToList<SizeDetail>();
            products = _applicationDBContext.Products.ToList<Product>();
            for (int i = 0; i < products.Count; i++)
            {
                products[i].SetCategory(productCategories);
                List<SizeDetail> sizeList = _applicationDBContext.SizeDetails.Where(products[i].ProductId) as List<SizeDetail>;
                foreach (var obj in sizeList)
                {
                    products[i].Sizes.Add(obj.);

                }
            }
        }

        public IActionResult Index()
        {
            ViewBag.Product = true;
            ViewData["productCategories"] = productCategories;
            ViewData["products"] = products;
            return View();
        }

        public IActionResult Create()
        {
            //ViewBag.Product = true;
            return View();
        }
    }
}
