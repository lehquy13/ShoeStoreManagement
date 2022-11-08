using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoeStoreManagement.Controllers;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.CRUD.Interfaces;

namespace ShoeStoreManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductCRUD _productCRUD;
        private readonly ISizeDetailCRUD _sizeDetailCRUD;
        private readonly IProductCategoryCRUD _productCategoryCRUD;
        private List<ProductCategory>? productCategories;
        private List<SizeDetail>? sizes;
        private IList<SelectListItem>? test;

        private List<Product>? products;
        public ProductController(ILogger<ProductController> logger, IProductCRUD productCRUD
            , IProductCategoryCRUD productCategoryCRUD, ISizeDetailCRUD sizeDetailCRUD)
        {
            _logger = logger;
            _productCRUD = productCRUD;
            _productCategoryCRUD = productCategoryCRUD;
            _sizeDetailCRUD = sizeDetailCRUD;
            Init();
        }

        private void Init()
        {
            productCategories = _productCategoryCRUD.GetAllAsync().Result;
            products = _productCRUD.GetAllAsync().Result;
            for (int i = 0; i < products.Count; i++)
            {
                products[i].SetCategory(productCategories);
                List<SizeDetail> sizeList = _sizeDetailCRUD.GetAllByIdAsync(products[i].ProductId).Result;
                int totalNumberShoeOfThatSize = 0;
                foreach (var obj in sizeList)
                {
                    products[i].Sizes.Add(obj);
                    totalNumberShoeOfThatSize += obj.Amount;
                }
                products[i].Amount = totalNumberShoeOfThatSize;
            }

        }

        public IActionResult Index()
        {
            ViewBag.Product = true;
            ViewData["productCategories"] = productCategories;
            ViewData["products"] = products;
            ViewData["test"] = test;
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null || id == "")
            {
                return NotFound();
            }
            var obj = await _productCRUD.GetByIdAsync(id);
            ViewData["productCategories"] = productCategories;

        [HttpPost]
        public IActionResult Create(Product obj)
        {
            if (ModelState.IsValid)
            {
                var temp = obj.TestSizeAmount.Where(x => x != "0").ToList();

                for (var i = 0; i < obj.TestSize.Count; i++)
                {
                    _sizeDetailCRUD.CreateAsync(new SizeDetail() { Size = int.Parse(obj.TestSize[i]), Amount = int.Parse(temp[i]), ProductId = obj.ProductId });
                }

                _productCRUD.CreateAsync(obj);
                TempData["success"] = "Category is Created Successfully!!";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Edit()
            return View(obj);
        }

        
        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            if (ModelState.IsValid)
            {
                _productCRUD.Update(obj);
                
  
                return RedirectToAction("Index");
            }
            return View(obj);
        }
    }
}
