using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoeStoreManagement.Controllers;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.CRUD.Interfaces;
using System.Drawing;

namespace ShoeStoreManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        public string radio1 { set; get; }
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
                    obj.IsChecked = true;
                    //products[i].Sizes.Add(obj);
                    products[i].SizeHashtable.Add(obj.Size, obj.Amount);

                    totalNumberShoeOfThatSize += obj.Amount;
                }
                products[i].Amount = totalNumberShoeOfThatSize;
            }

        }

        //[HttpPost]
        public IActionResult Index(string categoryRadio, string priceRadio)
        {
            List<Product> productFilter = new List<Product>();
            float minvalue = -1, maxvalue = -1;

            if (products.Count > 0)
            {
                if (!string.IsNullOrEmpty(priceRadio))
                {
                    string[] strsplt = priceRadio.Split('-', 2, StringSplitOptions.None);
                    minvalue = float.Parse(strsplt[0]);
                    maxvalue = float.Parse(strsplt[1]);
                }
            }

            foreach (Product i in products)
            {
                if ((i.ProductCategory.ProductCategoryName.Equals(categoryRadio) || string.IsNullOrEmpty(categoryRadio)) && minCheck(minvalue, i.ProductUnitPrice) && maxCheck(maxvalue, i.ProductUnitPrice))
                    productFilter.Add(i);
            }

            ViewBag.Product = true;
            ViewData["productCategories"] = productCategories;
            ViewData["products"] = productFilter;
            ViewData["test"] = test;
            return View();
        }

        private bool minCheck(float value, float price)
        {
            if (value == -1)
                return true;

            if (price >= value)
                return true;
            else 
                return false;
        }

        private bool maxCheck(float value, float price)
        {
            if (value == -1)
                return true;

            if (price < value)
                return true;
            else
                return false;
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
            return View(obj);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(Product obj)
        {
            if (obj != null)
            {
                if (obj.ProductCategoryId == null)
                {
                    return NotFound(obj.ProductCategoryId);
                }
                else
                {
                    obj.ProductCategory = _productCategoryCRUD.GetByIdAsync(obj.ProductCategoryId).Result;//note

                }
                ModelState.Clear();
                if (TryValidateModel(obj))
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

            return NotFound();
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            if (obj.ProductCategoryId == null)
            {
                return NotFound(obj.ProductCategoryId);
            }
            else
            {
                obj.ProductCategory = _productCategoryCRUD.GetByIdAsync(obj.ProductCategoryId).Result;//note

            }
            ModelState.Clear();
            if (TryValidateModel(obj))
            {
                //var temp = obj.TestSizeAmount.Where(x => x != "0").ToList();

                for (var i = 35; i <= 44; i++)
                {
                    var tempDetail = _sizeDetailCRUD.GetProductSizeAsync(obj.ProductId, i).Result;
                    int amount = Int32.Parse(obj.TestSizeAmount[i - 35]);
                    var newDetail = new SizeDetail() { Amount = amount, Size = i, ProductId = obj.ProductId };

                    if (tempDetail != null)
                    {
                        if (amount > 0 && amount != tempDetail.Amount)
                        {
                            _sizeDetailCRUD.Update(newDetail);

                        }
                        else if (amount == 0 || !obj.TestSize.Contains(i.ToString()))
                        {
                            _sizeDetailCRUD.Remove(newDetail);
                        }

                    }
                    else
                    {
                        if (amount > 0)
                            _sizeDetailCRUD.CreateAsync(new SizeDetail()
                            {
                                Size = i,
                                Amount = amount,
                                ProductId = obj.ProductId
                            });
                    }
                }
                _productCRUD.Update(obj);


                return RedirectToAction("Index");
            }
            return View(obj);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var obj = await _productCRUD.GetByIdAsync(id);
            if (obj != null)
            {
                _sizeDetailCRUD.DeleteAllDetailsByIdAsync(obj.ProductId);
                _productCRUD.Remove(obj);
            }
            return RedirectToAction("Index");
        }
    }
}
