using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoeStoreManagement.Areas.Identity.Data;
using ShoeStoreManagement.Controllers;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.CRUD.Implementations;
using ShoeStoreManagement.CRUD.Interfaces;
using System.Drawing;
using System.Security.Claims;

namespace ShoeStoreManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private const int minusIndex = 35;

        private readonly ILogger<ProductController> _logger;
        private readonly IProductCRUD _productCRUD;
        private readonly ICartCRUD _cartCRUD;
        private readonly ICartDetailCRUD _cartDetailCRUD;
        private readonly ISizeDetailCRUD _sizeDetailCRUD;
        private readonly IProductCategoryCRUD _productCategoryCRUD;
        private List<ProductCategory>? productCategories;
        private List<SizeDetail>? sizes;
        private IList<SelectListItem>? test;

        private List<Product>? products;
        private readonly UserManager<ApplicationUser> _usermanager;
        private ApplicationUser _currentUser;
        private Cart _userCart;

        public ProductController(ILogger<ProductController> logger, IProductCRUD productCRUD, UserManager<ApplicationUser> usermanager
            , IProductCategoryCRUD productCategoryCRUD, ISizeDetailCRUD sizeDetailCRUD, ICartCRUD cartCRUD, ICartDetailCRUD cartDetailCRUD)
        {
            _logger = logger;
            _productCRUD = productCRUD;
            _productCategoryCRUD = productCategoryCRUD;
            _sizeDetailCRUD = sizeDetailCRUD;
            _cartCRUD = cartCRUD;
            _cartDetailCRUD = cartDetailCRUD;

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

        public IActionResult ToCart(Product product)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Cart? cart = _cartCRUD.GetAsync(userId).Result;

            if (cart == null)
            {
                cart = new Cart();
                cart.UserId = userId;
                _cartCRUD.CreateAsync(cart);
            }

            if (product == null)
            {
                return NotFound();
            }

            List<SizeDetail> sizes = _sizeDetailCRUD.GetAllByIdAsync(product.ProductId).Result;

            foreach (SizeDetail i in sizes)
            {
                var a = product.CheckedSize;
                var b = product.CheckedSizeAmount;
                // Handle Change Size in Size Detail
                if (product.CheckedSize[i.Size - minusIndex]) //Size 35 in Index 0 and so on.
                {
                    if (product.CheckedSizeAmount[i.Size - minusIndex].HasValue)
                    {
                        i.Amount -= product.CheckedSizeAmount[i.Size - minusIndex].Value;
                        _sizeDetailCRUD.Update(i);

                        CartDetail? cartDetail = null;

                        //Get Cart Detail with suitable size and id
                        foreach (CartDetail cd in _cartDetailCRUD.GetAllAsync(cart.CartId).Result)
                        {
                            if (cd.ProductId.Equals(product.ProductId) && cd.Size == i.Size)
                                cartDetail = cd;
                        }

                        if (cartDetail != null)
                        {
                            if (cartDetail.Amount < product.Amount)
                            {
                                cartDetail.Amount += product.CheckedSizeAmount[i.Size - minusIndex].Value;
                                cartDetail.CartDetailTotalSum = cartDetail.Amount * product.ProductUnitPrice;
                                _cartDetailCRUD.Update(cartDetail);
                            }
                        }
                        else
                        {
                            cartDetail = new CartDetail()
                            {
                                CartId = cart.CartId,
                                ProductId = product.ProductId,
                                Amount = product.CheckedSizeAmount[i.Size - minusIndex].Value,
                                Size = i.Size,
                                CartDetailTotalSum = cartDetail.Amount * product.ProductUnitPrice,
                            };

                            _cartDetailCRUD.CreateAsync(cartDetail);
                        }
                    }
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult ChooseSize(string id) //This is Product's ID 
        {
            Product? product = _productCRUD.GetByIdAsync(id).Result;

            if (product == null)
            {
                return NotFound();
            }

            List<SizeDetail> sizes = _sizeDetailCRUD.GetAllByIdAsync(id).Result;

            ViewData["chosenProductSizes"] = sizes;

            return PartialView(product);
        }

        //[HttpPost]
        public IActionResult Index(string categoryRadio, string priceRadio, int page = 1)
        {
            List<Product> productFilter = new List<Product>();
            List<string> filters = new List<string>();
            filters.Add(categoryRadio);
            filters.Add(priceRadio);

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
            ViewData["filters"] = filters;
            ViewData["page"] = page;
            ViewData["test"] = test;
            return View(new Product());
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

        [HttpGet]
        public async Task<IActionResult> Edit1(string? id)
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
                if (obj.ProductCategoryId != null)
                {
                    obj.ProductCategory = _productCategoryCRUD.GetByIdAsync(obj.ProductCategoryId).Result;//note
                }
                else
                {
                    return NotFound(obj.ProductCategoryId);

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

        // Admin/Edit/id
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var obj = await _productCRUD.GetByIdAsync(id);
            if(obj != null)
            {
                obj.ProductCategory = _productCategoryCRUD.GetByIdAsync(obj.ProductCategoryId).Result;
                ViewData["productCategories"] = productCategories;
                return PartialView(obj);

            }
            return RedirectToAction("Index");
        }
    }
}
